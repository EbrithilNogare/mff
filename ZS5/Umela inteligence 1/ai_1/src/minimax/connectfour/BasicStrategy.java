package minimax.connectfour;

import java.util.*;

import minimax.SeededStrategy;

// A basic strategy for Connect Four.
//
// If any move will win, play it.  Otherwise, if any move will block an opponent's
// immediate win, play it.  Otherwise, play a random move.

public class BasicStrategy extends SeededStrategy<ConnectFour, Integer> {
    @Override
    public Integer action(ConnectFour game) {
        int me = game.turn();
        ArrayList<Integer> possible = new ArrayList<Integer>();

        int block = -1;
        for (int x = 0 ; x < game.width() ; ++x) {
            int y = game.move_y(x);
            if (y >= 0) {
                if (game.winningMove(me, x, y))
                    return x;

                if (game.winningMove(3 - me, x, y))
                    block = x;
                else
                    possible.add(x);
            }
        }

        return block >= 0 ? block : possible.get(random.nextInt(possible.size()));
    }
}
