package minimax.connectfour;

import java.awt.*;
import java.awt.event.*;
import java.awt.geom.*;
import java.util.ArrayList;

import javax.swing.*;

import minimax.*;

class View extends JPanel {
    private static final long serialVersionUID = 0;

    static final Color[] colors = { Color.WHITE, Color.YELLOW.darker(), Color.RED.darker() };

    ConnectFour game;
    Area grid;
    int lastMove;

    public View(ConnectFour game) {
        this.game = game;

        setPreferredSize(new Dimension(800, 700));

        grid = new Area(new Rectangle(50, 50, 700, 600));
        for (int x = 0 ; x < game.width() ; ++x)
            for (int y = 0 ; y < game.height() ; ++y) {
                Ellipse2D c = new Ellipse2D.Double(50 + 100 * x + 10, 50 + 100 * y + 10, 80, 80);
                grid.subtract(new Area(c));
            }
    }

    void circle(Graphics2D g, int x, int y, int width, int height, int thickness) {
        Stroke save = g.getStroke();
        g.setStroke(new BasicStroke(thickness));
        g.drawOval(x, y, width, height);
        g.setStroke(save);
    }

    @Override
    protected void paintComponent(Graphics g1) {
        super.paintComponent(g1);
        Graphics2D g = (Graphics2D) g1;

        for (int x = 0; x < game.width(); ++x)
            for (int y = 0; y < game.height(); ++y) {
                g.setColor(colors[game.at(x, y)]);
                g.fillOval(50 + 100 * x + 10, 50 + 100 * y + 10, 80, 80);
            }

        g.setColor(Color.BLUE.darker());
        g.fill(grid);

        int w = game.winner();
        if (w >= 1) {
            g.setColor(Color.GREEN);
            for (int i = 0 ; i < 4 ; ++i) {
                int x = game.win_x + i * game.win_dx;
                int y = game.win_y + i * game.win_dy;
                circle(g, 50 + 100 * x + 10, 50 + 100 * y + 10, 80, 80, 5);
            }
        } else if (game.lastMove >= 0) {
            int x = game.lastMove;
            int y = 0;
            while (game.board[x][y] == 0)
                y += 1;
            g.setColor(Color.CYAN);
            circle(g, 50 + 100 * x + 10, 50 + 100 * y + 10, 80, 80, 3);
        }

    }
}

public class ConnectFourUI extends JFrame
                           implements UI<ConnectFour, Integer>, KeyListener, MouseListener {

    private static final long serialVersionUID = 0;

    ConnectFour game;
    ArrayList<Strategy<ConnectFour, Integer>> players;

    public ConnectFourUI() {
        super("Connect Four");
    }

    @Override
    public void init(int seed, ArrayList<Strategy<ConnectFour, Integer>> players) {
        this.game = new ConnectFour(seed);
        this.players = players;

        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        add(new View(game));
        pack();
        setLocationRelativeTo(null);

        addKeyListener(this);
        addMouseListener(this);
    }

    Strategy<ConnectFour, Integer> currentStrategy() {
        return players.get(game.turn());
    }

    void computerMove() {
        int move = currentStrategy().action(game);
        if (!game.move(move))
            throw new Error("strategy chose illegal move!");
    }

    void move(int x) {
        if (game.winner() >= 0)
                System.exit(0);

        if (currentStrategy() != null)  // computer's turn
            computerMove();
        else {    // human's turn
            if (!game.move(x))
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
            move(-1);
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
        int x = (e.getX() - 50) / 100;
        move(x);
    }

    @Override
    public void mouseReleased(MouseEvent arg0) {
    }

    @Override
    public void run() {
        setVisible(true);
    }
}
