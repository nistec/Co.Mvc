using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netcell.Data;
using Netcell.Data.Db.Entities;
//using Nistec.Messaging;
using System.Data;

namespace Netcell.Remoting
{
    public class SchedulerHandler
    {
        public static SchedulerHandler Instance
        {
            get { return new SchedulerHandler(); }
        }

        public int RenderScheduler_Queue(int accountId, int itemId, int argId, decimal itemPrice, int userId, DateTime timeToSend, SchedulerDataSource dataSource = SchedulerDataSource.Batch)
        {
            bool isPending = DateTime.Now.AddMinutes(2) < timeToSend;
            Scheduler_Queue q = new Scheduler_Queue()
            {
                AccountId = accountId,
                ArgId = argId,
                Creation = DateTime.Now,
                DataSource = dataSource.ToString(),
                ExecTime = timeToSend,
                Expiration = Scheduler_Queue.GetDefaultExpiration(timeToSend),
                ItemPrice = itemPrice,
                ItemsCount = 1,
                ItemType = isPending ? (int)SchedulerItemType.Executed : (int)SchedulerItemType.Scheduled,
                ItemId = itemId,
                ItemIndex = 0,
                ItemRange = 0,
                UserId = userId,
                Server = AppServers.NextServer(PlatformType.Cell),


            };

            return Scheduler_Queue_Context.Insert(q);
        }

        public int RenderScheduler_Queue(int accountId, int itemId, int argId, int itemsCount, int itemIndex, int itemRange, decimal itemPrice, int userId, DateTime timeToSend, SchedulerDataSource dataSource = SchedulerDataSource.Batch)
        {
            bool isPending = DateTime.Now.AddMinutes(2) < timeToSend;
            Scheduler_Queue q = new Scheduler_Queue()
            {
                AccountId = accountId,
                ArgId = argId,
                Creation = DateTime.Now,
                DataSource = dataSource.ToString(),
                ExecTime = timeToSend,
                Expiration = Scheduler_Queue.GetDefaultExpiration(timeToSend),
                ItemPrice = itemPrice,
                ItemsCount = itemsCount,
                ItemType = isPending ? (int)SchedulerItemType.Executed : (int)SchedulerItemType.Scheduled,
                ItemId = itemId,
                ItemIndex = itemIndex,
                ItemRange = itemRange,
                UserId = userId,
                Server = AppServers.NextServer(PlatformType.Cell),


            };

            return Scheduler_Queue_Context.Insert(q);
        }

        public int RenderScheduler_Queue(int accountId, int itemId, BatchListItem[] list, decimal itemPrice, int userId, DateTime timeToSend, SchedulerDataSource dataSource = SchedulerDataSource.Batch)
        {
            int count = 0;
            for (int i = 0; i < list.Length; i++)
            {
                BatchListItem item = list[i];
                RenderScheduler_Queue(accountId, itemId, item.BatchId,item.BatchValue,item.BatchIndex,item.BatchRange, item.BatchPrice, userId, item.SendTime, SchedulerDataSource.Batch);
                count++;
            }

            return count;
        }
    }
}
