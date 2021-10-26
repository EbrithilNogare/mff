package minimax.tictactoe;

import java.util.*;

import minimax.HeuristicGame;

public class TicTacToeGame implements HeuristicGame<TicTacToe, Integer> {
    public TicTacToe initialState(int seed) { return new TicTacToe(); }

    public TicTacToe clone(TicTacToe state) { return state.clone(); }

    public int player(TicTacToe state) { return state.turn; }

    public List<Integer> actions(TicTacToe state) { return state.actions(); }

    public void apply(TicTacToe state, Integer action) {
        if (!state.move(action))
            throw new Error("illegal move");
    }

    public boolean isDone(TicTacToe state) { return state.winner() >= 0; }

    public double outcome(TicTacToe state) {
        switch (state.winner()) {
            case 0: return DRAW;   // draw
            case 1: return PLAYER_1_WIN;
            case 2: return PLAYER_2_WIN;
            default: throw new Error();
        }
    }

    public double evaluate(TicTacToe state) {
        return DRAW;   // just a guess
    }
}

/*
class TicTacTest {
    public static void main(String[] args) {
        Runner.play2(new TicTacToe(), new BasicTicTacToeStrategy(), new RandomTicTacToeStrategy(), 1000);
    }
}
*/
