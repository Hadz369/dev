using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Ruby.Data
{
    [DataContract]
    public class FaultData
    {
        public FaultData(int code, string type, string message)
        {
            Code = code;
            Type = type;
            Message = message;
        }

        [DataMember]
        public virtual int Code { get; private set; }

        [DataMember]
        public virtual string Type { get; private set; }

        [DataMember]
        public virtual string Message { get; private set; }

        public override string ToString()
        {
            return String.Format("Code={0}; Type={1}; Mesg={2}", Code, Type, Message);
        }
    }
}
