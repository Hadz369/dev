using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace HS.Network
{
    public interface IPacket
    {
        PacketHeader Header { get; }
        IPEndPoint EndPoint { get; }        
    }

    public abstract class PacketBase : IPacket
    {
        PacketHeader _header   = null;
        IPEndPoint   _endPoint = null;

        public PacketBase(IPEndPoint endpoint, ref byte[] data)
        {
            _header = new PacketHeader(ref data);
            _endPoint = endpoint;

            CreatePacket(ref data);
        }

        public IPEndPoint   EndPoint { get { return _endPoint; } }
        public PacketHeader Header   { get { return _header; } }

        protected abstract void CreatePacket(ref byte[] data);
    }
}
