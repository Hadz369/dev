using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;
using Newtonsoft.Json;

namespace FSW
{
    class AsyncRequestWrapper
    {
        Packet _response = null;

        public bool ResponseReceived { get; set; }

        public Packet Req { get; set; }
        public Packet Rsp { get { return _response; } }

        public bool SetResponse(Packet response)
        {
            if (Req.Header.SessionKey == response.Header.SessionKey
                && Req.Header.CommandId == response.Header.CommandId
                && Req.Header.CommandClass == response.Header.CommandClass
                && Req.Header.Command == response.Header.Command)
            {
                _response = response;
                ResponseReceived = true;
            }

            return ResponseReceived;
        }
    }

    class Program
    {
        static List<AsyncRequestWrapper> _AsyncRequestWrapperList = new List<AsyncRequestWrapper>();
        static FSWClient _client = new FSWClient("testProvider", 121);

        static void Main(string[] args)
        {
            Log.Initialise("FSWClient", false);
            Log.ConfigureDefaultTrace(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

            FSWCallbackEvent _callback = new FSWCallbackEvent();
            _callback.CallbackEvent += _callback_CallbackEvent;

            try
            {
                 _client.Connect();

                 if (_client.IsConnected)
                 {
                    try
                    {
                        _client.
                            p.Body = new string[] { "FSW" };

                            p = svc.Execute(p);

                            Packet p1 = svc.Execute(PacketHandler.GetPacket((int)PacketType.Request, "TestCommand1", "GetStuffForCommand1", sessionKey));
                            Log.Debug(JsonConvert.SerializeObject(p1));
                        }
                        catch (Exception ex)
                        {
                            Log.Debug("Error: " + ex.Message);
                        }

                        Packet req = null;

                        try
                        {
                            req = PacketHandler.GetPacket((int)PacketType.Request, "TestCommand2", "GetStuffForCommand2", sessionKey);

                            PropertyBag props = new PropertyBag();
                            props.Add("Test1", 123);
                            props.Add("Test2", DateTime.UtcNow);
                            req.Body = props;

                            Packet rsp = svc.Execute(req);
                            Log.Debug(String.Format("Response: {0}", JsonConvert.SerializeObject(rsp, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore })));
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Error during execute request", ex);
                        }

                        try
                        {
                            AsyncRequestWrapper r = new AsyncRequestWrapper() { Req = req };
                            _AsyncRequestWrapperList.Add(r);                     
       
                            svc.ExecuteAsync(req);

                            while (!r.ResponseReceived) { Thread.Sleep(10); }

                            Log.Debug(String.Concat("Callback: ", JsonConvert.SerializeObject(r.Rsp, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore })));

                        }
                        catch (FaultException<FaultData> ex)
                        {
                            Log.Error("Fault exception received from server", ex);
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Error during async service call", ex);
                        }

                        Console.ReadLine();

                        try
                        {
                            Packet p = PacketHandler.GetPacket((int)PacketType.Request, "Communication", "Subscribe");
                            p.Body = new string[] { "FSW" };

                            p = svc.Execute(p);
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Error during unsubscribe call", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug(ex.Message);
            }

            Console.ReadLine();
        }

        static void _callback_CallbackEvent(Packet packet)
        {
            if (packet != null)
            {
                if (packet.Header.PacketType == 1) // Response
                {
                    foreach (AsyncRequestWrapper req in _AsyncRequestWrapperList)
                    {
                        if (req.SetResponse(packet))
                            break;
                    }
                }
                else
                    Log.Debug(String.Format("Message: {0}", JsonConvert.SerializeObject(packet.Body)));
            }
        }
    }
}
