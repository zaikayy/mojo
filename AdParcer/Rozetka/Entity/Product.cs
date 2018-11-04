using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdParcer.Rozetka.Entity
{
    [Serializable]
    public class ProductPrepared1
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Href { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
    }
    public class ProductDirty
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Href { get; set; }
        public string PriceWithCurrency { get; set; }
        public string Category { get; set; }
    }
}
