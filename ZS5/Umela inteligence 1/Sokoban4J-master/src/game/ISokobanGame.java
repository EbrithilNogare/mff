package game;

public interface ISokobanGame {

	public static enum SokobanGameState {
		/**
		 * Sokoban game is ready to be {@link ISokobanGame#run()}.
		 */
		INIT,
		
		/**
		 * Sokoban game is running.
		 */
		RUNNING,
		
		/**
		 * Sokoban game has finished (agent won or game timed out).
		 */
		FINISHED,
		
		/**
		 * Sokoban game has ended with an agent failure or exception.
		 */
		FAILED, 
		
		/**
		 * Sokoban game has been terminated via {@link ISokobanGame#stop()}.
		 */
		TERMINATED
	}
	
	/**
	 * Starts the game.
	 */
	public void startGame();
	
	/**
	 * Ends the game if {@link SokobanGameState#RUNNING}.
	 */
	public void stopGame();
	
	/**
	 * Current state of the game.
	 * @return
	 */
	public SokobanGameState getGameState();
		
	/**
	 * Result of the simulation; non-null only iff {@link #getGameState()} == {@link SokobanGameState#FINISHED} or {@link SokobanGameState#FAILED}. 
	 * @return
	 */
	public SokobanResult getResult();
	
	/**
	 * Wait for the game to finish if {@link #getGameState() == {@link SokobanGameState#RUNNING}; will block the thread that calls this until the game ends.
	 * @return
	 * @throws InterruptedException 
	 */
	public void waitFinish() throws InterruptedException;
	
}
