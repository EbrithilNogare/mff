package controllers.pacman.examples;

import controllers.pacman.PacManControllerBase;
import game.core.G;
import game.core.Game;

public final class RandomPacMan extends PacManControllerBase
{
	@Override
	public void tick(Game game, long timeDue) {
		int[] directions=game.getPossiblePacManDirs(true);		//set flag as true to include reversals		
		pacman.set(directions[G.rnd.nextInt(directions.length)]);
	}
}
