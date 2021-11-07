package game.actions.oop;

import game.actions.EDirection;
import game.board.oop.*;
import game.board.oop.entities.Entity;

public class Move implements IAction {
    EDirection dir;

    public Move(EDirection dir) { this.dir = dir; }

    public static IAction orPush(Board board, EDirection dir) {
        IAction action = new Move(dir);
        return action.isPossible(board) ? action : new Push(dir);
    }

    @Override
    public EActionType getType(Board board) {
        return EActionType.MOVE;
    }

    @Override
    public EDirection getDirection() {
        return dir;
    }

    @Override
    public boolean isPossible(Board board) {
		Tile playerTile = board.player.getTile();
		
		// NO PLAYER TILE
		if (playerTile == null) return false;
		
		// PLAYER ON THE EDGE
        if (!board.isPosValid(playerTile.tileX + dir.dX, playerTile.tileY + dir.dY))
            return false;
		
		// TILE TO THE DIR IS FREE
		return board.tile(playerTile.tileX+dir.dX, playerTile.tileY+dir.dY).isFree();
    }

    @Override
    public boolean perform(Board board) {
		if (!isPossible(board)) return false;
		
		Tile playerTile = board.player.getTile();
		
		// MOVE THE PLAYER
		Entity player = board.player;
		board.move(player, playerTile.tileX+dir.dX, playerTile.tileY+dir.dY);
		
		return true;
    }
    
    @Override
    public void undo(Board board) {
		Tile playerTile = board.player.getTile();
		Entity player = board.player;
        board.move(player, playerTile.tileX - dir.dX, playerTile.tileY - dir.dY);
    }
}
