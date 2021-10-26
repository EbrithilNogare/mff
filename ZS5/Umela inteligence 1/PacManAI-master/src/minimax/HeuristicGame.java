package minimax;

// S = state type, A = action type
public interface HeuristicGame<S, A> extends AbstractGame<S, A> {
    double evaluate(S state);  // return estimated outcome in range -1000.0 <= x <= 1000.0
}
