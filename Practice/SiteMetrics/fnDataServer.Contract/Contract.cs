using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace fnDataServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ThirdPartyContract : IService
    {
        BusinessRuleLayer1 _business = BusinessRuleLayer1.Instance;

        public void Dispose()
        {
            _business.Dispose();
        }

        public SignonResponseData SignOn(SignonData Data)
        {
            return _business.SignOn(Data);
        }

        public bool SignOff(int Handle)
        {
            Console.WriteLine("SignOff packet received. Handle=" + Handle.ToString());
            _business.SignOff(Handle);

            return true;
        }

        public ErrorMessage PutDataPacket(DataPacket packet)
        {
            Console.WriteLine("Data packet received");
            return _business.PutDataPacket(packet);
        }

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public class TimeTrackerContract : IService
        {
            BusinessRuleLayer1 _business = BusinessRuleLayer1.Instance;

            public void Dispose()
            {
                _business.Dispose();
            }

public GetPeople()
{
}
            public ErrorMessage PutDataPacket(DataPacket packet)
            {
                Console.WriteLine("Data packet received");
                return _business.PutDataPacket(packet);
            }
        }
    }
}
