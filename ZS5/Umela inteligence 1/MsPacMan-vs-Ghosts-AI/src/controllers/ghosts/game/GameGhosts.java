package controllers.ghosts.game;

import controllers.ghosts.GhostsActions;
import controllers.ghosts.IGhostsController;
import game.core.G;
import game.core.Game;
import game.core.GameView;

import java.awt.Color;

/**
 * This is the class you need to modify for your entry. In particular, you need to
 * fill in the getActions() method. Any additional classes you write should either
 * be placed in this package or sub-packages (e.g., game.entries.ghosts.mypackage).
 */
public class GameGhosts implements IGhostsController
{
	public final int NUM_SCATTERS_PER_LEVEL = 4;
	public final int SCATTER = 0;
	public final int CHASE = 1;
	public final int FRIGHTENED = 2;
	
	public int ghostCount = 4;
	
	public GhostsActions actions;
	
	public GameGhostAI GhostCurrentAI;
	public GameGhostAI GhostPrevAI;
	public long stateChangeCurrentTime;
	public long stateChangeShiftTime;
	public int numberOfScatterOccurred;
	public int numberOfChaseOccurred;
	
	public GhostScatterState ScatterHandler;
	public GameGhostChaseState ChaseHandler;
	public GameGhostFrightenedState FrightHandler;
	
	public int currentGlobalState;
	
	public boolean Debugging = false;
	
	int X;
	int Y;
	
	int[] ghostTarget;
	
	/**
	 * [RED, PINK, ORANGE, BLUE]
	 */
	public int[] GhostState;			
	
	public GameGhosts() {
		this(4, false);
	}
	
	public GameGhosts(int ghostCount) {
		this((ghostCount < 0 ? 0 : (ghostCount > 4 ? 4 : ghostCount)), false);
	}
	
	public GameGhosts(int ghostCount, boolean debugging){
		this.ghostCount = ghostCount;
		Debugging = debugging;
		actions = new GhostsActions(ghostCount);
	}
	
	@Override
	public int getGhostCount() {
		return ghostCount;
	}
	
	
	@Override
	public void reset(Game game) {
		actions.reset();
		
		GhostCurrentAI = new GhostScatterState();
		GhostPrevAI = new GhostScatterState();
		stateChangeCurrentTime= game.getTotalTime();
		stateChangeShiftTime = stateChangeCurrentTime + (7*25);
		numberOfScatterOccurred = 0;
		numberOfChaseOccurred = 0;
		
		ScatterHandler = new GhostScatterState();
		ChaseHandler   = new GameGhostChaseState();
		FrightHandler  = new GameGhostFrightenedState();
		
		currentGlobalState = SCATTER;

		X = 0;
		Y = 1;
		
		ghostTarget = new int[]{0,0};
		
        GhostState = new int[]{SCATTER, SCATTER, SCATTER, SCATTER};
        
        // We must make sure that the Color class is loaded now.  Otherwise it will first be
        // loaded inside Game.tick(), which can take some time and cause the ghosts to
        // miss their first tick timeout.
        try {
            Class.forName("java.awt.Color");
        } catch (ClassNotFoundException e) {
            throw new RuntimeException(e);
        }
	}
	
	@Override
	public void nextLevel(Game game) {		
	}
	
