import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.PriorityQueue;
import java.util.Set;

import search.*;

// uniform-cost search

public class Ucs<S, A> {
	public static <S, A> Solution<S, A> search(Problem<S, A> prob) {
		PriorityQueue<Node<S,A>> nodes = new PriorityQueue<Node<S,A>>();
		Set<S> visitedStates = new HashSet<S>();

		nodes.add(new Node<S,A>(prob.initialState(), 0));

		while(nodes.peek() != null && !prob.isGoal(nodes.peek().state)){
			Node<S,A> node = nodes.peek(); 
			List<A> actions = prob.actions(node.state);
			for(A action : actions){
				if(!visitedStates.contains(prob.result(node.state, action))){
					addOrRepair(
						nodes,
						new Node<S,A>(
							prob.result(node.state, action),
							prob.cost(node.state, action) + node.cost,
							node,
							action
						)
					);
				}
			};		
			
			//System.out.println("cost: " + nodes.peek().cost);
			visitedStates.add(node.state);
			nodes.remove();	
		}
		if(nodes.peek() == null)
			return null;
		else{
			List<A> finalActions = new ArrayList<A>();
			Node<S,A> nodeIndex = nodes.peek();
			while(nodeIndex != null && nodeIndex.backtracking != null){
				finalActions.add(0,nodeIndex.action);
				nodeIndex = nodeIndex.backtracking;
			}
			return new Solution<S,A>(finalActions, nodes.peek().state, nodes.peek().cost);
		}
	}

	private static <S,A> void addOrRepair(PriorityQueue<Node<S, A>> nodes, Node<S, A> newNode) {
		Node<S,A> foundNode = null;
		for(Node<S,A> node : nodes)
			if(node.state.equals(newNode.state)){
				foundNode = node;
				break;
			}

		if(foundNode == null){
			nodes.add(newNode);
		} else {
			if(foundNode.cost > newNode.cost){
				nodes.remove(foundNode);
				nodes.add(newNode);
			}
		}
		
	}
}

class Node<S,A> implements Comparable<Node<S,A>> {
    S state;
    double cost;
	Node<S,A> backtracking;
	A action;
    
    public Node(S state, double cost)
    {
        this.state = state;
        this.cost = cost;
		this.backtracking = null;
    }
	
    public Node(S state, double cost, Node<S,A> backtracking, A action)
    {
        this.state = state;
        this.cost = cost;
		this.backtracking = backtracking;
		this.action = action;
    }
  
    @Override public int compareTo(Node<S,A> foreign)
    {
        if (this.cost < foreign.cost)
            return -1;
        if (this.cost > foreign.cost)
            return 1;
        return 0;
    }
}