using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantoriniBot
{
    class MinMaxTreeNode
    {
        public bool IsOpponent;
        public Board Board;
        public Action Action;
        public List<MinMaxTreeNode> Children;


        public MinMaxTreeNode(Board board, Action action, int depth, bool isOpponent)
        {
            IsOpponent = isOpponent;
            Board = board;
            Action = action;
            Children = new List<MinMaxTreeNode>();

            if (depth > 0 && !BoardEvaluator.IsFinished(board))
            {
                if (isOpponent)
                {
                    AddWorkerMoves(board.OpponentWorker1, board, isOpponent, true, depth);
                    AddWorkerMoves(board.OpponentWorker2, board, isOpponent, false, depth);
                }
                else
                {
                    AddWorkerMoves(board.Worker1, board, isOpponent, true, depth);
                    AddWorkerMoves(board.Worker2, board, isOpponent, false, depth);
                }
            }
        }

        private void AddWorkerMoves(Coord worker, Board board, bool isOpponent, bool isWorker1, int depth)
        {
            foreach ((int offsetX, int offsetY) in Directions.NeighborOffsets)
            {
                int moveX = worker.X + offsetX;
                int moveY = worker.Y + offsetY;
                Coord moveCoord = new Coord { X = moveX, Y = moveY };

                if (!board.ValidCoord(moveCoord) || board.IsOccupied(moveCoord))
                {
                    continue;
                }
                
                int workerElevation = board.GetElevation(worker);
                int targetElevation = board.GetElevation(moveX, moveY);
                if (targetElevation > workerElevation + 1 || targetElevation == Board.MaxHeight)
                {
                    continue;
                }

                foreach ((int buildOffsetX, int buildOffsetY) in Directions.NeighborOffsets)
                {
                    int buildX = moveX + buildOffsetX;
                    int buildY = moveY + buildOffsetY;
                    Coord buildCoord = new Coord { X = buildX, Y = buildY };
                    
                    if (!board.ValidCoord(buildCoord) || board.IsOccupied(buildCoord) || board.GetElevation(buildCoord) == Board.MaxHeight)
                    {
                        continue;
                    }

                    Action action = new Action
                    {
                        IsOpponent = isOpponent,
                        IsWorker1 = isWorker1,
                        Move = moveCoord,
                        Build = buildCoord
                    };

                    Board newBoard = new Board(board, action);
                    Children.Add(new MinMaxTreeNode(newBoard, action, depth - 1, !isOpponent));
                }
            }
        }

        public int ChildCount()
        {
            int count = Children.Count;
            foreach (MinMaxTreeNode child in Children)
            {
                count += child.ChildCount();
            }
            return count;
        }

        public int LeafCount()
        {
            if (Children == null || Children.Count == 0)
            {
                return 1;
            }
            int sum = 0;
            foreach (MinMaxTreeNode child in Children)
            {
                sum += child.LeafCount();
            }
            return sum;
        }
    }
}