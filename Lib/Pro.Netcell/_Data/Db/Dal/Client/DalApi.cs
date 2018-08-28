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


    public class DalApi : Nistec.Data.SqlClient.DbCommand
    {
        #region ctor

        public DalApi()
            : base(Netcell.Data.DBRule.CnnNetcell)
        {

        }

        public static DalApi Instance
        {
            get { return new DalApi(); }
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

        [DBCommand(DBCommandType.StoredProcedure, "sp_Users_Sessions")]
        public void Sessions_Insert(
            [DbField()]string SessionId,
            [DbField()]int ActionType,
            [DbField()]string SiteName,
            [DbField()]string Host,
            [DbField()]int UserId,
            [DbField()]string Details,
            [DbField()]string Server
            )
        {
            base.Execute(SessionId,
                ActionType, SiteName, Host, UserId, Details, Server);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Users_LognIn")]
        public DataRow Users_LognIn
        (
            [DbField()]string LogInName,
            [DbField()]string Pass
        )
        {
            return (DataRow)base.Execute(LogInName, Pass);
        }

        #region Lists

        [DBCommand("SELECT * from [Accounts_Category]")]
        public DataTable Accounts_Category()
        {
            return (DataTable)base.Execute();
        }
        [DBCommand("SELECT * from [Accounts_Type]")]
        public DataTable Accounts_Type()
        {
            return (DataTable)base.Execute();
        }
        [DBCommand("SELECT * from [Accounts_PaymentType]")]
        public DataTable Accounts_PaymentType()
        {
            return (DataTable)base.Execute();
        }
        [DBCommand("SELECT * from [Accounts_BillingType]")]
        public DataTable Accounts_BillingType()
        {
            return (DataTable)base.Execute();
        }

        
        [DBCommand("SELECT * from [Countries]")]
        public DataTable Countries()
        {
            return (DataTable)base.Execute();
        }
        [DBCommand("SELECT * from [HoroSigns]")]
        public DataTable HoroSigns()
        {
            return (DataTable)base.Execute();
        }
        [DBCommand("SELECT * from [Methods]")]
        public DataTable Methods()
        {
            return (DataTable)base.Execute();
        }
        [DBCommand("SELECT * from [Users_Lvl]")]
        public DataTable Users_Lvl()
        {
            return (DataTable)base.Execute();
        }
        [DBCommand("SELECT * from [Users_Group]")]
        public DataTable Users_Group()
        {
            return (DataTable)base.Execute();
        }

        //[DBCommand(@"select * from vw_Pricing_Code_Display")]
        //public DataTable Pricing_Code_Display()
        //{
        //    return (DataTable)base.Execute();
        //}
        #endregion

        [DBCommand(@"SELECT top 50000 i.Id,i.BatchId,i.Email  from vw_Campaigns_Mail_SentItems i
        left join Campaigns_Reply_Items r on r.SentId=i.Id and r.Platform=2
        where i.CampaignId=@CampaignId and i.BatchId between @BatchFrom and @BatchTo and i.SmtpStatus=250 and r.SentId is null")]
        public DataTable Campaigns_NonReply_MailItems(int CampaignId, int BatchFrom, int BatchTo)
        {
            return (DataTable)base.Execute(CampaignId, BatchFrom, BatchTo);
        }

        [DBCommand(@"SELECT top 50000 i.Id,i.BatchId,i.Email  from vw_Campaign_SentItems_Mail i
        inner join Campaigns_Reply_Items r on r.SentId=i.Id and r.Platform=2
        where i.CampaignId=@CampaignId and i.BatchId between @BatchFrom and @BatchTo and i.SmtpStatus=250")]
        public DataTable Campaigns_ReReply_MailItems(int CampaignId, int BatchFrom, int BatchTo)
        {
            return (DataTable)base.Execute(CampaignId, BatchFrom, BatchTo);
        }

        public int Counter_Lock(int counterType, int Count)
        {
            if (Count <= 0)
            {
                Count = 1;
            }
            int num = Types.ToInt(base.ExecuteScalar<int>("tst_Counter_New", new SqlParameter[] { new SqlParameter("CounterId", counterType), new SqlParameter("Count", Count) },0, CommandType.StoredProcedure), 0);
            //int num = Types.ToInt(base.ExecuteProcedure("sp_Counter_lock", new SqlParameter[] { new SqlParameter("CounterId", counterType), new SqlParameter("Count", Count) }), 0);
            if (num <= 0)
            {
                throw new Exception("Counters error");
            }
            return num;
        }

        //==============================================================================================

        public void ValidateIP(int AccountId, string Ip)
        {
            int Status = 0;
            ValidateIP(AccountId, Ip, ref Status);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Accounts_ValidateIp")]
        public int ValidateIP
            (
            [DbField()] int AccountId,
            [DbField()] string Ip,
            [DbField()] ref int Status
           )
        {
            object[] values = new object[] { AccountId, Ip, Status };

            int res = (int)base.Execute(values);
            Status = Types.ToInt(values[2], -2);
            if (Status != 0)
            {
                throw new Exception("Invalid IP address for Account: " + AccountId.ToString() + ", IP: " + Ip);
            }
            return res;
        }

        public void ValidateApp(int AccountId, string AppName, string UrlReferrer)
        {
            int Status = 0;
            ValidateApp(AccountId, AppName, UrlReferrer, ref Status);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_App_ValidateReferrer")]
        public int ValidateApp
            (
            [DbField()] int AccountId,
            [DbField()] string AppName,
            [DbField()] string UrlReferrer,
            [DbField()] ref int Status
           )
        {
            object[] values = new object[] { AccountId, AppName, UrlReferrer, Status };

            int res = (int)base.Execute(values);
            Status = Types.ToInt(values[3], -2);
            if (Status != 0)
            {
                throw new Exception("Client address Not allowed: " + UrlReferrer);
            }
            return res;
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Send_Query")]
        public DataTable Send_Query
            (
            [DbField()] int AccountId,
            [DbField()] string QueryType,
            [DbField()] string Query,
            [DbField()] string Args,
            [DbField()] int PostType
           )
        {
            return (DataTable)base.Execute(AccountId, QueryType, Query, Args, PostType);
        }

        public int Send_Message
       (
       [DbField()] int AccountId,
       [DbField()] int MtId,
       [DbField()] DateTime ExecTime,
       [DbField()] string Sender,
       [DbField()] string Target,
       [DbField()] string Message
      )
        {
            return Send_Message(AccountId, MtId, ExecTime, Sender, Target, Message, null, null, null, null, null, 0, null, 0, 0);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Send_Message_Multi_b")]
        public DataRow Send_Message_Multi
            (
            [DbField()] int AccountId,
            [DbField()] int MtId,
            [DbField()] DateTime ExecTime,
            [DbField()] string Sender,
            [DbField()] string Target,
            [DbField()] string Message,
            [DbField()] string Body,
            [DbField()] string Title,
            [DbField()] string PersonalDisplay,
            [DbField()] string Args,
            //[DbField()] string Personal,
            [DbField()] string Coupon,
            [DbField()] int Server,
            [DbField()] int UserId
           )
        {
            return (DataRow)base.Execute(AccountId, MtId, ExecTime, Sender, Target, Message, Body, Title, PersonalDisplay, Args, Coupon, Server, UserId);
        }


        [DBCommand(DBCommandType.StoredProcedure, "sp_Send_Message_Group")]
        public DataRow Send_Message_Group
            (
            [DbField()] int AccountId,
            [DbField()] int MtId,
            [DbField()] DateTime ExecTime,
            [DbField()] string Sender,
            [DbField()] string Group,
            [DbField()] string Message,
            [DbField()] string Body,
            [DbField()] string Title,
            [DbField()] string PersonalDisplay,
            [DbField()] string Args,
            [DbField()] string Coupon,
            [DbField()] int Server,
            [DbField()] int UserId
           )
        {
            return (DataRow)base.Execute(AccountId, MtId, ExecTime, Sender, Group, Message, Body, Title, PersonalDisplay, Args, Coupon, Server, UserId);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Send_Message_Co")]
        public DataRow Send_Message_Co
            (
            [DbField()] int AccountId,
            [DbField()] int MtId,
            [DbField()] DateTime ExecTime,
            [DbField()] string Sender,
            [DbField()] string Group,
            [DbField()] string Message,
            [DbField()] string Body,
            [DbField()] string Title,
            [DbField()] string PersonalDisplay,
            [DbField()] string Args,
            [DbField()] string Coupon,
            [DbField()] int Server,
            [DbField()] int UserId
           )
        {
            return (DataRow)base.Execute(AccountId, MtId, ExecTime, Sender, Group, Message, Body, Title, PersonalDisplay, Args, Coupon, Server, UserId);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Send_Message_d")]
        public int Send_Message
            (
            [DbField()] int AccountId,
            [DbField()] int MtId,
            [DbField()] DateTime ExecTime,
            [DbField()] string Sender,
            [DbField()] string Target,
            [DbField()] string Message,
            [DbField()] string Body,
            [DbField()] string Title,
            [DbField()] string PersonalDisplay,
            [DbField()] string Args,
            [DbField()] string Personal,
            [DbField()] int GroupId,
            [DbField()] string Coupon,
            [DbField()] int Server,
            [DbField()] int UserId
           )
        {
            return (int)base.Execute(AccountId, MtId, ExecTime, Sender, Target, Message, Body, Title, PersonalDisplay, Args, Personal, GroupId, Coupon, Server, UserId);
        }


        [DBCommand(DBCommandType.StoredProcedure, "sp_Send_Message_Campaign_d")]
        public int Send_Message_Campaign
            (
            [DbField()] int AccountId,
            [DbField()] int CampaignId,
            [DbField()] DateTime ExecTime,
            [DbField()] string Target,
            [DbField()] string Personal,
            [DbField()] int GroupId,
            [DbField()] string Coupon,
            [DbField()] int Server,
            [DbField()] int UserId
            //ValidateCredit
           )
        {
            return (int)base.Execute(AccountId, CampaignId, ExecTime, Target, Personal, GroupId, Coupon, Server, UserId);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Send_Message_Catalog_d")]
        public int Send_Message_Catalog
            (
            [DbField()] int AccountId,
            [DbField()] int MtId,
            [DbField()] DateTime ExecTime,
            [DbField()] string Sender,
            [DbField()] string Target,
            [DbField()] string Message,
            [DbField()] int CatalogId,
            [DbField()] string PersonalDisplay,
            [DbField()] string Args,
            [DbField()] string Personal,
            [DbField()] int GroupId,
            [DbField()] string Coupon,
            [DbField()] int Server,
            [DbField()] int UserId
           )
        {
            return (int)base.Execute(AccountId, MtId, ExecTime, Sender, Target, Message, CatalogId, PersonalDisplay, Args, Personal, GroupId, Coupon, Server, UserId);
        }


        [DBCommand(DBCommandType.StoredProcedure, "sp_Send_Message_MailAction")]
        public int Send_Message_Mail_Action
            (
            [DbField()] int Application,
            [DbField()] int AccountId,
            [DbField()] string Method,
            [DbField()] string MailFrom,
            [DbField()] string MailRecipients,
            [DbField()] string MailSubject,
            [DbField()] string MaiBody
           )
        {
            return (int)base.Execute(Application, AccountId, Method, MailFrom, MailRecipients, MailSubject, MaiBody);
        }

    }
}

