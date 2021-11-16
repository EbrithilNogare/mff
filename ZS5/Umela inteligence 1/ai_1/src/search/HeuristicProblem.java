package search;

// S = state type, A = action type
public interface HeuristicProblem<S, A> extends Problem<S, A> {
    double estimate(S state);  // optimistic estimate of cost from state to goal
}
