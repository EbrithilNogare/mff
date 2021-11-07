package game;

import game.actions.EDirection;
import game.board.compact.BoardCompact;

public interface IAgent {
    public void init(boolean optimal, boolean verbose);

	/**
	 * Agent got into a new level.
	 */
	public void newLevel();
	
	/**
	 * An agent receives current state of the board.
	 * @param board
	 */
	public void observe(BoardCompact board);
	
	/**
	 * An agent is queried where to move next. 
	 * @return
	 */
	public EDirection act();
	
	/**
	 * Agent managed to finish the level.
	 */
	public void victory();
	
	/**
	 * Terminate the agent as the game has finished.
	 */
	public void stop();
	
}
