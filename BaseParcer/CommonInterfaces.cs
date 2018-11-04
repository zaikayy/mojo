using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseParcer
{
    public interface IContainer<T>
    {
        T GetContainer();
    }

    public interface ICsd
    {
        string GetShemaString();
    }

    /// <summary>
    /// проверка данных по схеме
    /// </summary>
    public interface IDataValidator
    {
        bool CheckCSD<T>(IContainer<T> container);
        void SetCSD(ICsd CSD);
    }

    // маппер данных
    public interface IDataParser
    {
        void Select(ICPath cPath);
        void SetConnection(IConnectionStringBuilder connectionStringBuilder);
        void SetValidator(IDataValidator dataValidator);
    }

    public interface ICPath
    {
        string GetPath();
    }

    public interface IConnectionStringBuilder
    {
        string GetCeonnectionString();
    }
}
