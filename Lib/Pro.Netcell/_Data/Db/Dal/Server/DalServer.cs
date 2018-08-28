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

namespace Netcell.Data.Server
{
    
    public class DalServer : Nistec.Data.SqlClient.DbCommand
    {
        #region ctor

        public DalServer()
            : base(Netcell.Data.DBRule.CnnNetcell)
        {

        }

        public static DalServer Instance
        {
            get { return new DalServer(); }
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

        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_job_b")]
        public DataRow Campaigns_job([DbField] int Platform, [DbField] int Server)
        {
            return (DataRow)base.Execute(new object[] { Platform, Server });
        }
        
        //[DBCommand(DBCommandType.StoredProcedure, "sp_Campaigns_Exec")]
        //public DataRow Campaigns_Execute(int CampaignId, int BatchId, int Server, int UserId)
        //{
        //    return (DataRow)base.Execute(new object[] { CampaignId, BatchId, Server, UserId });
        //}

        //[DBCommand(DBCommandType.StoredProcedure, "sp_Campaigns_Exec")]
        //public DataRow Campaigns_Execute(int CampaignId, int BatchId, int Server, int UserId, bool ValidateExists)
        //{
        //    return (DataRow)base.Execute(new object[] { CampaignId, BatchId, Server, UserId, ValidateExists });
        //}

        //[DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Finally")]
        //public int Campaigns_Finally([DbField] int CampaignId, [DbField] int BatchId, [DbField] int Platform)
        //{
        //    return (int)base.Execute(new object[] { CampaignId, BatchId, Platform });
        //}

        //[DBCommand(DBCommandType.Update, "Campaigns_Batch_History")]
        //public int Campaigns_Batch_History_Update
        //    (
        //    [DbField(DalParamType.Key)] int CampaignId,
        //    [DbField(DalParamType.Key)] int BatchId,
        //    [DbField] int BatchCount,
        //    [DbField] int BatchStatus,
        //    [DbField] DateTime BatchFinalTime,
        //    [DbField] int BatchExecState,
        //    [DbField] int BatchWrongCount,
        //    [DbField] decimal BatchPrice,
        //    [DbField] int BatchUnits,
        //    [DbField] int Server)
        //{
        //    return (int)base.Execute(new object[] { CampaignId, BatchId, BatchCount, BatchStatus, BatchFinalTime, BatchExecState, BatchWrongCount, BatchPrice, BatchUnits, Server });
        //}

#if(RB)
        //public int GetAggregatorBySC(string SC)
        //{
        //    return base.Dlookup("AggregatorId", "Operators_SC", "SC='" + SC + "'", 0);
        //}
        //public decimal GetPriceCodePrice(int PriceCode, int AggregatorId, string Method)
        //{
        //    return base.Dlookup("Price", "Pricing_RB", string.Format("PriceCode={0} and AggregatorId={1} and Method in({2})", PriceCode, AggregatorId, Method), 0M);
        //}
#endif

        //[DBCommand(DBCommandType.Insert, "Campaigns_SentItems_Cell")]
        //public int Campaigns_SentItems_Cell_Update([DbField] int SentId, [DbField] int Status, [DbField] int NotifyStatus)
        //{
        //    return (int)base.Execute(new object[] { SentId, Status, NotifyStatus });
        //}

        //[DBCommand(DBCommandType.Insert, "Campaigns_SentItems_Mail")]
        //public int Campaigns_SentItems_Mail_Update([DbField] int SentId, [DbField] int Status, [DbField] int NotifyStatus)
        //{
        //    return (int)base.Execute(new object[] { SentId, Status, NotifyStatus });
        //}


        //[DBCommand(DBCommandType.StoredProcedure, "sp_Campaigns_Batch_Watch")]
        //public int Campaigns_Batch_Scheduled
        //    (
        //    [DbField(DalParamType.Key)] int CampaignId,
        //    [DbField(DalParamType.Key)] int BatchId,
        //    [DbField] int BatchCount,
        //    [DbField] int BatchStatus,
        //    [DbField] DateTime BatchTime,
        //    [DbField] int BatchIndex,
        //    [DbField] int BatchRange,
        //    [DbField] int BatchType,
        //    [DbField] int UserId,
        //    [DbField] int Server,
        //    [DbField] string QueueId
        //    )
        //{
        //    return (int)base.Execute(new object[] { CampaignId, BatchId, BatchCount, BatchStatus, BatchTime, BatchIndex, BatchRange, BatchType, UserId, Server, QueueId });
        //}

        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaigns_Watch_Items")]
        public DataTable Campaigns_WatchItems(
            [DbField] int CampaignId,
            [DbField] string DateField
            )
        {
            return (DataTable)base.Execute(new object[] { CampaignId, DateField });
        }

        #endregion

        #region contacts
       
        #endregion

    }
}

