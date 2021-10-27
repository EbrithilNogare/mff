package game;

import game.core.G;

public class GameConfig {
	
	public int seed = -1;
	
	/**
	 * Whether POWER PILLS should be present within the environment.
	 */
	public boolean powerPillsEnabled = true;
	
	/**
	 * Total percentage of PILLS present within the level. If < 1, some (random) pills will be taken away.
	 */
	public double totalPills = 1;
    
    public int startingLevel = 1;

	/**
	 * How many levels Ms PacMan may play (-1 => unbound).
	 */
	public int levelsToPlay = -1;
	
	/**
	 * How many lives to start with.
	 */
	public int lives = G.NUM_LIVES;
	
	public GameConfig clone() {
		GameConfig result = new GameConfig();
		
		result.seed = seed;
		result.powerPillsEnabled = powerPillsEnabled;
        result.totalPills = totalPills;
        result.startingLevel = startingLevel;
		result.levelsToPlay = levelsToPlay;
		
		return result;
	}

	public String asString() {
        return "" + seed + ";" + powerPillsEnabled + ";" + totalPills + ";" +
               startingLevel + ";" + levelsToPlay;
	}
	
	public void fromString(String line) {
		String[] all = line.split(";");
		seed = Integer.parseInt(all[0]);
		powerPillsEnabled = Boolean.parseBoolean(all[1]);
		totalPills = Double.parseDouble(all[2]);
		startingLevel = Integer.parseInt(all[3]);
		levelsToPlay = Integer.parseInt(all[4]);
	}
}
