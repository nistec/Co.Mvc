using System;
using System.Data;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using Nistec.Data;
using Nistec.Data.SqlClient;
using System.Data.SqlClient;
using System.Collections.Generic;
using Nistec.Data.Factory;

namespace Netcell.Data.Server
{
    /// <summary>
    /// Dal common.
    /// </summary>
    public sealed class Dal : Nistec.Data.Factory.AutoBase//.DalBase
    {
         public static readonly Dal DB = new Dal();

        //private static readonly Dal DB = new Dal();
        //public static List<CONNECTION> _Connections;

       
        private Dal()
        {
            Init( Nistec.Data.DBProvider.SqlServer,DBRule.ConnectionString, true);
        }

        public static string ConnectionString
        {
            get
            {
                //if (_ConnectionString == null)
                //{
                //    _ConnectionString = DBconfig.ConnectionString;
                //}
                return DBRule.ConnectionString;// _ConnectionString;
            }
        }
        public static string ConnectionStringApp
        {
            get
            {
                //if (_ConnectionStringApp == null)
                //{
                //    _ConnectionStringApp = DBconfig.ConnectionStringApp;
                //}
                return DBRule.CnnApp; //_ConnectionStringApp;
            }
        }
  
        //public static Dal DB
        //{
        //    get
        //    {
        //        //if (_DB == null)// || _ConnectionString == null)
        //        //{
        //        //    _DB.Init(DBconfig.ConnectionString, true);
        //        //}
        //        return _DB;
        //    }
        //}

        public static IAutoBase IDal
        {
            get { return DB as IAutoBase; }
        }

        public DalConfig Config { get { return (DalConfig.Instance); } }
        //public DalCmdCB CB { get { return (DalCmdCB.Instance); } }
        //public DalRB RB { get { return (DalRB)GetDalDB(); } }


        
        //public DalOperators Operators { get { return (DalOperators)GetDalDB(); } }
        //public DalGeneric Generic { get { return (DalGeneric)GetDalDB(); } }
        //public DalOperatorsDef OpDef { get { return (DalOperatorsDef)GetDalDB(); } }
        //public DalPermissions Permissions { get { return (DalPermissions)GetDalDB(); } }
        //public DalRules Rules { get { return (DalRules)GetDalDB(); } }


    
    }

}
