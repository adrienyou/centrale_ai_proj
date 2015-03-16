using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VampiresVSWerewolves;
using System.Collections.Generic;

namespace UnitTesting
{
    [TestClass]
    public class TreeTests
    {
        /// <summary>
        /// Test to check that the properties are correctly set when the TreeNode object is instantiated.
        /// </summary>
        [TestMethod]
        public void EmptyConstr_Test()
        {
            TreeNode<int> t_empty = new TreeNode<int>();

            Assert.IsNull(t_empty.Parent);
            Assert.IsInstanceOfType(t_empty.Children, typeof(TreeNodeList<int>));
            Assert.AreEqual<int>(0, t_empty.Profondeur);            
        }
        /// <summary>
        /// Test to check that the Move properties of the object is instantiated.
        /// </summary>
        [TestMethod]
        public void Move_Test()
        {
            Position posFrom = new Position(0, 0);
            Position posTo = new Position(1, 1);
            Move move = new Move(posFrom, posTo, 1);

            Assert.AreEqual<int>(move.PosFrom.X, 0);
            Assert.AreEqual<int>(move.Pop, 1);

            byte[] byteArrayOfXFrom = move.convertToOrder();
            Console.WriteLine(byteArrayOfXFrom);
        }
        /// <summary>
        /// Test to check that the properties are correctly set when the TreeNode object is instantiated.
        /// </summary>
        [TestMethod]
        public void ValueConstr_Test()
        {
            int value = 5;

            Position posFrom = new Position(0, 0);
            Position posTo = new Position(1, 1);
            Move move = new Move(posFrom, posTo, 1);
            List<Move> moves = new List<Move>();
            moves.Add(move);

            NodeState state = NodeState.Min;
            TreeNode<int> t_value = new TreeNode<int>(value, moves, state);

            Assert.IsNull(t_value.Parent);
            Assert.IsInstanceOfType(t_value.Children, typeof(TreeNodeList<int>));
            Assert.AreEqual<int>(0, t_value.Profondeur);
            Assert.AreEqual<int>(value, t_value.Value);
            Assert.AreEqual<NodeState>(state, t_value.State);
        }

        /// <summary>
        /// Test to check that the properties are correctly set when the TreeNode object is instantiated.
        /// </summary>
        [TestMethod]
        public void FullConstr_Test()
        {
            int value = 5;
            Position posFrom = new Position(0, 0);
            Position posTo = new Position(1, 1);
            Move move = new Move(posFrom, posTo, 1);
            List<Move> moves = new List<Move>();
            moves.Add(move);

            // Nodeconstructed as root --> with State
            TreeNode<int> t_racine = new TreeNode<int>(value, moves, NodeState.Max);
            // Node constructed with parent --> State is inferred
            TreeNode<int> t_full = new TreeNode<int>(value, moves, t_racine);

            Assert.IsNotNull(t_full.Parent);
            Assert.AreEqual<TreeNode<int>>(t_racine, t_full.Parent);
            Assert.IsInstanceOfType(t_full.Children, typeof(TreeNodeList<int>));
            Assert.AreEqual<int>(t_racine.Profondeur + 1, t_full.Profondeur);
            Assert.AreEqual<int>(value, t_full.Value);
            Assert.AreNotEqual<NodeState>(t_racine.State, t_full.State);

            //Create an array of the children, and take the first one.
            TreeNode<int> [] a_racine = t_racine.Children.ToArray();
            TreeNode<int> first_node = a_racine[0];

            Assert.AreEqual<TreeNode<int>>(first_node, t_full);
        }
    }
}
