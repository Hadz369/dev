using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Ruby.Data
{
    /// <summary>
    /// This is a static class containing a few common methods for parsing and comparison of 
    /// CMS meter reads
    /// </summary>
    public static class CMSMeterRead
    {
        public static ICMSMeterRead Parse(CMSMeterSetType meterSetType, DataRow row)
        {
            ICMSMeterRead read = null;

            switch (meterSetType)
            {
                case CMSMeterSetType.Egm:
                    read = new CMSEgmMeterRead(row);
                    break;
                case CMSMeterSetType.Link:
                    read = new CMSLinkMeterRead(row);
                    break;
            }

            return read;
        }

        /// <summary>
        /// Compares a previous meter set with a current meter set and marks the 
        /// meters in the current meter set as changed when the meter values are different
        /// </summary>
        /// <param name="prev"></param>
        /// <param name="curr"></param>
        public static void CompareCMSMeters(CMSMeterReadBase prev, CMSMeterReadBase curr)
        {
            for (int x = 0; x < prev.Meters.Count; x++)
            {
                if (prev.Meters[x].Value == curr.Meters[x].Value)
                    curr.Meters[x].IsChanged = false;
            }
        }

        public static int GetMeterCode(int setType, int meterType, Meter meter)
        {
            int code = 0;

            return code;
        }
    }

    /// <summary>
    /// Base class for all CMS meter reads
    /// </summary>
    public abstract class CMSMeterReadBase : ICMSMeterRead
    {
        int _gmid, _seqnum, _rtype, _flags, _repseqnum;
        protected CMSMeterSetType _meterSetType;
        string _repAction;
        DateTime _readDt;
        List<Meter> _meters;

        public CMSMeterReadBase(CMSMeterSetType meterSetType, DataRow row)
        {
            _gmid = Convert.ToInt32(row["Gmid"]);
            _readDt = Convert.ToDateTime(row["ReadingTime"]);
            _seqnum = Convert.ToInt32(row["DataSequenceNumber"]);
            _rtype = Convert.ToInt32(row["ReadingType"]);
            _flags = Convert.ToInt32(row["Flags"]);
            _repAction = row["RepAction"].ToString();
            _repseqnum = Convert.ToInt32(row["RepSequenceNumber"]);
            _meterSetType = meterSetType;

            _meters = new List<Meter>();
        }

        public int              Gmid               { get { return _gmid; } }
        public DateTime         ReadTime           { get { return _readDt; } }
        public int              DataSequenceNumber { get { return _seqnum; } }
        public int              ReadingType        { get { return _rtype; } }
        public int              Flags              { get { return _flags; } }
        public string           RepAction          { get { return _repAction; } }
        public int              RepSequenceNumber  { get { return _repseqnum; } }
        public CMSMeterSetType  MeterSetType       { get { return (CMSMeterSetType)_meterSetType; } }

        public List<Meter> Meters { get { return _meters; } }

        protected void AddMeter(string codename, int value)
        {
            codename = codename.ToUpper().Trim();
            this.Meters.Add(new Meter((int)_meterSetType, codename, value));
        }

    }

    /// <summary>
    /// CMS EGM meter read class
    /// </summary>
    public class CMSEgmMeterRead : CMSMeterReadBase
    {
        public CMSEgmMeterRead(DataRow row)
            : base(CMSMeterSetType.Egm, row)
        {
            AddMeters(row);
        }

        void AddMeters(DataRow row)
        {
            base.AddMeter("TURNOVER", Convert.ToInt32(row["Turnover"]));
            base.AddMeter("TOTALWINS", Convert.ToInt32(row["TotalWins"]));
            base.AddMeter("CASHBOX", Convert.ToInt32(row["CashBox"]));
            base.AddMeter("CANCELLEDCREDITS", Convert.ToInt32(row["CancelledCredits"]));
            base.AddMeter("MONEYIN", Convert.ToInt32(row["MoneyIn"]));
            base.AddMeter("MONEYOUT", Convert.ToInt32(row["MoneyOut"]));
            base.AddMeter("CASHIN", Convert.ToInt32(row["CashIn"]));
            base.AddMeter("CASHOUT", Convert.ToInt32(row["CashOut"]));
            base.AddMeter("CREDIT", Convert.ToInt32(row["Credit"]));
            base.AddMeter("FIRMTURNOVER", Convert.ToInt32(row["FirmTurnover"]));
            base.AddMeter("FIRMTOTALWINS", Convert.ToInt32(row["FirmTotalWins"]));
        }
    }

    /// <summary>
    /// CMS Link meter read class
    /// </summary>
    public class CMSLinkMeterRead : CMSMeterReadBase
    {
        public CMSLinkMeterRead(DataRow row)
            : base(CMSMeterSetType.Link, row)
        {
        }
    }

    /// <summary>
    /// List for CMS meter reads
    /// </summary>
    public class CMSMeterReads : List<ICMSMeterRead>
    {
    }
}
