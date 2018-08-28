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
//using Netcell.Data.DbServices.Entities;
using Nistec.Generic;
using Netcell.Data.DbServices.Entities;

namespace Netcell.Data.Db.Entities
{
    ////[DBCommand(DBCommandType.StoredProcedure, "sp_Scheduler_DeQueue")]
    //[Entity("Scheduler_Queue", "Scheduler_Queue", "cnn_Netcell", EntityMode.Generic, "QueueId")]
    //public class Scheduler_Queue_Context : EntityContext<Scheduler_Queue>
    //{

    //}

 

    [Entity("Scheduler_Queue", "Scheduler_Queue", "cnn_Netcell", EntityMode.Generic, "QueueId")]
    public class Scheduler_Queue_Context : EntityContext<Scheduler_Queue>
    {
        #region ctor
        public Scheduler_Queue_Context(Guid QueueId)
            : base(QueueId)
        {

        }
        public Scheduler_Queue_Context(DataFilter filter)
            : base()
        {
            Init(filter);
        }
        public Scheduler_Queue_Context(Scheduler_Queue item)
            : base(item)
        {

        }
        private Scheduler_Queue_Context()
            : base()
        {
        }
        #endregion

        #region binding

        protected override void EntityBind()
        {
            //base.EntityDb.EntityCulture = Netcell.Data.DB.NetcellDB.GetCulture();
            //If EntityAttribute not define you can initilaize the entity here
            //base.InitEntity<AdventureWorks>("Contact", "Person.Contact", EntityKeys.Get("ContactID"));
        }

        #endregion

        #region methods
        public static List<Scheduler_Queue> DequeueBulk(int BulkMode, int server)
        {
            DataTable dt = null;
            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            {
                dt = cmd.ExecuteCommand<DataTable>("sp_Scheduler_DeQueue_Bulk", DataParameter.GetSql("BulkMode", BulkMode, "Server", server), CommandType.StoredProcedure);
            }
            if (dt == null)
            {
                return null;
            }

            return GetBulk(dt);
        }
        public static Scheduler_Queue Dequeue(int server)
        {
            //using (Scheduler_Queue_Context context = new Scheduler_Queue_Context())
            //{
            //    context.Init("sp_Scheduler_DeQueue", DataParameter.Get("Server", server), CommandType.StoredProcedure);
            //    return context.IsEmpty ? null : context.Entity;
            //}

            DataRow dr = null;
            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            {
                dr = cmd.ExecuteCommand<DataRow>("sp_Scheduler_DeQueue", DataParameter.GetSql("Server", server), CommandType.StoredProcedure);
            }
            if (dr == null)
            {
                return null;
            }

            return Get(dr);
        }

