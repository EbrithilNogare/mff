package minesweeper4j.game;

import java.util.*;

/**
 * Minesweeper board as used by the simulator.
 * 
 * @author Jimmy
 */
public class Board implements Cloneable {
	
	public int width;
	public int height;
	
	public Tile[][] tiles;
	
	public int totalMines;
	
	/**
	 * Whether a "mine" has been {@link #uncoverTile(int, int)}ed and thus a player has died.
	 */
	public boolean boom = false;
	
	/**
	 * Name of the level being run, not available to {@link IAgent}s.
	 */
	public String level;
	
	/**
	 * Last suggested safe tile; guaranteed to be unique; may be null if no suggestion is possible.
	 * If you issue {@link EAction#SUGGEST_SAFE_TILE} action, then in the next {@link IAgent#observe(Board)}, you will
	 * have a unique (== useful) suggestion here or null if no suggestion is possible. 
	 */
	public Pos safeTilePos;
	
	protected Board() {
		
	}
	
	public Board(int width, int height) {
		this.width = width;
		this.height = height;
		tiles = new Tile[width][height];
		for (int x = 0; x < width; ++x) {
			for (int y = 0; y < height; ++y) {
				Tile tile = new Tile(ETile.FREE);
				tile.mines = 0;
				tile.tileX = x;
				tile.tileY = y;
				
				tiles[x][y] = tile;
			}			
		}
	}
	
	@Override
	public Board clone() {
		Board result = new Board();
		
		result.width = width;
		result.height = height;
		result.tiles = new Tile[width][height];
		result.totalMines = totalMines;
		result.boom = boom;
		result.level = level;
		result.safeTilePos = safeTilePos;
		
		for (int x = 0; x < width; ++x) {
			for (int y = 0; y < height; ++y) {
				result.tiles[x][y] = tiles[x][y].clone();
			}
		}
		
		return result;
	}
		
	/**
	 * Returns tile on [X;Y] ... 0 &lt;= x &lt; {@link #width}, 0 &lt;= y &lt; {@link #height}. 
	 * @param x 0 &lt;= x &lt; {@link #width}
	 * @param y 0 &lt;= y &lt; {@link #height}
	 * @return
	 */
	public Tile tile(int x, int y) {
		return tiles[x][y];
	}
	
	/**
	 * Returns tile on [{@link Pos#x}, {@link Pos#y}].
	 * @param pos
	 * @return
	 */
	public Tile tile(Pos pos) {
		return tiles[pos.x][pos.y];
	}
	
	/**
	 * Returns an agent view of the board, non-{@link Tile#visible} tiles are made {@link ETile#UNKNOWN} and its {@link Tile#mines} are
	 * nullified.
	 * 
	 * @return
	 */
	public Board getAgentView() {
		Board result = clone();
		
		result.level = null;
		
		for (int x = 0; x < width; ++x) {
			for (int y = 0; y < height; ++y) {
				if (!tiles[x][y].visible) {
					result.tiles[x][y].type = ETile.UNKNOWN;
					result.tiles[x][y].mines = null;
				}				
			}
		}
		
		return result;
	}
	
	/**
	 * Is this board solved?
	 * @return
	 */
	public boolean isVictory() {
		int visibles = 0;
		int flags = 0;
		
		for (int x = 0; x < width; ++x) {
			for (int y = 0; y < height; ++y) {
				if (tiles[x][y].visible) ++visibles;
				else
				if (tiles[x][y].flag) ++flags;
			}
		}
		
		// all tiles either visible or have a mine and are flagged as a mine...
		return width * height - totalMines == visibles && flags == totalMines;
	}
	
	// ===============
	// BOARD MINE INIT
	// ===============
	
	public void placeRandomMines(Random random, int mines) {
		List<Pos> allPos = new ArrayList<Pos>();
		
		for (int x = 0; x < width; ++x) {
			for (int y = 0; y < height; ++y) {
				if (tiles[x][y].type == ETile.FREE) {
					allPos.add(new Pos(x, y));
				}
			}
		}
		
		while (allPos.size() > 0 && mines > 0) {
			--mines;
			Pos pos = allPos.remove(random.nextInt(allPos.size()));
			placeMine(pos.x, pos.y);
		}		
	}
	
