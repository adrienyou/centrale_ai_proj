using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    public class Position
    {
        private int _X;
        private int _Y;

        public Position(int x, int y)
        {
            _X = x;
            _Y = y;
        }

        public static bool IsEqual(Position p1, Position p2)
        {
            return (p1._X == p2._X) && (p1._Y == p2._Y);
        }

        public bool isValid(Map map)
        {
            return 0 <= _X && _X < map.Width && 0 <= Y && _Y < map.Height;
        }

        public string Stringify()
        {
            return string.Format("{0},{1}", _X, _Y);
        }

        // Fields accessors
        public int X
        {
            get { return _X; }
        }

        public int Y
        {
            get { return _Y; }
        }


        public int distance (cellA, cellB) //Calcule le nombre de coups min nécessaires pour aller de cellA à cellB
                                           // ne tient pas compte des obstacles éventuels sur le chemin
        { 
            int xA = ;
            int yA = ;
            int xB = ;
            int yB = ;

            int distanceAB = Math.Max( Math.Abs(xB - xA) , Math.Abs(yB - yA) ) ;

            return distanceAB;
        }
    }
}
