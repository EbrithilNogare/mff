package minesweeper4j.game;

public class Action {

	public EAction type;
	
	public int tileX;
	
	public int tileY;
	
	public Action() {		
	}

	public Action(EAction type) {
		this.type = type;
		this.tileX = -1;
		this.tileY = -1;
	}
	
	public Action(EAction type, int tileX, int tileY) {
		this.type = type;
		this.tileX = tileX;
		this.tileY = tileY;
	}
	
	public boolean isPossible(Board board) {
		if (type == EAction.SUGGEST_SAFE_TILE) return true;
		
		if (tileX < 0) return false;
		if (tileY < 0) return false;
		if (tileX >= board.width) return false;
		if (tileY >= board.height) return false;
		if (type == null) return false;
		
		switch (type) {
		case OPEN:
			return !board.tile(tileX, tileY).visible;
		case FLAG:
			return !board.tile(tileX, tileY).visible && !board.tile(tileX, tileY).flag;
		case UNFLAG:
			return !board.tile(tileX, tileY).visible && board.tile(tileX, tileY).flag;		
		default:
			 throw new RuntimeException("INVALID ACTION TYPE: " + type);
		}
	}
	
	@Override
	public String toString() {
		return "Action[" + type + (type == EAction.SUGGEST_SAFE_TILE ? "" : "|" + tileX + "," + tileY) + "]";
	}

	public static Action open(int tileX, int tileY) {
		return new Action(EAction.OPEN, tileX, tileY);
	}
	
	public static Action flag(int tileX, int tileY) {
		return new Action(EAction.FLAG, tileX, tileY);
	}
	
	public static Action unflag(int tileX, int tileY) {
		return new Action(EAction.UNFLAG, tileX, tileY);
	}
	
	public static Action open(Pos pos) {
		return new Action(EAction.OPEN, pos.x, pos.y);
	}
	
	public static Action flag(Pos pos) {
		return new Action(EAction.FLAG, pos.x, pos.y);
	}
	
	public static Action unflag(Pos pos) {
		return new Action(EAction.UNFLAG, pos.x, pos.y);
	}
	
	public static Action advice() {
		return new Action(EAction.SUGGEST_SAFE_TILE);
	}
	
}
