using System;
using System.Data;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using System.Data.SqlClient;
using System.Text;
using Netcell.Data;
using Nistec.Data.SqlClient;
using Nistec.Data;
using Nistec.Data.Factory;
using Nistec;

namespace Netcell.Data.Db
{

    public class DalCampaign : DbCommand
    {


        #region ctor

        public DalCampaign()
            : base(DBRule.CnnNetcell)
        {

        }
        public static DalCampaign Instance
        {
            get
            {
                return new DalCampaign();
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

            IDbCmd cmd = DbFactory.Create(DBRule.CnnNetcell, DBProvider.SqlServer);
            return cmd.Adapter.UpdateChanges(dt.GetChanges(), destinationTable);
        }

        public int DeleteFromTable(string tableName, string primaryKeyName, int primaryKey)
        {
            string sql = string.Format("delete from {0} where {1}={2}", tableName, primaryKeyName, primaryKey);
            return base.ExecuteNonQuery(sql);
        }
        #endregion

      

        [DBCommand("SELECT AccountId,ContactCapacity,ContentCapacity from [Accounts_Features] where AccountId=@AccountId")]
        public DataRow Accounts_Capacity(int AccountId)
        {
            return (DataRow)base.Execute(new object[] { AccountId });
        }

        [DBCommand("SELECT AccountDetails from [vw_Accounts_Details] where AccountId=@AccountId")]
        public object Accounts_Details(int AccountId)
        {
            return base.Execute(new object[] { AccountId });
        }

        [DBCommand("SELECT * from [Accounts_Features] where AccountId=@AccountId")]
        public DataRow Accounts_Features(int AccountId)
        {
            return (DataRow)base.Execute(new object[] { AccountId });
        }

 
        [DBCommand("SELECT * from Campaigns c inner join Campaigns_Watch w on c.CampaignId=w.CampaignId where c.CampaignId=@CampaignId")]
        public DataRow Campaign_Watch(int CampaignId)
        {
            return (DataRow)base.Execute(new object[] { CampaignId });
        }

        [DBCommand("SELECT * from [Campaigns]")]
        public DataTable Campaigns()
        {
            return (DataTable)base.Execute();
        }

        [DBCommand("SELECT * from [Campaigns] where CampaignId=@CampaignId")]
        public DataRow Campaigns(int CampaignId)
        {
            return (DataRow)base.Execute(new object[] { CampaignId });
        }
 
        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Batch_Insert")]
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
            [DbField] bool IsPending,
            [DbField] int AccountId

            )
        {
            return (int)base.Execute(new object[] { CampaignId, BatchId, BatchCount, BatchStatus, BatchTime, BatchIndex, BatchRange, BatchType, UserId, Server,IsPending,AccountId });
        }

 
        [DBCommand("SELECT c.* from [Campaigns_Comment] c inner join Campaigns a\r\n        on c.CampaignId=a.CampaignId where a.AccountId=@AccountId")]
        public DataTable Campaigns_Comment(int CampaignId)
        {
            return (DataTable)base.Execute(new object[] { CampaignId });
        }

        [DBCommand(DBCommandType.Insert, "Campaigns_Comment")]
        public DataTable Campaigns_Comment_Insert([DbField] int CampaignId, [DbField] int SentId, [DbField] string BillingType, [DbField] string Comment)
        {
            return (DataTable)base.Execute(new object[] { CampaignId, SentId, BillingType, Comment });
        }

        [DBCommand(DBCommandType.Update, "Campaigns_Comment")]
        public DataTable Campaigns_Comment_Update
            (
            [DbField(DalParamType.Key)] int CommentId,
            [DbField] bool Checked
            )
        {
            return (DataTable)base.Execute(new object[] { CommentId, Checked });
        }

   
        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaigns_GetLinkItemsToSend_b")]
        public DataTable Campaigns_GetLinkItemsToSend([DbField] int CampaignId, [DbField] int Platform, [DbField] string Args, [DbField] string List)
        {
            return (DataTable)base.Execute(new object[] { CampaignId, Platform, Args, List });
        }

    
       
