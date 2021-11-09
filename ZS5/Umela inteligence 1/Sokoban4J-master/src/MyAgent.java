import java.util.ArrayList;
import java.util.List;

import agents.ArtificialAgent;
import game.actions.EDirection;
import game.actions.compact.*;
import game.board.compact.BoardCompact;
import game.board.compact.CTile;
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
	int[][] distanceToGoal;
	
	public SokobanProblem(BoardCompact state){
		this.state = state;
		this.dead = DeadSquareDetector.detect(state);
		PrecomputeDistancesToGoal();
	}

	private void PrecomputeDistancesToGoal() {
		for (int x = 0; x < state.width(); x++) {
			for (int y = 0; y < state.height(); y++) {
				


				List<MyTile> visited[] = new List<MyTile>;

				LinkedList<Integer> queue = new LinkedList();
			
				visited[s] = true;
				queue.add(s);
			
				while (queue.size() != 0) {
				  s = queue.poll();
				  System.out.print(s + " ");
			
				  Iterator<Integer> i = adj[s].listIterator();
				  while (i.hasNext()) {
					int n = i.next();
					if (!visited[n]) {
					  visited[n] = true;
					  queue.add(n);
					}
				  }
				}




			}
		}
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

class MyTile{
	int x;
	int y;
	public MyTile(int x, int y){
		this.x = x;
		this.y = y;
	}
	@Override
	public boolean equals(Object o){
		if (o == this) {
            return true;
        }

        if (!(o instanceof MyTile)) {
            return false;
        }
        
        MyTile c = (MyTile) o;

        return x == c.x && y == c.y;
	}
}
