package game.actions.compact;

import game.actions.EDirection;
import game.actions.oop.EActionType;
import game.board.compact.BoardCompact;

/**
 * Combines {@link CWalk} and {@link CPush} in the single macro action.
 * @author Jimmy
 */
public class CWalkPush extends CAction {

	private CWalk walk;
	private CPush push;

	public CWalkPush(CWalk teleport, CPush push) {
		this.walk = teleport;
		this.push = push;
	}
	
	@Override
	public EActionType getType() {
		return EActionType.WALK_AND_PUSH;
	}

	public EDirection getPushDirection() {
		return push.getDirection();
	}
	
	@Override
	public EDirection getDirection() {
		return walk.getDirection();
	}

	@Override
	public EDirection[] getDirections() {
		EDirection[] result = new EDirection[walk.getDirections().length+1];
		
		for (int i = 0; i < walk.getDirections().length; ++i) {
			result[i] = walk.getDirections()[i];
		}
		result[result.length-1] = push.getDirection();
		
		return result;
	}
	
	/**
	 * How many steps do you need in order to perform the walk+push;
	 *  defined only if directions are provided for {@link CWalk} during construction using {@link CWalk#CWalk(int, int, EDirection[])}.
	 * @return
	 */	
	public int getSteps() {
		return 1 + walk.getSteps();
	}

	@Override
	public boolean isPossible(BoardCompact board) {
		return walk.isPossible(board);
	}

	@Override
	public void perform(BoardCompact board) {
		if (walk.isPossible(board)) {
			walk.perform(board);
		} else {
			throw new RuntimeException("Walk action part not possible!");
		}
		if (push.isPossible(board)) {
			push.perform(board);
		} else {
			throw new RuntimeException("Cannot push after the walk!");
		}		
	}

	@Override
	public void reverse(BoardCompact board) {
		push.reverse(board);
		walk.reverse(board);
	}

	@Override
	public String toString() {
		return "CWalkPush[\n  " + walk + "\n  " + push + "\n]";
	}

}
