using AdParcer.Rozetka.Entity;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AdParcer.Rozetka
{
    class Constraints
    {
        public const int MAX_DEPTH_ENTRY = 300;
        public const int FIRST_PAGE_INDEX = 1;
    }
    public class RozetkaProductParcer : Parcer, IEnumerable<ProductDirty>
    {
        private HtmlDocument _doc;
        private readonly ICPath _initPath;
        private ICPath _currentPath;
        private int _currentPageNum;

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
                _currentPageNum++;

                Thread.Sleep(new Random().Next(1000, 3000));
                _currentPath = new CPath(nextHref.Replace(connectionStringBuilder.GetCeonnectionString(), string.Empty));
                
                foreach (var o in Get())
                {
                    yield return o;
                }
            }
        }

        private bool TryParseNextPage(HtmlDocument doc, out string path)
        {            
            path = "";
            //return false;
            string sCurrentPage = HttpUtility.ParseQueryString(_currentPath.GetPath()).Get("page");
            int.TryParse(sCurrentPage, out int tmpCurrentPage);
            _currentPageNum = Math.Max(tmpCurrentPage, _currentPageNum);

            HtmlNode node = doc.DocumentNode.SelectSingleNode(".//nav[@class='paginator-catalog pos-fix']");
            if (node == null)
                return false;

            HtmlNodeCollection nodes = node.SelectNodes(".//a[@class='blacklink paginator-catalog-l-link']");
            if (nodes == null)
                return false;

            var l1 = nodes.Where(a => int.TryParse(a.InnerText.Trim(), out int j));

            var pageList = l1.Select(a => new {
                Value = int.Parse(a.InnerText.Trim()),
                Href = a.Attributes["href"].Value
            }).ToList();

            string nextPageHref = pageList.Where(a => a.Value > _currentPageNum)?.OrderBy(a => a.Value).Select(a => a.Href).FirstOrDefault();
            path = nextPageHref ?? "";
            return nextPageHref != null;

            if (nextPageHref == null)
            {
                return false;
            }

            string sNumPage = node.InnerText.Trim();

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
            //RemoveOffset(_doc);
        }

        public IEnumerator<ProductDirty> GetEnumerator()
        {
            _currentPageNum = Constraints.FIRST_PAGE_INDEX;
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