	@Override
	public void tick(Game game, long timeDue) {
		if(game.getLevelTime()<=10){
			numberOfScatterOccurred = 0;
			numberOfChaseOccurred = 0;
			stateChangeShiftTime = game.getTotalTime();
		}
		
		int[] directions = new int[]{3, 3, 3, 3};
		
		long stateChangeTimer = stateChangeShiftTime - game.getTotalTime();
		if(stateChangeTimer<0 && numberOfScatterOccurred<NUM_SCATTERS_PER_LEVEL){
			int nextStateTimeinSec = 0;
			if(currentGlobalState == SCATTER){
				currentGlobalState = CHASE;
				numberOfScatterOccurred ++;
				if(game.getCurLevel() == 1){
					nextStateTimeinSec = 20;
				}
				else if(game.getCurLevel()<5){
					if(numberOfChaseOccurred<2){
						nextStateTimeinSec = 20;
					}
					else{
						nextStateTimeinSec = 1033;
					}
				}
				else{
					if(numberOfChaseOccurred<2){
						nextStateTimeinSec = 20;
					}
					else{
						nextStateTimeinSec = 1037;
					}
				}
				
			}
			else{
				currentGlobalState = SCATTER;
				if(game.getCurLevel() == 1){
					if(numberOfScatterOccurred<2){
						nextStateTimeinSec = 7;
					}
					else{
						nextStateTimeinSec = 5;
					}
				}
				else if(game.getCurLevel()<5){
					if(numberOfScatterOccurred<2){
						nextStateTimeinSec = 7;
					}
					else if(numberOfScatterOccurred == 2){
						nextStateTimeinSec = 5;
					}
					else{
						nextStateTimeinSec = 1;
					}
				}
				else{
					if(numberOfScatterOccurred<2){
						nextStateTimeinSec = 5;
					}
					else if(numberOfScatterOccurred == 2){
						nextStateTimeinSec = 5;
					}
					else{
						nextStateTimeinSec = 1;
					}
				
				}
				numberOfChaseOccurred++;
			}
			stateChangeShiftTime = game.getTotalTime() + (nextStateTimeinSec*25);

			// REWRITE DIRECTIONS
			for (int i = 0; i < directions.length; ++i) {
				actions.ghost(i).set(directions[i]);
			}
			return;
		}
		
		for(int i =0; i<Game.NUM_GHOSTS; i++){
			if(game.getEdibleTime(i)>0){
				GhostState[i]=FRIGHTENED;
			}
			else if(GhostState[i]!= currentGlobalState){
				GhostState[i] = currentGlobalState;
			}
			
			if(GhostState[i]== SCATTER){
				ghostTarget = ScatterHandler.execute(i, game, timeDue);
			}
			if(GhostState[i]== CHASE){
				ghostTarget= ChaseHandler.execute(i, game, timeDue);
			}
			if(GhostState[i]!= FRIGHTENED){
				Color color;
				if(i==0){
					color = Color.RED;
				}
				else if(i==1){
					color = Color.PINK;
				}
				else if (i==2){
					color = Color.ORANGE;
				}
				else{
					color = Color.CYAN;
				}
                if (Debugging) {
					GameView.addLines(
                        game, color,
                        game.getX(game.getCurGhostLoc(i)), game.getY(game.getCurGhostLoc(i)),
                        ghostTarget[X], ghostTarget[Y]);
				}
            }
			int chosenDirection = -1;
			if(game.ghostRequiresAction(i) && GhostState[i] != FRIGHTENED){
				int[] possibleDirections = game.getPossibleGhostDirs(i);
				double chosenDirectionDistance = 100000;
				boolean equalPathsCheck = false;
				for(int j=0; j<possibleDirections.length;j++){
					int directionNodeNum = game.getNeighbour(game.getCurGhostLoc(i), possibleDirections[j]);
                    double distanceBetweenNeighbor =
                        GameGhosts.getEuclideanDistance(
                            game.getX(directionNodeNum), game.getY(directionNodeNum),
                            ghostTarget[X],ghostTarget[Y]);
					
					if(Double.compare(distanceBetweenNeighbor, chosenDirectionDistance) < 0){
						equalPathsCheck = false;
						chosenDirectionDistance = distanceBetweenNeighbor;
						chosenDirection = possibleDirections[j];
					}
					else if(Double.compare(distanceBetweenNeighbor, chosenDirectionDistance) == 0){
						equalPathsCheck = true;
					}
					
				}
				if (equalPathsCheck){
					boolean leftPriority = false;
					for(int j=0; j<possibleDirections.length; j++){
						if(possibleDirections[j] == Game.UP){
							chosenDirection = Game.UP;
							break;
						}
						else if( possibleDirections[j] == Game.LEFT){
							chosenDirection = Game.LEFT;
							leftPriority = true;
						}
						else if(possibleDirections[j] == Game.DOWN  && !leftPriority){
							chosenDirection = Game.DOWN;
						}
						
					}
				}
				
			}
			else if(game.ghostRequiresAction(i) && GhostState[i] == FRIGHTENED){
				int[] ghostPossibleDirs = game.getPossibleGhostDirs(i);
				if(ghostPossibleDirs.length >0){
					int numOfPossibleDirs = Math.abs(G.rnd.nextInt())%ghostPossibleDirs.length;
					if(numOfPossibleDirs < 0){
						chosenDirection = ghostPossibleDirs[numOfPossibleDirs];
					}
				}
			}
			directions[i]=chosenDirection;
		}

		// REWRITE DIRECTIONS
		for (int i = 0; i < directions.length; ++i) {
			actions.ghost(i).set(directions[i]);
		}
	}
	
	@Override
	public GhostsActions getActions() {
		return actions;
	}
	
	public static double getEuclideanDistance(int x1, int y1, int x2, int y2){
		return Math.sqrt(Math.pow((x2 - x1), 2) + Math.pow((y2-y1), 2));
	}

}
