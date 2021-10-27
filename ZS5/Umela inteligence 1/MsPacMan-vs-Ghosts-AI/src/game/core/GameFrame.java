package game.core;

import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.Toolkit;

import javax.swing.JComponent;
import javax.swing.JFrame;

public class GameFrame extends JFrame
{
    static final long serialVersionUID = 0;
    
    public GameFrame(JComponent comp)
    {
        super("Ms. Pac-Man vs. Ghosts");
        getContentPane().add(BorderLayout.CENTER,comp);
        pack();
        Dimension screen=Toolkit.getDefaultToolkit().getScreenSize();
        this.setLocation((int)(screen.getWidth()*3/8),(int)(screen.getHeight()*3/8));
        this.setResizable(false);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        repaint();            
    }
    
    public void center() {
    	Dimension screen=Toolkit.getDefaultToolkit().getScreenSize();
    	this.setLocation((int)(screen.getWidth()- getWidth()) / 2,(int)(screen.getHeight() - getHeight()) / 2);
    }
}
