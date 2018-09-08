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
//using System.Web.UI.WebControls;
using Nistec.Data.Factory;

namespace Netcell.Data
{
    public class DalServices : DbCommand
    {
        #region ctor

        public DalServices()
            : base(DBRule.CnnServices)
        {
        }

        public static DalServices Instance
        {
            get
            {
                return new DalServices();
            }
        }
        #endregion

        #region entities

        //public static IDbCmd GetDbCmd
        //{
        //    get { return DbFactory.Create(DBRule.CnnNetcell, ProNetcellvider.SqlServer); }
        //}

        //public static DataRow GetEntity(string tableName, string where, params object[] parameters)
        //{
        //    return Instance.DRow("*", tableName, string.Format(where, parameters));
        //}

        //public static DataView GetEntityList(string valueMember, string displayMember, string tableName, string where, params object[] parameters)
        //{
        //    DataTable dt= Instance.DView(valueMember, displayMember, tableName, string.Format(where, parameters));
        //}

        //public static void BindList(ListControl ctl, string valueMember, string displayMember, string tableName, string where, params object[] parameters)
        //{
        //    ctl.DataTextField = displayMember;
        //    ctl.DataValueField = valueMember;
        //    ctl.DataSource = Instance.DView(valueMember, displayMember, tableName, string.Format(where, parameters));
        //    ctl.DataBind();
        //}

        #endregion

        #region Services

        [DBCommand(DBCommandType.StoredProcedure, "sp_ServiceAlert_Get")]
        public DataRow ServiceAlert_Get()
        {
            return (DataRow)base.Execute();
        }
       
        #endregion

        #region Lookup

        public string Lookup_Template(int TemplateId)
        {
            return base.LookupQuery<string>("Body", "Templates", "TemplateId=@TemplateId", "", new object[] { TemplateId });
        }

        [DBCommand("select * from Templates where TemplateId=@TemplateId")]
        public DataRow Lookup_Template_Row([DbField] int TemplateId)
        {
            return (DataRow)base.Execute(new object[] { TemplateId });
        }


        //public int Lookup_Service_MO
        //    (
        //    [DbField] string KeyCode,
        //    [DbField] string SC,
        //    [DbField] int OperatorId
        //    )
        //{
        //    return base.LookupQuery<int>("ServiceId", "Services_MO", "KeyCode=@KeyCode and SC=@SC and OperatorId=@OperatorId and State=@State", 0, new object[] { KeyCode, SC, OperatorId, 0 });
        //}

        public int Lookup_Service_MO
           (
           [DbField] string KeyCode,
           [DbField] string SC,
           [DbField] int OperatorId
           )
        {
            int ServiceId = 0;
            GetService_Mo(KeyCode, SC, OperatorId, ref ServiceId);
            return ServiceId;
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_GetService_Mo")]
        public int GetService_Mo
        (
          [DbField] string KeyCode,
          [DbField] string SC,
          [DbField] int OperatorId,
          [DbField]ref int ServiceId
        )
        {
            object[] values = new object[] { KeyCode, SC, OperatorId,ServiceId };
            int res = (int)base.Execute(values);
            ServiceId = Types.ToInt(values[3]);
            return res;
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_GetService_Mo_ByIp")]
        public int GetService_Mo_ByIp
        (
          [DbField] string KeyCode,
          [DbField] string SC,
          [DbField] string Ip,
          [DbField]ref int ServiceId
        )
        {
            object[] values = new object[] { KeyCode, SC, Ip, ServiceId };
            int res = (int)base.Execute(values);
            ServiceId = Types.ToInt(values[3]);
            return res;
        }
        #endregion

        #region Mailer

        [DBCommand("SELECT * from [Mail_Host_Relay]")]
        public DataTable Mail_Host_Relay()
        {
            return (DataTable)base.Execute();
        }

        [DBCommand("SELECT * from [Mail_Host] where (IsActive=1  and Server=@Server)")]
        public DataTable Mail_Hosts(int Server)
        {
            return (DataTable)base.Execute(Server);
        }

        [DBCommand("SELECT top 1 * from [Mail_Host] where (HostId=@HostId and IsActive=1)")]
        public DataRow Mail_Host(string HostId)
        {
            return (DataRow)base.Execute(HostId);
        }

        [DBCommand("SELECT top 1 * from [Mail_Host] where (AccountId=@AccountId or AccountId=0)and (HostType=@HostType) and IsActive=1 order by AccountId desc")]
        public DataRow Mail_Host(int AccountId, string HostType)
        {
            return (DataRow)base.Execute(AccountId, HostType);
        }

        //[DBCommand("SELECT m.* from Mail_Channel m where m.IsActive=1 and Server=@Server")]
        //public DataTable Mail_Channels(int Server)
        //{
        //    return (DataTable)base.Execute(Server);
        //}


        #endregion

        #region Scheduler

        /*
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
         */ 
        #endregion
    }

}

