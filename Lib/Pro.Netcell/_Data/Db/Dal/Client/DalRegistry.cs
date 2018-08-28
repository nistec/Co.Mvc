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
using Nistec.Data.Factory;

namespace Netcell.Data.Client
{


  
    public class DalRegistry : Nistec.Data.SqlClient.DbCommand
    {

        public DalRegistry()
            : base(Netcell.Data.DBRule.CnnNetcell)
        {
        }

        public static DalRegistry Instance
        {
            get { return new DalRegistry(); }
        }

        [DBCommand("SELECT top 1 * from [Registry_Form] where FormId=@FormId and AccountId=@AccountId")]
        public DataRow Registry_Form(int FormId, int AccountId)
        {
            return (DataRow)base.Execute(FormId, AccountId);
        }

        [DBCommand("SELECT top 1 * from [Registry_Form] where FormId=@FormId")]
        public DataRow Registry_FormItem(int FormId)
        {
            return (DataRow)base.Execute(FormId);
        }

        [DBCommand("SELECT * from [Registry_Form] where AccountId=@AccountId")]
        public DataTable Registry_Form(int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }

        [DBCommand("SELECT * from [Registry_Items] where FormId=@FormId")]
        public DataTable Registry_Items(int FormId)
        {
            return (DataTable)base.Execute(FormId);
        }


        [DBCommand("SELECT * from [Registry_Items] where FormId=@FormId and Creation between @DateFrom and @DateTo")]
        public DataTable Registry_Items(int FormId, string DateFrom, string DateTo)
        {
            return (DataTable)base.Execute(FormId, DateFrom,DateTo);
        }

        [DBCommand(DBCommandType.Update, "Registry_Items")]
        public int Registry_Items_Update(
        [DbField( DalParamType.Key)] int RegisterId,
        [DbField()] int Status,
        [DbField()] string LastUpdate
        )
        {
            return (int)base.Execute(RegisterId, Status, LastUpdate);
        }


        //[DBCommand(DBCommandType.Insert, "Registry_Items")]
        //public int Registry_Items_Insert(
        ////[DbField( DalParamType.Key)] int RegisterId,
        //[DbField()] int FormId,
        //[DbField()] string Name,
        //[DbField()] string Cli,
        //[DbField()] string Email,
        //[DbField()] string Company,
        //[DbField()] string Details
        //)
        //{
        //    return (int)base.Execute(FormId, Name, Cli,Email,Company,Details);
        //}

        [DBCommand(DBCommandType.Insert, "Registry_Items")]
        public int Registry_Items_Insert(
        [DbField(DalParamType.Identity)] ref int RegisterId,
        [DbField()] int FormId,
        [DbField()] string Name,
        [DbField()] string Cli,
        [DbField()] string Email,
        [DbField()] string Company,
        [DbField()] string Details,
        [DbField()] int EnableNews,
        [DbField()] int ActionType,
        [DbField()] string Args
        )
        {
            object[] values = new object[] { RegisterId, FormId, Name, Cli, Email, Company, Details, EnableNews, ActionType, Args };
            int res= (int)base.Execute(values);
            RegisterId = Types.ToInt(values[0],0);
            return res;
        }

        [DBCommand("delete from Registry_Items where FormId=@FormId; delete from Registry_Form where FormId=@FormId")]
        public int Registry_Delete(
        [DbField()] int FormId
        )
        {
            return (int)base.Execute(FormId);
        }

    }
   

}
