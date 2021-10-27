package game;

import controllers.ghosts.IGhostsController;
import controllers.pacman.IPacManController;

import java.io.File;

public class SimulatorConfig {

	public GameConfig game = new GameConfig();
	
	public boolean visualize = true;
	
	public IPacManController pacManController;
	public IGhostsController ghostsController;
	
	/**
	 * How long can PacMan / Ghost controller think about the game before we compute next frame.
	 * If {@ #visualize} than it also determines the speed of the game.
	 * 
	 * DEFAULT: 25 FPS
	 */
	public int thinkTimeMillis = 40;
	
	public boolean replay = false;
	public File replayFile = null;
}
