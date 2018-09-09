using Nistec;
using Nistec.Data;
using Nistec.Data.Factory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
//using WebMatrix.Data;

namespace Pro.Data
{
 
    public class DalPro:Nistec.Data.SqlClient.DbCommand
    {

        #region ctor

        public DalPro()
            : base(DbConfig.CnnPro)
        {

        }

        #endregion

        #region common

        public int ExecNonQuery(string sql)
        {
            return base.ExecuteNonQuery(sql);
        }

        public void InsertTable(DataTable dt, string destinationTable)
        {
            //MAPPING[] maps = MAPPING.Create("FirstName", "LastName", "City", "CellNumber", "AccountId");
            base.ExecuteBulkCopy(dt, destinationTable, null);
        }
        public int UpdateTable(DataTable dt, string destinationTable)
        {
            using (IDbCmd cmd = DbFactory.Create(DbConfig.CnnPro, DBProvider.SqlServer))
            {
                return cmd.Adapter.UpdateChanges(dt.GetChanges(), destinationTable);
            }
        }
        public int DeleteFromTable(string tableName, string primaryKeyName, int primaryKey)
        {
            string sql = string.Format("delete from {0} where {1}={2}", tableName, primaryKeyName, primaryKey);
            return base.ExecuteNonQuery(sql);
        }
        public int DeleteFromTable(string tableName, string primaryKeyName, string primaryKey)
        {
            string sql = string.Format("delete from {0} where {1}='{2}'", tableName, primaryKeyName, primaryKey);
            return base.ExecuteNonQuery(sql);
        }
        #endregion

    }
}
