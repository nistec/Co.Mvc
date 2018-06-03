using System;
using System.Data;
using System.ComponentModel;
using Nistec.Data.Entities;
using Netcell.Data.Db;

namespace Netcell.Remoting
{
   
	/// <summary>
    /// Summary description for ActivePrice.
	/// </summary>
    public class ActivePrice : ActiveEntity
	{
        public readonly int RequestAccId;
        public readonly int  RequestMethod;

       	#region Ctor

        const string activeName = "AccountPrice";


        public ActivePrice(int accountId, MethodType method)
        {
            RequestAccId = accountId;
            RequestMethod =(int) method;
            DataRow dr = null;
            try
            {
                using (var dal = DalRule.Instance)
                {
                    dr = dal.Accounts_Price(accountId, (int)method);
                }

                base.Init(dr);
            }
            catch (Exception ex)
            {
                throw new Exception(" Could not load  " + activeName + " " + ex.Message);
            }
        }

        public ActivePrice(DataRow dr)
            : base(dr)
        {
        }

       
        #endregion

        #region properties

        [EntityProperty(EntityPropertyType.Default)]
        public int AccountId
        {
            get { return base.GetValue<int>("AccountId"); }
        }

        [EntityProperty(EntityPropertyType.Default, false)]
        public int MtId
        {
            get { return base.GetValue<int>("MtId"); }
        }
        //[DalProperty(DalPropertyType.Default,false)]
        //public string Method
        //{
        //    get { return base.GetStringValue("Method"); }
        //}
        [EntityProperty(EntityPropertyType.Default)]
        public int OperatorId
        {
            get { return base.GetValue<int>("OperatorId"); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public decimal Price
        {
            get { return base.GetValue<decimal>("Price"); }
        }
       
        //[EntityProperty(EntityPropertyType.Default)]
        //public string Curr
        //{
        //    get { return base.GetStringValue("Curr"); }
        //    set { base.SetValue(value); }
        //}
        [EntityProperty(EntityPropertyType.Default)]
        public int Quota
        {
            get { return base.GetValue<int>("Quota"); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int BillingType
        {
            get { return base.GetValue<int>("BillingType"); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public decimal ExPrice
        {
            get { return base.GetValue<decimal>("ExPrice"); }
        }
       

        [EntityProperty(EntityPropertyType.Default)]
        public int MinQty
        {
            get { return base.GetValue<int>("MinQty"); }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public decimal ItemPrice
        {
            get { return (BillingType > 1) ? ExPrice : Price; }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int MaxQty
        {
            get { return base.GetValue<int>("MaxQty"); }
        }
 
        #endregion

        public void ValidateAccountPrice()
        {
            if (IsEmpty || BillingType == 0)
            {
                throw new MsgException(AckStatus.BillingException, RequestAccId, "Invalid BillingUnit for Account:" + RequestAccId);
            }
            //return true;
        }

        public decimal GetValidPrice()
        {
            if (BillingType > 1)
                return ExPrice;
            return Price;
        }

       
        public int CalcUnits(int bytes)
        {
            if (Quota <= 0)
            {
                return 1;
            }
            int kb = bytes / 1024;
            return (int)Math.Ceiling((decimal)((decimal)kb / (decimal)Quota));
        }

    }
}
