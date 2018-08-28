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
//using Netcell.Data.Common;
using Netcell;

namespace Netcell.Data
{

    public class DalController : Nistec.Data.SqlClient.DbCommand
    {
        public DalController()
            : base(DBRule.ConnectionString)
        {
        }

        public static DalController Instance
        {
            get { return new DalController(); }
        }

        public void ActiveConnectionClose()
        {
            base.ConnectionClose();
        }

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

        #region counter

        [DBCommand(DBCommandType.StoredProcedure, "sp_Counters_b")]
        public int Counter_New
           (
           [DbField]int CounterId,
           [DbField]int Count,
           [DbField]ref int MinValue,
           [DbField]ref int MaxValue
           )
        {

            object[] values = new object[] { CounterId, Count, MinValue, MaxValue };

            int res = (int)base.Execute(values);
            MinValue = Types.ToInt(values[2], 0);
            MaxValue = Types.ToInt(values[3], 0);

            if (MinValue <= 0 || MaxValue <= 0)
            {
                throw new Exception("Counters error");
            }
            return res;
        }

 
        [DBCommand(DBCommandType.Insert, "Trans_Items")]
        public int Trans_Items_Insert
            (
            [DbField(DalParamType.Key)]  int MessageId,
            [DbField] int CampaignId,
            [DbField] int BatchId,
            [DbField] DateTime SendTime,
            [DbField(50)] string Target,
            [DbField] int State,
            [DbField] int GroupId,
            [DbField] string Coupon,
            [DbField] string Personal,
            [DbField] string Sender,
            [DbField] int ItemIndex,
            [DbField] int Platform,
            [DbField] int ContactId
            )
        {
            object[] values = new object[] { MessageId, CampaignId, BatchId, SendTime, Target, State, GroupId, Coupon, Personal, Sender, ItemIndex, Platform,ContactId };
            int res = (int)base.Execute(values);
            return res;
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Session_Update")]
        public int Session_Exec
           (
           [DbField]int Mode, //--2=remove,0=insert,1=update,
           [DbField]string SessionId,
           [DbField]int AppId,
           [DbField]string SessionValue,        
           [DbField]string SC,          
           [DbField]int State,        
           [DbField]int ttl)
        {
            return (int)base.Execute(Mode,SessionId,AppId,SessionValue,SC,State,ttl);
        }
        //public int Session_Add
        //  (
        //  string SessionId,
        //  SessionApps AppId,
        //  string SessionValue,
        //  string SC,
        //  int State,
        //  int ttl)
        //{
        //    return (int)Session_Exec(0, SessionId, (int)AppId, SessionValue, SC, State, ttl);
        //}
        //public int Session_Update
        //   (
        //   string SessionId,
        //   SessionApps AppId,
        //   string SessionValue,
        //   string SC,
        //   int State)
        //{
        //    return (int)Session_Exec(1, SessionId, (int)AppId, SessionValue, SC, State, 0);
        //}
        //public int Session_Remove
        //   (
        //   string SessionId,
        //   SessionApps AppId)
        //{
        //    return (int)Session_Exec(2, SessionId, (int)AppId, null, null, 0, 0);
        //}

        #endregion

        #region billing
        
        [DBCommand(DBCommandType.StoredProcedure, "sp_Billing_Item_b")]
        public void Billing_Item
        (
            [DbField()]int SentId,
            [DbField()]int OperatorId,
            [DbField()]int Retry,
            [DbField()]int Status,
            [DbField()]int Size,
            [DbField()]int Units,
            [DbField()]decimal Price,
            //[DbField()]Guid ItemId,
            [DbField()]int OpId,
            [DbField()]int AckStatus,
            [DbField()]string Result,
            [DbField()]int BatchId,
            [DbField()]string Target,
            [DbField()]int AccountId,
            [DbField()]decimal DefaultPrice,
            [DbField()]int MtId,
            [DbField()]int Platform,
            [DbField()]string ConfirmId,
            [DbField()]string UrlNotify,
            [DbField()]int Notify,
            [DbField()]string Reference,
            [DbField()]bool IsBlocked

        )
        {
            base.Execute(SentId, OperatorId, Retry, Status, Size, Units, Price, /*ItemId,*/ OpId, AckStatus, Result, BatchId, Target, AccountId, DefaultPrice, MtId, Platform, ConfirmId, UrlNotify, Notify, Reference, IsBlocked);
        }
        #endregion

        #region Batch 

        //[DBCommand(DBCommandType.StoredProcedure, "sp_Batch_Items_ToSend_b")]
        //public DataTable Batch_Items_ToSend
        //    (
        //        [DbField()]int BatchId,
        //        [DbField()]int AccountId,
        //        [DbField()]int Platform
        //    )
        //{
        //    return (DataTable)base.Execute(BatchId, AccountId, Platform);
        //}

        [DBCommand(DBCommandType.StoredProcedure, "sp_Batch_Items_ToSend_c")]
        public DataTable Batch_Items_ToSend
                  (
                      [DbField()]int BatchId,
                      [DbField()]int AccountId,
                      [DbField()]int Platform,
                      [DbField()]int BatchType
                  )
        {
            return (DataTable)base.Execute(BatchId, AccountId, Platform, BatchType);
        }

        //[DBCommand(DBCommandType.StoredProcedure, "sp_Batch_Scheduler_Next")]
        //public DataTable Batch_Items_ToSend
        //          (
        //              [DbField()]int BatchId
        //          )
        //{
        //    return (DataTable)base.Execute(BatchId);
        //}

        //[DBCommand(DBCommandType.StoredProcedure, "sp_Trans_Process_Exec_b")]
        //public void Execute_Trans_Process
        //(
        //  [DbField()]int BatchId,
        //  [DbField()]int CampaignId,
        //  [DbField()]int AccountId,
        //  [DbField()]int BatchCount,
        //  [DbField()]int Success,
        //  [DbField()]int Failed,
        //  [DbField()]int Canceled,
        //  [DbField()]byte Status,
        //  [DbField()]int Units,
        //  [DbField()]decimal DefaultPrice,
        //  [DbField()]int Platform,
        //  [DbField()]int MtId,
        //  [DbField()]int Server,
        //  [DbField()]int ProcessType,
        //  [DbField()]int UserId
        //)
        //{
        //    base.Execute(BatchId, CampaignId, AccountId, BatchCount, Success, Failed, Canceled, Status, Units, DefaultPrice, Platform, MtId, Server, ProcessType, UserId);
        //}

        public DataTable Batch_Items_ToSend(DataTable targets, int BatchId, int AccountId, int Platform)
        {
            InsertTable(targets, "Trans_Items");
            return Batch_Items_ToSend(BatchId, AccountId, Platform, 0);
        }

        //     public void Batch_Items_ToSend
        //        (
        //        [DbField]ref int BatchId,
        //        [DbField]ref  int MessageId,
        //        [DbField] int AccountId,
        //        [DbField] int UserId,
        //        [DbField] int UserId,
        //        [DbField] int CampaignId,
        //        [DbField] DateTime SendTime,
        //        [DbField] string Target,
        //        [DbField] int State,
        //        [DbField] int Retry,
        //        [DbField] int GroupId,
        //        [DbField] string Coupon,
        //        [DbField] string Personal,
        //        [DbField] string Sender,
        //        [DbField] int ItemIndex,
        //        [DbField] int BatchIndex,
        //        [DbField] int Platform)
        //{
        //         Trans_Id_New(ref BatchId,AccountId,UserId,
        //    BatchId=Batch_Items_ToSend(BatchId,AccountId,Platform);
        //         Trans_Items_Insert(ref MessageId, CampaignId, BatchId, SendTime, Target, State, Retry, GroupId, Coupon, Personal, Sender, ItemIndex, BatchIndex, Platform);
        //}


        /*
        //change:Trans_Batch_View
        [DBCommand(DBCommandType.InsertOrUpdate, "Trans_Batch_View")]
         public void Trans_Batch_View
        (
          [DbField(DalParamType.Key)]int BatchId,
          [DbField()]int PlatformView,
          [DbField()]string Body,
          [DbField()]int Size,
          [DbField()]int Units,
          [DbField()]int State,
          [DbField()]DateTime Modified,
          [DbField()]int MaxWidth,
          [DbField()]string Css,
          [DbField()]string Title,
          [DbField()]bool IsRtl,
          [DbField()]int PagesCount,
          [DbField()]string PersonalDisplay
        )
        {
            base.Execute(BatchId, PlatformView, Body, Size, Units, State, Modified, MaxWidth, Css, Title, IsRtl, PagesCount, PersonalDisplay);
        }

        public void Trans_Batch_View_Sms
       (
         int BatchId,
         string Body,
         int Size,
         int Units,
         int State,
         DateTime Modified,
         bool IsRtl,
         string PersonalDisplay
       )
        {
            Trans_Batch_View(BatchId, 1, Body, Size, Units, State, Modified, 0, null, null, IsRtl, 1, PersonalDisplay);
        }

        */



        [DBCommand(DBCommandType.InsertOrUpdate, "Trans_Batch_Content")]
        public void Trans_Batch_Content
       (
         [DbField(DalParamType.Key)]int BatchId,
         [DbField()]int PlatformView,
         [DbField()]string Message,
         [DbField()]string Body,
         [DbField()]int Size,
         [DbField()]int Units,
         [DbField()]int MaxWidth,
         [DbField()]string Title,
         [DbField()]string Sender,
         [DbField()]bool IsRtl,
         [DbField()]int PagesCount,
         [DbField()]string PersonalDisplay,
         [DbField()]string AltHtml,
         [DbField()]string Args
       )
        {
            base.Execute(BatchId, PlatformView, Message, Body, Size, Units, MaxWidth, Title, Sender, IsRtl, PagesCount, PersonalDisplay, AltHtml,Args);
        }

        public void Trans_Batch_Content_Sms
       (
         int BatchId,
         string Message,
         int Size,
         int Units,
         string Sender,
         bool IsRtl,
         string PersonalDisplay
       )
        {
            Trans_Batch_Content(BatchId, 1, Message, null, Size, Units, 0, null, Sender, IsRtl, 1, PersonalDisplay, null, null);
        }
        public void Trans_Batch_Content_Mail
       (
         int BatchId,
         string Subject,
         string Message,
         int Size,
         int Units,
         string Title,
         string Sender,
         bool IsRtl,
         string PersonalDisplay
       )
        {
            Trans_Batch_Content(BatchId, 2, Subject, Message, Size, Units, 0, Title, Sender, IsRtl, 1, PersonalDisplay, null, null);
        }
        #endregion

        #region trans pending

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

         }

