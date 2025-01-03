import java.util.ArrayList;
import java.util.Collections;
import java.util.HashSet;
import java.util.List;
import java.util.PriorityQueue;
import java.util.Set;

import search.*;

// A* search
public class AStar<S, A> {
	public static <S, A> Solution<S, A> search(HeuristicProblem<S, A> prob) {
		PriorityQueue<HeuristicNode<S,A>> nodes = new PriorityQueue<HeuristicNode<S,A>>();
		Set<S> visitedStates = new HashSet<S>();

		nodes.add(new HeuristicNode<S,A>(prob.initialState()));

		while(nodes.peek() != null && !prob.isGoal(nodes.peek().state)){
			HeuristicNode<S,A> node = nodes.peek(); 
			List<A> actions = prob.actions(node.state);
			for(A action : actions){
				if(visitedStates.contains(prob.result(node.state, action)))
					continue;

				S newState = prob.result(node.state, action);

				HeuristicNode<S,A> newNode = new HeuristicNode<S,A>(
					newState,
					prob.cost(node.state, action) + node.pathCost,
					node,
					action,
					prob.estimate(newState)
				);
				nodes.add(newNode);
			};	
			
			visitedStates.add(node.state);
			nodes.remove();	
		}

		System.out.print("visited " + visitedStates.size() + ", ");

		if(nodes.peek() == null)
			return null;
		else{
			List<A> finalActions = new ArrayList<A>();
			HeuristicNode<S,A> nodeIndex = nodes.peek();
			while(nodeIndex != null && nodeIndex.backtracking != null){
				finalActions.add(nodeIndex.action);
				nodeIndex = nodeIndex.backtracking;
			}
			Collections.reverse(finalActions);
			return new Solution<S,A>(finalActions, nodes.peek().state, nodes.peek().pathCost);
		}
	}
}

class HeuristicNode<S,A> implements Comparable<HeuristicNode<S,A>> {
	S state;
	HeuristicNode<S,A> backtracking;
	A action;
	double pathCost;
	double estimatedCost;
	double totalCost;
	
	public HeuristicNode(S state)
	{
		this.state = state;
	}
	
	public HeuristicNode(S state, double pathCost, HeuristicNode<S,A> backtracking, A action, double estimatedCost)
	{
		this.state = state;
		this.pathCost = pathCost;
		this.backtracking = backtracking;
		this.action = action;
		this.estimatedCost = estimatedCost;
		this.totalCost = estimatedCost + pathCost;
	}
  
	@Override public int compareTo(HeuristicNode<S,A> foreign)
	{
		if (this.totalCost < foreign.totalCost)
			return -1;
		if (this.totalCost > foreign.totalCost)
			return 1;
		return 0;
	}
}