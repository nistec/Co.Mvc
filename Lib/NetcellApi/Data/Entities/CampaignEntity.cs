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

        [EntityProperty(EntityPropertyType.Identity, Caption = "�����")]
        public int CampaignId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��� ������")]
        public int CampaignType//send type
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�� ������")]
        public string CampaignName
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
        public string CampaignPromo
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� �")]
        public DateTime CreationDate
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� �����")]
        public DateTime DateToSend
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
        public DateTime ExpirationDate
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "������")]
        public int TotalCount
        {
            get;
            set;
        }

        //[EntityProperty(EntityPropertyType.Default, Caption = "��-����")]
        //public int Units
        //{
        //    get;
        //    set;
        //}

        [EntityProperty(EntityPropertyType.Default, Caption = "���")]
        public string Sender
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
        public int Status
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
        public int AccountId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
        public string MessageText
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
        public string Url
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� ��� �")]
        public int ValidTimeBegin
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� ��� ��")]
        public int ValidTimeEnd
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
        public int BatchCount
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��� ��")]
        public string ReplyTo
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� ����� �������")]
        public int NotifyType
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "������� ��")]
        public string NotifyCells
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��������")]
        public int PersonalLength
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��-�����")]
        public int DesignId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
        public string Display
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.NA, Caption = "��������")]
        public bool IsPersonal
        {
            get { return PersonalLength > 0; }
            //set;
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "��� ����")]
        public int BillType
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
        public int Platform
        {
            get;
            set;
        }
        //0=he 1=en
        [EntityProperty(EntityPropertyType.Default, Caption = "���")]
        public int Lang
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
        public bool IsDraft
        {
            get;
            set;
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
        public int DateIndex
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
        public int MtId
        {
            get;
            set;
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "���� ���������")]
        public int CampaignProductType
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��������")]
        public string Features
        {
            get;
            set;
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "���� ���������")]
        public string PersonalFields
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� ���������")]
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

        [EntityProperty(EntityPropertyType.Identity, Caption = "�����")]
         int CampaignId
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��� ������")]
         int CampaignType//send type
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�� ������")]
         string CampaignName
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
         string CampaignPromo
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� �")]
         DateTime CreationDate
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� �����")]
         DateTime DateToSend
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
         DateTime ExpirationDate
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "������")]
         int TotalCount
        {
            get;
            
        }
        // int Units
        //{
        //    get;
            
        //}

        [EntityProperty(EntityPropertyType.Default, Caption = "���")]
         string Sender
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
         int Status
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
         int AccountId
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
         string MessageText
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
         string Url
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� ��� �")]
         int ValidTimeBegin
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� ��� ��")]
         int ValidTimeEnd
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
        int BatchCount
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� ����� �����")]
         int NotifyType
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��� ��")]
         string NotifyCells
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��������")]
         int PersonalLength
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��-�����")]
         int DesignId
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
         string Display
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.NA, Caption = "��������")]
        bool IsPersonal
        {
            get;
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "��� ����")]
         int BillType
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
         int Platform
        {
            get;
            
        }
        //0=he 1=en
        [EntityProperty(EntityPropertyType.Default, Caption = "���")]
         int Lang
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
        bool IsDraft
        {
            get;
            
        }

         [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
         int DateIndex
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
         int MtId
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��� ��")]
        string ReplyTo
        {
            get;

        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� ���������")]
         int CampaignProductType
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��������")]
        string Features
        {
            get;
            
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "���� ���������")]
         string PersonalFields
        {
            get;
            
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� ���������")]
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
