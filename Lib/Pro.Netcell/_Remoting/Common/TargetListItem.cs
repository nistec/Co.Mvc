using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Xml;
using System.Linq;
using Netcell.Data.Client;
using Netcell.Data;
using Netcell;
using Nistec.Data;


namespace Netcell.Remoting
{
    public class TargetUtil
    {
        public const int MaxTableItems = 50000;
        public const int MaxRetry = 3;
        public const bool IsAsync=true;

        public static int RenderDestination(int campaignId, List<IDestinationItem> destList, List<BatchListItem> batchList)//, bool isAsync)
        {
            if (destList == null || destList.Count <= 0)
            {
                throw new Exception("Invalid Destination List");
            }
            if (batchList == null || batchList.Count <= 0)
            {
                throw new Exception("Invalid BatchList");
            }

            //DataTable dt = TargetListItem.Target_Items_Schema();

            

            object[] values = null;
            int destCount = destList.Count;
            int index = 0;
            //int tablesCount =(int) Math.Ceiling(Convert.ToDecimal(destCount / 50000)) + 1;
            //List<DataTable> tables = new List<DataTable>();

            Log.DebugFormat("Counters.MessageId is CampaignId:{0},destCount:{1},batchList.Count:{2}", campaignId, destCount, batchList.Count);

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
                DataTable dt = TargetListItem.Target_Items_Schema();

                Log.DebugFormat("RenderDestination batch is batchItems:{0},batchId:{1},batchIndex:{2},minId:{3}, maxId:{4}", batchItems, batchId, batchIndex, minId, maxId);


                for (int j = 0; j < batchItems && index < destCount && sentId <= maxId; j++)
                {
                    IDestinationItem item = destList[index];
                    //Log.DebugFormat("RenderDestination item is item.Target:{0},item.Personal:{1}", item.Target, item.Personal);

                    values = TargetListItem.ItemArray(item, sentId, campaignId, batchId, timeSend, index);
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

            //InsertTargets(dt);//, isAsync);

            return index;
        }

        public static int RenderDestination(int campaignId, List<IDestinationItem> destList, int batchId, DateTime timeSend)//, bool isAsync)
        {
            if (destList == null || destList.Count <= 0)
            {
                throw new Exception("Invalid Destination List");
            }

            DataTable dt = TargetListItem.Target_Items_Schema();

            object[] values = null;
            int destCount = destList.Count;
            Log.DebugFormat("Counters.MessageId is CampaignId:{0},destCount:{1}", campaignId, destCount);

            int minId = 0;
            int maxId = 0;
            Counters.MessageId(destCount, ref minId, ref maxId);
            int sentId = minId;

            for (int i = 0; i < destCount && sentId <= maxId; i++)
            {
                IDestinationItem item = destList[i];
                values = TargetListItem.ItemArray(item, sentId, campaignId, batchId, timeSend, i);
                if (values != null)
                {
                    dt.Rows.Add(values);
                    sentId++;
                }
            }

            InsertTargets(dt, IsAsync, 0);//, isAsync);

            return destCount;
        }
        
        public static int RenderDestination(int campaignId, List<TargetListItem> targetList, int batchId, DateTime timeSend, PlatformType platform)
        {
            if (targetList == null || targetList.Count <= 0)
            {
                throw new Exception("Invalid Destination List");
            }

            DataTable dt = TargetListItem.Target_Items_Schema();

            object[] values = null;
            int destCount = targetList.Count;
            Log.DebugFormat("Counters.MessageId is CampaignId:{0},destCount:{1}", campaignId, destCount);
            int minId = 0;
            int maxId = 0;
            Counters.MessageId(destCount, ref minId, ref maxId);
            int sentId = minId;

            for (int i = 0; i < destCount && sentId<=maxId; i++)
            {
                TargetListItem item = targetList[i];
                item.SentId = sentId;
                values = item.ItemArray(sentId,campaignId, batchId, timeSend, null, i, (int)platform,item.ContactId);
                if (values != null)
                {
                    dt.Rows.Add(values);
                    sentId++;
                }
            }

            InsertTargets(dt, IsAsync, 0);//, isAsync);

            return destCount;
        }
               
        public static int RenderDestination(int campaignId, IDestinationItem item, int batchId, DateTime timeSend, string sender, PlatformType platform)
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
  
        public static int RenderDestination(int campaignId, TargetListItem item, int batchId, DateTime timeSend, string sender, PlatformType platform)
        {
            int minId = 0;
            int maxId = 0;
            Counters.MessageId(1, ref minId, ref maxId);

            int messageId = maxId;
            item.SentId = messageId;
             using (DalController dal = new DalController())
            {
                dal.Trans_Items_Insert(messageId, campaignId, batchId, timeSend, item.To.Trim(), 0, 0, null, item.Personal, sender, 0, (int)platform, item.ContactId);
            }
            return messageId;
        }

        public static DataTable DestinationToDataTable(int campaignId, ICollection items)
        {

            if (items == null || items.Count <= 0)
            {
                throw new Exception("Invalid Destination List");
            }

            List<IDestinationItem> destList = items.Cast<IDestinationItem>().ToList();


            DataTable dt = TargetListItem.Target_Items_Schema();

            object[] values = null;
            int destCount = destList.Count;
            int index = 0;
            DateTime timeSend = DateTime.Now;

            foreach (IDestinationItem item in destList)
            {
                values = TargetListItem.ItemArray(item, index, campaignId, 0, timeSend, index);
                if (values != null)
                {
                    dt.Rows.Add(values);
                    index++;
                }
            }


            return dt;
        }

        public static void InsertTargets(DataTable dt, bool isAsync, int retry)//, bool isAsync)
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
                        //dal.InsertTable(dt, table);

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

            if (dt==null || dt.Rows.Count==0)
            {
                throw new MsgException(AckStatus.FatalException, string.Format("InsertTargets error DataTable is null or empty"));
            }
            string table = "Trans_Items";
            bool hasError = false;
            try
            {
                Log.DebugFormat("DalBulkInsert Trans_Items destCount:{0}", dt.Rows.Count);

                //using (DalBulkInsert dal = new DalBulkInsert())
                //{
                //    //dal.InsertTable(dt, table);
                    
                //    if (isAsync)
                //        dal.BulkInsertAsync(dt, table);
                //    else
                //        dal.BulkInsert(dt, table);
                //}

                using (DalUpload dal = new DalUpload())
                {
                    //dal.InsertTable(dt, table);

                    if (isAsync)
                        dal.BulkInsertAsync(dt, table,1000,null);
                    else
                        dal.BulkInsert(dt, table, 1000, null);
                }
 
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("DalBulkInsert error:{0}", ex.Message);

                 MsgException.Trace(AckStatus.FatalException, string.Format("Destination insert error (items inserted to Trans_Items), Error:{0}", ex.Message));

                hasError = true;
            }

            if (hasError)
            {
                InsertTargets(dt,isAsync, retry + 1);
            }
        }

    }

