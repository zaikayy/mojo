/*
 * Создано в SharpDevelop.
 * Пользователь: Zaika
 * Дата: 07.12.2017
 * Время: 22:21
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Data;
using AdParcer;
using AdParcer.Olx;
using System.Collections.Concurrent;
//using Data;

namespace ParcerOlxSite
{
    public class People
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    class Program
	{
		private static void ShowBalloon(string title, string body)
		{
            NotifyIcon notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Exclamation,
                Visible = true
            };
            notifyIcon.ShowBalloonTip(20000, title, body, ToolTipIcon.Info);
		}
        #region test
  //      private static string GetShortInfoAD(IEnumerable<AD> list, int maxCount)
		//{
		//	string s = "";
		//	int i = 0;
		//	foreach (AD ad in list)
		//	{
		//		s += ad.title + Environment.NewLine + "Цена: " + ad.price + " " + ad.currency;
		//		if (maxCount != -1 && ++i >= maxCount)
		//			break;				
		//	}
		//	return s;
		//}
        #endregion
  //      public static List<AdOlxDirty> DirtParceOlx(IConnectionStringBuilder conn, ICPath path)
		//{
		//	List<AdOlxDirty> listAdDirty = new List<AdOlxDirty>();
  //          ParcerProduct paserProduct = new ParcerProduct(listAdDirty);
		//	paserProduct.SetConnection(conn);
		//	paserProduct.Select(path);
  //          return listAdDirty;
		//}
  //      public static List<AdPrepared1> DirtyToPreparedAd1(List<AdOlxDirty> listAdDirty)
  //      {
  //          List<AdPrepared1> listAdPrepared1 = listAdDirty.Select(a => Preparer1Ad.Prepare(a)).ToList();
  //          return listAdPrepared1;
  //      }
        #region OldFunc
        //public static void OldFunc(List<AdPrepared1> listAdPrepared1)
        //{
        //    var comparer = new AdPrepared1Comparer();
        //    HashSet<AdPrepared1> distinctAD = new HashSet<AdPrepared1>(listAdPrepared1, comparer);

        //    XmlAdMapper<AdPrepared1> mapper = new XmlAdMapper<AdPrepared1>(Directory.GetCurrentDirectory() + "\\storage.xml");
        //    HashSet<AdPrepared1> storedListAD = new HashSet<AdPrepared1>(comparer);

        //    foreach (AdPrepared1 o in mapper.ReadAll())
        //        storedListAD.Add(o);

        //    distinctAD = new HashSet<AdPrepared1>(distinctAD.Except(storedListAD, comparer), comparer);

        //    if (storedListAD.SequenceEqual(storedListAD.Union(distinctAD, comparer), comparer) == false)
        //    {
        //        //				ShowBalloon("Новые товары", GetShortInfoAD(distinctAD, 3));
        //        foreach (AdPrepared1 ad in distinctAD)
        //            Console.WriteLine(ad);
        //        listAdPrepared1 = storedListAD.Union(distinctAD).ToList();
        //        mapper.WriteAll(listAdPrepared1);
        //    }
        //}
        #endregion 
        public static void Main(string[] args)
		{
            #region test google
            //Data.GoogleSheetProvider<People> provider = new Data.GoogleSheetProvider<People>("My Project 72320", "client_secret.json", "1kE61PAN5zG_Ygx98PmaRxKZp0L1_NxSelEfOFC2owJs");
            //var p = new People { Age = 44, Name = "Solovian Andrev" };
            //provider.Insert(new List<object>() { p.Name, p.Age }, "Class Data!A1:B");

            //foreach (var a in provider.Get("Class Data!A1:B"))
            //{
            //    Console.WriteLine($"age: {a.Age} people: {a.Name}");
            //}
            //Console.ReadKey();
            #endregion

            ConcurrentQueue<AdPrepared1> buffer = new ConcurrentQueue<AdPrepared1>();
            IEqualityComparer<AdPrepared1> comparer = new AdPrepared1Comparer();

            GoogleSheetProvider<AdPrepared1> repository = new GoogleSheetProvider<AdPrepared1>("My Project 72320", "client_secret.json", "1kE61PAN5zG_Ygx98PmaRxKZp0L1_NxSelEfOFC2owJs", "Class Data!A1:F");
            DataKeeper<AdPrepared1> dataKeeper = new DataKeeper<AdPrepared1>(repository, buffer, comparer);

            IEnumerable<AdOlxDirty> parcerDirtProduct = new ParcerProduct(new Connection("https://www.olx.ua"), new CPath("/elektronika/kompyutery-i-komplektuyuschie/komplektuyuschie-i-aksesuary/videokarty/dnepr/"));
            IEnumerable<AdPrepared1> parcer = new PreparerProductDecorator(parcerDirtProduct);
            ParcerRobot<AdPrepared1> robo = new ParcerRobot<AdPrepared1>(parcer, dataKeeper, buffer);
            robo.Run();
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}