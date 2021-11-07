package ui.entities;

import game.board.oop.entities.Entity;
import ui.atlas.SpriteAtlas;

public class UIBox extends UIEntity {

	public UIBox(Entity entity, SpriteAtlas sprites) {
		super(entity, sprites);
		if (!entity.getType().isSomeBox()) throw new RuntimeException("NOT A BOX!");		
	}
	
	public void inPlace() {
		currentSprite = entity.getType().getSpriteBoxAtPosition();
	}
	
	public void outOfPlace() {
		currentSprite = entity.getType().getSprite();
	}
	
}