    public class TargetsHandler
    {
        public static TargetsHandler Instance
        {
            get { return new TargetsHandler(); }
        }
        public static int RenderDestination(string publishKey, PlatformType platform, string sender, TargetsList Targets, int batchId, DateTime timeSend)
        {
            if (Targets == null || Targets.Count <= 0)
            {
                throw new Exception("Invalid Destination List");
            }
            if (batchId <= 0)
            {
                throw new Exception("Invalid BatchId");
            }
            int sentItems = 0;
            object[] values = null;
            int destCount = Targets.Count;
            int index = 0;
            //int tablesCount =(int) Math.Ceiling(Convert.ToDecimal(destCount / 50000)) + 1;
            //List<DataTable> tables = new List<DataTable>();

            //int firstBatchId = batchList[0].BatchId;
            Log.DebugFormat("Counters.MessageId is publishKey:{0},destCount:{1},batchList.Count:{2}", publishKey, destCount, 1);

            int minId = 0;
            int maxId = 0;
            Counters.MessageId(destCount, ref minId, ref maxId);
            int sentId = minId;

            DataTable dtRange = null;

            if (batchId > 1)
            {
                dtRange = TargetListItem.Batch_Range_Schema();
            }
            DataTable dt = TargetListItem.Target_Items_Schema();

            Log.DebugFormat("RenderDestination batch is batchItems:{0},batchId:{1},batchIndex:{2},minId:{3}, maxId:{4}", destCount, batchId, index, minId, maxId);

            for (int j = 0; j < destCount && sentId <= maxId; j++)
            {
                TargetListItem item = Targets[index];
                item.SentId = sentId;
                values = TargetListItem.ItemArray(sentId, 0, batchId, timeSend, item.To, 0, item.GroupId, null, item.Personal, sender, batchId, (int)platform, item.ContactId);
                if (values != null)
                {
                    dt.Rows.Add(values);
                    sentId++;
                    sentItems++;
                }
                index++;
            }
            TargetUtil.InsertTargets(dt, TargetUtil.IsAsync, TargetUtil.MaxRetry);

            return sentItems;
        }

