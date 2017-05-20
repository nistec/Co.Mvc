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

namespace Pro.Data.Registry
{

    [Entity(EntityName = "RegistryMain", MappingName = "Party_Registry_Main", ConnectionKey = "cnn_pro", EntityKey = new string[] { "AccountId" })]
    public class RegistryContext : EntityContext<RegistryMain>
    {

        #region AnalyticsCode

        public const string AnalyticsScriptTemplate="";

        #endregion

        #region ctor

        public RegistryContext()
        {
        }
        public RegistryContext(int AccountId)
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
            return DbPro.Instance.QueryScalar<string>("select Path from Party_AccountProperty where AccountId=@AccountId","", "AccountId", AccountId);
        }

        public static RegistryMain GetRegistryMain(int AccountId)
        {
            return DbPro.Instance.QueryEntity<RegistryMain>("Party_Registry_Main", "AccountId", AccountId);
        }

        public static RegistryPages GetRegistryPages(int AccountId)
        {
            return DbPro.Instance.QueryEntity<RegistryPages>("Party_Registry_Pages", "AccountId", AccountId);
        }

        public static IEnumerable<RegistryMessages> GetRegistryMessages(int AccountId)
        {
            return DbPro.Instance.QueryEntityList<RegistryMessages>("Party_Registry_Messages", "AccountId", AccountId);
        }

        public static RegistryMain GetRegistryMain(string  path)
        {
            return DbPro.Instance.ExecuteSingle<RegistryMain>("sp_Party_Registry_Get", "Path", path,"RegistryType", "Party_Registry_Main");
        }

        public static RegistryPages GetRegistryPages(string  path)
        {
            return DbPro.Instance.ExecuteSingle<RegistryPages>("sp_Party_Registry_Get", "Path", path,"RegistryType", "Party_Registry_Pages");
        }
        public static RegistryPageSection GetRegistryPageContent(string path, string section)
        {
            return DbPro.Instance.ExecuteSingle<RegistryPageSection>("sp_Party_Registry_Get", "Path", path, "RegistryType", "Party_Registry_Pages", "Section", section);
        }

      
        public static IEnumerable<RegistryPages> GetRegistryPagesList(string path)
        {
            return DbPro.Instance.ExecuteQuery<RegistryPages>("sp_Party_Registry_Get", "Path", path, "RegistryType", "Party_Registry_Pages");
        }
        public static IEnumerable<RegistryMessages> GetRegistryMessages(string  path)
        {
            return DbPro.Instance.ExecuteQuery<RegistryMessages>("sp_Party_Registry_Get", "Path", path,"RegistryType", "Party_Registry_Messages");
        }

        public static string GetRegistryMessageText(int AccountId,string MessageId)
        {
            return DbPro.Instance.QueryScalar<string>("Select MessageText from Party_Registry_Messages where AccountId=@AccountId and MessageId=@MessageId","", "AccountId", AccountId, "MessageId", MessageId);
        }
        public static string GetRegistryMessageText(int AccountId, int status)
        {
            switch (status)
            {
                case -2:
                    return RegistryContext.GetRegistryMessageText(AccountId, "InvalidCampaign");
                case -1:
                    return RegistryContext.GetRegistryMessageText(AccountId, "MemberStillValid");
                case 1:
                    return RegistryContext.GetRegistryMessageText(AccountId, "SignupSuccess");
                case 0:
                    return RegistryContext.GetRegistryMessageText(AccountId, "SignupFaild");
            }
            return RegistryContext.GetRegistryMessageText(AccountId, "UnexpectedError");

        }
                 

      
        #endregion

        #region cache

        public static void ClearCacheRegistry(int accountId, string entityName)
        {
            string path = GetPath(accountId);
            WebCache.Remove(WebCache.GetKey("Party", "Registry", path, entityName));
        }

        public static void ClearCacheRegistry(string path)
        {
            WebCache.Remove(WebCache.GetKey("Party", "Registry", path, "Party_Registry_Main"));
            WebCache.Remove(WebCache.GetKey("Party", "Registry", path, "Party_Registry_Pages"));
            WebCache.Remove(WebCache.GetKey("Party", "Registry", path, "Party_Registry_Messages"));
        }

