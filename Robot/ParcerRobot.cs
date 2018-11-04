using Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Robot
{
    public class ParcerRobot<T> where T : class
    {
        private IEnumerable<T> _preparerProduct;
        private ConcurrentQueue<T> _buffer;
        private DataKeeper<T> _dataKeeper;

        private Task DirtParceOlx()
        {
            return Task.Run(() =>
            {
                foreach (T o in _preparerProduct)
                {
                    _buffer.Enqueue(o);
                }
            });
        }
        private async Task RunRead()
        {
            await DirtParceOlx();
        }
        private Task SaveToStorage()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    _dataKeeper.SaveFromBuffer();
                    Thread.Sleep(1000);
                }
            });
        }

        private async Task RunSaveToStorage()
        {
            await SaveToStorage();
        }

        public void Run()
        {
            while (true)
            {
                Task taskRead = RunRead();
                Task taskSave = RunSaveToStorage();
                Thread.Sleep(3600000);
            }
        }

        public ParcerRobot(IEnumerable<T> preparerProduct, DataKeeper<T> dataKeeper, ConcurrentQueue<T> buffer)
        {
            _preparerProduct = preparerProduct;
            _dataKeeper = dataKeeper;
            _buffer = buffer;
        }
    }
}
