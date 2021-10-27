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

import controllers.ghosts.GhostsActions;
import controllers.pacman.PacManAction;

/*
 * This interface defines the contract between the game engine and the controllers. It provides all
 * the methods a controller may use to (a) query the game state, (b) compute game-related attributes
 * and (c) test moves by using a forward model (i.e., copy() followed by advanceGame()).
 */
public interface Game
{
    // These constants specify the exact nature of the game
    
	public static final int UP=0;							//direction going up
	public static final int RIGHT=1;						//direction going right
	public static final int DOWN=2;							//direction going down
	public static final int LEFT=3;							//direction going left
	public static final int EMPTY=-1;						//value of an non-existing neighbour
	public static final int PILL=10;						//points for a pill
    public static final int POWER_PILL=50;					//points for a power pill
    
    //score for the first ghost eaten (doubles every time for the duration of a single power pill)
    public static final int GHOST_EAT_SCORE=200;			
    
    //initial time a ghost is edible for (decreases as level number increases) 
    public static final int EDIBLE_TIME=200;				
    
    //reduction factor by which edible time decreases as level number increases
    public static final float EDIBLE_TIME_REDUCTION=0.9f;	
    
    //time spent in the lair by each ghost at the start of a level
    public static final int[] LAIR_TIMES={40,60,80,100};	
    
    public static final int COMMON_LAIR_TIME=40;			//time spend in lair after being eaten
    
    //reduction factor by which lair times decrease as level number increases
    public static final float LAIR_REDUCTION=0.9f;			
    
    public static final int LEVEL_LIMIT=3000;				//time limit for a level
    
    //probability of a global ghost reversal event
    public static final float GHOST_REVERSAL=0.0015f;
    
    //maximum number of levels played before the end of the game
    public static final int MAX_LEVELS=16;					
    
    //extra life is awarded when this many points have been collected
    public static final int EXTRA_LIFE_SCORE=10000;			
    
    //distance in the connected graph considered close enough for an eating event to take place
    public static final int EAT_DISTANCE=2;					
    
	public static final int NUM_GHOSTS=4;					//number of ghosts in the game
    public static final int NUM_MAZES=4;					//number of different mazes in the game
    
    //total number of lives Ms Pac-Man has (current + NUM_LIVES-1 spares)
    public static final int NUM_LIVES=3;					
    
    public static final int INITIAL_PAC_DIR=3;				//initial direction taken by Ms Pac-Man
    
    //initial directions for the ghosts (after leaving the lair)
    public static final int[] INITIAL_GHOST_DIRS={3,3,3,3};	
    
    //difference in speed when ghosts are edible
    // (every GHOST_SPEED_REDUCTION, a ghost remains stationary)	
	public static final int GHOST_SPEED_REDUCTION=2;		
	
    public Game copy();						//returns an exact copy of the game (forward model)
    
    //advances the game using the actions (directions) supplied;
    // returns all directions played [PacMan, Ghost1, Ghost2, Ghost3, Ghost4]
	public int[] advanceGame(PacManAction pacMan, GhostsActions ghosts);	
    
    public int getReverse(int direction);		//returns the reverse of the direction supplied

    //returns true is Ms Pac-Man has lost all her lives or if MAX_LEVELS has been reached
    public boolean gameOver();								
    		
    public boolean checkPill(int pillIndex);	//checks if the pill specified is still available
    
    //checks if the power pill specified is still available
	public boolean checkPowerPill(int powerPillIndex);	
    
    //returns an array of size 4, indicating neighbouring nodes for the current position
    // of Ms Pac-Man. E.g., [-1,12,-1,44] for neighbours 12 and 44 in direction RIGHT and LEFT
    public int[] getPacManNeighbours();
    
    //returns an array of size 4, indicating neighbouring nodes for the current position
    // of the ghost specified. Replaces the direction corresponding to the opposite
    // previous direction with -1
	public int[] getGhostNeighbours(int whichGhost);				
	
	public int getCurLevel();		//returns the current level; the first level is 1
	public int getCurMaze();				//returns the current maze
	public int getCurPacManLoc();			//returns the node index Ms Pac-Man is at
	public int getCurPacManDir();			//returns the last direction taken by Ms Pac-Man
	public int getLivesRemaining();			//returns the number of lives remaining for Ms Pac-Man
    public int getCurGhostLoc(int whichGhost);		//returns the node index for the ghost specified
    
    //returns the last direction taken by the ghost specified
    public int getCurGhostDir(int whichGhost);	
    
    //returns the edible time (time left in which the ghost can be eaten) for the ghost specified
    public int getEdibleTime(int whichGhost);	
    					
	public boolean isEdible(int whichGhost);		//returns true if the ghost is currently edible
	public int getScore();							//returns the score of the game
    public int getLevelTime();		//returns the time for which the CURRENT level has been played
    
