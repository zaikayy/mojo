using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;

namespace Data
{
    class XmlAdMapper<T>
    {
        private readonly string storageFile;
        public XmlAdMapper(string source_)
        {
            storageFile = source_;
        }
        public IEnumerable<T> ReadAll()
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
        public void WriteAll(List<T> list)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<T>));

            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(storageFile, FileMode.Create))
            {
                formatter.Serialize(fs, list);
            }
        }
    }
    public static class XmlRepositoryProvider
    {
        public static List<T> GetRepozitory<T>()
        {
            XmlAdMapper<T> mapper = new XmlAdMapper<T>(Directory.GetCurrentDirectory() + "\\storage.xml");
            return mapper.ReadAll().ToList();
        }
    }
}
