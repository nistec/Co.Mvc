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
