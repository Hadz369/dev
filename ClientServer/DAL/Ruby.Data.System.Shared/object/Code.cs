using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.ServiceModel;
using System.Data;

namespace Ruby.Data
{
    [DataContract]
    public class Code
    {
        public Code(DataRow row)
        {
            CodeTypeId = Convert.ToInt32(row[0]);
            CodeTypeDefn = row[1].ToString();
            CodeGuid = (Guid)row[2];
            CodeId = Convert.ToInt32(row[3]);
            CodeDefn = row[4].ToString();
            CodeName = row[5].ToString();
            CodeDesc = row[6].ToString();
            CodeValue = row[7].ToString();
            Sequence = Convert.ToInt32(row[8]);

        }

        [DataMember]
        public int CodeTypeId { get; set; }
        [DataMember]
        public string CodeTypeDefn { get; set; }
        [DataMember]
        public int CodeId { get; set; }
        [DataMember]
        public Guid CodeGuid { get; set; }
        [DataMember]
        public string CodeDefn { get; set; }
        [DataMember]
        public string CodeName { get; set; }
        [DataMember]
        public string CodeDesc { get; set; }
        [DataMember]
        public string CodeValue { get; set; }
        [DataMember]
        public int Sequence { get; set; }

        public void Update(Code newcode)
        {
            CodeName = newcode.CodeName;
            CodeDesc = newcode.CodeDesc;
            CodeValue = newcode.CodeValue;
            Sequence = newcode.Sequence;
        }
    }
}
