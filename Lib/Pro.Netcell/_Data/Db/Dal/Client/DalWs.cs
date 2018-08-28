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

namespace Netcell.Data.Client
{


    public class DalWs : Nistec.Data.SqlClient.DbCommand
    {
        #region ctor

        public DalWs()
            : base(Netcell.Data.DBRule.CnnNetcell)
        {

        }

        public static DalWs Instance
        {
            get { return new DalWs(); }
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

            IDbCmd cmd = DbFactory.Create(Netcell.Data.DBRule.CnnNetcell, DBProvider.SqlServer);
            return cmd.Adapter.UpdateChanges(dt.GetChanges(), destinationTable);
        }
        public int DeleteFromTable(string tableName, string primaryKeyName, int primaryKey)
        {
            string sql = string.Format("delete from {0} where {1}={2}", tableName, primaryKeyName, primaryKey);
            return base.ExecuteNonQuery(sql);
        }
        #endregion

        #region campaigns

        [DBCommand(DBCommandType.StoredProcedure, "sp_Ws_Campaigns_Items")]
        public DataTable Campaigns_Ws(
            //1=items,2=sentItems,3=blockItems,4=reply,4=findItems,6=summarize
            [DbField()]int ReportType,
            [DbField()]int AccountId,
            [DbField()]int CampaignId,
            [DbField()]int BatchId
            )
        {
            return (DataTable)base.Execute(ReportType,
                AccountId, CampaignId, BatchId);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Ws_Campaigns_Items")]
        public DataTable Campaigns_Ws(
            //1=items,2=sentItems,3=blockItems,4=reply,4=findItems,6=summarize
            [DbField()]int ReportType,
            [DbField()]int AccountId,
            [DbField()]int CampaignId,
            [DbField()]int BatchId,
            [DbField()]DateTime DateFrom,
            [DbField()]DateTime DateTo
            )
        {
            return (DataTable)base.Execute(ReportType,
                AccountId, CampaignId, BatchId, DateFrom, DateTo);
        }


        #endregion

        #region contacts
        [DBCommand(DBCommandType.StoredProcedure, "sp_Ws_Contacts")]
        public DataTable Contacts_Ws(
            [DbField()]int ReportType,//1=all,2=by group,3=blockItems,4=filter
            [DbField()]int AccountId,
            [DbField()]string Filter,
            [DbField()]int Layout
            )
        {
            return (DataTable)base.Execute(ReportType,
                AccountId, Filter, Layout);
        }
        [DBCommand(DBCommandType.StoredProcedure, "sp_Ws_Contacts")]
        public DataTable Contacts_Ws(
            [DbField()]int ReportType,//1=all,2=by group,3=blockItems,4=filter
            [DbField()]int AccountId,
            [DbField()]string Filter,
            [DbField()]int Layout, // 0=all,1=minimaized
            [DbField()]int Platform
            )
        {
            return (DataTable)base.Execute(ReportType,
                AccountId, Filter, Layout, Platform);
        }


        #endregion

        [DBCommand(DBCommandType.StoredProcedure, "sp_Ws_Trans_Items")]
        public DataTable TransItems_Ws(
            //1=items,2=sentItems,3=blockItems,4=reply,4=findItems,6=summarize
            [DbField()]int ReportType,
            [DbField()]int AccountId,
            [DbField()]int BatchId
            )
        {
            return (DataTable)base.Execute(ReportType, AccountId, BatchId);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Api")]
        public DataTable ApiQueryTable(
            [DbField()]string Type,
            [DbField()]int AccountId,
            [DbField()]string Args
            )
        {
            return (DataTable)base.Execute(Type, AccountId, Args);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Api")]
        public DataRow ApiQueryRecord(
            [DbField()]string Type,
            [DbField()]int AccountId,
            [DbField()]string Args
            )
        {
            return (DataRow)base.Execute(Type, AccountId, Args);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Api")]
        public T ApiQueryScalar<T>(
            [DbField()]string Type,
            [DbField()]int AccountId,
            [DbField()]string Args,
            [DbField()]string Field
            )
        {
            DataRow dr = ApiQueryRecord(Type, AccountId, Args);
            if(dr==null)
                return default(T);
            return GenericTypes.Convert<T>( dr[Field]);
        }

    }
}

