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

   

    
    [DbContext("cnn_proxy")]
    [Serializable]
    public partial class ProNetcellxy : DbContext
    {
        #region static

        public const bool EnableCache = true;
        //private static readonly ProNetcellxy _instance = new ProNetcellxy();

        //public static ProNetcellxy Instance
        //{
        //    get { return _instance; }
        //}

        public static string Cnn
        {
            get { return NetConfig.ConnectionString("cnn_proxy"); }
        }

        public ProNetcellxy()
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
  

}
