package minimax;

import java.util.Random;

public abstract class SeededStrategy<S, A> implements Strategy<S, A> {
    protected Random random = new Random(0);

    public void setSeed(int seed) {
        if (seed >= 0)
            // With Java's random number generator, the first random values produced by
            // similar seeds are often similar, which can bias the results of small games
            // such as Tic-Tac-Toe.  To avoid that effect, we multiply the seed by a
            // prime number.
            random = new Random(seed * 1_000_003);
        else
            random = new Random();
    }
}
