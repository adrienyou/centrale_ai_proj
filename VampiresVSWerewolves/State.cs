using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    class State
    {
        private Map _Map; // ref to the Map
        private List<Cell> _HumanCells { get; set; }
        private List<Cell> _VampireCells { get; set; }
        private List<Cell> _WerewolvesCells { get; set; }
      
        public State(Map map)
        {
            // Create a reference to the map in order to have access to height & width
            _Map = map;
        }

        // Accessors
        public Map Map
        {
            get { return _Map; }
        }
    }
}
