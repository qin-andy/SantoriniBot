using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantoriniBot
{
    class MoveGenerator
    {
        public static Action GetNextAction(Board board)
        {
            MinMaxTreeNode root = new MinMaxTreeNode(board, null, 4, false);

            double maxScore = -2;
            MinMaxTreeNode maxChild = null;
            foreach (MinMaxTreeNode child in root.Children)
            {
                double score = GetMinMaxScore(child);
                if (score > maxScore)
                {
                    maxScore = score;
                    maxChild = child;
                }
            }

            Console.WriteLine($"Leaves: {root.LeafCount()}");
            return maxChild.Action;
        }

        private static double GetMinMaxScore(MinMaxTreeNode node)
        {
            if (node.Children.Count == 0)
            {
                return BoardEvaluator.Evaluate(node.Board);
            }
            double score = GetMinMaxScore(node.Children[0]);
            for (int i = 1; i < node.Children.Count; i++)
            {
                if (node.IsOpponent)
                {
                    score = Math.Min(score, GetMinMaxScore(node.Children[i]));
                } else
                {
                    score = Math.Max(score, GetMinMaxScore(node.Children[i]));
                }
            }
            return score;
        }
    }
}
