package minimax;

import java.util.*;

public class RandomStrategy<S, A> extends SeededStrategy<S, A> {
    AbstractGame<S, A> game;

    public RandomStrategy(AbstractGame<S, A> game) {
        this.game = game;
    }

    @Override
    public A action(S state) {
        List<A> actions = game.actions(state);
        return actions.get(random.nextInt(actions.size()));
    }
}
