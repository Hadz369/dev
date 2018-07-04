using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Runtime.Serialization;


namespace IG.ThirdParty
{
    [DataContract]
    public class TcpParameter
    {
        string _name;
        object _value;

        public TcpParameter(string name, object value)
        {
            _name = name;
            _value = value;
        }

        [DataMember]
        public string Name { get { return _name; } }
        
        [DataMember]
        public object Value { get { return _value; } }
    }

    /// <summary>
    /// The TcpRequest object is a wrapper for the packet of data to avoid 
    /// so that there is only one public method required on the service.
    /// </summary>
    /// 
    [DataContract]
    public class TcpRequest
    {
        int _sessionId;

        List<TcpParameter> _parms = new List<TcpParameter>();

        public TcpRequest(int sessionId)
        {
            _sessionId = sessionId;
        }

        [DataMember]
        public int SessionId { get { return _sessionId; } set { _sessionId = value; }  }

        [DataMember]
        public List<TcpParameter> Parameters { get { return _parms; } set { _parms = value; } }

        public void Add(TcpParameter parm)
        {
            if (!_parms.Contains(parm)) _parms.Add(parm);
        }
    }

    /// <summary>
    /// The TcpResponse object is a wrapper for the response to a request packet.
    /// KnownTypes must be specified for each possible data object for deserialisation
    /// on the client.
    /// </summary>
    [DataContract]
    [KnownType(typeof(SessionInfo))]
    public class TcpResponse
    {
        int _rc;
        object _data;

        public TcpResponse(object data)
        {
            _rc = 0;
            _data = data;
        }

        public TcpResponse(int rc, object data)
        {
            _rc = rc;
            _data = data;
        }

        [DataMember]
        public int ResponseCode { get { return _rc; } set { _rc = value; } }
        
        [DataMember]
        public object Data { get { return _data; } set { _data = value; } }


    }

    /// <summary>
    /// Return data from a successful sign on process
    /// </summary>
    [DataContract]
    public class SessionInfo
    {
        public SessionInfo(int sessionId) 
        {
            SessionId = sessionId;
        }

        [DataMember]
        public int SessionId;
    }

    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        TcpResponse SignOn();

        [OperationContract]
        TcpResponse GetSiteName(TcpRequest parms);
    }

    [ServiceContract()]
    public interface IQuickReturnTraderChat
    {
        [OperationContract(IsOneWay = true)]
        void Say(string user, string message);
    }
}
