using AdParcer.Rozetka.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdParcer.Rozetka.Comparators
{
    public class ProductPrepared1Comparer : IEqualityComparer<ProductPrepared1>
    {
        public bool Equals(ProductPrepared1 x, ProductPrepared1 y)
        {
            bool equal = x.Id.Equals(y.Id);
            return equal;
        }

        public int GetHashCode(ProductPrepared1 obj)
        {
            string hCode = obj.Id; //ad.id ^ ad.href ^ ad.price ^ ad.title;
            // TODO: hCode.GetHashCode() заменить на код
            return hCode.GetHashCode();
        }
    }
}