        #endregion

        #region campaign batch
        /*
        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Batch_Create_b")]
         internal object Campaigns_Batch_Create([DbField] int CampaignId)
         {
             return (int)base.Execute(new object[] { CampaignId });
         }

         [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Batch_Create_b")]
         internal object Campaigns_Batch_Create([DbField] int CampaignId, [DbField] int AccountId, [DbField] int MtId, [DbField] int BillType)
         {
             return (int)base.Execute(new object[] { CampaignId, AccountId, MtId, BillType });
         }

         [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Batch_Insert_b")]
         public int Campaign_Batch_Insert
             (
             [DbField(DalParamType.Key)] int CampaignId,
             [DbField(DalParamType.Key)] int BatchId,
             [DbField] int BatchCount,
             [DbField] int BatchStatus,
             [DbField] DateTime BatchTime,
             [DbField] int BatchIndex,
             [DbField] int BatchRange,
             [DbField] int BatchType,
             [DbField] int UserId,
             [DbField] int Server,
             [DbField] int Units,
             [DbField] decimal DefaultPrice,
             [DbField] bool IsPending

             )
         {
             return (int)base.Execute(new object[] { CampaignId, BatchId, BatchCount, BatchStatus, BatchTime, BatchIndex, BatchRange, BatchType, UserId, Server,Units, DefaultPrice, IsPending });
         }
         
         [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Batch_Enqueue_b")]
         public int Campaign_Batch_Enqueue
             (
             [DbField(DalParamType.Key)] int BatchId,
             [DbField] int ItemUnits,
             [DbField] decimal ItemPrice,
             [DbField] string Args

             )
         {
             return (int)base.Execute(new object[] { BatchId, ItemUnits, ItemPrice, Args });
         }
        */

