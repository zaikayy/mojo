using AdParcer.Rozetka.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdParcer.Rozetka.Comparators
{
    public class ProductDirtyComparer : IEqualityComparer<ProductDirty>
    {
        public bool Equals(ProductDirty x, ProductDirty y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(ProductDirty obj)
        {
            string hCode = obj.Id; //ad.id ^ ad.href ^ ad.price ^ ad.title;
            // TODO: заменить на код
            return hCode.GetHashCode();
        }
    }
}
