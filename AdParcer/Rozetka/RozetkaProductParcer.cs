using AdParcer.Rozetka.Entity;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdParcer.Rozetka
{
    public class RozetkaProductParcer : Parcer, IEnumerable<ProductDirty>
    {
        private HtmlDocument _doc;
        private ICPath _initPath;
        private ICPath _currentPath;

        private void RemoveOffset(HtmlDocument doc)
        {
            HtmlNodeCollection nodesToRemove = doc.DocumentNode.SelectNodes(".//table[@class='fixed offers breakword no-results-table']");

            if (nodesToRemove != null)
                foreach (HtmlNode node in nodesToRemove)
                    node.Remove();
        }

        public IEnumerable<ProductDirty> Get()
        {
            Select(_currentPath);
            HtmlNodeCollection nodes = _doc?.DocumentNode?
                .SelectNodes(".//div[@class='g-i-tile-l g-i-tile-catalog-hover-left-side clearfix']")?.FirstOrDefault()
                .SelectNodes(".//div[@class='g-i-tile g-i-tile-catalog']");

            //			HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(".//[not(parent::table[@class='fixed offers breakword no-results-table'])]//tr[@class='wrap']");
            if (nodes == null)
                yield break;

            foreach (HtmlNode adProductNode in nodes)
            {
                string sId = adProductNode.SelectSingleNode(".//div[@class='g-id']").InnerText.Trim();
                int.TryParse(sId, out int id);
                HtmlNode nameAndHrefNode = adProductNode.SelectSingleNode(".//div[@class='g-i-tile-i-title clearfix']");
                string href = nameAndHrefNode.SelectSingleNode(".//a").Attributes["href"].Value;
                string title = nameAndHrefNode.InnerText.Trim();
                string category = "видеокарты";
                string priceWithCurrency = adProductNode.SelectSingleNode(".//div[@class='g-price-uah']")?.InnerText.Trim();

                yield return new ProductDirty { Id = id.ToString(), Title = title, Href = href, PriceWithCurrency = priceWithCurrency ?? "0 грн.", Category = category };
            }

            if (TryParseNextPage(_doc, out string nextHref))
            {
                Thread.Sleep(new Random().Next(1000, 3000));
                _initPath = new CPath(nextHref);
                foreach (var o in Get())
                {
                    yield return o;
                }
            }
        }

        private bool TryParseNextPage(HtmlDocument doc, out string path)
        {            
            path = "";
            return false;

            HtmlNode node = doc.DocumentNode.SelectSingleNode(".//div[@class='pager rel clr']");
            if (node == null)
                return false;

            node = node.SelectSingleNode(".//span[@class='fbold next abs large']");
            if (node == null)
                return false;

            node = node.SelectSingleNode(".//a");
            if (node == null)
                return false;

            path = node.Attributes["href"].Value;

            return true;
        }

        public override void Select(ICPath cPath)
        {
            string conn = connectionStringBuilder.GetCeonnectionString() + cPath.GetPath().Replace(connectionStringBuilder.GetCeonnectionString(), "");
            string content = getRequest(conn);

            _doc = GetDocument(ref content);

            //SaveToLog(content, "logProduct.txt");
            RemoveOffset(_doc);
        }

        public IEnumerator<ProductDirty> GetEnumerator()
        {
            _currentPath = new CPath(_initPath.GetPath());
            return Get().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public RozetkaProductParcer(IConnectionStringBuilder conn, ICPath initPath)
        {
            SetConnection(conn);
            _initPath = initPath; ;
        }
    }
}
