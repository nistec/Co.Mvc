using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Data;
using Nistec.Data;
using Nistec;
using Pro.Lib;
using System.Data;
using Nistec.Web.Controls;
using Pro.Data.Entities;

namespace Pro.Data.Registry
{

    [Entity(EntityName = "RegistryPage", MappingName = "Cms_Main", ConnectionKey = "cnn_pro", EntityKey = new string[] { "AccountId" })]
    public class CmsRegistryContext : EntityContext<RegistryPage>
    {

        #region AnalyticsCode

        public const string GoogleAnalyticsScriptTemplate="";
        public const string FacebookAnalyticsScriptTemplate="";

        #endregion

        #region ctor

        public CmsRegistryContext()
        {
        }
        public CmsRegistryContext(int AccountId)
            : base(AccountId)
        {
           // SetByParam("RecordId", RecordId);
        }

        #endregion

        #region update


        #endregion

        #region static

        public static string GetPath(int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.QueryScalar<string>("select Path from AccountProperty where AccountId=@AccountId","", "AccountId", AccountId);
        }

        public static RegistryPage GetRegistryPage(string Folder, string PageType)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                var rp = db.ExecuteSingle<RegistryPage>("sp_Cms_Page_Get", "Folder", Folder, "PageType", PageType);
                if (rp != null)
                    rp.Decode();
                return rp;
            }
        }
        public static RegistryHead GetRegistryHead(string Folder)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                var rp = db.ExecuteSingle<RegistryHead>("sp_Cms_Page_Get", "Folder", Folder, "PageType", "Head");
                if (rp != null)
                    rp.Decode();
                return rp;
            }
        }
        public static CreditPage GetRegistryPageCredit(string Folder, string PageType)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                var rp = db.ExecuteSingle<CreditPage>("sp_Cms_Page_Get", "Folder", Folder, "PageType", PageType);
                if (rp != null)
                    rp.Decode();
                return rp;
            }
        }

        public static IEnumerable<RegistryMessages> GetRegistryMessages(int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<RegistryMessages>("Cms_Messages", "AccountId", AccountId);
        }

        public static IEnumerable<PageHead> GetRegistryPagesList(int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.Query<PageHead>("select AccountId,PageType from Cms_Head where AccountId=@AccountId", "AccountId", AccountId);
        }
        //public static IEnumerable<RegistryMessages> GetRegistryMessages(string path)
        //{
        //    return db.ExecuteQuery<RegistryMessages>("sp_Registry_Get", "Path", path, "RegistryType", "Cms_Messages");
        //}

        public static string GetRegistryMessageText(int AccountId,string MessageId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.QueryScalar<string>("Select MessageText from Cms_Messages where AccountId=@AccountId and MessageId=@MessageId","", "AccountId", AccountId, "MessageId", MessageId);
        }
        public static string GetRegistryMessageText(int AccountId, int status)
        {
            switch (status)
            {
                case -2:
                    return CmsRegistryContext.GetRegistryMessageText(AccountId, "InvalidCampaign");
                case -1:
                    return CmsRegistryContext.GetRegistryMessageText(AccountId, "MemberStillValid");
                case 1:
                    return CmsRegistryContext.GetRegistryMessageText(AccountId, "SignupSuccess");
                case 0:
                    return CmsRegistryContext.GetRegistryMessageText(AccountId, "SignupFaild");
            }
            return CmsRegistryContext.GetRegistryMessageText(AccountId, "UnexpectedError");

        }

        public static string LoadRegistryMessageText(int AccountId, int status)
        {
            switch (status)
            {
                case -2:
                    return CmsRegistryContext.LoadRegistryMessageText(AccountId, "InvalidCampaign");
                case -1:
                    return CmsRegistryContext.LoadRegistryMessageText(AccountId, "MemberStillValid");
                case 1:
                    return CmsRegistryContext.LoadRegistryMessageText(AccountId, "SignupSuccess");
                case 0:
                    return CmsRegistryContext.LoadRegistryMessageText(AccountId, "SignupFaild");
            }
            return CmsRegistryContext.LoadRegistryMessageText(AccountId, "UnexpectedError");

        }
      
        #endregion

        #region cache

        public static void ClearCacheRegistry(int accountId, string entityName)
        {
            string path = GetPath(accountId);
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Registry, path,entityName));
        }

        public static void ClearCacheRegistry(string path)
        {
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Registry, path, "Cms_Main"));
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Registry, path, "Cms_Pages"));
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Registry, path, "Cms_Messages"));
        }

      

        public static int ClearCacheAllRegistry(int accountId)
        {
            string path = GetPath(accountId);
            //string key = WebCache.GetGroupKey(Settings.ProjectName, EntityGroups.Registry, path);
            var keys = WebCache.FindKeys(Settings.ProjectName, EntityGroups.Registry,accountId,0,null, path);
            //var keys = WebCache.FindKeys(key, true);
            WebCache.Remove(keys);
            return keys.Count;
        }
        public static int ClearCacheAll(int accountId)
        {
            string path = GetPath(accountId);
            List<string> keys = new List<string>();
            keys.Add(WebCache.GetGroupKey(Settings.ProjectName, EntityGroups.Registry, path));
            keys.Add(WebCache.GetGroupKey(Settings.ProjectName, EntityGroups.Enums, path));
            keys.Add(WebCache.GetGroupKey(Settings.ProjectName, EntityGroups.Reports, path));
            WebCache.Remove(keys);
            return keys.Count;
        }

    

        public static RegistryPage LoadRegistryPage(string folder,string pageType)
        {
            string key = WebCache.GetKey(Settings.ProjectName, EntityGroups.Registry, folder, pageType);

            return EntityPro.CacheGetOrCreate<RegistryPage>(key, () => GetRegistryPage(folder, pageType));
        }
        public static RegistryHead LoadRegistryHead(string folder)
        {
            string key = WebCache.GetKey(Settings.ProjectName, EntityGroups.Registry, folder, "Head");

            return EntityPro.CacheGetOrCreate<RegistryHead>(key, () => GetRegistryHead(folder));
        }
        public static CreditPage LoadRegistryPageCredit(string folder)
        {
            string key = WebCache.GetKey(Settings.ProjectName, EntityGroups.Registry, folder, "Credit");

            var cp= EntityPro.CacheGetOrCreate<CreditPage>(key, () => GetRegistryPageCredit(folder, "Credit"));
            if (cp == null)
                cp = new CreditPage();

            //cp.Price = price;
            //cp.SignupKey = signupKey;

            return cp;
        }
        public static string LoadRegistryMessageText(int AccountId,string MessageId)
        {
            string key = WebCache.GetKey(Settings.ProjectName, EntityGroups.Registry, AccountId, "Cms_Messages-" + MessageId);
            return EntityPro.CacheGetOrCreate<string>(key, () => GetRegistryMessageText(AccountId, MessageId));
        }

        //public static IEnumerable<PageHead> LoadRegistryPagesList(int accountId, bool enableCache)
        //{
        //    string key = WebCache.GetKey(Settings.ProjectName, EntityGroups.Registry, accountId, "Registry_Pages");
        //    return EntityPro.CacheGetOrCreate<IEnumerable<PageHead>>(key, () => GetRegistryPagesList(accountId));
        //}

        //public static IEnumerable<RegistryMessages> LoadRegistryMessages(sint AccountId,, bool enableCache)
        //{
        //    string key = WebCache.GetKey(Settings.ProjectName, EntityGroups.Registry, AccountId, "Cms_Messages");
        //    return EntityPro.CacheGetOrCreate<IEnumerable<RegistryMessages>>(key, () => GetRegistryMessages(AccountId));
        //}

        #endregion
    }

    public class CreditPage : RegistryPage
    {
        //[EntityProperty(EntityPropertyType.NA)]
        //public float Price { get; set; }
        //[EntityProperty(EntityPropertyType.NA)]
        //public string SignupKey { get; set; }
        //[EntityProperty(EntityPropertyType.NA)]
        //public int CanPay { get; set; }
        [EntityProperty(EntityPropertyType.NA)]
        public string IframeSrc { get; set; }

    }

    public class RegistryPage : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }
        public string PageType { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }
        public string Ph1 { get; set; }
        public string Ph2 { get; set; }
        public string Ph3 { get; set; }
        public string Title { get; set; }
        public string Folder { get; set; }
        //public string HAnalyticsGoogleCode { get; set; }
        //public string HAnalyticsFacebookCode { get; set; }
        public string HScript { get; set; }
        public string Script { get; set; }
        public int HeadConfig { get; set; }

        [EntityProperty(EntityPropertyType.View)]
        public string Design { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public bool EnableSignupCredit { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string CreditTerminal { get; set; }

        [EntityProperty(EntityPropertyType.View)]
        public string HiddenScript { get; set; }

        [EntityProperty(EntityPropertyType.NA)]
        public string Token { get; set; }
        [EntityProperty(EntityPropertyType.NA)]
        public string Message { get; set; }
        [EntityProperty(EntityPropertyType.NA)]
        public int Id { get; set; }

        [EntityProperty(EntityPropertyType.NA)]
        public object Ds1 { get; set; }
        [EntityProperty(EntityPropertyType.NA)]
        public object Ds2 { get; set; }

        //public string AnalyticsGoogleScript
        //{
        //    get { return CmsRegistryContext.GoogleAnalyticsScriptTemplate.Replace("#AnalyticsGoogleCode#", HAnalyticsGoogleCode); }
        //}
        //public string AnalyticsFacebookScript
        //{
        //    get { return CmsRegistryContext.FacebookAnalyticsScriptTemplate.Replace("#AnalyticsFacebookCode#", HAnalyticsFacebookCode); }
        //}
        public void Decode()
        {
            //HeadConfig 0=none,1=header,2=fopter,3=both
            if (HeadConfig == 2 || HeadConfig == 0)
                Header = "";
            else if (Header != null && Header != "")
                Header = System.Web.HttpUtility.HtmlDecode(Header);
            if (HeadConfig < 2)
                Footer = "";
            else if (Footer != null && Footer != "")
                Footer = System.Web.HttpUtility.HtmlDecode(Footer);


            if (Body != null && Body != "")
                Body = System.Web.HttpUtility.HtmlDecode(Body);
            if (Ph1 != null && Ph1 != "")
                Ph1 = System.Web.HttpUtility.HtmlDecode(Ph1);
            if (Ph2 != null && Ph2 != "")
                Ph2 = System.Web.HttpUtility.HtmlDecode(Ph2);
            if (Ph3 != null && Ph3 != "")
                Ph3 = System.Web.HttpUtility.HtmlDecode(Ph3);
            if (Script != null && Script != "")
                Script = System.Web.HttpUtility.HtmlDecode(Script);
            if (HScript != null && HScript != "")
                HScript = System.Web.HttpUtility.HtmlDecode(HScript);
            if (HiddenScript != null && HiddenScript != "")
                HiddenScript = System.Web.HttpUtility.HtmlDecode(HiddenScript);
        }

        [EntityProperty(EntityPropertyType.Key)]
        public string Args { get; set; }
    }

    public class RegistryHead : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        //public string Title { get; set; }
        //public string HGoogleCode { get; set; }
        //public string HFacebookCode { get; set; }
        public string Folder { get; set; }
        public string HScript { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string Design { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public bool EnableSignupCredit { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string CreditTerminal { get; set; }
        //public string AnalyticsGoogleScript
        //{
        //    get { return CmsRegistryContext.GoogleAnalyticsScriptTemplate.Replace("#AnalyticsGoogleCode#", HGoogleCode); }
        //}
        //public string AnalyticsFacebookScript
        //{
        //    get { return CmsRegistryContext.FacebookAnalyticsScriptTemplate.Replace("#AnalyticsFacebookCode#", HFacebookCode); }
        //}
        public void Decode()
        {
            if (Header != null && Header != "")
                Header = System.Web.HttpUtility.HtmlDecode(Header);
            if (Footer != null && Footer != "")
                Footer = System.Web.HttpUtility.HtmlDecode(Footer);
            if (HScript != null && HScript != "")
                HScript = System.Web.HttpUtility.HtmlDecode(HScript);
        }

        [EntityProperty(EntityPropertyType.Key)]
        public string Args { get; set; }
    }
    public class PageHead : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }
        public string PageType { get; set; }
    }

     public class RegistryMessages : IEntityItem
     {
         [EntityProperty(EntityPropertyType.Key)]
         public int AccountId { get; set; }
         public string MessageId { get; set; }
         public string MessageText { get; set; }
 
     }
   
}
