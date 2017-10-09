using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Data;
using Nistec.Data;
using Nistec.Channels.RemoteCache;
using Nistec.Web.Controls;
using Pro.Lib;

namespace Pro.Data.Entities
{

    public class AccountsContext
    {
        //public static string LookupAccountFolder(int AccountId)
        //{
        //    using (var db = DbContext.Create<DbPro>())
        //        return db.QueryScalar<string>("select Path from Ad_Account where AccountId=@AccountId", null, "AccountId", AccountId);
        //}
        //public static int LookupAccountFolder(string folder)
        //{
        //    using (var db = DbContext.Create<DbPro>())
        //        return db.QueryScalar<int>("select AccountId from Ad_Account where Path=@Path", 0, "Path", folder);
        //}

        public static int DoAccountRegister(AccountRegister v)
        {
            int result = 0;

            var args = new object[]{
                 "AccountName",v.AccountName, 
                "ContactName",v.ContactName,
                "Address",v.Address,
                "City",v.City ,
                "ZipCode",v.ZipCode ,
                "Phone",v.Phone ,
                "Fax",v.Fax ,
                "Mobile",v.Mobile ,
                "Email",v.Email ,
                "IdNumber",v.IdNumber ,
                "Country",v.Country ,
                "OwnerId",v.OwnerId ,
                "AccType",v.AccType ,
                "AccountCategory",v.AccountCategory ,
                "Details",v.Details ,
                "IsActive",v.IsActive ,
                "Path",v.Path ,
                "UserDisplayName",v.UserDisplayName ,
                "UserName",v.UserName ,
                "UserRole",v.UserRole ,
                "UserLang",v.UserLang ,
                "UserEvaluation",v.UserEvaluation ,
                "UserIsBlocked",v.UserIsBlocked ,
                "UserPassword",v.UserPassword ,
                "UserProfession",v.UserProfession ,
                "SmsSender",v.SmsSender ,
                "MailSender",v.MailSender ,
                "EnableSms",v.EnableSms ,
                "EnableMail",v.EnableMail ,
                "SignupPage",v.SignupPage,
                "EnableInputBuilder",v.EnableInputBuilder ,
                "BlockCms",v.BlockCms,
                "Design",v.Design 
                };
            using (var db = DbContext.Create<DbPro>())
                result = db.ExecuteNonQuery("sp_Account_Register", args);

            return result;

        }
    }
     public class AccountRegister : IEntityItem
    {
        #region ad account
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
          public string Country { get; set; }
          public string OwnerId { get; set; }
          public string AccType { get; set; }
          public string AccountCategory { get; set; }
          public string Details { get; set; }
          public string IsActive { get; set; }
          
            #endregion

        #region user
         public string UserDisplayName { get; set; }
         public string UserName { get; set; }
         public string UserRole { get; set; }
         public string UserLang { get; set; }
         public string UserEvaluation { get; set; }
         public string UserIsBlocked { get; set; }
          public string UserPassword { get; set; }
          public string UserProfession { get; set; }

        #endregion

        #region account properties
        public int AccountId { get; set; }
        public string SmsSender { get; set; }
        public string MailSender { get; set; }
        public string AuthUser { get; set; }
        public string AuthUser_Name { get; set; }
        public string AuthPass { get; set; }
        public int AuthAccount { get; set; }
        public bool EnableSms { get; set; }
        public bool EnableMail { get; set; }
        public string Path { get; set; }
        public string SignupPage { get; set; }
        public bool EnableInputBuilder { get; set; }
        public bool BlockCms { get; set; }
        public int SignupOption { get; set; }
        public string RecieptAddress { get; set; }
        public string RecieptEvent { get; set; }
        public string Design { get; set; }

        #endregion
    }



    [EntityMapping("AccountProperty")]
    public class AccountsView : EntityItem<DbPro>, IEntityPro
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

        //public static IEnumerable<T> ViewEntityItemList<T>(string GroupName, string TableName, int AccountId) where T : IEntityPro
        //{
        //    IEnumerable<T> list = null;

        //        list = db.EntityQuery<T>(TableName, "AccountId", AccountId);

        //    return list;
        //}

        public static void Refresh(int AccountId)
        {
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, AccountId, TableName));
        }

        public static string AccountsList()
        {
            using (var db = DbContext.Create<DbPro>())
            return db.QueryJson(TableName);
        }

        public static IEnumerable<AccountsView> ViewList()
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<AccountsView>(TableName);
        }

        public static AccountsView View(int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<AccountsView>(TableName, "AccountId", AccountId);
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
            using (var db = DbContext.Create<DbPro>())
            return db.EntityItemGet<T>(TableName, "AccountId", PropId);
        }

        public static int DoSave(AccountsView v, UpdateCommandType command)
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
                using (var db = DbContext.Create<DbPro>())
                result = db.ExecuteNonQuery("sp_Account_Property_Save", args);
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
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, v.PropId, TableName));
            return result;
        }

    }
  
    [EntityMapping("AccountProperty")]
    public class AccountsCreditView : EntityItem<DbPro>, IEntityPro
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

        public static AccountsCreditView View(int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.EntityItemGet<AccountsCreditView>(TableName, "AccountId", AccountId);
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
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<T>(TableName, "AccountId", PropId);
        }

        public static int DoSave(AccountsCreditView v)
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
            using (var db = DbContext.Create<DbPro>())
            {
                result = db.ExecuteNonQuery("sp_Account_Property_Credit_Save", args);

                WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, v.PropId, TableName));
                return result;
            }
        }

    }
}
