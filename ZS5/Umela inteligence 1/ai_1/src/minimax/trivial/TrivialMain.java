package minimax.trivial;

import java.util.ArrayList;

import minimax.*;

public class TrivialMain {
    public static void main(String[] args) {
        var strategies = new ArrayList<NamedStrategy<TrivialState, Integer>>();
        strategies.add(new NamedStrategy<>("perfect", () -> new PerfectStrategy()));
        
        new GameMain<TrivialState, Integer>().main("trivial", new TrivialGame(),
           new TrivialUI(), strategies, args);
    }
}
