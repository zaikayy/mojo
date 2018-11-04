using AdParcer.Olx.Entity;
using AdParcer.Olx.Helpers;
using System.Collections;
using System.Collections.Generic;

namespace AdParcer.Olx
{
    public class OlxPreparerProductDecorator : IEnumerable<AdPrepared1>
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

        public OlxPreparerProductDecorator(IEnumerable<AdOlxDirty> enumerator) 
        {
            _enumerator = enumerator;
        }
    }
}
