using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantoriniBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board
            {
                Worker1 = new Coord { X = 1, Y = 1 },
                Worker2 = new Coord { X = 3, Y = 3 },
                OpponentWorker1 = new Coord { X = 3, Y = 1 },
                OpponentWorker2 = new Coord { X = 1, Y = 3 },
                Cells = new int[,]
                {
                    { 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0 }
                }
            };
            /*Board board = new Board
            {
                Worker1 = new Coord { X = 0, Y = 0 },
                Worker2 = new Coord { X = 2, Y = 2 },
                OpponentWorker1 = new Coord { X = 2, Y = 1 },
                OpponentWorker2 = new Coord { X = 1, Y = 0 },
                Cells = new int[,]
                {
                    { 0, 4, 1, 0, 0 },
                    { 0, 4, 2, 0, 0 },
                    { 1, 2, 0, 0, 0 },
                    { 0, 2, 0, 0, 0 },
                    { 0, 0, 2, 0, 0 }
                }
            };*/
            board.Print();

            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            Console.WriteLine("\n----- Bot's Turn -----\n");
            Action action = MoveGenerator.GetNextAction(board);
            board.Update(action);
            board.Print();
            watch.Stop();

            Console.WriteLine($"Time: {watch.ElapsedMilliseconds}ms");
            /*
            while (true)
            {
                PlayerAction(board);
                board.Print();

                Console.WriteLine("\n----- Bot's Turn -----\n");
                Action action = MoveGenerator.GetNextAction(board);
                board.Update(action);
                board.Print();
            }
            */
            Console.ReadKey();
        }

        private static void PlayerAction(Board board)
        {
            Console.Write("Move worker 1? (y/n): ");
            bool isWorker1 = Console.ReadLine() == "y";

            Console.Write("Movement (NE,N,NW,W,SW,S,SE,E): ");
            string moveDirection = Console.ReadLine();
            (int offsetX, int offsetY) = Directions.DirectionToOffset(moveDirection);
            Coord worker = isWorker1 ? board.OpponentWorker1 : board.OpponentWorker2;
            worker.X += offsetX;
            worker.Y += offsetY;
            if (!board.ValidCoord(worker))
            {
                throw new InvalidOperationException("Cannot move off board");
            }
            if (isWorker1)
            {
                board.OpponentWorker1 = worker;
            } else
            {
                board.OpponentWorker2 = worker;
            }

            Console.Write("Build (NE,N,NW,W,SW,S,SE,E): ");
            string buildDirection = Console.ReadLine();
            (int buildOffsetX, int buildOffsetY) = Directions.DirectionToOffset(buildDirection);
            int buildX = worker.X + buildOffsetX;
            int buildY = worker.Y + buildOffsetY;
            if (!board.ValidCoord(buildX, buildY))
            {
                throw new InvalidOperationException("Cannot build off board");
            }
            board.Cells[buildX, buildY] += 1;
        }
    }
}