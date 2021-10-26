package minimax;

import java.util.function.Supplier;

public class NamedStrategy<S, A> {
    String name;
    Supplier<Strategy<S, A>> strategy;

    public NamedStrategy(String name, Supplier<Strategy<S, A>> strategy) {
        this.name = name; this.strategy = strategy;
    }
}
