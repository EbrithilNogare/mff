package minesweeper4j.game;

public enum ETile {
	
	MINE("M"), 
	FREE("."),
	UNKNOWN("?");
	
	public final String debugChar;
	
	private ETile(String debug) {
		this.debugChar = debug;
	}

}
