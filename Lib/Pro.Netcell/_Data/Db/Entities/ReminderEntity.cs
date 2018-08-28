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


namespace Netcell.Data.Db.Entities
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

            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.ExecuteNonQuery("sp_Reminder_Completed", DataParameter.GetSql("ReminderId", ReminderId, "QueueId", QueueId, "TransId", TransId), CommandType.StoredProcedure);
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
            return Nistec.Runtime.Serialization.SerializeToBase64(this);
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

        /*
        public static string SerializeTargets(string targets)
        {
            if (targets == null)
            {
                throw new ArgumentNullException("ReminderEntity.CreateTargets");
            }
            string[] args = targets.Split(';');
            return SerializeTargets(args);

            return TargetEntity.SerializeList(targets);
        }

        public static string SerializeTargets(string[] targets)
        {
            if (targets == null)
            {
                throw new ArgumentNullException("ReminderEntity.CreateTargets");
            }
            if (targets.Length > 2000)
            {
                throw new ArgumentException("ReminderEntity.CreateTargets: targets max send exceeded");
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            foreach (string s in targets)
            {
                sb.Append(FormatTarget(s) + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");

            return sb.ToString();
        }

        public static string FormatTarget(string to)
        {
            string[] args = to.Split(':');
            if (args.Length > 1)
                return FormatTarget(args[0], args[1]);
            return FormatTarget(args[0], "");
        }

        public static string FormatTarget(string to, string personal)
        {
            return "{" + string.Format("To:{0},Pr:{1}", to, personal) + "}";
        }
        */
        #endregion
    }
/*
    [Serializable]
    public class ReminderTarget
    {

        public string To { get; set; }
        public string Args { get; set; }

        public ReminderTarget()
        {

        }
        public ReminderTarget(string jtarget)
        {
            //string[] args = jtarget.Split(',');

            string[] args = jtarget.Split(':');

            Target = args[0].Replace("{", "").Replace("}", "");
            Personal = args.Length > 1 ? args[1].Replace("}", "") : "";
        }

        public ReminderTarget(string target, string personal)
        {
            Target = target;
            Personal = personal;
        }


        public static DataTable ReminderTargetSchema()
        {
            DataTable dt = new DataTable("Reminder_Targets");
            //dt.Columns.Add("ReminderId", typeof(Int32));
            dt.Columns.Add("Target");
            dt.Columns.Add("Personal");
            return dt.Clone();
        }

    }
 */

    /*
    [Serializable]
    public class Reminder_Targets : IEntityItem
    {

        #region Ex Properties

        public bool IsEmpty
        {
            get { return Types.IsEmpty(Cli); }
        }

        #endregion

        #region Properties

       [EntityProperty(EntityPropertyType.Identity, Caption = "רשומה")]
        public int ReminderId
        {
            get;
            set;
        }
      
        [EntityProperty(EntityPropertyType.Default, Caption = "אל")]
        public string Cli
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "פרסונאל")]
        public string Personal
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מדיה")]
        public int Platform
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מס-רשומה")]
        public int Id
        {
            get;
            set;
        }
        #endregion

        #region methods

        public string Print()
        {
            return string.Format("Scheduler Item ReminderId:{0}, Cli:{1}", ReminderId, Cli);
        }
        
        #endregion
    }
    */
}
