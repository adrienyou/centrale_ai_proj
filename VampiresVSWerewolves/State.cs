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
      
        public State(Map map)
        {
            // Initialize an empty state
            // Create a reference to the map in order to have access to height & width
            _Map = map;

            _HumanCells = new List<Cell>();
            _VampireCells = new List<Cell>();
            _WerewolvesCells = new List<Cell>();

            _Cells = new Hashtable();
        }

        public void Update(int read, byte[] buffer)
        {
            // Update the whole state with the UPD order received from the server
            for (int i = 0; i < read / 5; i++)
            {
                int x = buffer[5 * i + 0];
                int y = buffer[5 * i + 1];
                int humans = buffer[5 * i + 2];
                int vampires = buffer[5 * i + 3];
                int werewolves = buffer[5 * i + 4];

                CellType cellType = CellType.Empty;
                int pop = 0;
                if (humans > 0)
                {
                    cellType = CellType.Humans;
                    pop = humans;
                }
                else if (vampires > 0)
                {
                    cellType = CellType.Vampires;
                    pop = vampires;
                }
                else if (werewolves > 0)
                {
                    cellType = CellType.Werewolves;
                    pop = werewolves;
                }

                this.UpdateCell(x, y, cellType, pop);

                Console.WriteLine("X: " + Convert.ToString(buffer[5 * i + 0]));
                Console.WriteLine("Y: " + Convert.ToString(buffer[5 * i + 1]));
                Console.WriteLine("humains: " + Convert.ToString(buffer[5 * i + 2]));
                Console.WriteLine("vampires: " + Convert.ToString(buffer[5 * i + 3]));
                Console.WriteLine("loups: " + Convert.ToString(buffer[5 * i + 4]));
            }
        }

        public void UpdateCell(int x, int y, CellType cellType, int pop)
        {
            Position pos = new Position(x, y);
            string pos_str = pos.Stringify();

            if (_Cells.ContainsKey(pos_str))
            {
                // The destination cell already exists
                Cell cell = (Cell)_Cells[pos_str];
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

        public string getKey()
        {
            // Return a key (string) that uniquely identifies the State 
            // by using the position of all units.
            // Ex: if the state has only 8 humans on the cell (x=3, y=6)
            // the key would be ";3,6-8H;"
            string key = ";";

            for (int x = 0; x < this._Map.Width; x++)
            {
                for (int y = 0; y < this._Map.Height; y++)
                {
                    Position pos = new Position(x, y);
                    string pos_string = pos.Stringify();
                    if (_Cells.ContainsKey(pos_string))
                    {
                        Cell cell = (Cell)_Cells[pos_string];
                        key = key + cell.Stringify() + ";";
                    }
                }
            }

            return key;
        }

        // Accessors
        public Map Map
        {
            get { return _Map; }
        }

        public Hashtable Cells
        {
            get { return _Cells; }
        }

        // Functions needed for the evaluation function
        public int getCurrentNbEnnemies(cellType ennemyType)
        {
            return ennemyType.pop;
        }

        public int getCurrentNbOurMonsters(cellType ourType)
        {
            return ourType.pop;
        }

    }
}