        public static int RenderDestination(string publishKey,PlatformType platform, string sender, TargetsList Targets, List<BatchListItem> batchList)
        {
            if (Targets == null || Targets.Count <= 0)
            {
                throw new Exception("Invalid Destination List");
            }
            if (batchList == null || batchList.Count <= 0)
            {
                throw new Exception("Invalid BatchList");
            }
            int sentItems = 0;
            object[] values = null;
            int destCount = Targets.Count;
            int index = 0;
            //int tablesCount =(int) Math.Ceiling(Convert.ToDecimal(destCount / 50000)) + 1;
            //List<DataTable> tables = new List<DataTable>();
            
            //int firstBatchId = batchList[0].BatchId;
            Log.DebugFormat("Counters.MessageId is publishKey:{0},destCount:{1},batchList.Count:{2}", publishKey, destCount, batchList.Count);

            int minId = 0;
            int maxId = 0;
            Counters.MessageId(destCount, ref minId, ref maxId);
            int sentId = minId;

            DataTable dtRange=null; 

            if(batchList.Count>1)
            {
                dtRange = TargetListItem.Batch_Range_Schema();
            }

            for (int i = 0; i < batchList.Count; i++)
            {
                int batchItems = batchList[i].BatchValue;
                int batchId = batchList[i].BatchId;
                int batchIndex = batchList[i].BatchIndex;
                int batchRange = batchList[i].BatchRange;
                DateTime timeSend = batchList[i].SendTime;
                DataTable dt = TargetListItem.Target_Items_Schema();

                Log.DebugFormat("RenderDestination batch is batchItems:{0},batchId:{1},batchIndex:{2},minId:{3}, maxId:{4}", batchItems, batchId, batchIndex, minId, maxId);
                if(batchList.Count>1)
                {
                    dtRange.Rows.Add(publishKey, batchId, timeSend, batchIndex, batchRange, batchItems,0);
                }

                for (int j = 0; j < batchItems && index < destCount && sentId <= maxId; j++)
                {
                    TargetListItem item = Targets[index];
                    item.SentId = sentId;
                    values = TargetListItem.ItemArray(sentId, 0, batchId, timeSend, item.To, 0, item.GroupId, null, item.Personal, sender, i, (int)platform, item.ContactId);
                    if (values != null)
                    {
                        dt.Rows.Add(values);
                        sentId++;
                        sentItems++;
                    }
                    index++;
                }
                TargetUtil.InsertTargets(dt, TargetUtil.IsAsync, TargetUtil.MaxRetry);
            }

            if(batchList.Count>1)
            {
                TargetUtil.InsertTargets(dtRange, TargetUtil.IsAsync, TargetUtil.MaxRetry);
            }

            return sentItems;
        }
        public int RenderDestination(TargetsList Targets,int batchId, DateTime timeSend, string sender, PlatformType platform)//, bool isAsync)
        {
            DataTable dt = TargetListItem.Target_Items_Schema();

            object[] values = null;
            int destCount = Targets.Count;
            int minId = 0;
            int maxId = 0;
            Counters.MessageId(destCount, ref minId, ref maxId);
            int sentId = minId;
            for (int i = 0; i < destCount && sentId <= maxId; i++)
            {
                TargetListItem item = Targets[i];
                values = TargetListItem.ItemArray(sentId, 0, batchId, timeSend, item.To, 0, 0, null, item.Personal, sender, i, (int)platform, item.ContactId);
                if (values != null)
                {
                    dt.Rows.Add(values);
                    sentId++;
                }
            }

            TargetUtil.InsertTargets(dt, TargetUtil.IsAsync, TargetUtil.MaxRetry);//,isAsync);

            return destCount;
        }

        public DataTable RenderDestinationToSend(TargetsList Targets, int batchId, int accountId, DateTime timeSend, string sender, PlatformType platform)
        {
            DataTable dt = TargetListItem.Target_Items_Schema();
            object[] values = null;
            int destCount = Targets.Count;
            int i = 0;
            int minId = 0;
            int maxId = 0;
            Counters.MessageId(destCount, ref minId, ref maxId);
            int sentId = minId;
            foreach (TargetListItem item in Targets)
            {
                
                values = TargetListItem.ItemArray(sentId, 0, batchId, timeSend, item.To, 0, 0, null, item.Personal, sender, i, (int)platform, item.ContactId);
                if (values != null)
                {
                    dt.Rows.Add(values);
                    sentId++;
                    i++;
                }

                if (sentId > maxId)
                {
                    break;
                }
            }

            using (DalController dal = new DalController())
            {
                return dal.Batch_Items_ToSend(dt, batchId, accountId, (int)platform);
            }
        }

