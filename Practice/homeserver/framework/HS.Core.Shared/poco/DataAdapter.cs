using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HS
{
    public interface IIndexedDataRecord
    {
        object this[string name] { get; }

        T GetValue<T>(string name);
        T GetValue<T>(int index);
    }

    public class DataReaderAdapter : IIndexedDataRecord
    {
        private readonly IDataReader reader;

        public DataReaderAdapter(IDataReader reader)
        {
            this.reader = reader;
        }

        public object this[string name]
        {
            get { return reader[name]; }
        }

        public T GetValue<T>(string name)
        {
            try
            {
                return (T)Convert.ChangeType(reader[name], typeof(T));
            }
            catch
            {
                throw new InvalidPropertyException(String.Format("Error converting field '{0}' to {1}", name, typeof(T).ToString()));
            }
        }

        public T GetValue<T>(int index)
        {
            try
            {
                return (T)Convert.ChangeType(reader[index], typeof(T));
            }
            catch
            {
                throw new InvalidPropertyException(String.Format("Error converting field {0} to {1}", index, typeof(T).ToString()));
            }
        }
    }

    public class DataRowAdapter : IIndexedDataRecord
    {
        private readonly DataRow row;

        public DataRowAdapter(DataRow row)
        {
            this.row = row;
        }

        public object this[string name]
        {
            get { return row[name]; }
        }

        public T GetValue<T>(string name)
        {
            try
            {
                return (T)Convert.ChangeType(row[name], typeof(T));
            }
            catch
            {
                throw new InvalidPropertyException(String.Format("Error converting field '{0}' to {1}", name, typeof(T).ToString()));
            }
        }

        public T GetValue<T>(int index)
        {
            try
            {
                return (T)Convert.ChangeType(row[index], typeof(T));
            }
            catch
            {
                throw new InvalidPropertyException(String.Format("Error converting field {0} to {1}", index, typeof(T).ToString()));
            }
        }
    }
}
