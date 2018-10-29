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
    public class ParcerProduct : Parcer
    {
        private ConcurrentQueue<AdOlxDirty> list;

        private void RemoveOffset(HtmlDocument doc)
        {
            HtmlNodeCollection nodesToRemove = doc.DocumentNode.SelectNodes(".//table[@class='fixed offers breakword no-results-table']");

            if (nodesToRemove != null)
                foreach (HtmlNode node in nodesToRemove)
                    node.Remove();
        }

        private void PrintProduct(HtmlDocument doc)
        {
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(".//tr[@class='wrap']");

            //			HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(".//[not(parent::table[@class='fixed offers breakword no-results-table'])]//tr[@class='wrap']");
            if (nodes == null)
                return;
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

                try
                {
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
                    list.Enqueue(new AdOlxDirty { Id = id, Title = sTitle, Href = href, PriceWithCurrency = sPrice, Location = location });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
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

            HtmlDocument doc = GetDocument(ref content);

            SaveToLog(content, "logProduct.txt");
            RemoveOffset(doc);
            PrintProduct(doc);

            if (TryParseNextPage(doc, out string href))
            {
                Thread.Sleep(new Random().Next(1000, 3000));
                Select(new CPath(href));
            }
        }
        public ParcerProduct(ConcurrentQueue<AdOlxDirty> list_)
        {
            list = list_;
        }
    }
}
