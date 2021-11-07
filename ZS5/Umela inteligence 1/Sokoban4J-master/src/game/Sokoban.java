package game;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import javax.swing.SwingUtilities;

import agents.HumanAgent;
import game.board.oop.Board;
import ui.SokobanFrame;
import ui.SokobanView;
import ui.UIBoard;
import ui.atlas.SpriteAtlas;

public class Sokoban {
	
	private List<SokobanResult> results = new ArrayList<SokobanResult>();
	
	private SokobanConfig config;
	
	private SpriteAtlas sprites;	
	private Board board;
	private UIBoard uiBoard;
	private SokobanView view;
	private SokobanFrame frame;	
	private ISokobanGame game;
	
	/**
	 * Resets everything except {@link #sprites} and {@link #results}.
	 */
	public void reset() {
		if (game != null) {
			game.stopGame();
			game = null;
		}
		if (frame != null) {
			final SokobanFrame frameToDispose = frame; 
				SwingUtilities.invokeLater(new Runnable() {
					@Override
					public void run() {
						frameToDispose.setVisible(false);
						frameToDispose.dispose();									
					}
				}
			);
			frame = null;
		}
		view = null;
		uiBoard = null;
		board = null;
		config = null;
	}
	
	/**
	 * Current game that is running if any.
	 */
	public ISokobanGame getGame() {
		return game;
	}
	
	/**
	 * Results this instance have aggregated so far.
	 */
	public List<SokobanResult> getResults() {
		return results;
	}
	
	/**
	 * Returns first result from {@link #getResults()} if any.
	 */
	public SokobanResult getResult() {
		if (results == null || results.size() == 0) return null;
		return results.get(0);
	}
	
	/**
	 * Returns last result from {@link #getResults()} if any.
	 */
	public SokobanResult getLastResult() {
		if (results == null || results.size() == 0) return null;
		return results.get(results.size()-1);
	}
	
	private void validateConfig() {
        if (config == null)
            throw new RuntimeException("Config is null! Have you forget to setConfig()?");
		config.validate();
	}
	
	private void setConfig(SokobanConfig config) {
		if (this.config != null) {
			throw new RuntimeException("Config already set! You have to reset() first!");
		}
		this.config = config;
	}
	
	private SpriteAtlas initSprites() {
		if (sprites != null) return sprites;
		SpriteAtlas result = new SpriteAtlas();
		result.load();
		return sprites = result;
	}
	
	private Board initBoard() {
		if (board != null) return board;
		// PREREQ
		validateConfig();		
		// IMPL
		Board result = null;
		switch (config.levelFormat) {
		case S4JL: result = Board.fromFileS4JL(config.levelFile, config.levelNumber); break;
		case SOK: result = Board.fromFileSok(config.levelFile, config.levelNumber); break;
		}
		result.validate();
		return board = result;
	}
	
	private UIBoard initUIBoard() {
		if (uiBoard != null) return uiBoard;
		// PREREQ
		initSprites();
		initBoard();
		// IMPL
		UIBoard result = new UIBoard(sprites);
		result.init(board);
		return uiBoard = result;
	}
	
	private SokobanView initView() {		
		if (view != null) return view;
		// PREREQ
		initUIBoard();
		// IMPL
		return view = new SokobanView(board, sprites, uiBoard);
	}
	
	private SokobanFrame initFrame() {
		if (frame != null) return frame;
		// PREREQ
		initView();
		// IMPL
		return frame = new SokobanFrame(view, board.level);
	}
	
	/**
	 * Runs SOKOBAN according to the config.
	 * 
	 * Result of the game or games is going to be stored within {@link #getResults()}.
	 * Use {@link #getResults()}, {@link #getResult()} and {@link #getLastResult()} to obtain it/them.
	 * 
	 */
	public void run(SokobanConfig config) {
		if (game != null) {
			throw new RuntimeException(
                "Cannot run game as the game instance already exists; did you forget to reset()?");
		}
		
		setConfig(config);
		validateConfig();
		
		if (config.levelNumber == 0) {
			runFile();
		} else {
			runLevel();			
		}
	}
	
	private void runFile() {
		// RUN ALL LEVELS WITHIN ONE FILE
		if (config.levelNumber >= 1) {
			// run particular level
			runLevel();				
		} else {
			// run all levels
			SokobanConfig config = this.config;
			int levelNumber = 1;
			try {
				while (true) {
					// RESET INSTANCE (does not reset this.results)
					reset();
					// WAIT A BIT BETWEEN GAMES
					try {
						Thread.sleep(50);
					} catch (InterruptedException e) {
						throw new RuntimeException("Interrupted on Thread.sleep(100) in between levels.");
					}
					if (levelNumber != 1 && (getLastResult() == null || getLastResult().getResult() != SokobanResultType.VICTORY)) {
						// AGENT FAILED TO PASS THE LEVEL...
						break;
					}
					// INIT CONFIG					
					setConfig(config);					
					config.levelNumber = levelNumber;
					// TRY TO LOAD THE BOARD
					try {
						initBoard();
					} catch (Exception e) {
						// FAILED TO LOAD THE LEVEL => end of file hopefully
						break;
					}
					runLevel();				
					++levelNumber;
				}
			} finally {
				config.levelNumber = 0;
			}
			
		}
	}
		
