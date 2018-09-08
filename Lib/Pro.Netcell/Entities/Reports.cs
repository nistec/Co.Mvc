using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using ProNetcell.Data;
using Nistec.Data;
using Nistec;
using ProNetcell;
using System.Data;
using ProNetcell.Query;

namespace ProNetcell.Data.Entities
{

    public class ReportContext 
    {
        const int TtlDashbord = 3;

       #region static

        public static DataTable DoReport(int AccountId, string ReportType)
        {
            using (var db = DbContext.Create<DbNetcell>())
            return db.ExecuteDataTable("sp_Report", "AccountId", AccountId, "ReportType", ReportType);
        }
        public static string DoStatisticReport(StatisticReportQuery q)//int AccountId, string ReportType, int Campaign)
        {
            using (var db = DbContext.Create<DbNetcell>())
            return db.ExecuteDataTable("sp_Report", "AccountId", q.AccountId, "ReportType", q.ReportType, "Campaign", q.Campaign, "SignupDateFrom", q.SignupDateFrom, "SignupDateTo", q.SignupDateTo).ToCSV(false, false);
        }
        public static string Dashbord_Members(int AccountId)
        {
            return EntityPro.CacheGetOrCreate(EntityPro.CacheKey(EntityGroups.Reports, AccountId, "Dashboard"), () => DoReport(AccountId, "Dashboard"), TtlDashbord).ToCSV(false);
        }
    
        public static string Dashbord_Counters(int AccountId)
        {
            return EntityPro.CacheGetOrCreate(EntityPro.CacheKey(EntityGroups.Reports, AccountId, "Counters"), () => DoReport(AccountId, "Counters"), TtlDashbord).ToCSV(false,false);
        }

        public static DataTable SendReportData(int AuthAccount, int Platform, DateTime DateFrom, DateTime DateTo, int BatchId = 0, string Target = null, bool EnableNotif = false)
        {
            using (var db = DbContext.Create<ProNetcellxy>())
            return db.ExecuteDataTable("sp_Trans_Items_Report", "AccountId", AuthAccount, "Platform", Platform, "DateFrom", DateFrom, "DateTo", DateTo, "BatchId", BatchId, "Target", Target);
        }
        public static string SendReport(int AuthAccount, int Platform, DateTime DateFrom, DateTime DateTo, int BatchId = 0, string Target = null, bool EnableNotif = false)
        {
            using (var db = DbContext.Create<ProNetcellxy>())
            return db.ExecuteJson("sp_Trans_Items_Report", "AccountId", AuthAccount, "Platform", Platform, "DateFrom", DateFrom, "DateTo", DateTo, "BatchId", BatchId, "Target", Target);
        }

        /*
        Status, 
        StatusName, 
        NotifyStatus,
        AckStatus,
        MessageId, 
        BatchId, 
        Target, 
        CampaignId,
        Platform,
        Sender,
        ItemIndex,
        Personal
        OperatorId,
        OpId, 
        Price, 
        Units,
        Size, 
        SentTime,
        AccountId, 
        MtId
        */

        public static SendMessageView BatchContentView(int BatchId)
        {
            using (var db = DbContext.Create<ProNetcellxy>())
            return db.ExecuteSingle<SendMessageView>("sp_Batch_Message_View", "BatchId", BatchId);
        }
        //public static string BatchContentView(int BatchId)
        //{
        //    return ProNetcellxy.Instance.ExecuteJson("sp_Batch_Message_View", "BatchId", BatchId);
        //}
/*
[BatchId]
,[PlatformView]
,[Message]
,[Body]
--,[Size]
--,[Units]
--,[MaxWidth]
,[Title]
,[Sender]
,[IsRtl]
,[PagesCount]
,[PersonalDisplay]
--,[AltHtml]
,[Args]
*/

        public static string ToCsv(DataTable dt)
        {
            return dt.ToCSV();
        }

        #endregion
    }

    public class SendMessageView:IEntityItem
    {
        public string Message { get; set; }
        public string Body { get; set; }
        public int BatchId { get; set; }
        public int PlatformView { get; set; }

        public string View
        {
            get
            {
                if (string.IsNullOrEmpty(Body))
                    return Message;
                else
                    return Body;
            }

        }

    }
}
