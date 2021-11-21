package minesweeper4j.agents;

import java.awt.event.KeyEvent;

import minesweeper4j.game.Action;
import minesweeper4j.game.Board;
import minesweeper4j.game.EAction;
import minesweeper4j.game.IAgent;
import minesweeper4j.game.Tile;

public class HumanAgent implements IAgent {

	private Object mutex = new Object();
	
	private Action action;

	private Board board;
	
	@Override
	public void newBoard() {
	}

	@Override
	public void observe(Board board) {
		synchronized(mutex) {
			this.board = board;
		}
	}

	@Override
	public Action act() {
		if (action == null) {
			try {
				Thread.sleep(10);
			} catch (Exception e) {					
			}
			return null;
		}
		synchronized(mutex) {			
			Action action = this.action;
			this.action = null;
			return action;
		}
	}

	@Override
	public void victory() {		
	}

	@Override
	public void died() {
	}

	@Override
	public void stop() {
	}
	
	@Override
	public void tileClicked(int tileX, int tileY, boolean rightBtn) {
		synchronized(mutex) {
			if (board == null) return;
			Tile tile = board.tile(tileX, tileY);
			if (rightBtn) {
				if (tile.visible) return;
				if (tile.flag) {
					action = new Action(EAction.UNFLAG, tileX, tileY);
				} else {
					action = new Action(EAction.FLAG, tileX, tileY);
				}
			} else {
				if (tile.visible) return;
				action = new Action(EAction.OPEN, tileX, tileY);
			}
		}
	}
	
	@Override
	public void keyPressed(KeyEvent event) {
		if (event.getKeyCode() == KeyEvent.VK_S) {
			synchronized(mutex) {
				action = new Action(EAction.SUGGEST_SAFE_TILE);
			}
		}
		
	}

}
