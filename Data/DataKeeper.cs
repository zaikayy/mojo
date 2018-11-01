using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    public class DataKeeper<T> where T : class
    {
        private IRepository<T> _repository;
        private ConcurrentQueue<T> _buffer;
        private IEqualityComparer<T> _comparer;
        private Lazy<List<T>> _cache;
        public void SaveFromBuffer()
        {
            List<T> tmpList = new List<T>();
            
            while (_buffer.TryDequeue(out T value))
            {
                tmpList.Add(value);
            }
            List<T> newList = tmpList.Except(_cache.Value, _comparer).ToList();

            newList.ForEach(a => {
                _repository.Create(a);
                Thread.Sleep(1000);
            });
            if (newList.Any())
                _cache.Value.AddRange(newList);
            newList.ForEach(a => Console.WriteLine(a.ToString()));
        }
        public DataKeeper(IRepository<T> repository, ConcurrentQueue<T> buffer, IEqualityComparer<T> comparer)
        {
            _repository = repository;
            _buffer = buffer;
            _comparer = comparer;
            _cache = new Lazy<List<T>>(() => _repository.GetAll().ToList());
        }
    }
}
