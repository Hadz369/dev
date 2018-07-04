using System;
using System.Collections.Generic;
using System.Text;

namespace Alma.Event
{
    public enum EventLevel
    {
        Debug,
        Information,
        Warning,
        Error,
        Fatal
    }

    public enum TaskType
    {
        Database,
        FileArchive,
        RemoteCopy,
        FileCleanup,
        FileSync
    }

    public enum TaskManagerState
    {
        Starting,
        Started,
        Paused,
        Stopping,
        Stopped
    }

    public enum JobState
    {
        Starting,
        Completed,
        Failed
    }

    public enum JobType
    {
        Backup,
        Restore,
        Reindex, 
        FileCopy,
        FileCleanup
    }

    public enum SchedulePeriod
    {
        Month,
        Week,
        Day,
        Hour,
        Minute
    }

    public enum DatabaseBackupType
    {
        FullBackup,
        DifferentialBackup,
        TransactionLogBackup
    }

    public enum DatabaseOperationMode
    {
        Backup,
        Restore
    }

    enum ParmValueType : int
    {
        Unknown = 0,
        AlphaNumeric = 4,
        Text = 5,
        Ordinal = 6,
        Integer = 7,
        Decimal = 8,
        Money = 9,
        Boolean = 10,
        List = 11,
        Named = 12
    }

    public enum EventGroup
    {
        SystemEvents = 1,
        MachineHandlerEvents = 2,
        DeviceHandlerEvents = 3,
        MembershipHandlerEvents = 4,
        WideAreaTicketEvents = 5,
        WideAreaMemberEvents = 6
    }

    public enum EventType
    {
        MachineChange = 40
    }

    public enum AuditEventType
    {
        MaintenanceHandlerEvent = 1001,
        MembershipHandlerEvent = 1002
    }

    public enum ExecutionMode
    {
        Console,
        Service
    }
}