    //returns the time for which the game has been played (across all levels)
    public int getTotalTime();	
    
    //returns the total number of pills in this maze (at the beginning of the level)
    public int getNumberPills();		
    
    //returns the total number of power pills in this maze (at the beginning of the level)
    public int getNumberPowerPills();		
    
    //returns the time remaining the ghost specified spends in the lair
    public int getLairTime(int whichGhost);		
    
    //returns true of ghost is at a junction and a direction is needed	
    public boolean ghostRequiresAction(int whichGhost);		
    		
    public String getName();	//returns the name of the maze
    
    //returns the position where Ms Pac-Man starts at the beginning of the level
    public int getInitialPacPosition();
    
    //returns the position where the ghosts starts at the beginning of the level,
    // AFTER leaving the lair
    public int getInitialGhostsPosition();							
    
    //returns the total number of nodes in the graph (pills, power pills and empty)
    public int getNumberOfNodes();	
    
	public int getX(int nodeIndex);					//returns the x-coordinate of the node specified
    public int getY(int nodeIndex);					//returns the y-coordinate of the node specified
    
    //returns the pill index of the node specified (can be used with the bitset for the pills)
    public int getPillIndex(int nodeIndex);

    //returns the power pill index of the node specified
    // (can be used with the bitset for the power pills)
    public int getPowerPillIndex(int nodeIndex);
    
    //returns the neighbour of the node specified for the direction supplied
    public int getNeighbour(int nodeIndex,int direction);
    			
	public int[] getPillIndices();				//returns indices to all nodes with pills
	public int[] getPowerPillIndices();			//returns indices to all nodes with power pills	
	public int[] getJunctionIndices();			//returns indices to all nodes that are junctions
    
    //returns the score awarded for the next ghost to be eaten
    public int getNextEdibleGhostScore();
    	
	public int getNumActivePills();			//returns the number of pills still in the maze
	public int getNumActivePowerPills();	//returns the number of power pills still in the maze
    public int[] getPillIndicesActive();	//returns the indices of all active pills in the maze
    
    //returns the indices of all active power pills in the maze
	public int[] getPowerPillIndicesActive(); 
    
    //returns true if node is a junction (more than 2 neighbours)
    public boolean isJunction(int nodeIndex);	
    
    //returns the number of neighbours of the node specified
	public int getNumNeighbours(int nodeIndex);	
    
    //simple enumeration for use with the direction methods (below)
    public enum DM{PATH,EUCLID,MANHATTAN};
    
    //returns the direction Ms Pac-Man should take to approach/retreat from the node specified,
    // using the distance measure specified
    public int getNextPacManDir(int to,boolean closer,DM measure);
    
    //returns the direction the ghost specified should take to approach/retreat from
    // the node specified, using the distance measure specified
	public int getNextGhostDir(int whichGhost,int to,boolean closer,DM measure);	
    
    //returns the shortest path distance (Dijkstra) from one node to another
    public int getPathDistance(int from,int to);
        
    //returns the Euclidean distance between two nodes
    public double getEuclideanDistance(int from,int to);
        
    //returns the Manhattan distance between two nodes
	public int getManhattanDistance(int from,int to);
    
    // returns the set of possible directions for Ms Pac-Man, with or without
    // the direction opposite to the last direction taken
    public int[] getPossiblePacManDirs(boolean includeReverse);
    
    //returns the set of possible directions for the ghost specified
    //(excludes the opposite of the previous direction)
    public int[] getPossibleGhostDirs(int whichGhost);
                    
    // returns the possible directions from the given location
	public int[] getPossibleDirs(int curLoc,int curDir,boolean includeReverse);  
    
    //returns the path from one node to another (e.g., [1,2,5,7,9] for 1 to 9)
    public int[] getPath(int from,int to);
        
    //returns the path from one node to another, taking into account that reversals are not possible
    public int[] getGhostPath(int whichGhost,int to);
        
    //selects a target from 'targets' given current position ('from'), a distance measure
    // and whether it should be the point closest or farthest
    public int getTarget(int from,int[] targets,boolean nearest,DM measure);
        
    //selects a target for a ghost (accounts for the fact that ghosts may not reverse)
    public int getGhostTarget(int from,int[] targets,boolean nearest);
            
    //returns the distance of a path for the ghost specified (accounts for the fact
    //that ghosts may not reverse)
    public int getGhostPathDistance(int whichGhost,int to);	
    
    // Return the location at which a fruit currently exists, or -1 if none.
    public int getFruitLoc();
    
    // Return the type of fruit that currently exists, or -1 if none.  Fruit types
    // are numbered 0 = cherries, 1 = strawberry, ..., 6 = banana.
    public int getFruitType();

    // Return the point value of the fruit that currently exists, or 0 if none.
    public int getFruitValue();
}
