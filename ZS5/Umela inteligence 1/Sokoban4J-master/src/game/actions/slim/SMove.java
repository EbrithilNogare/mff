package game.actions.slim;

import java.util.Collection;
import java.util.HashMap;
import java.util.Map;

import game.actions.EDirection;
import game.actions.oop.EActionType;
import game.board.compact.BoardCompact;
import game.board.slim.BoardSlim;
import game.board.slim.STile;

/**
 * MOVE ONLY, if there is a box, an edge or no free space, then the action is considered "not possible".
 * @author Jimmy
 */
public class SMove extends SAction {
	
	private static Map<EDirection, SMove> actions = new HashMap<EDirection, SMove>();
	
	static {
		actions.put(EDirection.DOWN, new SMove(EDirection.DOWN));
		actions.put(EDirection.UP, new SMove(EDirection.UP));
		actions.put(EDirection.LEFT, new SMove(EDirection.LEFT));
		actions.put(EDirection.RIGHT, new SMove(EDirection.RIGHT));
	}
	
	public static Collection<SMove> getActions() {
		return actions.values();
	}
	
	public static SMove getAction(EDirection direction) {
		return actions.get(direction);
	}
	
	private EDirection dir;
	
	public SMove(EDirection dir) {
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
	public boolean isPossible(BoardSlim board) {
		// PLAYER ON THE EDGE
		if (!onBoard(board, board.playerX, board.playerY, dir)) return false;
		
		// TILE TO THE DIR IS FREE
		if (STile.isFree(board.tile(board.playerX+dir.dX, board.playerY+dir.dY))) return true;
				
		// TILE WE WISH TO MOVE TO IS NOT FREE
		return false;
	}
		
	/**
	 * PERFORM THE MOVE, no validation, call {@link #isPossible(BoardCompact, EDirection)} first!
	 * @param board
	 * @param dir
	 */
	@Override
	public void perform(BoardSlim board) {
		// MOVE THE PLAYER
		board.movePlayer(board.playerX, board.playerY, (byte)(board.playerX + dir.dX), (byte)(board.playerY + dir.dY));
	}
	
	/**
	 * REVERSE THE MOVE PRVIOUSLY DONE BY {@link #perform(BoardCompact, EDirection)}, no validation.
	 * @param board
	 * @param dir
	 */
	@Override
	public void reverse(BoardSlim board) {
		// REVERSE THE PLAYER
		board.movePlayer(board.playerX, board.playerY, (byte)(board.playerX - dir.dX), (byte)(board.playerY - dir.dY));
	}
	
	@Override
	public String toString() {
		return "SMove[" + dir.toString() + "]";
	}

}
