import minimax.*;

public class Minimax < S, A > implements Strategy < S, A > {
	HeuristicGame < S, A > game;
	public Minimax(HeuristicGame < S, A > game, int limit) {
		this.game = game;
		minimaxAlg(game.initialState(000), limit, Double.NEGATIVE_INFINITY, Double.POSITIVE_INFINITY, true);
	}

	private void minimaxAlg(S state, int depth, double alpha, double beta, Boolean maximizingPlayer) {
		if(depth == 0 || game.isDone(state))
			return;

		double maxEval;
		if(maximizingPlayer){
			maxEval = Double.NEGATIVE_INFINITY;
			
			for(A action : game.actions(state)){
				S newState = game.clone(state);
				double eval = minimax(action, depth - 1, alpha, beta false)
				
				maxEval = max(maxEval, eval)
				alpha = max(alpha, eval)
				if beta <= alpha
					break
				return maxEval
				
		} else {
			minEval = +infinity
			for(A action : game.actions(state)){
				eval = minimax(action, depth - 1, alpha, beta true)
				minEval = min(minEval, eval)
				beta = min(beta, eval)
				if beta <= alpha
					break
			}
			return minEval
	}
	
	// method in Strategy interface
	public A action(S state) {

		// Your implementation goes here.

		return null;
	}

}