using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    public class Engine
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

            return null;
        }

        public HashSet<Move> getMoves(Cell cell, List<Cell> ennemyCells) {
            HashSet<Move> moves = new HashSet<Move>();
            foreach (Cell ennemyCell in ennemyCells)
            {
                int x = Math.Max(cell.Position.X + Math.Max(Math.Min(cell.Position.X, 1), -1), 0);
                int y = Math.Max(cell.Position.Y + Math.Max(Math.Min(cell.Position.Y, 1), -1), 0);
                Position posTo = new Position(x, y);
                Move move = new Move(cell.Position, posTo, cell.Pop);
                moves.Add(move);
            }

            return moves;
        }

        static IEnumerable<List<Move>> GetCombinations(IEnumerable<HashSet<Move>> lists, IEnumerable<Move> selectedMoves)
        {
            // At this point we have a list of possible moves for each of our groups of units.
            // We need to generate all the possible states using these moves.
            // To do that we have to compute all the combinations of moves between all our groups.
            // For performance concerns, this is a generator. It yields a list of moves.

            if (lists.Any())
            {
                var remainingLists = lists.Skip(1);
                foreach (var item in lists.First().Where(x => !selectedMoves.Contains(x)))
                    foreach (var combo in GetCombinations(remainingLists, selectedMoves.Concat(new List<Move>{item})))
                        yield return combo;
            }
            else
            {
                yield return selectedMoves.ToList();
            }
        }

        public List<Move> RandomSuccessor(State state)
        {
            // We get the list of our cells
            List<Cell> cells = state.GetFriendlyCells();

            // The moves list returned
            List<Move> moves = new List<Move>();

            foreach(Cell cell in cells)
            {

                // We loop while the random position is not a valid position
                bool isNewPositionOK = false;
                while(!isNewPositionOK)
                {
                    Random rand = new Random();
                    int ToX = rand.Next(3);
                    int ToY = rand.Next(3);

                    if (ToX == 2) { ToX = -1; };
                    if (ToY == 2) { ToY = -1; };
                    // Avoid not moving
                    if (ToX == 0 && ToY == 0) { ToX = 1; };

                    Position randomPosition = new Position(ToX + cell.Position.X, ToY + cell.Position.Y);

                    if (randomPosition.isValid(state.Map))
                    {
                        // The move is all the pop, from old cell to new cell
                        Move randomMove = new Move(cell.Position, randomPosition, cell.Pop);

                        moves.Add(randomMove);

                        isNewPositionOK = true;
                    }
                }
            }

            return moves;
        }
    }
}
