using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VampiresVSWerewolves;

namespace UnitTesting
{
    /// <summary>
    /// Summary description for StateTest
    /// </summary>
    [TestClass]
    public class StateTest
    {

         /// <summary>
        /// Test to check that the list of cells is sorted the right way.
        /// </summary>
        [TestMethod]
        public void sortCellsByDistanceToCenterCell_Test()
        {
            Cell centerCell = new Cell(0, 0, CellType.Humans, 10);
            Cell vampCell1 = new Cell(1, 1, CellType.Vampires, 3);
            Cell vampCell2 = new Cell(2, 2, CellType.Vampires, 2);
            List<Cell> cells = new List<Cell> {vampCell2, vampCell1};

            State.sortCellsByDistance(centerCell, cells);
            Assert.AreEqual(cells[0], vampCell1);

        }

        [TestMethod]
        public void State_Test()
        {
            Map map = new Map(12, 12, CellType.Vampires);
            State state = new State(map);
            Cell centerCell = new Cell(0, 0, CellType.Humans, 4);
            Cell vCell =  new Cell(0, 1, CellType.Vampires, 1);
            Cell wCell = new Cell(0, 2, CellType.Werewolves, 4);

            Assert.AreEqual(centerCell.distanceToCell(vCell) + 1, centerCell.distanceToCell(wCell));

            // Humans
            state.UpdateCell(2, 3, CellType.Humans, 4);
            state.UpdateCell(10,5, CellType.Humans, 3);
            // Vampires
            state.UpdateCell(4, 3, CellType.Vampires, 14);
            state.UpdateCell(3, 5, CellType.Vampires, 4);
            state.UpdateCell(2, 7, CellType.Vampires, 2);
            state.UpdateCell(9, 7, CellType.Vampires, 1);
            // Werewolves
            state.UpdateCell(2, 6, CellType.Werewolves, 3);
            state.UpdateCell(11, 8, CellType.Werewolves, 4);

            Assert.AreEqual(1, state.evalScore());

        }

        /// <summary>
        /// Test to check that the properties are correctly set when the State object is instantiated.
        /// </summary>
        [TestMethod]
        public void UpdateCell_Test()
        {
            Map map = new Map(10, 10, CellType.Vampires);
            State state = new State(map);

            List<State> empty_list = new List<State>();
            Assert.AreEqual(empty_list, state.GetCells(CellType.Humans));
            Assert.AreEqual(empty_list, state.GetCells(CellType.Vampires));
            Assert.AreEqual(empty_list, state.GetCells(CellType.Werewolves));

            Position pos = new Position(2, 3);
            state.UpdateCell(2, 3, CellType.Humans, 6);

            Cell cell = (Cell)state.Cells[pos.Stringify()];
            Assert.AreEqual(6, cell.Pop);
            Assert.AreEqual(2, cell.Position.X);
            Assert.AreEqual(3, cell.Position.Y);
        }
    }
}
