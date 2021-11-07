package game;

public enum SokobanResultType {
	
	/**
	 * Simulation ended with agent winning the game.
	 */
	VICTORY(0),
	
	/**
	 * Simulation ended with timeout; agent failed to finish the game.
	 */
	TIMEOUT(1),
	
	/**
	 * Simulation ended with an agent exception; agent failed. 
	 */
	AGENT_EXCEPTION(2),
	
	/**
	 * Simulation ended with simulation exception; simulation failed.
	 */
	SIMULATION_EXCEPTION(3),
	
	/**
	 * Simulation has been terminated from the outside.
	 */
    TERMINATED(4),
    
    /* The agent failed to return a valid solution or legal move. */
    AGENT_FAILED(5),
    
    // An optimal solution was required, and the agent's solution was not optimal.
    NOT_OPTIMAL(6);
	
	private int exitValue;

	private SokobanResultType(int exitValue) {
		this.exitValue = exitValue;
	}

	public int getExitValue() {
		return exitValue;
	}
	
	public static SokobanResultType getForExitValue(int exitValue) {
		for (SokobanResultType value : SokobanResultType.values()) {
			if (value.exitValue == exitValue) return value;
		}
		return null;
	}

}
