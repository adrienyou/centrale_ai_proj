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
        private int _EnnemyPop { get; set; }
        private CellType _EnnemyType { get; set; }

        public Cell(int x, int y, CellType type, int pop)
        {
            _Position = new Position(x, y);
            _Pop = pop;
            _Type = type;
            _EnnemyPop = 0;
            _EnnemyType = null;
        }

        public CellType Update(int pop, CellType type)
        {
            // Update the cell type and population
            _Pop = pop;
            _Type = type;
            _EnnemyPop = 0;
            _EnnemyType = null;

            return CellType;
        }

        public void Move(int pop, CellType type)
        {
            // Move "pop" units of type "type" from/to this cell
            // If pop > 0 => units are arriving in the cell
            // If pop < 0 => units are leaving the cell
            if (type == _Type)
            {
                _Pop += pop;
            }
            else 
            {
                _EnnemyType = type;
                _EnnemyPop += pop;
            }
        }

        public CellType Fight()
        {
            // If ennemies are on the cell, perform the fight.
            // Depending on the result of the fight, the cell may change type.
            // Return the cell type after the fight.
            if (_Type == CellType.Humans)
            {
                if (_EnnemyPop >= 1.5 * _Pop)
                {
                    // Humans are converted into the ennemies
                    // The type of the cell changes
                    return this.Update(_Pop + _EnnemyPop, _EnnemyType);
                }
                else
                {
                    // TO DO
                    // Probability fight
                }
            }
        }

        // Fields accessors
        public CellType Type
        {
            get { return _Type; }
        }
    }
}
