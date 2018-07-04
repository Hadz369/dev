using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace HS.Network.WCF
{
    public enum PacketType
    {
        Poll         = 10,
        EnergyMeter  = 20,
    }

    public abstract class PacketBase
    {
        internal abstract PacketType PktType    { get; }
        internal abstract int        PktVersion { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Packet<T>
    {
        /* Example:
            EnergyMeter meter = new EnergyMeter(bytes);
            Packet<EnergyMeter> pkt = new Packet<EnergyMeter>(meter);
            Console.WriteLine(pkt.ToJSON(true));
        */
        public Packet(T data)
        {
            // Get the version and type from the data object
            PacketBase obj = data as PacketBase;
            if (obj != null)
            {
                typ = obj.PktType;
                ver = obj.PktVersion;
            }

            dat = data;
        }

        public PacketType typ  { get; set; }
        public int        ver  { get; set; }
        public T          dat  { get; set; }

        public string ToJSON()
        {
            return ToJSON(false);
        }

        public string ToJSON(bool indent)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings() {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Formatting = indent ? Formatting.Indented : Formatting.None };

            return JsonConvert.SerializeObject(this, settings);
        }
    }
}
