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
using ProNetcell.Data.Entities;
using System.Data;

namespace ProNetcell.Data
{


    [DbContext("netcell_logs")]
    [Serializable]
    public class DbLogs : DbContext
    {
        #region static

        public const bool EnableCache = true;
 
        public static string Cnn
        {
            get { return NetConfig.ConnectionString("netcell_logs"); }
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
                return base.GetLocalizer<ProLocalizer>();
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

       //  public static IList<Dictionary<string,object>> ViewLog(int PageNum, string Action=null,string Folder=null, DateTime? DateFrom=null, DateTime? DateTo=null)
       // {
       //     int PageSize=20;
       //     using (var db = DbContext.Create<DbLogs>())
       //     {
       //         return db.ExecuteDictionary("sp_LogReader", "QueryType", "co", "PageSize", PageSize, "PageNum", PageNum, "Action", Action, "Folder", Folder, "DateFrom", DateFrom, "DateTo", DateTo);
       //     }
       //}

        //public static IList<Dictionary<string, object>> ViewLog(string QueryType, int PageNum, string Action = null, string Folder = null, DateTime? DateFrom = null, DateTime? DateTo = null)
        //{
        //    //QueryType-- co, sys,auth
        //    int PageSize = 20;
        //    using (var db = DbContext.Create<DbLogs>())
        //    {
        //        return db.ExecuteDictionary("sp_LogReader", "QueryType", QueryType, "PageSize", PageSize, "PageNum", PageNum, "Action", Action, "Folder", Folder, "DateFrom", DateFrom, "DateTo", DateTo);
        //    }
        //}
        public static IList<Dictionary<string, object>> ViewLog(string QueryType, int PageNum, string Action = null, string Folder = null, DateTime? DateFrom = null, DateTime? DateTo = null)
        {
            //QueryType-- co, sys,auth
            int PageSize = 20;
            using (var db = DbContext.Create<DbLogs>())
            {
                return db.ExecuteDictionary("sp_Log_View", "QueryType", QueryType, "PageSize", PageSize, "PageNum", PageNum, "Action", Action, "Folder", Folder, "DateFrom", DateFrom, "DateTo", DateTo);
            }
        }

        

    }
    
}
