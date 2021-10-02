using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SantoriniBotMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoveGeneratorController : ControllerBase
    {
        [HttpGet]
        public int Get()
        {
            return 3;
        }
    }
}
