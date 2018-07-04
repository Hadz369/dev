using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using ProtoBuf;

namespace FSW
{
    public enum PacketType
    {
        Request  = 0,
        Response = 1,
        Message  = 2,
    }

    [DataContract(Namespace = "http://www.hadz.net/FSW")]
    public class Packet : ICloneable
    {
        [DataMember]
        public Header Header      { get; set; }

        [DataMember]
        public object Body { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone() as Packet;
        }
    }

    [DataContract(Namespace = "http://www.hadz.net/FSW")]
    public class Header
    {
        public Header(int packetType, string commandClass, string command, int commandId)
        {
            PacketType     = packetType;
            CommandClass   = commandClass;
            Command        = command;
            CommandId      = commandId;

            MessageId = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            DateTimeSent = DateTime.UtcNow;
        }

        [DataMember]
        public int PacketType { get; set; }
        [DataMember]
        public string CommandClass { get; set; }
        [DataMember]
        public string Command { get; set; }
        [DataMember]
        public int CommandId { get; set; }
        [DataMember]
        public int CommandVersion { get; set; }
        [DataMember]
        public Guid SessionKey { get; set; }
        [DataMember]
        public DateTime DateTimeSent { get; set; }
        [DataMember]
        public string SourceEndpoint { get; set; }
        [DataMember]
        public string TargetEndpoint { get; set; }
        [DataMember]
        public string MessageId { get; private set; }

    }

    [DataContract(Namespace = "http://www.hadz.net/FSW")]
    public class Body
    {
        public Body(Packet packet, object obj)
        {
            if (obj is FaultData)
            {
                // Remove the exception message when passing between services if the flag is set
                if (FaultHandler.ExcludeExceptionData)
                    (obj as FaultData).ErrorMessage = null;

                ReturnValue = 1;
            }
            else
            {
                ReturnValue = 0;
            }

            Type objType = obj.GetType();

            // Serialise the object to a byte array using the Protobuf library
            if (Attribute.IsDefined(objType, typeof(ProtoContractAttribute)))
            {
                MemoryStream ms = new MemoryStream();
                Serializer.Serialize(ms, obj);
                Value = ms.ToArray();
                Hash = HashCalc.GenerateHash(
                    HashCalc.HashType.SHA1,
                    String.Concat(packet.Header.CommandId, packet.Header.SessionKey),
                    Value as byte[]);
            }
            else
            {
                Value = obj;
                Hash = null;
            }

            TypeName = objType.FullName;
        }

        [DataMember]
        public string TypeName { get; private set; }

        [DataMember]
        public string Hash { get; private set; }

        [DataMember]
        public object Value { get; private set; }

        [DataMember]
        public int ReturnValue { get; set; }

        public T Deserialise<T>()
        {
            T rsp;

            if (Value is byte[])
            {
                int x = Environment.TickCount;

                using (MemoryStream ms = new MemoryStream(Value as byte[]))
                {
                    rsp = Serializer.Deserialize<T>(ms);
                }

                Log.Debug(String.Format("Ticks = {0}", Environment.TickCount - x));
            }
            else
            {
                rsp = (T)Value;
            }

            return rsp;
        }

        public object Deserialise()
        {
            int x = Environment.TickCount;

            object rsp = null;

            Type type = Type.GetType(TypeName);

            if (type == null)
            {
                throw new TypeInitializationException(TypeName, null);
            }
            else if (Value.GetType() == typeof(byte[]))
            {
                using (MemoryStream ms = new MemoryStream(Value as byte[]))
                    rsp = Serializer.NonGeneric.Deserialize(type, ms);
            }
            else
            {
                rsp = Value;
            }

            Log.Debug(String.Format("Object deserialised: Type={0}, TickCount={1}", TypeName, Environment.TickCount - x));

            return rsp;
        }
    }

    public class PacketHandler
    {
        int _packetCount = 0;

        public Guid SessionKey { get; set; }

        public Packet GetPacket(PacketType packetType, string commandClass, string command)
        {
            Packet p = new Packet() { Header = new Header((int)packetType, commandClass, command, ++_packetCount) };
            p.Header.SessionKey = SessionKey;

            return p;
        }

        public Packet PrepareResponse(Packet requestPacket, object body)
        {
            Packet p = requestPacket.Clone() as Packet;
            p.Header.PacketType = (int)PacketType.Response;

            string srcTemp = p.Header.TargetEndpoint;
            p.Header.TargetEndpoint = p.Header.SourceEndpoint;
            p.Header.SourceEndpoint = srcTemp;
            p.Header.DateTimeSent   = DateTime.UtcNow;

            if (body != null)
                p.Body = new Body(p, body);

            return p;
        }
    }
}
