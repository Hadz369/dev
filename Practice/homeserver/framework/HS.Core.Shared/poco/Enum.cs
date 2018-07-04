using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HS
{
    public enum SyncResult
    {
        Success = 0,
        Error = 1,
        AlreadyProcessed = 3
    }

    public enum SyncType
    {
        Member = 0,
        Tier   = 1,
        Photo  = 2
    }

    public enum Metric
    {
        FloorPacketsRcvd,
        FloorPacketsSent,
        ControlPacketsRcvd,
        ControlPacketsSent
    }

    public enum ExecutionMode
    {
        Console,
        Service
    }

    public enum EventLevel
    {
        Debug,
        Information,
        Warning,
        Error,
        Fatal
    }
}
