using System;
using System.Data;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using System.Data.SqlClient;
using System.Text;
using Nistec.Data.Factory;
using Nistec.Data;
using Nistec.Data.SqlClient;

namespace Netcell.Data.Db
{
  
    public class DalTrace : DbCommand
    {
        public DalTrace()
            : base(DBRule.CnnTrace)
        {
        }

        public static DalTrace Instance
        {
            get { return new DalTrace(); }
        }

        public void ActiveConnectionClose()
        {
            base.ConnectionClose();
        }

        [DBCommand(DBCommandType.Insert, "Exceptions")]
        public int Exceptions_Insert
        (
          [DbField()]string Description,
          [DbField()]int Priority,
          [DbField()]string Method,
          [DbField()]int Status,
          [DbField()]int AccountId)
        {
            return (int)base.Execute(Description, Priority, Method, Status, AccountId);
        }

        [DBCommand(DBCommandType.Insert, "Trans_Log_Status")]
        public void Trans_Log_Status
            (
            [DbField()]int TransId,
            [DbField()]int AckStatus,
            [DbField()]int Step,
            [DbField()]string Cli,
            [DbField()]string Description
            )
        {
        }

        public int Trans_Log
        (
            [DbField()]int AccountId,
            [DbField()]int MtId,
            [DbField()]int BillType,
            [DbField()]string Login,
            [DbField()]string Password,
            [DbField()]string IP,
            [DbField()]string Module

        )
        {
            int res = base.ExecuteScalar<int>("sp_Trans_Log_Exec", new SqlParameter[] { 
                new SqlParameter("AccountId", AccountId),
                new SqlParameter("MtId", MtId),
                new SqlParameter("BillType", BillType),
                new SqlParameter("Login", Login), 
                new SqlParameter("Password", "***"),
                new SqlParameter("IP", IP),
                new SqlParameter("Module", Module) }, 0,CommandType.StoredProcedure);

            //object o = base.Execute(AccountId, Method, BillingType, Login, Password);
            return res;// Convert.ToInt32(o);

        }

        [DBCommand(DBCommandType.Insert, "Wap_Brows")]
        public int Wap_Brows_Insert(
        [DbField()] string BrowserId,
        [DbField()] string UA,
        [DbField()] string Host,
        [DbField()] int ItemId,
        [DbField()] string App,
        [DbField()]bool IsMobile
        )
        {
            return (int)base.Execute(BrowserId, UA, Host, ItemId, App, IsMobile);
        }

      

        [DBCommand(DBCommandType.Insert, "sp_Trace_Log")]
        public int Trace_Log
            (
            [DbField()] string Subject,
            [DbField()] int Status,
            [DbField()] string StatusDescription,
            [DbField()] DateTime RequestTime,
            [DbField()] string Host,
            [DbField()] string Server,
            [DbField()] string Method,
            [DbField()] string Request,
            [DbField()] int TransId,
            [DbField()] int AccountId,
            [DbField()] int ArgId,
            [DbField()] int InOut
            )
        {
            return (int)base.Execute(null, null, Subject, Status, StatusDescription, RequestTime,
              null, Host, Server, Method, Request, null, TransId, AccountId, ArgId, InOut);
        }

        [DBCommand(DBCommandType.Insert, "sp_Trace_Log")]
        public int Trace_Log
            (
            [DbField()] string RequestId,
            [DbField()] string ParentRequest,
            [DbField()] string Subject,
            [DbField()] int Status,
            [DbField()] string StatusDescription,
            [DbField()] DateTime RequestTime,
            [DbField()] DateTime? ResponseTime,
            [DbField()] string Host,
            [DbField()] string Server,
            [DbField()] string Method,
            [DbField()] string Request,
            [DbField()] string Response,
            [DbField()] int TransId,
            [DbField()] int AccountId,
            [DbField()] int ArgId,
            [DbField()] int InOut
            )
        {
            return (int)base.Execute(RequestId, ParentRequest, Subject, Status, StatusDescription, RequestTime,
              ResponseTime, Host, Server, Method, Request, Response, TransId, AccountId, ArgId, InOut);
        }

      
    
        [DBCommand(DBCommandType.Insert, "sp_Trace_Exec")]
        public int Trace_Exec
            (
            [DbField()] string InteractionId,
            [DbField()] int AccountId,
            [DbField()] int AppId,
            [DbField()] string User,
            [DbField()] string Referr,
            [DbField()] string RawUrl,
            [DbField()] DateTime RequestTime,
            [DbField()] DateTime ResponseTime,
            [DbField()] double Duration,
            [DbField()] string RequestBody,
            [DbField()] string ResponseBody,
            [DbField()] string UserHostAddress,
            [DbField()] int StatusCode,
            [DbField()] string StatusDescription,
            [DbField()] DateTime LoadTime
            )
        {
            return (int)base.Execute(InteractionId,AccountId, AppId, User, Referr, RawUrl,
              RequestTime, ResponseTime, Duration, RequestBody, ResponseBody, UserHostAddress, StatusCode, StatusDescription,LoadTime);
        }
   
 
        [DBCommand(DBCommandType.Insert, "select top 100 * from Exceptions order by ExceptionId desc")]
        public DataTable Exceptions_Top()
        {
            return (DataTable)base.Execute();
        }



    }

}

