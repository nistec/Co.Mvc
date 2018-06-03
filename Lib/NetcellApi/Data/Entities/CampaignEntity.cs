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
using Nistec.Runtime;

namespace Netcell.Data.Entities
{
      
    [Entity("CampaignEntity", "Campaigns", "cnn_Netcell", EntityMode.Generic, "CampaignId")]
    public class CampaignEntity_Context : EntityContext<CampaignEntity>
    {
        #region ctor
        public CampaignEntity_Context(int CampaignId)
            : base(CampaignId)
        {

        }
        public CampaignEntity_Context(DataFilter filter)
            : base()
        {
            Init(filter);
        }
        public CampaignEntity_Context(CampaignEntity item)
            : base(item)
        {

        }
        protected CampaignEntity_Context()
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

        //public static CampaignEntity Dequeue(int server)
        //{
        //    //using (Campaigns_Context context = new Campaigns_Context())
        //    //{
        //    //    context.Init("sp_Scheduler_DeQueue", DataParameter.Get("Server", server), CommandType.StoredProcedure);
        //    //    return context.IsEmpty ? null : context.Entity;
        //    //}

        //    DataRow dr = null;
        //    using (IDbCmd cmd = NetcellDB.Instance.DbCmd())
        //    {
        //        dr = cmd.ExecuteCommand<DataRow>("sp_Scheduler_DeQueue", DataParameter.Get("Server", server), CommandType.StoredProcedure);
        //    }
        //    if (dr == null)
        //    {
        //        return null;
        //    }

        //    return Get(dr);
        //}


        public static int Insert(CampaignEntity item)
        {
            using (CampaignEntity_Context context = new CampaignEntity_Context(item))
            {
               return context.SaveChanges(UpdateCommandType.Insert);
            }
        }

        public static CampaignEntity Get(int CampaignId)
        {
            using (CampaignEntity_Context context = new CampaignEntity_Context(CampaignId))
            {
                return context.Entity;
            }
        }

        public static CampaignEntity Get(DataRow dr)
        {
            using (CampaignEntity_Context context = new CampaignEntity_Context())
            {
                context.EntityRecord = new GenericRecord(dr);
                return context.Entity;
            }
        }

        public string ToHtml(bool encrypt)
        {
            string h = this.EntityProperties.ToHtmlTable("", "");
            if (encrypt)
                return Encryption.ToBase64String(h, true);
            return h;
        }

      
        #endregion

        #region lookup methods

        public static int Lookup_Method(int CampaignId)
        {
            using (var cmd = NetcellDB.Instance)
            {
                return cmd.NewCmd().LookupQuery<int>("MtId", "Campaigns", "CampaignId=@CampaignId", 0, new object[] { CampaignId });
            }
        }

   
        #endregion
    }


    public class CampaignEntity : ICampaignEntity
    {

        #region Ex Properties

        public bool IsEmpty
        {
            get { return Types.IsEmpty(CampaignId); }
        }
        
        #endregion

        #region properties

