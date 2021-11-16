package problems;

import java.util.List;

import search.Problem;

// A simple puzzle involving movement on a grid.
//
// You start at (0, 0).  In each move you may move either
//   - left 1 (cost = 1)
//   - right 1 (cost = 1)
//   - up 1 (cost = 1)
//   - down 1 (cost = 1)
//   - right 1, up 2 (cost = 2)
//   - right 2, up 2 (cost = 5)
//
// The goal is to get to (80, 80) with minimal total cost.
// The cheapest solution has cost = 120.

class GPos {
	int x, y;
	
	GPos(int x, int y) {
		this.x = x; this.y = y;
	}
	
	@Override
	public boolean equals(Object o) {
		if (!(o instanceof GPos))
			return false;
		
	    GPos q = (GPos) o;
	    return x == q.x && y == q.y;
	}
	
	@Override
	public int hashCode() { return x + y; }

	@Override
	public String toString() { return "(" + x + ", " + y + ")"; }
	
	GPos plus(Vec v) {
		return new GPos(x + v.x, y + v.y);
	}
}

class Vec {
	int x, y;
	
	Vec(int x, int y) {
		this.x = x; this.y = y;
	}
}

class Move {
	Vec v;
	int cost;
	
	Move(Vec v, int cost) {
		this.v = v; this.cost = cost;
	}
}

public class Grid implements Problem<GPos, Integer> {
	
	static final Move[] moves = {
		new Move(new Vec(-1, 0), 1),
		new Move(new Vec(1, 0), 1),
		new Move(new Vec(0, -1), 1),
		new Move(new Vec(0, 1), 1),
		new Move(new Vec(1, 2), 2),
		new Move(new Vec(2, 2), 5)
	};

	public GPos initialState() {
		return new GPos(0, 0);
	}
	
	static final List<Integer> allActions = List.of(0, 1, 2, 3, 4, 5);

	public List<Integer> actions(GPos state) { return allActions; }

	public GPos result(GPos state, Integer action) {
		return state.plus(moves[action].v);
	}

	public boolean isGoal(GPos state) {
		return state.x == 80 && state.y == 80;
	}

	public double cost(GPos state, Integer action) {
		return moves[action].cost;
	}

}
