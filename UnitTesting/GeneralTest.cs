using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtificialIntelligence.Tree;
using VampiresVSWerewolves;
using System.Collections.Generic;

namespace UnitTesting
{
    [TestClass]
    public class GeneralTests
    {
        /// <summary>
        /// Test if the isValid method works properly
        /// </summary>
        [TestMethod]
        public void PositionValid_Test()
        {
            // Map(height, width, celltype);
            Map map = new Map(5, 10, CellType.Werewolves);

            Position goodPos = new Position(3, 4);
            Position goodPos2 = new Position(0, 0);

            Position badPos = new Position(0, 5);
            Position badPos2 = new Position(10, 2);

            Position badPos3 = new Position(15, 20);

            Assert.AreEqual<bool>(true, goodPos.isValid(map));
            Assert.AreEqual<bool>(true, goodPos2.isValid(map));
            Assert.AreEqual<bool>(false, badPos.isValid(map));
            Assert.AreEqual<bool>(false, badPos2.isValid(map));
            Assert.AreEqual<bool>(false, badPos3.isValid(map));
        }

        /// <summary>
        /// Test about the generation of random moves
        /// </summary>
        [TestMethod]
        public void RandomMoves_Test()
        {
            Engine engine = new Engine();
            Map map = new Map(10, 10, CellType.Werewolves);
            State state = new State(map);
            state.UpdateCell(3, 4, CellType.Werewolves, 10);

            List<Move> moves = engine.RandomSuccessor(state);
        }
    }
}
