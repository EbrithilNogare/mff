package problems;

/* The classic sliding block puzzle, i.e. the 8-puzzle or 15-puzzle.
 * 
 * Construct the 8-puzzle like this:
 * 
 *    new NPuzzle(3);
 * 
 * The starting position has the tiles in reversed order:
 * 
 *   8 7 6
 *   5 4 3
 *   2 1 _
 *   
 * The goal position is
 *  
 *   _ 1 2
 *   2 3 4
 *   5 6 7
 *   
 * The minimal solution has 28 steps.
 * 
 * The heuristic function below is the sum of the Manhattan distances of tiles
 * from their goal positions.  With this heuristic, A* should find the solution
 * while expanding only a few hundred nodes.
 * 
 * The corresponding 15-puzzle is
 * 
 *   new NPuzzle(4);
 *   
 * This is much harder, and requires pattern databases to solve effectively.
 */

import java.util.*;

import search.HeuristicProblem;

// direction in which the a tile moves
enum Dir { Left, Right, Up, Down }

public class NPuzzle implements HeuristicProblem<PuzzleState, Dir> {
	PuzzleState initial;

	public NPuzzle(PuzzleState initial) { this.initial = initial; }

	public NPuzzle(int i) { this(PuzzleState.reversed(i)); }

	public PuzzleState initialState() {
		return initial;
	}

	public List<Dir> actions(PuzzleState state) {
		return state.possibleDirections();
	}
	
	public PuzzleState result(PuzzleState state, Dir action) {
		return state.slide(action);
	}
	
	public boolean isGoal(PuzzleState state) { return state.isGoal(); }
	
	// taxicab distance between squares i and j
	static int dist(int size, int i, int j) {
		return Math.abs(i / size - j / size) + Math.abs(i % size - j % size);
	}
	
	public double cost(PuzzleState state, Dir action) {
		return 1;
	}

	public double estimate(PuzzleState state) {
		// Compute the sum of the taxicab distances of tiles from their goal positions.
		int sum = 0;
		for (int i = 0 ; i < state.size * state.size ; ++i)
			if (state.squares[i] > 0)
				sum += dist(state.size, state.squares[i], i);
		return sum;
	}
}
