using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace HS
{
    [DataContract(Namespace = "")]
    public class Header
    {
        public Header(string sessionKey, string action, string handler)
        {
            SessionKey = sessionKey;
            Action = action;
            Handler = handler;
            Created = DateTime.Now;
        }

        [DataMember]
        public string SessionKey { get; set; }

        [DataMember]
        public int MessageId { get; set; }

        [DataMember]
        public string Handler { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public DateTime Created { get; set; }

        public override string ToString()
        {
            return String.Format("Handler=\"{0}\", Action=\"{1}\", MessageId=\"{2}\", SessionKey=\"{3}\"", Handler, Action, MessageId, SessionKey);
        }
    }

    [DataContract(Namespace = "")]
    public class Request
    {
        Header _header;
        PropertyBag _props = null;

        public Request() { }

        public Request(string sessionKey, string handler, string action) : this(sessionKey, action, handler, 0) { }

        public Request(string sessionKey, string handler, string action, int messageId)
        {
            _header = new Header(sessionKey, handler, action) { MessageId = messageId };
            _props = new PropertyBag();
        }

        [DataMember]
        public Header Header { get { return _header; } set { _header = value; } }

        [DataMember]
        public string SessionKey { get; set; }

        [DataMember]
        public PropertyBag Parameters { get { return _props; } set { _props = value; } }

        /// <summary>
        /// Match supplied parameters with a list of required or optional parameters
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public Dictionary<string, Property> ValidateParameters(Dictionary<string, Property> props)
        {
            foreach (Property p in Parameters)
            {
                try 
                {
                    props[p.Name] = p;
                }
                catch
                {
                    throw new InvalidPropertyException(String.Format("Parameter {0} was not found in the supplied property list", p.Name));
                }
            }

            return props;
        }

        public string ToXmlString()
        {
            string str = "";

            str = String.Concat(
                "<req><hdr ",
                Header.ToString(),
                "/><prm ",
                Parameters.ToNameValueString(),
                "/></req>");

            return str;
        }
    }

    [DataContract(Namespace = "")]
    public class Response
    {
        object _data = null;

        public Response() { }

        public Response(Header header) : this(header, null) { }

        public Response(Header header, object data)
        {
            Created = DateTime.Now;
            Header = header;
            SetData(data);
        }

        [DataMember]
        public Header Header { get; set; }

        [DataMember]
        public Int32 ReturnCode { get; set; }

        [DataMember]
        public object Data
        {
            get { return _data; }
            set { SetData(value); } 
        }

        [DataMember]
        public DateTime Created { get; set; }

        private void SetData(object value)
        {
            _data = value;
            if (value != null && value.GetType() == typeof(FaultData))
                IsFault = true;
            else
               IsFault = false;
        }

        [DataMember]
        public bool IsFault { get; set; }
    }

    [DataContract(Namespace = "")]
    public class ACK { }

    [DataContract(Namespace = "")]
    public class NAK { }


    public class RequestCompletedAsyncResult<T> : IAsyncResult
    {
        T _data;

        public RequestCompletedAsyncResult(T data)
        {
            _data = data;
        }

        public T Data
        { get { return _data; } }

        #region IAsyncResult Members

        public object AsyncState
        { get { return (object)_data; } }

        public WaitHandle AsyncWaitHandle
        { get { throw new Exception("The method or operation is not implemented."); } }

        public bool CompletedSynchronously
        { get { return true; } }

        public bool IsCompleted
        { get { return true; } }

        #endregion
    }
}
