using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace fnDataServer
{
    public class BusinessRuleLayer1 : IDisposable
    {
        DataLayer _data;

        #region Singleton Initialisation

        private static readonly BusinessRuleLayer1 instance = new BusinessRuleLayer1();

        private BusinessRuleLayer1()
        {
            _data = new DataLayer();
        }

        public static BusinessRuleLayer1 Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        List<int> handleList = new List<int>();

        public void Dispose()
        {
            if (_data != null)
            {
                _data.Dispose();
                _data = null;
            }
        }

        public SignonResponseData SignOn(SignonData Data)
        {
            Console.WriteLine("SignOn packet received. DeviceId=" + Data.DeviceId.ToString() + ", VendorName=" + Data.VendorName);

            ErrorMessage emsg;

            if (_data.ValidateSignOn(Data, out emsg)) 
                return new SignonResponseData(GenerateHandle());
            else 
                return new SignonResponseData(emsg);
        }

        public void SignOff(int Handle)
        {
            if (handleList.Contains(Handle))
                handleList.Remove(Handle);
        }

        public int GenerateHandle()
        {
            int h = 0;
            Random r = new Random();

            while (true)
            {
                h = r.Next();
                if (!handleList.Contains(h))
                {
                    handleList.Add(h);
                    break;
                }                
            }

            return h;
        }

        public ErrorMessage PutDataPacket(DataPacket packet)
        {
            Console.WriteLine("Data received: Type={0}, Handle={1}, Rows={2}", packet.Type, packet.Handle, packet.Data.Count);
            DataTable dt = packet.GetDataTable();
            Console.WriteLine("Columns={0}, Rows={1}", dt.Columns.Count, dt.Rows.Count);
            
            return null;
        }
    }
}
