package minesweeper4j;

import minesweeper4j.game.MinesweeperResult;

public interface IMinesweeperGame {

	public static enum MinesweeperGameState {
		/**
		 * Minesweeper game is ready to be {@link IMinesweeperGame#run()}.
		 */
		INIT,
		
		/**
		 * Minesweeper game is running.
		 */
		RUNNING,
		
		/**
		 * Minesweeper game has finished (agent won or game timed out).
		 */
		FINISHED,
		
		/**
		 * Minesweeper game has ended with an exception, check {@link IMinesweeperGame#getResult()}
         * and {@link MinesweeperResult#getExecption()} for more info.
		 */
		FAILED, 
		
		/**
		 * Minesweeper game has been terminated via {@link IMinesweeperGame#stop()}.
		 */
		TERMINATED
	}
	
	/**
	 * Starts the game.
	 */
	public void startGame();
	
	/**
	 * Ends the game if {@link MinesweeperGameState#RUNNING}.
	 */
	public void stopGame();
	
	/**
	 * Current state of the game.
	 * @return
	 */
	public MinesweeperGameState getGameState();
		
	/**
	 * Result of the simulation; non-null only iff
     * {@link #getGameState()} == {@link MinesweeperGameState#FINISHED} or
     * {@link MinesweeperGameState#FAILED}. 
	 * @return
	 */
	public MinesweeperResult getResult();
	
	/**
	 * Wait for the game to finish if
     * {@link #getGameState() == {@link MinesweeperGameState#RUNNING};
     * will block the thread that calls this until the game ends.
	 * @return
	 * @throws InterruptedException 
	 */
	public MinesweeperResult waitFinish() throws InterruptedException;
	
}
