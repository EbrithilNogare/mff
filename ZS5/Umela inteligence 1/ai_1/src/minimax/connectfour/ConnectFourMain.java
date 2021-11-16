package minimax.connectfour;

import java.util.ArrayList;

import minimax.*;

public class ConnectFourMain {
    public static void main(String[] args) {
        var strategies = new ArrayList<NamedStrategy<ConnectFour, Integer>>();
        strategies.add(new NamedStrategy<>("basic", () -> new BasicStrategy()));
        strategies.add(new NamedStrategy<>("heuristic", () -> new HeuristicStrategy()));
        new GameMain<ConnectFour, Integer>().main("connect_four", new ConnectFourGame(),
            new ConnectFourUI(), strategies, args);
    }
}
