import java.awt.Graphics;
import agents.controllers.MarioAIBase;
import engine.core.IEnvironment;
import engine.core.LevelScene;
import engine.graphics.VisualizationComponent;
import engine.input.*;

public class MyAgent extends MarioAIBase {
	/*  
		0 1 2 3 4 5
	-2  . . . . . .
	-1  . # . . . .
	 0  M # # # . .
	 1  . # # . . .
	 2  . # . . . .
	*/
	private boolean enemyAhead() {
		return    
			false ||
									 entities.danger(1,-1) ||
			entities.danger(0, 0) || entities.danger(1, 0) || entities.danger(2, 0) || entities.danger(3, 0) ||
									 entities.danger(1, 1) || entities.danger(2, 1) ||
									 entities.danger(1, 2)
		;
	}
	
	/*  
		0 1 2 3 4 5
	-2  . . . . . .
	-1  . . . . . .
	 0  M # . . . .
	 1  . # # . . .
	 2  . . . . . .
	*/
	private boolean shootableEnemyAhead() {
		return entities.shootable(1, 0) ||entities.shootable(1, 1) ||entities.shootable(1, 2);
	}
	
	private boolean brickAhead() {
		return     tiles.anyTile(1, 0) 
				|| tiles.anyTile(2, 0)
				|| tiles.anyTile(3, 0);
	}

	private boolean holeAhead() {
		return tiles.emptyTile(1, 1); 
	}

	@Override
	public void debugDraw(VisualizationComponent vis, LevelScene level,	IEnvironment env, Graphics g) {
		super.debugDraw(vis, level, env, g);
		if (mario == null) return;

		String debug = "Debug info: " + (int)mario.sprite.mapX;
		VisualizationComponent.drawStringDropShadow(g, debug, 0, 26, 1);
	}

	@Override
	public MarioInput actionSelectionAI() {
        MarioInput input = new MarioInput();

		input.press(MarioKey.RIGHT);

		if(enemyAhead()){
			if (shootableEnemyAhead() && mario.mayShoot && !lastInput.isPressed(MarioKey.SPEED)) {
				input.press(MarioKey.SPEED);    
			} else {
				input.release(MarioKey.RIGHT);
				input.press(MarioKey.LEFT);
			}
		}else{
			input.press(MarioKey.SPEED);
		}

        if (mario.mayJump && (enemyAhead() || brickAhead() || holeAhead()))
            input.press(MarioKey.JUMP);
		
		if (mario.isJumping()) {
			input.press(MarioKey.JUMP);
		}
		
		return input;
	}
}
