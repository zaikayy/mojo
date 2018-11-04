using AdParcer.Olx.Entity;
using CommonClassLibrary;

namespace AdParcer.Olx.Helpers
{
    public class Preparer1Ad
    {
        public static AdPrepared1 Prepare(AdOlxDirty oldAd)
        {
            AdPrepared1 newAd = new AdPrepared1
            {
                Id = oldAd.Id,
                Title = oldAd.Title,
                Currency = ParcerCurrency.ParceCurrency(oldAd.PriceWithCurrency),
                Href = oldAd.Href,
                Location = oldAd.Location,
                Price = ParcerPrice.ParcePrice(oldAd.PriceWithCurrency)
            };
            return newAd;
        }
    }
    public static class AdHelper
    {
        public static string ToString(this AdPrepared1 ad)
        {
            return $"[AD \n\tId={ad.Id}, \n\tTitle={ad.Title}, \n\tHref={ad.Href}, \n\tPrice={ad.Price}{ad.Currency}, \n\tLocation={ad.Location}]";
        }
    }
}
