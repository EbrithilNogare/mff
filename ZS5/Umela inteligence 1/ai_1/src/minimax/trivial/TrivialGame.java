package minimax.trivial;

import java.util.List;

import minimax.*;

class PerfectStrategy implements Strategy<TrivialState, Integer> {
    @Override
    public Integer action(TrivialState state) {
        return 3;
    }
}

// A trivial game. There are two moves. First player 1 chooses a number from 1 to 3,
// then player 2 chooses a number from 1 to 3. Whoever chose the higher number wins.
public class TrivialGame implements HeuristicGame<TrivialState, Integer> {

    @Override
    public TrivialState initialState(int seed) { return new TrivialState(0, 0); }

    @Override
    public TrivialState clone(TrivialState state) {
        return new TrivialState(state.p1move, state.p2move);
    }

    @Override
    public int player(TrivialState state) {
        return state.p1move == 0 ? 1 : 2;
    }

    @Override
    public List<Integer> actions(TrivialState state) {
        if (state.p1move == 0 || state.p2move == 0)
            return List.of(1, 2, 3);
        return List.of();
    }

    @Override
    public void apply(TrivialState state, Integer action) {
        if (action < 1 || action > 3)
            throw new Error("illegal move");
        if (state.p1move == 0)
            state.p1move = action;
        else if (state.p2move == 0)
            state.p2move = action;
        else throw new Error("game is over");
    }

    @Override
    public boolean isDone(TrivialState state) {
        return state.p1move != 0 && state.p2move != 0;
    }

    @Override
    public double outcome(TrivialState state) {
        if (state.p1move > state.p2move)
            return PLAYER_1_WIN;
        if (state.p1move < state.p2move)
            return PLAYER_2_WIN;
        return DRAW;
    }

    @Override
    public double evaluate(TrivialState state) {
        return DRAW;   // just a guess
    }
}
