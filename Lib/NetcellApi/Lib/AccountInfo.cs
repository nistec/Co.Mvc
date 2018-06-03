using System;
using System.Data;
using System.Collections;
using System.ComponentModel;


using Netcell.Remoting;
using Nistec.Data.Entities;
using Netcell.Data.Db;
using Nistec;
using Nistec.Data;

namespace Netcell.Lib
{

    /// <summary>
    /// Summary description for AccountsAccount.
    /// </summary>
    public class AccountInfo : ActiveEntity
    {
        const string activeName = "ActiveAccountInfo";

        #region Ctor

        public AccountInfo(int accountId)
        {
            DataRow dr = null;
            try
            {
                using (var dl = DalAccounts.Instance)
                {
                   dr= dl.Accounts(accountId);
                }
                base.Init(dr);
            }
            catch (Exception ex)
            {
                throw new MsgException(AckStatus.EntityException," Could not load  ActiveAccount" + ex.Message);
            }

        }


        #endregion


        //IsHold	bit	Unchecked
         //RepId	int	Unchecked
        //CreationDate	smalldatetime	Checked

        #region Properties


        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId
        {
            get { return GetValue<int>("AccountId"); }
        }
        [EntityProperty(EntityPropertyType.Default,50,false)]
        public string AccountName
        {
            get { return GetValue<string>("AccountName"); }
            set { SetValue(value); }
        }
        public int CreditBillingType
        {
            get { return GetValue<int>("CreditBillingType"); }
            set { SetValue(value); }
        }
        public int ParentId
        {
            get { return GetValue<int>("ParentId"); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int PriceCode
        {
            get { return GetValue<int>("PriceCode"); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int UsingType
        {
            get { return GetValue<int>("UsingType"); }
            set { SetValue(value); }
        }

        public AccUsingType AccUsingType
        {
            get { return (AccUsingType)UsingType; }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public int AccType
        {
            get { return GetValue<int>("AccType"); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int Country
        {
            get { return GetValue<int>("Country"); }
            set { SetValue(value); }
        }
        
        [EntityProperty(EntityPropertyType.Default)]
        public int BusinessGroup
        {
            get { return GetValue<int>("BusinessGroup"); }
            set { SetValue(value); }
        }

        [EntityProperty(EntityPropertyType.Default,50)]
        public string ContactName
        {
            get { return GetValue<string>("ContactName"); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default, 50)]
        public string Address
        {
            get { return GetValue<string>("Address"); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default, 50)]
        public string City
        {
            get { return GetValue<string>("City"); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default, 10)]
        public string ZipCode
        {
            get { return GetValue<string>("ZipCode"); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string Phone
        {
            get { return GetValue<string>("Phone"); }
            set { SetValidValue(value, "", CLI.PhonePattern); }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public string Fax
        {
            get { return GetValue<string>("Fax"); }
            set { SetValidValue(value, "", CLI.PhonePattern); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string Mobile
        {
            get { return GetValue<string>("Mobile"); }
            set { SetValidValue(value, "", CLI.MobilePattern); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string Email
        {
            get { return GetValue<string>("Email"); }
            set { SetValidValue(value, "", Regx.RegexPattern.Email); }
        }
        [EntityProperty(EntityPropertyType.Default,10)]
        public string IdNumber
        {
            get { return GetValue<string>("IdNumber"); }
            set { SetValidValue(value, "", Regx.RegexPattern.Number); }
        }
        [EntityProperty(EntityPropertyType.Default,50)]
        public string Details
        {
            get { return GetValue<string>("Details"); }
            set { SetValue(value); }
        }
        public string CreationDate
        {
            get { return GetValue<string>("CreationDate"); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public DateTime LastUpdate
        {
            get { return GetValue<DateTime>("LastUpdate"); }
            set { SetValue(value); }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public int OwnerId
        {
            get { return GetValue<int>("OwnerId"); }
            set { SetValue(value); }
        }

        public int PayType
        {
            get { return GetValue<int>("PayType"); }
        }

        [EntityProperty(EntityPropertyType.Default, 12, false)]
        public string DefaultSender
        {
            get { return GetValue<string>("DefaultSender"); }
            set { SetValue(value); }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public bool IsActive
        {
            get { return GetValue<bool>("IsActive"); }
            set { SetValue(value); }
        }
        #endregion

        #region Methods
       
        public AccountType GetAccType()
        {
            return (AccountType)AccType;
        }

        public string GetOwnerName()
        {
            return DalAccounts.Instance.GetOwnerName(OwnerId);
        }

        //public int UpdateCommand()
        //{
        //    return base.ExecuteCommand(UpdateCommandType.Update, this, DBRule.NetcellConnection, "Accounts");
        //}
        #endregion

        #region static

        public static int GetAccountByName(string accName)
        {
            if (string.IsNullOrEmpty(accName))
            {
                return -1;
            }
            return DalAccounts.Instance.GetAccountByName(accName);
        }

        public static string GetDefaultSender(int accountId)
        {
            return DalAccounts.Instance.GetAccountSender(accountId);
        }

        public static string GetDefaultEmail(int accountId)
        {
            return DalAccounts.Instance.GetAccountMail(accountId);
        }

        public static int User_Evaluation_Update(int evaluation, int accountId)
        {
            return DalAccounts.Instance.User_Evaluation_Update(evaluation, accountId);
        }

        
        #endregion

    }
}
