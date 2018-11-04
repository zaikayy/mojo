using AdParcer.Rozetka.Entity;
using AdParcer.Rozetka.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdParcer.Rozetka
{
    public class RozetkaPreparerProductDecorator : IEnumerable<ProductPrepared1>
    {
        private readonly IEnumerable<ProductDirty> _enumerator;
        public IEnumerator<ProductPrepared1> GetEnumerator()
        {
            foreach (var o in _enumerator)
            {
                yield return Preparer1Product.Prepare(o);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public RozetkaPreparerProductDecorator(IEnumerable<ProductDirty> enumerator)
        {
            _enumerator = enumerator;
        }
    }
}
