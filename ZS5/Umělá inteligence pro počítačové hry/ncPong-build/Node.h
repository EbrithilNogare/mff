#pragma once

#include "PongGameState.h"

struct Node
{
	PongGameState state;
	struct Node* backtracking;
	int action;
	float pathCost;
	float estimatedCost;
	float totalCost;
	Node(PongGameState state, float pathCost, float estimatedCost, Node* backtracking, int action) {
		this->state = state;
		this->backtracking = backtracking;
		this->action = action;
		this->pathCost = pathCost;
		this->estimatedCost = estimatedCost;
		this->totalCost = pathCost + estimatedCost;
	}
};

struct NodeComparator {
	bool operator()(const Node& left, const Node& right) {
		return left.totalCost < right.totalCost;
	}
};

