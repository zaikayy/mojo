using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public abstract class StorageAdapter<T> where T : class
    {
        public abstract IEnumerable<T> GetValues();
        public abstract void AddRange(List<T> list);
    }
}
