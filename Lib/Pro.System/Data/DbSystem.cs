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
using ProSystem.Data.Entities;
using System.Data;
using Nistec.Web.Controls;

namespace ProSystem.Data
{

    #region ResourceLang

    [Serializable]
    public class ProSystemLocalizer : EntityLocalizer
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
            base.Init("ProSystem.Data.Resources.ProSystem");
            //or
            //base.Init(Nistec.Sys.NetResourceManager.GetResourceManager("Pro,Data.Resources.Pro_Crm", this.GetType()));
            //or
            //base.Init(Nistec.Sys.NetResourceManager.GetResourceManager( "Pro,Data.Resources.Pro_Crm",this.GetType()));
        }
        #endregion
    }
    #endregion

    [DbContext("netcell_system")]
    [Serializable]
    public partial class DbSystem : DbContext
    {
        #region static

        public const bool EnableCache = true;
        //private static readonly DbSystem _instance = new DbSystem();

        //public static DbSystem Instance
        //{
        //    get { return _instance; }
        //}

        public static string Cnn
        {
            get { return NetConfig.ConnectionString("netcell_system"); }
        }

        public DbSystem()
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
                return base.GetLocalizer<ProSystemLocalizer>();
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

        #region methods
        //public static int EntitySave<T>(T entity, UpdateCommandType commandType, object[] keyvalueParameters)
        //    where T : IEntityItem
        //{
        //    return DbContext.EntitySave<DbSystem, T>(entity, commandType, keyvalueParameters);
        //}

        public static int SysCounters(int CounterId, int Count = 1)
        {
            int Value = 0;
            using (var context = new DbSystem())
            {
                var result = context.ExecuteOutput("sp_Sys_Counters", "CounterId", CounterId, 0, "Count", Count, 0, "Value", Value, 2);
                if (result != null && result.AffectedRecords > 0)
                {
                    Value= result.GetValue<int>("Value");
                }
            }
            return Value;
        }
        public static int SysCounters(int CounterId, int Count, out int MinValue, out int MaxValue)
        {
            int Value = 0;
            using (var context = new DbSystem())
            {
                var result = context.ExecuteOutput("sp_Sys_Counters", "CounterId", CounterId, 0, "Count", Count, 0, "Value", Value, 2);
                if (result != null && result.AffectedRecords > 0)
                {
                    Value = result.GetValue<int>("Value");
                }
            }
            MinValue = Value - Count + 1;
            MaxValue = Value;
            return Value;
        }
        #endregion
    }


    public class DbSystemContext<T> : EntityContext<DbSystem, T> where T : IEntityItem
    {
        public static void Refresh(string entityCacheGroups, int userId)
        {
            DbContextCache.Remove<T>(Settings.ProjectName, entityCacheGroups, 0, userId);
        }
        public static DbSystemContext<T> Get(string entityCacheGroups, int accountId, int userId)
        {
            return new DbSystemContext<T>(entityCacheGroups, accountId,userId);
        }
        public DbSystemContext(string entityCacheGroups, int accountId, int userId)
        {
            CacheKey = DbContextCache.GetKey<T>(Settings.ProjectName, entityCacheGroups, accountId, userId);
        }
        public override IList<T> ExecOrViewList(params object[] keyValueParameters)
        {
            //int ttl = 3;
            return DbContextCache.EntityList<DbSystem, T>(CacheKey, keyValueParameters);
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

}
