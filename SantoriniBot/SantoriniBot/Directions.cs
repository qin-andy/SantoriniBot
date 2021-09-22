using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantoriniBot
{
    class Directions
    {
        public static (int, int)[] NeighborOffsets = { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };

        public static (int, int) DirectionToOffset(string direction)
        {
            switch(direction)
            {
                case "N":
                    return (0, 1);
                case "NE":
                    return (1, 1);
                case "E":
                    return (1, 0);
                case "SE":
                    return (1, -1);
                case "S":
                    return (0, -1);
                case "SW":
                    return (-1, -1);
                case "W":
                    return (-1, 0);
                case "NW":
                    return (-1, 1);
            }
            throw new InvalidOperationException("Unrecognized direction");
        }
    }
}
