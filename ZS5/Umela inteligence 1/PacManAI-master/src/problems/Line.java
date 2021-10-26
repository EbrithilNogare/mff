package problems;

import java.util.*;

import search.Problem;

// A simple puzzle involving movement on a line.
// You start at square 0.  In each move you may move right either
// - by 1 square (cost = 8)
// - by 2 squares (cost = 3)
// - by 3 squares (cost = 5)
//
// The goal is to get to square 101 with minimal total cost.
// The minimal solution has cost = 152 (e.g. move 49 times by 2, then once by 3.)

public class Line implements Problem<Integer, Integer> {
	public Integer initialState() { return 0; }
	
	public List<Integer> actions(Integer state) { return List.of(1, 2, 3); }

	public Integer result(Integer state, Integer action) { return state + action; }
	
	public boolean isGoal(Integer state) { return state == 101; }
	
	static final int cost[] = { 0, 8, 3, 5 };
	
	public double cost(Integer state, Integer action) { return cost[action]; }
	
}
