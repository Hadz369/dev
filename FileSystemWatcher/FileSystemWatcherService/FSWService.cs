using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.ServiceModel;

namespace FSW
{
    public class FSWService
    {
        ConcurrentQueue<EventArgs> _queue;
        Thread _queueWatcher;
        bool _monitorQueue = false;

        public FSWService()
        {
            Cache.SubscriberChannels.TryAdd("FSW", new SubscriberCollection("FSW"));
        }

        public void Start()
        {
            Log.Debug("Starting the file system watcher service");

            _queue = new ConcurrentQueue<EventArgs>();
            _queueWatcher = new Thread(new ThreadStart(MonitorQueue));
            _monitorQueue = true;
            _queueWatcher.Start();
            Log.Debug("Queue watcher started");

            FileSystemWatcher watcher = new FileSystemWatcher(@"C:\Temp");
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Only watch text files.
            //watcher.Filter = "*.txt";

            // Add event handlers.

            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
            Log.Debug("File system watcher started");

            while (_monitorQueue)
                Thread.Sleep(1);

            _monitorQueue = false;

            if (_queueWatcher != null)
                _queueWatcher.Join(3000);

            watcher.EnableRaisingEvents = false;
        }

        public void Stop()
        {
            _monitorQueue = false;

            try
            {
                _queueWatcher.Join(3000);
            }
            catch
            {
                try { _queueWatcher.Abort(); }
                catch { }
            }
        }

        // Define the event handlers.
        private void OnChanged(object source, EventArgs e) 
        {
            _queue.Enqueue(e); 
        }

        void MonitorQueue()
        {
            while (_monitorQueue)
            {
                if (_queue.Count > 0)
                {
                    EventArgs e = null;
                    if (_queue.TryDequeue(out e))
                    {
                        var msg = new FSWMessage(e);

                        if (msg.ChangeType != (int)WatcherChangeTypes.Deleted)
                        {
                            msg.IsFolder = Directory.Exists(msg.Path);
                        }

                        Packet p = PacketHandler.GetPacket(PacketType.Message, "FSW", msg.ChangeType.ToString());
                        p.Body = new Body(p, msg);

                        try
                        {
                            int x = Cache.SubscriberChannels["FSW"].SendPacket(p);
                            Log.Debug(String.Format("Packet sent to {0} subscriber{1}", x, x == 0 || x > 1 ? "s" : ""));
                        }
                        catch (KeyNotFoundException)
                        {
                            Log.Debug("Error sending callback: The FSW channel was not found in the channel collection");
                        }
                        catch (Exception ex)
                        {
                            Log.Debug(String.Format("Unknown error sending FSW change: Type={0}, Msg={1}", ex.GetType(), ex.Message));
                        }
                    }
                }

                Thread.Sleep(1);
            }
        }
    }
}
