using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HS.Network
{
    public static class PacketHelper
    {
        static Dictionary<PacketType, Type> _types;

        static PacketHelper()
        {
            _types = new Dictionary<PacketType, Type>();

            _types.Add(PacketType.PowerSummary, typeof(PowerSummary));
            _types.Add(PacketType.EnergyMeter,  typeof(PowerMeter));
            //_types.Add(PacketType.Signon, typeof(SignOnPacket));
        }

        public static Dictionary<PacketType, Type> PacketTypes { get { return _types; } }
    }
}
