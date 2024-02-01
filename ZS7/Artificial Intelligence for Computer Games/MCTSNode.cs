using Chess;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MCTS_Testing
{
    public class MCTSNode
    {
        public Move move;
        public MCTSNode parent;
        public List<MCTSNode> childrens;
        public int visits;
        public float score;

        public MCTSNode(Move move, MCTSNode parent)
        {
            this.move = move;
            this.parent = parent;
            this.childrens = new List<MCTSNode>();
            this.visits = 0;
            this.score = 0;
        }

        public MCTSNode Select(Board state, bool paritySource, bool chearringToWhite)
        {
            var node = this;
            for (bool parity = paritySource; node.childrens.Count > 0; parity = !parity)
            {
                var bestChild = node.childrens[0];
                var bestValue = float.MinValue;
                foreach (var child in node.childrens)
                {
                    float uctValue = child.UCT(state.WhiteToMove != chearringToWhite);
                    if (uctValue > bestValue)
                    {
                        bestChild = child;
                        bestValue = uctValue;
                    }
                }
                node = bestChild;
                state.MakeMove(node.move);
            }
            return node;
        }

        public MCTSNode Expand(Board state, MoveGenerator moveGenerator)
        {
            MCTSNode child = null;
            bool isRoot = parent == null;

            foreach (var move in moveGenerator.GenerateMoves(state, isRoot))
            {
                if (state.KingSquare[0] == move.TargetSquare || state.KingSquare[1] == move.TargetSquare)
                {
                    childrens.Clear();
                    return this;
                }
                child = new MCTSNode(move, this);
                childrens.Insert(0, child);
            }
            if (child == null) return this;

            state.MakeMove(child.move);

            return child;
        }

        public float Simulate(Board state, Evaluation evaluation, MoveGenerator moveGenerator, System.Random random, int maxPlayoutDepth, bool chearringToWhite)
        {
            var simulatedState = state.GetLightweightClone();
            bool whiteToMove = state.WhiteToMove;
            for (int i = 0; i < maxPlayoutDepth; i++)
            {
                List<SimMove> possibleMoves = moveGenerator.GetSimMoves(simulatedState, whiteToMove);

                if (possibleMoves.Count == 0)
                {
                    break;
                }
                SimMove nextMove = possibleMoves[random.Next(possibleMoves.Count)];

                // check end of game
                if (simulatedState[nextMove.endCoord1, nextMove.endCoord2]?.type == SimPieceType.King)
                {
                    return whiteToMove == chearringToWhite ? 1 : 0;
                }

                AlterBoardWithMove(simulatedState, nextMove);

                whiteToMove = !whiteToMove;
            }

            switch (IsWinningState(simulatedState))
            {
                case 1:
                    return chearringToWhite ? 1 : 0;
                case 2:
                    return chearringToWhite ? 0 : 1;
            }
            return evaluation.EvaluateSimBoard(simulatedState, chearringToWhite);
        }

        public void Backpropagate(float diff)
        {
            for (var node = this; node != null; node = node.parent)
            {
                node.score += diff;
                node.visits++;
            }
        }

        public float UCT(bool asEnemy = false)
        {
            if (visits == 0)
            {
                return float.MaxValue;
            }
            float exploitationValue = asEnemy ? 1 - score / visits : score / visits;
            float c = 1; // Mathf.Sqrt(2);
            float explorationValue = Mathf.Sqrt(Mathf.Log(parent.visits) / visits);
            return exploitationValue + c * explorationValue;
        }

        private void AlterBoardWithMove(SimPiece[,] board, SimMove move)
        {
            board[move.endCoord1, move.endCoord2] = board[move.startCoord1, move.startCoord2];
            board[move.startCoord1, move.startCoord2] = null;
        }

        // return 1 of white is winning, 2 if black and 0 or 3 if none
        private byte IsWinningState(SimPiece[,] board)
        {
            byte toReturn = 0;
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                {
                    if (board[x, y] != null && board[x, y].type == SimPieceType.King)
                        toReturn += board[x, y].team ? (byte)1 : (byte)2;
                }
            return toReturn;
        }
    }
}
