package game.actions.oop;

import game.actions.EDirection;
import game.board.oop.Board;

public interface IAction {
	
	public EActionType getType(Board board);
	
	public EDirection getDirection();
	
	public boolean isPossible(Board board);
	
	public boolean perform(Board board);

    public void undo(Board board);
}
