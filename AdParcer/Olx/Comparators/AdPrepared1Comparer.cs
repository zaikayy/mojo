using AdParcer.Olx.Entity;
using System.Collections.Generic;

namespace AdParcer.Olx.Comparators
{
    public class AdPrepared1Comparer : IEqualityComparer<AdPrepared1>
    {
        public bool Equals(AdPrepared1 ad1, AdPrepared1 ad2)
        {
            bool equal = ad1.Id.Equals(ad2.Id);
            return equal;
        }

        public int GetHashCode(AdPrepared1 ad)
        {
            int hCode = ad.Id; //ad.id ^ ad.href ^ ad.price ^ ad.title;
            // TODO: hCode.GetHashCode() заменить на код
            return hCode;
        }
    }
}
