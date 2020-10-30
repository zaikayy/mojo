using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Z_Common.CustomCollections
{
    public class Graph<T>
    {
        public readonly List<Vertex<T>> Vertices = new List<Vertex<T>>();
        public Graph(params Vertex<T>[] vertices)
        {
            Vertices.AddRange(vertices);
        }
        public IEnumerable<Vertex<T>> GetEnumerator()
        {
            return GetEnumerator(Vertices);
        }
        public static IEnumerable<Vertex<T>> GetEnumerator(IEnumerable<Vertex<T>> list)
        {
            foreach (var v in list)
            {
                yield return v;
                foreach (var adjacent in GetEnumerator(v.Adjacents))
                    yield return adjacent;
            }
        }
        public static IEnumerable<Vertex<T>> GetEnumerator(Vertex<T> start, Vertex<T> finish)
        {
            foreach (var v in start.Adjacents)
            {
                bool existFinish = false;
                foreach (var adjacent in GetEnumerator(v, finish))
                {
                    existFinish = true;
                    yield return adjacent;
                }

                if (v == finish || existFinish)
                    yield return v;
            }
        }
    }
}