	public void placeMine(int x, int y) {
		if (tiles[x][y].type == ETile.MINE) return;
		++totalMines;
		tiles[x][y].type = ETile.MINE;
		for (int dX = -1; dX < 2; ++dX) {
			for (int dY = -1; dY < 2; ++dY) {
				int tX = x + dX;
				int tY = y + dY;
				if (tX >= 0 && tX < width && tY >= 0 && tY < height) {
					Tile tile = tiles[tX][tY];
					if (tile.type == ETile.FREE) {
						if (tile.mines == null) tile.mines = 1;
						else tile.mines += 1;
					}
				}
			}				
		}		
	}
	
	// =======
	// ACTIONS
	// =======
	
	public void uncoverTile(int x, int y) {
		if (tiles[x][y].visible) return;
		tiles[x][y].visible = true;
		
		if (tiles[x][y].type == ETile.MINE) {
			// BOOOM
			boom = true;
			return;
		}
		
		if (tiles[x][y].mines > 0) {
			// we're done...
			return;
		}
		
		// FLOOD-FILL
		Set<Tile> expand = new HashSet<Tile>();
		expand.add(tiles[x][y]);
		Set<Tile> done = new HashSet<Tile>();
		
		while (!expand.isEmpty()) {
			Tile tile = expand.iterator().next();
			expand.remove(tile);
			done.add(tile);
			
			tile.visible = true;
			
			if (tile.mines > 0) {
				// stop here
				continue;
			}
			
			for (int dX = -1; dX < 2; ++dX) {
				for (int dY = -1; dY < 2; ++dY) {
					int tX = tile.tileX + dX;
					int tY = tile.tileY + dY;
					if (tX >= 0 && tX < width && tY >= 0 && tY < height) {
						Tile nextTile = tiles[tX][tY];
						if (done.contains(nextTile) || expand.contains(nextTile)) continue;
						expand.add(nextTile);
					}
					
				}				
			}		
		}		
	}
	
	public void flagTile(int x, int y) {
		if (tiles[x][y].visible) return;
		tiles[x][y].flag = true;
	}
	
	public void unflagTile(int x, int y) {
		if (tiles[x][y].visible) return;
		tiles[x][y].flag = false;		
	}

	/**
	 * List of possible safe-tiles with 0-mines around; not cloned!
	 */
	private List<Pos> safeTiles0;
	
	/**
	 * List of possible safe-tiles with 1+mines around; not cloned!
	 */
	private List<Pos> safeTilesNum;
	
	/**
	 * Only valid for Simulation-side board! I.e., calling this from {@link IAgent} is not fruitful!
	 * @param random
	 * @return
	 */
	public Pos suggestSafeTile(Random random) {
		if (safeTiles0 == null) {
			safeTiles0 = new ArrayList<Pos>();
			safeTilesNum = new ArrayList<Pos>();
			for (int x = 0; x < width; ++x) {
				for (int y = 0; y < height; ++y) {
					if (tiles[x][y].type == ETile.FREE) {
						if (tiles[x][y].mines == 0) {
							safeTiles0.add(new Pos(x,y));
						} else
						if (tiles[x][y].mines > 0) {
							safeTilesNum.add(new Pos(x,y));
						}
					}
					
				}
			}
		}
		
		while (safeTiles0.size() > 0) {
			int index = random.nextInt(safeTiles0.size());
			Pos pos = safeTiles0.remove(index);
			if (tiles[pos.x][pos.y].visible) continue;
			safeTilePos = pos;
			return pos;
		}
		
		while (safeTilesNum.size() > 0) {
			int index = random.nextInt(safeTilesNum.size());
			Pos pos = safeTilesNum.remove(index);
			if (tiles[pos.x][pos.y].visible) continue;
			safeTilePos = pos;
			return pos;
		}

		safeTilePos = null;
		return null;		
	}
	
	/**
	 * Debug-print of the board into {@link System#out}.
	 * @param board
	 */
	public void printBoard() {
		for (int y = 0; y < height; ++y) {
			for (int x = 0; x < width; ++x) {
				Tile tile = tile(x, y);
				if (tile.visible) {					
					System.out.print(tile.mines);
				} else 
				if (tile.flag) {
					System.out.print("F");
				} else {
					System.out.print(tile.type.debugChar);
				}							
			}
			System.out.println();
		}
	}

}
