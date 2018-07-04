using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Metro.Service
{
    [DataContract(Namespace = "")]
    public class MachineDetail
    {
        public MachineDetail() { }

        public MachineDetail(IIndexedDataRecord row)
        {
            DeviceId = row.GetValue<Int32>(0);
            GMID = row.GetValue<Int32>(1);
            SerialNo = row.GetValue<String>(2);
            GameName = row.GetValue<String>(3);
            HouseNo = row.GetValue<String>(4);
            BaseNo = row.GetValue<String>(5);
            Denomination = row.GetValue<Int32>(6);
            Location = row.GetValue<String>(7);
            Manufacturer = row.GetValue<String>(8);
            IsMTGM = row.GetValue<Boolean>(9);
        }

        [DataMember]
        public int DeviceId { get; set; }
        [DataMember]
        public int GMID { get; set; }
        [DataMember]
        public string SerialNo { get; set; }
        [DataMember]
        public string GameName { get; set; }
        [DataMember]
        public string HouseNo { get; set; }
        [DataMember]
        public string BaseNo { get; set; }
        [DataMember]
        public int Denomination { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string Manufacturer { get; set; }
        [DataMember]
        public bool IsMTGM { get; set; }
    }

    [CollectionDataContract(Name = "MachineDetailCollection", ItemName = "MachineDetail")]
    public class MachineDetailCollection : List<MachineDetail> { }
}
