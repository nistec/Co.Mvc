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

namespace ProSystem.Data
{

   

    
    [DbContext("cnn_proxy")]
    [Serializable]
    public partial class DbProxy : DbContext
    {
        #region static

        public const bool EnableCache = true;
        //private static readonly DbProxy _instance = new DbProxy();

        //public static DbProxy Instance
        //{
        //    get { return _instance; }
        //}

        public static string Cnn
        {
            get { return NetConfig.ConnectionString("cnn_proxy"); }
        }

        public DbProxy()
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
                return base.Localization;//.GetLocalizer<ProLocalizer>();
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
  

}
