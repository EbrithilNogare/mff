package minimax.tictactoe;

import java.awt.*;
import java.awt.event.*;
import java.util.ArrayList;

import javax.swing.*;

import minimax.*;

class View extends JPanel {
    private static final long serialVersionUID = 0;

    TicTacToe game;

    public View(TicTacToe game) {
        this.game = game;

        setPreferredSize(new Dimension(400, 400));
    }

    int square(int x) { return 50 + 100 * x; }

    @Override
    protected void paintComponent(Graphics g1) {
        super.paintComponent(g1);

        Graphics2D g = (Graphics2D) g1;
        g.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
        g.setRenderingHint(RenderingHints.KEY_RENDERING, RenderingHints.VALUE_RENDER_QUALITY);

        if (game.winner() > 0) {
            g.setColor(Color.GREEN);

            for (int i = 0 ; i < 3 ; ++i)
                g.fillRect(square(game.win_x + i * game.win_dx),
                           square(game.win_y + i * game.win_dy),
                           100, 100);

            g.setColor(Color.BLACK);
        }

        for (int i = 1 ; i < 3 ; ++i) {
            int s = square(i);
            g.drawLine(s, 50, s, 350);
            g.drawLine(50, s, 350, s);
        }

        g.setStroke(new BasicStroke(2));

        for (int x = 0 ; x < 3 ; ++x)
            for (int y = 0 ; y < 3 ; ++y) {
                int p = game.board[x][y];
                int sx = square(x), sy = square(y);
                if (p == 1) {
                    g.drawLine(sx + 10, sy + 10, sx + 90, sy + 90);
                    g.drawLine(sx + 10, sy + 90, sx + 90, sy + 10);
                } else if (p == 2)
                    g.drawOval(sx + 10, sy + 10, 80, 80); 
            }
    }
}

public class TicTacToeUI extends JFrame
                         implements UI<TicTacToe, Integer>, KeyListener, MouseListener {

    private static final long serialVersionUID = 0;

    TicTacToe game;
    ArrayList<Strategy<TicTacToe, Integer>> players;

    public TicTacToeUI() {
        super("Tic Tac Toe");
    }

    @Override
    public void init(int seed, ArrayList<Strategy<TicTacToe, Integer>> players) {
        this.game = new TicTacToe();
        this.players = players;

        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        View view = new View(game);
        add(view);
        pack();
        setLocationRelativeTo(null);

        addKeyListener(this);
        view.addMouseListener(this);
    }

    Strategy<TicTacToe, Integer> currentStrategy() {
        return players.get(game.turn);
    }

    void computerMove() {
        int move = currentStrategy().action(game);
        game.move(move);
    }

    void moveAt(int x, int y) {
        if (game.winner() >= 0)
                System.exit(0);

        if (currentStrategy() != null)  // computer's turn
            computerMove();
        else {    // human's turn
            if (!game.move(x, y))
                return;

            if (game.winner() >= 0) {
                repaint();
                return;     // human won
            }

            if (currentStrategy() != null)  // computer's turn
                computerMove();
        }

        repaint();
    }

    @Override
    public void keyPressed(KeyEvent arg0) {
    }

    @Override
    public void keyReleased(KeyEvent arg0) {
    }

    @Override
    public void keyTyped(KeyEvent e) {
        if (e.getKeyChar() == ' ')
            moveAt(-1, -1);
    }

    @Override
    public void mouseClicked(MouseEvent e) {
    }

    @Override
    public void mouseEntered(MouseEvent arg0) {
    }

    @Override
    public void mouseExited(MouseEvent arg0) {
    }

    @Override
    public void mousePressed(MouseEvent e) {
        int x = (e.getX() - 50) / 100, y = (e.getY() - 50) / 100;
        moveAt(x, y);
    }

    @Override
    public void mouseReleased(MouseEvent arg0) {
    }

    @Override
    public void run() {
        setVisible(true);
    }
}