        [DBCommand(DBCommandType.StoredProcedure, "sp_Scheduler_Enqueue_b")]
         public int Scheduler_Enqueue
             (
             [DbField] int ItemId,
             [DbField] int ItemType,
             [DbField] int ItemsCount,
             [DbField] int ItemIndex,
             [DbField] int ItemRange,
             [DbField] decimal ItemPrice,
             [DbField] string DataSource,
             [DbField] DateTime ExecTime,
             [DbField] int AccountId,
             [DbField] int ArgId,
             [DbField] string Args,
             [DbField] int UserId,
             [DbField] int Server,
             [DbField] int MtId,
             [DbField] int Units

             )
         {
             return (int)base.Execute(new object[] { ItemId, ItemType, ItemsCount, ItemIndex, ItemRange, ItemPrice, 
                 DataSource, ExecTime, AccountId, ArgId,Args,UserId, Server,MtId,Units });
         }


 
          [DBCommand(DBCommandType.Insert, "Trans_Batch")]
         public int Trans_Batch_Insert
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
             [DbField] string PublishKey
             )
         {
             object[] values = new object[] { BatchId, AccountId, CampaignId, BatchCount, BatchStatus, BatchTime, BatchIndex, BatchRange, BatchType, UserId, Server, Platform, MtId, DefaultPrice, PublishKey };
             int res= (int)base.Execute(values);
             BatchId = Types.ToInt(values[0]);
             return res;
         }

