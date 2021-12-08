import java.util.ArrayList;
import java.util.Arrays;
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
		distanceToGoal = new int[state.width()][state.height()];

		List<MyTile> visited = new ArrayList<MyTile>();
		List<MyTile> queue = new ArrayList<MyTile>();

		for (int x = 0; x < state.width(); x++)
			for (int y = 0; y < state.height(); y++)
				if(CTile.forSomeBox(state.tile(x, y))){
					queue.add(new MyTile(x, y, 0));
				}


		while (queue.size() != 0) {
			MyTile tile = queue.get(0);
			queue.remove(0);
			if(visited.contains(tile))
				continue;
			visited.add(tile);
			distanceToGoal[tile.x][tile.y] = tile.distance;

			for (EDirection dir : EDirection.arrows()) {
				int newX = tile.x + dir.dX;
				int newY = tile.y + dir.dY;
				MyTile newTile = new MyTile(newX, newY, tile.distance + 1);
				if(!visited.contains(newTile) && !queue.contains(newTile) && !CTile.isWall(state.tile(newTile.x, newTile.y)))
					queue.add(newTile);
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
		int distanceSum = 0;
		for (int x = 1; x < state.width()-1; x++)
			for (int y = 1; y < state.height()-1; y++)
				if(CTile.isSomeBox(state.tile(x, y)))
					distanceSum += distanceToGoal[x][y];

		return distanceSum;
	}
}

class MyTile{
	int x;
	int y;
	int distance;
	public MyTile(int x, int y){
		this(x,y,0);
	}
	public MyTile(int x, int y, int distance){
		this.x = x;
		this.y = y;
		this.distance = distance;
	}
	@Override
	public boolean equals(Object o){
		if (o == this)
            return true;

        if (!(o instanceof MyTile))
            return false;

        MyTile c = (MyTile) o;
        return x == c.x && y == c.y;
	}
}
