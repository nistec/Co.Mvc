using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Nistec.Data.Entities;
using Nistec.Generic;
using Nistec.Data;

namespace Netcell.Data.Db
{
   
    #region ResourceLang

    //[Serializable]
    //public class NetcellTraceResources : EntityLang
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
    //        //base.Init(MControl.Sys.NetResourceManager.GetResourceManager("DataEntityDemo.Resources.AdventureWorks", this.GetType()));
    //        //or
    //        //base.Init(MControl.Sys.NetResourceManager.GetResourceManager( "DataEntityDemo.Resources.AdventureWorks",this.GetType()));
    //    }
    //    #endregion
    //}
    #endregion

    #region DbContext

    [DbContext("Netcell_Trace", ConnectionKey = "cnn_Trace", Provider = DBProvider.SqlServer)]
    [Serializable]
    public class Netcell_Trace : DbContext
    {
        public static string Cnn
        {
            get { return NetConfig.AppSettings["cnn_Trace"]; }
        }

        protected override void EntityBind()
        { 
            //base.SetConnection("AdventureWorks", Cnn, DBProvider.SqlServer);
            //base.SetEntity("Contact", "Person.Contact", new EntityKeys("ContactID"));
            //base.SetEntity<ActiveContact>();
        }

        public static IDbContext Instance
        {
            get { return new Netcell_Trace(); }
        }

        //public override IEntityLang LangManager
        //{
        //    get
        //    {
        //        return base.GetLangManager<NetcellTraceResources>();
        //    }
        //}

        
    }
    #endregion


}
