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
    public class CellTest
    {


        /// <summary>
        /// Test to check that the properties are correctly set when the Cell object is instantiated.
        /// </summary>
        [TestMethod]
        public void distanceToCell_Test()
        {
            Cell cell1 = new Cell(0, 0, CellType.Humans, 10);
            Cell cell2 = new Cell(1, 10, CellType.Werewolves, 7);

            Assert.AreEqual(cell1.distanceToCell(cell2), 10);

        }
    }
}
