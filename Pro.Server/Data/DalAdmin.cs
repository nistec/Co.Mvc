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
using Nistec.Data.Factory;

namespace Pro.Server.Data
{
   

    public class DalAdmin : Nistec.Data.SqlClient.DbCommand
    {
        public DalAdmin()
            : base(DBconfig.CnnServices)
        {

        }

        public static DalAdmin Instance
        {
            get { return new DalAdmin(); }
        }

        public int ExecuteCommand(string command, int timeout, bool async)
        {
            if (async)
            {
                return base.ExecuteAsyncCommand(command, 100, timeout, 0);
            }
            else
            {
                return base.ExecuteNonQuery(command, null, CommandType.StoredProcedure, timeout);
            }
        }


        public int ExecuteCommand(string command)
        {
            return base.ExecuteNonQuery(command);
        }

        #region Admin

        [DBCommand(DBCommandType.StoredProcedure, "sp_Admin_Scheduler_job")]
        public DataTable Scheduler_job()
        {
            return (DataTable) base.Execute();
        }

        
        [DBCommand(DBCommandType.Insert, "Admin_Scheduler_History")]
        public int Scheduler_History
            (
            [DbField]int SchedulerId,
            [DbField]int CommandId,
            [DbField]string DB,
            [DbField]int Status,
            [DbField(500)]string Result
            )
        {
            return (int)base.Execute(SchedulerId, CommandId, DB, Status, Result);
        }
        #endregion
    }
   

 
}

