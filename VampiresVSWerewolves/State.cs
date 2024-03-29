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
        private double _Proba; // (optional) if the state has a given proba to occur in case of a fight
      
        public State(Map map)
        {
            // Initialize an empty state
            // Create a reference to the map in order to have access to height & width
            _Map = map;

            _HumanCells = new List<Cell>();
            _VampireCells = new List<Cell>();
            _WerewolvesCells = new List<Cell>();

            _Cells = new Hashtable();
            _Proba = 0;
        }

        public void FirstUpdate(int read, byte[] buffer, Map map, Position myHomePosition)
        {
            // Update the whole state with the UPD order received from the server
            for (int i = 0; i < read / 5; i++)
            {
                int x = buffer[5 * i + 0];
                int y = buffer[5 * i + 1];
                int humans = buffer[5 * i + 2];
                int vampires = buffer[5 * i + 3];
                int werewolves = buffer[5 * i + 4];

                // If test pass, it means that we are on our home, so we get the pop and deduce our type
                if (x == myHomePosition.X && y == myHomePosition.Y)
                {
                    if (vampires > 0)
                    {
                        map.FriendlyType = CellType.Vampires;
                    }
                    else
                    {
                        map.FriendlyType = CellType.Werewolves;
                    }
                }

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
            }
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

                    // Because we don't have an _EmptyCells list
                    if (newType != CellType.Empty)
                    {
                        GetCells(newType).Add(cell);
                    }
                    // If newType == Empty, we have to remove the cell from the Cells hash
                    else
                    {
                        _Cells.Remove(pos_str);
                    }
                }
                else
                {
                    if (pop == 0)
                    {
                        GetCells(oldType).Remove(cell);
                    }
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

        public List<State> Move(Move move, CellType type) 
        {
            // Return the list of states generated by this move
            // Can return several states because of the fights that give several results...
            // Create deep copies of the given state

            List<State> states = new List<State>();
            Cell cellFrom = (Cell)_Cells[move.PosFrom.Stringify()];

            // If the target cell contains nothing
            if (!_Cells.ContainsKey(move.PosTo.Stringify()))
            {
                State new_state = this.DeepCopy();
                new_state.UpdateCell(move.PosFrom.X, move.PosFrom.Y, type, cellFrom.Pop - move.Pop);
                new_state.UpdateCell(move.PosTo.X, move.PosTo.Y, type, move.Pop);
                states.Add(new_state);
            }
            else {
                Cell cell = (Cell)_Cells[move.PosTo.Stringify()];

                // If the target cell contains units from the same type
                if (cell.Type == type)
                {
                    State new_state = this.DeepCopy();
                    new_state.UpdateCell(move.PosFrom.X, move.PosFrom.Y, type, cellFrom.Pop - move.Pop);
                    new_state.UpdateCell(move.PosTo.X, move.PosTo.Y, type, cell.Pop + move.Pop);
                    states.Add(new_state);
                }
                else
                {
                    // If the target cell contains Humans
                    if (cell.Type == CellType.Humans)
                    {
                        // If humans are less numerous, they are all converted
                        if (cell.Pop <= move.Pop)
                        {
                            State new_state = this.DeepCopy();
                            new_state.UpdateCell(move.PosFrom.X, move.PosFrom.Y, type, cellFrom.Pop - move.Pop);
                            new_state.UpdateCell(move.PosTo.X, move.PosTo.Y, type, cell.Pop + move.Pop);
                            states.Add(new_state);
                        }
                        else
                        {
                            int S = cell.Pop + move.Pop;
                            double p = move.Pop / cell.Pop; // Proba of winning / surviving
 
                            // Cases when we win the fight (humans are converted)
                            // We apply the probabilty to survive
                            for (int i = 0; i <= S; i++)
                            {
                                State new_state = this.DeepCopy();
                                new_state.UpdateCell(move.PosFrom.X, move.PosFrom.Y, type, cellFrom.Pop - move.Pop);
                                new_state.UpdateCell(move.PosTo.X, move.PosTo.Y, type, i);

                                // Compute the probability of this state
                                // TO DO: improve the computation (very heavy...)
                                double p_state = p * Factoriel(S)/(Factoriel(S-i)*Factoriel(i)) * Math.Pow(p, i) * Math.Pow(1-p, S-i);
                                new_state.Proba = p_state;

                                states.Add(new_state);
                            }

                            // Cases when we lose the fight (humans win)
                            // We apply the probabilty to survive
                            for (int i = 0; i <= cell.Pop; i++)
                            {
                                State new_state = this.DeepCopy();
                                new_state.UpdateCell(move.PosFrom.X, move.PosFrom.Y, type, cellFrom.Pop - move.Pop);
                                new_state.UpdateCell(move.PosTo.X, move.PosTo.Y, type, i);

                                // Compute the probability of this state
                                // TO DO: improve the computation (very heavy...)
                                double p_state = (1 - p) * Factoriel(cell.Pop) / (Factoriel(cell.Pop - i) * Factoriel(i)) * Math.Pow(p, i) * Math.Pow(1 - p, cell.Pop - i);
                                new_state.Proba = p_state;

                                states.Add(new_state);
                            }
                        }
                    }
                    // If the target cell contains Ennemies
                    else
                    {
                        // If we are 1.5x more numerous than ennemies
                        if (cell.Pop <= 1.5*move.Pop)
                        {
                            State new_state = this.DeepCopy();
                            new_state.UpdateCell(move.PosFrom.X, move.PosFrom.Y, type, cellFrom.Pop - move.Pop);
                            new_state.UpdateCell(move.PosTo.X, move.PosTo.Y, type, move.Pop);
                            states.Add(new_state);
                        }
                        else
                        {
                            State new_state = this.DeepCopy();
                            new_state.UpdateCell(move.PosFrom.X, move.PosFrom.Y, type, cellFrom.Pop - move.Pop);
                            new_state.UpdateCell(move.PosTo.X, move.PosTo.Y, type, move.Pop);

                            // TO DO compute the right probability rule for the fights with ennemies...
                            // For now just use this simple rule...
                            double p_state = Math.Abs(1 - move.Pop/cell.Pop);
                            new_state.Proba = p_state;
                            states.Add(new_state);
                        }
                    }
                }
            }

            return states;
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
            copy.Proba = _Proba;
            foreach (Cell cell in _Cells.Values)
            {
                copy.UpdateCell(cell.Position.X, cell.Position.Y, cell.Type, cell.Pop);
            }
            return copy;
        }

        public static double Factoriel(int n)
        {
            if (n > 1) return n * Factoriel(n - 1);
            else return 1;
        }

        // Sort list of cells from closest to centerCell to furthest
        public static void sortCellsByDistance(Cell centerCell, List<Cell> cells)
        { 
            cells.Sort((a, b) => a.distanceToCell(centerCell).CompareTo(b.distanceToCell(centerCell))); 
        }

        public int HumanProximity()
        {
            int totalCount = 0;
            foreach (Cell hCell in _HumanCells)
            {
                int iv = 0;
                int iw = 0;
                int dv = 0;
                int dw = 0;
                int countV = 0;
                int countW = 0;

                State.sortCellsByDistance(hCell, _VampireCells);
                State.sortCellsByDistance(hCell, _WerewolvesCells);

                while (countV < hCell.Pop && iv < _VampireCells.Count)
                {
                    Cell vCell = _VampireCells[iv];
                    countV += vCell.Pop;
                    dv = hCell.distanceToCell(vCell);
                    iv++;
                }
                while (countW < hCell.Pop && iw < _WerewolvesCells.Count)
                {
                    Cell wCell = _WerewolvesCells[iw];
                    countW += wCell.Pop;
                    dw = hCell.distanceToCell(wCell);
                    iw++;
                }
                // If our race is likely to win, we add the number of humans to the total count
                bool nobodyWins = false;
                CellType winnerType = CellType.Vampires;
                if (countV >= hCell.Pop) {
                    if (countW < hCell.Pop)
                    {
                        winnerType = CellType.Vampires;
                    }
                    else
                    {
                        if (dv < dw) {
                            winnerType = CellType.Vampires;
                        } else if (dv > dw) {
                            winnerType = CellType.Werewolves;
                        } else {
                            nobodyWins = true;
                        }
                    }
                } else {
                    if (countW >= hCell.Pop)
                    {
                        winnerType = CellType.Werewolves;
                    }
                    else
                    {
                        nobodyWins = true;
                    }
                }
                if (nobodyWins == false)
                {
                    if (_Map.FriendlyType == winnerType)
                    {
                        totalCount += hCell.Pop;
                    }
                    else
                    {
                        totalCount -= hCell.Pop;
                    }
                }
            }
            return totalCount;
        }

        public int evalScore()
        {
            int score = 0;

            // Count numbers of friends and enemies
            int sumUs = 0;
            foreach (Cell friend in this.GetFriendlyCells()) {
                sumUs += friend.Pop;
            }
            int sumThem = 0;
            foreach (Cell ennemy in this.GetEnnemyCells())
            {
                sumThem += ennemy.Pop;
            }
            int sumHumans = 0;
            foreach (Cell human in _HumanCells)
            {
                sumHumans += human.Pop;
            }

            int f1 = sumUs - sumThem;  // nb ennemies vs. nb our monsters
            int w1 = 10;  // weight

            // Who is the more likely to convert humans
            int f2 = this.HumanProximity();
            int w2 = 5;

            // We won because we have more monsters !
            bool a3 = sumUs >= 1.5 * (sumThem + sumHumans);
            int f3 = a3 ? 1 : 0;  // convert boolean to int
            int w3 = 1000;


            // We lost because ennemies have more monsters
            bool a4 = sumThem >= 1.5 * (sumUs + sumHumans);
            int f4 = a4 ? 1 : 0;  // convert boolean to int
            int w4 = -1000;


            // We won because all ennemies were killed !
            bool a5 = sumThem == 0;
            int f5 = a5 ? 1 : 0;  // convert boolean to int
            int w5 = 100000;


            // We lost because all our monsters were killed
            bool a6 = sumUs == 0;
            int f6 = a6 ? 1 : 0;  // convert boolean to int
            int w6 = -100000;

            score = f1 * w1 + f2 * w2 + f3 * w3 + f4 * w4 + f5 * w5 + f6 * w6;
            return score;
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

        public Double Proba
        {
            get { return _Proba; }
            set
            {
                _Proba = value;
            }
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

        // Return the list of friendly cells
        public List<Cell> GetFriendlyCells()
        {
            return GetCells(_Map.FriendlyType);
        }

        // Return the list of friendly cells
        public List<Cell> GetEnnemyCells()
        {
            if (_Map.FriendlyType == CellType.Vampires)
            {
                return _WerewolvesCells;
            }
            else
            {
                return _VampireCells;
            }
        }

    }
}
