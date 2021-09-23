using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantoriniBot
{
    class MoveGenerator
    {
        public static List<(Board, Action)> GetNextBoards(Board board, bool opponentTurn)
        {
            List<(Board, Action)> result = new List<(Board, Action)>();
            if (opponentTurn)
            {
                AddWorkerMoves(board, board.OpponentWorker1, result, true, true);
                AddWorkerMoves(board, board.OpponentWorker2, result, true, false);
            }
            else
            {
                AddWorkerMoves(board, board.Worker1, result, false, true);
                AddWorkerMoves(board, board.Worker2, result, false, false);
            }
            return result;
        }


        private static void AddWorkerMoves(Board board, Coord worker, List<(Board, Action)> result, bool isOpponent, bool isWorker1)
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

                    if (!board.ValidCoord(buildCoord) || ((buildX != worker.X || buildY != worker.Y) && board.IsOccupied(buildCoord)) || board.GetElevation(buildCoord) == Board.MaxHeight)
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
                    result.Add((newBoard, action));
                }
            }
        }

        /*public bool IsOpponent;
        public Board Board;
        public Action Action;
        public List<MoveGenerator> Children;

        public MoveGenerator(Board board, Action action, int depth, bool isOpponent)
        {
            IsOpponent = isOpponent;
            Board = board;
            Action = action;
            Children = new List<MoveGenerator>();

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
                    Children.Add(new MoveGenerator(newBoard, action, depth - 1, !isOpponent));
                }
            }
        }

        public int ChildCount()
        {
            int count = Children.Count;
            foreach (MoveGenerator child in Children)
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
            foreach (MoveGenerator child in Children)
            {
                sum += child.LeafCount();
            }
            return sum;
        }*/
    }
}