package minimax.trivial;

import java.io.*;
import java.util.ArrayList;

import minimax.*;

public class TrivialUI implements UI<TrivialState, Integer> {
    ArrayList<Strategy<TrivialState, Integer>> players;

    @Override
    public void init(int seed, ArrayList<Strategy<TrivialState, Integer>> players) {
        this.players = players;
    }

    @Override
    public void run() {
        TrivialGame game = new TrivialGame();
        TrivialState state = game.initialState(0);

        for (int turn = 1 ; turn <= 2 ; ++turn) {
            var strategy = players.get(turn);
            int move;

            if (strategy != null)
                move = strategy.action(state);
            else {
                System.out.print("Your move (1-3)? ");
                BufferedReader in = new BufferedReader(new InputStreamReader(System.in));
                try {
                    move = Integer.parseInt(in.readLine());
                } catch (IOException e) { throw new Error(e); }
            }
            System.out.printf("Player %d chooses %d\n", turn, move);

            game.apply(state, move);
        }

        double outcome = game.outcome(state);

        String s = outcome > 0 ? "Player 1 wins" :
                   outcome < 0 ? "Player 2 wins" :
                   "Draw";
        System.out.println(s);
    }
    
}
