using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data;
using Nistec.Data.Factory;
using Nistec;
using System.Data;

namespace Netcell.Data.Client
{


    public class DalBilling : Nistec.Data.SqlClient.DbCommand
    {

        public DalBilling()
            : base(Netcell.Data.DBRule.CnnNetcell)
        {

        }

        public static DalBilling Instance
        {
            get { return new DalBilling(); }
        }



        [DBCommand(DBCommandType.StoredProcedure, "sp_Accounts_Billing_ToPay")]
        public DataTable Accounts_Billing_ToPay([DbField()]int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Accounts_Billing_Pay")]
        public int Accounts_Billing_Pay
            (
            [DbField()]int AccountId,
            [DbField()]int Invoice,
            [DbField()] decimal CreditValue,
            [DbField()]string Args,
            [DbField(DalParamType.SPReturnValue)]ref int RV
            )
        {
            object[] values = new object[] { AccountId, Invoice, CreditValue, Args, RV };
            int res = (int)base.Execute(values);
            RV = Types.ToInt(values[4]);
            return res;
        }
    }
}
