package ui.actions;

import game.actions.oop.IAction;
import game.board.oop.Board;
import ui.UIBoard;
import ui.atlas.SpriteAtlas;

public class UIActionFactory {

    public static IUIAction createUIAction(Board board, SpriteAtlas sprites, UIBoard uiBoard,
                                           IAction agentAction, boolean undo) {
		switch (agentAction.getType(board)) {
            case MOVE:
                return new UIMove(board, uiBoard, sprites, agentAction.getDirection(),
                                  231, 8, undo);

            case PUSH:
                return new UIPush(board, uiBoard, sprites, agentAction.getDirection(),
                                  231, 8, undo);
                                  
            default: return null;
		}
	}
	
}
