using Netcell;
using Netcell.Data.Db;
using Nistec.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netcell.Remoting
{
    public class BatchRender
    {

        public const int MaxTableItems = 50000;
        public const int MaxRetry = 3;
        public const bool IsAsync = true;

        public static int RenderDestination(int campaignId, TargetItem item, int batchId, DateTime timeSend, string sender, PlatformType platform)
        {
            int minId = 0;
            int maxId = 0;
            Counters.MessageId(1, ref minId, ref maxId);

            int messageId = maxId;

            using (DalController dal = new DalController())
            {
                dal.Trans_Items_Insert(messageId, campaignId, batchId, timeSend, item.Target.Trim(), 0, item.GroupId, item.Coupon, item.Personal, sender, 0, (int)platform, item.ContactId);
            }
            return messageId;
        }


        public static int RenderDestination(int campaignId, List<TargetItem> destList, List<BatchListItem> batchList)
        {
            if (destList == null || destList.Count <= 0)
            {
                throw new Exception("Invalid Destination List");
            }
            if (batchList == null || batchList.Count <= 0)
            {
                throw new Exception("Invalid BatchList");
            }

            object[] values = null;
            int destCount = destList.Count;
            int index = 0;

            Netlog.DebugFormat("Counters.MessageId is CampaignId:{0},destCount:{1},batchList.Count:{2}", campaignId, destCount, batchList.Count);

            int minId = 0;
            int maxId = 0;
            Counters.MessageId(destCount, ref minId, ref maxId);
            int sentId = minId;


            for (int i = 0; i < batchList.Count; i++)
            {
                int batchItems = batchList[i].BatchValue;
                int batchId = batchList[i].BatchId;
                int batchIndex = batchList[i].BatchIndex;
                DateTime timeSend = batchList[i].SendTime;
                DataTable dt = TargetItem.Target_Items_Schema();

                Netlog.DebugFormat("RenderDestination batch is batchItems:{0},batchId:{1},batchIndex:{2},minId:{3}, maxId:{4}", batchItems, batchId, batchIndex, minId, maxId);


                for (int j = 0; j < batchItems && index < destCount && sentId <= maxId; j++)
                {
                    var item = destList[index];
                    //Log.DebugFormat("RenderDestination item is item.Target:{0},item.Personal:{1}", item.Target, item.Personal);

                    values = TargetItem.ItemArray(item, sentId, campaignId, batchId, timeSend, index);
                    if (values != null)
                    {
                        dt.Rows.Add(values);
                        //Log.DebugFormat("RenderDestination item added sentId:{0}", sentId);
                        sentId++;
                        index++;
                    }
                }

                InsertTargets(dt, IsAsync, 0);
            }

            return index;
        }
        public static void InsertTargets(DataTable dt, bool isAsync, int retry)
        {

            #region Max retry
            if (retry > MaxRetry)
            {
                //================= begin max retry  ===================

                bool alt_inserted = false;
                try
                {
                    using (DalUpload dal = new DalUpload())
                    {
                        if (isAsync)
                            dal.BulkInsertAsync(dt, "Trans_Items_Alt", 1000, null);
                        else
                            dal.BulkInsert(dt, "Trans_Items_Alt", 1000, null);
                    }
                    alt_inserted = true;
                }
                catch (Exception)
                {
                }

                if (alt_inserted)
                {
                    throw new MsgException(AckStatus.FatalException, string.Format("Destination insert error ===(items inserted to Trans_Items_Alt)===, After MaxRetry"));
                }
                else
                {
                    throw new MsgException(AckStatus.FatalException, string.Format("Destination insert error (items inserted to Trans_Items), After MaxRetry"));
                }

                //================= end max retry  ===================
            }
            #endregion

            if (dt == null || dt.Rows.Count == 0)
            {
                throw new NetException(AckStatus.FatalException, string.Format("InsertTargets error DataTable is null or empty"));
            }
            string table = "Trans_Items";
            bool hasError = false;
            try
            {
                Netlog.DebugFormat("DalBulkInsert Trans_Items destCount:{0}", dt.Rows.Count);

                using (DalUpload dal = new DalUpload())
                {

                    if (isAsync)
                        dal.BulkInsertAsync(dt, table, 1000, null);
                    else
                        dal.BulkInsert(dt, table, 1000, null);
                }

            }
            catch (Exception ex)
            {
                Netlog.ErrorFormat("DalBulkInsert error:{0}", ex.Message);

                MsgException.Trace(AckStatus.FatalException, string.Format("Destination insert error (items inserted to Trans_Items), Error:{0}", ex.Message));

                hasError = true;
            }

            if (hasError)
            {
                InsertTargets(dt, isAsync, retry + 1);
            }
        }


    }
}
