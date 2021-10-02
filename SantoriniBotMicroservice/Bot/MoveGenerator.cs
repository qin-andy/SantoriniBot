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
                result.AddRange(GetHephaestusWorkerMoves(board, board.OpponentWorker1, true, true));
                result.AddRange(GetHephaestusWorkerMoves(board, board.OpponentWorker2, true, false));
            }
            else
            {
                result.AddRange(GetWorkerMoves(board, board.Worker1, false, true));
                result.AddRange(GetWorkerMoves(board, board.Worker2, false, false));
            }
            return result;
        }


        private static List<(Board, Action)> GetWorkerMoves(Board board, Coord worker, bool isOpponent, bool isWorker1)
        {
            List<(Board, Action)> result = new List<(Board, Action)>();
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
            return result;
        }

        private static List<(Board, Action)> GetHephaestusWorkerMoves(Board board, Coord worker, bool isOpponent, bool isWorker1)
        {
            List<(Board, Action)> result = GetWorkerMoves(board, worker, isOpponent, isWorker1);
            for (int i = result.Count - 1; i >= 0; i--)
            {
                result[i].Item2.Type = ActionType.Hephaestus;

                (Board childBoard, Action childAction) = result[i];
                if (childBoard.GetElevation(childAction.Build) <= 2)
                {
                    Action secondBuildAction = new Action
                    {
                        Type = ActionType.Hephaestus,
                        Move = childAction.Move,
                        Build = childAction.Build,
                        IsOpponent = childAction.IsOpponent,
                        IsWorker1 = childAction.IsWorker1,
                        SecondBuild = true
                    };
                    Board newBoard = new Board(board, secondBuildAction);
                    result.Add((newBoard, secondBuildAction));
                }
            }
            return result;
        }

        private static List<(Board, Action)> GetAtlasWorkerMoves(Board board, Coord worker, bool isOpponent, bool isWorker1)
        {
            List<(Board, Action)> result = GetWorkerMoves(board, worker, isOpponent, isWorker1);
            for (int i = result.Count - 1; i >= 0; i--)
            {
                result[i].Item2.Type = ActionType.Atlas;

                (Board childBoard, Action childAction) = result[i];
                if (childBoard.GetElevation(childAction.Build) < Board.MaxHeight)
                {
                    Action domeAction = new Action
                    {
                        Type = ActionType.Atlas,
                        Move = childAction.Move,
                        Build = childAction.Build,
                        IsOpponent = childAction.IsOpponent,
                        IsWorker1 = childAction.IsWorker1,
                        AtlasDome = true
                    };
                    Board newBoard = new Board(board, domeAction);
                    result.Add((newBoard, domeAction));
                }
            }
            return result;
        }
    }
}