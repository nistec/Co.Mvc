using System;
using System.Data;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using Nistec.Data;
using Nistec.Data.SqlClient;
using System.Data.SqlClient;
using System.Collections.Generic;
using Nistec;
using System.Text;
using Nistec.Data.Factory;

namespace Netcell.Data.Reports
{

    public class DalReports : DbCommand
    {
        public DalReports()
            : base(Netcell.Data.DBRule.CnnEphone)
        {
        }

        public static DalReports Instance
        {
            get { return new DalReports(); }
        }

        public void ActiveConnectionClose()
        {
            base.ConnectionClose();
        }

        [DataObjectMethod(DataObjectMethodType.Select), DBCommand(DBCommandType.StoredProcedure, "sp_Accounts_Balance")]
        public DataTable Accounts_Balance
            (
            [DbField] int AccountId,
            [DbField] DateTime DateFrom,
            [DbField] DateTime DateTo,
            [DbField] int BalanceType,
            [DbField] int FilterType)
        {
            return (DataTable)base.Execute(new object[] { AccountId, DateFrom, DateTo, BalanceType, FilterType });
        }

        [DataObjectMethod(DataObjectMethodType.Select), DBCommand(DBCommandType.StoredProcedure, "sp_Account_Credit_Report")]
        public DataTable Account_Credit_Report
            (
            [DbField] int AccountId
            )
        {
            return (DataTable)base.Execute(new object[] { AccountId });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Report_b"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Campaigns_Report
            (
            [DbField] int ReportType,
            [DbField] int AccountId,
            [DbField] int CampaignId,
            [DbField] DateTime DateFrom,
            [DbField] DateTime DateTo,
            [DbField] int Platform,
            [DbField] int BillType)
        {
            return (DataTable)base.Execute(new object[] { ReportType, AccountId, CampaignId, DateFrom, DateTo, Platform, BillType });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_List_b"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Campaign_List
            (
            [DbField] int AccountId,
            [DbField] int Status,
            [DbField] int Platform,
            [DbField] int Top)
        {
            return (DataTable)base.Execute(new object[] { AccountId, Status, Platform, Top });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Report_Items_b")]
        public DataTable CampaignsItems_Details
        (
        [DbField]int CampaignId,
        [DbField]int Platform,
        [DbField]bool IsPending,
        [DbField]int StatusMode,
        [DbField]bool EnableNotif
        )
        {
            return (DataTable)base.Execute(new object[] { CampaignId, Platform, IsPending, StatusMode,EnableNotif });
        }
         

        [DBCommand(DBCommandType.StoredProcedure, "sp_Trans_Batch_Sent_Report_c"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Trans_Batch_Sent_Report
            (
            [DbField] int ReportType,
            [DbField] int AccountId,
            [DbField] DateTime DateFrom,
            [DbField] DateTime DateTo,
            [DbField] int Platform)
        {
            return (DataTable)base.Execute(new object[] { ReportType,AccountId, DateFrom, DateTo, Platform });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Trans_Batch_Items_Report_c"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Trans_Batch_Items_Report
            (
            [DbField] int BatchId,
            [DbField] int Platform,
            [DbField] bool EnableNotif)
        {
            return (DataTable)base.Execute(new object[] { BatchId, Platform, EnableNotif });
        }

        /*
        //change:Trans_Batch_View
        [DBCommand(DBCommandType.StoredProcedure, "sp_Trans_Item_View_c"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataRow Trans_Item_View
            (
            [DbField] int MessageId
            )
        {
            return (DataRow)base.Execute(new object[] { MessageId });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Trans_Item_View_c"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Trans_Item_View_Table
            (
            [DbField] int MessageId
            )
        {
            return (DataTable)base.Execute(new object[] { MessageId });
        }
        */

        [DBCommand(DBCommandType.StoredProcedure, "sp_Trans_Item_View_d"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataRow Trans_Item_View_Row
            (
            [DbField] int MessageId
            )
        {
            return (DataRow)base.Execute(new object[] { MessageId });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Trans_Item_View_d"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Trans_Item_View_Table
            (
            [DbField] int MessageId
            )
        {
            return (DataTable)base.Execute(new object[] { MessageId });
        }

        /*
        //change:Trans_Batch_View
        public string Lookup_Trans_Batch_View_Body(int BatchId)
        {
            return base.LookupQuery<string>("Body", "Trans_Batch_View", "BatchId=@BatchId", "", new object[] { BatchId });
        }
        */
        public string Lookup_Trans_Batch_Content_Body(int BatchId)
        {
            return base.LookupQuery<string>("Body", "Trans_Batch_Content", "BatchId=@BatchId", "", new object[] { BatchId });
        }
        public string Lookup_Trans_Batch_Content_Message(int BatchId)
        {
            return base.LookupQuery<string>("Message", "Trans_Batch_Content", "BatchId=@BatchId", "", new object[] { BatchId });
        }

        [DataObjectMethod(DataObjectMethodType.Select), DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Reply_Report_c")]
        public DataTable Campaigns_Reply_Rpt
            (
            [DbField] int ReportType,
            [DbField] int Platform,
            [DbField] int AccountId,
            [DbField] int CampaignId,
            [DbField] DateTime DateFrom,
            [DbField] DateTime DateTo)
        {
            return (DataTable)base.Execute(new object[] { ReportType, Platform, AccountId, CampaignId, DateFrom, DateTo });
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand(DBCommandType.StoredProcedure, "sp_Reports_Find_Items_b")]
        public DataTable Reports_Find_Items
        (
        [DbField()]int AccountId,
        [DbField()]string Target,
        [DbField()]int Platform,
        [DbField()]DateTime DateFrom,
        [DbField()]DateTime DateTo,
        [DbField()]bool EnableNotif
        )
        {
            return (DataTable)base.Execute(AccountId, Target, Platform, DateFrom, DateTo, EnableNotif);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand(DBCommandType.StoredProcedure, "sp_Contacts_Items_Trans_Report")]
        public DataTable Contacts_Items_Trans_Report
        (
        [DbField()]int AccountId,
        [DbField()]int Platform,
        [DbField()]DateTime DateFrom,
        [DbField()]DateTime DateTo,
        [DbField()]bool EnableNotif,
        [DbField()]string Target
        )
        {
            return (DataTable)base.Execute(AccountId, Platform, DateFrom, DateTo, EnableNotif, Target);
        }

        [DataObjectMethod(DataObjectMethodType.Select), DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Statistic_Report_b")]
        public DataTable Campaigns_Statistic_Report
            (
            [DbField] int ReportType,
            [DbField] int CampaignId,
            [DbField] int AccountId)
        {
            return (DataTable)base.Execute(new object[] { ReportType, CampaignId, AccountId });
        }

        #region Mail

        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand("SELECT * from [Campaigns_Mail_Restricted] where AccountId=@AccountId")]
        public DataTable Campaigns_Mail_Restricted(int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }

        #endregion

        #region Registry

        public DataTable Registry_Items(int FormId, int Status, string DateFrom, string DateTo)
        {
            string sql = "";
            string statusCase = "case r.Status when 2 then N'נסגר' when 1 then N'מעקב' else N'ממתין' end as StatusName";
            if (Status == 9)
                sql = string.Format("SELECT r.*," + statusCase + " FROM Registry_Items r where r.FormId={0} and r.Creation between '{1}' and '{2}'", FormId, DateFrom, DateTo);
            else
                sql = string.Format("SELECT r.*," + statusCase + " FROM Registry_Items r where r.FormId={0} and r.Status={1} and r.Creation between '{2}' and '{3}'", FormId, Status, DateFrom, DateTo);

            return (DataTable)base.ExecuteCommand<DataTable>(sql);
        }

        [DBCommand(@"SELECT * FROM Registry_Items where FormId=@FormId and Creation between @DateFrom and @DateTo")]
        public DataTable Registry_Items(int FormId, string DateFrom, string DateTo)
        {
            return (DataTable)base.Execute(FormId, DateFrom, DateTo);
        }

        [DBCommand(@"SELECT * FROM Registry_Form where AccountId=@AccountId and State=@State")]
        public DataTable Registry_Form(int AccountId, int State)
        {
            return (DataTable)base.Execute(AccountId, State);
        }

        #endregion

        #region accounts Site

        [DataObjectMethod(DataObjectMethodType.Select), DBCommand("SELECT * from [Sites] where AccountId=@AccountId  order by SiteId DESC")]
        public DataTable Sites_Reports(int AccountId)
        {
            return (DataTable)base.Execute(new object[] { AccountId });
        }

        [DBCommand(DBCommandType.Delete, "Sites")]
        public int Sites_Remove(int SiteId)
        {
            if (SiteId <= 0)
            {
                return 0;
            }
            return base.ExecuteNonQuery(string.Format("Delete from Sites_Pages where SiteId={0}", SiteId));

            return base.ExecuteNonQuery(string.Format("Delete from Sites where SiteId={0}", SiteId));
        }

      
        #endregion

        #region Reminder

        [DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Reminder_Report(int AccountId, DateTime DateFrom, DateTime DateTo, int Mode)
        {
            if (Mode == 2)
            {
                return this.Reminder_Report_History(AccountId, DateFrom, DateTo);
            }
            return this.Reminder_Report_Pending(AccountId);
        }

        [DataObjectMethod(DataObjectMethodType.Select), DBCommand("SELECT * from [Reminder] where AccountId=@AccountId and CreationDate between @DateFrom and @DateTo")]
        public DataTable Reminder_Report_History(int AccountId, DateTime DateFrom, DateTime DateTo)
        {
            return (DataTable)base.Execute(new object[] { AccountId, DateFrom, DateTo });
        }

        [DBCommand("SELECT * from [Reminder] where AccountId=@AccountId and DateToSend > getdate()"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Reminder_Report_Pending(int AccountId)
        {
            return (DataTable)base.Execute(new object[] { AccountId });
        }

        [DBCommand("SELECT Reminder_Targets.*,'' as SentTime from [Reminder_Targets] where ReminderId=@ReminderId"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Reminder_Targets(int ReminderId)
        {
            return (DataTable)base.Execute(new object[] { ReminderId });
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Reminder_Targets(int ReminderId, int Mode)
        {
            if (Mode == 2)
            {
                return this.Reminder_Targets_History(ReminderId);
            }
            return this.Reminder_Targets(ReminderId);
        }

        [DataObjectMethod(DataObjectMethodType.Select), DBCommand("SELECT * from [vw_Reminder_History] where ReminderId=@ReminderId")]
        public DataTable Reminder_Targets_History(int ReminderId)
        {
            return (DataTable)base.Execute(new object[] { ReminderId });
        }

        #endregion

        #region Campaign watch

        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaigns_Watch_Remove")]
        public int Campaigns_Watch_Remove(int CampaignId)
        {
            return (int)base.Execute(new object[] { CampaignId });
        }

        [DBCommand("SELECT * from [vw_Campaigns_Watch] where AccountId=@AccountId AND MtId=@MtId order by CampaignId DESC"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Campaigns_Watch_View(int AccountId, int MtId)
        {
            return (DataTable)base.Execute(new object[] { AccountId, MtId });
        }

        [DBCommand("SELECT * from [vw_Campaigns_Watch] where AccountId=@AccountId AND [Platform]=@Platform order by CampaignId DESC"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Campaigns_Watch_View_Platform(int AccountId, int Platform)
        {
            return (DataTable)base.Execute(new object[] { AccountId, Platform });
        }

        [DBCommand("SELECT * from [Scheduler_Queue_History] where AccountId=@AccountId AND ItemId=@ItemId and DataSource=@DataSource order by Executed DESC"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Scheduler_Queue_History(int AccountId, int ItemId, string DataSource)
        {
            return (DataTable)base.Execute(new object[] { AccountId, ItemId, DataSource });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Watch_Registry"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Campaign_Watch_Registry
            (
            [DbField] int CampaignId
            )
        {
            return (DataTable)base.Execute(new object[] { CampaignId });
        }
        #endregion

        #region Campaign templates

        //[DBCommand(DBCommandType.Delete, "Campaigns_Templates")]
        //public int Campaigns_Templates_Remove(int CampaignId, bool IsDraft)
        //{
        //    if (CampaignId <= 0)
        //    {
        //        return 0;
        //    }
        //    if (IsDraft)
        //    {
        //        return base.ExecuteNonQuery(string.Format("Update Campaigns SET IsDraft=0 where CampaignId={0}", CampaignId));
        //    }
        //    return base.ExecuteNonQuery(string.Format("Delete from Campaigns_Templates  where CampaignId={0}", CampaignId));
        //}

        //[DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Templates_View")]
        //public DataTable Campaigns_Templates_View
        //(
        //[DbField]int AccountId,
        //[DbField]int Platform,
        //[DbField]string CampaignType
        //)
        //{
        //    return (DataTable)base.Execute(new object[] { AccountId, Platform, CampaignType });
        //}


        #endregion

        public int Monitor_Exists(int CampaignId, DateTime StartTime)
        {
            return base.ExecuteCommand<int>("sp_Monitor_Exists_b", DataParameter.GetSql("CampaignId", CampaignId, "StartTime", StartTime), CommandType.StoredProcedure);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Monitor_All_b")]
        public DataTable Monitor_All
            (
            [DbField] int OId,
            [DbField] int OType,
            [DbField] int RType
            )
        {
            return (DataTable)base.Execute(new object[] { OId, OType, RType });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Monitor_Queue_c")]
        public DataTable Monitor_Queue
            (
            [DbField] int CampaignId,
            [DbField] DateTime StartTime,
            [DbField] string PublishKey
            )
        {
            return (DataTable)base.Execute(new object[] { CampaignId, StartTime, PublishKey });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_MonitorView_b")]
        public DataTable Monitor_View
            (
            [DbField] int OId,
            [DbField] int OType,
            [DbField] int RType
            )
        {
            return (DataTable)base.Execute(new object[] { OId, OType, RType });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Monitor_ProcessView_b")]
        public DataTable Monitor_Process
            (
            [DbField] int OId,
            [DbField] int OType,
            [DbField] int RType
            )
        {
            return (DataTable)base.Execute(new object[] { OId, OType, RType });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Monitor_Scheduler_b")]
        public DataTable Monitor_Scheduler
            (
            [DbField] int OId,
            [DbField] int OType,
            [DbField] int RType
            )
        {
            return (DataTable)base.Execute(new object[] { OId, OType, RType });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Monitor_Scheduler_b")]
        public DataTable Monitor_Scheduler
            (
            [DbField] int OId,
            [DbField] int OType,
            [DbField] int RType,
            [DbField] string DataSource
            )
        {
            return (DataTable)base.Execute(new object[] { OId, OType, RType, DataSource });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Contacts_Upload_Monitor")]
        public DataTable Monitor_Contacts_Upload
            (
            [DbField] string SessionId,
            //[DbField] DateTime StartTime,
            [DbField] string Lang
            )
        {
            return (DataTable)base.Execute(new object[] { SessionId, Lang });
        }


        [DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Monitor_Contacts_Upload_Async
            (
            string SessionId,
            string Lang,
            DateTime StartTime,
            int Duration,
            int State,
            int MaxState
            )
        {

            if (State >= MaxState || DateTime.Now.Subtract(StartTime).Minutes > Duration)
                return null;
            var dt= (DataTable)Monitor_Contacts_Upload(SessionId, Lang);
            return dt;
        }

#if(false)

        [DataObjectMethod(DataObjectMethodType.Select), DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Process")]
        public DataRow Campaigns_Process([DbField] int CampaignId, [DbField] int BatchId, [DbField] int Platform, [DbField] bool CheckState)
        {
            return (DataRow)base.Execute(new object[] { CampaignId, BatchId, Platform, CheckState });
        }

       

        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Report"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Campaigns_Report
            (
            [DbField] int ReportType,
            [DbField] int AccountId,
            [DbField] int CampaignId,
            [DbField] DateTime DateFrom,
            [DbField] DateTime DateTo,
            [DbField] int Platform,
            [DbField] int BillType)
        {
            return (DataTable)base.Execute(new object[] { ReportType, AccountId, CampaignId, DateFrom, DateTo, Platform, BillType });
        }


   
 
 
  
       


        [DBCommand(DBCommandType.StoredProcedure, "sp_Find_CampaignItems")]
        public DataTable Campaigns_Items_Find
        (
        int AccountId,
        int CampaignId,
        string Target,
        DateTime DateFrom,
        DateTime DateTo,
        int Platform,
        bool IsPending,
        int UserType
        )
        {
            return (DataTable)base.Execute(new object[] { AccountId, CampaignId, Target, DateFrom, DateTo, Platform, IsPending, UserType });
        }

        public string GetMessageCB(int MessageId)
        {
            return base.Dlookup("MessageText", "Trans_CB", "MessageId=" + MessageId.ToString(), "<None>");
        }

        public string GetUrlCB(int MessageId)
        {
            return base.Dlookup("Url", "Trans_CB", "MessageId=" + MessageId.ToString(), "<None>");
        }

        

        [DBCommand(DBCommandType.StoredProcedure, "sp_Report_CB"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Reports_CB
            (
            [DbField] int ReportType,
            [DbField] int AccountId,
            [DbField] DateTime DateFrom,
            [DbField] DateTime DateTo, 
            [DbField] int MtId, 
            [DbField] int TransId)
        {
            return (DataTable)base.Execute(new object[] { ReportType, AccountId, DateFrom, DateTo, MtId, TransId });
        }

       

    
        [DBCommand("SELECT * from [vw_Trans_CB] where TransId=@TransId"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Trans_CB(int TransId)
        {
            return (DataTable)base.Execute(new object[] { TransId });
        }

        [DBCommand("SELECT * from [vw_Trans_CB] where TransId=@TransId and SendTime between @DateFrom and @DateTo"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Trans_CB(int TransId, DateTime DateFrom, DateTime DateTo)
        {
            return (DataTable)base.Execute(new object[] { TransId, DateFrom, DateTo });
        }

        [DataObjectMethod(DataObjectMethodType.Select), DBCommand("SELECT * from [vw_Trans_CB] where TransId=@TransId and SendTime between @DateFrom and @DateTo and MtId=@MtId")]
        public DataTable Trans_CB(int TransId, DateTime DateFrom, DateTime DateTo, int MtId)
        {
            return (DataTable)base.Execute(new object[] { TransId, DateFrom, DateTo, MtId });
        }

        [DBCommand("SELECT * from [vw_Trans_CB_NotifInfo] where MessageId=@MessageId")]
        public DataRow Trans_CB_NotifInfo(int MessageId)
        {
            return (DataRow)base.Execute(new object[] { MessageId });
        }

        [DBCommand("SELECT * from [Trans_CB] where MessageId=@MessageId")]
        public DataRow Trans_CB_Row(int MessageId)
        {
            return (DataRow)base.Execute(new object[] { MessageId });
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Trans_Details(int AccountId, int TransId, string CellNumber, DateTime DateFrom, DateTime DateTo, int Method)
        {
            string str = (Method <= 0) ? "" : string.Format(" and MtId={0}", Method);
            string str2 = string.IsNullOrEmpty(CellNumber) ? "" : string.Format(" and MsgTo='{0}'", CellNumber);
            string str3 = "vw_Trans_CB";
            if (TransId > 0)
            {
                return base.ExecuteCommand<DataTable>(string.Format("SELECT * from [" + str3 + "] where TransId={0}{1}{2}", TransId, str2, str));
            }
            return base.ExecuteCommand<DataTable>(string.Format("SELECT * from [" + str3 + "] where AccountId={0} and SendTime between '{1}' and '{2}'{3}{4}", new object[] { AccountId, DateFrom.ToString("s"), DateTo.ToString("s"), str2, str }));
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Trans_Find(int AccountId, string CellNumber, DateTime DateFrom, DateTime DateTo)
        {
            return base.ExecuteCommand<DataTable>(string.Format("SELECT * from [vw_Trans_CB] where AccountId={0} and MsgTo LIKE '%{1}%' and SendTime between '{2}' and '{3}'", new object[] { AccountId, CellNumber, DateFrom.ToString("s"), DateTo.ToString("s") }));
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Trans_Find(int AccountId, string CellNumber, DateTime DateFrom, DateTime DateTo, int MtId)
        {
            return base.ExecuteCommand<DataTable>(string.Format("SELECT * from [vw_Trans_CB] where AccountId={0} and MsgTo LIKE '%{1}%' and SendTime between '{2}' and '{3}' and MtId={4}", new object[] { AccountId, CellNumber, DateFrom.ToString("s"), DateTo.ToString("s"), MtId }));
        }

        public DataTable Trans_FindLIKE(int AccountId, string CellNumber, DateTime DateFrom, DateTime DateTo)
        {
            return base.ExecuteCommand<DataTable>(string.Format("SELECT * from [vw_Trans_CB] where AccountId={0} and MsgTo LIKE '%{1}%' and SendTime between '{2}' and '{3}'", new object[] { AccountId, CellNumber, DateFrom.ToString("s"), DateTo.ToString("s") }));
        }

        public DataTable Trans_FindLIKE(int AccountId, string CellNumber, DateTime DateFrom, DateTime DateTo, int MtId)
        {
            return base.ExecuteCommand<DataTable>(string.Format("SELECT * from [vw_Trans_CB] where AccountId={0} and MsgTo LIKE '%{1}%' and SendTime between '{2}' and '{3}' and MtId={4}", new object[] { AccountId, CellNumber, DateFrom.ToString("s"), DateTo.ToString("s"), MtId }));
        }

        [DataObjectMethod(DataObjectMethodType.Select), DBCommand("SELECT * from [Trans_CB] where MessageId=@MessageId")]
        public DataTable Trans_Message_CB(int MessageId)
        {
            return (DataTable)base.Execute(new object[] { MessageId });
        }

        [DataObjectMethod(DataObjectMethodType.Select), DBCommand("SELECT * from [Trans_Process] where AccountId=@AccountId")]
        public DataTable Trans_Process(int AccountId)
        {
            return (DataTable)base.Execute(new object[] { AccountId });
        }

        [DBCommand("SELECT * from [vw_Trans_Process]")]
        public DataTable Trans_Process_Admin()
        {
            return (DataTable)base.Execute();
        }

        [DBCommand("SELECT * from [vw_Trans_Process_All]")]
        public DataTable Trans_Process_All()
        {
            return (DataTable)base.Execute();
        }

        [DBCommand("SELECT * from [vw_Trans_Process_All] where OwnerId=@OwnerId")]
        public DataTable Trans_Process_All_Owners(int OwnerId)
        {
            return (DataTable)base.Execute(new object[] { OwnerId });
        }

        [DBCommand("SELECT * from [vw_Trans_Process_All] where ParentId=@ParentId")]
        public DataTable Trans_Process_All_Parents(int ParentId)
        {
            return (DataTable)base.Execute(new object[] { ParentId });
        }

        [DBCommand("SELECT * from [Trans_Process] where CampaignId=@CampaignId")]
        public DataRow Trans_Process_Campaign(int CampaignId)
        {
            return (DataRow)base.Execute(new object[] { CampaignId });
        }

        [DataObjectMethod(DataObjectMethodType.Select), DBCommand("SELECT * from [vw_Trans_Process_Union] where AccountId=@AccountId")]
        public DataTable Trans_Process_Union(int AccountId)
        {
            return (DataTable)base.Execute(new object[] { AccountId });
        }

        [DBCommand("SELECT * from [vw_Trans_Process_Union] where CampaignId=@CampaignId and BatchId=@BatchId")]
        public DataRow Trans_Process_Union(int CampaignId, int BatchId)
        {
            return (DataRow)base.Execute(new object[] { CampaignId, BatchId });
        }

 
       

        [DBCommand("SELECT * from [Trans_Completed] where TransId=@TransId")]
        public DataTable TransCompleted(int TransId)
        {
            return (DataTable)base.Execute(new object[] { TransId });
        }

        [DataObjectMethod(DataObjectMethodType.Select), DBCommand("SELECT * from [vw_TransCompleted] where AccountId=@AccountId and DateIndex between @DateFrom and @DateTo ORDER BY TransId DESC")]
        public DataTable TransCompleted(int AccountId, int DateFrom, int DateTo)
        {
            return (DataTable)base.Execute(new object[] { AccountId, DateFrom, DateTo });
        }

        [DataObjectMethod(DataObjectMethodType.Select), DBCommand("SELECT * from [vw_TransCompleted] where AccountId=@AccountId and DateIndex between @DateFrom and @DateTo and MtId=@MtId ORDER BY TransId DESC")]
        public DataTable TransCompleted(int AccountId, int DateFrom, int DateTo, int MtId)
        {
            return (DataTable)base.Execute(new object[] { AccountId, DateFrom, DateTo, MtId });
        }

#endif
     
    }

}
