using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prong
{
    class PongEngine
    {
        public StaticState Config { get; }

        private DynamicState state;

        public PongEngine(StaticState config)
        {
            this.Config = config;
        }

        public float clamp(float value, float min, float max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        public float plr1PaddleBallBounceVelocityY()
        {
            float up = state.plr1PaddleY - Config.paddleHeight() / 2.0f;
            float down = state.plr1PaddleY + Config.paddleHeight() / 2.0f;
            float ball = clamp(state.ballY, up, down);
            float ratio = (ball - up) / (down - up);
            ratio = 2 * (ratio - 0.5f);
            ratio = clamp(ratio, -0.9f, 0.9f);
            return ratio;
        }

        public float plr2PaddleBallBounceVelocityY()
        {
            float up = state.plr2PaddleY - Config.paddleHeight() / 2.0f;
            float down = state.plr2PaddleY + Config.paddleHeight() / 2.0f;
            float ball = clamp(state.ballY, up, down);
            float ratio = (ball - up) / (down - up);
            ratio = 2 * (ratio - 0.5f);
            ratio = clamp(ratio, -0.9f, 0.9f);
            return ratio;
        }

        void bounceBall(float ratio, float xDir)
        {
            state.ballVelocityY = Config.ballSpeed * ratio;
            state.ballVelocityX = (float)Math.Sqrt(Config.ballSpeed * Config.ballSpeed - state.ballVelocityY * state.ballVelocityY);
            state.ballVelocityX *= xDir;
            state.ballYDirection = 1;
        }

        public int plr1PaddleBounceX()
        {
            return -Config.ClientSize_Width / 2 + Config.paddleWidth() / 2;
        }

        public int plr2PaddleBounceX()
        {
            return Config.ClientSize_Width / 2 - Config.paddleWidth() / 2;
        }

        public bool ballFlyingRight()
        {
            return state.ballVelocityX > 0;
        }

        public bool ballHitsRightPaddle()
        {
            return state.ballX + Config.gridCellSize / 2 > plr2PaddleBounceX() - Config.paddleWidth() / 2
                && state.ballY - Config.gridCellSize / 2 < state.plr2PaddleY + Config.paddleHeight() / 2
                && state.ballY + Config.gridCellSize / 2 > state.plr2PaddleY - Config.paddleHeight() / 2;
        }

        public bool ballPastPlayer2Edge()
        {
            return state.ballX > Config.ClientSize_Width / 2;
        }

        public bool ballHitsLeftPaddle()
        {
            return state.ballX - Config.gridCellSize / 2 < plr1PaddleBounceX() + Config.paddleWidth() / 2
                && state.ballY - Config.gridCellSize / 2 < state.plr1PaddleY + Config.paddleHeight() / 2
                && state.ballY + Config.gridCellSize / 2 > state.plr1PaddleY - Config.paddleHeight() / 2;
        }

        public bool ballPastPlayer1Edge()
        {
            return state.ballX < -Config.ClientSize_Width / 2;
        }

        public bool ballHitsTop()
        {
            return state.ballVelocityY * state.ballYDirection >= 0 && state.ballY + Config.gridCellSize / 2 > Config.ClientSize_Height / 2;
        }

        public bool ballHitsBottom()
        {
            return state.ballVelocityY * state.ballYDirection <= 0 && state.ballY - Config.gridCellSize / 2 < -Config.ClientSize_Height / 2;
        }

        void moveBall(float timeDelta)
        {
            state.ballX += state.ballVelocityX * timeDelta;
            state.ballY += state.ballVelocityY * state.ballYDirection * timeDelta;
        }

        void resetBall()
        {
            state.ballX = 0;
            state.ballY = 0;
            int sign = -Math.Sign(state.ballVelocityX);
            state.ballVelocityX = sign * 400;
            state.ballVelocityY = 300;
        }

        public void SetState(DynamicState state)
        {
            this.state = state;
        }

        public TickResult Tick(DynamicState state, PlayerAction plr1, PlayerAction plr2, float timeDelta)
        {
            SetState(state);
            TickResult result = tickMechanics(timeDelta);
            tickPlayersActions(plr1, plr2, timeDelta);
            return result;
        }

        private TickResult tickMechanics(float timeDelta)
        {
            moveBall(timeDelta);

            if (ballFlyingRight())
            {
                if (ballHitsRightPaddle())
                {
                    bounceBall(plr2PaddleBallBounceVelocityY(), -1);
                }
                if (ballPastPlayer2Edge())
                {
                    state.plr1Score += 1;
                    resetBall();
                    return TickResult.PLAYER_1_SCORED;
                }
            }
            else
            {
                if (ballHitsLeftPaddle())
                {
                    bounceBall(plr1PaddleBallBounceVelocityY(), 1);
                }
                if (ballPastPlayer1Edge())
                {
                    state.plr2Score += 1;
                    resetBall();
                    return TickResult.PLAYER_2_SCORED;
                }
            }

            if (ballHitsTop())
            {
                state.ballYDirection *= -1;
            }

            if (ballHitsBottom())
            {
                state.ballYDirection *= -1;
            }

            return TickResult.TICK;
        }

        private void tickPlayersActions(PlayerAction plr1, PlayerAction plr2, float timeDelta) { 
            if (plr1 == PlayerAction.UP)
            {
                state.plr1PaddleY = state.plr1PaddleY + Config.paddle1Speed * timeDelta;
            } 
            if (plr1 == PlayerAction.DOWN)
            {
                state.plr1PaddleY = state.plr1PaddleY - Config.paddle1Speed * timeDelta;
            }
            if (plr2 == PlayerAction.UP)
            {
                state.plr2PaddleY = state.plr2PaddleY + Config.paddle2Speed * timeDelta;
            }
            if (plr2 == PlayerAction.DOWN)
            {
                state.plr2PaddleY = state.plr2PaddleY - Config.paddle2Speed * timeDelta;
            }
        }
    }
}
