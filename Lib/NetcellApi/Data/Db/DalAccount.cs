using System;
using System.Data;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Nistec.Data.SqlClient;
using Nistec.Data;
using Nistec.Data.Factory;
using Nistec;

namespace Netcell.Data.Db
{


    public class DalAccounts : DbCommand
    {

        public DalAccounts()
            : base(DBRule.CnnNetcell)
        {

        }

        public static DalAccounts Instance
        {
            get { return new DalAccounts(); }
        }

     
        [DBCommand("SELECT * from [Accounts]")]
        public DataTable Accounts()
        {
            return (DataTable)base.Execute();
        }
        [DBCommand("SELECT * from [Accounts] where AccountId=@AccountId")]
        public  DataRow Accounts(int AccountId)
        {
            return (DataRow)base.Execute(AccountId);
        }
        [DBCommand( DBCommandType.Text,"SELECT * from [Accounts] where AccountId=-1","", MissingSchemaAction.AddWithKey)]
        public DataTable AccountsSchema()
        {
            return (DataTable)base.Execute();
        }
        [DBCommand("SELECT * from [Accounts] where ParentId=@ParentId")]
        public DataTable AccountsByParent(int ParentId)
        {
            return (DataTable)base.Execute(ParentId);
        }
        [DBCommand("SELECT * from [vw_AccountsByKey] where BusinessGroup=@BusinessGroup")]
        public DataTable AccountsByCategory(int BusinessGroup)
        {
            return (DataTable)base.Execute(BusinessGroup);
        }
        [DBCommand("SELECT * from [vw_AccountsByKey] where ParentId=@ParentId and BusinessGroup=@BusinessGroup")]
        public DataTable AccountsByCategory(int ParentId,int BusinessGroup)
        {
            return (DataTable)base.Execute(BusinessGroup);
        }

        public DataTable AccountsKeyByCategory(int ParentId, string Filter)
        {
            return base.ExecuteCommand<DataTable>(string.Format("SELECT * FROM vw_AccountsByKey WHERE  ParentId={0} and BusinessGroup {1} ", ParentId, Filter));
        }
        public DataTable AccountsKeyByCategory(string Filter)
        {
            return base.ExecuteCommand<DataTable>(string.Format("SELECT * FROM vw_AccountsByKey WHERE BusinessGroup {0} ", Filter));
        }

        public DataTable AccountsKeyByFilter(int ParentId, string Filter)
        {
            return base.ExecuteCommand<DataTable>(string.Format("SELECT * FROM vw_AccountsByKey WHERE  ParentId={0} and {1} ", ParentId, Filter));
        }
        public DataTable AccountsKeyByFilter(string Filter)
        {
            return base.ExecuteCommand<DataTable>(string.Format("SELECT * FROM vw_AccountsByKey WHERE {0} ", Filter));
        }


        [DBCommand("SELECT * from [vw_AccountsByKey] where ParentId=@ParentId")]
        public DataTable AccountsKeys(int ParentId)
        {
            return (DataTable)base.Execute(ParentId);
        }
        [DBCommand("SELECT * from [vw_AccountsByKey]")]
        public DataTable AccountsKeys()
        {
            return (DataTable)base.Execute();
        }

        [DBCommand("SELECT AccountId,AccountName from [Accounts]")]
        public DataTable AccountsList()
        {
            return (DataTable)base.Execute();
        }
        [DBCommand("SELECT AccountId,AccountName from [Accounts] where ParentId=@ParentId")]
        public DataTable AccountsList(int ParentId)
        {
            return (DataTable)base.Execute();
        }

