package minimax.tictactoe;

import java.util.*;

public class TicTacToe {
    int[][] board = new int[3][];
    int moves = 0;
    public int turn = 1;

    int winner = -1;
    int win_x, win_y, win_dx, win_dy;

    public TicTacToe(int[][] b) {
        for (int i = 0 ; i < 3 ; ++i)
            board[i] = b == null ? new int[3] : b[i].clone();
    }

    public TicTacToe() {
        this(null);
    }

    @Override
    public TicTacToe clone() {
        TicTacToe t = new TicTacToe(board);

        t.turn = turn;
        t.moves = moves;
        t.winner = winner;

        return t;
    }

    List<Integer> actions() {
        List<Integer> r = new ArrayList<Integer>();

        if (winner == -1)
            for (int x = 0 ; x < 3 ; ++x)
                for (int y = 0 ; y < 3 ; ++y)
                    if (board[x][y] == 0)
                        r.add(x + 3 * y);

        return r;
    }

    int randomAction(Random random) {
        List<Integer> a = actions();
        return a.get(random.nextInt(a.size()));
    }

    TicTacToe result(int action) {
        TicTacToe s = clone();
        s.move(action);
        return s;
    }

    boolean win(int x, int y, int dx, int dy) {
        if (board[x][y] > 0 &&
            board[x][y] == board[x + dx][y + dy] &&
            board[x][y] == board[x + 2 * dx][y + 2 * dy]) {

            winner = board[x][y];
            win_x = x; win_y = y;
            win_dx = dx; win_dy = dy;
            return true;
        } else return false;
    }

    void checkWin() {
        for (int i = 0 ; i < 3 ; ++i)
            if (win(i, 0, 0, 1) || win(0, i, 1, 0))
                return; 

        if (win(0, 0, 1, 1) || win(0, 2, 1, -1))
            return;

        if (moves == 9)
            winner = 0;
        }

    boolean move(int x, int y) {
        if (winner >= 0 || x < 0 || x >= 3 || y < 0 || y >= 3 || board[x][y] != 0)
            return false;

        board[x][y] = turn;
        moves += 1;
        checkWin();

        turn = 3 - turn;
        return true;
    }

    boolean move(int action) {
        return move(action % 3, action / 3);
    }

    public int winner() { return winner; }

    char asChar(int i) {
        switch (i) {
        case 0: return '.';
        case 1: return 'X';
        case 2: return 'O';
        default: throw new Error();
        }
    }

    public String toString() {
        StringBuilder sb = new StringBuilder();

        for (int x = 0 ; x < 3 ; ++x) {
            for (int y = 0 ; y < 3 ; ++y)
                sb.append(String.format("%c ", asChar(board[x][y])));
            sb.append("\n");
        }

        return sb.toString();
    }
}
