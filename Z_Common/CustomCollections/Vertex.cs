using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z_Common.CustomCollections
{
    public class Vertex<T>
    {
        public readonly T Value;
        public readonly List<Vertex<T>> Adjacents = new List<Vertex<T>>();
        public Vertex(T value)
        {
            Value = value;
        }
        public void AddAdjacents(params Vertex<T>[] vertices)
        {
            Adjacents.AddRange(vertices);
        }
    }
}
