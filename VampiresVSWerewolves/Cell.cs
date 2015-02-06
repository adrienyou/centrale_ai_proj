using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    public enum CellState {Empty, Humans, Vampires, Werewolves}

    class Cell
    {
        private Position Position { get; set; }
        private int Pop { get; set; }
        private CellState State { get; set; }
        

        public Cell(int x, int y, CellState state, int pop)
        {
            Position = new Position(x, y);
            Pop = pop;
            State = state;
        }        
    }
}
