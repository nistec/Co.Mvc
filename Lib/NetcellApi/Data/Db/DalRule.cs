using System;
using System.Data;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using System.Data.SqlClient;
using System.Text;
using Nistec.Data.SqlClient;
using Nistec.Data;
using Nistec.Data.Factory;
using Nistec;

namespace Netcell.Data.Db
{
     public class DalRule : DbCommand
     {

         static object m_lock = new object();

        #region ctor

         public DalRule()
            : base(DBRule.CnnNetcell)
        {
        }

        public static DalRule Instance
        {
            get
            {
                return new DalRule();
            }
        }
        #endregion

        #region entities

        public IDbCmd GetDbCmd
        {
            get { return DbFactory.Create(DBRule.CnnNetcell, DBProvider.SqlServer); }
        }

        public DataRow GetEntity(string tableName, string where, params object[] parameters)
        {
            return base.ExecuteCommand<DataRow>("select * from "+ tableName + " where " + where,DataParameter.Get(parameters));
        }

        public DataView GetEntityList(string valueMember, string displayMember, string tableName, string where, params object[] parameters)
        {
            DataTable dt = base.ExecuteCommand<DataTable>("select [" + valueMember + "],[" + displayMember + "] from [" + tableName + "] where " + where, DataParameter.Get(parameters));
            if (dt == null)
                return null;
            return dt.DefaultView;
            //return Instance.DView(valueMember, displayMember, tableName, string.Format(where, parameters));
        }

        //public static void BindList(ListControl ctl, string valueMember, string displayMember, string tableName, string where, params object[] parameters)
        //{
        //    ctl.DataTextField = displayMember;
        //    ctl.DataValueField = valueMember;
        //    ctl.DataSource = Instance.DView(valueMember, displayMember, tableName, string.Format(where, parameters));
        //    ctl.DataBind();
        //}

        #endregion

        #region accounts

        [DBCommand(DBCommandType.StoredProcedure, "sp_Billing_Exec")]
        public int Account_Billing
            (
            [DbField(DalParamType.SPReturnValue)] ref int BillingId, 
            [DbField] int AccountId, [DbField] int CampaignId, 
            [DbField] int TransId, 
            [DbField] decimal CreditValue, 
            [DbField] decimal DefaultPrice, 
            [DbField] int ActionType, 
            [DbField] int UserId, 
            [DbField] string Remarks, 
            [DbField] int Units, 
            [DbField] int Platform
            )
        {
            object[] objArray = new object[] { (int)BillingId, AccountId, CampaignId, TransId, CreditValue, DefaultPrice, ActionType, UserId, Remarks, Units, Platform };
            object obj2 = base.Execute(objArray);
            BillingId = Types.ToInt(objArray[0], 0);
            return (int)obj2;
        }

