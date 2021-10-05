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
            int depth = 5;
            double score = 0;
            Action action = null;
/*           
     for (int depth = 3; depth < 10; depth++)
    {
        Console.WriteLine("Exploring depth: " + depth);
        Leaves = 0;
        (score, action) = Minimax(board, depth, Double.NegativeInfinity, Double.PositiveInfinity, false);
        Console.WriteLine($"Leaves: {Leaves} " + (action == null));
        if (Leaves > 50000)
        {
            break;
        }
    }*/

            Console.WriteLine("Exploring depth: " + depth);
            Leaves = 0;
            (score, action) = Minimax(board, depth, Double.NegativeInfinity, Double.PositiveInfinity, false);
            Console.WriteLine($"Leaves: {Leaves} " + (action == null));
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
                if (childBoards.Count == 0)
                {
                    return (-1, null);
                }
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
                if (childBoards.Count == 0)
                {
                    return (1, null);
                }
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
