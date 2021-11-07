package game.actions.compressed;

import game.actions.EDirection;
import game.actions.oop.EActionType;
import game.board.compressed.BoardCompressed;

public abstract class MAction {
	
	public abstract EActionType getType();
	
	public abstract EDirection getDirection();

	public abstract boolean isPossible(BoardCompressed board);
	
	public abstract void perform(BoardCompressed board);
	
	public abstract void reverse(BoardCompressed board);
	
	/**
	 * If we move 1 step in given 'dir', will we still be at board? 
	 * @param tile
	 * @param dir
	 * @param steps
	 * @return
	 */
	protected boolean onBoard(BoardCompressed board, int tileX, int tileY, EDirection dir) {
		int targetX = tileX + dir.dX;
		if (targetX < 0 || targetX >= board.width()) return false;
		int targetY = tileY + dir.dY;
		if (targetY < 0 || targetY >= board.height()) return false;
		return true;
	}
	
}
