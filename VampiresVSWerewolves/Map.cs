using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    class Map
    {
        private int Height { get; set; }
        private int Width { get; set; }
        private Cell[][] Matrix { get; set; }

        public Map(int height, int width)
        {
            //First set the values of the parameters
            Height = height;
            Width = width;

            //Then instantiate the array of array of cells
            Matrix = new Cell[height][];
            for (int r = 0; r < height; ++r)
            {
                Matrix[r] = new Cell[width];
            }
        }

        public void ModifyMap()
        {

        }
    }
}
