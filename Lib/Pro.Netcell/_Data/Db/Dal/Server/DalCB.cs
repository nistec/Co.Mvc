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

    public class DalCmdCB : Nistec.Data.SqlClient.DbCommand
    {
        public DalCmdCB()
            : base(DBRule.ConnectionString)
        {
        }

        public static DalCmdCB Instance
        {
            get { return new DalCmdCB(); }
        }

        public void ActiveConnectionClose()
        {
            base.ConnectionClose();
        }

        #region DalAccount

        [DBCommand("SELECT * FROM [vw_AccountsCB]")]
        public DataTable AccountsCB() { return (DataTable)base.Execute(); }

        [DBCommand("SELECT * FROM [vw_AccountsCB] where AccountId=@AccountId")]
        public DataRow AccountsCB(int AccountId) { return (DataRow)base.Execute(AccountId); }

        public int AccountBillingType(int AccountId)
        {
            return (int)base.Dlookup("AccountBillingType", "Accounts", string.Format("AccountId={0}", AccountId), (int)0);
        }
        public int GetParentId(int AccountId)
        {
            return
                (int)base.Dlookup("ParentId", "Accounts", "AccountId =" + AccountId.ToString(), AccountId);
        }

        //[DBCommand("SELECT * FROM [Contacts_Blocked] where GroupId=0 and BlockLevel>0")]
        //public DataTable Contacts_Blocked() { return (DataTable)base.Execute(); }

        //[DBCommand("SELECT * FROM [Contacts_Mail_Blocked] where GroupId=0 and BlockLevel>0")]
        //public DataTable Contacts_Mail_Blocked() { return (DataTable)base.Execute(); }


        [DBCommand("SELECT * FROM [Contacts_Items_Blocked_Cli]")]
        public DataTable Contacts_Blocked_Cli() { return (DataTable)base.Execute(); }

        [DBCommand("SELECT * FROM [Contacts_Items_Blocked_Mail]")]
        public DataTable Contacts_Blocked_Mail() { return (DataTable)base.Execute(); }
 
  
        public void BeginTransaction()
        {
            base.ConnectionOpen();
            base.Transaction = base.Connection.BeginTransaction();
        }

        public void EndTransaction(bool commit)
        {
            if (commit)
                base.Transaction.Commit();
            else
                base.Transaction.Rollback();
            base.Transaction = null;
        }

       
        [DBCommand(DBCommandType.StoredProcedure, "sp_CancelAccountPending")]
        public void CancelAccountPending
        (
          [DbField()]int AccountId,
          [DbField()]decimal PendingPrice
            //[DbField()]int ParentId
        )
        {
            base.Execute(AccountId, PendingPrice);//,ParentId);
        }
     
        #endregion

        #region Message

 

        [DBCommand(DBCommandType.StoredProcedure, "sp_MessageResult")]
        public DataRow Messages_Ack
        (
            [DbField()]int TransId
        )
        {
            return (DataRow)base.Execute(TransId);
        }

        #endregion

        #region DalBilling


        //public void BillingMessageCB(IQueueItem item, string message, string url, int status, int segment, int totalSegments, int units, string result,decimal defaultPrice,string ConfirmId)
        //{
        //    BillingMessageCB(Convert.ToInt32(item.TransactionId), item.MessageId, item.OperationId, message, url, item.Destination, item.Price, status, item.Retry, segment, totalSegments, units, result, item.ItemId, item.SenderId, defaultPrice,ConfirmId, item.Notify);
        //}


        [DBCommand(DBCommandType.StoredProcedure, "sp_BillingMessageBlocked")]
        public void BillingMessageBlocked
        (
        [DbField()]int TransId
        )
        {
            base.Execute(TransId);
        }

        //[DBCommand(DBCommandType.StoredProcedure, "sp_BillingMessage_CB")]
        //public void BillingMessageCB
        //(
        //[DbField()]int TransId,
        //[DbField()]int MessageId,
        //[DbField()]int OperatorId,
        //[DbField(500)]string MessageText,
        //[DbField(250)]string Url,
        //[DbField(20)]string MsgTo,
        //[DbField()]decimal Price,
        //[DbField()]int Status,
        //[DbField()]int Retry,
        //[DbField()]byte Segment,
        //[DbField()]byte TotalSegments,
        //[DbField()]byte Units,
        //[DbField(250)]string Result,
        //[DbField()]Guid ItemId,
        //[DbField()]int AccountId,
        //[DbField()]decimal DefaultPrice,
        //[DbField(50)]string ConfirmId,
        //[DbField(250)]string UrlNotify,
        //[DbField()]byte Notify,
        //[DbField()]int OpId,
        //[DbField(50)]string Reference,
        //[DbField()]int MtId,
        //[DbField(20)]string Sender,
        //[DbField()]int Cid
        //)
        //{
        //    base.Execute(TransId, MessageId, OperatorId, MessageText, Url, MsgTo, Price, Status, Retry, Segment, TotalSegments, Units, Result, ItemId, AccountId, DefaultPrice, ConfirmId, UrlNotify, Notify, OpId,Reference ,MtId,Sender,Cid);
        //}



        [Obsolete("not used")]
        [DBCommand(DBCommandType.Insert, "WapTrans")]
        public void InsertWapTrans
            (

            //[DbField()]string ItemId,
            [DbField()]int ItemId,
            [DbField(15)]string CellNumber,
            [DbField(50)]string MimeType,
            [DbField()]int ContentId,
            [DbField()]int Retry,
            //[DbField()]string CreationDate
            [DbField()]string Path,
            [DbField()]int ExpirationDays,
            [DbField()]int MaxRetry,
            //[DbField()]int CampaignId,
            [DbField()]bool IsRB
            )
        {

            base.Execute(/*ItemId,*/ ItemId, CellNumber, MimeType, ContentId, Retry, Path, ExpirationDays, MaxRetry, IsRB);
        }


       
        #endregion

        #region DalPending



        [DBCommand(DBCommandType.StoredProcedure, "sp_FindPendingToSend")]
        public DataRow FindPendingToSend()
        {
            return (DataRow)base.Execute();
        }

        public int ExecNonQuery(string sql)
        {
            return base.ExecuteNonQuery(sql);
        }

        #endregion


         [DBCommand(DBCommandType.StoredProcedure, "sp_Operators_GetByIp")]
        public int GetOperator_ByIp
        (
          [DbField] string Ip,
          [DbField]ref int OperatorId
        )
        {
            object[] values = new object[] { Ip, OperatorId };
            int res = (int)base.Execute(values);
            OperatorId = Types.ToInt(values[1]);
            return res;
        }

         public int GetOperator_ByIp(string Ip)
         {
             int OperatorId = 0;
             GetOperator_ByIp(Ip, ref OperatorId);
             return OperatorId;
         }
      


        //[DBCommand(DBCommandType.Insert, "Trans_Html")]
        //public int Trans_Html_Insert(
        //[DbField()] int TransId,
        //[DbField()] string Body
        // )
        //{
        //    return (int)base.Execute(TransId, Body);
        //}

        [DBCommand("SELECT * from [Accounts_Features] where AccountId=@AccountId")]
        public DataRow Accounts_Features(int AccountId)
        {
            return (DataRow)base.Execute(AccountId);
        }
        [DBCommand("SELECT AccountDetails from [vw_Accounts_Details] where AccountId=@AccountId")]
        public object Accounts_Details(int AccountId)
        {
            return base.Execute(AccountId);
        }

        public decimal AccountActualCredit(int AccountId)
        {
            //decimal credit = base.Dlookup("ActualCredit", "vw_Accounts_Credit", string.Format("AccountId={0}", AccountId), (decimal)0.0);
            //return credit;
            return base.Dlookup("ActualCredit", "vw_Accounts_Credit", string.Format("AccountId={0}", AccountId), (decimal)0.0);
        }

        #region obsolete
        /*
                 [Obsolete("not used")]
        [DBCommand(DBCommandType.StoredProcedure, "sp_BillingMessage_Mail")]
        public int BillingMessage_Mail(
        [DbField()] int CampaignId,
        [DbField()] int SentId,
        [DbField()] int BatchId,
        [DbField()] string Target,
        [DbField()] decimal Price,
        //[DbField()] decimal DefaultPrice,
        [DbField()] int Status,
        [DbField()] int NotifyStatus,
        [DbField()] int Units,
        [DbField()] int AccountId,
        [DbField()] bool IsBlocked
        //[DbField()] string Method,
        )
        {
            return (int)base.Execute(CampaignId, SentId, BatchId, Target, Price, Status, NotifyStatus, Units, AccountId, IsBlocked);
        }
         
         [Obsolete("not used")]
        [DBCommand(DBCommandType.Insert, "Trans_Remote")]
        public int Trans_Remote_Insert(
        [DbField()] int TransId,
        [DbField()] int Platform,
        [DbField()] int AccountId,
        [DbField()] string Body,
        [DbField()] string MsgTo,
        [DbField()] decimal Price,
        [DbField()] int Units,
        [DbField()] int Status,
        [DbField()] int NotifyStatus,
        [DbField()] int CampaignId,
        [DbField()] int ItemsCount
        )
        {
            return (int)base.Execute(TransId, Platform, AccountId, Body, MsgTo, Price, Units, Units, NotifyStatus, CampaignId, ItemsCount);
        }

        [DBCommand("UPDATE [Trans_Pending] SET Status=@Status,Ack=@Ack Where TransId=@TransId")]
        public void Trans_PendingUpdate(string TransId, int Status, int Ack)
        {
            base.Execute(TransId, Status, Ack);
        }


        [DBCommand(DBCommandType.StoredProcedure, "sp_Trans_Pending")]
        public void Trans_PendingInsert
        (
          [DbField()]int TransId,
          [DbField()]DateTime DateToSend,
          [DbField()]decimal Price,
          [DbField()]string Sender,
          [DbField()]int MtId,
          [DbField()]byte Status,
          [DbField()]int Retry,
          [DbField()]int MsgCount,
          [DbField()]int AccountId,
          [DbField()]string Messages,
          [DbField()]int DateIndex,
          [DbField()]int Ack,
          [DbField()]string Reference
            //[DbField()]string Method

        )
        {
            base.Execute(TransId, DateToSend, Price, Sender, MtId, Status, Retry, MsgCount, AccountId, Messages, DateIndex, Ack, Reference);

            //try
            //{
            //    BeginTransaction();
            //    base.ExecuteTrans(base.Transaction as SqlTransaction, false, new object[] { TransId, DateToSend, Price, Sender, MediaType, Status, Retry, MsgCount, AccountId, Messages, DateIndex, Ack });
            //    UpdateCredit_Pending_Transaction(AccountId, Price);
            //    EndTransaction(true);

            //}
            //catch (Exception ex)
            //{
            //    string s = ex.Message;
            //    EndTransaction(false);
            //}
            //finally
            //{
            //    base.Connection.Close();
            //}
        }


        [DBCommand(DBCommandType.StoredProcedure, "sp_Trans_Process_Exec")]
        public void Execute_Trans_Process
        (
          [DbField()]int TransId,
          [DbField()]int AccountId,
          [DbField()]DateTime DateToSend,
          [DbField()]int Received,
          [DbField()]int Canceled,
          [DbField()]int Success,
          [DbField()]int Failed,
          [DbField()]decimal Price,
          [DbField()]int MtId,
          [DbField()]byte Status,
          [DbField()]int Ack,
          [DbField()]string Notify,
          [DbField()]int CampaignId,
          [DbField()]int DateIndex,
          [DbField()]string Reference,
          [DbField()]string ShortMessage,
          [DbField()]int Units,
          [DbField()]int UserId,
          [DbField()]int Module,
          [DbField()]int Server
        )
        {
            base.Execute(TransId, AccountId, DateToSend, Received, Canceled, Success, Failed, Price, MtId, Status, Ack, Notify, CampaignId, DateIndex, Reference, ShortMessage, Units, UserId, Module, Server);
        }


        [DBCommand(DBCommandType.StoredProcedure, "sp_TransProcessUpdate")]
        public void Trans_ProcessUpdate
        (
          [DbField()]int TransId,
          [DbField()]int Canceled,
          [DbField()]int AccountId,
          [DbField()]decimal Cost
        )
        {
            base.Execute(TransId, Canceled, AccountId, Cost);
        }
        */
        #endregion

    }

}

