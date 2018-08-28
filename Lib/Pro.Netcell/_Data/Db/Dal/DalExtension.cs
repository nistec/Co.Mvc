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

namespace Netcell.Data
{

    public class DalExtension : DbCommand
    {

        #region ctor

        public DalExtension()
            : base(Netcell.Data.DBRule.CnnNetcell)
        {

        }
        public static DalExtension Instance
        {
            get
            {
                return new DalExtension();
            }
        }

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


        [DBCommand(DBCommandType.StoredProcedure, "sp_View_Remove_b")]
        public int View_Remove
             (
             [DbField] int SentId,
             [DbField] int Status,
             [DbField] int Platform,
             [DbField] bool IsAll,
             [DbField] int SrcId,
             [DbField] int Version,
             [DbField] int AccountId,
             [DbField] string Target)
        {
            return (int)base.Execute(new object[] { SentId, Status, Platform, IsAll, SrcId, Version, AccountId, Target });
        }

        [DBCommand("UPDATE Trans_Batch SET Status=@Status where BatchId=@BatchId")]
        public int Batch_UpdateStatus(int Status, int BatchId)
        {
            return (int)base.Execute(new object[] { Status, BatchId });
        }
        
        [DBCommand(DBCommandType.InsertOrUpdate, "Trans_Batch")]
        public int Trans_Batch_InsertOrUpdate
            (
            [DbField(DalParamType.Identity)]ref int BatchId,
            [DbField] int AccountId,
            [DbField] int CampaignId,
            [DbField] int BatchCount,
            [DbField] int BatchStatus,
            [DbField] DateTime BatchTime,
            [DbField] int BatchIndex,
            [DbField] int BatchRange,
            [DbField] int BatchType,
            [DbField] int UserId,
            //[DbField] DateTime Creation
            [DbField] int Server,
            [DbField] int Platform,
            [DbField] int MtId,
            [DbField] decimal DefaultPrice,
            [DbField] string PublishKey,
            [DbField] string BatchMessage,
            [DbField] int BatchSize,
            [DbField] int BatchUnits,
            [DbField] string BatchSender
            )
        {
            object[] values = new object[] { BatchId, AccountId, CampaignId, BatchCount, BatchStatus, BatchTime, BatchIndex, BatchRange, BatchType, UserId, Server, Platform, MtId, DefaultPrice, PublishKey, BatchMessage,BatchSize,BatchUnits,BatchSender };
            int res = (int)base.Execute(values);
            BatchId = Types.ToInt(values[0]);
            return res;
        }

        public int Batch_Items_Insert(DataTable targets)
        {
            InsertTable(targets, "Trans_Items");
            return 0;
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Batch_Items_ToSend_b")]
        public DataTable Batch_Items_ToSend
            (
                [DbField()]int BatchId,
                [DbField()]int AccountId,
                [DbField()]int Platform
            )
        {
            return (DataTable)base.Execute(BatchId, AccountId, Platform);
        }

        public DataTable Batch_Items_ToSend(DataTable targets, int BatchId, int AccountId, int Platform)
        {
            InsertTable(targets, "Trans_Items");
            return Batch_Items_ToSend(BatchId, AccountId, Platform);
        }

        [DBCommand(DBCommandType.InsertOrUpdate, "Trans_Batch_Content")]
        public void Trans_Batch_Content_InsertOrUpdate
       (
         [DbField(DalParamType.Key)]int BatchId,
         [DbField()]int PlatformView,
         [DbField()]string Message,
         //[DbField()]string Body,
         [DbField()]int Size,
         [DbField()]int Units,
         [DbField()]int MaxWidth,
         //[DbField()]string Title,
         [DbField()]string Sender,
         //[DbField()]bool IsRtl,
         [DbField()]int PagesCount,
         [DbField()]string PersonalDisplay
         //[DbField()]string AltHtml,
         //[DbField()]string Args
       )
        {
            base.Execute(BatchId, PlatformView, Message, Size, Units, MaxWidth, Sender, PagesCount, PersonalDisplay);
            //base.Execute(BatchId, PlatformView, Message, Body, Size, Units, MaxWidth, Title, Sender, IsRtl, PagesCount, PersonalDisplay, AltHtml, Args);
        }

        [DBCommand("SELECT * from [Accounts_Features] where AccountId=@AccountId")]
        public DataRow Accounts_Features(int AccountId)
        {
            return (DataRow)base.Execute(new object[] { AccountId });
        }
    }
}

