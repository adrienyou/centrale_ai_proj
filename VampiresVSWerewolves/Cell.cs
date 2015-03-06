using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    public enum CellType {Empty, Humans, Vampires, Werewolves}

    public class Cell
    {
        private Position _Position { get; set; }
        private int _Pop { get; set; }
        private CellType _Type { get; set; }
        

        public Cell(int x, int y, CellType type, int pop)
        {
            _Position = new Position(x, y);
            _Pop = pop;
            _Type = type;
        }        
    }
}
