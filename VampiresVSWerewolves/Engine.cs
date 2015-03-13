using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    class Engine
    {
        // Class contenant les méthodes du moteur de décision du jeu

        public Engine()
        {

        }

        public List<State> Successor(State state, CellType type) 
        {
            // Copy the parent in order to use it to generate all successors
            

            CellType ennemyType = CellType.Vampires;
            if (type == CellType.Vampires)
            {
                ennemyType = CellType.Werewolves;
            }

            List<Cell> friendCells = state.GetCells(type);
            List<Cell> ennemyCells = state.GetCells(ennemyType);

        }

        public HashSet<Move> getMoves(Cell cell, List<Cell> ennemyCells) {
            HashSet<Move> moves = new HashSet<Move>();
            foreach (Cell ennemyCell in ennemyCells)
            {
                int x = Math.Max(cell.Position.X + Math.Max(Math.Min(cell.Position.X, 1), -1), 0);
                int y = Math.Max(cell.Position.Y + Math.Max(Math.Min(cell.Position.Y, 1), -1), 0);
                Position pos = new Position(x, y);
                Move move = new Move(cell.Position.X, cell.Position.Y, cell.Pop, x, y);
                moves.Add(move);
            }

            return moves;
        }
    }
}
