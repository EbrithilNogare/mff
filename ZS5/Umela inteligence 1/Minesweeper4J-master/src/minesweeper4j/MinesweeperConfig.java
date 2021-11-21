package minesweeper4j;

import java.util.Random;

import minesweeper4j.game.IAgent;

public class MinesweeperConfig {
	
	/**
	 * Can be used to mark unique name of the simulation.
	 */
	public String id = "Minesweeper";
	
	/**
	 * Board width.
	 */
	public int width;
	
	/**
	 * Board height.
	 */
	public int height;
	
	/**
	 * Random seed that has been used to initialize {@link #random}.
	 */
	public int randomSeed = -1;
	
	/**
	 * Random used for generating mines and suggesting a tile.
	 */
	public Random random = new Random();
	
	/**
	 * How many mines to generate.
	 */
	public int totalMines;
    

    // Density for calculating the number of mines if not specified.
    public double density = 0.2;

	/**
	 * Timeout for the game; positive number == timeout in effect; otherwise no timeout.
	 */
	public long timeoutMillis = 0;
	
	/**
	 * TRUE == start Minesweeper visualized; FALSE == start Minesweeper headless.
	 */
	public boolean visualization = false;
	
	/**
	 * Instance of the agent that should play the game.
	 */
    public IAgent agent;
    
    public void setSeed(int seed) {
        randomSeed = seed;
        random = new Random(seed);
    }

    public void setSize(int width, int height, int totalMines) {
        this.width = width;
        this.height = height;
        this.totalMines = totalMines;
    }

    void calcMines() {
        this.totalMines = (int) Math.round(density * width * height);
    }

    public void setSize(int width, int height) {
        this.width = width;
        this.height = height;
        calcMines();
    }

    public void setDensity(double density) {
        if (density < 0 || density > 1.0)
            throw new Error("density must be between 0.0 and 1.0");

        this.density = density;
        calcMines();
    }
		
	/**
	 * Validates the configuration; throws {@link RuntimeException} if config is found invalid. 
	 */
	public void validate() {
		if (id == null) throw new RuntimeException("ID is null.");
		if (id.length() == 0) throw new RuntimeException("ID is of zero length.");
		if (agent == null) throw new RuntimeException("Agent is null.");
		if (width <= 0) throw new RuntimeException("width == " + width + " <= 0");
		if (height <= 0) throw new RuntimeException("height == " + height + " <= 0");
		if (totalMines < 0) throw new RuntimeException("totalMines == " + totalMines + " < 0");		
	}

}
