#pragma once

#include <queue>
#include <set>

#include "Solution.h"
#include "Problem.h"
#include "Node.h"
#include "PongGameState.h"

class AStar
{
	Solution search(Problem* prob) {
		std::priority_queue<Node, NodeComparator> nodes;
		std::set<PongGameState> visitedStates;
	
		nodes.push(new Node(prob->initialState(), 0, 0, nullptr, 0));

		while (nodes.top() != nullptr && !prob->isGoal(nodes.top().state)) {
			Node node = nodes.top();
			std::list<int> actions = prob->actions(node.state);
			for (int action : actions){
				PongGameState newState = prob->result(node.state, action);
				if (visitedStates.find(newState) != visitedStates.end())
					continue;

				double newPathCost = prob->cost(node.state, action) + node.pathCost;

				Node newNode(
					newState,
					newPathCost,
					prob->estimate(newState),
					node,
					action,
					prob->estimate(newState)
					);

				if (foundNode == null) {
					nodes.add(newNode);
				}
				else {
					if (foundNode.pathCost > newPathCost) {
						nodes.remove(foundNode);
						nodes.add(newNode);
					}
				}
			}
			visitedStates.add(node.state);
			nodes.remove();
		}
			




		//System.out.print("visited " + visitedStates.size() + ", ");

		if (nodes.peek() == null)
			return null;
		else {
			List<A> finalActions = new ArrayList<A>();
			HeuristicNode<S, A> nodeIndex = nodes.peek();
			while (nodeIndex != null && nodeIndex.backtracking != null) {
				finalActions.add(0, nodeIndex.action);
				nodeIndex = nodeIndex.backtracking;
			}
			return new Solution<S, A>(finalActions, nodes.peek().state, nodes.peek().pathCost);
		}
	
	
	}
};

