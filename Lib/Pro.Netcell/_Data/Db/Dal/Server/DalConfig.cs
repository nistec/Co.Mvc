using System;
using System.Data;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using Nistec.Data;
using Nistec.Data.SqlClient;
using System.Data.SqlClient;
using Nistec.Data.Factory;

namespace Netcell.Data.Server
{

 
    public class DalConfig : Nistec.Data.SqlClient.DbCommand
    {
        public DalConfig()
            : base(DBRule.ConnectionString)
        {

        }

        public static DalConfig Instance
        {
            get { return new DalConfig(); }
        }


        [DBCommand("SELECT * from [App_Config]")]
        public DataTable AppConfig()
        {
            return (DataTable)base.Execute();
        }

       
        [DBCommand("SELECT * from [Operators_Definition] WHERE (Server=0 or Server=@Server)")]
        public DataTable Operators_Definition(int Server)
        {
            return (DataTable)base.Execute(Server);
        }


        [DBCommand("SELECT * from [Operators_Connection]")]
        public DataTable Operators_Connection()
        {
            return (DataTable)base.Execute();
        }

        [DBCommand("SELECT * from [vw_Operators_Connection] where Enabled=1")]
        public DataTable Operators_ConnectionView()
        {
            return (DataTable)base.Execute();
        }

        [DBCommand("SELECT * FROM [vw_Carrier_Rules_CB]")]
        public DataTable Carrier_Rules_CB() { return (DataTable)base.Execute(); }

       
        [DBCommand("SELECT * from [Operators_BillingCode]")]
        public DataTable Operators_BillingCode()
        {
            return (DataTable)base.Execute();
        }

     
        [DBCommand("SELECT * from [vw_ActiveUsers]")]
        public DataTable ActiveUsers()
        {
            return (DataTable)base.Execute();
        }

        
        [DBCommand("SELECT * from [Trans_Rules]")]
        public DataTable Trans_Rules()
        {
            return (DataTable)base.Execute();
        }

        [DBCommand("SELECT * from [Trans_Blocked]")]
        public DataTable Trans_Blocked()
        {
            return (DataTable)base.Execute();
        }

        [DBCommand("SELECT * from [Operators_Mnp]")]
        public DataTable Operators_Mnp()
        {
            return (DataTable)base.Execute();
        }
        //[DBCommand("SELECT * from Mail_Channels]")]
        //public DataTable Mail_Channels()
        //{
        //    return (DataTable)base.Execute();
        //}

        public int GetLogIn(string LogInName, string Pass) 
        {
           return (int) base.LookupQuery<int>("AccountId", "Users",
               "IsBlocked=0 and LogInName=@LogInName and Pass=@Pass", -1,
               new object[] { LogInName, Pass });
        }

        [DBCommand("SELECT * from [Accounts_Price] where BillingType>0")]
        public DataTable Accounts_Price()
        {
            return (DataTable)base.Execute();
        }

       
        [DBCommand("SELECT * from [Operators_Price_CB]")]
        public DataTable Operators_Price_CB()
        {
            return (DataTable)base.Execute();
        }

       

    }
 
}
