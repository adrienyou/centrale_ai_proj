using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    public class Map
    {
        private int _Height;
        private int _Width;

        public Map(int height, int width)
        {
            _Height = height;
            _Width = width;
        }

        // Fields accessors
        public int Height
        {
            get { return _Height; }
        }

        public int Width
        {
            get { return _Width; }
        }
    }
}
