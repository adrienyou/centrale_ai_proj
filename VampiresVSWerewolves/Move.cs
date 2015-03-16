using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    public class Move
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
    }
}
