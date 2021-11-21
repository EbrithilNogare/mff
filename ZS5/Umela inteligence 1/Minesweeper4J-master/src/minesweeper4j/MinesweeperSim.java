package minesweeper4j;

import java.util.Random;

import minesweeper4j.game.Action;
import minesweeper4j.game.Board;
import minesweeper4j.game.IAgent;
import minesweeper4j.game.MinesweeperResult;
import minesweeper4j.game.MinesweeperResult.MinesweeperResultType;
import minesweeper4j.ui.MinesweeperFrame;

public class MinesweeperSim implements IMinesweeperGame, Runnable {

	// SETUP
	
	private Board board;
	private IAgent agent;
	private long timeoutMillis;
	private boolean visualization;
	
	
	// THREAD
	
	private Thread gameThread;
	
	// RUNTIME
	
	private MinesweeperGameState state;
	
	private Action agentAction;
	
	private boolean observe = true;
	
	private boolean shouldRun = true;
	
	private MinesweeperFrame vis;
	
	// RESULT
	
	private MinesweeperResult result = new MinesweeperResult();
	
	private int steps = 0;
	
	private int safeTileSuggestions = 0;
	
	private Random random;
	
	/**
	 * @param id
	 * @param board
	 * @param agent
	 * @param sprites
	 * @param uiBoard
	 * @param view
	 * @param frame
	 * @param timeoutMillis negative number or zero == no time; in milliseconds
	 */
	public MinesweeperSim(String id, Board board, IAgent agent, long timeoutMillis, boolean visualization, Random random) {
		// SETUP
		
		if (id == null) id = "MinesweeperSim";		
		this.board = board;
		this.agent = agent;
		this.timeoutMillis = timeoutMillis;
		this.visualization = visualization;
		this.random = random;
		
		// RUNTIME
		
		this.state = MinesweeperGameState.INIT;
		
		// RESULT
		
		result.setId(id);
		result.setAgent(agent);
		result.setLevel(board.level == null ? "N/A" : board.level);
	}
	
	@Override
	public void startGame() {
		if (state != MinesweeperGameState.INIT) return;
		try { 
			state = MinesweeperGameState.RUNNING;
			gameThread = new Thread(this, "MinesweeperSim");
			gameThread.start();
		} catch (Exception e) {
			stopGame();  
			onSimulationException(e);
		}
	}
	
