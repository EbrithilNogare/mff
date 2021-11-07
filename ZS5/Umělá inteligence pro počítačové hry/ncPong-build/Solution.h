#pragma once

#include <list>
#include "PongGameState.h"

struct Solution
{
	std::list<int> actions;
	PongGameState goalState;
	float pathCost;
	Solution(std::list<int> actions, PongGameState& goalState, float pathCost) {
		this->actions = actions;
		this->goalState = goalState;
		this->pathCost = pathCost;
	}
};