          public int Trans_Batch_Insert
              (
                ref int BatchId,
                int AccountId,
                int CampaignId,
                int BatchCount,
                BatchTypes BatchType,
                int UserId,
                int Server,
                PlatformType Platform,
                int MtId,
                decimal DefaultPrice,
                string PublishKey
              )
          {
              return Trans_Batch_Insert(ref BatchId, AccountId, CampaignId, BatchCount, 0, DateTime.Now, 0, 0, (int)BatchType, UserId, Server, (int)Platform, MtId, DefaultPrice, PublishKey);
          }


      [DBCommand(DBCommandType.StoredProcedure, "sp_Publish_Comment")]
          public int Publish_Comments_Insert
             (
             [DbField]string PublishKey,
             [DbField] string SessionId,
             [DbField] int ItemId,
             [DbField] int AckStatus,
             [DbField] string PublishState,
             [DbField] string PublishComment,
             [DbField] int Server
             //[DbField] DateTime Creation
            )
         {
             return (int)base.Execute(PublishKey, SessionId, ItemId, AckStatus, PublishState, PublishComment, Server);
         }

      //public int Publish_Comments_Insert(object[] values)
      //{
      //    SqlParameter[] parameters = DalUtil.CreateParameters(new string[]{"PublishKey","ItemId","AckStatus","PublishState","PublishComment","SessionId","Server"},values);
      //    return base.ExecuteNonQuery("sp_Publish_Comment", parameters, CommandType.StoredProcedure);
      //}
        #endregion

        #region trans process
        /*
         [DBCommand(DBCommandType.Delete, "Trans_Batch")]
         public int Trans_Batch_Delete
             (
             [DbField(DalParamType.Key)] int CampaignId,
             [DbField(DalParamType.Key)] int BatchId
             )
         {
             return (int)base.Execute(new object[] { CampaignId, BatchId });
         }
        */

         [DBCommand(DBCommandType.StoredProcedure, "sp_Trans_Batch_Update_b")]
         public int Trans_Batch_Update
             (
             //[DbField(DalParamType.Key)] int CampaignId,
             [DbField(DalParamType.Key)] int BatchId,
             [DbField()] DateTime BatchTime,
             [DbField()] int ActionType//1=update time,2=remove batch
             )
         {
             return (int)base.Execute(new object[] { BatchId, BatchTime, ActionType });
         }



         [DBCommand(DBCommandType.Update, "Trans_Batch_Process")]
         public int Trans_Batch_Process_Update
             (
             [DbField(DalParamType.Key)] int BatchId,
             [DbField] int Canceled,
             [DbField] int Server)
         {
             return (int)base.Execute(new object[] { BatchId, Canceled, Server });
         }


         [DBCommand(DBCommandType.StoredProcedure, "sp_Trans_Process_Exec_b")]
         public int Trans_Process_Exec
             (
            [DbField] int BatchId
            , [DbField] int CampaignId
            , [DbField] int AccountId
            , [DbField] int BatchCount
            , [DbField] int Success
            , [DbField] int Failed
            , [DbField] int Canceled
            , [DbField] int Status
            , [DbField] int Units
            , [DbField] decimal DefaultPrice
            , [DbField] int Platform
            , [DbField] int MtId
            , [DbField] int Server
            , [DbField] int ProcessType
            , [DbField] int UserId
            , [DbField] string PublishKey
            //, [DbField] bool IsPending
           )
         {
             return (int)base.Execute(new object[] { BatchId, CampaignId, AccountId, BatchCount, Success, Failed, Canceled, Status, Units, DefaultPrice, Platform, MtId, Server, ProcessType, UserId, PublishKey });
         }

