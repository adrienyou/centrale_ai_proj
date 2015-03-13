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
        private CellType _Type;

        public Cell(int x, int y, CellType type, int pop)
        {
            _Position = new Position(x, y);
            _Pop = pop;
            _Type = type;
        }

        public CellType Update(int pop, CellType type)
        {
            // Update the cell type and population
            _Pop = pop;
            _Type = type;

            return type;
        }

        public string Stringify()
        {
            return string.Format("{0},{1}-{2}{3}", _Position.X, _Position.Y, _Pop, _Type.ToString()[0]);
        }

        // Fields accessors
        public CellType Type
        {
            get { return _Type; }
        }

        public int Pop
        {
            get { return _Pop; }
        }
    }
}
