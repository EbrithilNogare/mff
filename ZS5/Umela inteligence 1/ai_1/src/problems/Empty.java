package problems;

import java.util.*;

import search.Problem;

// An empty problem.  The start state is the goal.
// There are no actions.

public class Empty implements Problem<Integer, Integer> {
  public Integer initialState() { return 0; }

  public List<Integer> actions(Integer state) { return new ArrayList<Integer>(); }

  public Integer result(Integer state, Integer action) {
    throw new AssertionError("should not be called");
  }

  public boolean isGoal(Integer state) { return state == 0; }

  public double cost(Integer state, Integer action) {
    throw new AssertionError("should not be called");
  }
}
