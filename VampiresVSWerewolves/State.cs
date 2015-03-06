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
      
        public State(Map map, List<Cell> humanCells, List<Cell> vampireCells, List<Cell> werewolvesCells)
        {
            // Create a reference to the map in order to have access to height & width
            _Map = map;

            _HumanCells = humanCells;
            _VampireCells = vampireCells;
            _WerewolvesCells = werewolvesCells;
        }

        public void UpdateCells(List<Cell> vampireCells, List<Cell> werewolvesCells)
        {
            // Update the entire list of Vampires
            if (vampireCells != null) {
                _VampireCells = vampireCells;
            }

            // Update the entire list of Werevolves
            if (werewolvesCells != null)
            {
                _WerewolvesCells = werewolvesCells;
            }

            // Check if some fights should happen after the moves


        }

        // Accessors
        public Map Map
        {
            get { return _Map; }
        }
    }
}
