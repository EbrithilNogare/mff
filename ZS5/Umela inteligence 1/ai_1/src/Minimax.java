import minimax.*;

public class Minimax < S, A > implements Strategy < S, A > {
	HeuristicGame < S, A > game;
	public int mainLimit;
	Boolean playerGoalMax;
	A lastAction = null;
	
	public Minimax(HeuristicGame < S, A > game, int limit) {
		this.game = game;
		this.mainLimit = limit == 0 ? Integer.MAX_VALUE : limit;
	}

	private double minimaxAlg(S state, int depth, double alpha, double beta, Boolean maximizingPlayer) {
		if(depth == 0)
			return game.evaluate(state);
		
		if(game.isDone(state))
			return game.outcome(state) + (game.outcome(state) > 0 ? -1 : 1) * (this.mainLimit - depth);

		if(maximizingPlayer){
			double maxEval = Double.NEGATIVE_INFINITY;
			
			for(A action : game.actions(state)){
				S newState = game.clone(state);
				game.apply(newState, action);

				double eval = minimaxAlg(newState, depth - 1, alpha, beta, false);
				if(this.playerGoalMax && maxEval < eval && depth == this.mainLimit){
					lastAction = action;
				}
				maxEval = Math.max(maxEval, eval);
				
				if (maxEval >= beta)
					break;
				
				alpha = Math.max(alpha, eval);
			}
			return maxEval;
		} else {
			double minEval = Double.POSITIVE_INFINITY;
			for(A action : game.actions(state)){
				S newState = game.clone(state);
				game.apply(newState, action);

				double eval = minimaxAlg(newState, depth - 1, alpha, beta, true);
				if(!this.playerGoalMax && minEval > eval && depth == this.mainLimit){
					lastAction = action;
				}
				minEval = Math.min(minEval, eval);
				
				if (minEval <= alpha)
					break;
				
				beta = Math.min(beta, eval);
			}
			return minEval;
		}
	}
	
	// method in Strategy interface
	public A action(S state) {
		playerGoalMax = game.player(state) == 1; // true for maximizing
		double score = minimaxAlg(state, this.mainLimit, Double.NEGATIVE_INFINITY, Double.POSITIVE_INFINITY, playerGoalMax);
		//System.out.println("score: " + score);
		return lastAction;
	}
}