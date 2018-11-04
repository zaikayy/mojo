using AdParcer;
using AdParcer.Rozetka;
using AdParcer.Rozetka.Comparators;
using AdParcer.Rozetka.Entity;
using Data;
using Robot;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcerRozetka
{
    class Program
    {
        static void Main(string[] args)
        {
            ConcurrentQueue<ProductPrepared1> buffer = new ConcurrentQueue<ProductPrepared1>();
            IEqualityComparer<ProductPrepared1> comparer = new ProductPrepared1Comparer();

            GoogleSheetProvider<ProductPrepared1> repository = new GoogleSheetProvider<ProductPrepared1>("My Project 72320", "client_secret.json", "1kE61PAN5zG_Ygx98PmaRxKZp0L1_NxSelEfOFC2owJs", "Классификатор - розетка!A1:F");
            DataKeeper<ProductPrepared1> dataKeeper = new DataKeeper<ProductPrepared1>(repository, buffer, comparer);

            IEnumerable<ProductDirty> parcerDirtProduct = new RozetkaProductParcer(new Connection("https://hard.rozetka.com.ua"), new CPath("/videocards/c80087/"));
            IEnumerable<ProductPrepared1> parcer = new RozetkaPreparerProductDecorator(parcerDirtProduct);
            ParcerRobot<ProductPrepared1> robo = new ParcerRobot<ProductPrepared1>(parcer, dataKeeper, buffer);
            robo.Run();
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }
    }
}
