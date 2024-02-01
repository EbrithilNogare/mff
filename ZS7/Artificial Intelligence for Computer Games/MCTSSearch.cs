using Assets.Scripts.MCTS_Testing;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    class MCTSSearch : ISearch
    {
        public event System.Action<Move> onSearchComplete;

        MoveGenerator moveGenerator;

        float bestScore;
        Move bestMove;
        bool abortSearch;

        MCTSSettings settings;
        Board board;
        Evaluation evaluation;

        System.Random rand;

        // Diagnostics
        public SearchDiagnostics Diagnostics { get; set; }
        System.Diagnostics.Stopwatch searchStopwatch;

        public MCTSSearch(Board board, MCTSSettings settings)
        {
            this.board = board;
            this.settings = settings;
            evaluation = new Evaluation();
            moveGenerator = new MoveGenerator();
            rand = new System.Random();
        }

        public void StartSearch()
        {
            InitDebugInfo();

            // Initialize search settings
            bestMove = Move.InvalidMove;

            moveGenerator.promotionsToGenerate = settings.promotionsToSearch;
            abortSearch = false;
            Diagnostics = new SearchDiagnostics();

            MCTSNode root = new MCTSNode(new Move(), null);

            SearchMoves(root);

            onSearchComplete?.Invoke(bestMove);

            if (!settings.useThreading)
            {
                LogDebugInfo();
            }
        }

        public void EndSearch()
        {
            if (settings.useTimeLimit)
            {
                abortSearch = true;
            }
        }

        void SearchMoves(MCTSNode root)
        {
            var maxNumOfPlayouts = !settings.limitNumOfPlayouts || settings.maxNumOfPlayouts <= 0 ? 100 * 1000 : settings.maxNumOfPlayouts;
            var random = new System.Random(42);
            for (int numOfPlayouts = 0; numOfPlayouts < maxNumOfPlayouts && !abortSearch; numOfPlayouts++)
            {
                // Setup
                Board state = board.Clone();

                // Selection
                MCTSNode selected = root.Select(state, state.WhiteToMove == board.WhiteToMove, board.WhiteToMove);

                // Expansion
                selected = selected.Expand(state, moveGenerator);

                // Simulation
                int maxPlayoutDepth = settings.playoutDepthLimit > 0 ? settings.playoutDepthLimit : 100;
                maxPlayoutDepth -= getDepth(selected) - 1;
                float diff = selected.Simulate(state, evaluation, moveGenerator, random, maxPlayoutDepth, board.WhiteToMove);

                // Backpropagation
                selected.Backpropagate(diff);

                // Diagnostics
                Diagnostics.numPositionsEvaluated = numOfPlayouts + 1;
            }

            // Return best so far
            ReturnBestSoFar(root);

            // Diagnostics
            Diagnostics.move = bestMove.Name;
            Diagnostics.moveVal = bestScore.ToString();

            /**/ // switch of debug
            StringBuilder sb = new StringBuilder("- Tree: \n");
            ExportTree(root, sb, 2);
            UnityEngine.Debug.Log(sb.ToString());
            UnityEngine.Debug.Log("selected node: " + bestMove.Name);
            /**/
        }

        private int getDepth(MCTSNode selected)
        {
            int depth = 0;
            while (selected != null)
            {
                depth++;
                selected = selected.parent;
            }
            return depth;
        }

        void ReturnBestSoFar(MCTSNode root)
        {
            bestScore = float.MinValue;
            foreach (var child in root.childrens)
            {
                if (child.visits > 0 && child.score / child.visits > bestScore)
                {
                    bestScore = child.score / child.visits;
                    bestMove = child.move;
                }
            }
        }

        void LogDebugInfo()
        {
            // Optional
            // Debug.Log("Search time: " + searchStopwatch.ElapsedMilliseconds + "ms");
        }

        void InitDebugInfo()
        {
            searchStopwatch = System.Diagnostics.Stopwatch.StartNew();
        }

        // Data format for: https://vgarciasc.github.io/tree-renderer/
        void ExportTree(MCTSNode node, StringBuilder sb, int depth)
        {
            List<MCTSNode> nodes = new List<MCTSNode>(node.childrens);
            nodes.Sort((MCTSNode a, MCTSNode b) => { return (a.score / a.visits) - (b.score / b.visits) > 0 ? 1 : (a.score / a.visits) - (b.score / b.visits) < 0 ? -1 : 0; });
            foreach (var child in nodes)
            {
                if (child.UCT() == float.MaxValue)
                    continue;

                sb.Append(string.Format("{0} Move: {1}; Score/Visits: {2,5}/{3,5}={4,5} Score: {5,5}\n",
                    new string('-', depth),
                    child.move.Name,
                    child.score,
                    child.visits,
                    child.score / child.visits,
                    child.UCT()
                ));
                if (child.move.Name == "d5-c6" || child.move.Name == "f7-e6" || child.move.Name == "d5-e6")
                    ExportTree(child, sb, depth + 1);
            }
        }
    }
}
