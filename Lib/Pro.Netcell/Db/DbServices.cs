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


    #region DbContext
    [DbContext("netcell_services")]
    [Serializable]
    public partial class DbServices : DbContext
    {
        #region static

        public const bool EnableCache = true;
        //private static readonly DbServices _instance = new DbServices();

        //public static DbServices Instance
        //{
        //    get { return _instance; }
        //}

        public static string Cnn
        {
            get { return NetConfig.ConnectionString("netcell_services"); }
        }

        public DbServices()
        {
        }

        #endregion

        #region override

        protected override void EntityBind()
        {
            //base.SetConnection("cnn_pro");//, Cnn, ProNetcellvider.SqlServer);
            //base.Items.SetEntity("Contact", "Person.Contact", EntitySourceType.Table, new EntityKeys("ContactID"));
            //base.SetEntity<ActiveContact>();
        }

        public override ILocalizer Localization
        {
            get
            {
                return base.GetLocalizer<NetcellLocalizer>();
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

    }
    #endregion

    public class AdminTask : IEntityItem
    {
        public const string Proc = "sp_Admin_Task_Add"; 

        public string CommandType { get; set; }
        public string CommandText { get; set; }
        public string Arguments { get; set; }
        public string DbName { get; set; }
        public DateTime ExecTime { get; set; }
        public int Expiration { get; set; }
        public string Sender { get; set; }
    }


}