        [DBCommand(@"SELECT c.AccountId, c.AccountName FROM dbo.Accounts a INNER JOIN
        dbo.Users u ON a.AccountId = u.AccountId INNER JOIN
        dbo.Accounts c  ON a.AccountId = c.ParentId where u.UserId=@UserId")]
        public DataTable AccountsChildList(int UserId)
        {
            return (DataTable)base.Execute(UserId);
        }

        [DBCommand("SELECT CountryId,CountryName from [Countries]")]
        public  DataTable Countries()
       {
          return (DataTable)  base.Execute();
        }

        [DBCommand("SELECT AccountCategoryId,AccountCategoryName from [Accounts_Category]")]
        public  DataTable Accounts_Category()
       {
          return (DataTable)  base.Execute();
        }

        [DBCommand("SELECT AccTypeId,AccTypeName,AccTypeHe from [Accounts_Type]")]
        public  DataTable Accounts_Type()
       {
          return (DataTable)  base.Execute();
        }
        public int GetParentId(int AccountId)
        {
            return base.Dlookup<int>("ParentId", "Accounts", "AccountId=" + AccountId.ToString(), (int)0);
        }

        public int GetAccountNewId()
        {
            return base.DMax<int>("AccountId", "Accounts", "AccountId<79999",null) + 1;
        }

        public bool ValidateAccountIP(int AccountId, string IP)
        {
            return  base.DExists("IP", "Accounts_IP", "AccountId=@AccountId and IP=@IP",new object[]{ AccountId,IP}) ;
        }

        [DBCommand("SELECT * from [vw_Accounts_Registry] where RegisterId=@RegisterId and IP=@IP")]
        public DataRow Account_Registry(string RegisterId, string IP)
        {
            return (DataRow)base.Execute(RegisterId, IP);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Accounts_Create")]
        public int Account_Insert(
        [DbField(DalParamType.Identity)]ref int AccountId,
        [DbField()]int ParentId,
        [DbField()] string AccountName,
        [DbField()] string ContactName,
        [DbField()] string Address,
        [DbField()] string City,
        [DbField()] string ZipCode,
        [DbField()] string Phone,
        [DbField()] string Fax,
        [DbField()] string Mobile,
        [DbField()] string Email,
        [DbField()] string IdNumber,
        [DbField()]int Country,
        [DbField()]int OwnerId,
        [DbField()]int AccType,
        [DbField()]int BusinessGroup,
        [DbField()] bool IsHold,
        [DbField()]int PayType,
        [DbField()]int CreditBillingType,
        [DbField()] string Details,
        [DbField()] bool IsActive,
        [DbField()]int RepId,
            //[DbField()] DateTime CreationDate,
            //[DbField()] DateTime LastUpdate,
        [DbField()]int PriceCode,
        [DbField()]int UsingType,
        [DbField()] string DefaultSender
        )
        {
            object[] values = new object[]{
            AccountId,ParentId,AccountName,ContactName,Address,City,ZipCode,Phone,
                Fax,Mobile,	Email,IdNumber,Country,OwnerId,AccType,BusinessGroup,IsHold,
                PayType,CreditBillingType,Details,IsActive, RepId, PriceCode,UsingType,DefaultSender};

            object o = base.Execute(values);
            AccountId = Types.ToInt(values[0], 0);
            /*
            if (AccountId > 0)
            {
                Accounts_Credit_Insert(AccountId, 0, 0, 0, 0, 0, false, DateTime.Now, 0, 0);
            }*/
            return (int)o;
        }

        [DBCommand(DBCommandType.Insert, "Accounts_Credit")]
        public  void Accounts_Credit_Insert(
        [DbField( DalParamType.Key)]int AccountId,
        [DbField()]decimal Credit,
        [DbField()] decimal Debit,
        [DbField()] decimal Process,
        [DbField()] decimal Pending,
        [DbField()] decimal Obligo,
        [DbField()] bool IsHold,
        [DbField()] DateTime LastUpdate,
        [DbField()] decimal AlertOn,
        [DbField()] int AlertFlag)
        {
            base.Execute(AccountId, Credit, Debit,Process,Pending,Obligo,IsHold,LastUpdate,AlertOn,AlertFlag);
        }

        [DBCommand("update Accounts_Credit set Pending=0 where AccountId=@AccountId")]
        public int Pending_Clear(int AccountId)
        {
            return (int)base.Execute(AccountId);

        }
        [DBCommand("update Accounts_Credit set Pending=0,Process=0 where AccountId=@AccountId")]
        public int PendingAndProcess_Clear(int AccountId)
        {
            return (int)base.Execute(AccountId);

        }

        [DBCommand("update Accounts set City=@City where AccountId=7000")]
        public int UpdateTest(string City)
        {
          return (int)  base.Execute(City);

        }


        [DBCommand(DBCommandType.StoredProcedure, "sp_AccountCreditAlert_Update")]
        public void AccountCreditAlert_Update(
        [DbField()]int AccountId,
        [DbField()]decimal AlertOn,
        [DbField()] bool AlertFlag,
        [DbField()] string Sender)
        {
            base.Execute(AccountId, AlertOn, AlertFlag,Sender);
        }
     

        [DBCommand(DBCommandType.Insert, "Users")]
        public  int User_Insert(
        [DbField( DalParamType.Identity)]ref int UserId,
        [DbField()] string UserName,
        //[DbField()]int GroupId,	
        [DbField()]int Perms,
        [DbField()] string LogInName,
        [DbField()] string Pass,
        [DbField()]int AccountId,
        [DbField()] string Details,
        [DbField()] string Lang,
        [DbField()] string MailAddress,
        [DbField()] string Phone,
        [DbField()] bool IsBlocked,
        [DbField()] bool ConfirmArticle,
        [DbField()]int Evaluation
        )
        {
            object[] values = new object[]{
            UserId,UserName, /*GroupId,*/Perms, LogInName, Pass, AccountId, Details, Lang, 
            MailAddress, Phone, IsBlocked, ConfirmArticle,Evaluation};

            object o = base.Execute(values);
            UserId = Types.ToInt(values[0], 0);
            return (int)o;
        }

        [DBCommand(DBCommandType.Update, "Users")]
        public int User_Update(
        [DbField(DalParamType.Key)]int UserId,
        [DbField()] string UserName,
        //[DbField()]int GroupId,
        [DbField()]int Perms,
        [DbField()] string LogInName,
        [DbField()] string Pass,
        [DbField()]int AccountId,
        [DbField()] string Details,
        [DbField()] string Lang,
        [DbField()] string MailAddress,
        [DbField()] string Phone,
        [DbField()] bool IsBlocked,
        [DbField()] bool ConfirmArticle,
        [DbField()]int Evaluation,
        [DbField()] bool ShowDashboard
        )
        {
            return (int)base.Execute(UserId,
          UserName, /*GroupId,*/ Perms,LogInName, Pass, AccountId, Details, Lang, MailAddress, Phone,
          IsBlocked, ConfirmArticle, Evaluation,ShowDashboard);
        }

        [DBCommand(DBCommandType.Update, "Users")]
        public int User_Update(
        [DbField(DalParamType.Key)]int UserId,
        [DbField()] string UserName,
        [DbField()] string LogInName,
        [DbField()] string Pass,
        [DbField()]int AccountId,
        [DbField()] string Details,
        [DbField()] string Lang,
        [DbField()] string MailAddress,
        [DbField()] string Phone,
        [DbField()] bool ShowDashboard
        )
        {
          return (int)base.Execute(UserId,
          UserName, LogInName, Pass, AccountId, Details, Lang, MailAddress, Phone, ShowDashboard);
        }

        [DBCommand("UPDATE [Users] SET Evaluation=@Evaluation where AccountId=@AccountId")]
        public int User_Evaluation_Update(int Evaluation,int AccountId)
        {
            return (int)base.Execute(Evaluation,AccountId);
        }

        [DBCommand("UPDATE [Users] SET PermsBits='',UserType=0 where UserId=@UserId")]
        public int User_Perms_Clear(int UserId)
        {
            return (int)base.Execute(UserId);
        }

        [DBCommand("SELECT * from [Users] where AccountId=@AccountId")]
        public DataTable Users(int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }

        [DBCommand("SELECT * from [Users_Group] where PermsGroupId not in(1,2) order by PermsGroupId")]
        public DataTable Users_Group()
        {
            return (DataTable)base.Execute();
        }
   
        [DBCommand("SELECT  * from [Users_Group] where SubGroup=@GroupId")]
        public DataTable Users_SubGroup(int GroupId)
        {
            return (DataTable)base.Execute(GroupId);
        }
        [DBCommand(DBCommandType.StoredProcedure, "sp_UserPermsInfo")]
        public DataTable UserPermsInfo
        (
            [DbField()]int UserId
        )
        {
            return (DataTable)base.Execute(UserId);
        }
        [DBCommand(DBCommandType.StoredProcedure, "sp_CreditAdd")]
        public DataRow CreditAdd(
        [DbField()]int AccountId,
        [DbField()] decimal CreditValue,
        [DbField()]string Remarks)
        {
            return (DataRow)base.Execute(AccountId, CreditValue, Remarks);
        }
        [DBCommand(DBCommandType.StoredProcedure, "sp_CreditTransfer")]
        public DataRow CreditTransfer(
        [DbField()]int AccountId,
        [DbField()] decimal CreditValue,
        [DbField()]string Remarks,
         [DbField()]int AccountFrom)
        {
            return (DataRow)base.Execute(AccountId, CreditValue, Remarks, AccountFrom);
        }
        [DBCommand(DBCommandType.StoredProcedure, "sp_Billing_Payed")]
        public int Billing_Payed(
        [DbField(DalParamType.Identity)]ref int BillingId,
        [DbField()]int AccountId,
        [DbField()] decimal CreditValue,
        [DbField()]string Remarks,
        [DbField()]int ActionType,
        [DbField()]int Invoice,
        [DbField()]int UserId)
        {
            object[] values = new  object[] { BillingId,AccountId, CreditValue, Remarks, ActionType ,Invoice,UserId};
            int res= (int)base.Execute(values);
            BillingId = Types.ToInt(values[0], 0);
            return res;
        }



        //===========================================

         [DBCommand("SELECT * from [App_Config]")]
         public DataTable AppConfig()
         {
             return (DataTable)base.Execute();
         }

         public int GetLogIn(string LogInName, string Pass)
         {
             return (int)base.LookupQuery<int>("AccountId", "Users",
                 "LogInName=@LogInName and Pass=@Pass", -1,
                 new object[] { LogInName, Pass });
         }
         public int GetLogIn(int LogInId)
         {
             return (int)base.LookupQuery<int>("AccountId", "Users",
                 "Id=@LogInId and Pass=@Pass", -1,
                 new object[] { LogInId });
         }
         public int GetLogInId(string LogInName, string Pass)
         {
             return (int)base.LookupQuery<int>("UserId", "Users",
                 "LogInName=@LogInName and Pass=@Pass", -1,
                 new object[] { LogInName, Pass });
         }


         [DBCommand(DBCommandType.Lookup, "select top 1 UserId from users where AccountId=@AccountId and IsBlocked=0 and IsActive=1 order by UserType desc", 0)]
         public int GetFirstUserId(int AccountId)
         {
             return (int)base.Execute<int>(AccountId);
         }

         [DBCommand("select top 1 * from users where AccountId=@AccountId and IsBlocked=0 and IsActive=1 order by UserType desc")]
         public DataRow GetFirstUser(int AccountId)
         {
             return (DataRow)base.Execute(AccountId);
         }

         [DBCommand("SELECT top 1 * from [Users] where LogInName=@LogInName and Pass=@Pass")]
         public DataRow User(string LogInName, string Pass)
         {
             return (DataRow)base.Execute(LogInName, Pass);
         }

      
         [DBCommand("SELECT top 1 * from [Users] where UserId=@UserId")]
         public DataRow User(int UserId)
         {
             return (DataRow)base.Execute(UserId);
         }
         public string GetUserPerms(int userId)
         {
             return base.Dlookup("PermsBits", "Users", "UserId=" + userId.ToString(), "0");
         }

          public DataTable UsersSessionInfo(string args)
         {
             string fields = "'' as SessionId,'' as Creation,'' as LastUsed,'false' as IsTimeOut,UserId,UserName,AccountId,AccountName";

             string sql = string.Format("SELECT  {0} from [vw_UsersInfo] where UserId in({1})", fields,args);
             return (DataTable)base.ExecuteCommand<DataTable>(sql);
         }

         [DBCommand("SELECT top 1 * from [vw_UsersInfo] where UserId=@UserId")]
         public DataRow UserInfo(int UserId)
         {
             return (DataRow)base.Execute(UserId);
         }

         [DBCommand("SELECT top 1 * from [vw_UsersInfo] where LogInName=@LogInName and Pass=@Pass")]
         public DataRow UserInfo(string LogInName, string Pass)
         {
             return (DataRow)base.Execute(LogInName, Pass);
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


         public void User_Update_ConfirmArticle(int UserId, string version, string IP, string Name, string Cli, string Mail)
         {
             //base.ExecuteNonQuery(string.Format("Update Users set ConfirmArticle=1,MailAddress='{3}',Phone='{2}',Details=N'{1}' where UserId={0}", UserId,Name,Cli,Mail));
             Users_Articles_Update(UserId,true,Name,Cli,Mail);
             Users_Articles_Insert(UserId, version, DateTime.Now,IP,Name,Cli,Mail);
         }

         [DBCommand(DBCommandType.Update, "Users")]
         public int User_ConfirmArticle
         (

             [DbField(DalParamType.Key)]int UserId,
             [DbField()]bool ConfirmArticle

         )
         {
             return (int)base.Execute(UserId, ConfirmArticle);
         }

         [DBCommand(DBCommandType.Update, "Users")]
         public int Users_Articles_Update
         (
             [DbField(DalParamType.Key)]int UserId,
             [DbField()]bool ConfirmArticle,
             [DbField()]string Details,
             [DbField()]string Phone,
             [DbField()]string MailAddress
         )
         {
             return (int)base.Execute(UserId, ConfirmArticle, Details, Phone, MailAddress);
         }


         [DBCommand(DBCommandType.Insert, "Users_Articles")]
         public int Users_Articles_Insert
         (
             [DbField(DalParamType.Key)]int UserId,
             [DbField()]string Article,
             [DbField()]DateTime ConfirmDate,
             [DbField()]string IP,
             [DbField()]string Name,
             [DbField()]string Cli,
             [DbField()]string Mail
         )
         {
             return (int)base.Execute(UserId, Article, ConfirmDate,IP,Name,Cli,Mail);
         }


         public string[] AccountToLogIn(int AccountId)
         {
             DataRow dr = base.ExecuteCommand<DataRow>("select LogInName,Pass from [Users] where AccountId=" + AccountId.ToString());
             if (dr == null)
                 return null;
             return new string[] { dr["LogInName"].ToString(), dr["Pass"].ToString() };
         }


         public decimal GetActualCredit(int AccountId)
         {
             return base.Dlookup("ActualCredit", "vw_Accounts_Credit", "AccountId=" + AccountId.ToString(), (decimal)0.00);
         }
         public string GetAccountSender(int AccountId)
         {
             return base.LookupQuery<string>("DefaultSender", "Accounts", "AccountId=@AccountId",null, new object[]{ AccountId.ToString()});
         }
         public string GetAccountMail(int AccountId)
         {
             return base.Dlookup("Email", "Accounts", "AccountId=" + AccountId.ToString(), "");
         }

         public string GetCellSender(int AccountId)
         {
             return base.Dlookup("Sender", "Accounts_Features", "AccountId=" + AccountId.ToString(), "");
         }
         public string GetEmailSender(int AccountId)
         {
             return base.Dlookup("MailSender", "Accounts_Features", "AccountId=" + AccountId.ToString(), "");
         }

         public string GetOwnerName(int OwnerId)
         {
             return base.Dlookup("OwnerName", "Accounts_Owners", "OwnerId=" + OwnerId.ToString(), "EPHONE");
         }

         public decimal GetTotalCreditGroup(int ParentId)
         {
             return base.Dlookup("TotalCredit", "vw_Accounts_CreditGroup", "ParentId=" + ParentId.ToString(), (decimal)0.00);
         }

         [DBCommand("SELECT * from [vw_AccountsCB] where AccountId=@AccountId")]
         public DataRow AccountsCB(int AccountId)
         {
             return (DataRow)base.Execute(AccountId);
         }

         [DBCommand("SELECT * from [vw_AccountStatus] where AccountId=@AccountId")]
         public DataRow AccountStatus(int AccountId)
         {
             return (DataRow)base.Execute(AccountId);
         }

         [DBCommand("SELECT * from [vw_Accounts_Credit] where AccountId=@AccountId")]
         public DataRow AccountCredit(int AccountId)
         {
             return (DataRow)base.Execute(AccountId);
         }

       

         public decimal AccountActualCredit(int AccountId)
         {
             return base.LookupQuery<decimal>("ActualCredit", "vw_Accounts_Credit", "AccountId=@AccountId", 0, new object[] { AccountId });
         }
        

         [DBCommand(DBCommandType.StoredProcedure, "sp_AccountCreditInfo")]
         public DataRow AccountCreditInfo(
             [DbField()]int AccountId,
             [DbField()]string Method
         )
         {
             return (DataRow)base.Execute(AccountId, Method);
         }

    
           [DBCommand(DBCommandType.StoredProcedure, "sp_Billing_Exec")]
         public int Account_Billing(
             [DbField( DalParamType.SPReturnValue)]ref int BillingId,
             [DbField()]int AccountId,
             [DbField()]int CampaignId,
             [DbField()]int TransId,
             [DbField()]decimal CreditValue,
             [DbField()]decimal DefaultPrice,
             [DbField()]int ActionType,
             [DbField()]int UserId,
             [DbField()]string Remarks,
             [DbField()]int Units,
             [DbField()]int Platform,
             [DbField()]int BillingType,
             [DbField()]int MtId
         )
         {
             object[] values = new object[] { BillingId,AccountId, CampaignId, TransId, CreditValue, DefaultPrice, ActionType, UserId, Remarks, Units, Platform,BillingType,MtId };

             object o= base.Execute(values);
             BillingId = Types.ToInt(values[0], 0);
             return (int)o;
         }


         public int GetLogIn(string LogInPass)
         {
             return (int)base.LookupQuery<int>("UserId", "vw_Users_Pass",
                 "ValidPass=@LogInPass", -1,
                 new object[] { LogInPass });
         }
         public int GetLogInAccount(string LogInPass)
         {
             return (int)base.LookupQuery<int>("AccountId", "vw_Users_Pass",
                 "ValidPass=@LogInPass", -1,
                 new object[] { LogInPass });
         }
         public int GetAccountByName(string AccountName)
         {
             return (int)base.LookupQuery<int>("AccountId", "Accounts",
                 "AccountName=@AccountName", -1,
                 new object[] { AccountName });
         }

         public int Accounts_Disable(int[] AccountId)
         {
             if (AccountId == null || AccountId.Length == 0)
                 return 0;
             StringBuilder sb = new StringBuilder();
             foreach (int acc in AccountId)
             {
                 sb.AppendFormat("{0},", acc);
             }
             if (sb.Length > 0)
                 sb.Remove(sb.Length - 1, 1);
             string list = sb.ToString();
             string sql1 = string.Format("update Accounts set IsActive=0 where AccountId in({0})", list);
             string sql2 = string.Format("update Users set IsBlocked=1 where AccountId in({0})", list);
             int res = base.ExecuteNonQuery(sql1);
             base.ExecuteNonQuery(sql2);
             return res;
         }

         #region Account price

         [DBCommand(DBCommandType.StoredProcedure, "sp_Account_Price", null, MissingSchemaAction.AddWithKey)]
         public DataTable Accounts_Price(
         [DbField()]int AccountId,
         [DbField()]bool ActiveOnly,
         [DbField()]int MtId
         )
         {
             return (DataTable)base.Execute(AccountId, ActiveOnly, MtId);
         }
         [DBCommand(DBCommandType.StoredProcedure, "sp_Account_Price", null, MissingSchemaAction.AddWithKey)]
         public DataTable Accounts_Price(
         [DbField()]int AccountId,
         [DbField()]bool ActiveOnly
         )
         {
             return (DataTable)base.Execute(AccountId, ActiveOnly);
         }

         public bool GetAccountPrice(int accountId, int method, ref decimal price)
         {
             price= base.LookupQuery<decimal>("Price", "Accounts_Price", "AccountId=@AccountId and MtId=@MtId and BillingType>0",0m, new object[]{ accountId, method});
             //if (o == null)
             //{
             //    return false;
             //}
             //price = Types.ToDecimal(o, 0);
             return price>0;
         }

         #endregion

  
         #region Features

         public string GetAccount_Segments(int accountId)
         {
             return base.Dlookup("Segments", "Accounts_Features", "AccountId=" + accountId.ToString(), (string)null);
         }

        #endregion

       
    }

 

}
