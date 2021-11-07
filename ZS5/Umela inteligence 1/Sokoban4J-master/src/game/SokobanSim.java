package game;

import game.actions.EDirection;
import game.actions.oop.*;
import game.board.compact.BoardCompact;
import game.board.oop.Board;

public class SokobanSim implements ISokobanGame, Runnable {
	private SokobanConfig config;
	private Board board;
	private IAgent agent;
	
	private Thread gameThread;
	
	// RUNTIME
	
	private SokobanGameState state;
	private IAction agentAction;
	private boolean observe = true;
    private boolean shouldRun = true;
    long startTime;
	
	// RESULT
	
	private SokobanResult result;
	
	private int steps = 0;
	
	public SokobanSim(SokobanConfig config, Board board) {
		this.config = config;
		this.board = board;
		this.agent = config.agent;
		
		this.state = SokobanGameState.INIT;
        
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
	
	@Override
	public void run() {
		try {
			startTime = System.currentTimeMillis();
			
			try {
				agent.newLevel();
			} catch (Exception e) {
				onAgentException(e);
				return;
			}
			
			while (shouldRun && !Thread.interrupted()) {

				// TIMEOUT?
				if (config.timeoutMillis > 0) {
					long now = System.currentTimeMillis();
					long timeLeftMillis = config.timeoutMillis - (now - startTime);
					if (timeLeftMillis <= 0) {						
						onTimeout();
						return;
					}					
				}
				
				// VICTORY?
				if (board.isVictory()) {					
					onVictory();				 
					return;
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
				EDirection whereToMove = agent.act();
				
				if (whereToMove == null) continue;
                
                if (whereToMove == EDirection.NONE) {   // agent gave up
                    stopSimulation(SokobanResultType.AGENT_FAILED, SokobanGameState.FAILED);
                    break;
                }

                agentAction = Move.orPush(board, whereToMove);
	
				// AGENT ACTION VALID?
				if (agentAction.isPossible(board)) {
					// PERFORM THE ACTION
					agentAction.perform(board);
					++steps;
					observe = true;
				} else {
                    System.out.println("Agent returned an illegal move!");
                    stopSimulation(SokobanResultType.AGENT_FAILED, SokobanGameState.FAILED);
				}
				
				agentAction = null;
			}
		} catch (Exception e) {
			onSimulationException(e);
		}
	}

    void stopSimulation(SokobanResultType resultType, SokobanGameState endState) {
		result.setSimTimeMillis(System.currentTimeMillis() - startTime);
		result.setResult(resultType);
		try {
			agent.stop();
		} catch (Exception e) {						
		}
		shouldRun = false;
		state = endState;
    }

	private void onSimulationException(Exception e) {
        result.setException(e);
        stopSimulation(SokobanResultType.SIMULATION_EXCEPTION, SokobanGameState.FAILED);
	}

	private void onTermination() {
        stopSimulation(SokobanResultType.TERMINATED, SokobanGameState.TERMINATED);
	}

	private void onVictory() {
        result.setSimTimeMillis(System.currentTimeMillis() - startTime);

        SokobanResultType outcome = SokobanResultType.VICTORY;
        if (board.minMoves > 0) {
            if (steps < board.minMoves)
                result.message = 
                    "warning: solution in fewer moves than supposedly optimal move count of " +
                    board.minMoves;
            else if (steps > board.minMoves && config.requireOptimal) {
                result.message = String.format(
                    "solution of %d steps exceeded optimal move count of %d",
                    steps, board.minMoves);
                outcome = SokobanResultType.NOT_OPTIMAL;
            }
        } else if (config.requireOptimal)
            result.message = "warning: optimal move count is unknown";

        result.setResult(outcome);
        result.setSteps(steps);
        
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

	private void onTimeout() {
        stopSimulation(SokobanResultType.TIMEOUT, SokobanGameState.FINISHED);
	}

	private void onAgentException(Exception e) {
        result.setException(e);
        stopSimulation(SokobanResultType.AGENT_EXCEPTION, SokobanGameState.FAILED);
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
