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
	List<Integer> availableMoves = new ArrayList<>();
			
	@Override
	protected Action thinkImpl(Board board, Board previousBoard) {
		if(csp == null)
			createCSP(board);
			
		reloadCSP(board, previousBoard);
		
		while(availableMoves.size() != 0) {
			int val = availableMoves.remove(0);
			int x = val / board.width;
			int y = val % board.width;
			if(csp.value[val] == true && !board.tile(x,y).flag){
				Action action = Action.flag(x, y);
				return action;
			}
			if(csp.value[val] == false && !board.tile(x,y).visible){
				Action action = Action.open(x, y);
				return action;
			}
		}
		
		//System.out.println(csp.toString());
		//System.out.println(csp.constraints.size());
		//System.out.println();
		

		List<Integer> resultFCH = solver.forwardCheck(csp);
		if(resultFCH == null){
			return Action.advice();
		}
		if(resultFCH.isEmpty()){			
			int resultIV = solver.inferVar(csp);
			if(resultIV != -1)
				resultFCH.add(resultIV);
		}
		availableMoves.addAll(resultFCH);
		
		
		while(availableMoves.size() != 0) {
			int val = availableMoves.remove(0);
			int x = val / board.width;
			int y = val % board.width;
			if(csp.value[val] == true && !board.tile(x,y).flag){
				Action action = Action.flag(x, y);
				return action;
			}
			if(csp.value[val] == false && !board.tile(x,y).visible){
				Action action = Action.open(x, y);
				return action;
			}
		}
		
		return Action.advice();
	}

	private void reloadCSP(Board board, Board previousBoard) {
		for (int x = 0; x < board.width; x++)
			for (int y = 0; y < board.height; y++)
				if(board.tile(x,y).visible && (previousBoard == null || !previousBoard.tile(x,y).visible)){
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
		csp = new BooleanCSP(board.width*board.height);
	}
	
}
