using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantoriniBot
{
    class BoardEvaluator
    {
        public static double Evaluate(Board board)
        {
            if (IsLoss(board))
            {
                return -1;
            }

            if (IsWin(board))
            {
                return 1;
            }

            double score = board.GetElevation(board.Worker1) + board.GetElevation(board.Worker2)
                - board.GetElevation(board.OpponentWorker1) - board.GetElevation(board.OpponentWorker2);
            return score / 6;
        }

        public static bool IsWin(Board board)
        {
            return board.GetElevation(board.Worker1) == 3 || board.GetElevation(board.Worker2) == 3;
        }

        public static bool IsLoss(Board board)
        {
            return board.GetElevation(board.OpponentWorker1) == 3 || board.GetElevation(board.OpponentWorker2) == 3;
        }

        public static bool IsFinished(Board board)
        {
            return IsWin(board) || IsLoss(board);
        }
    }
}