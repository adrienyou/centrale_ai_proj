using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    class Position
    {
        private int X { get; set; }
        private int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool IsEqual(Position p1, Position p2)
        {
            return (p1.X == p2.X) && (p1.Y == p2.Y);
        }
    }
}
