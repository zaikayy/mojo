using AdParcer.Rozetka.Entity;
using CommonClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdParcer.Rozetka.Helpers
{
    public class Preparer1Product
    {
        public static ProductPrepared1 Prepare(ProductDirty productDirty)
        {
            ProductPrepared1 newAd = new ProductPrepared1
            {
                Id = productDirty.Id,
                Title = productDirty.Title,
                Currency = ParcerCurrency.ParceCurrency(productDirty.PriceWithCurrency),
                Href = productDirty.Href,
                Category = productDirty.Category,
                Price = ParcerPrice.ParcePrice(productDirty.PriceWithCurrency)
            };
            return newAd;
        }
    }
    public static class ProductHelper
    {
        public static string ToString(this ProductPrepared1 product)
        {
            return $"[AD \n\tId={product.Id}, \n\tTitle={product.Title}, \n\tHref={product.Href}, \n\tPrice={product.Price}{product.Currency}, \n\tCategory={product.Category}]";
        }
    }
}
