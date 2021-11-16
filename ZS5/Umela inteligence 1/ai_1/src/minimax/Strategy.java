package minimax;

// S = state type, A = action type
public interface Strategy<S, A> {
  A action(S state);
}
