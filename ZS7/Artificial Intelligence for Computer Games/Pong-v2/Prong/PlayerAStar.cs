using System;

namespace Prong
{
    class PlayerAStar : Player
    {
        int player;
        float frameTime = 1 / 120f;
        Func<StaticState, DynamicState, PlayerAction> otherPlayerMove;

        public PlayerAStar(int player, Func<StaticState, DynamicState, PlayerAction> otherPlayerMove)
        {
            this.player = player;
            this.otherPlayerMove = otherPlayerMove;
        }

        public void SetFrameTime(float frameTime)
        {
            this.frameTime = frameTime;
        }

        public PlayerAction GetAction(StaticState config, DynamicState state)
        {
            AStar astar = new AStar(config, player, otherPlayerMove);
            return astar.FindNextMove(state, frameTime);
        }
    }
}
