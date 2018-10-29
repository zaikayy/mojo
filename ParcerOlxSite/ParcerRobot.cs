using AdParcer;
using AdParcer.Olx;
using Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParcerOlxSite
{
    class ParcerRobot
    {
        public static Task DirtParceOlx(IConnectionStringBuilder conn, ICPath path, ConcurrentQueue<AdOlxDirty> listAdDirty) 
        {
            IDataParser paserProduct = new ParcerProduct(listAdDirty);
            paserProduct.SetConnection(conn);
            
            return Task.Run(() =>
            {
                paserProduct.Select(path);
            });
        }
        static async Task RunRead(IConnectionStringBuilder conn, ICPath path, ConcurrentQueue<AdOlxDirty> listAdDirty)
        {
            await DirtParceOlx(conn, path, listAdDirty);
        }
        static Task SaveToStorage(ConcurrentQueue<AdOlxDirty> coll)
        {
            return Task.Run(() =>
            {
                Data.GoogleSheetProvider<AdPrepared1> provider = new Data.GoogleSheetProvider<AdPrepared1>("My Project 72320", "client_secret.json", "1kE61PAN5zG_Ygx98PmaRxKZp0L1_NxSelEfOFC2owJs", "Class Data!A1:F");
                List<AdPrepared1> currentList = provider.Get().ToList();
                AdPrepared1Comparer adComparer = new AdPrepared1Comparer();

                List<AdPrepared1> tmpList = new List<AdPrepared1>();

                while (true)
                {
                    tmpList.Clear();
                    while (coll.TryDequeue(out AdOlxDirty value))
                    {
                        tmpList.Add(Preparer1Ad.Prepare(value));
                    }
                    List<AdPrepared1> newList = tmpList.Except(currentList, adComparer).ToList();

                    newList.ForEach(a => {
                        provider.Insert(new List<object>() { a.Id, a.Href, a.Title, a.Location, a.Price, a.Currency });
                        Thread.Sleep(1000);
                    });
                    if (newList.Any())
                        currentList.AddRange(newList);
                    newList.ForEach(a => Console.WriteLine(a.ToStringFormat1()));
                    Thread.Sleep(1000);
                }
            });
        }

        static async Task RunSaveToStorage(ConcurrentQueue<AdOlxDirty> listAdDirty)
        {
            await SaveToStorage(listAdDirty);
        }

        public static void Run()
        {
            //XmlAdProvider<AdPrepared1> oldListProvider = new XmlAdProvider<AdPrepared1>(new FileXmlStorageAdapter<AdPrepared1>(Directory.GetCurrentDirectory() + "\\storage.xml"));
            //List<AdPrepared1> oldList = oldListProvider.Get().ToList();
            ConcurrentQueue<AdOlxDirty> listAdDirty = new ConcurrentQueue<AdOlxDirty>();
            

            //Data.GoogleSheetProvider<AdPrepared1> provider = new Data.GoogleSheetProvider<AdPrepared1>("My Project 72320", "client_secret.json", "1kE61PAN5zG_Ygx98PmaRxKZp0L1_NxSelEfOFC2owJs", "Class Data!A1:F");
            //List<AdPrepared1> oldList = provider.Get().ToList();
            
            while (true)
            {
                IConnectionStringBuilder conn = new Connection("https://www.olx.ua");
                //ICPath path1 = new CPath("/elektronika/kompyutery-i-komplektuyuschie/komplektuyuschie-i-aksesuary/protsessory/dnepr/q-5450/" +
                //                         "?search%5Bfilter_float_price%3Afrom%5D=500&search%5Bfilter_float_price%3Ato%5D=1000"
                //                        );
                //ICPath path2 = new CPath("/elektronika/kompyutery-i-komplektuyuschie/komplektuyuschie-i-aksesuary/protsessory/dnepr/q-5460/" +
                //                         "?search%5Bfilter_float_price%3Afrom%5D=500&search%5Bfilter_float_price%3Ato%5D=1000");
                ICPath path3 = new CPath("/elektronika/kompyutery-i-komplektuyuschie/komplektuyuschie-i-aksesuary/videokarty/dnepr/" //+
                                         //"?search%5Bfilter_float_price%3Afrom%5D=500&search%5Bfilter_float_price%3Ato%5D=2000"
                                         );
                Task taskRead = RunRead(conn, path3, listAdDirty);
                Task taskSave = RunSaveToStorage(listAdDirty);

                //List<AdPrepared1> newList = DirtyToPreparedAd1(dirtList).Except(oldList, adComparer).ToList();
                //oldList.AddRange(newList);
                //oldListProvider.Add(oldList);
                //newList.ForEach(a => Console.WriteLine(a.ToStringFormat1()));
                //newList.ForEach(a => provider.Insert(new List<object>() { a.Id, a.Href, a.Title, a.Location, a.Price, a.Currency }));
                //Thread.Sleep(2000);
                //dirtList = DirtParceOlx(conn, path2);
                //Thread.Sleep(3600);
                //				Parce(conn, path3);
                Thread.Sleep(3600000);
                //				Parce(new CPath("/elektronika/kompyutery-i-komplektuyuschie/komplektuyuschie-i-aksesuary/videokarty/dnepr/q-450/?search%5Bfilter_float_price%3Afrom%5D=1500&search%5Bfilter_float_price%3Ato%5D=1500"));
                //				Thread.Sleep(3000);
            }
        }
    }
}
