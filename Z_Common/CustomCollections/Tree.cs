using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z_Common.CustomCollections
{
    public class Tree<T>
    {
        private readonly List<Node<T>> _roots = new List<Node<T>>();

        public IEnumerable<Node<T>> Roots() { return _roots; }

        public void AddRoot(Node<T> root)
        {
            _roots.Add(root);
        }

        public static IEnumerable<Node<T>> GetEnumerator(IEnumerable<Node<T>> list)
        {
            foreach (Node<T> node in list)
            {
                yield return node;
                if (node.IsLeaf() == false)
                    foreach (Node<T> chaild in GetEnumerator(node.Children()))
                        yield return chaild;
            }
        }
        public static Node<T> GetRoot(Node<T> currentNode)
        {
            if (currentNode.Parent() != null)
                return GetRoot(currentNode.Parent());
            return currentNode;
        }
        public static bool HasChild(Node<T> childNode, Node<T> parentNode)
        {
            if (childNode == null)
                return false;
            if (childNode == parentNode)
                return true;
            return HasChild(childNode.Parent(), parentNode);
        }
        public static IEnumerable<Node<T>> GetParents(Node<T> node)
        {
            if (node == null)
                yield break;
            foreach (var parent in GetParents(node.Parent()))
                yield return parent;
            yield return node;
        }
        public static bool ExistValueInBranch<U>(Node<U> node, U value) where U : class
        {
            if (node == null)
                return false;
            if (node.Value == value)
                return true;
            return ExistValueInBranch(node.Parent(), value);
        }
        public static int BranchLength(Node<T> leafNode)
        {
            if (leafNode == null)
                return 0;
            return BranchLength(leafNode.Parent()) + 1;
        }
        public static IEnumerable<Node<T>> GetEnumerator(Node<T> node)
        {
            return GetEnumerator(node.Children());
        }
        public IEnumerable<Node<T>> GetEnumerator()
        {
            return GetEnumerator(Roots());
        }
    }

    public class Node<T>
    {
        private readonly List<Node<T>> _children = new List<Node<T>>();
        private Node<T> _parent;

        public Node(T value, Node<T> parent)
        {
            Value = value;
            _parent = parent;

            if (_parent != null)
            {
                _parent.AddChild(this);
            }
        }

        public T Value { get; set; }

        public IEnumerable<Node<T>> Children() { return _children; }

        public Node<T> Parent() { return _parent; }

        public bool IsRoot() { return _parent == null; }

        public bool IsLeaf() { return !_children.Any(); }

        public void AddChild(Node<T> node)
        {
            _children.Add(node);
            node._parent = this;
        }
    }
}
