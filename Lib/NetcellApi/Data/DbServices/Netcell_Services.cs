using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Nistec.Data.Entities;
using Nistec.Generic;
using Nistec.Data;

namespace Netcell.Data.DbServices
{
   
    #region ResourceLang

    //[Serializable]
    //public class Netcell_ServicesResources : EntityLang
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
    //        base.Init("Netcell.Data.Resources.Netcell_Services");
    //        //or
    //        //base.Init(MControl.Sys.NetResourceManager.GetResourceManager("DataEntityDemo.Resources.AdventureWorks", this.GetType()));
    //        //or
    //        //base.Init(MControl.Sys.NetResourceManager.GetResourceManager( "DataEntityDemo.Resources.AdventureWorks",this.GetType()));
    //    }
    //    #endregion
    //}
    #endregion

    #region DbContext

    [DbContext("Netcell_Services", ConnectionKey = "cnn_Services", Provider = DBProvider.SqlServer)]
    [Serializable]
    public class Netcell_Services : DbContext
    {
        public static string Cnn
        {
            get { return NetConfig.AppSettings["cnn_Services"]; }
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
            get { return new Netcell_Services(); }
        }

        //public override IEntityLang LangManager
        //{
        //    get
        //    {
        //        return base.GetLangManager<Netcell_ServicesResources>();
        //    }
        //}

        
    }
    #endregion


}