	private void runLevel() {
		if (config.visualization) {
			runVisualization();
		} else {
			runSimulation();
		}
	}

	private void runSimulation() {
		// PREREQS
		validateConfig();
        Board board = initBoard();
        if (config.verbose)
            board.debugPrint();
		
		// START GAME W/O VISUALIZATION
		runGame(new SokobanSim(config, board));		
	}
	
	private void runVisualization() {
		// PREREQS
		validateConfig();
		initFrame();
		
		// OPEN FRAME
		view.renderLater();
		frame.setVisible(true);
		
		// START GAME WITH VISUALIZATION
		runGame(new SokobanVis(config, board, config.agent, sprites, uiBoard, view, frame));
	}
	
	private void runGame(ISokobanGame game) {
		this.game = game;
		game.startGame();
		try {
			game.waitFinish();
		} catch (InterruptedException e) {
			throw new RuntimeException("Interrupted on game.waitFinish()");
		}
		if (game.getResult() != null) {
			SokobanResult result = game.getResult();
			results.add(result);
			if (result.getResult() == SokobanResultType.SIMULATION_EXCEPTION) {
				throw new RuntimeException("Game failed.", result.getException());
			}
		}	
	}
	
	// ================================
	// STATIC METHODS FOR EASY START-UP 
	// ================================
	
    public static File findFile(String path) {
		String[] dirs = { ".", "levels" };

		for (String d : dirs) {
			File f = new File(d, path);
			if (f.exists())
				return f;
		}

		return new File(path);
	}

	private static ELevelFormat determineLevelFormat(String pathToFile) {
		ELevelFormat levelFormat = ELevelFormat.getExpectedLevelFormat(new File(pathToFile));
		if (levelFormat == null) {
			throw new RuntimeException("Could not determine ELevelFormat for: " + pathToFile);
		}
		return levelFormat;
	}
	
	private static String determineId(IAgent agent) {
		return agent == null ? "NULL" : agent.getClass().getSimpleName();
	}
	
	// ----------------
	// GENERIC STARTUPS
	// ----------------

	/**
	 * Runs Sokoban game according to the 'config'; method assumes the configuration is going to play single level only.
	 */
	public static SokobanResult runAgentLevel(SokobanConfig config) {
		Sokoban sokoban = new Sokoban();
		sokoban.run(config);
		return sokoban.getResult();
	}
	
	/**
	 * Runs Sokoban game according to the 'config'; method assumes the configuration is going to play one or more levels.
	 * If there are multiple levels to be played, the run will stop when agent fails to solve the level.
	 */
	public static List<SokobanResult> runAgentLevels(SokobanConfig config) {
		Sokoban sokoban = new Sokoban();
		sokoban.run(config);
		return sokoban.getResults();
	}
	
	// --------------------
	// HEADLESS SIMULATIONS
	// --------------------
	
	/**
	 * 'agent' will play (headless == simulation only) 'levelNumber' (1-based) level from file on 'levelFilePath' assuming 'levelFormat'.
	 * An agent will be given 'timeoutMillis' time to solve the level.
	 * 
	 * @param id id to be given to the config; may be null
	 * @param levelFilePath file to load the level from
	 * @param levelFormat expected format of the file (if it is null, it will be auto-determined)
	 * @param levelNumber 1-based; a level to be played
	 * @param timeoutMillis time given to the agent to solve every level; non-positive number == no timeout
	 * @param agent
	 * @return
	 */
	public static SokobanResult simAgentLevel(
        String id, String levelFilePath, int levelNumber,
        int timeoutMillis, IAgent agent, boolean verbose, boolean optimal) {

		// CREATE CONFIG
		SokobanConfig config = new SokobanConfig();
		if (id == null) id = determineId(agent);
		config.id = id;
		config.agent = agent;
		config.levelFile = findFile(levelFilePath);
		if (!config.levelFile.exists() || !config.levelFile.isFile())
            throw new RuntimeException("Not a level file at '" + config.levelFile.getAbsolutePath() +
                                       "'\nResolved from: " + levelFilePath);
        config.levelFormat = determineLevelFormat(config.levelFile.getName());
		config.levelNumber = levelNumber;		
		config.visualization = false;
        config.timeoutMillis = timeoutMillis;
        config.verbose = verbose;
        config.requireOptimal = optimal;
		
		return runAgentLevel(config);
	}
	
	// ----------------------
	// VISUALIZED SIMULATIONS
	// ----------------------
	
