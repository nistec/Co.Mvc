using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using System.Globalization;
using Nistec.Data.Factory;
using Nistec.Generic;
using ProNetcell.Data;

namespace Netcell.Data.Db
{
   
    #region ResourceLang

    //[Serializable]
    //public class NetcellResources : EntityLang
    //{
    //    public static CultureInfo GetCulture()
    //    {
    //        return new CultureInfo( "he-IL");
    //    }
    //    #region override

    //    protected override string CurrentCulture()
    //    {
    //        return GetCulture().Name;
    //    }

    //    protected override void LangBind()
    //    {
    //        //init by config key
    //        //base.Init(CurrentCulture(), "DataEntityDemo.Resources.AdventureWorks");
    //        //or
    //        base.Init("Netcell.Data.Resources.NetcellDB");
    //        //or
    //        //base.Init(Nistec.Sys.NetResourceManager.GetResourceManager("DataEntityDemo.Resources.AdventureWorks", this.GetType()));
    //        //or
    //        //base.Init(Nistec.Sys.NetResourceManager.GetResourceManager( "DataEntityDemo.Resources.AdventureWorks",this.GetType()));
    //    }
    //    #endregion
    //}
    #endregion

    #region DbContext

    [DbContext("NetcellDB", ConnectionKey = "cnn_Netcell", Provider = DBProvider.SqlServer)]
    [Serializable]
    public class NetcellDB : DbContext
    {
        public static string Cnn
        {
            get { return NetConfig.AppSettings["cnn_Netcell"]; }
            //.ConnectionString("NetcellDB"); }
        }

        protected override void EntityBind()
        { 
            //base.SetConnection("AdventureWorks", Cnn, DBProvider.SqlServer);
            //base.SetEntity("Contact", "Person.Contact", new EntityKeys("ContactID"));
            //base.SetEntity<ActiveContact>();
        }

        public static IDbContext Instance
        {
            get { return new NetcellDB(); }
        }

        public override ILocalizer Localization
        {
            get
            {
                return base.GetLocalizer<NetcellLocalizer>();
            }
        }

        //public override IEntityLang LangManager
        //{
        //    get
        //    {
        //        return base.GetLangManager<NetcellResources>();
        //    }
        //}

        
    }
    #endregion


}
