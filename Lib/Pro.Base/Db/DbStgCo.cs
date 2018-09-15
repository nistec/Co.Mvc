using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using System.Globalization;
using Nistec.Data.Factory;
using Nistec.Data.Entities.Cache;
using Nistec.Generic;
using System.Data;
using Nistec.Web.Controls;


namespace Pro.Data
{

    #region ResourceLang

    [Serializable]
    public class DbStgCoLocalizer : EntityLocalizer
    {
        public static CultureInfo GetCulture()
        {
            return new CultureInfo("he-IL");
        }
        #region override

        protected override string CurrentCulture()
        {
            return GetCulture().Name;
        }

        protected override void BindLocalizer()
        {
            //init by config key
            //base.Init(CurrentCulture(), "Pro.Data.Resources.Pro_Crm");
            //or
            base.Init("Pro.Data.Resources.Pro.Lib");
            //or
            //base.Init(Nistec.Sys.NetResourceManager.GetResourceManager("Pro,Data.Resources.Pro_Crm", this.GetType()));
            //or
            //base.Init(Nistec.Sys.NetResourceManager.GetResourceManager( "Pro,Data.Resources.Pro_Crm",this.GetType()));
        }
        #endregion
    }
    #endregion

    #region DbContext
    [DbContext("cnn_pro")]
    [Serializable]
    internal partial class DbStgCo : DbContext
    {
        #region static

        public const bool EnableCache = true;
        //private static readonly DbPro _instance = new DbPro();

        //public static DbPro Instance
        //{
        //    get { return _instance; }
        //}

        public static string Cnn
        {
            get { return NetConfig.ConnectionString("cnn_pro"); }
        }

        public DbStgCo()
        {
        }

        #endregion

        #region override

        protected override void EntityBind()
        {
            //base.SetConnection("cnn_pro");//, Cnn, DBProvider.SqlServer);
            //base.Items.SetEntity("Contact", "Person.Contact", EntitySourceType.Table, new EntityKeys("ContactID"));
            //base.SetEntity<ActiveContact>();
        }

        public override ILocalizer Localization
        {
            get
            {
                return base.GetLocalizer<DbStgCoLocalizer>();
            }
        }

       

        #endregion

        #region Entities

        static EntityDbCache cache;

        public EntityDbCache Cache
        {
            get
            {
                if (cache == null)
                {
                    cache = new EntityDbCache(this);
                }
                return cache;
            }
        }

        //public EntityDb Contact { get { return this.DbEntities.Get("Contact", "Person.Contact", EntitySourceType.Table, EntityKeys.Get("ContactID"), EnableCache); } }

        #endregion

        #region Cache

        //const string LibName = "Party";
        //public static string CacheKey(string group, int AccountId, string TableName)
        //{
        //    return WebCache.GetKey(LibName, group, AccountId, TableName);
        //}
        //public static string CacheGroupKey(string group, int AccountId)
        //{
        //    return WebCache.GetGroupKey(LibName, group, AccountId);
        //}
        //public static T CacheGet<T>(string key)
        //{
        //    return WebCache.Get<T>(key);
        //}
        public static T CacheGetOrCreate<T>(string key, Func<T> function)
        {
            return WebCache.GetOrCreate<T>(key, function);
        }
        //public static void CacheAdd(string key, object value)
        //{
        //    WebCache.Insert(key, value);
        //}
        //public static void CacheAdd(string key, object value, int expirationMimute)
        //{
        //    WebCache.Insert(key, value, expirationMimute);
        //}
        //public static void CacheDelete(string key)
        //{
        //    WebCache.Remove(key);
        //}

        #endregion

        //public static void DbRefresh(ListsTypes entity, int AccountId)
        //{
        //    using (var db = DbContext.Create<DbPro>())
        //    {
        //        db.ExecuteNonQuery("sp_Entity_Refresh", "Entity", Lists.GetListsType(entity), "AccountId", AccountId);
        //    }
        //}
      
    }
    #endregion

