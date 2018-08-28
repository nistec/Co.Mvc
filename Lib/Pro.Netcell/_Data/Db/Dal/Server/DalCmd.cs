using System;
using System.Data;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using Nistec.Data;
using Nistec.Data.SqlClient;
using System.Data.SqlClient;

using Nistec;

using System.Text;
using Netcell.Data;
using Nistec.Data.Factory;

namespace Netcell.Data.Server
{
   

    public class DalCmd : Nistec.Data.SqlClient.DbCommand
    {
        public DalCmd()
            : base(DBRule.ConnectionString)
        {
           
        }

        public static DalCmd Instance
        {
            get { return new DalCmd(); }
        }

       


        public int ExecuteCommand(string command, int timeout, params object[] parameters)
        {
            return base.ExecuteNonQuery(command, DataParameter.GetSql( parameters), CommandType.Text,timeout);
        }

        public int ExecuteCommand(string command, int timeout, bool async)
        {
            if (async)
            {
                return base.ExecuteAsyncCommand(command, 100, timeout, 0);
            }
            else
            {
                return base.ExecuteNonQuery(command,null, CommandType.Text, timeout);
            }
        }


        public int GetLogIn(string LogInName, string Pass)
        {
            return (int)base.LookupQuery<int>("AccountId", "Users",
                "IsBlocked=0 and LogInName=@LogInName and Pass=@Pass", -1,
                new object[] { LogInName, Pass });
        }

        //[DBCommand(DBCommandType.StoredProcedure, "sp_Trans_new")]
        //public int Trans_New()
        //{
        //    object o=base.ExecuteProcedure("sp_Trans_new", null);
        //    return Convert.ToInt32(o);
        //    //return base.Execute();
        //}

      

        [DBCommand( DBCommandType.StoredProcedure,"sp_NotifyToClient")]
        public DataTable NotifyToClient() 
        { 
            return (DataTable)base.Execute(); 
        }

        public int DeleteNotify_Map(string msgs)
        {
           return base.ExecuteNonQuery(string.Format("delete from Notification_Map where MessageId IN({0})", msgs));
        }

        [Obsolete("not used")]
        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaigns_Relay")]
        public int Campaigns_Relay_Status
            (
            [DbField()] int SentId,
            [DbField()] int RelayStatus,
            [DbField(500)] string StatusDescription,
            [DbField(50)] string MessageId 
            )
        {
            return (int)base.Execute(SentId,RelayStatus,StatusDescription,MessageId);
        }


        #region Accounts

        [DBCommand("SELECT * FROM [Accounts]")]
        public DataTable Accounts() { return (DataTable)base.Execute(); }

        #endregion

        #region Operators

        //[DBCommand("SELECT * FROM [Operators_Prefix]")]
        //public DataTable OperatorPrefix() { return (DataTable)base.Execute(); }

        [DBCommand("SELECT * FROM [Operators_Definition]")]
        public DataTable Operator_Definition() { return (DataTable)base.Execute(); }

        [DBCommand("SELECT * FROM [Operators_Definition] WHERE (Server=0 or Server=@Server)")]
        public DataTable Operator_Definition(int Server) { return (DataTable)base.Execute(Server); }

        //[DBCommand("SELECT * FROM [Operators_Rules]WHERE (Server=0 or Server=@Server)")]
        //public DataTable Operators_Rules(int Server) { return (DataTable)base.Execute(Server); }

        [DBCommand(DBCommandType.Update, "Operators_Definition")]
        public int Operators_Definition_HoldDequeue
            (
            [DbField(DalParamType.Key)] int OperatorId,
            [DbField()] bool HoldDequeue
            )
        {
            return (int)base.Execute(OperatorId, HoldDequeue);
        }

        public bool Lookup_Operator_HoldDequeue(int OperatorId)
        {
            return base.Dlookup("HoldDequeue", "Operators_Definition", string.Format("OperatorId={0}", OperatorId),false);
        }
        public bool Lookup_Operator_HoldDequeue(string QueueName)
        {
            return base.Dlookup("HoldDequeue", "Operators_Definition", string.Format("QueueName='{0}'", QueueName), false);
        }

        #endregion

        [DBCommand("SELECT * FROM [Channels] WHERE (Server=0 or Server=@Server) and Enabled=1")]
        public DataTable Channels(int Server) { return (DataTable)base.Execute(Server); }


        //[DBCommand("SELECT * FROM [vw_Aggregator_Rules] WHERE (Server=0 or Server=@Server)")]
        //public DataTable Aggregator_Rules(int Server) { return (DataTable)base.Execute(Server); }

        //[DBCommand("SELECT * FROM [vw_Carrier_Rules]")]
        //public DataTable Carrier_Rules() { return (DataTable)base.Execute(); }


        [DBCommand("SELECT * FROM [vw_Users_Relation] WHERE AccountId=@AccountId and GroupId=@GroupId")]
        public DataTable Users_Relation(int AccountId, int GroupId)
        { return (DataTable)base.Execute(AccountId, GroupId); }

     
    }
   

  

}

