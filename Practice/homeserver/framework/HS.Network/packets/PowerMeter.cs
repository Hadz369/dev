using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HS.Network
{
    class PowerMeter : PacketBase
    {
        int _p1 = 0, _p2 = 0;

        public PowerMeter(System.Net.IPEndPoint endpoint, ref byte[] data) : base(endpoint, ref data) { }

        protected override void CreatePacket(ref byte[] data)
        {
            throw new NotImplementedException();
        }

        public int Prop1 { get { return _p1; } }
        public int Prop2 { get { return _p2; } }
    }
}
