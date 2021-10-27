import controllers.pacman.PacManControllerBase;
import game.core.G;
import game.core.Game;

public final class MyAgent extends PacManControllerBase
{	
	@Override
	public void tick(Game game, long timeDue) {
		
		// Code your agent here.
		
		// Dummy implementation: move in a random direction.  You won't live long this way,
		int[] directions = game.getPossiblePacManDirs(false);	
		pacman.set(directions[G.rnd.nextInt(directions.length)]);
		
	}
}
