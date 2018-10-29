using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdParcer
{
    #region old   
    //   public class AdOlxDirty2
    //{
    //	public int id {get; set;}
    //	public string title {get; set;}
    //	public string href {get; set;}
    //	public double price { get { return ParcePrice(dirtPrice); } }
    //	private double ParcePrice(string sPrice)
    //	{
    //		string s = Regex.Match(sPrice, "[0-9]*[.,]?[0-9]+").Value;
    //		double price_ = 0;
    //		double.TryParse(s, out price_);
    //		return price_;
    //	}
    //	private string ParceCurrency(string sPrice)
    //	{			
    //		return Regex.Match(sPrice, "[^.\\d]\\D+").Value;
    //	}
    //	public string dirtPrice {get; set;}
    //	public string location {get; set;}
    //	public string currency { get { return ParceCurrency(dirtPrice); } }
    //	public AdOlxDirty2(int id_, string title_, string href_, string price_, string location_)
    //	{
    //		id = id_; title = title_; href = href_; dirtPrice = price_; location = location_;
    //	}
    //	public AdOlxDirty2() {}
    //	public override string ToString()
    //	{
    //		return string.Format("[AD \n\tId={0}, \n\tTitle={1}, \n\tHref={2}, \n\tPrice={3}{4}, \n\tLocation={5}]", id, title, href, price, currency, location);
    //	}
    //}
    #endregion
    [Serializable]
    public class AdPrepared1
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Href { get; set; }
        public double Price { get; set; }
        public string Location { get; set; }
        public string Currency { get; set; }
    }
    public class Preparer1Ad
    {
        private static string ParceCurrency(string sPrice)
        {
            return Regex.Match(sPrice, "[^.\\d]\\D+").Value;
        }
        private static double ParcePrice(string sPrice)
        {
            string s = Regex.Match(sPrice, "[0-9]*[.,]?[0-9]+").Value;
            double price_ = 0;
            double.TryParse(s, out price_);
            return price_;
        }
        public static AdPrepared1 Prepare(AdOlxDirty oldAd)
        {
            AdPrepared1 newAd = new AdPrepared1
            {
                Id = oldAd.Id,
                Title = oldAd.Title,
                Currency = ParceCurrency(oldAd.PriceWithCurrency),
                Href = oldAd.Href,
                Location = oldAd.Location,
                Price = ParcePrice(oldAd.PriceWithCurrency)
            };
            return newAd;
        }
    }
    public static class AdHelper
    {
        public static string ToStringFormat1(this AdPrepared1 ad)
        {
            return $"[AD \n\tId={ad.Id}, \n\tTitle={ad.Title}, \n\tHref={ad.Href}, \n\tPrice={ad.Price}{ad.Currency}, \n\tLocation={ad.Location}]";
        }
    }
    public class AdOlxDirty
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Href { get; set; }
        public string PriceWithCurrency { get; set; }
        public string Location { get; set; }
    }

    class AdOlxDirtyComparer : EqualityComparer<AdOlxDirty>
    {
        public override bool Equals(AdOlxDirty ad1, AdOlxDirty ad2)
        {
            return ad1.Id.Equals(ad2.Id);
        }

        public override int GetHashCode(AdOlxDirty ad)
        {
            int hCode = ad.Id; //ad.id ^ ad.href ^ ad.price ^ ad.title;
            // TODO: заменить на код
            return hCode.GetHashCode();
        }
    }
    public class AdPrepared1Comparer : EqualityComparer<AdPrepared1>
    {
        public override bool Equals(AdPrepared1 ad1, AdPrepared1 ad2)
        {
            bool equal = ad1.Id.Equals(ad2.Id);
            return equal;
        }

        public override int GetHashCode(AdPrepared1 ad)
        {
            int hCode = ad.Id; //ad.id ^ ad.href ^ ad.price ^ ad.title;
            // TODO: hCode.GetHashCode() заменить на код
            return hCode;
        }
    }
}
