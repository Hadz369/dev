using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HS.Network
{
    public class PacketHeader
    {
        public PacketHeader(ref byte[] data)
        {
            PacketType = PacketType.PowerSummary;
            Handle = 0;
            PacketLength = data.Length;
        }

        public PacketType PacketType { get; private set; }
        public int Handle { get; private set; }
        public int PacketLength { get; private set; }
    }
}
