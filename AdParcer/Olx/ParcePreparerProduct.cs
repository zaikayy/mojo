using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdParcer.Olx
{
    public class PreparerProductDecorator : IEnumerable<AdPrepared1>
    {
        private readonly IEnumerable<AdOlxDirty> _enumerator;
        public IEnumerator<AdPrepared1> GetEnumerator()
        {
            foreach (var o in _enumerator)
            {
                yield return Preparer1Ad.Prepare(o);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public PreparerProductDecorator(IEnumerable<AdOlxDirty> enumerator) 
        {
            _enumerator = enumerator;
        }
    }
}
