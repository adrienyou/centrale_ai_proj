using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    public class Position
    {
        private int _X { get; set; }
        private int _Y { get; set; }

        public Position(int x, int y)
        {
            _X = x;
            _Y = y;
        }

        public static bool IsEqual(Position p1, Position p2)
        {
            return (p1._X == p2._X) && (p1._Y == p2._Y);
        }
    }
}
