import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.PriorityQueue;
import java.util.Set;

import search.*;

// A* search
public class AStar<S, A> {
	public static <S, A> Solution<S, A> search(HeuristicProblem<S, A> prob) {
		double minDebugValue = 0;
		PriorityQueue<HeuristicNode<S,A>> nodes = new PriorityQueue<HeuristicNode<S,A>>();
		Set<S> visitedStates = new HashSet<S>();

		nodes.add(new HeuristicNode<S,A>(prob.initialState(), 0));

		while(nodes.peek() != null && !prob.isGoal(nodes.peek().state)){
			HeuristicNode<S,A> node = nodes.peek(); 
			List<A> actions = prob.actions(node.state);
			for(A action : actions){
				if(!visitedStates.contains(prob.result(node.state, action))){
					S newState = prob.result(node.state, action);
					double newCost = prob.cost(node.state, action) + node.cost;
					HeuristicNode<S,A> foundNode = null;
					for(HeuristicNode<S,A> nodeIndex : nodes)
					if(nodeIndex.state.equals(newState)){
						foundNode = nodeIndex;
						break;
					}
					
					if(foundNode != null && foundNode.cost > newCost)
						continue;
					
					HeuristicNode<S,A> newNode = new HeuristicNode<S,A>(
						newState,
						newCost,
						node,
						action,
						prob.estimate(newState)
					);
					if(foundNode == null){
						nodes.add(newNode);
					} else {
						if(foundNode.cost > newCost){
							nodes.remove(foundNode);
							nodes.add(newNode);
						}
					}
				}
			};	
			
			double newMinDebugValue = nodes.peek().cost + nodes.peek().estimatedCost;
			if(true && minDebugValue < newMinDebugValue){
				minDebugValue = newMinDebugValue;
				System.out.print("searched " + visitedStates.size() + ", ");
				System.out.println("cost: " + minDebugValue);
			}
			visitedStates.add(node.state);
			nodes.remove();	
		}
		if(nodes.peek() == null)
			return null;
		else{
			List<A> finalActions = new ArrayList<A>();
			HeuristicNode<S,A> nodeIndex = nodes.peek();
			while(nodeIndex != null && nodeIndex.backtracking != null){
				finalActions.add(0,nodeIndex.action);
				nodeIndex = nodeIndex.backtracking;
			}
			return new Solution<S,A>(finalActions, nodes.peek().state, nodes.peek().cost);
		}
	}
}



class HeuristicNode<S,A> implements Comparable<HeuristicNode<S,A>> {
	S state;
	double cost;
  	HeuristicNode<S,A> backtracking;
	A action;
	double estimatedCost;
	
	public HeuristicNode(S state, double cost)
	{
		this.state = state;
		this.cost = cost;
		this.backtracking = null;
	}
	
	public HeuristicNode(S state, double cost, HeuristicNode<S,A> backtracking, A action, double estimatedCost)
	{
		this.state = state;
		this.cost = cost;
		this.backtracking = backtracking;
		this.action = action;
		this.estimatedCost = estimatedCost;
	}
  
	@Override public int compareTo(HeuristicNode<S,A> foreign)
	{
		double thisTotalCost = this.cost + this.estimatedCost;
		double foreignTotalCost = foreign.cost + foreign.estimatedCost;  
		if (thisTotalCost < foreignTotalCost)
			return -1;
		if (thisTotalCost > foreignTotalCost)
			return 1;
		return 0;
	}
}