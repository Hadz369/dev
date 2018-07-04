using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ruby.Data
{
    public interface ICMSMeterRead
    {
        int Gmid { get; }
        DateTime ReadTime { get; }
        int DataSequenceNumber { get; }
        int ReadingType { get; }
        int Flags { get; }
        CMSMeterSetType MeterSetType { get; }
        List<Meter> Meters { get; }
    }
}
