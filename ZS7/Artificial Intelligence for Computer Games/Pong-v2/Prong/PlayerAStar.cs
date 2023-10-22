using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prong
{
    class PlayerAStar : Player
    {
        int player;
        Func<StaticState, DynamicState, PlayerAction> otherPlayerMove;

        public PlayerAStar(int player, Func<StaticState, DynamicState, PlayerAction> otherPlayerMove)
        {
            this.player = player;
            this.otherPlayerMove = otherPlayerMove;
        }

        public PlayerAction GetAction(StaticState config, DynamicState state)
        {
            AStar astar = new AStar(config, player, otherPlayerMove);
            return astar.FindNextMove(state, .01f);
        }
    }
}
