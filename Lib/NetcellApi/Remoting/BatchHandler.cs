using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netcell.Data;
using Netcell.Data.Entities;
using MControl.Messaging;
using System.Data;

namespace Netcell.Remoting
{
    public class BatchHandler
    {
        public static BatchHandler Instance
        {
            get { return new BatchHandler(); }
        }

        public int RenderBatchCell(int accountId, MethodType method, string sender, string message, List<TargetListItem> targetList, DateTime timeToSend)
        {

            if (targetList == null)
            {
                throw new ArgumentNullException("RenderBatchCell.targets");
            }

            bool isPending=timeToSend>DateTime.Now.AddMinutes(2);
            //var targetList = TargetListItem.SplitTarget(targets);
            int userId = 0;
            bool concat = true;
            //MethodType methodType = MsgMethod.ToMethodType(method);
            BillingType billType = BillingType.CB;
            SchedulerDataSource dataSource = isPending ? SchedulerDataSource.Pending : SchedulerDataSource.Batch;
            byte bunch = UnitsItem.GetBunch(accountId);
            decimal price = 0;
            var ui = UnitsItem.CreateUnitsItem(method, message, concat, bunch);
            BillingItem.ValidateCredit(accountId, method, targetList.Count(), ui, ref price);
            int units = ui.GetSMSBillingUnits(ui.Bunch);
            
            int batchId = 0;

            using (DalController dal = new DalController())
            {
                dal.Trans_Batch_Insert(ref batchId, accountId, 0, 1, BatchTypes.Single, 0, 0, PlatformType.Cell, (int)method, price,null);
            }

            //using (Counters counter = new Counters())
            //{
            //    batchId = counter.AddBatch(accountId, 0, (int)method, (int)billType, 0, ViewConfig.Server.ToString(), "BatchHandler.RenderBatchCell");
            //}

            /*
            //change:Trans_Batch_View
            Trans_Batch_View view = new Trans_Batch_View()
            {
                BatchId = batchId,
                Body = message,
                Modified = DateTime.Now,
                PlatformView = (int)PlatformType.Cell,
                Size = message.Length,
                Units = units,
            };

            Trans_Batch_View_Context.Insert(view);
            */

            Trans_Batch_Content view = new Trans_Batch_Content()
            {
                BatchId = batchId,
                Message = message,
                PlatformView = (int)PlatformType.Cell,
                Size = message.Length,
                Units = units,
            };

            Trans_Batch_Content_Context.Insert(view);



            TargetUtil.RenderDestination(0, targetList.ToList<TargetListItem>(), batchId, timeToSend, PlatformType.Cell);

            Scheduler_Queue q = new Scheduler_Queue()
            {
                AccountId = accountId,
                ArgId = batchId,
                Creation = DateTime.Now,
                DataSource = dataSource.ToString(),
                ExecTime = timeToSend,
                Expiration = Scheduler_Queue.GetDefaultExpiration(timeToSend),
                ItemPrice = price,
                ItemsCount = targetList.Count(),
                ItemType = isPending ? (int)SchedulerItemType.Executed : (int)SchedulerItemType.Scheduled,
                ItemId = 0,
                ItemIndex = 0,
                ItemRange = 0,
                UserId = userId,
                Server = AppServers.NextServer(PlatformType.Cell)

            };

            Scheduler_Queue_Context.Insert(q);

            return batchId;

        }

        public int RenderBatchCell(int batchId,int accountId, MethodType method, string sender, string message, List<TargetListItem> targetList, DateTime timeToSend)
        {

            if (targetList == null)
            {
                throw new ArgumentNullException("RenderBatchCell.targets");
            }

            bool isPending = timeToSend > DateTime.Now.AddMinutes(2);
            //var targetList = TargetListItem.SplitTarget(targets);
            int userId = 0;
            bool concat = true;
            //MethodType methodType = MsgMethod.ToMethodType(method);
            BillingType billType = BillingType.CB;
            SchedulerDataSource dataSource = isPending ? SchedulerDataSource.Pending : SchedulerDataSource.Batch;
            byte bunch = UnitsItem.GetBunch(accountId);
            decimal price = 0;
            var ui = UnitsItem.CreateUnitsItem(method, message, concat, bunch);
            BillingItem.ValidateCredit(accountId, method, targetList.Count(), ui, ref price);
            int units = ui.GetSMSBillingUnits(ui.Bunch);

            //int batchId = 0;

            //using (DalController dal = new DalController())
            //{
            //    dal.Trans_Batch_Insert(ref batchId, accountId, 0, 1, BatchTypes.Single, 0, 0, PlatformType.Cell, (int)method, price, null);
            //}

            //using (Counters counter = new Counters())
            //{
            //    batchId = counter.AddBatch(accountId, 0, (int)method, (int)billType, 0, ViewConfig.Server.ToString(), "BatchHandler.RenderBatchCell");
            //}

            /*
            //change:Trans_Batch_View
            Trans_Batch_View view = new Trans_Batch_View()
            {
                BatchId = batchId,
                Body = message,
                Modified = DateTime.Now,
                PlatformView = (int)PlatformType.Cell,
                Size = message.Length,
                Units = units,
            };

            Trans_Batch_View_Context.Insert(view);
            */

            Trans_Batch_Content view = new Trans_Batch_Content()
            {
                BatchId = batchId,
                Message = message,
                PlatformView = (int)PlatformType.Cell,
                Size = message.Length,
                Units = units,
            };

            Trans_Batch_Content_Context.Insert(view);



            TargetUtil.RenderDestination(0, targetList.ToList<TargetListItem>(), batchId, timeToSend, PlatformType.Cell);

            Scheduler_Queue q = new Scheduler_Queue()
            {
                AccountId = accountId,
                ArgId = batchId,
                Creation = DateTime.Now,
                DataSource = dataSource.ToString(),
                ExecTime = timeToSend,
                Expiration = Scheduler_Queue.GetDefaultExpiration(timeToSend),
                ItemPrice = price,
                ItemsCount = targetList.Count(),
                ItemType = isPending ? (int)SchedulerItemType.Executed : (int)SchedulerItemType.Scheduled,
                ItemId = 0,
                ItemIndex = 0,
                ItemRange = 0,
                UserId = userId,
                Server = AppServers.NextServer(PlatformType.Cell)

            };

            Scheduler_Queue_Context.Insert(q);

            return batchId;

        }

    }
}
