package minimax.tictactoe;

import minimax.SeededStrategy;

// A basic strategy for Tic-Tac-Toe.
//
// If any move will win, play it.  Otherwise, if any move will block an opponent's
// immediate win, play it.  Otherwise, play a random move.

public class BasicStrategy extends SeededStrategy<TicTacToe, Integer> {
    public Integer action(TicTacToe s) {
        TicTacToe t = s.clone();

        for (int check = 0 ; check < 2 ; ++check) {
            int player = check == 0 ? t.turn : 3 - t.turn;

            for (int x = 0 ; x < 3 ; ++x)
                for (int y = 0 ; y < 3 ; ++y)
                    if (t.board[x][y] == 0) {
                        t.board[x][y] = player;
                        t.checkWin();
                        if (t.winner() == player)
                            return x + 3 * y;
                        t.board[x][y] = 0;
                    }
        }

        return s.randomAction(random);
    }
}
