#pragma once

#include "PongGameState.h"

struct Node
{
	PongGameState state;
	float cost;
	struct Node* backtracking;
	int action;
	Node(PongGameState state, float cost, Node* backtracking, int action) {
		this->state = state;
		this->cost = cost;
		this->backtracking = backtracking;
		this->action = action;
	}
};

struct NodeComparator {
	bool operator()(const Node& left, const Node& right) {
		return left.cost < right.cost;
	}
};

