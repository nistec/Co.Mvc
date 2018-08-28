using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using Nistec.Data;
using Nistec.Data.Entities;
//using Nistec.Sys;
using Netcell.Data.Client;
using Nistec.Data.Factory;
using Nistec;
//using Nistec.Assist;


namespace Netcell.Data.DbServices.Entities
{

    public enum QueueState
    {
        Wait = 0,
        Exec = 1,
        Canceled = 2,
        Commited = 3,
        CommitedJurnal = 4,
        Expired = 5,
        Error=6
    }


    [Entity("Queue", "MQueue", "cnn_Services", EntityMode.Generic, "QueueId")]
    public class Queue_Context : EntityContext<Queue_Item>
    {
        #region ctor
        public Queue_Context(Guid QueueId)
            : base(QueueId)
        {

        }
        public Queue_Context(DataFilter filter)
            : base()
        {
            Init(filter);
        }
        public Queue_Context(Queue_Item item)
            : base(item)
        {

        }
        private Queue_Context()
            : base()
        {
        }
        #endregion

        #region binding

        protected override void EntityBind()
        {
            //base.EntityDb.EntityCulture = Netcell.Data.DB.Netcell_Services.GetCulture();
            //If EntityAttribute not define you can initilaize the entity here
            //base.InitEntity<AdventureWorks>("Contact", "Person.Contact", EntityKeys.Get("ContactID"));
        }

        #endregion

        #region static

        public static Guid NewId()
        {
            return UUID.NewUuid();
        }
             
        public static int Enqueue(Guid QueueId, string QueueName, int Priority, string Sender, int AccountId,string CorrelationId , string Body, int ItemsCount)
        {
            return Netcell_Services.Instance.DbCmd().ExecuteNonQuery("sp_Queue_Set", DataParameter.Get("QueueId", QueueId, "QueueName", QueueName, "Priority", Priority, "Sender", Sender, "AccountId", AccountId, "Body", Body, "ItemsCount", ItemsCount, "CorrelationId", CorrelationId, "Format", "json"), System.Data.CommandType.StoredProcedure);
        }
        public static int Enqueue(Guid QueueId, string QueueName, int Priority, string Sender, int AccountId, string CorrelationId, string Body, int ItemsCount, DateTime ExecTime, DateTime Expiration, string Format)
        {
            return Netcell_Services.Instance.DbCmd().ExecuteNonQuery("sp_Queue_Set", DataParameter.Get("QueueId", QueueId, "QueueName", QueueName, "Priority", Priority, "Sender", Sender, "AccountId", AccountId, "Body", Body, "ItemsCount", ItemsCount, "CorrelationId", CorrelationId, "Format", "json", "ExecTime", ExecTime, "Expiration", Expiration), System.Data.CommandType.StoredProcedure);
        }
        public static Queue_Item Dequeue(string QueueName, int server)
        {
            return Netcell_Services.Instance.DbCmd().ExecuteCommand<Queue_Item>("sp_Queue_Get", DataParameter.Get("QueueName", QueueName), System.Data.CommandType.StoredProcedure);
        }
        public static List<Queue_Item> DequeueBulk(string QueueName, int server)
        {
            return Netcell_Services.Instance.DbCmd().ExecuteCommand<Queue_Item, List<Queue_Item>>("sp_Queue_Get", DataParameter.Get("QueueName", QueueName), System.Data.CommandType.StoredProcedure);
        }
        public static int Commit(Guid QueueId,int State=4)
        {
            return Netcell_Services.Instance.DbCmd().ExecuteNonQuery("sp_Queue_Commit", DataParameter.Get("QueueId", QueueId, "State", State));
        }

        //public static int Message_Status_Log(Guid MessageId, string Source, int Status, string Reason, int AccountId, DateTime Creation,int BatchId, int ItemsCount,int AproxUnits)
        //{
        //    return Netcell_Services.Instance.DbCmd().ExecuteNonQuery("sp_Message_Status_Log", DataParameter.Get("MessageId", MessageId, "Source", Source, "Status", Status, "Reason", Reason, "AccountId", AccountId, "Creation", Creation, "BatchId",BatchId,"ItemsCount", ItemsCount, "AproxUnits", AproxUnits), System.Data.CommandType.StoredProcedure);
        //}
       

