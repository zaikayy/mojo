using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Data
{
    public class XmlAdProvider<T> where T : class
    {
        private readonly FileXmlStorageAdapter<T> _adapter;
        public XmlAdProvider(FileXmlStorageAdapter<T> adapter)
        {
            _adapter = adapter;
        }
        public void Add(List<T> list)
        {
            _adapter.AddRange(list);
        }
        public IEnumerable<T> Get()
        {
            return _adapter.GetValues();
        }
    }
    public class FileXmlStorageAdapter<T> : StorageAdapter<T> where T : class
    {
        private readonly string storageFile;
        public FileXmlStorageAdapter(string source)
        {
            storageFile = source;
        }
        public override void AddRange(List<T> list)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<T>));

            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(storageFile, FileMode.Create))
            {
                formatter.Serialize(fs, list);
            }
        }
        public override IEnumerable<T> GetValues()
        {
            List<T> list = new List<T>();
            XmlSerializer formatter = new XmlSerializer(typeof(List<T>));
            if (File.Exists(storageFile))
                using (FileStream fs = new FileStream(storageFile, FileMode.OpenOrCreate))
                {
                    list = (List<T>)formatter.Deserialize(fs);
                }
            return list;
        }
    }
}