        public static void ClearCacheAllRegistry()
        {
            string key= WebCache.GetGroupKey("Party", "Registry");

            var keys= WebCache.FindKeys(key, true);

            WebCache.Remove(keys);
        }

       
        public static RegistryMain GetRegistryMain(string path, bool enableCache)
        {
            if (enableCache)
            {
                string key = WebCache.GetKey("Party", "Registry", path, "Party_Registry_Main");
                var entity = WebCache.Get<RegistryMain>(key);

                if (entity == null)// || list.Count()==0)
                {
                    entity = GetRegistryMain(path);
                    if (entity != null)
                    {
                        //CacheAdd(key,GetSession(AccountId), (List<T>)list);
                        WebCache.Insert(key, entity);
                    }
                }
                return entity;
            }
            return GetRegistryMain(path);
        }

        public static RegistryPages GetRegistryPages(string path, bool enableCache)
        {
            if (enableCache)
            {
                string key = WebCache.GetKey("Party", "Registry", path, "Party_Registry_Pages");
                var entity = WebCache.Get<RegistryPages>(key);

                if (entity == null)// || list.Count()==0)
                {
                    entity = GetRegistryPages(path);
                    if (entity != null)
                    {
                        //CacheAdd(key,GetSession(AccountId), (List<T>)list);
                        WebCache.Insert(key, entity);
                    }
                }
                return entity;
            }
            return GetRegistryPages(path);
        }

        public static IEnumerable<RegistryPages> GetRegistryPagesList(string path, bool enableCache)
        {
            if (enableCache)
            {
                string key = WebCache.GetKey("Party", "Registry", path, "Party_Registry_Pages");
                var entity = WebCache.Get<IEnumerable<RegistryPages>>(key);

                if (entity == null || entity.Count() == 0)
                {
                    entity = GetRegistryPagesList(path);
                    if (entity != null)
                    {
                        //CacheAdd(key,GetSession(AccountId), (List<T>)list);
                        WebCache.Insert(key, entity);
                    }
                }
                return entity;
            }
            return GetRegistryPagesList(path);
        }

        public static RegistryPageSection GetRegistryPageContent(string path, string Section, bool enableCache)
        {
            if (enableCache)
            {
                string key = WebCache.GetKey("Party", "Registry", path, "Party_Registry_Pages" + "-" + Section);
                var entity = WebCache.Get<RegistryPageSection>(key);

                if (entity == null)// || list.Count()==0)
                {
                    entity = GetRegistryPageContent(path, Section);
                    if (entity != null)
                    {
                        //CacheAdd(key,GetSession(AccountId), (List<T>)list);
                        WebCache.Insert(key, entity);
                    }
                }
                return entity;
            }
            return GetRegistryPageContent(path, Section);
        }
/*
        public static RegistryPageContent GetRegistryPageAbout(string path, bool enableCache, string page)
        {
            var entity = GetRegistryPagesList(path, enableCache);
            if (entity == null)
                return new RegistryPageContent();
            var content = from a in entity
                select new RegistryPageContent()
                {
                    Content = a.About,
                    Title = a.Title,
                    AccountId = a.AccountId
                };
            return content.FirstOrDefault();
        }
        public static RegistryPageContent GetRegistryPageAbout(string path, bool enableCache)
        {
            var entity = GetRegistryPagesList(path, enableCache);
            if (entity == null)
                return new RegistryPageContent();
            var content = from a in entity
                          select new RegistryPageContent()
                          {
                              Content = a.About,
                              Title = a.Title,
                              AccountId = a.AccountId
                          };
            return content.FirstOrDefault();
        }
        public static RegistryPageContent GetRegistryPageContact(string path, bool enableCache)
        {
            var entity = GetRegistryPagesList(path, enableCache);
            if (entity == null)
                return new RegistryPageContent();
            var content = from a in entity
                          select new RegistryPageContent()
                          {
                              Content = a.Contact,
                              Title = a.Title,
                              AccountId = a.AccountId
                          };
            return content.FirstOrDefault();
        }

        public static RegistryPageContent GetRegistryPageStatement(string path, bool enableCache)
        {
            var entity = GetRegistryPagesList(path, enableCache);
            if (entity == null)
                return new RegistryPageContent();
            var content = from a in entity
                          select new RegistryPageContent()
                          {
                              Content = a.Statement,
                              Title = a.Title,
                              AccountId = a.AccountId
                          };
            return content.FirstOrDefault();
        }
        public static RegistryPageContent GetRegistryPageThanks(string path, bool enableCache)
        {
            var entity = GetRegistryPagesList(path, enableCache);
            if (entity == null)
                return new RegistryPageContent();
            var content = from a in entity
                          select new RegistryPageContent()
                          {
                              Content = a.Thanks,
                              Title = a.Title,
                              AccountId = a.AccountId
                          };
            return content.FirstOrDefault();
        }
*/
        public static IEnumerable<RegistryMessages> GetRegistryMessages(string path, bool enableCache)
        {
            if (enableCache)
            {
                string key = WebCache.GetKey("Party", "Registry", path, "Party_Registry_Messages");
                var entity = WebCache.Get<IEnumerable<RegistryMessages>>(key);

                if (entity == null || entity.Count() == 0)
                {
                    entity = GetRegistryMessages(path);
                    if (entity != null)
                    {
                        //CacheAdd(key,GetSession(AccountId), (List<T>)list);
                        WebCache.Insert(key, entity);
                    }
                }
                return entity;
            }
            return GetRegistryMessages(path);
        }

