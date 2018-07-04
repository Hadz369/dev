using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace FSW
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class FSWServiceContract : IFSWService
    {
        public Guid Connect(string providerKey, int deviceCode)
        {
            Guid guid;
            Session session;

            try
            {
                guid = Cache.Sessions.Connect(providerKey, deviceCode, out session);
                session.Callback = OperationContext.Current.GetCallbackChannel<IFSWServiceCallback>();

                return guid;
            }
            catch (Exception ex)
            {
                if (ex is FaultException<FaultData>) throw;

                FaultData fd = FaultHandler.GetFaultData(ErrorCode.FaultException, ex, "Subscribe");
                throw new FaultException<FaultData>(fd, new FaultReason("A general fault occured during the connect call"));
            }
        }

        public bool Disconnect(Guid sessionKey)
        {
            try
            {
                Session s = Cache.Sessions[sessionKey];

                if (s.IsOpen)
                {
                    s.Disconnect();
                    return true;
                }
            }
            catch { /* just return false */ }

            return false;
        }

        public bool KeepAlive(Guid sessionKey)
        {
            try
            {
                Session s = Cache.Sessions[sessionKey];

                if (s.IsOpen)
                {
                    s.SetLastActivity();
                    return true;
                }
            }
            catch { /* just return false */ }

            return false;
        }

        /*
        public void Subscribe(Guid sessionKey, string[] feeds)
        {
            Cache.Sessions.CheckAuthorisation(sessionKey, "Communication", "Subscribe");

            try
            {
                Cache.SubscriberChannels.Subscribe(sessionKey, new string[] { "FSW" });
            }
            catch (Exception ex)
            {
                FaultData fd = FaultHandler.GetFaultData(ErrorCode.FaultException, ex, "Subscribe");
                throw new FaultException<FaultData>(fd, new FaultReason("A general fault occured during the subscribe call"));
            }
        }

        public void Unsubscribe(Guid sessionKey, string[] channels)
        {
            Cache.Sessions.CheckAuthorisation(sessionKey, "Communication", "Unsubscribe");

            try
            {
                Cache.SubscriberChannels.Unsubscribe(sessionKey, channels);
            }
            catch (Exception ex)
            {
                FaultData fd = FaultHandler.GetFaultData(ErrorCode.FaultException, ex, "Unsubscribe");
                throw new FaultException<FaultData>(fd, new FaultReason("A general fault occured during the unsubscribe call"));
            }
        }
        */

        public Packet Execute(Packet packet)
        {
            Cache.Sessions.CheckAuthorisation(packet);

            try
            {
                Packet rsp = null;

                try
                {
                    Log.Debug(String.Concat("Packet received: ", JsonConvert.SerializeObject(packet)));


                    rsp = PacketHandler.PrepareResponse(packet, packet.Body);
                }
                catch (Exception ex)
                {
                    rsp = PacketHandler.PrepareResponse(packet, FaultHandler.GetFaultData(ErrorCode.GeneralError, ex));
                }

                return rsp;
            }
            catch (Exception ex)
            {
                if (ex is FaultException<FaultData>) throw;

                FaultData fd = FaultHandler.GetFaultData(ErrorCode.FaultException, ex, "Execute");
                throw new FaultException<FaultData>(fd, new FaultReason("A general fault occured during the execute call"));
            }
        }

        public void ExecuteAsync(Packet packet)
        {
            Cache.Sessions.CheckAuthorisation(packet);

            try
            {
                ThreadPool.QueueUserWorkItem(AsyncProcessRequest, packet);
            }
            catch (Exception ex)
            {
                FaultData fd = FaultHandler.GetFaultData(ErrorCode.FaultException, ex, "ExecuteAsync");
                throw new FaultException<FaultData>(fd, new FaultReason("A general fault occured during the execute async call"));
            }
        }

        void AsyncProcessRequest(object state)
        {
            try
            {
                Packet packet = state as Packet;

                if (packet != null)
                {
                    var session = Cache.Sessions[packet.Header.SessionKey];

                    Packet rsp = PacketHandler.PrepareResponse(packet, packet.Body);
                    session.Callback.Callback(rsp);
                }
            }
            catch (Exception ex)
            {
                Log.Debug(String.Format("A unhandled exception occured during async execution: Msg={0}{1}", ex.Message, ex.StackTrace));
            }
        }
    }
}
