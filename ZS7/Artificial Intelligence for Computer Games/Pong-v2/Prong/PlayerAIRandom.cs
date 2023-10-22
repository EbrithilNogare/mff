using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prong
{
    class PlayerAIRandom : Player
    {
        private Random rnd = new Random(42);

        public PlayerAction GetAction(StaticState config, DynamicState state)
        {
            double value = rnd.NextDouble();
            if (value < 0.4) return PlayerAction.UP;
            if (value > 0.6) return PlayerAction.DOWN;
            return PlayerAction.NONE;
        }
    }
}
