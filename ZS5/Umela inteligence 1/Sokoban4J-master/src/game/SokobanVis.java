package game;

import java.awt.event.KeyListener;
import java.util.*;

import javax.swing.SwingUtilities;

import agents.HumanAgent;
import game.actions.EDirection;
import game.actions.oop.*;
import game.board.compact.BoardCompact;
import game.board.oop.Board;
import ui.*;
import ui.actions.IUIAction;
import ui.actions.UIActionFactory;
import ui.atlas.SpriteAtlas;
import ui.utils.TimeDelta;

public class SokobanVis implements ISokobanGame, Runnable {
	// SETUP
	
	private Board board;
	private IAgent agent;
	private SpriteAtlas sprites;
	private UIBoard uiBoard;
	private SokobanView view;
	private SokobanFrame frame;
	
	// THREAD
	
	private Thread gameThread;
	
	// RUNTIME
	
	private SokobanGameState state;
	
	private TimeDelta timeDelta;
	
	private IAction agentAction;
	private IUIAction uiAction;
	
	private boolean observe = true;
	
	private boolean firstPack = true;

	private boolean shouldRun = true;
	
	// RESULT
	
	private SokobanResult result;
	
	public SokobanVis(
        SokobanConfig config, Board board, IAgent agent, SpriteAtlas sprites, UIBoard uiBoard,
        SokobanView view, SokobanFrame frame) {
            
		// SETUP
		
		this.board = board;
		this.agent = agent;
		this.sprites = sprites;
		this.uiBoard = uiBoard;
		this.view = view;
		this.frame = frame;
		
		// RUNTIME
		
		this.state = SokobanGameState.INIT;
		
		this.timeDelta = new TimeDelta();
		
		if (agent instanceof KeyListener) {
			frame.addKeyListener((KeyListener)agent);
		}
		
		// RESULT
        
        result = new SokobanResult(config);
	}
	
	@Override
	public void startGame() {
		if (state != SokobanGameState.INIT) return;
		try { 
			state = SokobanGameState.RUNNING;
			gameThread = new Thread(this, "SokobanVis");
			gameThread.start();
		} catch (Exception e) {
			stopGame();  
			onSimulationException(e);
		}
	}
	
	@Override
	public void stopGame() {
		if (state != SokobanGameState.RUNNING) return;
		try {
			shouldRun = false;
			gameThread.interrupt();
			try {
				if (gameThread.isAlive()) {
					gameThread.join();
				}
			} catch (Exception e) {			
			}
			gameThread = null;
			onTermination();
		} catch (Exception e) {
			onSimulationException(e);
		}
	}
	
	private boolean renderUpdating = false;
    
    void requestPaint() {
        if (!renderUpdating) {
            renderUpdating = true;
            SwingUtilities.invokeLater(new Runnable() {
                @Override
                public void run() {
                    view.render();
                    view.repaint();
                    frame.repaint();
                    renderUpdating = false;
                }
            });
        }
    }

	@Override
	public void run() {
        Stack<IAction> actions = new Stack<IAction>();
        boolean undo = false;

		try {
			frame.setVisible(true);
		
			try {
				agent.newLevel();
			} catch (Exception e) {
				onAgentException(e);
				return;
			}
			
			timeDelta.reset();
			
			renderUpdating = false;
            requestPaint();
            
			while (shouldRun && !Thread.interrupted()) {
				timeDelta.tick();
				
				if (uiAction != null) {
					// ADVANCE UI ACTION
                    if (!uiAction.isFinished()) uiAction.tick(timeDelta);
                    
					// HANDLE FINISHED ACTION
					if (uiAction.isFinished()) {
                        if (undo)
                            agentAction.undo(board);
                        else
                            agentAction.perform(board);
                            
						uiAction.finish();
						uiAction = null;
						agentAction = null;
                        observe = true;
                        undo = false;
					}
					
                    // UPDATE RENDER
                    requestPaint();
				}
							
				if (board.isVictory()) {					
					onVictory();				 
					return;
				}
				
				try {
					Thread.sleep(16);
				} catch (InterruptedException e) {
					// WE HAVE TO STOP...
					onTermination();
					return;
				}
								
				if (uiAction != null) {
					// WE HAVE UI ACTION IN EFFECT ... wait till it finishes
					continue;
				}
					
				if (firstPack) {
					frame.pack();
					firstPack = false;
				}
				
				// OTHERWISE QUERY AGENT FOR THE NEXT ACTION
				
				if (observe) {
					// EXTRACT COMPACT VERSION OF THE BOARD FOR AI
					BoardCompact compactBoard = board.makeBoardCompact();
					// PRESENT BOARD TO THE AGENT
					agent.observe(compactBoard);
					observe = false;
				}
					
                // GET AGENT ACTION
                
                Command command = Command.None;

                if (agent instanceof HumanAgent) {
                    command = ((HumanAgent) agent).getCommand();
                    if (command == Command.Undo) {
                        if (actions.empty())
                            continue;
                        agentAction = actions.peek();
                        undo = true;
                    }
                }

                if (command == Command.None) {
                    EDirection whereToMove = agent.act();
                    if (whereToMove == null || whereToMove == EDirection.NONE) continue;
                    agentAction = Move.orPush(board, whereToMove);
                }
	
				// AGENT ACTION VALID?
				if (agentAction.isPossible(board) || undo) {
					// START PERFORMING THE ACTION
					uiAction = UIActionFactory.createUIAction(
                                    board, sprites, uiBoard, agentAction, undo);
					if (uiAction == null) {
						// INVALID ACTION
						agentAction = null;					 
					} else {
						result.setSteps(result.getSteps() + (undo ? -1 : 1));
                        if (undo)
                            actions.pop();
                        else
                            actions.push(agentAction);
                        frame.setSteps(result.getSteps());
					}
				} else {
					agentAction = null;
				}
			}
		} catch (Exception e) {
			onSimulationException(e);
		} finally {
			frame.setVisible(false);			
		}
	}

	private void onSimulationException(Exception e) {
		result.setResult(SokobanResultType.SIMULATION_EXCEPTION);
		result.setException(e);
		try {
			agent.stop();
		} catch (Exception e2) {						
		}		
		shouldRun = false;
		state = SokobanGameState.FAILED;
	}

	private void onTermination() {
		result.setResult(SokobanResultType.TERMINATED);
		try {
			agent.stop();
		} catch (Exception e) {						
		}
		shouldRun = false;
		state = SokobanGameState.TERMINATED;
	}

	private void onVictory() {
		result.setResult(SokobanResultType.VICTORY);
		try {
			agent.victory();
		} catch (Exception e) {
			onAgentException(e);
			return;
		}		
		state = SokobanGameState.FINISHED;
		try {
			agent.stop();
		} catch (Exception e) {						
		}
	}

	private void onAgentException(Exception e) {
		result.setResult(SokobanResultType.AGENT_EXCEPTION);
		result.setException(e);
		try {
			agent.stop();
		} catch (Exception e2) {						
		}		
		shouldRun = false;
		state = SokobanGameState.FAILED;
	}

	@Override
	public SokobanGameState getGameState() {
		return state;
	}

	@Override
	public SokobanResult getResult() {
		if (state == SokobanGameState.INIT || state == SokobanGameState.RUNNING) return null;
		return result;
	}

	@Override
	public void waitFinish() throws InterruptedException {
        if (state == SokobanGameState.RUNNING && gameThread != null && gameThread.isAlive())
            gameThread.join();
	}
	
}
