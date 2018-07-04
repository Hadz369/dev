using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ServiceModel;

[assembly: ContractNamespaceAttribute("", ClrNamespace = "RestServer")]
namespace Metro.Data
{
    public static class TpServiceRC
    {
        public static int InvalidCredentials { get { return 40; } }
        public static int InvalidSessionKey { get { return 41; } }
        public static int Unauthorised { get { return 42; } }
        public static int SessionExpired { get { return 43; } }

        public static int UnhandledError { get { return 50; } }
        public static int SqlError { get { return 51; } }

        public static int InvalidParameter { get { return 60; } }
        public static int NoRecordsFound { get { return 61; } }

    }

    [DataContract]
    public class FaultData
    {
        public FaultData(int code, string message)
        {
            Code = code;
            Message = message;
        }

        [DataMember]
        public int Code { get; private set; }

        [DataMember]
        public string Message { get; private set; }
    }

    //[KnownType(typeof(Parm))]
    [DataContract(Namespace = "")]
    public class RequestData
    {
        List<Parm> _parms = new List<Parm>();

        [DataMember]
        public List<Parm> Parms { get { return _parms; } set { _parms = value; } }

        public string GetValue(string name)
        {
            string val = "";

            foreach (Parm p in _parms)
            {
                if (p.Name == name)
                {
                    val = p.Value;
                    break;
                }
            }

            return val;
        }
    }

    [DataContract(Namespace = "")]
    public class Parm
    {
        public Parm() { }

        public Parm(string name, string value)
        {
            Name = name;
            Value = value;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }
    }

    [KnownType(typeof(Member))]
    [KnownType(typeof(MemberList))]
    [KnownType(typeof(Tier))]
    [KnownType(typeof(TierList))]
    [KnownType(typeof(Machine))]
    [KnownType(typeof(MachineList))]
    [KnownType(typeof(MachineStats))]
    [KnownType(typeof(AccountTransaction))]
    [KnownType(typeof(Session))]
    [KnownType(typeof(SessionCollection))]
    [DataContract(Namespace = "")]
    public class ServiceResponse
    {
        int _rc = 0, _records = 1;
        string _msg = "";
        object _resp = String.Empty;

        [DataMember]
        public int ReturnCode { get { return _rc; } set { _rc = value; } }

        [DataMember]
        public string ErrorMessage { get { return _msg; } set { _msg = value; } }

        [DataMember]
        public Exception Exception { get; set; }

        [DataMember]
        public object ResponseData
        {
            get { return _resp; }
            //set { _resp = value; }
            set { SetResponse(value); }
        }

        [DataMember]
        public int Records { get { return _records; } set { _records = value; } }

        private void SetResponse(object value)
        {
            _resp = value;

            IObjectList list = value as IObjectList;
            if (list != null)
            {
                _records = list.ObjCount == 0 ? 1 : list.ObjCount; // Always return 1 unless greater.
            }
        }
    }

    [DataContract(Namespace = "")]
    public class Member
    {
        public Member() { }

        public Member(IDataReader reader)
        {
            MemberId = reader.GetInt32(0);
            BadgeNo = reader.GetInt32(1).ToString();
            FirstName = reader.GetString(2);
            LastName = reader.GetString(3);
            BirthDate = reader.GetDateTime(4);
            ExpiryDate = reader.GetDateTime(5);
            PointBalance = reader.GetDecimal(6);
            Status = reader.GetString(7);
            Tier = reader.GetInt32(8);
        }

        [DataMember]
        public int MemberId { get; set; }

