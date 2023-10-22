using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prong
{
    /// <summary>
    /// Comparer for comparing two keys, handling equality as beeing greater
    /// Use this Comparer e.g. with SortedLists or SortedDictionaries, that don't allow duplicate keys
    /// from https://stackoverflow.com/questions/5716423/c-sharp-sortable-collection-which-allows-duplicate-keys
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class DuplicateKeyComparer<TKey>
                    :
                 IComparer<TKey> where TKey : IComparable
    {
        #region IComparer<TKey> Members

        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);

            if (result == 0)
                return 1; // Handle equality as being greater. Note: this will break Remove(key) or
            else          // IndexOfKey(key) since the comparer never returns 0 to signal key equality
                return result;
        }

        #endregion
    }

    struct stateAndActions
    {
        public DynamicState state;
        public List<PlayerAction> actions;

        public stateAndActions(DynamicState state, List<PlayerAction> actions)
        {
            this.state = state;
            this.actions = actions;
        }
    }

    class AStar
    {
        float timeSimulationConst = 1/30f;

        PlayerAction[] possibleMoves = {
            PlayerAction.UP,
            PlayerAction.DOWN,
            PlayerAction.NONE
        };

        StaticState config;
        PongEngine engine;
        int player;
        Func<StaticState, DynamicState, PlayerAction> otherPlayerMove;

        public AStar(StaticState config, int player, Func<StaticState, DynamicState, PlayerAction> otherPlayerMove)
        {
            this.config = config;
            this.engine = new PongEngine(config);
            this.player = player;
            this.otherPlayerMove = otherPlayerMove;
        }

        public PlayerAction FindNextMove(DynamicState state, float timeToDie)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var statesToGo = new SortedList<float, stateAndActions>(new DuplicateKeyComparer<float>());
            statesToGo.Add(GetHeuristic(state), new stateAndActions(state, new List<PlayerAction>()));

            while (statesToGo.Count > 0 && stopwatch.ElapsedMilliseconds < timeToDie)
            {
                DynamicState currentState = statesToGo.First().Value.state;
                List<PlayerAction> actionsUntilNow = statesToGo.First().Value.actions;
                statesToGo.RemoveAt(0);

                foreach (PlayerAction action in possibleMoves)
                {
                    DynamicState nextState = currentState.Clone();
                    if(player == 1) {
                        engine.Tick(nextState, action, otherPlayerMove(config, nextState), timeSimulationConst);
                    } else {
                        engine.Tick(nextState, otherPlayerMove(config, nextState), action, timeSimulationConst);
                    }
                    List<PlayerAction> newActions = new List<PlayerAction>(actionsUntilNow);
                    newActions.Add(action);
                    statesToGo.Add(GetHeuristic(nextState), new stateAndActions(nextState, newActions));
                }
            }

            if (statesToGo.Count == 0)
                return PlayerAction.NONE;

            if (statesToGo.First().Value.actions.Count() == 0)
                return PlayerAction.NONE;

            Console.WriteLine("depth: " + statesToGo.First().Value.actions.Count());
            return statesToGo.First().Value.actions[0];
        }

        private float GetHeuristic(DynamicState state)
        {
            float ballToPaddle1Y = Math.Abs(state.plr1PaddleY - state.ballY);
            float ballToPaddle2Y = Math.Abs(state.plr2PaddleY - state.ballY);
            int direction = state.ballVelocityX > 0 ? 1 : -1; // 1 == ball flying right
            
            if (player == 1)
                return config.ClientSize_Height + ballToPaddle1Y - ballToPaddle2Y;
            else
                return config.ClientSize_Height + ballToPaddle2Y - ballToPaddle1Y;
        }
    }
}