        #endregion
    }

    public class RegistryMain : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }
        public string InputFields { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }
        public string PlaceHolder1 { get; set; }
        public string PlaceHolder2 { get; set; }
        public string PlaceHolder3 { get; set; }
        public string Credit { get; set; }
        public string Title { get; set; }
        public string AnalyticsCode { get; set; }

        public string AnalyticsScript
        {
            get { return RegistryContext.AnalyticsScriptTemplate.Replace("#AnalyticsCode#", AnalyticsCode); }
        }
        public void Decode()
        {
            if (InputFields != null && InputFields != "")
                InputFields = System.Web.HttpUtility.HtmlDecode(InputFields);
            if (Header != null && Header != "")
                Header = System.Web.HttpUtility.HtmlDecode(Header);
            if (Body != null && Body != "")
                Body = System.Web.HttpUtility.HtmlDecode(Body);
            if (Footer != null && Footer != "")
                Footer = System.Web.HttpUtility.HtmlDecode(Footer);
            if (PlaceHolder1 != null && PlaceHolder1 != "")
                PlaceHolder1 = System.Web.HttpUtility.HtmlDecode(PlaceHolder1);
            if (PlaceHolder2 != null && PlaceHolder2 != "")
                PlaceHolder2 = System.Web.HttpUtility.HtmlDecode(PlaceHolder2);
            if (PlaceHolder3 != null && PlaceHolder3 != "")
                PlaceHolder3 = System.Web.HttpUtility.HtmlDecode(PlaceHolder3);
        }
    }

     public class RegistryPages : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }
        public string About { get; set; }
        public string Contact { get; set; }
        public string Statement { get; set; }
        public string Thanks { get; set; }
        //public string Title { get; set; }
  
    }

     public class RegistryPageSection : RegistryPageContent
     {
         //[EntityProperty(EntityPropertyType.Key)]
         //public int AccountId { get; set; }
         //public string Content { get; set; }
         //public string Title { get; set; }
         public string Header { get; set; }
         public string Footer { get; set; }
         public string AnalyticsCode { get; set; }
         //public string PageType { get; set; }
         //public string Section { get; set; }

         public string AnalyticsScript
         {
             get { return Section == "Index" ? RegistryContext.AnalyticsScriptTemplate.Replace("#AnalyticsCode#", AnalyticsCode) : ""; }
         }

         public void Decode()
         {
             if (Header != null && Header != "")
                 Header = System.Web.HttpUtility.HtmlDecode(Header);
             if (Content != null && Content != "")
                 Content = System.Web.HttpUtility.HtmlDecode(Content);
             if (Footer != null && Footer != "")
                 Footer = System.Web.HttpUtility.HtmlDecode(Footer);
         }

     }

     public class RegistryPageContent : IEntityItem
     {
         [EntityProperty(EntityPropertyType.Key)]
         public int AccountId { get; set; }
         public string Content { get; set; }
         public string Title { get; set; }
         public string PageType { get; set; }
         public string Section { get; set; }

     }
 
     public class RegistryMessages : IEntityItem
     {
         [EntityProperty(EntityPropertyType.Key)]
         public int AccountId { get; set; }
         public string MessageId { get; set; }
         public string MessageText { get; set; }
 
     }

    

   
}
