using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z_Common.CustomCollections;

namespace Z_Common
{
    public static class Converter<T>
    {
        //public static long ObjectCount = 0;
        //public static int LimitLength = 10;
        //public static Action<Node<T>, int> MyAction;
        public static IEnumerable<Node<T>> VertexGetNodeEnumerator((Node<Vertex<T>> nodeVertex, Node<T> node) parentNode)
        {
            int l = Tree<T>.BranchLength(parentNode.node);
            //if (l > LimitLength && MyAction != null)
            //{
            //    MyAction(parentNode.node, l);
            //}
            foreach (var v in parentNode.nodeVertex.Value.Adjacents)
            {
                // разорвать зацикливание
                if (Tree<Vertex<T>>.ExistValueInBranch(parentNode.nodeVertex, v))
                    continue;

                //ObjectCount++;
                (Node<Vertex<T>> nodeVertex, Node<T> node) node = (new Node<Vertex<T>>(v, parentNode.nodeVertex), new Node<T>(v.Value, parentNode.node));

                yield return node.node;
                foreach (var adjacent in VertexGetNodeEnumerator(node))
                    yield return adjacent;
            }
        }
        public static IEnumerable<Node<T>> VertexToNodeEnumerator(Vertex<T> vertex)
        {
            var vertexNode = (new Node<Vertex<T>>(vertex, null), new Node<T>(vertex.Value, null));
            return VertexGetNodeEnumerator(vertexNode);
        }
        public static Tree<T> VertexToTree(Vertex<T> vertex)
        {
            (Node<Vertex<T>> nodeVertex, Node<T> node) vertexNode = (new Node<Vertex<T>>(vertex, null), new Node<T>(vertex.Value, null));
            VertexGetNodeEnumerator(vertexNode).ToList(); // отложенная инициализация - связать ноды 
            Tree<T> tree = new Tree<T>();
            tree.AddRoot(vertexNode.node);
            return tree;
        }
        public static IEnumerable<IEnumerable<T>> TreeToTable(Tree<T> tree)
        {
            var leafs = tree.GetEnumerator().Where(a => a.IsLeaf()).ToList();
            var result = new List<IEnumerable<T>>();
            leafs.ForEach(a => result.Add(Tree<T>.GetParents(a).Select(n => n.Value)));
            return result;
        }
    }
}
