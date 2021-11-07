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
	
		nodes.push(new Node(prob->initialState(), 0, nullptr, 0));

		while (nodes.top() != nullptr && !prob->isGoal(nodes.top().state)) {
			Node node = nodes.top();
			std::list<int> actions = prob->actions(node.state);
			for (int action : actions){
				PongGameState resultState = prob->result(node.state, action);
				std::set<PongGameState>::iterator foundState = visitedStates.find(resultState);
				if (foundState == visitedStates.end()) {
					Node* foundNode = nullptr;
					if (foundNode) {

					}

				}



			}




		}
	
	
	}
};

