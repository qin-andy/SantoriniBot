using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantoriniBot
{
    class Bot
    {
        private static int Leaves;

        public static (double, Action) GetAction(Board board)
        {
            double score = 0;
            Action action = null;
            for (int depth = 5; depth < 10; depth++)
            {
                Leaves = 0;
                (score, action) = Minimax(board, 5, Double.NegativeInfinity, Double.PositiveInfinity, false);
                Console.WriteLine($"Leaves: {Leaves}");
                if (Leaves > 100000)
                {
                    break;
                } else
                {
                    Console.WriteLine($"Deepening: {depth + 1}");
                }
            }
            return (score, action);
        }

        public static (double, Action) Minimax(Board board, int depth, double alpha, double beta, bool isOpponent, int index = 0)
        {
            if (depth == 0 || BoardEvaluator.IsFinished(board))
            {
                Leaves++;
                return (BoardEvaluator.Evaluate(board), null);
            }

            if (!isOpponent)
            {
                double maxEval = Double.NegativeInfinity;
                Action maxAction = null;
                int childIndex = 0;
                List<(Board, Action)> childBoards = 
                    MoveGenerator.GetNextBoards(board, false).OrderByDescending(item => BoardEvaluator.Evaluate(item.Item1)).ToList();
                foreach ((Board childBoard, Action childAction) in childBoards)
                {
                    (double eval, Action action) = Minimax(childBoard, depth - 1, alpha, beta, true, childIndex);
                    childIndex++;
                    if (eval > maxEval)
                    {
                        maxEval = eval;
                        maxAction = childAction;
                    }
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return (maxEval, maxAction);
            } else
            {
                double minEval = Double.PositiveInfinity;
                Action minAction = null;
                int childIndex = 0;
                List<(Board, Action)> childBoards = 
                    MoveGenerator.GetNextBoards(board, true).OrderBy(item => BoardEvaluator.Evaluate(item.Item1)).ToList();
                foreach ((Board childBoard, Action childAction) in MoveGenerator.GetNextBoards(board, true))
                {
                    (double eval, Action action) = Minimax(childBoard, depth - 1, alpha, beta, false, childIndex);
                    childIndex++;
                    if (eval < minEval)
                    {
                        minEval = eval;
                        minAction = childAction;
                    }
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return (minEval, minAction);
            }
        }
    }
}