        public decimal AccountActualCredit(int AccountId)
        {
            return base.Dlookup("ActualCredit", "vw_Accounts_Credit", string.Format("AccountId={0}", AccountId), 0M);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Account_Price")]
        public DataRow Accounts_Price([DbField] int AccountId, [DbField] int MtId)
        {
            return (DataRow)base.Execute(new object[] { AccountId, MtId });
        }

        //public string AccountSender(int AccountId)
        //{
        //    return base.Dlookup<string>("Sender", "Accounts_Features", string.Format("AccountId={0}", AccountId), "1234");
        //}
        public bool GetAccountPrice(int accountId, int method, ref decimal price)
        {
            price = base.LookupQuery<decimal>("Price", "Accounts_Price", "AccountId=@AccountId and MtId=@MtId and BillingType>0", 0m, new object[] { accountId, method });
            if (price <= 0)
            {
                return false;
            }
            //price = Types.ToDecimal(obj2, 0M);
            return true;
        }

        public int LookupAccountPriceCode(int accountId)
        {
            return base.LookupQuery<int>("PriceCode", "Accounts", "AccountId=@AccountId", 0, new object[] { accountId });
        }

        public byte LookupAccountBunch(int accountId)
        {
            int priceCode = LookupAccountPriceCode(accountId);
            return (byte)priceCode;
        }

        public decimal GetSumMonthlyPrice(int accountId, int method, int month, int year)
        {
            DateTime time = new DateTime(year, month, 1, 0, 0, 1);
            DateTime time2 = new DateTime(year, month, DateTime.DaysInMonth(year, month), 0x17, 0x3b, 0x3b);
            return base.DSum<decimal>("CreditValue", "Accounts_Billing", "AccountId=@AccountId and MtId=@MtId and BillingType=3 and Creation between @DateFrom and @DateTo and ActionType in(8,9) and BillingState=0 ", new object[] { accountId, method, time, time2 });
        }

        public int GetSumMonthlyUnits(int accountId, int method, int month, int year)
        {
            DateTime time = new DateTime(year, month, 1, 0, 0, 1);
            DateTime time2 = new DateTime(year, month, DateTime.DaysInMonth(year, month), 0x17, 0x3b, 0x3b);
            return base.DSum<int>("Units", "Accounts_Billing", "AccountId=@AccountId and MtId=@MtId and BillingType=3 and Creation between @DateFrom and @DateTo and ActionType in(8,9) and BillingState=0 ", new object[] { accountId, method, time, time2 });
        }



        [DBCommand(DBCommandType.StoredProcedure, "sp_Lookup_AccountMailHost")]
        public DataRow Lookup_AccountMailHost([DbField] int AccountId)
        {
            return (DataRow)base.Execute(new object[] { AccountId });
        }

        public string Lookup_MailHost(int accountId)
        {
            DataRow row = this.Lookup_AccountMailHost(accountId);
            if (row == null)
            {
                return null;
            }
            return string.Format("{0}", row["MailHost"]);
        }
        #endregion
 
        #region campaigns

        //[DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Batch_Create")]
        //internal object Campaigns_Batch_Create([DbField] int CampaignId)
        //{
        //    return (int)base.Execute(new object[] { CampaignId });
        //}

        //[DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Batch_Create")]
        //internal object Campaigns_Batch_Create([DbField] int CampaignId, [DbField] int AccountId, [DbField] int MtId, [DbField] int BillType)
        //{
        //    return (int)base.Execute(new object[] { CampaignId, AccountId, MtId, BillType });
        //}
        #endregion

        #region config

        public void ActiveConnectionClose()
        {
            base.ConnectionClose();
        }

        public string AppConfig(string ConfigKey)
        {
            return base.Dlookup<string>("ConfigValue", "App_Config", string.Format("ConfigKey='{0}'", ConfigKey),(string) null);
        }

        //internal int Counter_New(int counterType)
        //{
        //    return this.Counter_New(counterType, 1);
        //}

        //internal int Counter_New(int counterType, int Count)
        //{

        //    if (Count <= 0)
        //    {
        //        Count = 1;
        //    }
        //    int num = Types.ToInt(base.ExecuteScalar<int>("sp_Counter_New", CommandType.StoredProcedure, 0, 0, new SqlParameter[] { new SqlParameter("CounterId", counterType), new SqlParameter("Count", Count) }), 0);
        //    if (num <= 0)
        //    {
        //        throw new Exception("Counters error");
        //    }
        //    return num;

        //}

         [DBCommand(DBCommandType.StoredProcedure, "sp_Counters")]
         public int Counter_New
            (
            [DbField]int CounterId,
            [DbField]int Count,
            [DbField]ref int Value

            )
         {

             object[] values = new object[] { CounterId, Count, Value };

             int res = (int)base.Execute(values);
             Value = Types.ToInt(values[2], 0);
             if (Value <= 0)
             {
                 throw new Exception("Counters error");
             }
             return res;
         }


        //public bool Lookup_MessageIdDExists(int platform, int min, int max)
        //{
        //    string tabe = platform == 1 ? "Campaigns_Items_Cell" : "Campaigns_Items_Mail";
        //    return base.DExists("MessageId", tabe, "MessageId between @min and @max", new object[]{ min, max});
        //}

        //public new DataTable ExecuteDataTable(string sql)
        //{
        //    return base.ExecuteCommand<DataTable>(sql);
        //}
  
     

        [DBCommand("SELECT ConfigValue FROM [App_Config]WHERE ConfigType='Mail'")]
        public DataTable GetEmailList()
        {
            return (DataTable)base.Execute();
        }

        public int GetLastVersion(int VersionId, out bool Expired)
        {
            Expired = base.Dlookup("Expired", "Versions", "VersionId=" + VersionId.ToString(), false);
            return base.Dlookup("FullVersion", "Versions", "VersionId=" + VersionId.ToString(), VersionId);
        }

        public string GetLastVersion(string Version, out bool Expired)
        {
            Expired = base.Dlookup("Expired", "Versions", "VersionId=" + Version.ToString(), false);
            return base.Dlookup("FullVersion", "Versions", "FullVersion='" + Version + "'", Version);
        }

        [DBCommand("SELECT ConfigValue FROM [App_Config]WHERE ConfigType='Phone'")]
        public DataTable GetPhoneList()
        {
            return (DataTable)base.Execute();
        }

        public string GetSmsSender()
        {
            return base.Dlookup("ConfigValue", "App_Config", "ConfigType='SmsSender'", "");
        }

        public string GetUserAcount()
        {
            return base.Dlookup("ConfigValue", "App_Config", "ConfigType='UserAcount'", "");
        }

        [DBCommand("SELECT ConfigValue FROM [App_Config]WHERE ConfigType='UserPWD'")]
        public string GetUserPWD()
        {
            return base.Dlookup("ConfigValue", "App_Config", "ConfigType='UserPWD'", "");
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_ValidateVersion")]
        public void ValidateVersion
            (
            [DbField(DalParamType.Key)] ref string Version,
            [DbField] ref bool Expired
            )
        {
            object[] objArray = new object[] { Version, (bool)Expired };
            base.Execute(new object[] { Version, (bool)Expired });
        }


        [DBCommand("SELECT * from App_Servers")]
        public DataTable AppServersAll()
        {
            return (DataTable)base.Execute();
        }

        //[DBCommand("SELECT * from App_Servers where IsActive=1")]
        //public DataTable AppServers()
        //{
        //    return (DataTable)base.Execute();
        //}

        //[DBCommand("SELECT * from App_Servers where IsActive=1 and (EnablePlatform=3 or EnablePlatform=@Platform)")]
        //public DataTable AppServers(int Platform)
        //{
        //    return (DataTable)base.Execute(Platform);
        //}

        [DBCommand("SELECT * from App_Servers where ServerMode<>5")]
        public DataTable AppActiveServers()
        {
            return (DataTable)base.Execute();
        }
        [DBCommand("SELECT * from App_Servers where ServerMode<>5 and (EnablePlatform=3 or EnablePlatform=@Platform)")]
        public DataTable AppActiveServers(int Platform)
        {
            return (DataTable)base.Execute(Platform);
        }
        #endregion

        #region system alerts


        [DBCommand("SELECT * FROM [vw_SystemAlertsAction]WHERE ActionType > 0")]
        public DataTable SystemAlertsAction()
        {
            return (DataTable)base.Execute();
        }

        [DBCommand("SELECT * FROM [vw_SystemAlertsAction]WHERE ActionType=@ActionType")]
        public DataTable SystemAlertsAction(int ActionType)
        {
            return (DataTable)base.Execute(new object[] { ActionType });
        }
        #endregion

        #region users

        [DBCommand("SELECT top 1 * from [Users] where LogInName=@LogInName and Pass=@Pass")]
        public DataRow User_Auth([DbField] string LogInName, [DbField] string Pass)
        {
            return (DataRow)base.Execute(new object[] { LogInName, Pass });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Auth_WithIp")]
        public DataRow User_Auth
            (
            [DbField()]string LogInName,
            [DbField()] string Pass,
            [DbField()]string Ip
            )
        {
            return  (DataRow)base.Execute(LogInName, Pass, Ip);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Auth_Service")]
        public DataRow User_Auth_Service
            (
            [DbField()]ref int AuthState,
            [DbField()]string LogInName,
            [DbField()]string Pass,
            [DbField()]int AccountId,
            [DbField()]int UsingType,//1=ephone,2=api
            [DbField()]string Ip,
            [DbField()]int ExCheck//0=none,1=checkSmsBlocked,2=checkMailBlocked
            )
        {
            object[] values = new object[] { AuthState, LogInName, Pass, AccountId, UsingType, Ip, ExCheck };
            var row = (DataRow)base.Execute(values);
            AuthState = Types.ToInt(values[0], 0);
            return row;
        }


        [DBCommand(DBCommandType.StoredProcedure, "sp_Auth_LookupWithIp")]
        public int User_Auth_Lookup
            (
            [DbField()]string LogInName,
            [DbField()] string Pass,
            [DbField()]string Ip,
            [DbField()]ref int AuthId,
            [DbField()]ref int AuthUser
            )
        {
            object[] values = new object[] { LogInName, Pass, Ip, AuthId, AuthUser };
            int res = (int)base.Execute(values);
            AuthId = Types.ToInt(values[3], 0);
            AuthUser = Types.ToInt(values[4], 0);
            return res;
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Auth_ManagerWithIp")]
        public int Auth_Manager
            (
            [DbField()]string LogInName,
            [DbField()] string Pass,
            [DbField()]string Ip,
            [DbField()]ref int AuthId
            )
        {
            object[] values = new object[] { LogInName, Pass, Ip, AuthId};
            int res = (int)base.Execute(values);
            AuthId = Types.ToInt(values[3], 0);
            return res;
        }

        public bool IsAuth_Manager
            (
            string LogInName,
            string Pass,
            string Ip
            )
        {
            int AuthId = 0;
            Auth_Manager(LogInName, Pass, Ip, ref AuthId);
            return AuthId>0;
        }

 
        [DBCommand("SELECT top 1 * from [vw_UsersInfo] where UserId=@UserId")]
        public DataRow UserInfo(int UserId)
        {
            return (DataRow)base.Execute(new object[] { UserId });
        }

        [DBCommand("SELECT top 1 u.*,a.CellAlert FROM [Users]u \r\n        inner join Accounts_Features a on u.AccountId=a.AccountId \r\n        WHERE u.AccountId=@AccountId and u.IsActive=1 order by UserId")]
        public DataRow Users_Alert(int AccountId)
        {
            return (DataRow)base.Execute(new object[] { AccountId });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Users_LognIn")]
        public DataRow Users_LognIn([DbField] string LogInName, [DbField] string Pass)
        {
            return (DataRow)base.Execute(new object[] { LogInName, Pass });
        }

        [DBCommand("SELECT * FROM [vw_Users_Relation] WHERE AccountId=@AccountId and GroupId=@GroupId")]
        public DataTable Users_Relation(int AccountId, int GroupId)
        {
            return (DataTable)base.Execute(new object[] { AccountId, GroupId });
        }
        #endregion

        #region credit
        

        public static int ValidateCredit(int AccountId, int Method, int Units)
        {

            decimal Price = 0M;
            decimal Credit = 0M;
            int CreditStatus = 0;

            using (var dl = DalRule.Instance)
            {
                dl.ValidateCredit(AccountId, Method, Units, 0, ref Price, ref Credit, ref CreditStatus);
            }
            return CreditStatus;
        }
    
        [DBCommand(DBCommandType.StoredProcedure, "sp_Credit_Validation_b")]
        public int ValidateCredit
            (
            [DbField()]int AccountId,
            [DbField()] int MtId,
            [DbField()]int Units,
            [DbField()]int CreditMode,
            [DbField(DbType.Decimal, 12, 4)]ref decimal Price,
            [DbField(DbType.Decimal, 12, 4)]ref decimal Credit,
            [DbField()]ref int CreditStatus
            )
        {
            object[] values = new object[] { AccountId, MtId, Units, CreditMode, Price, Credit, CreditStatus };
            int res = (int)base.Execute(values);
            Price = Types.ToDecimal(values[4], 0M);
            Credit = Types.ToDecimal(values[5], 0M);
            CreditStatus = Types.ToInt(values[6], 0);
            return res;//Types.ToInt(obj2, 0);
        }
        #endregion

        #region pricing RB

        //public bool ValidatePricingRB(string PriceCode)
        //{
        //    return base.DExists("PriceCode", "Pricing_RB", "PriceCode=@PriceCode", new object[]{PriceCode});
        //}

        //public bool ValidatePricingRB(string PriceCode, string SC)
        //{
        //    return base.DExists("PriceCode", "vw_Pricing_SC", "PriceCode=@PriceCode and SC=@SC", new object[]{PriceCode, SC});
        //}
        #endregion

        #region rules

        [DBCommand("SELECT * FROM [Carrier_Rules_CB] where [AccountId]=0 and [OperatorId]=0 and [MethodRule]=0")]
        public DataRow Carrier_Rules_CB_Default()
        {
            return (DataRow)base.Execute();
        }

        [DBCommand("SELECT * FROM [Carrier_Rules_CB]")]
        public DataTable Carrier_Rules_CB()
        {
            return (DataTable)base.Execute();
        }
        [DBCommand(  DBCommandType.Delete,"Carrier_Rules_CB")]
        public int Carrier_Rules_CB_Delete
            (
             [DbField( DalParamType.Key)] int AccountId,
             [DbField(DalParamType.Key)] int OperatorId,
             [DbField(DalParamType.Key)]int MethodRule
            )
        {
            if (AccountId > 0)
            {
                return (int)base.Execute(AccountId, OperatorId, MethodRule);
            }
            return 0;
        }


        [DBCommand(DBCommandType.StoredProcedure, "sp_Server_Rules")]
        public int Server_Rules
           (
           [DbField]int AccountId,
           [DbField]int Platform,
           [DbField]ref int Server

           )
        {

            object[] values = new object[] { AccountId, Platform, Server };

            int res = (int)base.Execute(values);
            Server = Types.ToInt(values[2], 0);
            if (Server < 0)
            {
                Server=0;
            }
            return res;
        }
        #endregion

        #region remote obsolete
         /*
         [Obsolete("not used")]
        [DBCommand(DBCommandType.Insert, "Trans_Remote")]
        public int Trans_Remote_Insert(
        [DbField()] int TransId,
        [DbField()] int Platform,
        [DbField()] int AccountId,
        [DbField()] string Body,
        [DbField()] string Sender,
        [DbField()] decimal Price,
        [DbField()] int Units,
        [DbField()] int Status,
        [DbField()] int NotifyStatus,
        [DbField()] int CampaignId,
        [DbField()] int ItemsCount
        )
        {
            return (int)base.Execute(TransId, Platform, AccountId, Body, Sender, Price, Units, Units, NotifyStatus, CampaignId, ItemsCount);
        }
         */
        #endregion

        #region mail

        //[DBCommand("SELECT top 1 * from [Mail_Connection] where (AccountId=@AccountId or AccountId=0)and (HostName='Default') order by AccountId desc")]
        //public DataRow Mail_Connection(int AccountId)
        //{
        //    return (DataRow)base.Execute(AccountId);
        //}
        //[DBCommand("SELECT top 1 * from [Mail_Connection] where (AccountId=@AccountId or AccountId=0)and (HostName=@HostName) and IsActive=1 order by AccountId desc")]
        //public DataRow Mail_Connection(int AccountId, string HostName)
        //{
        //    return (DataRow)base.Execute(AccountId, HostName);
        //}
        //[DBCommand("SELECT top 1 * from [Mail_Connection] where (AccountId=@AccountId or AccountId=0)and (HostName like 'Default%') and Server=@Server order by AccountId desc")]
        //public DataRow Mail_Connection(int AccountId, int Server)
        //{
        //    return (DataRow)base.Execute(AccountId, Server);
        //}

        //[DBCommand("SELECT top 1 * from [Mail_Connection] where (CnnId=@CnnId and IsActive=1)")]
        //public DataRow Mail_Connection(string CnnId)
        //{
        //    return (DataRow)base.Execute(CnnId);
        //}
        //[DBCommand("SELECT * from [Mail_Connection] where (CnnId=@CnnId and IsActive=1)")]
        //public DataTable Mail_Connections(string CnnId)
        //{
        //    return (DataTable)base.Execute(CnnId);
        //}
        //[DBCommand("SELECT m.* from Mail_Channels m where m.IsActive=1 and Server=@Server")]
        //public DataTable Mail_Channels(int Server)
        //{
        //    return (DataTable)base.Execute(Server);
        //}

        //[DBCommand("SELECT m.* from Mail_Channels m where m.CnnId=@CnnId")]
        //public DataTable Mail_Channels(string CnnId)
        //{
        //    return (DataTable)base.Execute(CnnId);
        //}

        #endregion
    }

 }

