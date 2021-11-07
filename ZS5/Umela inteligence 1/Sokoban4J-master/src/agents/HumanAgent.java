package agents;

import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;

import game.IAgent;
import game.actions.EDirection;
import game.board.compact.BoardCompact;
import ui.Command;

public class HumanAgent implements IAgent, KeyListener {
    Command command = Command.None;
	EDirection direction = EDirection.NONE;
    
    @Override
    public void init(boolean optimal, boolean verbose) { }

	@Override
	public void newLevel() {
	}
	
	@Override
	public void observe(BoardCompact board) {		
	}

	@Override
	public EDirection act() {
		return direction == null ? EDirection.NONE : direction;
	}

	@Override
	public void victory() {
	}
    
    public Command getCommand() {
        Command c = command;
        command = Command.None;
        return c;
    }

	@Override
	public void stop() {
	}
	
	@Override
	public void keyTyped(KeyEvent e) {
        if (e.getKeyChar() == 'z')
            command = Command.Undo;
	}

	@Override
	public void keyPressed(KeyEvent e) {
		int key = e.getKeyCode();
		if      (key == KeyEvent.VK_UP || key == KeyEvent.VK_W) direction = EDirection.UP;
        else if (key == KeyEvent.VK_RIGHT || key == KeyEvent.VK_D) direction = EDirection.RIGHT;
        else if (key == KeyEvent.VK_DOWN || key == KeyEvent.VK_S) direction = EDirection.DOWN;
        else if (key == KeyEvent.VK_LEFT || key == KeyEvent.VK_A) direction = EDirection.LEFT;
	}

	@Override
	public void keyReleased(KeyEvent e) {
		int key = e.getKeyCode();
		if      (key == KeyEvent.VK_UP || key == KeyEvent.VK_W) {
			if (direction == EDirection.UP) direction = EDirection.NONE;
		}
        else if (key == KeyEvent.VK_RIGHT || key == KeyEvent.VK_D) {
        	if (direction == EDirection.RIGHT) direction = EDirection.NONE;
        }
        else if (key == KeyEvent.VK_DOWN || key == KeyEvent.VK_S) {
        	if (direction == EDirection.DOWN) direction = EDirection.NONE;
        }
        else if (key == KeyEvent.VK_LEFT || key == KeyEvent.VK_A) {
        	if (direction == EDirection.LEFT) direction = EDirection.NONE;
        }
		
	}
	
}
