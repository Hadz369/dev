using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;

namespace HS.Network.WCF
{
    public class EnergyMeter : PacketBase
    {
        public EnergyMeter(byte[] packet)
        {
        }

        internal override PacketType PktType    { get { return WCF.PacketType.EnergyMeter; } }
        internal override int        PktVersion { get { return 1; } }

        public int Meter { get; set; }
        public int Voltage { get; set; }
        public int Current { get; set; }
        public int RealPower { get; set; }
        public int PowerIncrement { get; set; }
    }
}