        /*
         public int Trans_Batch_State
             (
                [DbField(DalParamType.Key)] int BatchId,
                [DbField()] int CampaignId,
                [DbField()] int BatchExecState,
                [DbField()] int Server,
                [DbField()] string Comment
             )
         {
             return Trans_Batch_State(BatchId, CampaignId, BatchExecState, Server, Comment, 0, 0, 0);
         }

         [DBCommand(DBCommandType.StoredProcedure, "sp_Trans_Batch_State_b")]
         public int Trans_Batch_State
             (
                [DbField(DalParamType.Key)] int BatchId,
                [DbField()] int CampaignId,
                [DbField()] int BatchExecState,
                [DbField()] int Server,
                [DbField()] string Comment,
                [DbField()] int BatchStatus,
                [DbField()] int AccountId,
                [DbField()] int MtId
             )
         {
             return (int)base.Execute(new object[] { BatchId, CampaignId, BatchExecState, Server, Comment, BatchStatus, AccountId, MtId });
         }
        */
         [DBCommand(DBCommandType.StoredProcedure, "sp_Trans_Batch_State_c")]
         public int Trans_Batch_State
             (
                [DbField()] int BatchId,
                [DbField()] int BatchExecState,
                [DbField()] int Server
             )
         {
             return (int)base.Execute(new object[] { BatchId, BatchExecState, Server });
         }


         [DBCommand(DBCommandType.StoredProcedure, "sp_Trans_Batch_Command_c")]
         public int Trans_Batch_Command
             (
                [DbField()] int Id,
                [DbField()] string Cmd,
                [DbField()] DateTime ExecTime,
                [DbField()] int Server,
                [DbField()] int UserId
             )
         {
             return (int)base.Execute(new object[] { Id, Cmd, ExecTime, Server, UserId });
         }

        #endregion

        #region campaigns

         [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Finally_b")]
         public int Campaigns_Finally([DbField] int CampaignId, [DbField] int BatchId, [DbField] int Platform)
         {
             return (int)base.Execute(new object[] { CampaignId, BatchId, Platform });
         }

         [DBCommand(DBCommandType.StoredProcedure, "sp_Campaigns_Exec_b")]
         public DataRow Campaigns_Execute(int CampaignId, int BatchId, int Server, int UserId)
         {
             return (DataRow)base.Execute(new object[] { CampaignId, BatchId, Server, UserId });
         }

         [DBCommand(DBCommandType.StoredProcedure, "sp_Campaigns_Exec_b")]
         public DataRow Campaigns_Execute(int CampaignId, int BatchId, int Server, int UserId, bool ValidateExists)
         {
             return (DataRow)base.Execute(new object[] { CampaignId, BatchId, Server, UserId, ValidateExists });
         }

        #endregion
        
        #region Scheduler

        
        [DBCommand(DBCommandType.StoredProcedure, "sp_Scheduler_DeQueue")]
        public DataRow Scheduler_DeQueue
        (
          [DbField()]  int Server
        )
        {
            return (DataRow)base.Execute(Server);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Scheduler_DeCompleted")]
        public int Scheduler_DeCompleted
      (
        [DbField()]  Guid QueueId
      )
        {
            return (int)base.Execute(QueueId);
        }
        [DBCommand(DBCommandType.StoredProcedure, "sp_Scheduler_DeCompleted")]
        public int Scheduler_DeCompleted
        (
          [DbField()]  Guid QueueId,
          [DbField()]  int State
        )
        {
            return (int)base.Execute(QueueId, State);
        }
       
        [DBCommand("select * from vw_Reminder_Targets where ReminderId=@ReminderId")]
        public DataTable Reminder_Targets(int ReminderId)
        {
            return (DataTable)base.Execute(ReminderId);
        }


        [DBCommand(DBCommandType.StoredProcedure, "sp_Reminder_Completed")]
        public void Reminder_Completed
        (
           [DbField()]int ReminderId,
           [DbField()]string QueueId,
           [DbField()]int TransId
            //[DbField()]DateTime SentTime
        )
        {
            base.Execute(ReminderId, QueueId, TransId);
        }
         
        #endregion

    }

}

