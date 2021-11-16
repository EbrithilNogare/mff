package minimax.tictactoe;

import java.util.ArrayList;

import minimax.*;

public class TicTacToeMain {
    public static void main(String[] args) {
        var strategies = new ArrayList<NamedStrategy<TicTacToe, Integer>>();
        strategies.add(new NamedStrategy<>("basic", () -> new BasicStrategy()));
        
        new GameMain<TicTacToe, Integer>().main("tictactoe", new TicTacToeGame(),
           new TicTacToeUI(), strategies, args);
    }
}
