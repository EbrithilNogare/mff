package game;

import controllers.ghosts.GhostsActions;
import controllers.pacman.PacManAction;
import game.core.*;

import java.awt.event.KeyListener;
import java.util.Random;

/**
 * One simulator can run one instance of PacMan-vs-Ghosts game.
 * 
 * Can be used for both head/less games.
 * 
 * @author Jimmy
 */
public class PacManSimulator {
	private GameView gv;
	private _G_ game;
	
    private long due; 
    
    private StringBuilder replayData;
    private boolean replayFirstWrite;

	public synchronized Game run(final SimulatorConfig config) {
		gv = null;
		game = null;		
		
		// INIT RANDOMNESS
		if (config.game.seed < 0) {
			config.game.seed = new Random(System.currentTimeMillis()).nextInt();
			while (config.game.seed < 0) config.game.seed += Integer.MAX_VALUE;
		}
		G.rnd = new Random(config.game.seed);
		
		// INITIALIZE THE SIMULATION
		game = new _G_();
		game.newGame(config.game);
		
		// RESET CONTROLLERS
		config.pacManController.reset(game);
		if (config.ghostsController != null) config.ghostsController.reset(game);

		// INITIALIZE THE VIEW
		if (config.visualize) {
			gv = new GameView(game, 3);
			gv.showGame();
			
			if (config.pacManController instanceof KeyListener) {				
				gv.getFrame().addKeyListener((KeyListener)config.pacManController);
			}
			if (config.ghostsController != null && config.ghostsController instanceof KeyListener) {				
				gv.getFrame().addKeyListener((KeyListener)config.ghostsController);
			}
		} 
		
		// SETUP REPLAY RECORDING
		int lastLevel = game.getCurLevel();
		if (config.replay) {
			replayData = new StringBuilder();
			replayFirstWrite = true;
		}
		
		// START CONTROLLERS (threads auto-start during instantiation)
		ThinkingThread pacManThread = 
			new ThinkingThread(
				"PAC-MAN",
				new IThinkingMethod() {
					@Override
					public void think() {
						config.pacManController.tick(game.copy(), due);		
					}
				}
			);
		ThinkingThread ghostsThread =
			new ThinkingThread(
				"GHOSTS",
				new IThinkingMethod() {
					@Override
					public void think() {
                        if (config.ghostsController != null)
                            config.ghostsController.tick(game, due);			
					}
				}
			);
        
		// START THE GAME
		try {
			while(!game.gameOver())
			{
				due = System.currentTimeMillis() + config.thinkTimeMillis;
				
				pacManThread.startThinking();
				ghostsThread.startThinking();
                
                if (!pacManThread.waitForResult(due))
                    System.out.println("[SIMULATOR] PacMan is still thinking!");

                if (!ghostsThread.waitForResult(due))
                    System.out.println("[SIMULATOR] Ghosts are still thinking!");

                if (config.visualize) {
                    long sleepTime = due - System.currentTimeMillis();
                    if (sleepTime > 4) {
                        try {
                           Thread.sleep(sleepTime);
                        } catch (InterruptedException e) {
                            throw new RuntimeException(e);
                        }
                    }
                }
		        
		        // OBTAIN ACTIONS
		        PacManAction  pacManAction  = config.pacManController.getAction().clone();
                GhostsActions ghostsActions =
                    config.ghostsController == null ? null :
                                                      config.ghostsController.getActions().clone();
				
		        // SIMULATION PAUSED?
		        boolean advanceGame = true;
                if (pacManAction.pauseSimulation || (ghostsActions != null && ghostsActions.pauseSimulation)) {
                    if (!pacManAction.nextFrame && (ghostsActions == null || !ghostsActions.nextFrame)) {
                        advanceGame = false;
                    }
                    config.pacManController.getAction().nextFrame = false;
                    if (config.ghostsController != null)
                        config.ghostsController.getActions().nextFrame = false;
                }
		        
		        // ADVANCE GAME
		        if (advanceGame) {
		        	int pacManLives = game.getLivesRemaining();
		        	
			        int replayStep[] = game.advanceGame(pacManAction, ghostsActions);
			        
			        // SAVE ACTIONS TO REPLAY
			        if (config.replay && replayStep != null) {
			        	// STORE ACTIONS
			        	storeActions(replayStep, game.getCurLevel()==lastLevel);
			        }
			        
			        // NEW LEVEL?
			        if (game.getCurLevel() != lastLevel) {
			        	lastLevel=game.getCurLevel();
			        	
			        	// INFORM CONTROLLERS
			        	config.pacManController.nextLevel(game.copy());
                        if (config.ghostsController != null)
                            config.ghostsController.nextLevel(game.copy());
			    		
			    		// FLUSH REPLAY DATA TO FILE
			    		if (config.replay) {
			    			Replay.saveActions(
                                config.game,
                                config.ghostsController == null ? 0 : config.ghostsController.getGhostCount(),
                                replayData.toString(), config.replayFile, replayFirstWrite);
			        		replayFirstWrite = false;
			        		replayData = new StringBuilder();
			    		}
			        }
			        
			        // PAC MAN KILLED?
			        if (pacManLives != game.getLivesRemaining()) {
			        	config.pacManController.killed();
			        }
		        }
		        
		        // VISUALIZE GAME
		        if (config.visualize) {
		        	gv.repaint();
		        }
			}
		} finally {		
			// KILL THREADS
			pacManThread.kill();
			ghostsThread.kill();
			
			// SAVE REPLAY DATA
			if (config.replay) {
				Replay.saveActions(
                    config.game,
                    config.ghostsController == null ? 0 : config.ghostsController.getGhostCount(),
                    replayData.toString(), config.replayFile, replayFirstWrite);
			}
			
			// CLEAN UP
			if (config.visualize) {
				if (config.pacManController instanceof KeyListener) {				
					gv.getFrame().removeKeyListener((KeyListener)config.pacManController);
				}
				if (config.ghostsController instanceof KeyListener) {				
					gv.getFrame().removeKeyListener((KeyListener)config.ghostsController);
				}
				
				gv.getFrame().setTitle("[FINISHED]");
				gv.repaint();
			}					
		}
		
		return game;
	}

	private void storeActions(int[] replayStep, boolean newLine) {
		replayData.append( (game.getTotalTime()-1) + "\t" );
	
	    for (int i=0;i < replayStep.length; i++) {
	    	replayData.append(replayStep[i]+"\t");
	    }
	
	    if(newLine) {
	    	replayData.append("\n");
	    }
	}
	
	/**
	 * Run simulation according to the configuration.
	 */
	public static Game play(SimulatorConfig config) {
		PacManSimulator simulator = new PacManSimulator();
		return simulator.run(config);		
	}
}
