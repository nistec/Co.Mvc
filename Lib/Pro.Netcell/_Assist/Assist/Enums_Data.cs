using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netcell
{

    public enum BatchTypes
    {
        Single = 0,
        Multi = 1,
        Auto = 2,
        Scheduled=3,//not used
        Api=4,
        Remote = 5,
        Preview = 6,
        RestApi = 7,//not used
        SyncApi = 8,//not used
        Group = 9,//not used
        GroupCo = 10,
        ScheduledPart = 11//not used
    }

    public enum PlatformType
    {
        NA = 0,
        Cell = 1,
        Mail = 2
    }

    public enum PlatformEnv
    {
        NA = 0,
        Cell = 1,
        Web = 2,
    }

    public enum SchedulerDataSource
    {
        Batch,
        Watch,
        Reminder,
        Fixed,
        Pending,
        Alert,

    }

    public enum SchedulerState
    {
        Empty = 0,
        Exec = 1,
        Expired = 2,
        Canceled = 3,
        Error = 4,
    }

    public enum SchedulerItemType
    {
        Scheduled = 0,
        Executed = 1,
        Preview = 6
    }

    public enum VersionView
    {
        Html = 0,
        Preview = 6
    }

}
