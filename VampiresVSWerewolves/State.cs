﻿using System;
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
                CellType oldType = cell.Type;
                CellType newType = cell.Update(pop, cellType);

                // If the type has changed, remove the cell from the list 
                // it belonged to and add it to the right list
                if (newType != oldType)
                {
                    GetCells(oldType).Remove(cell);
                    GetCells(newType).Add(cell);
                } 
            }
            else
            {
                // The destination cell does not exist yet
                Cell cell = new Cell(x, y, cellType, pop);
                _Cells.Add(pos_str, cell);
                // Add the cell in the list it belongs to
                GetCells(cellType).Add(cell);
            }
        }

        public void Move(Move move, CellType type) 
        {
            // Return the list of states generated by this move
            // Can return several states because of the fights that give several results...

            Position posTo = new Position(move.XTo, move.YTo);

            // If the target cell contains nothing
            if (!_Cells.ContainsKey(posTo.Stringify()))
            {
                this.UpdateCell(move.XTo, move.YTo, type, move.Pop);
            }
            else {
                Cell cell = (Cell)_Cells[posTo.Stringify()];

                // If the target cell contains units from the same type
                if (cell.Type == type)
                {
                    this.UpdateCell(posTo.X, posTo.Y, type, cell.Pop+move.Pop);
                }
                else
                {
                    // If the target cell contains Humans
                    if (cell.Type == CellType.Humans)
                    {
                        // If humans are less numerous, they are all converted
                        if (cell.Pop <= move.Pop)
                        {
                            this.UpdateCell(posTo.X, posTo.Y, type, cell.Pop + move.Pop);
                        }
                        else
                        {
                            // TO DO: fight, proba
                        }
                    }
                    // If the target cell contains Ennemies
                    else
                    {
                        // If we are 1.5x more numerous than ennemies
                        if (cell.Pop <= 1.5*move.Pop)
                        {
                            this.UpdateCell(posTo.X, posTo.Y, type, move.Pop);
                        }
                        else
                        {
                            // TO DO: fight, proba
                        }
                    }
                }
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

        public State DeepCopy()
        {
            State copy = new State(_Map);
            foreach (Cell cell in _Cells.Values)
            {
                copy.UpdateCell(cell.Position.X, cell.Position.Y, cell.Type, cell.Pop);
            }
            return copy;
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

        public List<Cell> GetCells(CellType type) {
            if (type == CellType.Humans) {
                return _HumanCells;
            }
            else if (type == CellType.Vampires) 
            {
                return _VampireCells;
            }
            else if (type == CellType.Werewolves)
            {
                return _WerewolvesCells;
            }
            return null;
        }
    }
}
