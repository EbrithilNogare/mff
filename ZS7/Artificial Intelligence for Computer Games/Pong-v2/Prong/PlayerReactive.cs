using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prong
{
    class PlayerReactive : Player
    {
        int player = 1;

        public PlayerReactive(int player)
        {
            this.player = player;
        }

        public PlayerAction GetAction(StaticState config, DynamicState state)
        {
            float paddleY = player == 1 ? state.plr1PaddleY : state.plr2PaddleY;
            if (state.ballY > paddleY + config.paddleHeight()/4) return PlayerAction.UP;
            if (state.ballY < paddleY - config.paddleHeight()/4) return PlayerAction.DOWN;
            return PlayerAction.NONE;
        }
    }
}
