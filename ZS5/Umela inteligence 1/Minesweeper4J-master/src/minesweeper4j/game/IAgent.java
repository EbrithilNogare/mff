package minesweeper4j.game;

import java.awt.event.KeyEvent;

public interface IAgent {

	/**
	 * Agent got into a new level.
	 */
	public void newBoard();
	
	/**
	 * An agent receives current state of the board.
	 * @param board
	 */
	public void observe(Board board);
	
	/**
	 * An agent is queried what to do next. 
	 * @return
	 */
	public Action act();
		
	/**
	 * Agent managed to finish the board.
	 */
	public void victory();
	
	/**
	 * Agent has died by "clicking" onto the bomb.
	 */
	public void died();
	
	/**
	 * Terminate the agent as the game has finished (or has been terminated).
	 */
	public void stop();
	
	// =================
	// DEBUGGING SUPPORT
	// =================
	
	/**
	 * A tile at coordinates [tileX,tileY] has been clicked; the most probably from the visualization.
	 * @param tileX
	 * @param tileY
	 * @param rightBtn false == left button, right == right button
	 */
	public void tileClicked(int tileX, int tileY, boolean rightBtn);
	
	/**
	 * Some key has been pressed, see {@link KeyEvent#getKeyCode()}.
	 * @param keyCode
	 */
	public void keyPressed(KeyEvent event);

	
}
