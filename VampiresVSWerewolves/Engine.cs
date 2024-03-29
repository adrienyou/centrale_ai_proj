﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    public class Engine
    {
        // This class contains the main game engine methods (like alphabeta algorithm)
        public Engine() {}

        public IEnumerable<Tuple<List<Move>, State>> Successor(State state, CellType type) 
        {
            // Get all successors of a given state
            CellType ennemyType = CellType.Vampires;
            if (type == CellType.Vampires)
            {
                ennemyType = CellType.Werewolves;
            }

            List<Cell> friendCells = state.GetCells(type);
            List<Cell> ennemyCells = state.GetCells(ennemyType);
            List<Cell> humanCells = state.GetCells(CellType.Humans);

            // List of all possible moves for each of our goup of units
            List<HashSet<Move>> listOfMoves = new List<HashSet<Move>>();

            foreach (Cell cell in friendCells) {
                listOfMoves.Add(getMoves(cell, ennemyCells, humanCells, state.Map));
            }

            foreach (List<Move> moves in GetCombinations(listOfMoves, new List<Move>())) {
                // We have a list of move, we can now generate de state en yield it to the alphabeta method
                foreach (State childState in GenerateStates(state, moves, type)) {
                    Tuple<List<Move>, State> result = new Tuple<List<Move>, State>(moves, childState);
                    yield return result;
                }
            }
        }

        public HashSet<Move> getMoves(Cell cell, List<Cell> ennemyCells, List<Cell> humanCells, Map map) 
        {
            // For a given group of units, determine all the possible moves given our 3 kinds of missions:
            // "attack", "convert", "escape"
            HashSet<Move> moves = new HashSet<Move>();
            foreach (Cell ennemyCell in ennemyCells)
            {
                List<Position> positions = getMoveTo(cell.Position, ennemyCell.Position, map);

                foreach (Position positionAtt in positions)
                {
                    Move moveAtt = new Move(cell.Position, positionAtt, cell.Pop);                   
                    moves.Add(moveAtt);

                    Position positionDef = new Position(-positionAtt.X, -positionAtt.Y);
                    if(positionDef.isValid(map))
                    {
                        Move moveDef = new Move(cell.Position, positionDef, cell.Pop);
                        moves.Add(moveDef);
                    }
                }
            }

            foreach (Cell humanCell in humanCells)
            {
                List<Position> positions = getMoveTo(cell.Position, humanCell.Position, map);

                foreach (Position positionHum in positions)
                {
                    Move moveHum = new Move(cell.Position, positionHum, cell.Pop);
                    moves.Add(moveHum);

                }
            }

            return moves;
        }

        // Calculate the List<Position> where a should go to get close to b (either modifying x, modifying y or modifying both)
        public static List<Position> getMoveTo(Position a, Position b, Map map)
        {
            List<Position> positions = new List<Position>();

            int xTo = a.X;
            int yTo = a.Y;

            if (a.X < b.X)
            {
                xTo = a.X + 1;
            }
            else if (a.X > b.X)
            {
                xTo = a.X - 1;
            }

            if (a.Y < b.Y)
            {
                yTo = a.Y + 1;
            }
            else if (a.Y > b.Y)
            {
                yTo = a.Y - 1;
            }

            // Position posX = new Position(xTo, a.Y);
            // Position posY = new Position(a.X, yTo);
            Position posXY = new Position(xTo, yTo);

            /* if (posX.isValid(map))
            {
                positions.Add(posX);
            }
            if (posY.isValid(map))
            {
                positions.Add(posY);
            }
             */
            if (posXY.isValid(map))
            {
                positions.Add(posXY);
            }

            return positions;
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

        public List<State> GenerateStates(State currentState, List<Move> moves, CellType type) {
            // We a set of Moves, this function yields all States that can be created with those moves
            // starting from the current state.
            // type is the CellType of the units we are moving (our type)
            List<State> states = new List<State>();
            for (int i = 0; i < moves.Count; i++) {
                Move move = moves[i];
                // moves.Reverse<Move>().Take(moves.Count-1);
                if (moves.Count == 1)
                {
                    return currentState.Move(move, type);
                }
                else
                {
                    foreach (State state in currentState.Move(move, type)) {
                        // List<Move> leftMoves = moves.Reverse<Move>().Take(moves.Count - 1);
                        List<Move> leftMoves = moves.GetRange(1, moves.Count - 1);
                        states.AddRange(GenerateStates(state, leftMoves, type));
                    }
                    return states;
                }
            }
            return states;
        }

        public Tuple<int, TreeNode<State>> AlphaBeta(int depth, int alpha, int beta, Hashtable tree, TreeNode<State> parentNode, CellType currentPlayer)
        {
            // AlphaBeta algorithm (NegaMax)

            State parentState = parentNode.Value;
            TreeNode<State> bestTurn = null;

            if (parentNode.Value.GetEnnemyCells().Count == 0 || depth <= 0)
            {
                if (parentState.Proba > 0)
                {
                    return new Tuple<int, TreeNode<State>>(Convert.ToInt32(parentState.Proba * parentState.Proba * Convert.ToDouble(parentState.evalScore())), bestTurn); // evaluationScore
                }
                else
                {
                    return new Tuple<int, TreeNode<State>>(parentState.evalScore(), bestTurn); // evaluationScore
                }
            }

            foreach (Tuple<List<Move>, State> successorResult in Successor(parentState, currentPlayer))
            {
                List<Move> moves = successorResult.Item1;
                State state = successorResult.Item2;

                TreeNode<State> childNode = new TreeNode<State>(state, moves, parentNode);

                // Save node in the global tree
                // Should be used later on to avoid recomputing nodes that have been done before
                if (!tree.ContainsKey(childNode.Value.getKey()))
                {
                    tree.Add(childNode.Value.getKey(), childNode);
                }

                CellType nextPlayer = CellType.Vampires;
                if (currentPlayer == CellType.Vampires) {
                    nextPlayer = CellType.Werewolves;
                }

                Tuple<int, TreeNode<State>> r = AlphaBeta(depth - 1, -beta, -alpha, tree, childNode, nextPlayer);
                int score = -r.Item1;

                if (score >= alpha)
                {
                    alpha = score;
                    bestTurn = childNode;
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
            }
            return new Tuple<int, TreeNode<State>>(alpha, bestTurn);
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
