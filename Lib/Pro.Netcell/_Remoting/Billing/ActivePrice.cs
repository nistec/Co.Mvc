using System;
using System.Data;
using System.ComponentModel;
using Nistec.Data;
using Netcell.Data;
using Netcell.Data.Server;
using Nistec.Data.Entities;

namespace Netcell.Remoting
{
   
	/// <summary>
    /// Summary description for ActivePrice.
	/// </summary>
    public class ActivePrice : Nistec.Data.Entities.ActiveEntity//, IActivePrice
	{
        public readonly int RequestAccId;
        public readonly int  RequestMethod;

       	#region Ctor

        const string activeName = "AccountPrice";


        public ActivePrice(int accountId, MethodType method)
        {
            RequestAccId = accountId;
            RequestMethod =(int) method;
            try
            {
                    base.Init(
                    DalRule.Instance.Accounts_Price(accountId, (int)method));

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

        [EntityProperty( EntityPropertyType.Default)]
        public int AccountId
        {
            get { return base.GetValue<int>("AccountId"); }
        }

        [EntityProperty( EntityPropertyType.Default, false)]
        public int MtId
        {
            get { return base.GetValue<int>("MtId"); }
        }
        //[EntityProperty( EntityPropertyType.Default,false)]
        //public string Method
        //{
        //    get { return base.GetValue<string>("Method"); }
        //}
        [EntityProperty( EntityPropertyType.Default)]
        public int OperatorId
        {
            get { return base.GetValue<int>("OperatorId"); }
        }
        [EntityProperty( EntityPropertyType.Default)]
        public decimal Price
        {
            get { return base.GetValue<decimal>("Price"); }
        }
       
        //[EntityProperty( EntityPropertyType.Default)]
        //public string Curr
        //{
        //    get { return base.GetValue<string>("Curr"); }
        //    set { base.SetValue(value); }
        //}
        [EntityProperty( EntityPropertyType.Default)]
        public int Quota
        {
            get { return base.GetValue<int>("Quota"); }
        }
        [EntityProperty( EntityPropertyType.Default)]
        public int BillingType
        {
            get { return base.GetValue<int>("BillingType"); }
        }
        [EntityProperty( EntityPropertyType.Default)]
        public decimal ExPrice
        {
            get { return base.GetValue<decimal>("ExPrice"); }
        }
       

        [EntityProperty( EntityPropertyType.Default)]
        public int MinQty
        {
            get { return base.GetValue<int>("MinQty"); }
        }

        [EntityProperty( EntityPropertyType.Default)]
        public decimal ItemPrice
        {
            get { return (BillingType > 1) ? ExPrice : Price; }
        }
        [EntityProperty( EntityPropertyType.Default)]
        public int MaxQty
        {
            get { return base.GetValue<int>("MaxQty"); }
        }
 
        #endregion

        public void ValidateAccountPrice()
        {
            if (IsEmpty || BillingType == 0)
            {
                throw new AppException(AckStatus.BillingException, RequestAccId, "Invalid BillingUnit for Account:" + RequestAccId);
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
        /*
       public int CalcUnits(int length, bool isLatin, bool concat)
       {

           if (!concat) return 1;
           if (Method.StartsWith(MediaTypes.SMS))
           {
               return RemoteUtil.GetBillingUnits(0, length, isLatin, concat);
           }
           else
           {
               return CalcUnits(length);
           }
       }


       public BillingItem CalcBillingItem(int count, LengthItem li)
       {
           return CalcBillingItem(count, li.Length, li.IsLatin, li.Concat);
       }

       public BillingItem CalcBillingItem(int count, int length, bool isLatin, bool concat)
       {
           if (BillingType == (int)BillingMethodTypes.None)
           {
               throw new MsgException(AckStatus.BillingException, AccId, "Invalid Billing for method:" + Method);
           }
           int units = count * CalcUnits(length, isLatin, concat);

           switch ((BillingMethodTypes)BillingType)
           {
               case BillingMethodTypes.None:
                   throw new MsgException(AckStatus.BillingException, AccId, "Invalid Billing for method:" + Method);
               case BillingMethodTypes.Unit:
                   return new BillingItem(length, units,Price, BillingType, false);
               case BillingMethodTypes.Campaign:
                   if (units < MinQty)
                       return new BillingItem(length, units, 0m, BillingType, false);
                   if (units > MaxQty)
                   {
                       int ex = (MaxQty - units);
                       return new BillingItem(length, ex, ExPrice, BillingType, true);
                   }
                   return new BillingItem(length, units, Price, BillingType, false);
               case BillingMethodTypes.Monthly:
                   int month_units = GetSumMonthlyUnits();
                   if (month_units > MaxQty)
                   {
                       return new BillingItem(length, (month_units - MaxQty), ExPrice, (int)BillingMethodTypes.Unit, true);
                   }
                   return new BillingItem(length, units, 0m, BillingType, false);
               case BillingMethodTypes.Manual:
                   return new BillingItem(length, units, 0m, BillingType, false);

               default:
                   throw new MsgException(AckStatus.BillingException, AccId, "Billing type not supported:" + BillingType);
           }
       }

       public decimal GetSumMonthlyBilling()
       {
           DateTime t = DateTime.Now;

           DateTime df = new DateTime(t.Year, t.Month, 1);
           DateTime dt = new DateTime(t.Year, t.Month, DateTime.DaysInMonth(t.Year, t.Month));
           
           return DalRule.Instance.GetSumMonthlyPrice(AccountId, Method, dt.Month, dt.Year);
       }

       public int GetSumMonthlyUnits()
       {
           DateTime t = DateTime.Now;

           DateTime df = new DateTime(t.Year, t.Month,1);
           DateTime dt = new DateTime(t.Year, t.Month, DateTime.DaysInMonth(t.Year, t.Month));

           return DalRule.Instance.GetSumMonthlyUnits(AccountId, Method, dt.Month, dt.Year);
       }

  
       #region static methods

       /// <summary>
       /// GetPrice for account and method
       /// </summary>
       /// <param name="accountId"></param>
       /// <param name="operatorId"></param>
       /// <param name="method"></param>
       /// <returns></returns>
       /// <exception cref="MsgException"></exception>
       public static decimal GetPrice(int accountId, string method)
       {
           ActivePrice ap = new ActivePrice(accountId, method);
           if (ap == null || ap.IsEmpty || ap.BillingType == 0)
           {
               throw new MsgException(AckStatus.BillingException, accountId, "Invalid Billing for method:" + method);
           }
           if (ap.BillingType > 1)
               return ap.ExPrice;
           return ap.Price;

           //decimal price = 0m;
           //bool ok = DalRule.Instance.GetAccountPrice(accountId, method, ref price);
           //if (!ok)
           //{
           //    throw new MsgException(AckStatus.BillingException, accountId, "Invalid Price for method:" + method);
           //}
           //return price;
       }

       public static BillingItem GetCalcBillingItem(int accountId, string method, int count, int length)
       {
           return GetCalcBillingItem(accountId, method, count, length, false, true);
       }

       public static BillingItem GetCalcBillingItem(int accountId, string method, int count, int length, bool isLatin, bool concat)
       {
           ActivePrice ap = new ActivePrice(accountId, method);
           if (ap == null || ap.IsEmpty || ap.BillingType == 0)
           {
               throw new MsgException(AckStatus.BillingException, accountId, "Invalid Billing for method:" + method);
           }
           return ap.CalcBillingItem(count, length, isLatin, concat);
       }

       public static BillingItem GetValidBillingItem(int accountId, string method, int count, LengthItem li)
       {
           ActivePrice ap = new ActivePrice(accountId, method);
           if (ap == null || ap.IsEmpty || ap.BillingType == 0)
           {
               throw new MsgException(AckStatus.BillingException, accountId, "Invalid Billing for method:" + method);
           }

           ap.ValidateAccountPrice();

           return ap.CalcBillingItem(count, li);


       }
       public static BillingItem GetAndValidateBillingItem(int accountId, string method, int count, LengthItem li)
       {
           ActivePrice ap = new ActivePrice(accountId, method);
           if (ap == null || ap.IsEmpty || ap.BillingType == 0)
           {
               throw new MsgException(AckStatus.BillingException, accountId, "Invalid Billing for method:" + method);
           }

           ap.ValidateAccountPrice();

           BillingItem bi = ap.CalcBillingItem(count, li);
           decimal price = bi.ItemPrice;
           if (price > 0)
           {
               RemoteUtil.ValidateCredit(accountId, ap.Method, 1, bi.Units, ref price);
           }
           return bi;
       }

       #endregion
        * */
    }
}
