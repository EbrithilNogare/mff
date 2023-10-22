using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prong
{
    interface Player
    {
        PlayerAction GetAction(StaticState config, DynamicState state);
    }
}
