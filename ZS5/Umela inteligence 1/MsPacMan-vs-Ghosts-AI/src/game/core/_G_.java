/*
 * Implementation of "Ms Pac-Man" for the "Ms Pac-Man versus Ghost Team Competition", brought
 * to you by Philipp Rohlfshagen, David Robles and Simon Lucas of the University of Essex.
 * 
 * www.pacman-vs-ghosts.net
 * 
 * Code written by Philipp Rohlfshagen, based on earlier implementations of the game by
 * Simon Lucas and David Robles. 
 * 
 * You may use and distribute this code freely for non-commercial purposes. This notice 
 * needs to be included in all distributions. Deviations from the original should be 
 * clearly documented. We welcome any comments and suggestions regarding the code.
 */
package game.core;

import game.GameConfig;

public class _G_ extends G
{
	public static final int EDIBLE_ALERT=30;	//for display only (ghosts turning blue)
	
	public _G_(){}
	
	//Instantiates everything to start a new game
	public void newGame(GameConfig config)
	{	
		this.config = config;
		this.remainingLevels = config.levelsToPlay;
		
		init();		//load mazes if not yet loaded

        setLevel(config.startingLevel);
		
		curGhostLocs=new int[G.NUM_GHOSTS];
		lastGhostDirs=new int[G.NUM_GHOSTS];
		edibleTimes=new int[G.NUM_GHOSTS];
        lairTimes=new int[G.NUM_GHOSTS];
        lairX = new int[G.NUM_GHOSTS];
        lairY = new int[G.NUM_GHOSTS];

		score=0;
		totalTime=0;
		
		livesRemaining=config.lives;
		extraLife=false;
		gameOver=false;
		
        newBoard();
		reset(false);
	}
	
	//Size of the Maze (for display only)
	public int getWidth()
	{
		return mazes[curMaze].width;
	}
	
	//Size of the Maze (for display only)
	public int getHeight()
	{
		return mazes[curMaze].height;
	}
}
