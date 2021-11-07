#pragma once

#include <list>
#include "PongGameState.h"

class Problem
{
public:
	PongGameState initialState();
	std::list<int> actions(PongGameState state);
	PongGameState result(PongGameState state, int action);
	bool isGoal(PongGameState state);
	bool cost(PongGameState state, int action);
};

