using AdParcer.Olx.Entity;
using System.Collections.Generic;

namespace AdParcer.Olx.Comparators
{
    public class AdOlxDirtyComparer : IEqualityComparer<AdOlxDirty>
    {
        public bool Equals(AdOlxDirty ad1, AdOlxDirty ad2)
        {
            return ad1.Id.Equals(ad2.Id);
        }

        public int GetHashCode(AdOlxDirty ad)
        {
            int hCode = ad.Id; //ad.id ^ ad.href ^ ad.price ^ ad.title;
            // TODO: заменить на код
            return hCode.GetHashCode();
        }
    }
}