        [DBCommand(DBCommandType.Insert, "Campaigns")]
        public int Campaigns_Insert
            (
            [DbField(DalParamType.Identity)] ref int CampaignId, 
            [DbField] string CampaignName, 
            [DbField] int CampaignType, 
            [DbField] string CampaignPromo, 
            [DbField] int AccountId, 
            [DbField] DateTime DateToSend, 
            [DbField] DateTime ExpirationDate, 
            [DbField] int TotalCount, 
            [DbField] int Status, 
            [DbField] int DateIndex, 
            [DbField] string MessageText, 
            [DbField] string Url, 
            [DbField] int ValidTimeBegin, 
            [DbField] int ValidTimeEnd,
            [DbField] int NotifyType,
            [DbField] int BatchCount, 
            [DbField] bool IsDraft, 
            [DbField] string Sender,
            [DbField] string NotifyCells, 
            [DbField] int PersonalLength, 
            [DbField] int DesignId, 
            [DbField] string Display, 
            [DbField] string ReplyTo,
            [DbField] int BillType, 
            [DbField] string PersonalFields, 
            [DbField] string PersonalDisplay, 
            [DbField] int MtId, 
            [DbField] int CampaignProductType, 
            [DbField] string Features, 
            [DbField] int Platform
            )
        {
            object[] objArray = new object[] { 
            (int) CampaignId, CampaignName, CampaignType, CampaignPromo, AccountId, DateToSend, ExpirationDate, TotalCount, Status, DateIndex, MessageText, Url, ValidTimeBegin, ValidTimeEnd, NotifyType,BatchCount, IsDraft, 
            Sender, NotifyCells, PersonalLength, DesignId, Display, ReplyTo,BillType, PersonalFields, PersonalDisplay, MtId, CampaignProductType, Features, Platform
         };
            object obj2 = base.Execute(objArray);
            CampaignId = Types.ToInt(objArray[0], CampaignId);
            return Types.ToInt(obj2, 0);
        }

    
        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Links_Reports_b")]
        public DataTable Campaigns_Links_Reports([DbField] int CampaignId, [DbField] int Platform)
        {
            return (DataTable)base.Execute(new object[] { CampaignId, Platform });
        }

        [DBCommand(0, "SELECT * from [Campaigns_Links] where CampaignId=@CampaignId", "", MissingSchemaAction.AddWithKey)]
        public DataTable Campaigns_LinksWithSchema(int CampaignId)
        {
            return (DataTable)base.Execute(new object[] { CampaignId });
        }
  

        [DBCommand(DBCommandType.Update, "Campaigns")]
        public int Campaigns_Message_Update
            (
            [DbField(DalParamType.Key)] int CampaignId,
            [DbField] string MessageText, 
            [DbField] string Url
            )
        {
            return (int)base.Execute(new object[] { CampaignId, MessageText, Url });
        }

     
        [DBCommand(DBCommandType.Insert, "Campaigns_Queue")]
        public int Campaigns_Queue_Insert([DbField] int CampaignId, [DbField] int BatchId, [DbField] DateTime DateToSend, [DbField] DateTime Expiration)
        {
            return (int)base.Execute(new object[] { CampaignId, BatchId, DateToSend, Expiration });
        }