        [DataMember(IsRequired = true)]
        public string BadgeNo { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public DateTime BirthDate { get; set; }

        [DataMember]
        public DateTime ExpiryDate { get; set; }

        [DataMember]
        public Decimal PointBalance { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public Int32 Tier { get; set; }
    }

    [CollectionDataContract(Namespace = "", Name = "Members", ItemName = "Member")]
    public class MemberList : List<Member>, IObjectList
    {
        public MemberList() : base() { }

        public MemberList(Member[] items)
            : base()
        {
            foreach (Member item in items)
            {
                Add(item);
            }
        }

        public int ObjCount { get { return this.Count; } }
    }

    [DataContract(Namespace = "")]
    public class Tier
    {
        public Tier() { }

        public Tier(IDataRecord record)
        {
            TierId = record.GetInt32(0);
            Name = record.GetString(1);
            Description = record.GetString(2);
            Factor = record.GetInt32(3);
            PostFactor = record.GetInt32(4);
            Colour = record[5] as string ?? default(string);
        }

        [DataMember]
        public Int32 TierId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public Int32 Factor { get; set; }

        [DataMember]
        public Int32 PostFactor { get; set; }

        [DataMember]
        public string Colour { get; set; }
    }

    [CollectionDataContract(Namespace = "", Name = "Tiers", ItemName = "Tier")]
    public class TierList : List<Tier>, IObjectList
    {
        public TierList() : base() { }

        public TierList(Tier[] items)
            : base()
        {
            foreach (Tier item in items)
            {
                Add(item);
            }
        }

        public int ObjCount { get { return this.Count; } }
    }

    [DataContract(Namespace = "")]
    public class Machine
    {
        public Machine() { }

        public Machine(IDataRecord record)
        {
            MachineId = record.GetInt32(0);
            Status = record.GetString(1);
            GMID = record.GetInt32(2);
            SerialNo = record.GetString(3);
            Name = record.GetString(4);
            HouseNo = record.GetInt32(5);
            BaseNo = record.GetInt32(6);
            Denomination = record.GetInt32(7);
            Floor = record.GetString(8);
            Manufacturer = record.GetString(9);
            IsMtgm = (record.GetInt32(10) == 0 ? false : true);
            HopperCount = record.GetInt32(11);
            VarNo = record.GetString(12);
            SpecNo = record.GetString(13);
            RTP = record.GetDecimal(14);
            Updated = record.GetDateTime(15);
        }

        [DataMember]
        public Int32 MachineId { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public Int32 GMID { get; set; }

        [DataMember]
        public string SerialNo { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Int32 HouseNo { get; set; }

        [DataMember]
        public Int32 BaseNo { get; set; }

        [DataMember]
        public Int32 Denomination { get; set; }

        [DataMember]
        public string Floor { get; set; }

        [DataMember]
        public string Manufacturer { get; set; }

        [DataMember]
        public bool IsMtgm { get; set; }

        [DataMember]
        public Int32 HopperCount { get; set; }

        [DataMember]
        public string VarNo { get; set; }

        [DataMember]
        public string SpecNo { get; set; }

        [DataMember]
        public decimal RTP { get; set; }

        [DataMember]
        public DateTime Updated { get; set; }
    }

    [CollectionDataContract(Namespace = "", Name = "Machines", ItemName = "Machine")]
    public class MachineList : List<Machine>, IObjectList
    {
        public MachineList() : base() { }

        public MachineList(Machine[] items)
            : base()
        {
            foreach (Machine item in items)
            {
                Add(item);
            }
        }

        public int ObjCount { get { return this.Count; } }
    }

    [DataContract(Namespace = "")]
    public class MachineStats
    {
        public MachineStats() { }

        public MachineStats(IDataRecord record)
        {
            MachineId = record.GetInt32(0);
            Turnover = Convert.ToInt64(record[1]);
            Wins = Convert.ToInt64(record[2]);
            CashBox = Convert.ToInt64(record[3]);
            CanclCred = Convert.ToInt64(record[4]);
            MoneyIn = Convert.ToInt64(record[5]);
            MoneyOut = Convert.ToInt64(record[6]);
            CashIn = Convert.ToInt64(record[7]);
            CashOut = Convert.ToInt64(record[8]);
            Jackpot = Convert.ToInt64(record[9]);
            Stroke = Convert.ToInt64(record[10]);
            ExistCred = Convert.ToInt64(record[10]);
            BadgeNo = Convert.ToInt64(record[10]);
            MemberTO = Convert.ToInt64(record[10]);
            MemberTOA = Convert.ToInt64(record[10]);
            InPlay = record.GetBoolean(15);
            HotPlay = record.GetBoolean(16);
            Updated = record.GetDateTime(17);
        }

        [DataMember]
        public Int32 MachineId { get; set; }

        [DataMember]
        public Int64 Turnover { get; set; }

        [DataMember]
        public Int64 Wins { get; set; }

        [DataMember]
        public Int64 CashBox { get; set; }

        [DataMember]
        public Int64 CanclCred { get; set; }

        [DataMember]
        public Int64 MoneyIn { get; set; }

        [DataMember]
        public Int64 MoneyOut { get; set; }

        [DataMember]
        public Int64 CashIn { get; set; }

        [DataMember]
        public Int64 CashOut { get; set; }

        [DataMember]
        public Int64 Jackpot { get; set; }

        [DataMember]
        public Int64 Stroke { get; set; }

        [DataMember]
        public Int64 ExistCred { get; set; }

        [DataMember]
        public Int64 BadgeNo { get; set; }

        [DataMember]
        public Int64 MemberTO { get; set; }

        [DataMember]
        public Int64 MemberTOA { get; set; }

        [DataMember]
        public bool InPlay { get; set; }

        [DataMember]
        public bool HotPlay { get; set; }

        [DataMember]
        public DateTime Updated { get; set; }
    }


    [CollectionDataContract(Namespace = "", Name = "Objects", ItemName = "Object")]
    public class ObjectList : List<object[]>, IObjectList
    {
        public ObjectList() : base() { }

        public ObjectList(List<object[]> items)
            : base()
        {
            foreach (object[] item in items)
            {
                Add(item);
            }
        }

        public int ObjCount { get { return this.Count; } }
    }

    [DataContract(Namespace = "")]
    public class AccountTransaction
    {
        [DataMember(IsRequired = true)]
        public int MemberId { get; set; }

        [DataMember(IsRequired = true)]
        public int PrizeId { get; set; }

        [DataMember(IsRequired = true)]
        public string ChangeType { get; set; }

        [DataMember(IsRequired = true)]
        public int Amount { get; set; }
    }

    [DataContract(Namespace = "")]
    public class TestClass
    {
        [DataMember]
        public int TestId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int IntA { get; set; }

        [DataMember]
        public int IntB { get; set; }
    }

    [DataContract]
    public class UserAgentInfo
    {
        public UserAgentInfo(string userAgent)
        {
            UserAgent = userAgent;
        }

        [DataMember]
        public string UserAgent { get; set; }
    }

    [DataContract(Namespace = "")]
    public class Table
    {
        RowCollection _rows;

        public Table()
        {
            _rows = new RowCollection();
        }

        [DataMember]
        public RowCollection Rows { get { return _rows; } set { _rows = value; } }
    }

    [DataContract(Namespace = "")]
    public class Row
    {
        CellCollection _cells;

        public Row()
        {
            _cells = new CellCollection();
        }

        [DataMember]
        public CellCollection Cells { get { return _cells; } set { _cells = value; } }
    }

    [CollectionDataContract(Namespace = "", Name = "Rows", ItemName = "Row")]
    public class RowCollection : List<Row>, IObjectList
    {
        public RowCollection() : base() { }

        public RowCollection(Row[] items)
            : base()
        {
            foreach (Row item in items)
            {
                Add(item);
            }
        }

        public int ObjCount { get { return this.Count; } }
    }

    [DataContract(Namespace = "")]
    public class Cell : Object
    {
        object _obj = null;
        string _str;

        public Cell(object value)
        {
            _obj = value;
            _str = _obj.ToString();
        }

        [DataMember]
        public string Value { get { return _str; } }

        [DataMember]
        public object Object { get { return _obj; } }
    }

    [CollectionDataContract(Namespace = "", Name = "Cells", ItemName = "Cell")]
    public class CellCollection : List<Cell>, IObjectList
    {
        public CellCollection() : base() { }

        public CellCollection(Cell[] items)
            : base()
        {
            foreach (Cell item in items)
            {
                Add(item);
            }
        }

        public int ObjCount { get { return this.Count; } }
    }

    [DataContract(Namespace = "")]
    public class Session
    {
        bool _isExpired = false;

        public Session(string vendor, int device, string sessionkey)
        {
            Vendor = vendor;
            DeviceId = device;
            SessionKey = sessionkey;
            Created = LastUsed = DateTime.Now;
        }

        [DataMember]
        public string Vendor { get; private set; }
        [DataMember]
        public int DeviceId { get; private set; }
        [DataMember]
        public string SessionKey { get; private set; }
        [DataMember]
        public DateTime Created { get; private set; }
        [DataMember]
        public DateTime LastUsed { get; set; }
        [DataMember]
        public bool IsExpired { get { return _isExpired; } set { _isExpired = value; } }
    }

    [CollectionDataContract(Namespace = "", Name = "Sessions", ItemName = "Session")]
    public class SessionCollection : List<Session>, IObjectList
    {
        public SessionCollection() : base() { }

        public SessionCollection(Session[] items)
            : base()
        {
            foreach (Session item in items)
            {
                Add(item);
            }
        }

        public Session Find(string sessionkey)
        {
            Session session = null;

            foreach (Session s in this)
            {
                if (s.SessionKey == sessionkey)
                {
                    session = s;
                    break;
                }
            }

            return session;
        }

        public Session Find(string vendor, int device)
        {
            Session session = null;

            foreach (Session s in this)
            {
                if (s.Vendor == vendor && s.DeviceId == device)
                {
                    session = s;
                    break;
                }
            }

            return session;
        }

        public int ObjCount { get { return this.Count; } }
    }
}