    /*
     public class DbProContext<T> : EntityContext<DbPro, T> where T : IEntityItem
     {
         #region static
         //public static void DbRefresh(ListsTypes entity, int AccountId)
         //{
         //    string mapping = EntityMappingAttribute.Name<T>();
         //    if (mapping != null)
         //    {
         //        using (var db = DbContext.Create<DbPro>())
         //        {
         //            db.ExecuteNonQuery("sp_Entity_Refresh", "Entity", mapping, "AccountId", AccountId);
         //        }
         //    }
         //}
         public static void Refresh(string entityCacheGroups, int AccountId)
        {
            DbContextCache.Remove<T>(Settings.ProjectName, entityCacheGroups, AccountId);
        }

        // public static IEnumerable<T> ViewEntityList<T>(string GroupName, int AccountId) where T : IEntityPro
        // {
        //     string key = DbContextCache.GetKey<T>(Settings.ProjectName, GroupName, AccountId, 0);
        //     return DbContextCache.EntityList<DbPro, T>(key, DataParameter.ToArgs("AccountId", AccountId));

        //     //IEnumerable<T> list = null;

        //     //if (EntityPro.EnableCache)
        //     //    list = (IEnumerable<T>)WebCache.Get<List<T>>(key);
        //     //if (list == null || list.Count() == 0)
        //     //{
        //     //    using (var db = DbContext.Create<DbPro>())
        //     //    {
        //     //        list = db.EntityList<T>("AccountId", AccountId);
        //     //    }
        //     //    if (EntityPro.EnableCache && list != null)
        //     //    {
        //     //        WebCache.Insert(key, (List<T>)list);
        //     //    }
        //     //}

        //     //return list;
        // }
        // public static IEnumerable<T> ViewEntityList<T>(string GroupName, int AccountId, int EnumType) where T : IEntityProEnum
        // {
        //     string key = DbContextCache.GetKey<T>(Settings.ProjectName, GroupName, AccountId, 0, EnumType.ToString());
        //     return DbContextCache.EntityList<DbPro, T>(key, DataParameter.ToArgs("AccountId", AccountId, "EnumType", EnumType));

        //     //string EntiryName = TableName + EnumType.ToString();
        //     //string key = WebCache.GetKey(Settings.ProjectName, GroupName, AccountId, EntiryName);
        //     //IEnumerable<T> list = null;

        //     //if (EntityPro.EnableCache)
        //     //    list = (IEnumerable<T>)WebCache.Get<List<T>>(key);
        //     //if (list == null || list.Count() == 0)
        //     //{
        //     //    using (var db = DbContext.Create<DbPro>())
        //     //    {
        //     //        list = db.EntityItemList<T>(TableName, "AccountId", AccountId, "EnumType", EnumType);
        //     //    }
        //     //    if (EntityPro.EnableCache && list != null)
        //     //    {
        //     //        //CacheAdd(key,GetSession(AccountId), (List<T>)list);
        //     //        WebCache.Insert(key, (List<T>)list);
        //     //    }
        //     //}

        //     //return list;
        // }
        #endregion

        //public DbProContext(string entityCacheGroups, int AccountId)
        //{
        //    CacheKey = DbContextCache.GetKey<T>(Settings.ProjectName, entityCacheGroups, 0, AccountId);
        //}
        //public override IList<T> ExecOrViewList(params object[] keyValueParameters)
        //{
        //    //int ttl = 3;
        //    return DbContextCache.EntityList<DbPro, T>(CacheKey, keyValueParameters);
        //}
        //protected override void OnChanged(ProcedureType commandType)
        //{
        //    DbContextCache.Remove(CacheKey);
        //}

        //public EntityResultModel GetFormResult(EntityCommandResult res, string reason)
        //{
        //   return EntityResultModel.GetFormResult(res.AffectedRecords, this.EntityName, reason, res.GetIdentityValue<int>());
        //}

    }
    */
}
