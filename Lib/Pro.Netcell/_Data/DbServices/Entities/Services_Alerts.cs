using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Nistec.Data;
using Netcell.Data;
using Nistec.Threading;
using System.Threading;
using Nistec.Data.Entities;
using Nistec;
using Nistec.Data.Factory;
using Netcell.Data.Db.Entities;
using Nistec.Generic;

namespace Netcell.Data.DbServices.Entities
{

   [Entity("Services_Alerts", "Services_Alerts", "cnn_Services", EntityMode.Generic, "AlertId")]
    public class Services_Alerts_Context : EntityContext<Services_Alerts>
    {
        #region ctor
       public Services_Alerts_Context(int AlertId)
           : base(AlertId)
        {

        }
        public Services_Alerts_Context(DataFilter filter)
            : base()
        {
            Init(filter);
        }
        public Services_Alerts_Context(Services_Alerts item)
            : base(item)
        {

        }
        protected Services_Alerts_Context()
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

        #region methods static

        public static Services_Alerts Dequeue()
        {
            DataRow dr = null;
            using (IDbCmd cmd = Netcell_Services.Instance.NewCmd())
            {
                dr = cmd.ExecuteCommand<DataRow>("sp_ServiceAlert_Get", null, CommandType.StoredProcedure);
            }
            if (dr == null)
            {
                return null;
            }
            return Get(dr);
        }

        public static int ServiceAlert_Completed(int AlertId, int State)
        {
            using (IDbCmd cmd = Netcell_Services.Instance.NewCmd())
            {
                return cmd.ExecuteNonQuery("sp_ServiceAlert_Completed", DataParameter.GetSql("AlertId",AlertId,"State", State), CommandType.StoredProcedure);
            }
        }

        public static int Insert(Services_Alerts item)
        {
            int res = 0;
            using (Services_Alerts_Context context = new Services_Alerts_Context(item))
            {
               res= context.SaveChanges(UpdateCommandType.Insert);
               if (res > 0)
               {
                   Scheduler_Queue_Context.Insert(item);

                   //Log.DebugFormat("Services_Alerts  Scheduler_Queue Inserted : {0} ", item.Print());

               }
            }
            return res;
        }

        public static Services_Alerts Get(int AlertId)
        {
            using (Services_Alerts_Context context = new Services_Alerts_Context(AlertId))
            {
                return context.Entity;
            }
        }

        public static Services_Alerts Get(DataRow dr)
        {
            using (Services_Alerts_Context context = new Services_Alerts_Context())
            {
                context.EntityRecord = new GenericRecord(dr);
                return context.Entity;
            }
        }

        public static List<Services_Alerts> GetListItems(int ItemId)
        {
            using (Services_Alerts_Context context = new Services_Alerts_Context())
            {
                return context.EntityList(DataFilter.Get("ItemId=@ItemId", ItemId));
            }
        }

        public static DataTable GetList(int ItemId)
        {
            using (IDbCmd cmd = Netcell_Services.Instance.NewCmd())
            {
                return cmd.ExecuteCommand<DataTable>("select * from Services_Alerts where ItemId=@ItemId", DataParameter.GetSql("ItemId", ItemId));
            }
        }

        public static Services_Alerts Deserialize(string base64)
        {
            return Nistec.Serialization.BinarySerializer.DeserializeFromBase64<Services_Alerts>(base64);
        }

        #endregion
    }

    [Serializable]
    public class Services_Alerts : IEntityItem
    {
        #region ctor
        public Services_Alerts() { }

        public Services_Alerts(
            int alertType,
            int platformMode,
            DateTime execTime,
            string target,
            string sender,
            int accountId,
            int itemId,
            int argId,
            string subject,
            string body,
            string replyTo,
            string display,
            int userId,
            int units,
            int templateId) 
        { 
                AccountId = accountId;
                AlertType = (int)alertType;
                ArgId = argId;
                Body = body;
                Display = display;
                ExecTime = execTime;
                ItemId = itemId;
                Platform = platformMode;
                ReplyTo = replyTo;
                Sender = sender;
                Subject = subject;
                Target = target;
                Units = units;
                UserId = userId;
                TemplateId = templateId;
                //Expiration = ExecTime.AddHours(2);
        }
        #endregion

        #region Ex Properties

        public bool IsEmpty
        {
            get { return Types.IsEmpty(AlertId); }
        }

        #endregion

        #region properties

     
        [EntityProperty(EntityPropertyType.Identity)]
         public int AlertId
         {
             get;
             set;
         }

        [EntityProperty(EntityPropertyType.Default)]
        public int AccountId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int AlertType
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int Platform
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default)]
        public DateTime ExecTime
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string Target
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string Sender
        {
            get;
            set;
        }
       [EntityProperty(EntityPropertyType.Default)]
        public string ReplyTo
        {
            get;
            set;
        }
       [EntityProperty(EntityPropertyType.Default)]
        public string Display
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string Subject
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string Body
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int ItemId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int ArgId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int UserId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int Units
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int State
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int TemplateId
        {
            get;
            set;
        }
        
        #endregion

        #region context
        #endregion

        #region methods

        public string Serialize()
        {
            return Nistec.Serialization.BinarySerializer.SerializeToBase64(this);
        }

        public string Print()
        {
            return string.Format("Scheduler Item AlertKey:{0}, ItemId:{1}, ArgId:{2}, AccountId:{3}, Subject:{4}", AlertId, ItemId, ArgId, AccountId, Subject);
        }
        
        #endregion
    }

 
}
