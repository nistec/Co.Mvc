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
using System.Web;

namespace Pro.Data.Registry
{

   // [Entity(EntityName = "RegistryMain", MappingName = "Registry_Main", ConnectionKey = "cnn_pro", EntityKey = new string[] { "AccountId" })]
    public class CmsContext : EntityContext<CmsRegistryPage>
    {

        #region ctor

        public CmsContext()
        {
        }
        public CmsContext(int AccountId)
            : base(AccountId)
        {
           // SetByParam("RecordId", RecordId);
        }

        #endregion

        #region update

        //public static int DoSave(int AccountId, string PageType, string Body, string Ph1, string Ph2, string Ph3, string Title, string Script)
        //{
        //    var args = new object[]{
        //        "AccountId", AccountId
        //        ,"PageType", PageType
        //        ,"Body", Body
        //        ,"Ph1", Ph1
        //        ,"Ph2", Ph2
        //        ,"Ph3", Ph3
        //        ,"Title", Title
        //        ,"Script", Script
        //    };
        //    var parameters = DataParameter.GetSql(args);
        //    int res = db.ExecuteNonQuery("sp_Registry_Content_Update", parameters, System.Data.CommandType.StoredProcedure);
        //    return res;
        //}

 
        public static int DoSave_Page_Settings(CmsPageSettings cs)//int AccountId, string PageType, int HeadConfig, string Title, string FacebookCode, string GoogleAnalyticsCode, string GoogleAdsCode, string GoogleAdsLabel)
        {
            var args = new object[]{
                "AccountId", cs.AccountId
                ,"PageType", cs.PageType
                ,"HeadConfig", cs.HeadConfig
                ,"Title", cs.Title
                ,"FacebookCode", cs.FacebookCode
                ,"GoogleAnalyticsCode", cs.GoogleAnalyticsCode
                ,"GoogleAdsCode", cs.GoogleAdsCode
                ,"GoogleAdsLabel", cs.GoogleAdsLabel
                ,"FacebookCompletedCode", cs.FacebookCompletedCode
            };
            var parameters = DataParameter.GetSql(args);
            using (var db = DbContext.Create<DbPro>())
            {
                int res = db.ExecuteCommandNonQuery("sp_Cms_Page_Settings_Update_v1", parameters, System.Data.CommandType.StoredProcedure);
                return res;
            }
        }

        public static int DoSave(int AccountId, string PageType, string Section, string Content)
        {
            var args = new object[]{
                "AccountId", AccountId
                ,"PageType", PageType
                ,"Section", Section
                ,"Content", Content
            };
            var parameters = DataParameter.GetSql(args);
            using (var db = DbContext.Create<DbPro>())
            {
                int res = db.ExecuteCommandNonQuery("sp_Cms_Content_Update", parameters, System.Data.CommandType.StoredProcedure);
                return res;
            }
        }

        public static int DoSave(CmsContent v)
        {
            var args = new object[]{
                "AccountId", v.AccountId
                ,"PageType", v.PageType
                ,"Section", v.Section
                ,"Content", v.Content
            };
            var parameters = DataParameter.GetSql(args);
            using (var db = DbContext.Create<DbPro>())
            {
                int res = db.ExecuteCommandNonQuery("sp_Cms_Content_Update", parameters, System.Data.CommandType.StoredProcedure);
                return res;
            }
        }


        public static int DoSaveInputFields(int AccountId, IEnumerable<RegistryInputField> v)
        {

            InputFieldsCreator builder = new InputFieldsCreator(AccountId);
            builder.CreateContent(v);

            var args = new object[]{
                "AccountId", AccountId
                ,"FieldsList", builder.FieldList
                ,"Content", builder.FieldContent
                ,"Script", builder.FieldScript
            };
            var parameters = DataParameter.GetSql(args);
            using (var db = DbContext.Create<DbPro>())
            {
                int res = db.ExecuteCommandNonQuery("sp_Registry_Fields_Update", parameters, System.Data.CommandType.StoredProcedure);
                return res;
            }
        }