	/**
	 * 'agent' will play (visualized) 'levelNumber' (1-based) level from file on 'levelFilePath' assuming 'levelFormat'.
	 * An agent will be given 'timeoutMillis' time to solve the level.
	 * 
	 * @param id id to be given to the config; may be null
	 * @param levelFilePath file to load the level from
	 * @param levelFormat expected format of the file (if it is null, it will be auto-determined)
	 * @param levelNumber 1-based; a level to be played
	 * @param timeoutMillis time given to the agent to solve every level; non-positive number == no timeout
	 * @param agent
	 * @return
	 */
	public static SokobanResult playAgentLevel(String id, String levelFilePath, ELevelFormat levelFormat, int levelNumber, int timeoutMillis, IAgent agent) {
		// CREATE CONFIG
		SokobanConfig config = new SokobanConfig();
		if (id == null) id = determineId(agent);
		config.id = id;
		config.agent = agent;
		config.levelFile = findFile(levelFilePath);
		if (!config.levelFile.exists() || !config.levelFile.isFile())
			throw new RuntimeException("Not a level file at '" + config.levelFile.getAbsolutePath() + "'\nResolved from: " + levelFilePath);
		if (levelFormat == null) levelFormat = determineLevelFormat(config.levelFile.getName());
		config.levelFormat = levelFormat;
		config.levelNumber = levelNumber;		
		config.visualization = true;
		config.timeoutMillis = timeoutMillis;
		
		return runAgentLevel(config);
	}
	
	/**
	 * 'agent' will play (visualized) 'levelNumber' (1-based) level from file on 'levelFilePath'.
	 * An agent will be given 'timeoutMillis' time to solve the level.
	 * 
	 * @param id id to be given to the config; may be null
	 * @param levelFilePath file to load the level from
	 * @param levelNumber 1-based; a level to be played
	 * @param timeoutMillis time given to the agent to solve every level; non-positive number == no timeout
	 * @param agent
	 * @return
	 */
	public static SokobanResult playAgentLevel(String id, String levelFilePath, int levelNumber, int timeoutMillis, IAgent agent) {
		return playAgentLevel(id, levelFilePath, null, levelNumber, timeoutMillis, agent);
	}
	
	/**
	 * 'agent' will play (visualized) all levels from file on 'levelFilePath' assuming 'levelFormat'.
	 * An agent will be given 'timeoutMillis' time to solve every level.
	 * The run will stop on the level the agent fail to solve.
	 * 
	 * @param id id to be given to the config; may be null
	 * @param levelFilePath file to load the level from
	 * @param levelFormat expected format of the file; if it is null, it will be auto-determined using {@link ELevelFormat#getExpectedLevelFormat(File)}
	 * @param levelNumber 1-based; a level to be played
	 * @param timeoutMillis time given to the agent to solve every level; non-positive number == no timeout
	 * @param agent
	 * @return
	 */
	public static List<SokobanResult> playAgentFile(String id, String levelFilePath, ELevelFormat levelFormat, int timeoutMillis, IAgent agent) {
		// CREATE CONFIG
		SokobanConfig config = new SokobanConfig();
		if (id == null) id = determineId(agent);
		config.id = id;
		config.agent = agent;
		config.levelFile = findFile(levelFilePath);
		if (!config.levelFile.exists() || !config.levelFile.isFile())
			throw new RuntimeException("Not a level file at '" + config.levelFile.getAbsolutePath() + "'\nResolved from: " + levelFilePath);
		if (levelFormat == null) levelFormat = determineLevelFormat(config.levelFile.getName());
		config.levelFormat = levelFormat;
		config.visualization = true;
		config.timeoutMillis = timeoutMillis;
		
		return runAgentLevels(config);
	}
	
	/**
	 * 'agent' will play (visualized) the given level number from the file on 'levelFilePath'.
	 * 
	 * @param levelFilePath file to load
	 * @param levelNumber
	 * @param agent
	 * @return
	 */
	public static SokobanResult playAgentLevel(String levelFilePath, int levelNumber, IAgent agent) {
		return playAgentLevel(null, levelFilePath, null, levelNumber, -1, agent);
	}
	
	/**
	 * 'agent' will play (visualized) FIRST level from file on 'levelFilePath'.
	 * 
	 * @param levelFilePath file to load
	 * @param agent
	 * @return
	 */
	public static SokobanResult playAgentLevel(String levelFilePath, IAgent agent) {
		return playAgentLevel(levelFilePath, 1, agent);
	}
	
	/**
	 * 'agent' will play (visualized) all levels from file on 'levelFilePath'.
	 * The run will stop on the level the agent fail to solve.
	 * 
	 * @param levelFilePath file to load
	 * @param agent
	 * @return
	 */
	public static List<SokobanResult> playAgentFile(String levelFilePath, IAgent agent) {
		return playAgentFile(null, levelFilePath, null, -1, agent);
	}
	
	// ----------------------
	// HUMAN PLAYING THE GAME
	// ----------------------
	
	/**
	 * Human will play 'levelNumber' found within the file on 'levelFilePath'.
	 * 
	 * @param levelFilePath path to the level file to load
	 * @param levelNumber level number to be played; 1-based
	 * @return
	 */
	public static void playHumanLevel(String levelFilePath, int levelNumber) {
		playAgentLevel(levelFilePath, levelNumber, new HumanAgent());
	}
	
	/**
	 * Human will play all levels found within the file on 'levelFilePath'.
	 * @param levelFilePath path to the level file to load
	 * @return
	 */
	public static void playHumanFile(String levelFilePath) {
		playAgentFile(levelFilePath, new HumanAgent());
	}
}
