using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    public enum CellState {Empty, Humans, Vampires, Werewolves}

    public class Cell
    {
        private Position _Position { get; set; }
        private int _Pop { get; set; }
        private CellState _State { get; set; }
        

        public Cell(int x, int y, CellState state, int pop)
        {
            _Position = new Position(x, y);
            _Pop = pop;
            _State = state;
        }        
    }
}
