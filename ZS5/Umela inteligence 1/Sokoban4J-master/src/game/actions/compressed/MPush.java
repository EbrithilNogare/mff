package game.actions.compressed;

import java.util.Collection;
import java.util.HashMap;
import java.util.Map;

import game.actions.EDirection;
import game.actions.oop.EActionType;
import game.board.compact.BoardCompact;
import game.board.compressed.BoardCompressed;
import game.board.compressed.MTile;
import game.board.compressed.MTile.SubSlimTile;

/**
 * PUSH ONLY. If the player is not next to the box or there is nowhere to push the box, than the action is considered as not possible.
 * @author Jimmy
 */
public class MPush extends MAction {
	
	private static Map<EDirection, MPush> actions = new HashMap<EDirection, MPush>();
	
	static {
		actions.put(EDirection.DOWN, new MPush(EDirection.DOWN));
		actions.put(EDirection.UP, new MPush(EDirection.UP));
		actions.put(EDirection.LEFT, new MPush(EDirection.LEFT));
		actions.put(EDirection.RIGHT, new MPush(EDirection.RIGHT));
	}
	
	public static Collection<MPush> getActions() {
		return actions.values();
	}
	
	public static MPush getAction(EDirection direction) {
		return actions.get(direction);
	}
	
	private EDirection dir;
	
	public MPush(EDirection dir) {
		this.dir = dir;
	}
	
	@Override
	public EActionType getType() {
		return EActionType.PUSH;
	}

	@Override
	public EDirection getDirection() {
		return dir;
	}
	
	@Override
	public boolean isPossible(BoardCompressed board) {
		// PLAYER ON THE EDGE
		if (!onBoard(board, board.playerX, board.playerY, dir)) return false;
		
		SubSlimTile subSlimTile1 = MTile.getSubSlimTile(board.playerX+dir.dX, board.playerY+dir.dY);
		
		// TILE TO THE DIR IS NOT BOX
		if (!MTile.isBox(subSlimTile1, board.tile(board.playerX+dir.dX, board.playerY+dir.dY))) return false;
		
		// BOX IS ON THE EDGE IN THE GIVEN DIR
		if (!onBoard(board, board.playerX+dir.dX, board.playerY+dir.dY, dir)) return false;
		
		// TILE TO THE DIR OF THE BOX IS NOT FREE
		SubSlimTile subSlimTile2 = MTile.getSubSlimTile(board.playerX+dir.dX+dir.dX, board.playerY+dir.dY+dir.dY);
		if (!MTile.isFree(subSlimTile2, board.tile(board.playerX+dir.dX+dir.dX, board.playerY+dir.dY+dir.dY))) return false;
				
		// YEP, WE CAN PUSH
		return true;
	}
	
	/**
	 * PERFORM THE PUSH, no validation, call {@link #isPossible(BoardCompact, EDirection)} first!
	 * @param board
	 * @param dir
	 */
	@Override
	public void perform(BoardCompressed board) {
		// MOVE THE BOX
		board.moveBox(board.playerX + dir.dX, board.playerY + dir.dY, board.playerX + dir.dX + dir.dX, board.playerY + dir.dY + dir.dY);
		// MOVE THE PLAYER
		board.movePlayer(board.playerX, board.playerY, board.playerX + dir.dX, board.playerY + dir.dY);
	}
	
	/**
	 * REVERSE THE ACTION PREVIOUSLY DONE BY {@link #perform(BoardCompact, EDirection)}, no validation.
	 * @param board
	 * @param dir
	 */
	@Override
	public void reverse(BoardCompressed board) {
		// MARK PLAYER POSITION
		int playerX = board.playerX;
		int playerY = board.playerY;
		// MOVE THE PLAYER
		board.movePlayer(board.playerX, board.playerY, board.playerX - dir.dX, board.playerY - dir.dY);
		// MOVE THE BOX
		board.moveBox(playerX + dir.dX, playerY + dir.dY, playerX, playerY);
	}
	
	@Override
	public String toString() {
		return "MPush[" + dir.toString() + "]";
	}

}