        /*
        public static List<QueueItem> DequeueBulk(int BulkMode, int server)
        {
            DataTable dt = null;
            using (IDbCmd cmd = Netcell_Services.Instance.DbCmd())
            {
                dt = cmd.ExecuteCommand<DataTable>("sp_Scheduler_DeQueue_Bulk", DataParameter.Get("BulkMode", BulkMode, "Server", server), CommandType.StoredProcedure);
            }
            if (dt == null)
            {
                return null;
            }

            return GetBulk(dt);
        }
        public static QueueItem Dequeue(int server)
        {
            //using (QueueItem_Context context = new QueueItem_Context())
            //{
            //    context.Init("sp_Scheduler_DeQueue", DataParameter.Get("Server", server), CommandType.StoredProcedure);
            //    return context.IsEmpty ? null : context.Entity;
            //}

            DataRow dr = null;
            using (IDbCmd cmd = Netcell_Services.Instance.DbCmd())
            {
                dr = cmd.ExecuteCommand<DataRow>("sp_Scheduler_DeQueue", DataParameter.Get("Server", server), CommandType.StoredProcedure);
            }
            if (dr == null)
            {
                return null;
            }

            return Get(dr);
        }

        public static int DeCompleted(Guid QueueId)
        {
            using (IDbCmd cmd = Netcell_Services.Instance.DbCmd())
            {
                return cmd.ExecuteNonQuery("sp_Scheduler_DeCompleted", DataParameter.Get("QueueId", QueueId), CommandType.StoredProcedure);
            }
        }
        public static int DeCompleted(Guid QueueId, int State)
        {
            using (IDbCmd cmd = Netcell_Services.Instance.DbCmd())
            {
                return cmd.ExecuteNonQuery("sp_Scheduler_DeCompleted", DataParameter.Get("QueueId", QueueId, "State", State), CommandType.StoredProcedure);
            }
        }

        public static List<QueueItem> GetBulk(DataTable dt)
        {
            List<QueueItem> list = new List<QueueItem>();
            foreach (DataRow dr in dt.Rows)
            {
                QueueItem item = Get(dr);
                list.Add(item);
            }
            return list;
        }

        public static List<QueueItem> GetBulk(DataTable dt, out string bulkKey)
        {
            List<QueueItem> list = new List<QueueItem>();
            GenericRecord record = null;
            foreach (DataRow dr in dt.Rows)
            {
                record = new GenericRecord(dr);

                QueueItem item = new QueueItem()
                {
                    AccountId = record.GetValue<int>("AccountId"),
                    ArgId = record.GetValue<int>("ArgId"),
                    Args = record.GetValue<string>("Args"),
                    Creation = record.GetValue<DateTime>("Creation"),
                    //CurrentTime = record.GetValue<DateTime>("CurrentTime"),
                    DataSource = record.GetValue<string>("DataSource"),
                    ExecTime = record.GetValue<DateTime>("ExecTime"),
                    Expiration = record.GetValue<DateTime>("Expiration"),
                    ItemId = record.GetValue<int>("ItemId"),
                    ItemIndex = record.GetValue<int>("ItemIndex"),
                    ItemPrice = record.GetValue<decimal>("ItemPrice"),
                    ItemRange = record.GetValue<int>("ItemRange"),
                    ItemsCount = record.GetValue<int>("ItemsCount"),
                    ItemType = record.GetValue<int>("ItemType"),
                    QueueId = record.GetValue<Guid>("QueueId"),
                    //SchedulerState = (SchedulerState)record.GetValue<int>("SchedulerState"),
                    Server = record.GetValue<int>("Server"),
                    State = record.GetValue<int>("State"),
                    UserId = record.GetValue<int>("UserId")

                };
                list.Add(item);
            }
            bulkKey = record.GetValue<string>("BulkKey");

            return list;
        }


        public static QueueItem Get(Guid queueId)
        {
            using (Queue_Context context = new Queue_Context(queueId))
            {
                return context.Entity;
            }
        }

        public static QueueItem Get(DataRow dr)
        {
            using (Queue_Context context = new Queue_Context())
            {
                context.EntityRecord = new GenericRecord(dr);
                return context.Entity;
            }
        }

        public static List<QueueItem> GetListItems(int itemId)
        {
            using (Queue_Context context = new Queue_Context())
            {
                return context.EntityList(DataFilter.Get("ItemId=@ItemId", itemId));
            }
        }

        public static DataTable GetList(int itemId)
        {
            using (IDbCmd cmd = Netcell_Services.Instance.DbCmd())
            {
                return cmd.ExecuteCommand<DataTable>("select * from QueueItem where ItemId=@ItemId", DataParameter.Get("ItemId", itemId));
            }
        }

        public static QueueItem Deserialize(string base64)
        {
            return MControl.Runtime.Serialization.DeserializeFromBase64<QueueItem>(base64);
        }

        //ActionType=1=change date,2=remove,3=removeWithBilling
        public static int RemoveItem(Guid queueId, bool removePending)
        {
            int actionType = removePending ? 3 : 2;
            using (IDbCmd cmd = Netcell_Services.Instance.DbCmd())
            {
                return cmd.ExecuteNonQuery("sp_Scheduler_Item_Update_b", DataParameter.Get("QueueId", queueId, "ActionType", actionType), CommandType.StoredProcedure);
            }
        }

        public static int ChangeTime(Guid queueId, DateTime time)
        {
            int actionType = 1;
            using (IDbCmd cmd = Netcell_Services.Instance.DbCmd())
            {
                return cmd.ExecuteNonQuery("sp_Scheduler_Item_Update_b", DataParameter.Get("QueueId", queueId, "ActionType", actionType, "BatchTime", time), CommandType.StoredProcedure);
            }
        }
        */
        #endregion

