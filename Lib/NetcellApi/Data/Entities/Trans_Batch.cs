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

    public interface IMailView
    {
        string Body { get; }
        int Size { get; }
        int Units { get; }
    }
   
    [Entity("Trans_Batch", "Trans_Batch", "cnn_Netcell", EntityMode.Generic, "BatchId")]
    public class Trans_Batch_Context : EntityContext<Trans_Batch>
    {
        #region ctor
        public Trans_Batch_Context(int BatchId)
            : base(BatchId)
        {

        }
        public Trans_Batch_Context(DataFilter filter)
            : base()
        {
            Init(filter);
        }
        public Trans_Batch_Context(Trans_Batch item)
            : base(item)
        {

        }
        protected Trans_Batch_Context()
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

        public static Trans_Batch Get(int BatchId)
        {
            using (Trans_Batch_Context context = new Trans_Batch_Context(BatchId))
            {
                return context.Entity;
            }
        }

        
        public static Scheduler_Queue ToSchedulerQueue(Trans_Batch tb,SchedulerItemType itemType= SchedulerItemType.Executed)
        {
            if(tb==null)
            {
                throw new ArgumentNullException("ToSchedulerQueue.Trans_Batch");
            }
            Scheduler_Queue q = new Scheduler_Queue()
            {
                ItemId=tb.CampaignId,
                ItemIndex=tb.BatchIndex,
                ItemPrice=tb.DefaultPrice,
                ItemRange=tb.BatchRange,
                ItemsCount=tb.BatchCount,
                ItemType = (int)itemType,
                AccountId=tb.AccountId,
                ArgId=tb.BatchId,
                Creation=tb.Creation,
                DataSource=SchedulerDataSource.Batch.ToString(),
                ExecTime=tb.BatchTime,
                Expiration = Scheduler_Queue.GetDefaultExpiration(tb.BatchTime),
                Server=tb.Server,
                State=0,
                UserId=tb.UserId,
                Args=tb.PublishKey
            };

            return q;
        }

        
        public static Trans_Batch Get(DataRow dr)
        {
            using (Trans_Batch_Context context = new Trans_Batch_Context())
            {
                context.EntityRecord = new GenericRecord(dr);
                return context.Entity;
            }
        }

        public static List<Trans_Batch> GetCampaignBatchList(int CampaignId)
        {
            using (Trans_Batch_Context context = new Trans_Batch_Context())
            {
                return context.EntityList(DataFilter.Get("CampaignId=@CampaignId", CampaignId));
            }
        }

        public static DataTable GetCampaignBatches(int CampaignId)
        {
            using (var cmd = NetcellDB.Instance)
            {
                return cmd.ExecuteCommand<DataTable>("select * from Trans_Batch where CampaignId=@CampaignId", DataParameter.Get("CampaignId", CampaignId));
            }
        }
        public static string Lookup_TransBatchComment(int BatchId)
        {
            using (var cmd = NetcellDB.Instance)
            {
                return cmd.NewCmd().LookupQuery<string>("Comment","Trans_Batch_Sent", "BatchId=@BatchId","",new object[]{ BatchId});
            }
        }
        public static Trans_Batch Deserialize(string base64)
        {
            return NetSerializer.DeserializeFromBase64<Trans_Batch>(base64);
        }

          #endregion
    }

    [Serializable]
    public class Trans_Batch : IEntityItem
    {
        public Trans_Batch() { }


        #region Ex Properties

        public bool IsEmpty
        {
            get { return Types.IsEmpty(BatchId); }
        }

        
        #endregion

        #region Properties

       [EntityProperty(EntityPropertyType.Key, Caption = "רשומה")]
        public int BatchId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מס-פריט")]
       public int CampaignId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "סוג")]
        public int BatchType
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "כמות")]
        public int BatchCount
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מס-סידורי")]
        public int BatchIndex
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "תחום")]
        public int BatchRange
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מחיר")]
        public decimal DefaultPrice
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
        [EntityProperty(EntityPropertyType.Default, Caption = "סטטוס")]
        public int BatchStatus
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
        [EntityProperty(EntityPropertyType.Default, Caption = "מועד לביצוע")]
       public DateTime BatchTime
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
        [EntityProperty(EntityPropertyType.Default, Caption = "מדיה")]
        public int Platform
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "פריט")]
        public int MtId
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
        [EntityProperty(EntityPropertyType.Default, Caption = "מזהה פרסום")]
        public string PublishKey
        {
            get;
            set;
        }
        #endregion

        #region methods

        public string Serialize()
        {
            return NetSerializer.SerializeToBase64(this);
        }

        public string Print()
        {
            return string.Format("Batch Item BatchId:{0}, BatchType:{1}, BatchCount:{2}, AccountId:{3}", BatchId, ((BatchTypes)BatchType).ToString(), BatchCount, AccountId);
        }

        public bool Validate(int accountId)
        {
            if (AccountId != accountId)
                return false;
            return true;
        }
        #endregion
    }

    
    
}
