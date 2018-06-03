using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netcell.Data;
using Netcell.Data.Entities;

namespace Netcell.Remoting
{
    public enum ActionBillingState
    {
        InvalidItems = -2,
        InvalidBillingPrice = -1,
        NotEnoughCredit = 0,
        OkMonthlyBilling = 2,
        OkAlertFlag = 3,
        Ok = 1
    }

    public class PackageRules : Billing_Package_View
    {
        #region ctor
        public PackageRules(int accountId)
            : base(accountId)
        {

        }

        #endregion


        #region static

 

        #endregion



    }
}
