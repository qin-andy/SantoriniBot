using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SantoriniBot;

namespace SantoriniBotMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoveGeneratorController : ControllerBase
    {
        [HttpPost]
        public string Post(BoardData boardData)
        {
            /*            Board board = new Board
                        {
                            Worker1 = new Coord { X = 0, Y = 1 },
                            Worker2 = new Coord { X = 2, Y = 2 },
                            OpponentWorker1 = new Coord { X = 1, Y = 1 },
                            OpponentWorker2 = new Coord { X = 3, Y = 3 },
                            Cells = new int[,]
                            {
                                { 2, 0, 0, 3, 0 },
                                { 3, 0, 0, 3, 0 },
                                { 3, 2, 0, 2, 0 },
                                { 0, 2, 3, 0, 0 },
                                { 0, 0, 0, 0, 0 }
                            }
                        };

                        {
                            "Worker1": { "X": 0, "Y": 1 },
                            "Worker2": { "X": 2, "Y": 2 },
                            "OpponentWorker1": { "X": 1, "Y": 1 },
                            "OpponentWorker2": { "X": 3, "Y": 3 },
                            "Cells": 
                            [
                                [ 2, 0, 0, 3, 0 ],
                                [ 3, 0, 0, 3, 0 ],
                                [ 3, 2, 0, 2, 0 ],
                                [ 0, 2, 3, 0, 0 ],
                                [ 0, 0, 0, 0, 0 ]
                            ]
                        }
             */

            int[,] cells = new int[5,5];
            for (int i = 0; i < 5; i++)
            {
                List<int> list = boardData.Cells[i];
                for (int j = 0; j < 5; j++)
                    cells[i, j] = list[j];
            }

            Board board = new Board
            {
                Worker1 = new Coord { X = 0, Y = 1 },
                Worker2 = new Coord { X = 2, Y = 2 },
                OpponentWorker1 = new Coord { X = 1, Y = 1 },
                OpponentWorker2 = new Coord { X = 3, Y = 3 },
                Cells = cells
            };

            Console.WriteLine(board.Worker1);
            Console.WriteLine(board.Worker2);

            (double eval, SantoriniBot.Action action) = Bot.GetAction(board);
            if (action.IsWorker1)
            {
                if (action.IsOpponent)
                {
                    action.Worker = board.OpponentWorker1;
                }
                else
                {
                    action.Worker = board.Worker1;
                }
            } 
            else
            {
                if (action.IsOpponent)
                {
                    action.Worker = board.OpponentWorker2;
                }
                else
                {
                    action.Worker = board.Worker2;
                }
            }
            string json = JsonSerializer.Serialize(action);
            Console.WriteLine(action);
            Console.WriteLine(json);
            return json;
        }
    }

    public class BoardData
    {
        public Coord Worker1 { get; set; }
        public Coord Worker2 { get; set; }
        public Coord OpponentWorker1 { get; set; }
        public Coord OpponentWorker2 { get; set; }
        public List<List<int>> Cells { get; set; }
    }
}

