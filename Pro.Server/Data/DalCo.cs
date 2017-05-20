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


namespace Pro.Server.Data
{
   

    public class DalCo : Nistec.Data.SqlClient.DbCommand
    {
        public DalCo()
            : base(DBconfig.CnnCo)
        {

        }

        public static DalCo Instance
        {
            get { return new DalCo(); }
        }

        public int ExecuteCommand(string command, int timeout, bool async)
        {
            if (async)
            {
                return base.ExecuteAsyncCommand(command, 100, timeout, 0);
            }
            else
            {
                return base.ExecuteNonQuery(command,null, CommandType.StoredProcedure, timeout);
            }
        }


        public int ExecuteCommand(string command)
        {
            return base.ExecuteNonQuery(command);
        }

        //public new int ExecuteAsyncCommand(string command, int interval, int timeout, int waitForDelay)
        //{
        //    return base.ExecuteAsyncCommand(command, interval, timeout, waitForDelay);
        //}
    }
}

