using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using Nistec.Data.Entities;
using Nistec.Generic;
using Nistec.Data;
using Netcell.Data.Db;
using Nistec.Serialization;
using Nistec;

namespace Netcell.Data.Entities
{

    [Entity("ReminderEntity", "Reminder", "cnn_Netcell", EntityMode.Generic, "ReminderId")]
    public class ReminderEntity_Context : EntityContext<ReminderEntity>
    {
        #region ctor
        public ReminderEntity_Context(int ReminderId)
            : base(ReminderId)
        {

        }
        public ReminderEntity_Context(DataFilter filter)
            : base()
        {
            Init(filter);
        }
        public ReminderEntity_Context(ReminderEntity item)
            : base(item)
        {

        }
        protected ReminderEntity_Context()
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

        public static int Insert(ReminderEntity item)
        {
            int res = 0;
            using (ReminderEntity_Context context = new ReminderEntity_Context(item))
            {
                res = context.SaveChanges(UpdateCommandType.Insert);
            }
            if (res > 0)
            {
                Scheduler_Queue_Context.Insert(item);
            }

            return res;
        }

        public static ReminderEntity Get(int reminderId)
        {
            using (ReminderEntity_Context context = new ReminderEntity_Context(reminderId))
            {
                return context.Entity;
            }
        }


        //public static List<Reminder_Targets> GetReminderTargetsListItems(int ReminderId)
        //{
        //    DataTable dt = null;
        //    using (IDbCmd cmd = NetcellDB.Instance.DbCmd())
        //    {
        //        dt = cmd.ExecuteCommand<DataTable>("select * from Reminder_Targets where ReminderId=@ReminderId", DataParameter.Get("ReminderId", ReminderId));
        //        if (dt == null || dt.Rows.Count == 0)
        //            return null;
        //    }

        //    return EntityFormatter.DataTableToEntity<Reminder_Targets>(dt);
        //}

        public int Exec_Reminder_Completed(int ReminderId, Guid QueueId, int TransId)
        {

            //using (DalController instance = new DalController())
            //{
            //    instance.Reminder_Completed(ReminderId, QueueId.ToString(), transId);
            //}

            using (var cmd = NetcellDB.Instance)
            {
                return cmd.ExecuteCommandNonQuery("sp_Reminder_Completed", DataParameter.Get("ReminderId", ReminderId, "QueueId", QueueId, "TransId", TransId), CommandType.StoredProcedure);
            }
        }
       
       #endregion
    }

    [Serializable]
    public class ReminderEntity : IEntityItem
    {

        #region Ex Properties

        public bool IsEmpty
        {
            get { return Types.IsEmpty(ReminderId); }
        }

        [EntityProperty(EntityPropertyType.NA)]
        public DateTime CurrentTime
        {
            get { return DateTime.Now; }
        }

        [EntityProperty(EntityPropertyType.NA)]
        public bool IsExpired
        {
            get { return ExpirationDate < CurrentTime; }
        }
                       
        public static DateTime GetDefaultExpiration()
        {
           return DateTime.Now.AddHours(6);
        }

        #endregion

        #region Properties

       [EntityProperty(EntityPropertyType.Identity, Caption = "רשומה")]
        public int ReminderId
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
        [EntityProperty(EntityPropertyType.Default, Caption = "מאת")]
        public string Sender
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "נוסח ההודעה")]
        public string Message
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "סוג-תזכורת")]
        public int ReminderType
        {
            get;
            set;
        }
        //[EntityProperty(EntityPropertyType.Default, Caption = "שדה תאריך")]
        //public string DateField
        //{
        //    get;
        //    set;
        //}
        //[EntityProperty(EntityPropertyType.Default, Caption = "מחיר")]
        //public decimal ItemPrice
        //{
        //    get;
        //    set;
        //}
        //[EntityProperty(EntityPropertyType.Default, Caption = "שרת")]
        //public int Server
        //{
        //    get;
        //    set;
        //}
        //[EntityProperty(EntityPropertyType.Default, Caption = "סוג נתונים")]
        //public string DataSource
        //{
        //    get;
        //    set;
        //}
        [EntityProperty(EntityPropertyType.Default, Caption = "סטטוס")]
        public int Status
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "נוצר ב")]
        public DateTime CreationDate
        {
            get;
            set;
        }
       [EntityProperty(EntityPropertyType.Default, Caption = "תפוגה")]
        public DateTime ExpirationDate
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מועד לביצוע")]
        public DateTime DateToSend
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "פרסונאלי")]
        public bool IsPeronal
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "שדות פרסונאלים")]
        public string PersonalDisplay
        {
            get;
            set;
        }
        //[EntityProperty(EntityPropertyType.Default, Caption = "האם לשרשר")]
        //public bool Concatenate
        //{
        //    get;
        //    set;
        //}
        [EntityProperty(EntityPropertyType.Default, Caption = "פריט")]
        public int MtId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "נמענים")]
        public string Targets
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
        [EntityProperty(EntityPropertyType.Default, Caption = "מחיר")]
        public decimal ItemPrice
        {
            get;
            set;
        }

        #endregion

        #region context
        /*
        public static EntityDb GetDb()
        {
            return EntityDb.Create<NetcellDB>("ReminderEntity", EntityKeys.Get("QueueId"));
        }

        public static int Insert(ReminderEntity item)
        {
            using (EntityContext<ReminderEntity> context = new EntityContext<ReminderEntity>(item))
            {
                context.EntityDb = GetDb();
                return context.SaveChanges(UpdateCommandType.Insert);
            }
        }

        public static ReminderEntity Get(Guid queueId)
        {
            using (EntityContext<ReminderEntity> context = new EntityContext<ReminderEntity>())
            {
                context.EntityDb = GetDb();
                context.Set(queueId);
                return context.Entity;
            }
        }

        public static ReminderEntity Get(DataRow dr)
        {
            using (EntityContext<ReminderEntity> context = new EntityContext<ReminderEntity>())
            {
                context.EntityDb = GetDb();
                context.EntityRecord=new GenericRecord(dr);
                return context.Entity;
            }
        }

        public static List<ReminderEntity> GetListItems(int itemId)
        {
            using (EntityContext<ReminderEntity> context = new EntityContext<ReminderEntity>())
            {
                context.EntityDb = GetDb();
                return context.EntityList(DataFilter.Get("ItemId=@ItemId", itemId));
            }
        }

        public static DataTable GetList(int itemId)
        {
            using (IDbCmd cmd = NetcellDB.Instance.DbCmd())
            {
                return cmd.ExecuteCommand<DataTable>("select * from ReminderEntity where ItemId=@ItemId", DataParameter.Get("ItemId", itemId));
            }
        }
         */ 
        #endregion

        #region methods

        public string Serialize()
        {
            return NetSerializer.SerializeToBase64(this);
        }

        public string Print()
        {
            return string.Format("Scheduler Item ReminderId:{0}, DateToSend:{1}, AccountId:{2}", ReminderId, DateToSend, AccountId);
        }

        #endregion

        #region Targets

        public DataTable TargetsToDataTable()
        {
            if (string.IsNullOrEmpty(Targets))
                return null;
            return TargetEntity.TargetsToDataTable(TargetEntity.DeserializeList(Targets).ToArray());
        }

        public List<TargetEntity> DeserializeTargets()
        {
            if (string.IsNullOrEmpty(Targets))
                return null;
            return TargetEntity.DeserializeList(Targets);
        }

        #endregion
    }

}
