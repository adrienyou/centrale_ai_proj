using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    public class State
    {
        private Map _Map; // ref to the Map
        private List<Cell> _HumanCells { get; set; }
        private List<Cell> _VampireCells { get; set; }
        private List<Cell> _WerewolvesCells { get; set; }
        private Hashtable _Cells; // store ref to cells based on their coordinates on the map
      
        public State(Map map, List<Cell> humanCells, List<Cell> vampireCells, List<Cell> werewolvesCells)
        {
            // Create a reference to the map in order to have access to height & width
            _Map = map;

            _HumanCells = humanCells;
            _VampireCells = vampireCells;
            _WerewolvesCells = werewolvesCells;
            _Cells = new Hashtable();
        }

        public void Update(int read, string buffer)
        {
            // Update the whole state with the UPD order received from the server
            // TO DO
        }

        public void UpdateCell(int x, int y, CellType cellType, int pop)
        {
            Position pos = new Position(x, y);
            string pos_str = pos.ToString();

            if (_Cells.ContainsKey(pos_str))
            {
                // The destination cell already exists
                Cell cell = _Cells[pos_str];
                cell.Update(pop, cellType);
               // TO DO: if the type has changed, remove the cell from the list 
               // it belonged to and add it to the right list
            }
            else
            {
                // The destination cell does not exist yet
                Cell cell = new Cell(x, y, cellType, pop);
                _Cells.Add(pos_str, cell);
                // TO DO: add the cell in the right list
            }
        }

        public void Move(Position pos_from, Position pos_to, int pop)
        { 
            // Move "pop" units from cell in position "pos_from" to cell in position "pos_to"
            Cell cellFrom = _Cells[pos_from.ToString()];
            cellFrom.Move(-pop, cellFrom.Type);

            if (_Cells.ContainsKey(pos_to.ToString()))
            {
                // The destination cell already exists
                Cell cellTo = _Cells[pos_to.ToString()];
                cellTo.Move(pop, cellFrom.Type);
            }
            else
            {
                // The destination cell does not exist yet
                this.UpdateCell(pos_to.X, pos_to.Y, cellFrom.Type, pop);
            } 
        }

        // Accessors
        public Map Map
        {
            get { return _Map; }
        }
    }
}
