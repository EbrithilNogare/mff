package game.board;

import game.actions.EDirection;
import game.actions.compact.CAction;
import game.actions.compact.CMove;
import game.actions.compact.CPush;
import game.actions.compressed.MAction;
import game.actions.compressed.MMove;
import game.actions.compressed.MPush;
import game.actions.oop.*;
import game.actions.slim.SAction;
import game.actions.slim.SMove;
import game.actions.slim.SPush;
import game.board.compact.BoardCompact;
import game.board.compact.CTile;
import game.board.compressed.BoardCompressed;
import game.board.compressed.MTile;
import game.board.compressed.MTile.SubSlimTile;
import game.board.oop.Board;
import game.board.slim.BoardSlim;

public class ValidatingAgent {

	private Board board;

	public ValidatingAgent(Board board) {
		this.board = board;
	}
	
	// ===========
	// BOARD - OOP
	// ===========
	
	public void validateBoard() {
		// CHECK CLONING
		Board clone = board.clone();
		if (!board.equalsState(clone)) {
			throw new RuntimeException("CLONE FAILED!");
		}
		
		for (int x = 0; x < board.width; ++x) {
			for (int y = 0; y < board.height; ++y) {
				if (board.tile(x, y).isFree()) {
					board.player.getTile().entity = null;
					board.player.setTile(board.tile(x, y));
					board.player.getTile().entity = board.player;					
				}
				
				// CHECK CLONING
				clone = board.clone();
				if (!board.equalsState(clone)) {
					throw new RuntimeException("CLONE FAILED!");
				}
				
				// TEST ACTIONS
				testPlayerActions(board);
			}
		}
	}
	
	private void testPlayerActions(Board board) {
		for (EDirection direction : EDirection.arrows()) {
			IAction action = Move.orPush(board, direction);
			Board clone = board.clone();
			if (!action.isPossible(board)) continue;
			if (action.getType(board) == EActionType.MOVE) {
				IAction opposite = Move.orPush(board, direction.opposite());
				action.perform(clone);
				opposite.perform(clone);
				if (!board.equalsState(clone)) {
					throw new RuntimeException("MOVE BACK AND FORTH FAILED!");
				}
			} else {
				// we can just push
				action.perform(clone);
			}
		}
	}
	
	// ===============
	// BOARD - COMPACT
	// ===============

	public void validateBoardCompact() {
		BoardCompact compact = board.makeBoardCompact();
				
		BoardCompact clone = compact.clone();
		checkEqual(compact, clone, "CLONING");
				
		for (int x = 0; x < compact.width(); ++x) {
			for (int y = 0; y < compact.height(); ++y) {
				clone = compact.clone();
				
				if (CTile.isFree(clone.tile(x, y))) {
					clone.movePlayer(clone.playerX, clone.playerY, x, y);
				}
				
				// CHECK CLONING				
				BoardCompact clone2 = clone.clone();
				checkEqual(clone, clone2, "CLONING");
				
				// TEST ACTIONS
				testPlayerActions(clone);
			}
		}		
	}
	
	private void checkEqual(BoardCompact b1, BoardCompact b2, String msg) {
		if (!b1.equalsState(b2)) {
			throw new RuntimeException("NOT STATE EQUAL: " + msg);
		}
		if (b1.hashCode() != b2.hashCode()) {
			throw new RuntimeException("HASH CODE DIFFERS: " + msg);
		}
		if (!b1.equals(b2)) {
			throw new RuntimeException("NOT EQUAL: " + msg);
		}
	}
	
	private void testPlayerActions(BoardCompact board) {
		for (EDirection direction : EDirection.arrows()) {
			BoardCompact clone = board.clone();
			
			CAction action = CMove.getAction(direction);
			
			if (action.isPossible(clone)) {
				action.perform(clone);
				action.reverse(clone);
				checkEqual(board, clone, "ACTION: " + action);
			}
			
			action = CPush.getAction(direction);
			if (action.isPossible(clone)) {
				action.perform(clone);
				action.reverse(clone);
				checkEqual(board, clone, "ACTION: " + action);
			}
		}
	}
	
	// ==================
	// BOARD - COMPRESSED
	// ==================

