using System;
using System.Data;
using System.ComponentModel;
using Netcell.Data.Client;

using Netcell.Data;
using Netcell.Remoting;
using Netcell.Data.Server;
using Nistec;

namespace Netcell.Remoting
{

    public class CreditStatus
    {
        public decimal ActualCredit;
        public decimal ItemPrice;
        public int CreditState;
        public int Units;
        public int ItemUnit;

        //public CreditStatus(DataRow dr, int units)
        //{
        //    ActualCredit = Types.ToDecimal(dr["Credit"], 0);
        //    Cost = Types.ToDecimal(dr["Cost"], 0);
        //    CreditState = Types.ToInt(dr["CreditStatus"], 0);
        //    Count = units;
        //}
        public CreditStatus(decimal credit, decimal price, int creditState, int units, int itemUnit)
        {
            ActualCredit = credit;
            ItemPrice = price;
            CreditState = creditState;
            Units = units;
            ItemUnit = itemUnit;
        }

        public bool HasCredit
        {
            get { return CreditState > 0; }
        }
        public bool InvalidMethodPrice
        {
            get { return CreditState == -1; }
        }
        public bool InvalidItems
        {
            get { return CreditState == -2; }
        }
        public decimal TotalCost
        {
            get { return Units * ItemPrice; }
        }

        public string ToHtml()
        {
            return string.Format("<li><b>{0}</b> {1}</li><li><b>{2}</b> {3}</li>", "עלות משוערת:", TotalCost.ToString(), "ייתרת האשראי הנוכחית:", ActualCredit.ToString());
        }
    }

    public class CreditTransferState
    {
        public decimal ActualCredit;
        public int CreditState;
        public CreditTransferState(DataRow dr)
        {
            if (dr != null)
            {
                ActualCredit = Types.ToDecimal(dr["ActualCredit"], 0);
                CreditState = Types.ToInt(dr["CreditState"], 0);
            }

        }

        public bool HasCredit
        {
            get { return CreditState > 0; }
        }
        public string CreditStateText
        {
            get
            {
                if (CreditState > 0)
                    return "אשראי תקין";
                if (CreditState == -2)
                    return "חריגה ממסגרת אשראי";
                if (CreditState == -1)
                    return "העברה אינה מאושרת לביצוע";
                return "אשראי ללא שינוי";
            }
        }
        public override string ToString()
        {
            return string.Format("{0}:{1} {2}:{3}", "סטטוס:", CreditStateText, "ייתרת האשראי הנוכחית:", ActualCredit.ToString());
        }
        public string ToHtml()
        {
            return string.Format("<li><b>{0}</b> {1}</li><li><b>{2}</b> {3}</li>", "סטטוס:", CreditStateText, "ייתרת האשראי הנוכחית:", ActualCredit.ToString());
        }
    }

    /// <summary>
    /// Summary description for PartnerServices.
    /// </summary>
    public class ActiveCredit : Nistec.Data.Entities.ActiveEntity
    {

        #region Ctor

        const string activeName = "ActiveCredit";

        public ActiveCredit(DataRow dr)
            : base(dr)
        {
        }

        public ActiveCredit(int accountId)
        {
            try
            {
                base.Init(
                DalAccounts.Instance.AccountCredit(accountId));
            }
            catch (Exception ex)
            {
                throw new MsgException(AckStatus.EntityException, " Could not load  " + activeName + " " + ex.Message);
            }
        }


        #endregion

        #region properties

        public int AccountId
        {
            get { return base.GetValue<int>("AccountId"); }
        }
        public decimal Credit
        {
            get { return base.GetValue<decimal>("Credit"); }
        }
        public decimal Debit
        {
            get { return base.GetValue<decimal>("Debit"); }
        }
        public decimal Process
        {
            get { return base.GetValue<decimal>("Process"); }
        }
        public decimal Pending
        {
            get { return base.GetValue<decimal>("Pending"); }
        }
        public decimal Obligo
        {
            get { return base.GetValue<decimal>("Obligo"); }
        }
        public decimal ActualCredit
        {
            get { return base.GetValue<decimal>("ActualCredit"); }
        }
        public string LastUpdate
        {
            get { return base.GetValue<string>("LastUpdate"); }
        }
        public int CreditBillingType
        {
            get { return base.GetValue<int>("CreditBillingType"); }
        }

        

        #endregion


        public static decimal GetCredit(int AccountId)
        {
            using (DalAccounts dal = new DalAccounts())
            {
                return dal.AccountActualCredit(AccountId);
            }
        }

        public static CreditTransferState AddCredit(int AccountId, decimal value, string remark)
        {
            DataRow dr = null;
            using (DalAccounts dal = new DalAccounts())
            {
                dr = dal.CreditAdd(AccountId, value, remark);
            }
            return new CreditTransferState(dr);
        }

        public static CreditTransferState CreditTransfer(int accountFrom, int accountTo, decimal value, string remark)
        {
            DataRow dr = null;
            using (DalAccounts dal = new DalAccounts())
            {
                dr = dal.CreditTransfer(accountTo, value, remark, accountFrom);
            }

            return new CreditTransferState(dr);
        }

        public static CreditStatus GetCreditAndPrice(int AccountId, MethodType Method, int Count, int ItemUnit)
        {

            decimal price = 0;
            decimal credit = 0;
            int creditState = 0;
            if ((Count * ItemUnit) <= 0)
            {
                return new CreditStatus(credit, (decimal)price, -2, Count * ItemUnit, ItemUnit);
            }
            using (DalRule dal = new DalRule())
            {
                dal.ValidateCredit(AccountId, (int)Method, Count * ItemUnit,(int)CreditMode.Active, ref price, ref credit,ref creditState);
            }
            return new CreditStatus(credit, (decimal)price, creditState, Count * ItemUnit, ItemUnit);
        }

        public static CreditStatus GetCreditAndPrice(int AccountId, MethodType Method, int Count, UnitsItem ui)// string Message, byte PersonalLength, bool Concatenate)
        {
            int unit = 1;
            if (MsgMethod.IsSms(Method,true))
            {
                //bool isLatin = RemoteUtil.IsLatin(Message);

                unit = BillingItem.GetBillingUnits(0, Method, ui);
            }
            return GetCreditAndPrice(AccountId, Method, Count, unit);
        }

    }
}
