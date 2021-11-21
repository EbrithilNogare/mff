package minesweeper4j.game;

public class Tile implements Cloneable {

	/**
	 * What type this tile is? See {@link ETile}.
	 */
	public ETile type;
	
	/**
	 * How many mines are around? For an agent / a player, this is valid only iff the tile is {@link #visible}.
	 */
	public Integer mines = null;
	
	/**
	 * Is this tile visible to an agent / a player?
	 */
	public boolean visible = false;
	
	/**
	 * Was this tile flagged by an agent / a player?
	 */
	public boolean flag = false;
	
	/**
	 * Tile's X coordinate; 0-based.
	 */
	public int tileX;
	
	/**
	 * Tile's Y coordinate; 0-based.
	 */
	public int tileY;

	public Tile() {
	}
	
	public Tile(ETile type) {
		this.type = type;
	}
	
	@Override
	public Tile clone() {
		Tile result = new Tile();
		
		result.type = type;
		result.visible = visible;
		result.flag = flag;
		result.mines = mines;
		
		result.tileX = tileX;
		result.tileY = tileY;
		
		return result;
	}
	
	@Override
	public String toString() {
		return "Tile[" + tileX + "," + tileY + "|type= " + type + ", visible=" + visible + ", flag=" + flag + ", mines=" + mines +"]";
	}
	
}
