using System;
using System.Collections.Generic;
using System.Text;

namespace MControl.Messaging
{
    public enum ReceiveState
    {
        Wait = 0,
        Success=1,
        Failed=2,
        Timeout=3
    }

    public enum QueueMode
    {
        Manual = 0,
        Auto = 1
    }

    public enum TransItemState
    {
        Wait = 0,
        Timeout = 1,
        Abort = 2,
        Commit = 3,
    }

    //public enum TransState
    //{
    //    Wait = 0,
    //    Abort = 1,
    //    Commit = 2,
    //}

    public enum ItemState
    {
        Hold = 0,
        Enqueue = 1,
        Dequeue = 2,
        Wait=3,
        Abort = 4,
        Commit = 5,
    }


    //public enum QueueItemStatus
    //{
    //    Enqueue = 0,
    //    Dequeue = 1,
    //    Failed = 2,
    //    Completed = 4,
    //}

    public enum QueueItemType
    {
        QueueItems = 0,
        AttachItems = 1,
        HoldItems = 2,
        AllItems = 4,
    }
    
    public enum Priority
    {
        Normal = 0,
        Medium = 1,
        High = 2
    }

    public enum ChannelType
    {
        AsyncQueue,
        MSMQueue
    }

    //public enum QueueProvider
    //{
    //    None=0,
    //    Embedded=1,
    //    FbServer=2,
    //    SqlServer=3
    //}

    public enum QueueFormatter
    {
        Base64 = 0,
        Bytes = 1
    }
 
    public enum CoverMode
    {
        None=0,
        //ItemsOnly = 1,
        //ItemsAndLog = 2,
        //LogNoState = 3,
        //LogAndState = 4,
        FileSystem = 5
     }

}
