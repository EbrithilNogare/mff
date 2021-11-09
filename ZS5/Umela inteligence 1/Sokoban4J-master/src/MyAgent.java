import java.util.ArrayList;
import java.util.List;

import agents.ArtificialAgent;
import game.actions.EDirection;
import game.actions.compact.*;
import game.board.compact.BoardCompact;
import search.HeuristicProblem;
import search.Solution;


public class MyAgent extends ArtificialAgent {
	@Override
	protected List<EDirection> think(BoardCompact board) {

		SokobanProblem prob = new SokobanProblem(board);
		Solution<BoardCompact, CAction> solution = AStar.search(prob);
		List<EDirection> solutionDirections = new ArrayList<EDirection>();

		for(CAction action : solution.actions)
		solutionDirections.add(action.getDirection());
		
		if(solution == null)
			System.out.println("this shouldnt happened");
		
		return solutionDirections;
	}
}

class SokobanProblem implements HeuristicProblem<BoardCompact, CAction> {
	BoardCompact state;
	boolean[][] dead;
	
	public SokobanProblem(BoardCompact state){
		this.state = state;
		dead = DeadSquareDetector.detect(state);
	}

	public BoardCompact initialState() { 
		return state;
	}
  
	public List<CAction> actions(BoardCompact state) { 
		List<CAction> actions = new ArrayList<CAction>();
		
		for (CPush push : CPush.getActions()) {
			if (
				push.isPossible(state) &&
				!dead[state.playerX + 2*push.getDirection().dX][state.playerY + 2*push.getDirection().dY]
			)
				actions.add(push);
		}
		for (CMove move : CMove.getActions()) {
			if (move.isPossible(state))
				actions.add(move);
		}

		return actions;
	}

	public BoardCompact result(BoardCompact state, CAction action) {
		BoardCompact newBoard = state.clone();
		action.perform(newBoard);
		return newBoard;
	}
  
	public boolean isGoal(BoardCompact state) { 
		return state.isVictory();
	}

	@Override
	public double cost(BoardCompact state, CAction action) {
		return 1;
	}

	@Override
	public double estimate(BoardCompact state) {
		return state.boxCount - state.boxInPlaceCount;
	}
  }