        [Obsolete("use View_Remove insted")]
        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Reply_Blocker")]
        public int Campaigns_Reply_Blocker([DbField] int SentId, [DbField] int Status, [DbField] int Platform, [DbField] bool IsAll, [DbField] int Version)
        {
            return (int)base.Execute(new object[] { SentId, Status, Platform, IsAll, Version });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_View_Remove_b")]
        public int View_Remove
             (
             [DbField] int SentId, 
             [DbField] int Status, 
             [DbField] int Platform, 
             [DbField] bool IsAll, 
             [DbField] int Version,
             [DbField] int AccountId,
             [DbField] string Target)
        {
            return (int)base.Execute(new object[] { SentId, Status, Platform, IsAll, Version, AccountId,Target});
        }


        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Reply_Insert")]
        public int Campaigns_Reply_Items_Insert([DbField] int SentId, [DbField] int LinkId, [DbField] int ReplyType, [DbField] int Platform, [DbField] int Version)
        {
            return (int)base.Execute(new object[] { SentId, LinkId, ReplyType, Platform, Version });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaigns_Update")]
        public int Campaigns_Save
            ([DbField(DalParamType.Identity)] ref int CampaignId, 
            [DbField] string CampaignName, 
            [DbField] int CampaignType, 
            [DbField] string CampaignPromo, 
            [DbField] int Lang, 
            [DbField] int AccountId, 
            [DbField] DateTime DateToSend,
            [DbField] DateTime ExpirationDate, 
            [DbField] int TotalCount, 
            [DbField] int Status, 
            [DbField] string MessageText,
            [DbField] string Url, 
            [DbField] int ValidTimeBegin, 
            [DbField] int ValidTimeEnd,
            [DbField] int NotifyType, 
            [DbField] int BatchCount,
            [DbField] int CouponType, 
            [DbField] int CouponSource,
            [DbField] bool Concatenate, 
            [DbField] bool IsDraft,
            [DbField] string Sender, 
            [DbField] string NotifyCells,
            [DbField] int PersonalLength, 
            [DbField] int DesignId, 
            [DbField] string Display,
            [DbField] int BillType,
            [DbField] int ProductType,
            [DbField] string PersonalFields,
            [DbField] string PersonalDisplay,
            [DbField] int MtId, 
            [DbField] string Features, 
            [DbField] int Platform,
            [DbField] int PriceCode, 
            [DbField] int AppId, 
            [DbField] int DeviceRule, 
            [DbField] int SignCount, 
            [DbField] int Interval, 
            [DbField] int MaxSession
            )
        {
            object[] objArray = new object[] { 
            (int) CampaignId, CampaignName, CampaignType, CampaignPromo, Lang, AccountId, DateToSend, ExpirationDate, TotalCount, Status, MessageText, Url, ValidTimeBegin, ValidTimeEnd, NotifyType, BatchCount, 
            CouponType, CouponSource, Concatenate, IsDraft, Sender, NotifyCells, PersonalLength, DesignId, Display, BillType, ProductType, PersonalFields, PersonalDisplay, MtId, Features, Platform, 
            PriceCode, AppId, DeviceRule, SignCount, Interval, MaxSession
         };
            object obj2 = base.Execute(objArray);
            CampaignId = Types.ToInt(objArray[0], CampaignId);
            return Types.ToInt(obj2, 0);
        }

        [DBCommand("SELECT * from [Campaigns_Scheduler] where CampaignId=@CampaignId"), Obsolete("see Scheduler")]
        public DataRow Campaigns_Scheduler(int CampaignId)
        {
            return (DataRow)base.Execute(new object[] { CampaignId });
        }

        [DBCommand(DBCommandType.Insert, "Campaigns_Scheduler"), Obsolete("see Scheduler")]
        public int Campaigns_Scheduler_Insert([DbField] int CampaignId, [DbField] int ScheduleType, [DbField] DateTime LifeTime_Start, [DbField] DateTime LifeTime_End, [DbField] DateTime LastUse, [DbField] DateTime NextUse, [DbField] bool Enabled, [DbField] int CurrentStep, [DbField] int MaxSteps, [DbField] string DaysInWeek, [DbField] string ExecTime, [DbField] int ExpirationDelta)
        {
            return (int)base.Execute(new object[] { CampaignId, ScheduleType, LifeTime_Start, LifeTime_End, LastUse, NextUse, Enabled, CurrentStep, MaxSteps, DaysInWeek, ExecTime, ExpirationDelta });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Statistic_Refresh_b")]
        public int Campaigns_Statistic_Refresh(int CampaignId, int Platform, bool RefreshReply)
        {
            return (int)base.Execute(new object[] { CampaignId, Platform, RefreshReply });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Statistic_Update_b")]
        public int Campaigns_Statistic_Update(int CampaignId, int Platform)
        {
            return (int)base.Execute(new object[] { CampaignId, Platform });
        }

        [DBCommand("SELECT StatusId,StatusHe as StatusName from [Campaigns_Status] where Section='all' or Section=@Section")]
        public DataTable Campaigns_Status_HeAll(string Section)
        {
            return (DataTable)base.Execute(new object[] { Section });
        }

        [DBCommand(0, "SELECT * from [Campaigns_Targets] where CampaignId=@CampaignId")]
        public DataTable Campaigns_Targets(int CampaignId)
        {
            return (DataTable)base.Execute(new object[] { CampaignId });
        }

        [DBCommand(DBCommandType.Delete, "Campaigns_Targets")]
        public int Campaigns_Targets_Delete([DbField(DalParamType.Key)] int CampaignId)
        {
            return (int)base.Execute(new object[] { CampaignId });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaigns_Save")]
        public int Campaigns_Template_Save([DbField] int CampaignId, [DbField] string CampaignName)
        {
            object[] objArray = new object[] { CampaignId, CampaignName };
            return Types.ToInt(base.Execute(objArray), 0);
        }

        [DBCommand(DBCommandType.Update, "Campaigns")]
        public int Campaigns_Update
            (
            [DbField(DalParamType.Key)] int CampaignId, 
            [DbField] string CampaignName,
            [DbField] int CampaignType,
            [DbField] string CampaignPromo,
            [DbField] int AccountId,
            [DbField] DateTime DateToSend,
            [DbField] DateTime ExpirationDate,
            [DbField] int TotalCount,
            [DbField] int Status, 
            [DbField] int DateIndex,
            [DbField] string MessageText,
            [DbField] string Url,
            [DbField] int ValidTimeBegin,
            [DbField] int ValidTimeEnd,
            [DbField] int NotifyType,
            [DbField] int BatchCount,
            [DbField] bool IsDraft,
            [DbField] string Sender, 
            [DbField] string NotifyCells, 
            [DbField] int PersonalLength,
            [DbField] int DesignId, 
            [DbField] string Display,
            [DbField] string ReplyTo,
            [DbField] int BillType,
            [DbField] string PersonalFields,
            [DbField] string PersonalDisplay,
            [DbField] int MtId, 
            [DbField] string Features,
            [DbField] int Platform
            )
        {
            object[] objArray = new object[] { 
            CampaignId, CampaignName, CampaignType, CampaignPromo, AccountId, DateToSend, ExpirationDate, TotalCount, Status, DateIndex, MessageText, Url, ValidTimeBegin, ValidTimeEnd, NotifyType,BatchCount, IsDraft, 
            Sender, NotifyCells, PersonalLength, DesignId, Display,ReplyTo, BillType, PersonalFields, PersonalDisplay, MtId, Features, Platform
         };
            return Types.ToInt(base.Execute(objArray), 0);
        }

        [DBCommand(DBCommandType.Update, "Campaigns")]
        public int Campaigns_UpdateSend
            (
            [DbField(DalParamType.Key)] int CampaignId,
            [DbField] int CampaignType,
            [DbField] string CampaignPromo,
            [DbField] DateTime DateToSend,
            [DbField] DateTime ExpirationDate,
            [DbField] int TotalCount,
            [DbField] int Status,
            [DbField] int DateIndex,
            [DbField] int ValidTimeBegin,
            [DbField] int ValidTimeEnd,
            [DbField] int BatchCount,
            [DbField] string Sender,
            [DbField] string NotifyCells,
            [DbField] int PersonalLength,
            [DbField] string Display,
            [DbField] string ReplyTo,
            [DbField] int NotifyType 

            //[DbField] string PersonalFields,
            //[DbField] string PersonalDisplay,
            )
        {
            object[] objArray = new object[] { 
            CampaignId, CampaignType,CampaignPromo,DateToSend, ExpirationDate, TotalCount, Status, DateIndex, ValidTimeBegin, ValidTimeEnd, BatchCount, 
            Sender, NotifyCells, PersonalLength, Display,ReplyTo,NotifyType
         };
            return Types.ToInt(base.Execute(objArray), 0);
        }
        [DBCommand("UPDATE Campaigns SET Status=@Status where CampaignId=@CampaignId")]
        public int Campaigns_UpdateStatus(int Status, int CampaignId)
        {
            return (int)base.Execute(new object[] { Status, CampaignId });
        }

       [DBCommand(DBCommandType.StoredProcedure, "sp_Campaigns_Delete_Draft")]
        public int Campaigns_Delete_Draft
        (
            [DbField] int CampaignId
        )
          {
              return (int)base.Execute(CampaignId);
          }
 
        [DBCommand("SELECT g.* from Campaigns_Watch_Groups w inner join Contacts_Items_Groups g on w.GroupId=g.ContactGroupId where CampaignId=@CampaignId")]
        public DataTable Campaigns_Watch_Groups(int CampaignId)
        {
            return (DataTable)base.Execute(new object[] { CampaignId });
        }

        [DBCommand("DELETE from [Campaigns_Watch_Groups] where CampaignId=@CampaignId")]
        public void Campaigns_Watch_Groups_Delete(int CampaignId)
        {
            base.Execute(new object[] { CampaignId });
        }

        [DBCommand(DBCommandType.Insert, "Campaigns_Watch")]
        public int Campaigns_Watch_Insert([DbField] int CampaignId, [DbField] int ReminderField)
        {
            return (int)base.Execute(new object[] { CampaignId, ReminderField });
        }

        [DBCommand(DBCommandType.Update, "Campaigns_Watch")]
        public int Campaigns_Watch_Update([DbField(DalParamType.Key)] int CampaignId, [DbField] int ReminderField)
        {
            return (int)base.Execute(new object[] { CampaignId, ReminderField });
        }

      
        [DBCommand(DBCommandType.StoredProcedure, "sp_CampaignsReminder_job")]
        public DataRow CampaignsReminder_job()
        {
            return (DataRow)base.Execute();
        }

        [DBCommand("SELECT * from [Contacts] where AccountId=@AccountId")]
        public DataTable Contacts(int AccountId)
        {
            return (DataTable)base.Execute(new object[] { AccountId });
        }

        [DBCommand("SELECT * from [vw_Contacts_ByGroup] where AccountId=@AccountId and GroupId=@GroupId")]
        public DataTable Contacts_ByGroup(int AccountId, int GroupId)
        {
            return (DataTable)base.Execute(new object[] { AccountId, GroupId });
        }

        [DBCommand("SELECT * from [vw_Contacts_Cli_Blocked] where AccountId=@AccountId")]
        public DataTable Contacts_Cli_Blocked(int AccountId)
        {
            return (DataTable)base.Execute(new object[] { AccountId });
        }

        [DBCommand("SELECT * from [Contacts_Lists] where ListId=@ListId and BatchId=@BatchId")]
        public DataTable Contacts_Listss(string ListId, int BatchId)
        {
            return (DataTable)base.Execute(new object[] { ListId, BatchId });
        }

        [DBCommand("SELECT * from [vw_Contacts_Mail_Blocked] where AccountId=@AccountId")]
        public DataTable Contacts_Mail_Blocked(int AccountId)
        {
            return (DataTable)base.Execute(new object[] { AccountId });
        }
               
 
        [DBCommand(DBCommandType.StoredProcedure, "sp_FindCampaignToSend")]
        public DataRow FindCampaignToSend()
        {
            return (DataRow)base.Execute();
        }


        public int GetFirstTransItemId(int campaignId)
        {

            return base.DMin<int>("MessageId", "Trans_Items", "CampaignId=@CampaignId", new object[] { campaignId });
        }

        [DBCommand("SELECT Message from [Messages_Template] where MessageId=@MessageId")]
        public object GetMessages_Template(int MessageId)
        {
            return base.Execute(new object[] { MessageId });
        }

        public string GetMessagetext(int campaignId)
        {
            return base.Dlookup<string>("MessageText", "Campaigns", "CampaignId=" + campaignId.ToString(), "");
        }

       
      
        public bool Lookup_CampaignsIsPersonal(int CampaignId)
        {
            return (base.Dlookup("PersonalLength", "Campaigns", "CampaignId=" + CampaignId.ToString(), 0) > 0);
        }
        public int Lookup_CampaignsAccount(int CampaignId)
        {
            return base.Dlookup("AccountId", "Campaigns", "CampaignId=" + CampaignId.ToString(), 0);
        }
        public int Lookup_CampaignsSentItems(int CampaignId)
        {
            return base.Dlookup("TotalSent", "Campaigns_Statistic", "CampaignId=" + CampaignId.ToString(), 0);
        }

        [DBCommand(DBCommandType.Delete, "Messages_Template")]
        public int Messages_Template_Delete([DbField(DalParamType.Key)] int MessageId)
        {
            return (int)base.Execute(new object[] { MessageId });
        }

        [DBCommand(DBCommandType.Insert, "Messages_Template")]
        public int Messages_Template_Insert([DbField] int AccountId, [DbField] string Message, [DbField] bool IsPersonal, [DbField] string Preview)
        {
            return (int)base.Execute(new object[] { AccountId, Message, IsPersonal, Preview });
        }

        [DBCommand("SELECT * from [Messages_Template] where MessageId=@MessageId")]
        public DataRow Messages_Template_Item(int MessageId)
        {
            return (DataRow)base.Execute(new object[] { MessageId });
        }

        [DBCommand("SELECT MessageId,Preview from [Messages_Template] where AccountId=0 or AccountId=@AccountId")]
        public DataTable Messages_Template_List(int AccountId)
        {
            return (DataTable)base.Execute(new object[] { AccountId });
        }

        [DBCommand(DBCommandType.Update, "Messages_Template")]
        public int Messages_Template_Update([DbField(DalParamType.Key)] int MessageId, [DbField] string Message, [DbField] bool IsPersonal, [DbField] string Preview)
        {
            return (int)base.Execute(new object[] { MessageId, Message, IsPersonal, Preview });
        }

        [DBCommand("SELECT * from [Reminder] where ReminderId=@ReminderId")]
        public DataRow Reminder(int ReminderId)
        {
            return (DataRow)base.Execute(new object[] { ReminderId });
        }

        [DBCommand(DBCommandType.Insert, "Reminder")]
        public int Reminder_Insert
            (
            [DbField(DalParamType.Identity)] ref int ReminderId, 
            [DbField] string ReminderName, 
            [DbField] int ReminderType, 
            [DbField] int AccountId,
            [DbField] string Sender, 
            [DbField] string MessageText,
            [DbField] DateTime DateToSend,
            [DbField] DateTime ExpirationDate,
            [DbField] string DateField,
            [DbField] int Status,
            [DbField] bool Concatenate, 
            [DbField] bool IsPeronal,
            [DbField] int MtId
            )
        {
            object[] objArray = new object[] { (int)ReminderId, ReminderName, ReminderType, AccountId, Sender, MessageText, DateToSend, ExpirationDate, DateField, Status, Concatenate, IsPeronal, MtId };
            object obj2 = base.Execute(objArray);
            ReminderId = Types.ToInt(objArray[0], ReminderId);
            return Types.ToInt(obj2, 0);
        }

        [DBCommand("SELECT * from [Reminder_Registry] where ReminderId=0")]
        public DataTable Reminder_Registry_schema()
        {
            return (DataTable)base.Execute();
        }

        [DBCommand(DBCommandType.Update, "Reminder")]
        public int Reminder_Status
            (
            [DbField(DalParamType.Key)] int ReminderId, 
            [DbField] int Status
            )
        {
            return (int)base.Execute(new object[] { ReminderId, Status });
        }

        [DBCommand("SELECT * from [Scheduler] where SchedulerId=@SchedulerId")]
        public DataRow Scheduler(int SchedulerId)
        {
            return (DataRow)base.Execute(new object[] { SchedulerId });
        }

        [DBCommand("SELECT top 1 * from [Scheduler] where DataSource=@DataSource and ItemId=@ItemId")]
        public DataRow Scheduler(string DataSource, int ItemId)
        {
            return (DataRow)base.Execute(new object[] { DataSource, ItemId });
        }

        [DBCommand("SELECT * from [Scheduler] where AccountId=@AccountId")]
        public DataTable Scheduler_Monitor(int AccountId)
        {
            return (DataTable)base.Execute(new object[] { AccountId });
        }

        [DBCommand("SELECT * from [Scheduler_Queue_History] where AccountId=@AccountId")]
        public DataTable Scheduler_Queue_History(int AccountId)
        {
            return (DataTable)base.Execute(new object[] { AccountId });
        }

        [DBCommand(DBCommandType.Insert, "Scheduler_Queue")]
        public int Scheduler_Queue_Insert([DbField] int ItemId, [DbField] string DataSource, [DbField] DateTime Expiration)
        {
            return (int)base.Execute(new object[] { ItemId, DataSource, Expiration });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Scheduler_Update")]
        public int Scheduler_Update
            ([DbField] int ItemId, 
            [DbField] int AccountId, 
            [DbField] string DataSource, 
            [DbField(1)] string ScheduleType, 
            [DbField] int Cycle, 
            [DbField] DateTime LifeTime_Start, 
            [DbField] DateTime LifeTime_End, 
            [DbField] DateTime LastUse, 
            [DbField] DateTime NextUse, 
            [DbField] bool Enabled, 
            [DbField] int CurrentStep, 
            [DbField] int MaxSteps, 
            [DbField] string DaysInWeek, 
            [DbField] string ExecTime, 
            [DbField] int ExpirationDelta
            )
        {
            object[] objArray = new object[] { ItemId, AccountId, DataSource, ScheduleType, Cycle, LifeTime_Start, LifeTime_End, LastUse, NextUse, Enabled, CurrentStep, MaxSteps, DaysInWeek, ExecTime, ExpirationDelta };
            return Types.ToInt(base.Execute(objArray), 0);
        }

      
        #region Mail restricted
        [DBCommand(DBCommandType.Delete, "Campaigns_Mail_Restricted")]
        public void Mail_Restricted_Delete
            ([DbField(DalParamType.Key)] int AccountId,
            [DbField(DalParamType.Key)] string Email
            )
        {
            base.Execute(new object[] { AccountId, Email });
        }
       [DBCommand("Delete from Campaigns_Mail_Restricted where AccountId=@AccountId")]
       public void Mail_Restricted_DeleteAll
           ([DbField(DalParamType.Key)] int AccountId)
       {
           base.Execute(AccountId);
       }
        #endregion


    }


}

