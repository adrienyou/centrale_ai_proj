using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    public enum NodeState { Max, Min }

    public class TreeNode<T>
    {
        private TreeNode<T> _Parent;
        private TreeNodeList<T> _Children;
        private T _Value;
        private List<Move> _Moves;
        private int _Profondeur;
        private NodeState _State;

        // Constructors for a TreeNode
        public TreeNode()
        {
            _Parent = null;
            _Children = new TreeNodeList<T>(this);
            _Profondeur = 0;
        }

        public TreeNode(T Value, List<Move> Moves, NodeState State)
        {
            _Value = Value;
            _Moves = Moves;
            _Parent = null;
            _Children = new TreeNodeList<T>(this);
            _Profondeur = 0;
            _State = State;
        }

        public TreeNode(T Value, List<Move> Moves, TreeNode<T> Parent)
        {
            _Value = Value;
            _Moves = Moves;
            _State = Parent.State == NodeState.Max ? NodeState.Min : NodeState.Max;
            _Parent = Parent;
            _Children = new TreeNodeList<T>(this);
            _Profondeur = Parent.Profondeur + 1;

            Parent.Children.Add(this);
        }
        
        // Fields accessors
        public TreeNode<T> Parent
        {
            get { return _Parent; }
            set
            {
                if (value == null)
                    return;
                _Parent = value; 
            }
        }

        public NodeState State
        {
            get { return _State; }
            set
            {
                if (value == null)
                    return;
                _State = value;
            }
        }

        public List<Move> Moves
        {
            get { return _Moves; }
            set
            {
                if (value == null)
                    return;
                _Moves = value;
            }
        }

        public TreeNodeList<T> Children
        {
            get { return _Children; }
            set
            {
                if (value == null)
                    return;

               _Children = value;
            }
        }

        public T Value
        {
            get { return _Value; }
            set
            {
                if (value == null && _Value == null)
                    return;

                if (value != null && _Value != null && value.Equals(_Value))
                    return;

                _Value = value;
            }
        }

        public int Profondeur
        {
            get { return _Profondeur; }
            set
            {
                if (value == null)
                    return;

                _Profondeur = value;
            }
        }
    }
}