        public static int DeCompleted(Guid QueueId)
        {
            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.ExecuteNonQuery("sp_Scheduler_DeCompleted", DataParameter.GetSql("QueueId", QueueId), CommandType.StoredProcedure);
            }
        }
        public static int DeCompleted(Guid QueueId, int State)
        {
            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.ExecuteNonQuery("sp_Scheduler_DeCompleted", DataParameter.GetSql("QueueId", QueueId, "State", State), CommandType.StoredProcedure);
            }
        }

        public static List<Scheduler_Queue> GetBulk(DataTable dt)
        {
            List<Scheduler_Queue> list = new List<Scheduler_Queue>();
            foreach (DataRow dr in dt.Rows)
            {
                Scheduler_Queue item = Get(dr);
                list.Add(item);
            }
            return list;
        }

        public static List<Scheduler_Queue> GetBulk(DataTable dt, out string bulkKey)
        {
            List<Scheduler_Queue> list = new List<Scheduler_Queue>();
            GenericRecord record = null;
            foreach (DataRow dr in dt.Rows)
            {
                record = new GenericRecord(dr);

                Scheduler_Queue item = new Scheduler_Queue()
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


        public static Scheduler_Queue Get(Guid queueId)
        {
            using (Scheduler_Queue_Context context = new Scheduler_Queue_Context(queueId))
            {
                return context.Entity;
            }
        }

        public static Scheduler_Queue Get(DataRow dr)
        {
            using (Scheduler_Queue_Context context = new Scheduler_Queue_Context())
            {
                context.EntityRecord = new GenericRecord(dr);
                return context.Entity;
            }
        }

        public static List<Scheduler_Queue> GetListItems(int itemId)
        {
            using (Scheduler_Queue_Context context = new Scheduler_Queue_Context())
            {
                return context.EntityList(DataFilter.Get("ItemId=@ItemId", itemId));
            }
        }

        public static DataTable GetList(int itemId)
        {
            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.ExecuteCommand<DataTable>("select * from Scheduler_Queue where ItemId=@ItemId", DataParameter.GetSql("ItemId", itemId));
            }
        }

        public static Scheduler_Queue Deserialize(string base64)
        {
            return Nistec.Serialization.BinarySerializer.DeserializeFromBase64<Scheduler_Queue>(base64);
        }

        //ActionType=1=change date,2=remove,3=removeWithBilling
        public static int RemoveItem(Guid queueId, bool removePending)
        {
            int actionType = removePending ? 3 : 2;
            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.ExecuteNonQuery("sp_Scheduler_Item_Update_b", DataParameter.GetSql("QueueId", queueId, "ActionType", actionType), CommandType.StoredProcedure);
            }
        }

        public static int ChangeTime(Guid queueId, DateTime time)
        {
            int actionType = 1;
            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.ExecuteNonQuery("sp_Scheduler_Item_Update_b", DataParameter.GetSql("QueueId", queueId, "ActionType", actionType, "BatchTime", time), CommandType.StoredProcedure);
            }
        }

          #endregion

        #region Enqueue

        public static int Insert(Scheduler_Queue item)
        {
            using (Scheduler_Queue_Context context = new Scheduler_Queue_Context(item))
            {
                return context.SaveChanges(UpdateCommandType.Insert);
            }
        }

        public static int Insert(Trans_Batch item)
        {
            Scheduler_Queue q = new Scheduler_Queue()
            {
                AccountId = item.AccountId,
                ArgId = item.BatchId,
                Creation = item.Creation,
                DataSource = SchedulerDataSource.Batch.ToString(),
                ExecTime = item.BatchTime,
                Expiration = Scheduler_Queue.GetDefaultExpiration(item.BatchTime),
                ItemPrice = item.DefaultPrice,
                ItemsCount = item.BatchCount,
                ItemType = (int)SchedulerItemType.Scheduled,
                ItemId = item.CampaignId,
                ItemIndex = item.BatchIndex,
                ItemRange = item.BatchRange,
                UserId = item.UserId,
                Server = item.Server,
                QueueId = Guid.NewGuid()
            };
            return Scheduler_Queue_Context.Insert(q);
        }
        /*
         * TODO:fix this
        public static int Insert(ReminderEntity item)
        {
            Scheduler_Queue q = new Scheduler_Queue()
            {
                AccountId = item.AccountId,
                ArgId = item.ReminderId,
                Creation = DateTime.Now,
                DataSource = SchedulerDataSource.Reminder.ToString(),
                ExecTime = item.DateToSend,
                Expiration = Scheduler_Queue.GetDefaultExpiration(item.DateToSend),
                ItemPrice = item.ItemPrice,
                ItemsCount = item.ItemsCount,
                ItemType = (int)SchedulerItemType.Scheduled,
                ItemId = item.ReminderId,
                ItemIndex = 0,
                ItemRange = 0,
                UserId = 0,
                Server = 0,
                QueueId = Guid.NewGuid()
            };
            return Scheduler_Queue_Context.Insert(q);
        }
        */
        public static int Insert(Services_Alerts item)
        {
            Scheduler_Queue q = new Scheduler_Queue()
            {
                AccountId = item.AccountId,
                ArgId = item.AlertId,
                Creation = DateTime.Now,
                DataSource = SchedulerDataSource.Alert.ToString(),
                ExecTime = item.ExecTime,
                Expiration = Scheduler_Queue.GetDefaultExpiration(item.ExecTime),
                ItemPrice = 0,
                ItemsCount = 1,
                ItemType = (int)SchedulerItemType.Scheduled,
                ItemId = item.AlertId,
                ItemIndex = 0,
                ItemRange = 0,
                UserId = 0,
                Server = 0,
                QueueId = Guid.NewGuid()
            };
            return Scheduler_Queue_Context.Insert(q);
        }
        
        #endregion
    }

    [Serializable]
    public class Scheduler_Queue : IEntityItem
    {
        public Scheduler_Queue() { }

        #region static

        public static Scheduler_Queue CreateScheduledBatch(int accountId, int batchId, int count, decimal price, int userId, int server, DateTime timeToSend)
        {
            Scheduler_Queue q = new Scheduler_Queue()
            {
                AccountId = accountId,
                ArgId = batchId,
                Creation = DateTime.Now,
                DataSource = SchedulerDataSource.Batch.ToString(),
                ExecTime = timeToSend,
                Expiration = Scheduler_Queue.GetDefaultExpiration(timeToSend),
                ItemPrice = price,
                ItemsCount = count,
                ItemType = (int)SchedulerItemType.Scheduled,
                ItemId=0,
                ItemIndex=0,
                ItemRange=0,
                UserId = userId,
                Server = server,
                QueueId=Guid.NewGuid()
            };
            return q;
        }

        public static Scheduler_Queue CreateExecutedBatch(int accountId, int batchId, int count, decimal price, int userId, int server=0)
        {
            Scheduler_Queue q = new Scheduler_Queue()
            {
                AccountId = accountId,
                ArgId = batchId,
                Creation = DateTime.Now,
                DataSource = SchedulerDataSource.Batch.ToString(),
                ExecTime = DateTime.Now,
                Expiration = Scheduler_Queue.GetDefaultExpiration(DateTime.Now),
                ItemPrice = price,
                ItemsCount = count,
                ItemType = (int)SchedulerItemType.Executed,
                ItemId = 0,
                ItemIndex = 0,
                ItemRange = 0,
                UserId = userId,
                Server = server,
                QueueId=Guid.NewGuid()
            };
            return q;
        }

         #endregion

        #region Ex Properties

        public bool IsEmpty
        {
            get { return Types.IsEmpty(DataSource); }
        }

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

        public SchedulerState SchedulerState
        {
            get
            {
                if (Types.IsEmpty(DataSource))
                    return SchedulerState.Empty;
                return (SchedulerState)State;
            }
        }
        
        public static DateTime GetDefaultExpiration(DateTime execTime)
        {
            return execTime.AddHours(6);
        }

        #endregion

        #region Properties

       [EntityProperty(EntityPropertyType.Key, Caption = "רשומה")]
        public Guid QueueId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מס-פריט")]
        public int ItemId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "סוג")]
        public int ItemType
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "כמות")]
        public int ItemsCount
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מס-סידורי")]
        public int ItemIndex
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "תחום")]
        public int ItemRange
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מחיר")]
        public decimal ItemPrice
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "שרת")]
        public int Server
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "סוג נתונים")]
        public string DataSource
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
        [EntityProperty(EntityPropertyType.Default, Caption = "ארגומנט זיהוי")]
        public int ArgId
        {
            get;
            set;
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "ארגומנטים")]
        public string Args
        {
            get;
            set;
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "משתמש")]
        public int UserId
        {
            get;
            set;
        }
       
        #endregion

        #region context
        /*
        public static EntityDb GetDb()
        {
            return EntityDb.Create<NetcellDB>("Scheduler_Queue", EntityKeys.Get("QueueId"));
        }

        public static int Insert(Scheduler_Queue item)
        {
            using (EntityContext<Scheduler_Queue> context = new EntityContext<Scheduler_Queue>(item))
            {
                context.EntityDb = GetDb();
                return context.SaveChanges(UpdateCommandType.Insert);
            }
        }

        public static Scheduler_Queue Get(Guid queueId)
        {
            using (EntityContext<Scheduler_Queue> context = new EntityContext<Scheduler_Queue>())
            {
                context.EntityDb = GetDb();
                context.Set(queueId);
                return context.Entity;
            }
        }

        public static Scheduler_Queue Get(DataRow dr)
        {
            using (EntityContext<Scheduler_Queue> context = new EntityContext<Scheduler_Queue>())
            {
                context.EntityDb = GetDb();
                context.EntityRecord=new GenericRecord(dr);
                return context.Entity;
            }
        }

        public static List<Scheduler_Queue> GetListItems(int itemId)
        {
            using (EntityContext<Scheduler_Queue> context = new EntityContext<Scheduler_Queue>())
            {
                context.EntityDb = GetDb();
                return context.EntityList(DataFilter.Get("ItemId=@ItemId", itemId));
            }
        }

        public static DataTable GetList(int itemId)
        {
            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.ExecuteCommand<DataTable>("select * from Scheduler_Queue where ItemId=@ItemId", DataParameter.Get("ItemId", itemId));
            }
        }
         */ 
        #endregion

        #region methods

        public BatchTypes GetBatchType()
        {
            switch ((SchedulerItemType)this.ItemType)
            {
                case SchedulerItemType.Executed:
                    return BatchTypes.Single;
                case SchedulerItemType.Scheduled:
                    return BatchTypes.Scheduled;
                case SchedulerItemType.Preview:
                    return BatchTypes.Preview;
                default:
                    return BatchTypes.Auto;
            }
        }

 
        public string Serialize()
        {
            return Nistec.Serialization.BinarySerializer.SerializeToBase64(this);
        }

        public string Print()
        {
            return string.Format("Scheduler Item Args:{0}, ItemId:{1}, ArgId:{2}, AccountId:{3},DataSource:{4}", Args, ItemId, ArgId, AccountId, DataSource);
        }
        
        #endregion
    }
 
}
