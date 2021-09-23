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

            /*
            Board board = new Board
            {
                Worker1 = new Coord { X = 0, Y = 2 },
                Worker2 = new Coord { X = 3, Y = 3 },
                OpponentWorker1 = new Coord { X = 4, Y = 0 },
                OpponentWorker2 = new Coord { X = 1, Y = 3 },
                Cells = new int[,]
                {
                    { 0, 0, 1, 0, 0 },
                    { 1, 1, 0, 0, 0 },
                    { 0, 0, 0, 0, 0 },
                    { 2, 0, 0, 0, 0 },
                    { 2, 0, 0, 0, 0 }
                }
            };
            */

            board.Print();
            
            /*
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Console.WriteLine("\n----- Bot's Turn -----\n");
            (double eval, Action action) = Bot.GetAction(board);
            board.Update(action);
            board.Print();
            watch.Stop();
            Console.WriteLine($"Time: {watch.ElapsedMilliseconds}ms");
            */

            while (true)
            {
                PlayerAction(board);
                board.Print();

                Console.WriteLine("\n----- Bot's Turn -----\n");
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                (double eval, Action action) = Bot.GetAction(board);
                board.Update(action);
                board.Print();
                watch.Stop();
                Console.WriteLine($"Time: {watch.ElapsedMilliseconds}ms");
            }
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
            int workerX = worker.X + offsetX;
            int workerY = worker.Y + offsetY;
            if (!board.ValidCoord(workerX, workerY))
            {
                Console.WriteLine("CANNOT MOVE OFF BOARD");
                PlayerAction(board);
                return;
            }

            Console.Write("Build (NE,N,NW,W,SW,S,SE,E): ");
            string buildDirection = Console.ReadLine();
            (int buildOffsetX, int buildOffsetY) = Directions.DirectionToOffset(buildDirection);
            int buildX = workerX + buildOffsetX;
            int buildY = workerY + buildOffsetY;
            if (!board.ValidCoord(buildX, buildY))
            {
                Console.WriteLine("CANNOT BUILD OFF BOARD");
                PlayerAction(board);
                return;
            }

            if (isWorker1)
            {
                board.OpponentWorker1 = new Coord { X = workerX, Y = workerY };
            }
            else
            {
                board.OpponentWorker2 = new Coord { X = workerX, Y = workerY };
            }
            board.Cells[buildX, buildY] += 1;
        }
    }
}