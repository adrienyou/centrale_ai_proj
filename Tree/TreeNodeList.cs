using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialIntelligence.Tree
{
    public class TreeNodeList<T> : List<TreeNode<T>>
    {
        private TreeNode<T> _Parent;
 
        // Constructor
        public TreeNodeList(TreeNode<T> Parent)
        {
            _Parent = Parent;
        }

        // Add a new TreeNode to the list, set the right parent and return it.
        public new TreeNode<T> Add(TreeNode<T> Node)
        {
            base.Add(Node);
            Node.Parent = _Parent;
            Node.State = _Parent.State == NodeState.Max ? NodeState.Min : NodeState.Max;
            return Node;
        }

    }
}
