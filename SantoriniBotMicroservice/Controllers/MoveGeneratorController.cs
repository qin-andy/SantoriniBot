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
        [HttpGet]
        public string Get()
        {
            Board board = new Board
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
}

