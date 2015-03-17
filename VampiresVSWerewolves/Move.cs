using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    public class Move : System.Object
    {
        private Position _PosFrom;
        private Position _PosTo;
        private int _Pop;

        public Move(Position PosFrom, Position PosTo, int Pop)
        {
            _PosFrom = PosFrom;
            _PosTo = PosTo;
            _Pop = Pop;
        }

        public Position PosFrom
        {
            get { return _PosFrom; }
            set
            {
                _PosFrom = value;
            }
        }

        public Position PosTo
        {
            get { return _PosTo; }
            set
            {
                _PosTo = value;
            }
        }

        public int Pop
        {
            get { return _Pop; }
            set
            {
                _Pop = value;
            }
        }

        public byte[] convertToOrder()
        {
            int[] ints = new int[5] {_PosFrom.X, _PosFrom.Y, _Pop, _PosTo.X, _PosTo.Y};
            byte[] byteArray = new byte[5];
            for (int i = 0; i < ints.Length; i++)
            {
                byteArray[i] = BitConverter.GetBytes(ints[i])[0];
            }
            return byteArray;
        }

        public static byte[] convertToByteArray(List<Move> moves)
        {
            byte[] byteArray = new byte[5 * moves.Count];

            foreach (Move move in moves)
            {
                int[] ints = new int[5] { move.PosFrom.X, move.PosFrom.Y, move.Pop, move.PosTo.X, move.PosTo.Y };
                for (int i = 0; i < ints.Length; i++)
                {
                    byteArray[i] = BitConverter.GetBytes(ints[i])[0];
                }
            }

            return byteArray;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Move move = obj as Move;
            if ((System.Object)move == null)
            {
                return false;
            }

            return (_PosFrom.X == move.PosFrom.X && _PosFrom.Y == move.PosFrom.Y && _PosTo.X == move.PosTo.X && _PosTo.Y == move.PosTo.Y && _Pop == move.Pop);
        }

        public bool Equals(Move move)
        {
            // If parameter is null return false:
            if ((object)move == null)
            {
                return false;
            }

            return (_PosFrom.X == move.PosFrom.X && _PosFrom.Y == move.PosFrom.Y && _PosTo.X == move.PosTo.X && _PosTo.Y == move.PosTo.Y && _Pop == move.Pop);
        }

        public override int GetHashCode() 
        {
            return Convert.ToInt32(String.Format("{0}{1}{2}{3}", 50 + _PosFrom.X, 50 + _PosFrom.Y, 50 + _PosTo.X, 50 + _PosTo.Y));
        }

    }
}
