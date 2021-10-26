package minimax.connectfour;

import java.util.*;

import minimax.SeededStrategy;
 
// A heuristic strategy for Connect Four.
// 
// If any move will win, play it.  Otherwise, if any move will block an opponent's
// immediate win, play it.  Otherwise, choose the move that will yield the greatest
// increase in a heuristic score.  If there is a tie, choose randomly from among
// the possibilities.

public class HeuristicStrategy extends SeededStrategy<ConnectFour, Integer> {
    boolean debug = false;

    // values[n] is the value of having n discs within a span of 4 adjacent
    // positions, if the opponent has no discs in those positions.
    static final double[] values = { 0, 0.2, 1.0, 3.0, 100 };

    static final int[] dir_x = { -1, -1, -1, 0 }, dir_y = { -1, 0, 1, 1 };

    static boolean count(ConnectFour state, int[] p, int x, int y, int dx, int dy) {
        if (!state.valid(x, y) || !state.valid(x + 3 * dx, y + 3 * dy))
            return false;

        p[0] = p[1] = p[2] = 0;
        for (int i = 0 ; i < 4 ; ++i)
            p[state.board[x + i * dx][y + i * dy]] += 1;
        return true;
    }

    static double value(int[] p) {
        if (p[1] > 0 && p[2] == 0)
            return values[p[1]];

        if (p[2] > 0 && p[1] == 0)
            return - values[p[2]];

        return 0;
    }

    // Calculate the total heuristic score for a given board state.
    public static double evaluate(ConnectFour state) {
        double total = 0;
        int[] p = new int[3];

        for (int x = 0; x < state.width(); ++x)
            for (int y = 0 ; y < state.height(); ++y)
                for (int dir = 0 ; dir < 4 ; ++dir)
                    if (count(state, p, x, y, dir_x[dir], dir_y[dir]))
                        total += value(p);

        return total;
    }

    @Override
    public Integer action(ConnectFour game) {
        int me = game.turn();
        ArrayList<Integer> possible = new ArrayList<>();
        int[] p = new int[3];
        double bestVal = Double.NEGATIVE_INFINITY;
        int block = -1;    
        
        for (int x = 0 ; x < game.width() ; ++x) {
            int y = game.move_y(x);
            if (y < 0)
                continue;   // not a possible move

            if (game.winningMove(me, x, y))
                return x;

            if (game.winningMove(3 - me, x, y))
                block = x;

            if (block != -1)
                continue;

            // Calculate the incremental change in the heuristic score if we
            // play at x.
            double total = 0;
            for (int dir = 0 ; dir < 4 ; ++dir) {
                int dx = dir_x[dir], dy = dir_y[dir];
                for (int i = 0 ; i < 4 ; ++i)
                    if (count(game, p, x - i * dx, y - i * dy, dx, dy)) {
                        total -= value(p);
                        p[me] += 1;
                        total += value(p);
                    }
            }
            if (debug) {
                ConnectFour game1 = game.clone();
                game1.move(x);
                double total1 = evaluate(game1) - evaluate(game);
                if (total != total1) {
                    System.out.printf("total = %d, total1 = %d\n", total, total1);
                    throw new Error("incremental calculation is wrong!");
                }
            }

            if (me == 2)
                total = - total;
            if (total > bestVal) {
                bestVal = total;
                possible.clear();
            }
            if (total >= bestVal)
                possible.add(x);
        }

        return block >= 0 ? block : possible.get(random.nextInt(possible.size()));
    }
}