        #endregion

        #region static

        public static CmsPageSettings GetPageSettings(int AccountId, string PageType)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                var cs = db.ExecuteSingle<CmsPageSettings>("sp_Cms_Settings_Get", "AccountId", AccountId, "PageType", PageType);
                if (cs == null)
                    return null;
                if (cs.AccountId == 0)
                {
                    cs.AccountId = AccountId;
                    cs.PageType = PageType;
                }
                return cs;
            }
        }
        
        public static CmsRegistryPage GetRegistryPage(int accountId, string PageType)
        {
            string Folder = CmsRegistryContext.GetPath(accountId);
            return GetRegistryPage(Folder, PageType);
        }
        public static CmsRegistryHead GetRegistryHead(int accountId)
        {
            string Folder = CmsRegistryContext.GetPath(accountId);
            return GetRegistryHead(Folder);
        }
        public static CmsRegistryPage GetRegistryPage(string Folder, string PageType)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.ExecuteSingle<CmsRegistryPage>("sp_Cms_Page_Get", "Folder", Folder, "PageType", PageType, "IsCms", true);
        }
        public static CmsRegistryHead GetRegistryHead(string Folder)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.ExecuteSingle<CmsRegistryHead>("sp_Cms_Page_Get", "Folder", Folder, "PageType", "Head");
        }

        public static CmsContent GetContent(string ExtId)
        {
            if (string.IsNullOrEmpty(ExtId))
                return null;
            string[] ext = ExtId.Split('-');

            var args = new object[]{
                "AccountId", Types.ToInt(ext[0])
                ,"PageType", ext[1]
                ,"Section", ext[2]
            };
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteSingle<CmsContent>("sp_Cms_Content_Get", args);
            }
        }
        public static CmsContent GetContent(int AccountId, string PageType, string Section)
        {
            var args = new object[]{
                "AccountId", AccountId
                ,"PageType", PageType
                ,"Section", Section
            };
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteSingle<CmsContent>("sp_Cms_Content_Get", args);
            }
        }

        //public static CmsRegistryPage GetRegistryPage(int AccountId, string PageType)
        //{
        //    return db.EntityItemGet<CmsRegistryPage>("Cms_Pages", "PageAccountId", AccountId, "PageType", PageType);
        //}

        public static IEnumerable<CmsContentSectionInfo> GetContentList(int AccountId, string PageType)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.ExecuteList<CmsContentSectionInfo>("sp_Cms_Content_List", "AccountId", AccountId, "PageType", PageType);
        }
        //public static IEnumerable<CmsPageItem> GetContentList(int AccountId)
        //{
        //    return db.Query<CmsPageItem>("select AccountId,PageType from Cms_Pages where AccountId=@AccountId", "AccountId", AccountId);
        //}


        public static IEnumerable<RegistryInputField> GetInputFieldsList(int AccountId)
        {
            var args = new object[]{
                "AccountId", AccountId
                ,"PageType", "Registry_Fields"
            };
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteList<RegistryInputField>("sp_Cms_Content_List", args);
            }
        }

        public static IEnumerable<RegistryInputFieldTemplate> GetInputFieldsTemplate(int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<RegistryInputFieldTemplate>("Registry_Fields_Template", "AccountId", AccountId);
        }
        #endregion

        #region cache

        public static void ClearCacheAllRegistry()
        {
            //string key = WebCache.GetGroupKey(Settings.ProjectName, EntityGroups.Registry);
            //var keys = WebCache.FindKeys(key, true);
            var keys = WebCache.FindKeys(Settings.ProjectName, EntityGroups.Registry);

            WebCache.Remove(keys);
        }
        public static int ClearCacheAll()
        {
            //string key = WebCache.GetGroupKey(Settings.ProjectName);
            //var keys = WebCache.FindKeys(key, true);
            var keys = WebCache.FindKeys(Settings.ProjectName, null);
            WebCache.Remove(keys);
            return keys.Count;
        }

        #endregion

    }

    public class CmsPageSettings : IEntityItem
    {
        // [EntityProperty(EntityPropertyType.Key, Column = "PageAccountId")]
        public int AccountId { get; set; }
        public string PageType { get; set; }
        public string GoogleAnalyticsCode { get; set; }
        public string GoogleAdsCode { get; set; }
        public string GoogleAdsLabel { get; set; }
        public string FacebookCode { get; set; }
        public string FacebookCompletedCode { get; set; }
        public int HeadConfig { get; set; }
        public string Title { get; set; }
        public CmsPageSettings()
        {
        }
        public CmsPageSettings(HttpRequestBase Request)
        {
            AccountId = Types.ToInt(Request["AccountId"]);
            PageType = Request["PageType"];
            HeadConfig = Types.ToInt(Request["HeadConfig"]); ;
            Title = Request["Title"];
            GoogleAnalyticsCode = Request["GoogleAnalyticsCode"];
            GoogleAdsCode = Request["GoogleAdsCode"];
            GoogleAdsLabel = Request["GoogleAdsLabel"];
            FacebookCode = Request["FacebookCode"];
            FacebookCompletedCode = Request["FacebookCompletedCode"];
        }
    }
     public class CmsRegistryPage : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Key,Column="PageAccountId")]
        public int AccountId { get; set; }
        public string PageType { get; set; }
        public string Body { get; set; }
        public string Ph1 { get; set; }
        public string Ph2 { get; set; }
        public string Ph3 { get; set; }
        public string Title { get; set; }
        public string Script { get; set; }
        public DateTime Modified { get; set; }
        public int HeadConfig { get; set; }
        //public string HGoogleCode { get; set; }
        //public string HFacebookCode { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string HiddenScript { get; set; }
    }

    public class CmsRegistryHead : IEntityItem
    {
        public string Folder { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public int DesignType { get; set; }
        

        //public string Title { get; set; }
        //public string HGoogleCode { get; set; }
        //public string HFacebookCode { get; set; }
        public string HScript { get; set; }
        public DateTime HModified { get; set; }

    }

    public class CmsPageItem : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }
        public string PageType { get; set; }
    }

    public class CmsRegistryItem : IEntityItem
    {
        public static CmsRegistryItem Get(int accountId)
        {
            return new CmsRegistryItem()
            {
                AccountId = accountId,
                Folder = CmsRegistryContext.GetPath(accountId)
            };
        }

        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }
        public string Folder { get; set; }
    }

    public class CmsContent : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public string PageType { get; set; }
        public string Content { get; set; }
        public string Section { get; set; }

    }
    public class CmsContentSectionInfo : IEntityItem
    {
        //public int AccountId { get; set; }
        public string ContentType { get; set; }
        public string PageType { get; set; }
        public string Section { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public string ExtId { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }

     

      public class RegistryInputField : IEntityItem
     {
         [EntityProperty(EntityPropertyType.Key)]
         public int AccountId { get; set; }
         [EntityProperty(EntityPropertyType.Key)]
         public string Field { get; set; }
         public string FieldName { get; set; }
         public int FieldOrder { get; set; }
         public string FieldType { get; set; }
         public int FieldLength { get; set; }
         public string InputType { get; set; }
         public bool Enable { get; set; }
         public bool Mandatory { get; set; }
         public string ValidateType { get; set; }
         public int MinLength { get; set; }
         public string ErrorRequirerMessage { get; set; }
         public string ErrorTypeMessage { get; set; }
     }

      public class RegistryInputFieldTemplate : IEntityItem
      {
          [EntityProperty(EntityPropertyType.Key)]
          public int AccountId { get; set; }
          [EntityProperty(EntityPropertyType.Key)]
          public string InputFieldType { get; set; }
          public string FieldContent { get; set; }

      }


}
