using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbee.Network
{
    public static class XbeeConst
    {
        public const int XB92_DEVADDRESS = 1;
        public const int XB92_NETADDRESS = 9;
        public const int XB92_RCVOPTIONS = 11;
        public const int XB92_NUMSAMPLES = 12;
        public const int XB92_DIGITALMASK = 13;
        public const int XB92_ANALOGMASK = 15;
        public const int XB92_SAMPLES = 16;
    }

    public static class Common
    {
        /// <summary>
        /// Convert the first two bytes of a supplied byte array to an Int16
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Int16 BytesToInt16(byte[] bytes, int startPos)
        {
            Int16 val;

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            val = BitConverter.ToInt16(bytes, startPos);

            return val;
        }

        /// <summary>
        /// Convert the first two bytes of a supplied byte array to an Int64
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Int64 BytesToInt64(byte[] bytes, int startPos)
        {
            Int64 val;

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            val = BitConverter.ToInt64(bytes, startPos);

            return val;
        }

    }
}
