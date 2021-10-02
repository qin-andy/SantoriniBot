using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantoriniBot
{
    public enum ActionType { Base, Hephaestus, Atlas };

    public class Coord
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object other)
        {
            if ((other == null) || !GetType().Equals(other.GetType()))
            {
                return false;
            }
            return other.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return Y * Board.Size + X;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    public class Action
    {
        public ActionType Type = ActionType.Base;
        public Coord Worker { get; set; } = null;
        public Coord Move { get; set; }
        public Coord Build { get; set; }
        public bool IsOpponent { get; set; }
        public bool IsWorker1 { get; set; }

        public bool SecondBuild = false;
        public bool AtlasDome = false;

        public override string ToString()
        {
            string s = "Move Coord: " + Move +
                "\nBuild Coord: " + Build;
            return s;
        }
    }

    public class Board
    {
        public const int Size = 5;
        public const int MaxHeight = 4;

        public int[,] Cells;
        public Coord Worker1;
        public Coord Worker2;
        public Coord OpponentWorker1;
        public Coord OpponentWorker2;

        public int GetElevation(Coord coord)
        {
            return Cells[coord.X, coord.Y];
        }

        public int GetElevation(int x, int y)
        {
            return Cells[x, y];
        }

        public Board ()
        {
            Cells = new int[Size, Size];
        }

        public Board (Board oldBoard, Action action)
        {
            Cells = (int[,])oldBoard.Cells.Clone();
            Worker1 = oldBoard.Worker1;
            Worker2 = oldBoard.Worker2;
            OpponentWorker1 = oldBoard.OpponentWorker1;
            OpponentWorker2 = oldBoard.OpponentWorker2;

            Update(action);
        }

        public void Update(Action action)
        {
            if (action.Build == null)
            {
                Console.WriteLine("Action build null");
            }
            if (Cells == null)
            {
                Console.WriteLine("CElls null");
            }
            Cells[action.Build.X, action.Build.Y] += 1;
            
            switch (action.Type)
            {
                case ActionType.Hephaestus:
                    if (action.SecondBuild)
                    {
                        Cells[action.Build.X, action.Build.Y] += 1;
                    }
                    break;
                case ActionType.Atlas:
                    if (action.AtlasDome)
                    {
                        Cells[action.Build.X, action.Build.Y] = Board.MaxHeight;
                    }
                    break;
            }

            if (action.IsOpponent)
            {
                if (action.IsWorker1)
                {
                    OpponentWorker1 = action.Move;
                }
                else
                {
                    OpponentWorker2 = action.Move;
                }
            }
            else
            {
                if (action.IsWorker1)
                {
                    Worker1 = action.Move;
                }
                else
                {
                    Worker2 = action.Move;
                }
            }
        }

        public void Print()
        {
            string[,] workerStrings = new string[Size, Size];
            workerStrings[Worker1.X, Worker1.Y] = "X";
            workerStrings[Worker2.X, Worker2.Y] = "Y";
            workerStrings[OpponentWorker1.X, OpponentWorker1.Y] = "A";
            workerStrings[OpponentWorker2.X, OpponentWorker2.Y] = "B";

            for (int y = 0; y < Size; y++)
            {
                string str = "";
                for (int x = 0; x < Size; x++)
                {
                    str += Cells[x, y];
                    str += workerStrings[x, y] != null ? workerStrings[x, y] : " ";
                    str += " ";
                }
                Console.WriteLine(str);
                Console.WriteLine();
            }
        }

        public bool ValidCoord(int x, int y)
        {
            return x >= 0 && x < Size && y >= 0 && y < Size;
        }

        public bool ValidCoord(Coord coord)
        {
            return ValidCoord(coord.X, coord.Y);
        }

        public bool IsOccupied(Coord coord)
        {
            return coord.Equals(Worker1) || coord.Equals(Worker2) || coord.Equals(OpponentWorker1) || coord.Equals(OpponentWorker2);
        }
    }
}