	public void validateBoardCompressed() {
		BoardCompressed compressed = board.makeBoardCompact().makeBoardCompressed();
				
		BoardCompressed clone = compressed.clone();
		checkEqual(compressed, clone, "CLONING");
				
		for (int x = 0; x < compressed.width(); ++x) {
			for (int y = 0; y < compressed.height(); ++y) {
				clone = compressed.clone();
				
				SubSlimTile sst = MTile.getSubSlimTile(x, y);
				
				if (MTile.isFree(sst, clone.tile(x, y))) {
					clone.movePlayer(clone.playerX, clone.playerY, x, y);
				}
				
				// CHECK CLONING				
				BoardCompressed clone2 = clone.clone();
				checkEqual(clone, clone2, "CLONING");
				
				// TEST ACTIONS
				testPlayerActions(clone);
			}
		}		
	}
	
	private void checkEqual(BoardCompressed b1, BoardCompressed b2, String msg) {
		if (b1.hashCode() != b2.hashCode()) {
			System.out.println("HASH CODE DIFFERS: " + msg);
			System.out.println("-- BOARD B1 ---");
			b1.debugPrint();
			System.out.println("-- BOARD B2 ---");
			b2.debugPrint();
			throw new RuntimeException("HASH CODE DIFFERS: " + msg);
		}
		if (!b1.equals(b2)) {
			System.out.println("NOT EQUAL: " + msg);
			System.out.println("-- BOARD B1 ---");
			b1.debugPrint();
			System.out.println("-- BOARD B2 ---");
			b2.debugPrint();
			throw new RuntimeException("NOT EQUAL: " + msg);
		}
	}
	
	private void testPlayerActions(BoardCompressed board) {
		for (EDirection direction : EDirection.arrows()) {
			BoardCompressed clone = board.clone();
			
			MAction action = MMove.getAction(direction);
			
			if (action.isPossible(clone)) {
				action.perform(clone);
				action.reverse(clone);
				checkEqual(board, clone, "ACTION: " + action);
			}
			
			action = MPush.getAction(direction);
			if (action.isPossible(clone)) {
				action.perform(clone);
				action.reverse(clone);
				checkEqual(board, clone, "ACTION: " + action);
			}
		}
	}

	// ============
	// BOARD - SLIM
	// ============
	
	public void validateBoardSlim() {
		BoardSlim slim = board.makeBoardCompact().makeBoardSlim();
				
		BoardSlim clone = slim.clone();
		checkEqual(slim, clone, "CLONING");
				
		for (byte x = 0; x < slim.width(); ++x) {
			for (byte y = 0; y < slim.height(); ++y) {
				clone = slim.clone();
				
				if (CTile.isFree(clone.tile(x, y))) {
					clone.movePlayer(clone.playerX, clone.playerY, x, y);
				}
				
				// CHECK CLONING				
				BoardSlim clone2 = clone.clone();
				checkEqual(clone, clone2, "CLONING");
				
				// TEST ACTIONS
				testPlayerActions(clone);
			}
		}		
	}
	
	private void checkEqual(BoardSlim b1, BoardSlim b2, String msg) {
		if (b1.hashCode() != b2.hashCode()) {
			System.out.println("HASH CODE DIFFERS: " + msg);
			System.out.println("-- BOARD B1 ---");
			b1.debugPrint();
			System.out.println("-- BOARD B2 ---");
			b2.debugPrint();
			throw new RuntimeException("HASH CODE DIFFERS: " + msg);
		}
		if (!b1.equals(b2)) {
			System.out.println("NOT EQUAL: " + msg);
			System.out.println("-- BOARD B1 ---");
			b1.debugPrint();
			System.out.println("-- BOARD B2 ---");
			b2.debugPrint();
			throw new RuntimeException("NOT EQUAL: " + msg);
		}
	}
	
	private void testPlayerActions(BoardSlim board) {
		for (EDirection direction : EDirection.arrows()) {
			BoardSlim clone = board.clone();
			
			SAction action = SMove.getAction(direction);
			
			if (action.isPossible(clone)) {
				action.perform(clone);
				action.reverse(clone);
				checkEqual(board, clone, "ACTION: " + action);
			}
			
			action = SPush.getAction(direction);
			if (action.isPossible(clone)) {
				action.perform(clone);
				action.reverse(clone);
				checkEqual(board, clone, "ACTION: " + action);
			}
		}
	}
		

}
