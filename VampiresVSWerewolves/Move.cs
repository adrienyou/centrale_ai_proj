using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    public class Move
    {
        private int _XFrom;
        private int _YFrom;
        private int _Pop;
        private int _XTo;
        private int _YTo;

        public Move(int x, int y, int n, int xx, int yy)
        {
            _XFrom = x;
            _YFrom = y;
            _Pop = n;
            _XTo = xx;
            _YTo = yy;
        }

        public int XFrom
        {
            get { return _XFrom; }
            set
            {
                _XFrom = value;
            }
        }

        public int YFrom
        {
            get { return _YFrom; }
            set
            {
                _YFrom = value;
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

        public int XTo
        {
            get { return _XTo; }
            set
            {
                _XTo = value;
            }
        }

        public int YTo
        {
            get { return _YTo; }
            set
            {
                _YTo = value;
            }
        }

        public byte[] convertToOrder()
        {
            int[] ints = new int[5] {_XFrom, _YFrom, _Pop, _XTo, _YTo};
            byte[] byteArray = new byte[5];
            for (int i = 0; i < ints.Length; i++)
            {
                byteArray[i] = BitConverter.GetBytes(ints[i])[0];
            }
            return byteArray;
        }
    }
}