        public DataTable RenderDestinationToSend(TargetsList Targets, DalController dal, int batchId, int accountId, DateTime timeSend, string sender, PlatformType platform)
        {
            DataTable dt = TargetListItem.Target_Items_Schema();
            object[] values = null;
            int destCount = Targets.Count;
            int i = 0;
            int minId = 0;
            int maxId = 0;
            Counters.MessageId(destCount, ref minId, ref maxId);
            int sentId = minId;

            foreach (TargetListItem item in Targets)
            {
                
                values = TargetListItem.ItemArray(sentId, 0, batchId, timeSend, item.To, 0, 0, null, item.Personal, sender, i, (int)platform, item.ContactId);
                if (values != null)
                {
                    dt.Rows.Add(values);
                    sentId++;
                    i++;
                }
                if (sentId > maxId)
                {
                    break;
                }
            }

            return dal.Batch_Items_ToSend(dt, batchId, accountId, (int)platform);
        }

        public int RenderSingleDestination(TargetsList Targets, int batchId, DateTime timeSend, string sender, PlatformType platform)
        {
            int minId = 0;
            int maxId = 0;
            Counters.MessageId(1, ref minId, ref maxId);

            int messageId = maxId;

            TargetListItem item = Targets[0];
            using (DalController dal = new DalController())
            {
                dal.Trans_Items_Insert(messageId, 0, batchId, timeSend, item.To.Trim(), 0, 0, null, item.Personal, sender, 0, (int)platform, item.ContactId);
            }
            return messageId;
        }

        public int RenderSingleDestination(TargetsList Targets, DalController dal, int batchId, DateTime timeSend, string sender, PlatformType platform)
        {
            int minId = 0;
            int maxId = 0;
            Counters.MessageId(1, ref minId, ref maxId);

            int messageId = maxId;

            TargetListItem item = Targets[0];
            dal.Trans_Items_Insert(messageId, 0, batchId, timeSend, item.To.Trim(), 0, 0, null, item.Personal, sender, 0, (int)platform,item.ContactId);
            return messageId;
        }

    }

    [Serializable]
    public class TargetsList : List<TargetListItem>
    {
        public TargetsList() { }
        public TargetsList(IEnumerable<TargetListItem> items)
            : base(items)
        {
        }


        public List<TargetListItem> List
        {
            get { return this; }
        }

        public void Add(DataRow dr)
        {
            Add(dr.Get<string>("Target"), dr.Get<string>("Personal"));
        }

        public static TargetsList Parse(string targets)
        {
            if (string.IsNullOrEmpty(targets))
            {
                throw new ArgumentNullException(targets);
            }
            targets = targets.TrimStart('|').TrimEnd('|');

            string[] list = targets.Split(new char[] { '|' });

            TargetsList items = new TargetsList();
            int i = 0;
            foreach (string s in list)
            {
                string[] args = s.Split('#');
                if (args.Length > 1)
                    items.Add(new TargetListItem(args[0], args[1]));
                else if (args.Length > 0)
                    items.Add(new TargetListItem(args[0]));
                i++;
            }
            return items;
        }
        
        public TargetListItem Find(string to)
        {
            return base.Find(delegate(TargetListItem t) { return t.To == to; });
        }

        public void Add(string to, string personal)//, int sentId)
        {
            if (to != null)
            {
                base.Add(new TargetListItem(to, personal));//, sentId));
            }
        }

    }

    [Serializable]
    public class TargetListItem
    {
        public string To { get; private set; }
        public string Personal { get; private set; }
        public bool IsValid { get; private set; }

        public string Sender { get; set; }
        public int GroupId { get; set; }
        public int ContactId { get; set; }
        public int SentId { get; set; }
        public static TargetListItem Create(string to, string personal)
        {
            string target = null;
            if (string.IsNullOrEmpty(to))
            {
                return null;
            }
            if (to.Contains('@'))
            {
                if (!Nistec.Regx.IsEmail(to))
                {
                    return null;
                }
                target = to;
            }
            else
            {
                CLI cli = new CLI(to);
                if (!cli.IsValid)
                {
                    return null;
                }
                target = cli.CellNumber;
            }

            return new TargetListItem(target, personal);
        }

        private TargetListItem(string target, string personal)
        {
            this.To = target;
            this.Personal = personal;
            this.IsValid = true;
        }

        public TargetListItem(string to, string personal,bool isMail=false)//, int sentId)
        {
            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentNullException("TargetListItem:" + to);
            }
            if (isMail)
            {
                if (!Nistec.Regx.IsEmail(to))
                {
                    throw new ArgumentException("TargetListItem mail is incorrect :" + to);
                    //return;
                }
                this.To = to;
            }
            else
            {
                CLI cli = new CLI(to);
                this.IsValid = cli.IsValid;
                if (!IsValid)
                {
                    throw new ArgumentException("TargetListItem cell is incorrect :" + to);
                    //return;
                }
                //this.CarrierId = 0;
                this.To = cli.CellNumber;
            }
            this.Personal = personal;
            //this.SentId = sentId;
         }

