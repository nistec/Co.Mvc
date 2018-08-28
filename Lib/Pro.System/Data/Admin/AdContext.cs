using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using Nistec.Channels.RemoteCache;
using Nistec.Web.Controls;
using ProSystem;
using ProSystem.Query;
using Nistec;

namespace ProSystem.Data.Entities
{
    
    public class AdContext<T> : EntityContext<DbSystem, T> where T : IEntityItem
    {
        public static void Refresh(int AccountId)
        {
            DbContextCache.Remove<T>(Settings.ProjectName, EntityCacheGroups.System, AccountId, 0);
        }
        public static AdContext<T> Get()
        {
            return new AdContext<T>();
        }
        public AdContext()
        {
            //no cache
        }
        public AdContext(int accountId,int userId=0)
        {
            if (accountId > 0)
                CacheKey = DbContextCache.GetKey<T>(Settings.ProjectName, EntityCacheGroups.System, accountId, userId);
        }
        public IList<T> ExecList(params object[] keyValueParameters)
        {
            return DbContextCache.ExecuteList<DbSystem, T>(CacheKey, keyValueParameters);
        }
        public IList<T> GetList()
        {
            return DbContextCache.EntityList<DbSystem, T>(CacheKey, null);
        }
        public IList<T> GetList(int accountId)
        {
            return DbContextCache.EntityList<DbSystem, T>(CacheKey, new object[] { "AccountId", accountId });
        }
        protected override void OnChanged(ProcedureType commandType)
        {
            DbContextCache.Remove(CacheKey);
        }
        public FormResult GetFormResult(EntityCommandResult res, string reason)
        {
            return FormResult.Get(res, this.EntityName, reason);
        }

    }
    public class AdContext 
    {
        public static string LookupAccountFolder(int AccountId)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.QueryScalar<string>("select Path from Ad_Account where AccountId=@AccountId", null, "AccountId", AccountId);
        }
        public static int LookupAccountFolder(string folder)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.QueryScalar<int>("select AccountId from Ad_Account where Path=@Path", 0, "Path", folder);
        }
        public static string GetLabelJson(int AccountId, string SourceType="*")
        {
            using (var db = DbContext.Get<DbSystem>())
                return db.ExecuteJsonArray("sp_Ad_Labels_Get", "AccountId", AccountId, "SourceType", SourceType);
        }
        public static IEnumerable<AdAccountView> ListQueryView(AccountsQuery q)
        {
 
            using (var db = DbContext.Create<DbSystem>())
            {
                return db.ExecuteList<AdAccountView>("sp_Accounts_Query", "QueryType", q.QueryType, "PageSize", q.PageSize, "PageNum", q.PageNum, 
                   "AccountId", q.AccountId,
                   "ParentId", q.ParentId,
                    "IdNumber", q.IdNumber,
                    "Mobile", q.Mobile,
                    "Email", q.Email,
                    "Name", q.AccountName,
                    "Address", q.Address,
                    "City", q.City,
                    "Category", q.Category,
                    "Branch", q.Branch,
                    "JoinedFrom", q.JoinedFrom,
                    "JoinedTo", q.JoinedTo,
                    "ContactRule", q.ContactRule,
                    "Sort", q.Sort,
                    "Filter", q.Filter
                    );
            }
        }
        public static int DoSave(AdAccount v , bool EnableSync)
        {
             var args = new object[]{
                "AccountId", v.AccountId
                ,"ParentId", v.ParentId
                ,"ContactName",v.ContactName
                ,"Address", v.Address
                ,"City", v.City
                ,"ZipCode", v.ZipCode
                ,"Phone",v.Phone
                ,"Fax",v.Fax
                ,"Mobile",v.Mobile
                ,"Email", v.Email

                ,"IdNumber", v.IdNumber
                ,"OwnerId", v.OwnerId
                ,"AccType", v.AccType
                ,"AccountCategory", v.AccountCategory
                ,"Details", v.Details
                ,"IsActive", v.IsActive
                ,"Path", v.Path
                ,"Branch", v.Branch
                ,"MailSender", v.MailSender
                ,"SmsSender", v.SmsSender
                ,"MemberId", v.MemberId
                ,"EphoneId", v.EphoneId
                ,"EnableSync", EnableSync ? 1:0
            };


            //var parameters = DataParameter.GetSqlList(args);
            using (var db = DbContext.Create<DbSystem>())
            {
                int res = db.ExecuteReturnValue("sp_Ad_Account_Update",-1, args);
                v.AccountId = res;
                return res;
            }
        }
    }

    [EntityMapping("Ad_Account", ProcDelete = null, ProcUpdate = null, ProcInsert = null)]
    public class AdAccount : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Identity)]
        public int AccountId { get; set; }
        public int ParentId { get; set; }
        public string AccountName { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string IdNumber { get; set; }
        public int Country { get; set; }
        public int OwnerId { get; set; }
        public int AccType { get; set; }
        public int AccountCategory { get; set; }
        public string Details { get; set; }
        public bool IsActive { get; set; }
         [EntityProperty(EntityPropertyType.View)]
        public DateTime CreationDate { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime? LastUpdate { get; set; }
        public string Path { get; set; }
        public string Branch { get; set; }
        public string MailSender { get; set; }
        public string SmsSender { get; set; }
        public int EphoneId { get; set; }
        public int MemberId { get; set; }


        [EntityProperty(EntityPropertyType.View)]
        public string AccountCategoryName { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string AccTypeName { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string ParentName { get; set; }
        
    }
    public class AdAccountView : AdAccount,IEntityListItem
    {
        public int TotalRows { get; set; }
    }

     /*
        public class AccountItem : EntityItem<DbSystem>, IEntityPro
        {
            #region override

            public override string MappingName()
            {
                return "AccountProperty";
            }

            public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
            {
                EntityValidator validator = new EntityValidator("לקוח", "he");
                if (commandType != UpdateCommandType.Delete)
                    validator.Required(PropName, "שם לקוח");
                if (PropId == 0 && commandType != UpdateCommandType.Insert)
                {
                    validator.Append("רשומה זו אינה ניתנת לעריכה");
                }
                return validator;
            }
            #endregion

            //public static IEnumerable<T> ViewEntityList<T>(string GroupName, string TableName, int AccountId) where T : IEntityPro
            //{
            //    IEnumerable<T> list = null;

            //        list = db.QueryEntityList<T>(TableName, "AccountId", AccountId);

            //    return list;
            //}



            public static string AccountsList()
            {
                using (var db = DbContext.Create<DbSystem>())
                return db.QueryJson(TableName);
            }

            public static IEnumerable<AccountItem> ViewList()
            {
                using (var db = DbContext.Create<DbSystem>())
                    return db.EntityItemList<AccountItem>(TableName);
            }

            public static AccountItem View(int AccountId)
            {
                using (var db = DbContext.Create<DbSystem>())
                    return db.EntityItemGet<AccountItem>(TableName, "AccountId", AccountId);
            }

            public const string TableName = "AccountProperty";
            public const string TagPropId = "קוד לקוח";
            public const string TagPropName = "שם לקוח";
            public const string TagPropTitle = "הגדרות לקוח";

            #region properties
            [EntityProperty(EntityPropertyType.NA)]
            public int AccountId { get; set; }

            [EntityProperty(EntityPropertyType.Key, Column = "AccountId")]
            public int PropId { get; set; }

            [EntityProperty(Column = "AccountName")]
            [Validator("שם לקוח", true)]
            public string PropName { get; set; }
            public string SmsSender { get; set; }
            public string MailSender { get; set; }
            public int AuthUser { get; set; }
            public int AuthAccount { get; set; }
            public bool EnableSms { get; set; }
            public bool EnableMail { get; set; }
            public string Path { get; set; }
            public string SignupPage { get; set; }
            public bool EnableInputBuilder { get; set; }
            public bool BlockCms { get; set; }
            public string Design { get; set; }

            [EntityProperty(EntityPropertyType.View)]
            public int SignupOption { get; set; }
            [EntityProperty(EntityPropertyType.View)]
            public bool EnableSignupCredit { get; set; }
            [EntityProperty(EntityPropertyType.View)]
            public string CreditTerminal { get; set; }
            [EntityProperty(EntityPropertyType.View)]
            public string RecieptAddress { get; set; }
            [EntityProperty(EntityPropertyType.View)]
            public string RecieptEvent { get; set; }


            #endregion

            public T Get<T>(int PropId) where T : IEntityItem
            {
                using (var db = DbContext.Create<DbSystem>())
                    return db.EntityItemGet<T>(TableName, "AccountId", PropId);
            }

            public static int DoSave(AccountItem v, UpdateCommandType command)
            {
                int result = 0;

                string TableName = v.MappingName();

                if (command == UpdateCommandType.Delete)
                {
                    //result = v.DoDelete<CampaignView>();
                }
                else
                {

                    var args = new object[]{
                    "CommandType",(int)command,
                    "AccountId",v.PropId,
                    "AccountName",v.PropName,
                    "SmsSender",v.SmsSender,
                    "MailSender",v.MailSender,
                    "AuthUser",v.AuthUser,
                    "AuthAccount",v.AuthAccount,
                    "EnableSms",v.EnableSms,
                    "EnableMail",v.EnableMail,
                    "Path",v.Path,
                    "SignupPage",v.SignupPage,
                    "EnableInputBuilder",v.EnableInputBuilder,
                    "BlockCms",v.BlockCms,
                    "Design",v.Design
                    //"SignupOption",v.SignupOption,
                    //"EnableSignupCredit",v.EnableSignupCredit,
                    //"CreditTerminal",v.CreditTerminal,
                    //"RecieptEvent",v.RecieptEvent,
                    //"RecieptAddress",v.RecieptAddress
                    };
                    using (var db = DbContext.Create<DbSystem>())
                    {
                        result = db.ExecuteNonQuery("sp_Account_Property_Save", args);
                    }
                }

                //switch ((int)command)
                //{
                //    case 0://insert
                //        result = newItem.DoInsert<CampaignView>();
                //        break;
                //    case 2://delete
                //        result = newItem.DoDelete<CampaignView>();
                //        break;
                //    default:
                //        CampaignView current = newItem.Get<CampaignView>(PropId);
                //        result = current.DoUpdate(newItem);
                //        break;
                //}

                //EntityPro.CacheRemove(EntityPro.GetKey(TableName, AccountId));
                WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, v.PropId, TableName));
                return result;
            }

        }

        public class AccountCreditItem : EntityItem<DbSystem>, IEntityPro
        {
            #region override

            public override string MappingName()
            {
                return "AccountProperty";
            }

            public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
            {
                EntityValidator validator = new EntityValidator("סליקה", "he");
                if (commandType != UpdateCommandType.Delete)
                    validator.Required(PropName, "שם לקוח");
                if (PropId == 0 && commandType != UpdateCommandType.Insert)
                {
                    validator.Append("רשומה זו אינה ניתנת לעריכה");
                }
                return validator;
            }
            #endregion

            public static AccountCreditItem View(int AccountId)
            {
                using (var db = DbContext.Create<DbSystem>())
                    return db.EntityItemGet<AccountCreditItem>(TableName, "AccountId", AccountId);
            }

            public const string TableName = "AccountProperty";
            public const string TagPropId = "קוד לקוח";
            public const string TagPropName = "שם לקוח";
            public const string TagPropTitle = "הגדרות לקוח";

            #region properties
            [EntityProperty(EntityPropertyType.NA)]
            public int AccountId { get; set; }

            [EntityProperty(EntityPropertyType.Key, Column = "AccountId")]
            public int PropId { get; set; }

            [EntityProperty(Column = "AccountName")]
            [Validator("שם לקוח", true)]
            public string PropName { get; set; }
            public int SignupOption { get; set; }
            public bool EnableSignupCredit { get; set; }
            public string CreditTerminal { get; set; }
            public string RecieptAddress { get; set; }
            public string RecieptEvent { get; set; }


            #endregion

            public T Get<T>(int PropId) where T : IEntityItem
            {
                using (var db = DbContext.Create<DbSystem>())
                    return db.EntityItemGet<T>(TableName, "AccountId", PropId);
            }

            public static int DoSave(AccountCreditItem v)
            {
                int result = 0;

                string TableName = v.MappingName();

                var args = new object[]{
                    "AccountId",v.PropId,
                    "SignupOption",v.SignupOption,
                    "EnableSignupCredit",v.EnableSignupCredit,
                    "CreditTerminal",v.CreditTerminal,
                    "RecieptEvent",v.RecieptEvent,
                    "RecieptAddress",v.RecieptAddress
                    };
                using (var db = DbContext.Create<DbSystem>())
                {
                    result = db.ExecuteNonQuery("sp_Account_Property_Credit_Save", args);

                    WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, v.PropId, TableName));
                    return result;
                }
            }

        }
        */

    #region Ad
        [EntityMapping("Ad", "vw_Ad", ProcDelete = null, ProcUpdate = null, ProcInsert = null)]
    public class AdItem:IEntityItem
    {
        [EntityProperty(EntityPropertyType.Identity, Column = "GroupId")]
        public int GroupId { get; set; }

        [EntityProperty(Column = "GroupName")]
        [Validator("שם קבוצה", true)]
        public string GroupName { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }
       
        [EntityProperty(EntityPropertyType.View)]
        public DateTime Creation { get; set; }
        [EntityProperty(EntityPropertyType.Optional)]
        public int MembersCount { get; set; }
    }

    //[EntityMapping("vw_Ad")]
    //public class AdItemView : AdItem
    //{
    //   public int MembersCount { get; set; }
    //}

    [EntityMapping("Ad_Rel", "vw_Ad_Rel", ProcUpdate="sp_Ad_Rel_Update")]
    public class AdItemRel:IEntityItem
    {
        [EntityProperty(EntityPropertyType.Key)]
        public int GroupId { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int UserId { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime Creation { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string DisplayName { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string ProfessionName { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string Email { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string Phone { get; set; }
        public int AccountId { get; set; }
    }

    [EntityMapping(ProcListView = "sp_Ad_Rel")]
    public class AdItemRelAll : AdItemRel
    {

    }
    #endregion

    #region AdTeam

    [EntityMapping("Ad_Team", "vw_Ad_Team", ProcDelete = null, ProcUpdate = null, ProcInsert = null)]
    public class AdTeamItem : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Identity, Column = "TeamId")]
        public int TeamId { get; set; }

        [EntityProperty(Column = "TeamName")]
        [Validator("שם קבוצה", true)]
        public string TeamName { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }

        [EntityProperty(EntityPropertyType.View)]
        public DateTime Creation { get; set; }
        [EntityProperty(EntityPropertyType.Optional)]
        public int MembersCount { get; set; }
    }
    //[EntityMapping("vw_Ad_Team")]
    //public class AdTeamItemView : AdTeamItem
    //{
    //    public int MembersCount { get; set; }
    //}
     [EntityMapping("Ad_Team_Rel", "vw_Ad_Team_Rel", ProcUpdate = "sp_Ad_Team_Rel_Update")]
    public class AdTeamItemRel : IEntityItem
    {
 
        [EntityProperty(EntityPropertyType.Key)]
        public int TeamId { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int UserId { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime Creation { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string DisplayName { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string ProfessionName { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string Email { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string Phone { get; set; }
        public int AccountId { get; set; }
    }
     [EntityMapping(ProcListView = "sp_Ad_Team_Rel")]
     public class AdTeamItemRelAll : AdTeamItemRel
     {

     }
    #endregion

    #region AdUserProfile


     [EntityMapping("Ad_UserProfile", "vw_Ad_UserProfile", ProcDelete = "sp_Ad_UserDelete", ProcUpdate = null, ProcInsert = "sp_Ad_UserRegister", ProcListView = "sp_Ad_GetUsers")]
     public class AdUserProfile : IEntityItem
     {

         [EntityProperty(EntityPropertyType.Identity)]
         public int UserId
         {
             get;
             set;
         }
         public string UserName
         {
             get;
             set;
         }
         public string DisplayName
         {
             get;
             set;
         }
         [EntityProperty(EntityPropertyType.View)]
         public DateTime Creation
         {
             get;
             set;
         }
         public string Phone
         {
             get;
             set;
         }
         public string Email
         {
             get;
             set;
         }

         public int UserRole
         {
             get;
             set;
         }
         public int AccountId
         {
             get;
             set;
         }
         public string Lang
         {
             get;
             set;
         }
         public int Evaluation
         {
             get;
             set;
         }
         public bool IsBlocked
         {
             get;
             set;
         }
         [EntityProperty(EntityPropertyType.View)]
         public string RoleName
         {
             get;
             set;
         }
         [EntityProperty(EntityPropertyType.View)]
         public string AccountName
         {
             get;
             set;
         }
         [EntityProperty(EntityPropertyType.View)]
         public string AccountCategory  
         {
             get;
             set;
         }
         //[EntityProperty(EntityPropertyType.View)]
         //public bool IsResetPass
         //{
         //    get;
         //    set;
         //}
     }

    
     #endregion

    #region AdShare

    //sp_Ad_Share_UserList @ShareModel tinyint,@AccountId int,@UserId int

     [EntityMapping("Ad_Share", ProcDelete = "sp_Ad_Share_UserDelete", ProcUpdate = null, ProcInsert = "sp_Ad_Share_UserRegister", ProcListView = "sp_Ad_Share_UserList")]
     public class AdShare : IEntityItem
     {

         [EntityProperty(EntityPropertyType.Key)]
         public int ShareModel
         {
             get;
             set;
         }
           [EntityProperty(EntityPropertyType.Key)]
         public int UserId
         {
             get;
             set;
         }

           [EntityProperty(EntityPropertyType.Key)]
         public int ShareWith
         {
             get;
             set;
         }
         public bool AllowEdit
         {
             get;
             set;
         }
         [EntityProperty(EntityPropertyType.Optional)]
         public string DisplayName
         {
             get;
             set;
         }
         [EntityProperty(EntityPropertyType.Optional)]
         public string ShareUser
         {
             get;
             set;
         }
     }
    #endregion

    #region Accounts_Label

    [EntityMapping("Accounts_Label", ProcUpsert = "sp_Accounts_Label_AddOrUpdate", ProcDelete = "sp_Accounts_Label_Del")]
    public class Accounts_Label : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Identity)]
        [Validator(Required = true, Name = "מס")]
        public long LabelId
        {
            get;
            set;
        }

        //[EntityProperty(EntityPropertyType.Key)]
        [Validator(Required =true, Name ="חשבון")]
        public int AccountId
        {
            get;
            set;
        }
        //[EntityProperty(EntityPropertyType.Key)]
        [Validator(Required = true, Name = "שם השדה")]
        public string Label
        {
            get;
            set;
        }

        public string Val
        {
            get;
            set;
        }
        public DateTime Modified
        {
            get;
            set;
        }
    }


    #endregion

}
