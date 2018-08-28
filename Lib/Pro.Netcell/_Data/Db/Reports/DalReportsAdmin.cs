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

    public class DalReportsAdmin : Nistec.Data.SqlClient.DbCommand
    {
        public DalReportsAdmin()
            : base(Netcell.Data.DBRule.CnnEphone)
        {
        }

        public static DalReportsAdmin Instance
        {
            get { return new DalReportsAdmin(); }
        }

        public void ActiveConnectionClose()
        {
            base.ConnectionClose();
        }


        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand(DBCommandType.StoredProcedure, "sp_Reports_Admin_Find_Items_b")]
        public DataTable Reports_Admin_Find_Items
        (
        [DbField()]int AccountId,
        [DbField()]string Target,
        [DbField()]int Platform,
        [DbField()]DateTime DateFrom,
        [DbField()]DateTime DateTo,
        [DbField()]bool EnableNotif,
        [DbField()]int UserType
        )
        {
            return (DataTable)base.Execute(AccountId, Target, Platform, DateFrom, DateTo, EnableNotif, UserType);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand(DBCommandType.StoredProcedure, "sp_Reports_CampaignItems_b")]
        public DataTable Reports_CampaignItems
        (
        [DbField()]int ReportType,
        [DbField()]int CampaignId,
        [DbField()]int PlatformType,
        [DbField()]int BatchId,
        [DbField()]int StatusMode,
        [DbField()]string Target
        )
        {
            return (DataTable)base.Execute(ReportType, CampaignId, PlatformType, BatchId, StatusMode, Target);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand(DBCommandType.StoredProcedure, "sp_Reports_Campaign_Manager_b")]
        public DataTable Reports_Campaign_Manager
        (
        [DbField()]int ReportType,
        [DbField()]int AccountId,
        [DbField()]DateTime DateFrom,
        [DbField()]DateTime DateTo,
        [DbField()]int Platform,
        [DbField()]int UserType,
        [DbField()]int SumMode
        )
        {
            return (DataTable)base.Execute(ReportType, AccountId, DateFrom, DateTo, Platform, UserType, SumMode);
        }

         [DataObjectMethod(DataObjectMethodType.Select)]
         [DBCommand(DBCommandType.StoredProcedure, "sp_Reports_Batch_Manager_c")]
        public DataTable Reports_Batch_Manager
        (
        [DbField()]int ReportType,
        [DbField()]int AccountId,
        [DbField()]DateTime DateFrom,
        [DbField()]DateTime DateTo,
        [DbField()]int Platform,
        [DbField()]int UserType,
        [DbField()]int SumMode
        )
        {
            return (DataTable)base.Execute(ReportType, AccountId, DateFrom, DateTo, Platform, UserType, SumMode);
        }

        

        #region Account statisic

        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand(DBCommandType.StoredProcedure, "sp_Accounts_Dash_Report")]
        public DataTable Accounts_Dash_Report(
            [DbField()]int AccountId,
            [DbField()]int UserType
            )
        {


            return (DataTable)base.Execute(AccountId, UserType);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand("SELECT * from [vw_Accounts_Dash_Sum]")]
        public DataTable Accounts_Dash_Sum()
        {
            return (DataTable)base.Execute();
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand("SELECT * from [Accounts_Dash] where AccountId=@AccountId")]
        public DataTable Accounts_Dash_Sum(int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand("SELECT * from [Accounts_Dash]")]
        public DataTable Accounts_Dash_Sum_All()
        {
            return (DataTable)base.Execute();
        }

        //[DataObjectMethod(DataObjectMethodType.Select)]
        //[DBCommand(DBCommandType.StoredProcedure, "sp_Account_Statistic_Report")]
        //public DataTable Accounts_Statistic_Report(
        //[DbField()]int AccountId,
        //[DbField()]int ReportType,
        // [DbField()] int MtId
        //)
        //{

        //    return (DataTable)base.Execute(AccountId, ReportType, MtId);
        //}

        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand(DBCommandType.StoredProcedure, "sp_Account_Statistic_Report_b")]
        public DataTable Accounts_Statistic_Report(
        [DbField()]int AccountId,
        [DbField()]int ReportType,
         [DbField()] int Platform
        )
        {

            return (DataTable)base.Execute(AccountId, ReportType, Platform);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand(DBCommandType.StoredProcedure, "sp_Account_Statistic_Recreate")]
        public DataTable Accounts_Statistic_Refresh(
        [DbField()]int AccountFrom,
        [DbField()]int AccountTo
        )
        {

            return (DataTable)base.Execute(AccountFrom, AccountTo);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Accounts_List(int ParentId, int UType)
        {
            if (UType == 9)//admin
            {
                return Accounts_List_ByAdmin();
            }
            else if (UType == 2)//owner
            {
                return Accounts_List_ByOwner(ParentId);
            }
            else//parent
            {
                return Accounts_List_ByParent(ParentId);
            }
        }

        [DBCommand("SELECT * from [Accounts] where ParentId=@ParentId")]
        public DataTable Accounts_List_ByParent(int ParentId)
        {
            return (DataTable)base.Execute(ParentId);
        }

        [DBCommand("SELECT * from [Accounts] where OwnerId=@OwnerId")]
        public DataTable Accounts_List_ByOwner(int OwnerId)
        {
            return (DataTable)base.Execute(OwnerId);
        }


        [DBCommand("SELECT * from [Accounts]")]
        public DataTable Accounts_List_ByAdmin()
        {
            return (DataTable)base.Execute();
        }
        #endregion

        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand(DBCommandType.StoredProcedure, "sp_Mail_Notify_Report_b")]
        public DataTable Mail_Notify_Report
        (
        [DbField()]int ReportType,//0=all,1=sucess,2=failed,3=filter,4=sum
        [DbField()]int Campaignid,
        [DbField()]int StatusCode
        )
        {
            return (DataTable)base.Execute(ReportType, Campaignid, StatusCode);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Trans_Batch_Sent_Admin_c"), DataObjectMethod(DataObjectMethodType.Select)]
        public DataTable Trans_Batch_Sent_Admin
            (
            [DbField] int ReportType,
            [DbField] int AccountId,
            [DbField] int UserType,
            [DbField] DateTime DateFrom,
            [DbField] DateTime DateTo,
            [DbField] int Platform)
        {
            return (DataTable)base.Execute(new object[] { ReportType, AccountId, UserType, DateFrom, DateTo, Platform });
        }


        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand(DBCommandType.StoredProcedure, "sp_Operators_Balance_Admin_c")]
        public DataTable Operators_Balance_Admin(
            [DbField()]int OperatorId,
            [DbField()]int BalanceType,
            [DbField()]DateTime DateFrom,
            [DbField()]DateTime DateTo
            )
        {


            return (DataTable)base.Execute(OperatorId, BalanceType, DateFrom, DateTo);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand(DBCommandType.StoredProcedure, "sp_Trace_Monitor_Admin")]
        public DataTable Tracs_Monitor_Admin(
            [DbField()]int ReportType
            )
        {


            return (DataTable)base.Execute(ReportType);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand(DBCommandType.StoredProcedure, "sp_Accounts_Balance_Admin")]
        public DataTable Accounts_Balance_Admin(
            [DbField()]int AccountId,
            [DbField()]int BalanceType,
            [DbField()]int FilterType,
            [DbField()]DateTime DateFrom,
            [DbField()]DateTime DateTo
            )
        {
            return (DataTable)base.Execute(AccountId, BalanceType, FilterType, DateFrom, DateTo);
        }

    }

}
