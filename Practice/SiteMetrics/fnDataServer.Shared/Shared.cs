using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.IO;

namespace fnDataServer
{
    [ServiceContract]
    public interface IService : IDisposable
    {
        [OperationContract]
        SignonResponseData SignOn(SignonData Data);

        [OperationContract]
        bool SignOff(int Handle);

        [OperationContract]
        ErrorMessage PutDataPacket(DataPacket packet);
    }

    
    [ServiceContract(CallbackContract = typeof(IMyServiceCallback))] 
    public interface IMyService 
    { 
        [OperationContract] 
        void OpenSession(); 
    } 

    public interface IMyServiceCallback 
    { 
        [OperationContract] 
        void OnCallback(); 
    } 


    [DataContract]
    public class SignonData
    {
        int deviceId;
        string vendorName;

        public SignonData(int DeviceId, string VendorName)
        {
            deviceId = DeviceId;
            vendorName = VendorName;
        }

        [DataMember]
        public int DeviceId { get { return deviceId; } set { deviceId = value; }  }

        [DataMember]
        public string VendorName { get { return vendorName; } set { vendorName = value; } }
    }

    [DataContract]
    public class SignonResponseData
    {
        ErrorMessage _msg = null;
        bool _authorised = false;
        int _handle = 0;

        public SignonResponseData(int Handle)
        {
            _authorised = true;
            _handle = Handle;
        }
        
        public SignonResponseData(ErrorMessage ErrorMessage)
        {
            _authorised = false;
            _msg = ErrorMessage;
        }

        [DataMember]
        public bool Authorised { get { return _authorised; } set { _authorised = value; } }

        [DataMember]
        public int Handle { get { return _handle; } set { _handle = value; } }

        [DataMember]
        public ErrorMessage ErrorMessage { get { return _msg; } set { _msg = value; } }
    }

    [DataContract]
    public class ErrorMessage
    {
        int _code;
        string _message;

        public ErrorMessage(int Code, string Message)
        {
            _code = Code;
            _message = Message;
        }

        [DataMember]
        public int Code { get { return _code; } set { _code = value; } }

        [DataMember]
        public string Message { get { return _message; } set { _message = value; } }
    }

    [DataContract]
    public class DataPacket
    {
        int _type = 0;
        int _handle = 0;
        List<object[]> _data = new List<object[]>();

        public DataPacket(int type, int handle, DataTable data)
        {
            _type = type;
            _handle = handle;

            int cols = data.Columns.Count;

            object[] types = new object[cols];
            // Add the column datatype first record
            for (int x = 0; x < cols; x++) types[x] = data.Columns[x].DataType.AssemblyQualifiedName;
            _data.Add(types);

            // Add the column name as the second row
            object[] names = new object[cols];
            for (int x = 0; x < cols; x++) names[x] = data.Columns[x].ColumnName;
            _data.Add(names);

            foreach (DataRow r in data.Rows)
            {
                object[] o = new object[cols];
                for (int x = 0; x < cols; x++) o[x] = r[x];
                _data.Add(o);
            }
        }

        public DataTable GetDataTable()
        {
            DataTable dt = new DataTable("Data");
            for (int x = 0; x < _data[0].Length; x++)
            {
                dt.Columns.Add(new DataColumn(Data[1][x].ToString(), System.Type.GetType(_data[0][x].ToString())));
            }

            for (int x = 2; x < _data.Count; x++)
            {
                DataRow dr = dt.NewRow();
                object[] o = _data[x];
                for (int y = 0; y < dt.Columns.Count; y++)
                {
                    dr[y] = o[y];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        [DataMember]
        public int Type { get { return _type; } set { _type = value; } }

        [DataMember]
        public int Handle { get { return _handle; } set { _handle = value; } }

        [DataMember]
        public List<object[]> Data { get { return _data; } set { _data = value; } }
    }
}