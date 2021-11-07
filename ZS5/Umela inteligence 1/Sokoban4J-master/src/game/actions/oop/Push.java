package game.actions.oop;

import game.actions.EDirection;
import game.board.oop.*;
import game.board.oop.entities.Entity;

public class Push implements IAction {
    EDirection dir;

    public Push(EDirection dir) { this.dir = dir; }

    @Override
    public EActionType getType(Board board) {
        return EActionType.PUSH;
    }

    @Override
    public EDirection getDirection() {
        return dir;
    }

    @Override
    public boolean isPossible(Board board) {
        Tile playerTile = board.player.getTile();
        
		// PLAYER ON THE EDGE
        if (!board.isPosValid(playerTile.tileX + dir.dX, playerTile.tileY + dir.dY))
            return false;
		
		Tile boxTile = board.tile(playerTile.tileX+dir.dX, playerTile.tileY+dir.dY);
		
		// NOT A BOX NEXT TO THE PLAYER
		if (!boxTile.isSomeBox()) return false;
		
		// BOX ON THE EDGE
        if (!board.isPosValid(boxTile.tileX + dir.dX, boxTile.tileY + dir.dY))
            return false;
				
		// BOX BEHIND THE TILE IS FREE
		if (board.tile(boxTile.tileX+dir.dX, boxTile.tileY+dir.dY).isFree()) return true;
		
		// ???
		return false;
    }

    @Override
    public boolean perform(Board board) {
		if (!isPossible(board)) return false;
		
		Tile playerTile = board.player.getTile();
		
        // MOVE THE BOX FIRST
        Entity box = board.tile(playerTile.tileX+dir.dX, playerTile.tileY+dir.dY).entity;
        board.move(box, playerTile.tileX+dir.dX+dir.dX, playerTile.tileY+dir.dY+dir.dY);
    
		// MOVE THE PLAYER
		Entity player = board.player;
		board.move(player, playerTile.tileX+dir.dX, playerTile.tileY+dir.dY);
		
		return true;
    }
    
    @Override
    public void undo(Board board) {
		Tile playerTile = board.player.getTile();

		Entity player = board.player;
        Entity box = board.tile(playerTile.tileX+dir.dX, playerTile.tileY+dir.dY).entity;

        board.move(player, playerTile.tileX - dir.dX, playerTile.tileY - dir.dY);
        board.move(box, playerTile.tileX, playerTile.tileY);
    }
}
