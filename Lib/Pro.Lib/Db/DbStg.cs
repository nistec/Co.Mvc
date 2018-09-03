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
using Pro.Data.Entities;
using System.Data;
using Nistec.Web.Controls;
using Pro.Lib;

namespace Pro.Data
{

    #region ResourceLang

    [Serializable]
    public class StgLocalizer : EntityLocalizer
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
    [DbContext("netcell_Stg")]
    [Serializable]
    public partial class DbStg : DbContext
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
            get { return NetConfig.ConnectionString("netcell_Stg"); }
        }

        public DbStg()
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
                return base.GetLocalizer<StgLocalizer>();
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

        public static void DbRefresh(ListsTypes entity, int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                db.ExecuteNonQuery("sp_Entity_Refresh", "Entity", Lists.GetListsType(entity), "AccountId", AccountId);
            }
        }
      
    }
    #endregion

}
