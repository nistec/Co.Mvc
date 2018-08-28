using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netcell.Data
{
    public interface ISchedulerItem
    {
        int AccountId { get; }
        int ArgId { get; }
        DateTime Creation { get; }
        string DataSource { get; }
        DateTime ExecTime { get; }
        DateTime Expiration { get; }
        decimal ItemPrice { get; }
        int ItemsCount { get; }
        int ItemType { get; }
        int ItemId { get; }
        //ItemIndex {get;}
        //ItemRange {get;}
        //UserId {get;}
        //Server {get;}
        //QueueId=Guid.NewGuid()
    }
}
