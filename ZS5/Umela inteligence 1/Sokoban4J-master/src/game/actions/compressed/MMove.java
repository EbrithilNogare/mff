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
 * MOVE ONLY, if there is a box, an edge or no free space, then the action is considered "not possible".
 * @author Jimmy
 */
public class MMove extends MAction {
	
	private static Map<EDirection, MMove> actions = new HashMap<EDirection, MMove>();
	
	static {
		actions.put(EDirection.DOWN, new MMove(EDirection.DOWN));
		actions.put(EDirection.UP, new MMove(EDirection.UP));
		actions.put(EDirection.LEFT, new MMove(EDirection.LEFT));
		actions.put(EDirection.RIGHT, new MMove(EDirection.RIGHT));
	}
	
	public static Collection<MMove> getActions() {
		return actions.values();
	}
	
	public static MMove getAction(EDirection direction) {
		return actions.get(direction);
	}
	
	private EDirection dir;
	
	public MMove(EDirection dir) {
		this.dir = dir;
	}
	
	@Override
	public EActionType getType() {
		return EActionType.MOVE;
	}

	@Override
	public EDirection getDirection() {
		return dir;
	}
	
	@Override
	public boolean isPossible(BoardCompressed board) {
		// PLAYER ON THE EDGE
		if (!onBoard(board, board.playerX, board.playerY, dir)) return false;
		
		SubSlimTile subSlimTile = MTile.getSubSlimTile(board.playerX+dir.dX, board.playerY+dir.dY);
		
		// TILE TO THE DIR IS FREE
		if (MTile.isFree(subSlimTile, board.tile(board.playerX+dir.dX, board.playerY+dir.dY))) return true;
				
		// TILE WE WISH TO MOVE TO IS NOT FREE
		return false;
	}
		
	/**
	 * PERFORM THE MOVE, no validation, call {@link #isPossible(BoardCompact, EDirection)} first!
	 * @param board
	 * @param dir
	 */
	@Override
	public void perform(BoardCompressed board) {
		// MOVE THE PLAYER
		board.movePlayer(board.playerX, board.playerY, board.playerX + dir.dX, board.playerY + dir.dY);
	}
	
	/**
	 * REVERSE THE MOVE PRVIOUSLY DONE BY {@link #perform(BoardCompact, EDirection)}, no validation.
	 * @param board
	 * @param dir
	 */
	@Override
	public void reverse(BoardCompressed board) {
		// REVERSE THE PLAYER
		board.movePlayer(board.playerX, board.playerY, board.playerX - dir.dX, board.playerY - dir.dY);
	}
	
	@Override
	public String toString() {
		return "MMove[" + dir.toString() + "]";
	}

}
