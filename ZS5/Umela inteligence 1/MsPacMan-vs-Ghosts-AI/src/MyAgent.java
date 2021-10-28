import java.awt.Color;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import controllers.pacman.PacManControllerBase;
import game.core.G;
import game.core.Game;
import game.core.GameView;
import game.core.Game.DM;
import search.Problem;
import search.Solution;


public final class MyAgent extends PacManControllerBase
{	
	@Override
	public void tick(Game game, long timeDue) {
		PacmanProblem prob = new PacmanProblem(game);
		Solution<Integer, Integer> solution = Ucs.search(prob);
		
		if(solution == null){
			int[] allDirections = game.getPossiblePacManDirs(false);	
			pacman.set(allDirections[G.rnd.nextInt(allDirections.length)]); 
			return;
		}

		int[] directions = solution.actions.stream().mapToInt(i->i).toArray();
		if(directions.length>0)
			pacman.set(directions[0]);
		
		int state = game.getCurPacManLoc();
		for(int dir : directions){
			state = game.getNeighbour(state, dir);
			if(state != -1)
				GameView.addPoints(game, Color.RED, state);
		}
	}
}

class PacmanProblem implements Problem<Integer, Integer> {
	Game game;

	public PacmanProblem(Game game){
		this.game = game;
	}

	public Integer initialState() { 
		return game.getCurPacManLoc();
	}
  
	public List<Integer> actions(Integer state) { 
		List<Integer> directions = new ArrayList<Integer>();
		for(int i = 0; i<4;i++){
			if(game.getNeighbour(state, i) != -1)
				directions.add(i);
		}
		return directions;
	}

	public Integer result(Integer state, Integer action) {
		return game.getNeighbour(state, action);
	}
  
	public boolean isGoal(Integer state) { 
		boolean somebodyIsEdible = false;
		for(int i = 0; i < 4; i++)
			if(game.isEdible(i))
				somebodyIsEdible = true;
		
		if(somebodyIsEdible){
			for(int i = 0; i < 4; i++)
				if(game.isEdible(i) && game.getCurGhostLoc(i) == state){
					return true;
			}
			return false;
		}

		if(game.getFruitLoc() != -1 && game.getPathDistance(game.getCurPacManLoc(), game.getFruitLoc()) < game.getFruitValue() / 20)
			return game.getFruitLoc() == state;

		for(int i = 0; i < 4; i++)
			if(game.getManhattanDistance(state, game.getCurGhostLoc(i)) < 10)
				return false;

		int pillIndex = game.getPillIndex(state);
		if(pillIndex == -1){
			return false;
		}
		return game.checkPill(pillIndex);
	 }
  
	public double cost(Integer state, Integer action) {
		int nextStep = game.getNeighbour(state, action);
		int closest = Integer.MAX_VALUE;
		for(int i = 0; i < 4; i++){
			if(!game.isEdible(i))
				closest = Math.min(closest,game.getManhattanDistance(nextStep, game.getCurGhostLoc(i)));
		}

		return 10 + 100000 / Math.pow(closest,3);
	}
  }
