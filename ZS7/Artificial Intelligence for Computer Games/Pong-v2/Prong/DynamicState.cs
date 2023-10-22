using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prong
{
    class DynamicState
    {
        public float ballX = 0;
        public float ballY = 0;
        public float ballVelocityX = 400;
        public float ballVelocityY = 300;
        public float ballYDirection = 1;
        public float plr1PaddleY = 0;
        public float plr2PaddleY = 0;
        public int plr1Score = 0;
        public int plr2Score = 0;

        public DynamicState Clone()
        {
            DynamicState result = new DynamicState();

            result.ballX = ballX;
            result.ballY = ballY;
            result.ballVelocityX = ballVelocityX;
            result.ballVelocityY = ballVelocityY;
            result.ballYDirection = ballYDirection;
            result.plr1PaddleY = plr1PaddleY;
            result.plr2PaddleY = plr2PaddleY;
            result.plr1Score = plr1Score;
            result.plr2Score = plr2Score;

            return result;
    }
    }
}
