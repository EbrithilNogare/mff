package minimax.connectfour;

import java.awt.Point;
import java.util.Random;

public class ConnectFour {
    public static final int Width = 7, Height = 6;

    int[][] board = new int[7][];     // board[x][y] is disc at (x, y)
    int turn = 1;
    int lastMove;
    int winner = -1;
    int win_x, win_y, win_dx, win_dy;

    ConnectFour() {  }

    public ConnectFour(int seed) {
        this();
        Random random = seed >= 0 ? new Random(seed) : new Random();
        
        do {
            for (int x = 0 ; x < Width ; ++x)
                board[x] = new int[Height];

            // Make several random moves so that games will be varied even if
            // strategies are deterministic.
            for (int i = 0 ; i < 4 ; ++i)
                move(random.nextInt(7));
        } while (isPlayer1Win());

        lastMove = -1;
    }

    public int width() { return Width; }

    public int height() { return Height; }

    public int at(int x, int y) { return board[x][y]; }

    public boolean valid(int x, int y) {
        return 0 <= x && x < Width && 0 <= y && y < Height;
    }

    @Override
    public ConnectFour clone() {
        ConnectFour t = new ConnectFour();
        for (int x = 0 ; x < Width ; ++x)
            t.board[x] = board[x].clone();
        t.turn = turn;
        t.winner = winner;
        t.lastMove = lastMove;
        return t;
    }

    public int turn() { return turn; }

    final int[] dir_x = { -1, -1, -1, 0 }, dir_y = { -1, 0, 1, 1 };

    boolean at(int x, int y, int player) {
        return 0 <= x && x < Width && 0 <= y && y < Height && board[x][y] == player;
    }

    final int[][] patterns = {
        { 0, 1, 0, 1, 0 },
        { 0, 0, 1, 1, 0 },
        { 0, 1, 1, 0, 0 }
    };

    boolean match(int x, int[] pattern) {
        for (int i = 0 ; i < 5 ; ++i) {
            if (board[x + i][Height - 1] != pattern[i])
                return false;
        }

        return true;
    }

    boolean isPlayer1Win() {
        for (int x = 0 ; x <= Width - 5 ; ++x)
            for (int[] pattern : patterns)
                if (match(x, pattern))
                    return true;

        return false;
    }

    public boolean winningMove(int player, int x, int y, int dir, Point start) {
        int dx = dir_x[dir], dy = dir_y[dir];
        int count = 0;

        int start_x = x, start_y = y;
        while (true) {
            int sx = start_x - dx, sy = start_y - dy;
            if (!at(sx, sy, player))
                break;
            start_x = sx;
            start_y = sy;
            count += 1;
        }

        int end_x = x, end_y = y;
        while (true) {
            int ex = end_x + dx, ey = end_y + dy;
            if (!at(ex, ey, player))
                break;
            end_x = ex;
            end_y = ey;
            count += 1;
        }

        if (count >= 3) {
            if (start != null) {
                start.x = start_x;
                start.y = start_y;
            }
            return true;
        } else return false;
    }

    public boolean winningMove(int player, int x, int y) {
        for (int dir = 0 ; dir < dir_x.length ; ++dir)
            if (winningMove(player, x, y, dir, null))
                return true;

        return false;
    }

    void checkWin(int player, int x, int y) {
        Point start = new Point();

        for (int dir = 0 ; dir < 4 ; ++dir)
            if (winningMove(player, x, y, dir, start)) {
                win_x = start.x;
                win_y = start.y;
                win_dx = dir_x[dir];
                win_dy = dir_y[dir];
                winner = player;
                return;
            }

        for (x = 0 ; x < Width ; ++x)
            if (board[x][0] == 0)
                return;

        winner = 0;
    }

    public int move_y(int x) {
        int y = Height - 1;
        while (y >= 0 && board[x][y] > 0)
            y -= 1;

        return y;
    }

    public boolean move(int x) {
        if (x < 0 || x >= Width)
            return false;
        
        int y = move_y(x);
        if (y < 0)
            return false;

        board[x][y] = turn;
        lastMove = x;
        checkWin(turn, x, y);

        turn = 3 - turn;
        return true;
    }

    public int winner() { return winner; }
}
