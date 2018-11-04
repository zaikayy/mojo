using System;

namespace AdParcer.Olx.Entity
{
    [Serializable]
    public class AdPrepared1
    {
        public int Id { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
    }
    public class AdOlxDirty
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Href { get; set; }
        public string PriceWithCurrency { get; set; }
        public string Location { get; set; }
    }
}