        #region Enqueue

        public static int Insert(Queue_Item item)
        {
            using (Queue_Context context = new Queue_Context(item))
            {
                return context.SaveChanges(UpdateCommandType.Insert);
            }
        }

       

        #endregion
    }

    [Serializable]
    public class Queue_Item : IEntityItem
    {
       
        #region Ex Properties


        [EntityProperty(EntityPropertyType.NA)]
        public DateTime CurrentTime
        {
            get { return DateTime.Now; }
        }

        [EntityProperty(EntityPropertyType.NA)]
        public bool IsExpired
        {
            get { return Expiration < CurrentTime; }
        }
        
        //public static DateTime GetDefaultExpiration(DateTime execTime)
        //{
        //    return execTime.AddHours(6);
        //}

        #endregion

        #region Properties

       [EntityProperty(EntityPropertyType.Key, Caption = "רשומה")]
        public Guid QueueId
        {
            get;
            set;
        }

         [EntityProperty(EntityPropertyType.Default, Caption = "מס-נמענים")]
        public int ItemsCount
        {
            get;
            set;
        }
        
        [EntityProperty(EntityPropertyType.Default, Caption = "סטטוס")]
        public int State
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "נוצר ב")]
        public DateTime Creation
        {
            get;
            set;
        }
       [EntityProperty(EntityPropertyType.Default, Caption = "תפוגה")]
        public DateTime Expiration
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מועד לביצוע")]
        public DateTime ExecTime
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "חשבון")]
        public int AccountId
        {
            get;
            set;
        }
   
        [EntityProperty(EntityPropertyType.Default, Caption = "שם תור")]
        public string QueueName
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "תיעדוף")]
        public int Priority
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מאת")]
        public string Sender
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "סטאטוס אחרון")]
        public DateTime LastState
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מס-נסיונות")]
        public int Retry
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "גוף המסר")]
        public string Body
        {
            get;
            set;
        }

        //json,xml,base64
        [EntityProperty(EntityPropertyType.Default, Caption = "פורמט")]
        public string Format
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מתאם")]
        public string CorrelationId
        {
            get;
            set;
        }

       

        //public Guid QueueId { get; set; }
        //public string QueueName { get; set; }
        //public int Priority { get; set; }
        //public string Sender { get; set; }
        //public int AccountId { get; set; }
        //public int ItemId { get; set; }
        //public DateTime ExecTime { get; set; }
        //public DateTime Creation { get; set; }
        //public DateTime Expiration { get; set; }
        //public DateTime LastState { get; set; }
        //public int State { get; set; }
        //public int Retry { get; set; }
        //public string Args { get; set; }
        //public string Body { get; set; }

        public bool IsEmpty
        {
            get { return string.IsNullOrEmpty(Body); }
        }

        public string Print()
        {
            QueueState qstate = (QueueState)State;
            return string.Format("Queue Item Id:{0}, State:{1}, QueueName:{2}, AccountId:{3},Sender:{4}", QueueId, qstate.ToString(), QueueName, AccountId, Sender);
        }
        #endregion

        #region methods
        
        public string Serialize()
        {
            return MControl.Runtime.Serialization.SerializeToBase64(this);
        }
        
        #endregion
    }
 
}