        [EntityProperty(EntityPropertyType.Identity, Caption = "דיוור")]
        public int CampaignId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "סוג הדיוור")]
        public int CampaignType//send type
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "שם הדיוור")]
        public string CampaignName
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "נושא")]
        public string CampaignPromo
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
        [EntityProperty(EntityPropertyType.Default, Caption = "מועד שליחה")]
        public DateTime DateToSend
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
        [EntityProperty(EntityPropertyType.Default, Caption = "נמענים")]
        public int TotalCount
        {
            get;
            set;
        }

        //[EntityProperty(EntityPropertyType.Default, Caption = "יח-חיוב")]
        //public int Units
        //{
        //    get;
        //    set;
        //}

        [EntityProperty(EntityPropertyType.Default, Caption = "מאת")]
        public string Sender
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "סטטוס")]
        public int Status
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
        [EntityProperty(EntityPropertyType.Default, Caption = "נוסח")]
        public string MessageText
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "לינק")]
        public string Url
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "טווח זמן מ")]
        public int ValidTimeBegin
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "טווח זמן עד")]
        public int ValidTimeEnd
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מנות")]
        public int BatchCount
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "השב אל")]
        public string ReplyTo
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "אופן משלוח עדכונים")]
        public int NotifyType
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "עדכונים אל")]
        public string NotifyCells
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "פרסונאלי")]
        public int PersonalLength
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מס-עיצוב")]
        public int DesignId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "תצוגה")]
        public string Display
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.NA, Caption = "פרסונאלי")]
        public bool IsPersonal
        {
            get { return PersonalLength > 0; }
            //set;
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "סוג חיוב")]
        public int BillType
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
        //0=he 1=en
        [EntityProperty(EntityPropertyType.Default, Caption = "שפה")]
        public int Lang
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "טיוטא")]
        public bool IsDraft
        {
            get;
            set;
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "תאריך")]
        public int DateIndex
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

        [EntityProperty(EntityPropertyType.Default, Caption = "שדות פרסונאלים")]
        public int CampaignProductType
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "אפשרויות")]
        public string Features
        {
            get;
            set;
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "שדות פרסונאלים")]
        public string PersonalFields
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

        #endregion

        #region methods

        public string Print()
        {
            return string.Format("Scheduler Item QueueId:{0}, ItemId:{1}, ArgId:{2}, AccountId:{3}", CampaignId, CampaignName, AccountId);
        }

        public bool Validate(int accountId)
        {
            if (AccountId != accountId)
                return false;
            return true;
        }

        public string GetValidTimeBegin()
        {
            return Strings.IntTimeToString(ValidTimeBegin);
        }
        public string GetValidTimeEnd()
        {
            return Strings.IntTimeToString(ValidTimeEnd);
        }
        #endregion
    }


    public interface ICampaignEntity : IEntityItem
    {
        #region Ex Properties

         bool IsEmpty
        {
            get;
        }

        #endregion

        #region properties

        [EntityProperty(EntityPropertyType.Identity, Caption = "דיוור")]
         int CampaignId
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "סוג הדיוור")]
         int CampaignType//send type
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "שם הדיוור")]
         string CampaignName
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "נושא")]
         string CampaignPromo
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "נוצר ב")]
         DateTime CreationDate
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מועד שליחה")]
         DateTime DateToSend
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "תפוגה")]
         DateTime ExpirationDate
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "נמענים")]
         int TotalCount
        {
            get;
            
        }
        // int Units
        //{
        //    get;
            
        //}

        [EntityProperty(EntityPropertyType.Default, Caption = "מאת")]
         string Sender
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "סטטוס")]
         int Status
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "חשבון")]
         int AccountId
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "נוסח")]
         string MessageText
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "לינק")]
         string Url
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "טווח זמן מ")]
         int ValidTimeBegin
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "טווח זמן עד")]
         int ValidTimeEnd
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מנות")]
        int BatchCount
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "אופן משלוח התראה")]
         int NotifyType
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "השב אל")]
         string NotifyCells
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "פרסונאלי")]
         int PersonalLength
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מס-עיצוב")]
         int DesignId
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "תצוגה")]
         string Display
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.NA, Caption = "פרסונאלי")]
        bool IsPersonal
        {
            get;
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "סוג חיוב")]
         int BillType
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מדיה")]
         int Platform
        {
            get;
            
        }
        //0=he 1=en
        [EntityProperty(EntityPropertyType.Default, Caption = "שפה")]
         int Lang
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "טיוטא")]
        bool IsDraft
        {
            get;
            
        }

         [EntityProperty(EntityPropertyType.Default, Caption = "תאריך")]
         int DateIndex
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "פריט")]
         int MtId
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "השב אל")]
        string ReplyTo
        {
            get;

        }
        [EntityProperty(EntityPropertyType.Default, Caption = "שדות פרסונאלים")]
         int CampaignProductType
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "אפשרויות")]
        string Features
        {
            get;
            
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "שדות פרסונאלים")]
         string PersonalFields
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "שדות פרסונאלים")]
         string PersonalDisplay
        {
            get;
            
        }

        #endregion

        #region methods

         string Print();
       
         bool Validate(int accountId);
        

         string GetValidTimeBegin();
       
         string GetValidTimeEnd();
        
        #endregion
    }
}
