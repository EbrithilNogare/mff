package minesweeper4j.agents;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;

import minesweeper4j.game.Action;
import minesweeper4j.game.Board;
import minesweeper4j.game.Pos;
import minesweeper4j.game.Tile;

/**
 * An agent that auto-computes within {@link #observe(Board)}:
 * 1) positions of unknown tiles,
 * 2) positions of unknown tiles on the "known border", i.e., tiles that has at least one {@link Tile#visible} tile.
 * 
 * It contains a routine for checking whether the board is not solved.
 * 
 * @author Jimmy
 */
public abstract class ArtificialAgent extends ArtificialAgentBase {
	
	/**
	 * To be used for generating random numbers.
	 */
	protected Random random = new Random(System.currentTimeMillis());
	
	/**
	 * Sleep interval between actions...
	 */
	protected int sleepInterleveMillis = 300;
	
	/**
	 * List of tiles that are currently not {@link Tile#visible}.
	 */
	protected List<Pos> unknowns;
	
	/**
	 * List of tiles that are currently not {@link Tile#visible} but they have at least one
     * {@link Tile#visible} neighbor tile.
	 */
	protected List<Pos> borderUnknowns;
	
	/**
	 * List of tiles that are currently {@link Tile#visible}, they have
     * {@link Tile#mines} &gt; 1 and have at least one non-{@link Tile#visible} neighbor tile.
	 */
	protected List<Pos> borderNumbers;
	
	/**
	 * Board the {@link #thinkImpl(Board, Board)} seen previously, null during
     * the first {@link #thinkImpl(Board, Board)}.
	 */
	protected Board previousBoard;
	
	/**
	 * How many tiles are flagged as having a mine.
	 */
	protected int flaggedTileCount;
	
	long thinkTime;
	
	public void setSleepInterval(int ms) { sleepInterleveMillis = ms; }
	
	public long getThinkTime() { return thinkTime; }
	
	@Override
	public void observe(Board board) {
		super.observe(board);
		
		// COMPUTE UNKNOWN & BORDER SPACES
		if (unknowns == null) unknowns = new ArrayList<Pos>(board.width * board.height);
		else unknowns.clear();
		if (borderUnknowns == null) borderUnknowns = new ArrayList<Pos>(board.width * board.height);
		else borderUnknowns.clear();
		if (borderNumbers == null) borderNumbers = new ArrayList<Pos>(board.width * board.height);
		else borderNumbers.clear();
		
		// RESET FLAGGED TILE COUNT
		flaggedTileCount = 0;
		
		// ITERATE OVER TILES AND QUERY THEM
		for (int x = 0; x < board.width; ++x) {
			for (int y = 0; y < board.height; ++y) {
				Tile tile = board.tile(x, y);
				if (!tile.visible) {
					// ADD UKNOWN
					unknowns.add(new Pos(x, y));
					
					// TEST WHETHER IT IS A BORDER TILE
					boolean isBorder = false;
					for (int dX = -1; dX < 2; ++dX) {
						if (isBorder) break;
						for (int dY = -1; dY < 2; ++dY) {
							int tX = tile.tileX + dX;
							int tY = tile.tileY + dY;
							if (tX >= 0 && tX < board.width && tY >= 0 && tY < board.height) {
								Tile nextTile = board.tile(tX, tY);
								if (nextTile.visible) {
									borderUnknowns.add(new Pos(x, y));
									isBorder = true;
									break;
								}
							}
							
						}				
					}	
				} else
				if (tile.mines > 0) {
					// TEST WHETHER IT IS A BORDER TILE
					boolean isBorder = false;
					for (int dX = -1; dX < 2; ++dX) {
						if (isBorder) break;
						for (int dY = -1; dY < 2; ++dY) {
							int tX = tile.tileX + dX;
							int tY = tile.tileY + dY;
							if (tX >= 0 && tX < board.width && tY >= 0 && tY < board.height) {
								Tile nextTile = board.tile(tX, tY);
								if (!nextTile.visible) {
									borderNumbers.add(new Pos(x, y));
									isBorder = true;
									break;
								}
							}
							
						}				
					}	
				} else
				if (tile.flag) {
					flaggedTileCount += 1;
				}
			}
		}
	}

	@Override
	protected final Action think(Board board) {
		if (verbose)
			System.out.println("--- " + getClass().getSimpleName() + " THINK ---");
		
		// SLEEPING
		if (sleepInterleveMillis > 0) {
			try {
				Thread.sleep(sleepInterleveMillis);
			} catch (InterruptedException e) {
				return null;
			}
		}
		
		// BOARD STATE
		if (verbose)
			board.printBoard();
		
		// ALWAYS USE AN ADVICE
		if (board.safeTilePos != null && !board.tile(board.safeTilePos).visible) {
			if (verbose)
				System.out.format("took hint at %d, %d\n", board.safeTilePos.x, board.safeTilePos.y);
			return Action.open(board.safeTilePos);
		}
		
		// CHECK WHETHER THE BOARD IS COMPLETELY SOLVABLE, IF ONLY MINES, FLAG THEM 
		if (unknowns.size() == board.totalMines) {
			if (sleepInterleveMillis > 100) {
				sleepInterleveMillis = 100;
			}
			for (Pos pos : unknowns) {
				if (!board.tile(pos).flag) {
					return Action.flag(pos);
				}
			}
			throw new RuntimeException("Should not reach here; solution invalid? board.totalMines invalid?");
		}
		
		// DO THE THINKING
		
		long start = System.currentTimeMillis();
		
		Action result = thinkImpl(board, previousBoard);
		
		thinkTime += System.currentTimeMillis() - start;
		
		// SAVE BOARD thinkImpl HAS SEEN
		// -- mind the fact, that the next think() iteration may not invoke thinkImpl() 
		//    due to the fact we're auto-using suggestions
		previousBoard = board;
		
		return result;
	}
	
	/**
	 * Think over the 'board' and produce an 'action'; preferably using {@link #actions}.
	 * 
	 * Things already guaranteed:
	 * 1) board is not fully solvable yet
	 * 2) we do not have any new advice; if you want one, issue {@link #actions}.advice().
	 * 
	 * Thinks already computed:
	 * 1) {@link #unknowns}
	 * 2) {@link #borderUnknowns}
	 * 
	 * @param board current state of the board
	 * @param previousBoard a board from previous think, may be null during the first think tick
	 * @return
	 */
	protected abstract Action thinkImpl(Board board, Board previousBoard);
	
}
