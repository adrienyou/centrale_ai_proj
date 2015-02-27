using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    public class Map
    {
        private int _Height { get; set; }
        private int _Width { get; set; }
        private List<Cell> _HumanCells { get; set; }
        private List<Cell> _VampireCells { get; set; }
        private List<Cell> _WerewolvesCells { get; set; }
        
        //private Cell[][] _Matrix { get; set; }

        /*public Map(int height, int width)
        {
            //First set the values of the parameters
            _Height = height;
            _Width = width;

            //Then instantiate the array of array of cells
            _Matrix = new Cell[height][];
            for (int r = 0; r < height; ++r)
            {
                _Matrix[r] = new Cell[width];
            }
        }
        */

    }
}
