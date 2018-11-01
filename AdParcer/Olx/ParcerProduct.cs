using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using HtmlAgilityPack;

namespace AdParcer.Olx
{
    /// <summary>
    /// Парсер товаров
    /// </summary>
    public class ParcerProduct : Parcer, IEnumerable<AdOlxDirty>
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

        public IEnumerable<AdOlxDirty> Get()
        {
            Select(_currentPath);
            HtmlNodeCollection nodes = _doc?.DocumentNode?.SelectNodes(".//tr[@class='wrap']");

            //			HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(".//[not(parent::table[@class='fixed offers breakword no-results-table'])]//tr[@class='wrap']");
            if (nodes == null)
                yield break;

            foreach (HtmlNode title in nodes)
            {
                string s = title.InnerHtml;

                string location = "";
                try
                {
                    HtmlNode n = title.SelectSingleNode(".//td[@valign='bottom']");
                    n = n.SelectSingleNode(".//div[@class='space rel']");
                    location = n.SelectSingleNode(".//small [@class='breadcrumb x-normal']").InnerText.Trim();
                }
                catch { }

                int id = 0;
                try
                {
                    id = int.Parse(title.SelectSingleNode(".//table").Attributes["data-id"].Value);
                }
                catch { }

                string sNodePrev1 = ".//h3[@class='x-large lheight20 margintop5']";
                string sNodePrev2 = ".//h3[@class='lheight22 margintop5']";

                HtmlNode node = title.SelectSingleNode(sNodePrev1) ?? title.SelectSingleNode(sNodePrev2); ;

                string sTitle = node.SelectSingleNode(".//strong").InnerText;
                string href = node.SelectSingleNode(".//a").Attributes["href"].Value;
                node = title.SelectSingleNode(".//p[@class='price']").SelectSingleNode(".//strong");
                string sPrice = node.InnerText.Replace(" ", "");

                yield return new AdOlxDirty { Id = id, Title = sTitle, Href = href, PriceWithCurrency = sPrice, Location = location };
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

        public IEnumerator<AdOlxDirty> GetEnumerator()
        {
            _currentPath = new CPath(_initPath.GetPath());
            return Get().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public ParcerProduct(IConnectionStringBuilder conn, ICPath initPath)
        {
            SetConnection(conn);
            _initPath = initPath; ;
        }
    }
}