        public TargetListItem(string to, bool isMail = false)
            : this(to, null, isMail)
        {

        }
      
        public override string ToString()
        {
            return To;
        }



        /// <summary>
        /// Split string parameter by ';' or ',' and create list of Targets
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        public static TargetListItem[] SplitTarget(string targets)
        {
            string[] list = Split(targets);
            TargetListItem[] items = new TargetListItem[list.Length];
            int i = 0;
            foreach (string s in list)
            {
                items[i] = new TargetListItem(s);
                i++;
            }
            return items;
        }
        /// <summary>
        /// Split string parameter by ';' or ',' and create string array of targets
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string[] Split(string targets)
        {
            if (string.IsNullOrEmpty(targets))
            {
                throw new ArgumentNullException(targets);
            }
            targets = targets.TrimStart(';').TrimEnd(';');

            string[] list = targets.Split(new char[] { ';', ',', ' ' });
            return list;
        }
        
        public object[] ItemArray(int MessageId, int CampaignId, int BatchId, DateTime SendTime, string Coupon, int ItemIndex, int Platform, int ContactId)
        {
            if (To == null)
                return null;
            string target = To;
            if (target.Length > 50)
                return null;
            return new object[] { MessageId, CampaignId, BatchId, SendTime, target.Trim(), 0, GroupId, Coupon, Personal, Sender, ItemIndex, Platform, ContactId };
        }

        public static object[] ItemArray(int MessageId,int CampaignId,int BatchId,DateTime SendTime,string Target,int State,int GroupId,string Coupon,string Personal,string Sender,int ItemIndex,int Platform, int ContactId)
        {
            if (Target == null)
                return null;
            string target = Target;
            if (target.Length > 50)
                return null;
            return new object[] { MessageId, CampaignId, BatchId, SendTime, target.Trim(), State, GroupId, Coupon, Personal, Sender, ItemIndex, Platform ,ContactId};
        }

        public static object[] ItemArray(IDestinationItem item, int MessageId, int campaignId, int batchId, DateTime timeSend, int index)
        {
            if (item == null || item.Target ==null)
                return null;
            string target = item.Target;
            if (target.Length > 50)
                return null;
            return new object[] { MessageId, campaignId, batchId, timeSend, target.Trim(), 0, item.GroupId, item.Coupon, item.Personal, item.Sender, index, item.Platform, item.ContactId };
        }

        public static int GetTargetsCount(string targets)
        {
            string[] tr = TargetListItem.Split(targets);
            if (tr == null)
                return 0;
            return tr.Length;
        }

        public static DataTable Target_Items_Schema()
        {
            DataTable dt = new DataTable("Trans_Items");
            dt.Columns.Add(new DataColumn("MessageId", typeof(int)));
            dt.Columns.Add(new DataColumn("CampaignId", typeof(int)));
            dt.Columns.Add(new DataColumn("BatchId", typeof(int)));
            dt.Columns.Add(new DataColumn("SendTime", typeof(DateTime)));
            dt.Columns.Add("Target");
            dt.Columns.Add(new DataColumn("State", typeof(int)));
            
            dt.Columns.Add(new DataColumn("GroupId", typeof(int)));
            dt.Columns.Add("Coupon");
            dt.Columns.Add("Personal");
            dt.Columns.Add("Sender");
            
            dt.Columns.Add(new DataColumn("ItemIndex", typeof(int)));
            
            dt.Columns.Add(new DataColumn("Platform", typeof(int)));
            dt.Columns.Add(new DataColumn("ContactId", typeof(int)));
            return dt.Clone();
        }

        //publishKey, batchId, timeSend, batchIndex, batchRange, batchItems,0
        public static DataTable Batch_Range_Schema()
        {
            DataTable dt = new DataTable("Trans_Batch_Range");
            dt.Columns.Add(new DataColumn("PublishKey", typeof(string)));
            dt.Columns.Add(new DataColumn("BatchId", typeof(int)));
            dt.Columns.Add(new DataColumn("BatchTime", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("BatchIndex", typeof(int)));
            dt.Columns.Add(new DataColumn("BatchRange", typeof(int)));
            dt.Columns.Add(new DataColumn("BatchCount", typeof(int)));
            dt.Columns.Add(new DataColumn("BatchState", typeof(int)));
            return dt.Clone();
        }
    }
  
}
