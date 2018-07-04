using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Metro.Service
{
    [DataContract(Namespace = "")]
    public class MemberInfo
    {
        public MemberInfo(System.Data.DataRow row)
        {
            EPSID = Convert.ToInt32(row["EPSID"]);
        }

        [DataMember] 
        public Int32 EPSID { get; set; }
        [DataMember]
        public string CardNumber { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public Int32 MembNumber { get; set; }
        [DataMember]
        public DateTime Birthday { get; set; }
        [DataMember]
        public Decimal DollarCredit { get; set; }
        [DataMember]
        public Decimal LockedCredit { get; set; }
        [DataMember]
        public Decimal MaxAccountCredit { get; set; }
        [DataMember]
        public Int32 PointsBalance { get; set; }
        [DataMember]
        public DateTime CardLastUsed { get; set; }
        [DataMember]
        public DateTime ETCardLastUsed { get; set; }
        [DataMember]
        public DateTime ExpiresAt { get; set; }
        [DataMember]
        public Boolean IsAttendantCard { get; set; }
        [DataMember]
        public Boolean IsClrnceCard { get; set; }
        [DataMember]
        public Boolean IsCardLost { get; set; }
        [DataMember]
        public Boolean IsSuspended { get; set; }
        [DataMember]
        public Boolean AllowCashless { get; set; }
        [DataMember]
        public Int16 PINFails { get; set; }
        [DataMember]
        public Boolean AccountEnabled { get; set; }
        [DataMember]
        public Boolean AccountSuspended { get; set; }
        [DataMember]
        public Boolean BptAccountEnabled { get; set; }
        [DataMember]
        public Boolean BptAccountSuspended { get; set; }
    }
}
