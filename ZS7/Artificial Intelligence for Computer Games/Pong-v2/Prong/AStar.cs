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
            int result = -x.CompareTo(y);

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
        float timeSimulationConst = 1 / 30f;

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
                    if (player == 1)
                    {
                        engine.Tick(nextState, action, otherPlayerMove(config, nextState), timeSimulationConst);
                    }
                    else
                    {
                        engine.Tick(nextState, otherPlayerMove(config, nextState), action, timeSimulationConst);
                    }
                    List<PlayerAction> newActions = new List<PlayerAction>(actionsUntilNow);
                    newActions.Add(action);
                    statesToGo.Add(GetHeuristic(nextState)+ newActions.Count()/1000, new stateAndActions(nextState, newActions));
                }
            }

            if (statesToGo.Count == 0)
                return PlayerAction.NONE;

            if (statesToGo.First().Value.actions.Count() == 0)
                return PlayerAction.NONE;

            Console.Write("seconds ahead: {0, 10}", (statesToGo.First().Value.actions.Count() * timeSimulationConst).ToString("n2"));
            Console.Write("   first: {0, 12}", statesToGo.First().Key.ToString("n2"));
            Console.Write("   last: {0, 12}", statesToGo.Last().Key.ToString("n2"));
            Console.WriteLine();
            return statesToGo.First().Value.actions[0];
        }

        // more is better
        private float GetHeuristic(DynamicState state)
        {
            float ballYAtHit = simulateUntilBounceAndGetBallY(state);
            float score = ((player == 1 ? -1 : 1) * (state.plr2Score - state.plr1Score));
            float ballToPaddle1Y = Math.Max(Math.Abs(state.plr1PaddleY - ballYAtHit) - config.paddleHeight() / 2, 0);
            float ballToPaddle2Y = Math.Max(Math.Abs(state.plr2PaddleY - ballYAtHit) - config.paddleHeight() / 2, 0);
            float ballToPaddle1X = Math.Abs(-config.ClientSize_Width / 2 - state.ballX);
            float ballToPaddle2X = Math.Abs( config.ClientSize_Width / 2 - state.ballX);
            int direction = state.ballVelocityX > 0 ? 1 : -1; // 1 == ball flying right

            if (player == 1)
                return 1000 * score + ballToPaddle2Y - ballToPaddle1Y;
            else
                return 1000 * score - ballToPaddle2Y + ballToPaddle1Y;
        }

        private float simulateUntilBounceAndGetBallY(DynamicState staticState)
        {
            DynamicState state = staticState.Clone();
            bool originalToTheRight = state.ballVelocityX > 0;
            bool toTheRight = state.ballVelocityX > 0;
            var result = TickResult.TICK;
            float lastBallY = 0;

            while (result == TickResult.TICK&&toTheRight == originalToTheRight)
            {
                lastBallY = state.ballY;
                result = engine.Tick(state, PlayerAction.NONE, PlayerAction.NONE, timeSimulationConst);
                toTheRight = state.ballVelocityX > 0;
            }
            return lastBallY;
        }
    }
}
