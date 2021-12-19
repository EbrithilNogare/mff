import minesweeper4j.agents.ArtificialAgent;
import minesweeper4j.game.Action;
import minesweeper4j.game.Board;
import minesweeper4j.game.Tile;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import csp.*;


public class MyAgent extends ArtificialAgent {
	
	/* Code your custom agent here.  The existing dummy implementation always asks for a hint.
     *
     * The Board object passed to thinkImpl gives you the current board state.
     */
	Solver solver = new Solver();
	BooleanCSP csp = null;
	boolean wasAdvice = false;
			
	@Override
	protected Action thinkImpl(Board board, Board previousBoard) {
		if(csp == null)
			createCSP(board);

		reloadCSP(board, previousBoard);
		

		for (Integer val = 0; val < csp.numVars ; val++) {
			if(csp.value[val] == null)
				continue;
			if(csp.value[val] == true){
				Action action = Action.flag(val / board.width, val % board.width);
				if(action.isPossible(board))
					return action;
			} else {
				Action action = Action.open(val / board.width, val % board.width);
				if(action.isPossible(board))
					return action;
			}
		}

		
		//System.out.println(csp.toString());
		//System.out.println(csp.constraints.size());
		//System.out.println();
		
		List<Integer> resultFCH = solver.forwardCheck(csp);
		if(resultFCH.isEmpty()){			
			solver.forwardCheck(csp);

			int resultIV = solver.inferVar(csp);
			if(resultIV != -1)
				resultFCH.add(resultIV);
		}
		
		
		for (Integer val : resultFCH) {
			if(csp.value[val] == null)
				continue;
			if(csp.value[val] == true){
				Action action = Action.flag(val / board.width, val % board.width);
				if(action.isPossible(board))
					return action;
			} else {
				Action action = Action.open(val / board.width, val % board.width);
				if(action.isPossible(board))
					return action;
			}
		}

		
		wasAdvice = true;
		return Action.advice();
	}

	private void reloadCSP(Board board, Board previousBoard) {
		for (int x = 0; x < board.width; x++)
			for (int y = 0; y < board.height; y++)
				if(previousBoard == null || board.tile(x,y).visible != previousBoard.tile(x,y).visible){
					Tile tile = board.tile(x,y);
					if(!tile.visible)
						continue;
					
					csp.set(convert2Dto1D(x, y, board.width), false);

					if(tile.mines == 0)
						continue;

					ArrayList<Integer> vars = new ArrayList<Integer>();
					for (int sx = x-1; sx <= x+1; sx++)
					for (int sy = y-1; sy <= y+1; sy++) {
						if(sx < 0 || sy < 0 || sx >= board.width || sy >= board.height || (sx == x && sy == y) || board.tile(sx,sy).visible)
							continue;
						vars.add(convert2Dto1D(sx, sy, board.width));
					}
					csp.addConstraint(new Constraint(tile.mines, vars));
				}
	}

	Integer convert2Dto1D(int x, int y, int width){
		return x*width+y;
	}

	void createCSP(Board board){
		csp = new BooleanCSP(board.width*board.width);
	}
	
}
