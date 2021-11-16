package minimax;

import java.util.ArrayList;

public interface UI<S, A> {
    void init(int seed, ArrayList<Strategy<S, A>> players);
    void run();
}