	@Override
	public void stopGame() {
		if (state != MinesweeperGameState.RUNNING) return;
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
	
	@Override
	public void run() {
		try {
			result.setSimStartMillis(System.currentTimeMillis());
			
			// FIRST SUGGESTION
			if (board.safeTilePos == null) {
				board.suggestSafeTile(random);
				safeTileSuggestions += 1;
			}
			
			try {
				agent.newBoard();
			} catch (Exception e) {
				onAgentException(e);
				return;
			}
			
			if (visualization) {
				vis = new MinesweeperFrame(board, agent);
				vis.setVisible(true);
				vis.getPanel().updateBoardView();
			}
			
			result.setSimStartMillis(System.currentTimeMillis());			
			
			while (shouldRun && !Thread.interrupted()) {
				if (visualization)
					Thread.sleep(50);

				// TIMEOUT?
				if (timeoutMillis > 0) {
					long now = System.currentTimeMillis();
					long timeLeftMillis = timeoutMillis - (now - result.getSimStartMillis());
					if (timeLeftMillis <= 0) {						
						onTimeout();
						return;
					}			
					if (visualization) {
						long secs = timeLeftMillis / 1000;
						long millis = (timeLeftMillis - secs * 1000) / 100;
						vis.setTitle("Minesweeper4J: " + secs + "." + millis + "s" + " | #advices = " + safeTileSuggestions);
					}
				}
				
				// VICTORY?
				if (board.isVictory()) {					
					onVictory();				 
					return;
				}
				
				// DIED?
				if (board.boom) {
                    System.err.println("** Stepped on a mine! **");
					onDied();
					return;
				}
				
				// OTHERWISE QUERY AGENT FOR THE NEXT ACTION
				
				if (observe) {
					// UPDATE UI
					if (vis != null) {
						vis.getPanel().updateBoardView();
					}
					
					// EXTRACT COMPACT VERSION OF THE BOARD FOR AI
					Board agentBoard = board.getAgentView();
					
					// PRESENT BOARD TO THE AGENT
					agent.observe(agentBoard);
					observe = false;
				}
									
				// GET AGENT ACTION
				Action action = agent.act();
				
				if (action == null) continue;
				
				agentAction = action;
	
				// AGENT ACTION VALID?
				if (agentAction != null && agentAction.isPossible(board)) {
					// PERFORM THE ACTION
					switch (agentAction.type) {
					case SUGGEST_SAFE_TILE:
						board.suggestSafeTile(random);
						safeTileSuggestions += 1;
						break;
					case FLAG:
						board.flagTile(agentAction.tileX, agentAction.tileY);
						break;
					case UNFLAG:
						board.unflagTile(agentAction.tileX, agentAction.tileY);
						break;
					case OPEN:
						board.uncoverTile(agentAction.tileX, agentAction.tileY);
						break;
					}
					++steps;
					observe = true;
				} else
                    System.out.printf("warning: ignoring invalid action: %s\n", agentAction);
				
				agentAction = null;
			}
		} catch (Exception e) {
			onSimulationException(e);
		} finally {
			try {
				if (visualization) {
					vis.getPanel().updateBoardView();
					Thread.sleep(2000);					
				}
			} catch (Exception e) {				
			} finally {
				try {
					vis.setVisible(false);
					vis.dispose();
					vis = null;
				} catch (Exception e) {					
				}
			}
			
		}
	}

	private void onSimulationException(Exception e) {
		result.setSimEndMillis(System.currentTimeMillis());
		result.setResult(MinesweeperResultType.SIMULATION_EXCEPTION);
		result.setException(e);
		result.setSafeTileSuggestions(safeTileSuggestions);
		try {
			agent.stop();
		} catch (Exception e2) {						
		}		
		shouldRun = false;
		state = MinesweeperGameState.FAILED;
	}

	private void onTermination() {
		result.setSimEndMillis(System.currentTimeMillis());
		result.setResult(MinesweeperResultType.TERMINATED);
		result.setSafeTileSuggestions(safeTileSuggestions);
		try {
			agent.stop();
		} catch (Exception e) {						
		}
		shouldRun = false;
		state = MinesweeperGameState.TERMINATED;
	}

	private void onVictory() {
		result.setSimEndMillis(System.currentTimeMillis());
		result.setResult(MinesweeperResultType.VICTORY);
		result.setSteps(steps);
		result.setSafeTileSuggestions(safeTileSuggestions);
		try {
			agent.victory();
		} catch (Exception e) {
			onAgentException(e);
			return;
		}		
		state = MinesweeperGameState.FINISHED;
		try {
			agent.stop();
		} catch (Exception e) {						
		}
	}
	
	private void onDied() {
		result.setSimEndMillis(System.currentTimeMillis());
		result.setResult(MinesweeperResultType.DEATH);
		result.setSteps(steps);
		result.setSafeTileSuggestions(safeTileSuggestions);
		try {
			agent.victory();
		} catch (Exception e) {
			onAgentException(e);
			return;
		}		
		state = MinesweeperGameState.FINISHED;
		try {
			agent.stop();
		} catch (Exception e) {						
		}
	}

	private void onTimeout() {
		result.setSimEndMillis(System.currentTimeMillis());		
		result.setResult(MinesweeperResultType.TIMEOUT);
		result.setSafeTileSuggestions(safeTileSuggestions);
		try {
			agent.stop();
		} catch (Exception e) {						
		}		
		shouldRun = false;		
		state = MinesweeperGameState.FINISHED;
	}

	private void onAgentException(Exception e) {
		result.setSimEndMillis(System.currentTimeMillis());
		result.setResult(MinesweeperResultType.AGENT_EXCEPTION);
		result.setException(e);
		result.setSafeTileSuggestions(safeTileSuggestions);
		try {
			agent.stop();
		} catch (Exception e2) {						
		}		
		shouldRun = false;
		state = MinesweeperGameState.FAILED;
	}

	@Override
	public MinesweeperGameState getGameState() {
		return state;
	}

	@Override
	public MinesweeperResult getResult() {
		if (state == MinesweeperGameState.INIT || state == MinesweeperGameState.RUNNING) return null;
		return result;
	}

	@Override
	public MinesweeperResult waitFinish() throws InterruptedException {
		switch (state) {
		case INIT:
			return null;
			
		case RUNNING:
			if (gameThread != null && gameThread.isAlive()) this.gameThread.join();
			return getResult();
		
		default:
			return result;
		}
	}
	
}
