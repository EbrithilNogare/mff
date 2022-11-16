#pragma once
struct PongGameState
{
	float ballX;
	float ballY;
	int ballVelocity;
	float leftPaddle;
	float rightPaddle;
	
	PongGameState(float ballX, float ballY, int ballVelocity, float leftPaddle, float rightPaddle) {
		this->ballX = ballX;
		this->ballY = ballY;
		this->ballVelocity = ballVelocity;
		this->leftPaddle = leftPaddle;
		this->rightPaddle = rightPaddle;
	}
	PongGameState() = default;

	bool operator==(const PongGameState& right) {
		return ballX == right.ballX &&
			ballY == right.ballY &&
			ballVelocity == right.ballVelocity &&
			leftPaddle == right.leftPaddle &&
			rightPaddle == right.rightPaddle;
	}
};

