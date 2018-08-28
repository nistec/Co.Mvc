using System;
using System.Data;
using System.ComponentModel;
using Nistec.Data;
using Netcell.Data;
using Nistec;
using System.Text.RegularExpressions;
using Netcell.Data.Server;

namespace Netcell.Remoting
{


    public enum CreditMode
    {
        Active = 0,
        Current = 1,
        None = 255
    }

    public class BillingItem
    {
        #region members

        public readonly decimal ItemPrice;
        public readonly decimal TotalPrice;
        public readonly int ItemUnits;
        public readonly int ItemSize;
        public readonly BillingMethodTypes BillingType;
        public readonly bool IsOverQty;
        public readonly int BillingState;
        public readonly MethodType Method;
        public readonly int TotalCount;
        public readonly int UserId;
    
        public int TotalUnits
        {
            get { return ItemUnits * TotalCount; }
        }

        #endregion

        #region ctor

        public BillingItem(int size, int units, decimal itemPrice, int billingType, bool isOver, MethodType method)
        {
            ItemSize = size;
            ItemUnits = units;
            ItemPrice = itemPrice;
            TotalPrice = itemPrice * units;
            BillingType = (BillingMethodTypes)billingType;
            IsOverQty = isOver;
            BillingState = 0;
            Method = method;// MsgMethod.ToMethodType(method);
        }

        public BillingItem(int accountId, MethodType method, int length, int count, int userId, byte bunch)
            : this(new ActivePrice(accountId, method), new UnitsItem(length, false, true,bunch), count, userId)
        {

        }

        public BillingItem(int accountId, MethodType method, UnitsItem ui, int count, int userId)
            : this(new ActivePrice(accountId, method), ui, count, userId)
        {

        }

        public BillingItem(ActivePrice ap, UnitsItem ui, int count,int userId)
        {

            //ActivePrice ap = new ActivePrice(accountId, method);
            if (ap == null)
            {
                throw new MsgException(AckStatus.BillingException, "Invalid Billing item, ActivePrice is null");
            }
            if (ap.IsEmpty || ap.BillingType == 0)
            {
                throw new MsgException(AckStatus.BillingException, ap.RequestAccId, "Invalid Billing for method:" + ap.RequestMethod);
            }
            ItemPrice = ap.GetValidPrice();
            ItemSize = ui.Length;
            BillingType = (BillingMethodTypes)ap.BillingType;
            IsOverQty = false;
            BillingState = 0;
            Method = MsgMethod.ToMethodType(ap.MtId);
            TotalCount = count;
            UserId = userId;
            
            int units = CalcUnits((MethodType)ap.MtId, ap.Quota, ui.Length, ui.IsLatinCharSet, ui.Concat,ui.Bunch);
 
            switch ((BillingMethodTypes)ap.BillingType)
            {
                case BillingMethodTypes.None:
                    throw new MsgException(AckStatus.BillingException, ap.RequestAccId, "Invalid Billing for method:" + MsgMethod.ToMethodType(ap.MtId).ToString());
                case BillingMethodTypes.Unit:
                    ItemPrice = ap.Price;
                    ItemUnits = units;
                    IsOverQty = false;
                    break;
                case BillingMethodTypes.Campaign:
                    if (units < ap.MinQty)
                    {
                        ItemPrice = 0m;
                        ItemUnits = units;
                        IsOverQty = false;
                    }
                    else if (units > ap.MaxQty)
                    {
                        ItemUnits = (ap.MaxQty - units);
                        ItemPrice = ap.ExPrice;
                        IsOverQty = true;
                    }
                    else
                    {
                        ItemUnits = units;
                        ItemPrice = ap.Price;
                        IsOverQty = false;
                    }
                    break;
                case BillingMethodTypes.Monthly:
                    int month_units = GetSumMonthlyUnits(ap.AccountId, (MethodType)ap.MtId);
                    if (month_units > ap.MaxQty)
                    {
                        ItemUnits = (month_units - ap.MaxQty);
                        ItemPrice = ap.ExPrice;
                        BillingType = BillingMethodTypes.Unit;
                        IsOverQty = true;
                    }
                    else
                    {
                        ItemUnits = units;
                        ItemPrice = 0m;
                        IsOverQty = false;
                    }
                    break;
                case BillingMethodTypes.Manual:
                    ItemUnits = units;
                    ItemPrice = 0m;
                    IsOverQty = true;
                    break;
                default:
                    throw new MsgException(AckStatus.BillingException, ap.RequestAccId, "Billing type not supported:" + ap.BillingType);
            }

            //TotalPrice = ItemPrice * ItemUnits;
            this.TotalPrice = (this.TotalCount * this.ItemPrice) * this.ItemUnits;


        }

  

        #endregion

        #region public methods
        /*
        public int RenderFreezBilling(int AccountId, int CampaignId, int UserId)
        {
            int billingId = 0;
            PlatformType pm = MsgMethod.GetPlatform(Method.ToString());
            DalRule.Instance.Account_Billing(ref billingId, AccountId, CampaignId, 0, TotalPrice, ItemPrice, 5, UserId, "Render Campaign Billing", ItemUnits, (int)pm);
            return billingId;
        }
        public int RenderFreezBilling(int AccountId, int CampaignId, int TransId, int UserId)
        {
            int billingId = 0;
            PlatformType pm = MsgMethod.GetPlatform(Method.ToString());
            DalRule.Instance.Account_Billing(ref billingId, AccountId, CampaignId, TransId, TotalPrice, ItemPrice, 5, UserId, "Render Trans Billing", ItemUnits, (int)pm);
            return billingId;
        }*/
        #endregion

        #region override
        public override string ToString()
        {
            return string.Format("Billing Price:{0},Billing Type:{1},Is Over Quota:{2}", TotalPrice, BillingType.ToString(), IsOverQty);
        }

        #endregion

        #region CalcBilling

        //public static BillingItem CalcBillingItem(ActivePrice ap, int count, UnitsItem iu)
        //{
        //    return CalcBillingItem(ap, count, iu.Length, iu.CharSet, iu.Concat);
        //}

        //public static BillingItem CalcBillingItem(ActivePrice ap, int count, int length, CharSet charset, bool concat)
        //{
        //    if (ap.BillingType == (int)BillingMethodTypes.None)
        //    {
        //        throw new MsgException(AckStatus.BillingException, ap.RequestAccId, "Invalid Billing for method:" + ap.Method);
        //    }
        //    int units = count * CalcUnits(ap.Method, ap.Quota, length, UnitsItem.IsLatin(charset), concat);

        //    switch ((BillingMethodTypes)ap.BillingType)
        //    {
        //        case BillingMethodTypes.None:
        //            throw new MsgException(AckStatus.BillingException, ap.RequestAccId, "Invalid Billing for method:" + ap.Method);
        //        case BillingMethodTypes.Unit:
        //            return new BillingItem(length, units, ap.Price, ap.BillingType, false,ap.Method);
        //        case BillingMethodTypes.Campaign:
        //            if (units < ap.MinQty)
        //                return new BillingItem(length, units, 0m, ap.BillingType, false,ap.Method);
        //            if (units > ap.MaxQty)
        //            {
        //                int ex = (ap.MaxQty - units);
        //                return new BillingItem(length, ex, ap.ExPrice, ap.BillingType, true,ap.Method);
        //            }
        //            return new BillingItem(length, units, ap.Price, ap.BillingType, false,ap.Method);
        //        case BillingMethodTypes.Monthly:
        //            int month_units = GetSumMonthlyUnits(ap.AccountId,ap.Method);
        //            if (month_units > ap.MaxQty)
        //            {
        //                return new BillingItem(length, (month_units - ap.MaxQty), ap.ExPrice, (int)BillingMethodTypes.Unit, true,ap.Method);
        //            }
        //            return new BillingItem(length, units, 0m, ap.BillingType, false,ap.Method);
        //        case BillingMethodTypes.Manual:
        //            return new BillingItem(length, units, 0m, ap.BillingType, false,ap.Method);

        //        default:
        //            throw new MsgException(AckStatus.BillingException, ap.AccId, "Billing type not supported:" + ap.BillingType);
        //    }
        //}
        #endregion

        #region Monthly helper

        static decimal GetSumMonthlyBilling(int AccountId, MethodType Method)
        {
            DateTime t = DateTime.Now;

            DateTime df = new DateTime(t.Year, t.Month, 1);
            DateTime dt = new DateTime(t.Year, t.Month, DateTime.DaysInMonth(t.Year, t.Month));

            return DalRule.Instance.GetSumMonthlyPrice(AccountId, (int)Method, dt.Month, dt.Year);
        }

        static int GetSumMonthlyUnits(int AccountId, MethodType Method)
        {
            DateTime t = DateTime.Now;

            DateTime df = new DateTime(t.Year, t.Month, 1);
            DateTime dt = new DateTime(t.Year, t.Month, DateTime.DaysInMonth(t.Year, t.Month));

            return DalRule.Instance.GetSumMonthlyUnits(AccountId, (int)Method, dt.Month, dt.Year);
        }
        #endregion

        #region Pricees

        /// <summary>
        /// GetPrice for account and method
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="operatorId"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        /// <exception cref="MsgException"></exception>
        public static decimal GetPrice(int accountId, MethodType method)
        {
            ActivePrice ap = new ActivePrice(accountId, method);
            if (ap == null || ap.IsEmpty || ap.BillingType == 0)
            {
                throw new MsgException(AckStatus.BillingException, accountId, "Invalid Billing for method:" + method);
            }
           
            return ap.GetValidPrice();
        }

        //public static BillingItem CalcBillingItem(int accountId, string method, int count, int length)
        //{
        //    return CalcBillingItem(accountId, method, count, length, false, true);
        //}

        //public static BillingItem CalcBillingItem(int accountId, string method, int count, int length, bool isLatin, bool concat)
        //{
        //    ActivePrice ap = new ActivePrice(accountId, method);
        //    if (ap == null || ap.IsEmpty || ap.BillingType == 0)
        //    {
        //        throw new MsgException(AckStatus.BillingException, accountId, "Invalid Billing for method:" + method);
        //    }
        //    return CalcBillingItem(ap, count, length, isLatin, concat);
        //}

        //public static BillingItem GetValidBillingItem(int accountId, string method, int count, ItemUnits iu)
        //{
        //    ActivePrice ap = new ActivePrice(accountId, method);
        //    if (ap == null || ap.IsEmpty || ap.BillingType == 0)
        //    {
        //        throw new MsgException(AckStatus.BillingException, accountId, "Invalid Billing for method:" + method);
        //    }

        //    ap.ValidateAccountPrice();

        //    return CalcBillingItem(ap,count, iu);


        //}
        //public static BillingItem GetAndValidateBillingItem(int accountId, string method, int count, ItemUnits iu)
        //{
        //    ActivePrice ap = new ActivePrice(accountId, method);
        //    if (ap == null || ap.IsEmpty || ap.BillingType == 0)
        //    {
        //        throw new MsgException(AckStatus.BillingException, accountId, "Invalid Billing for method:" + method);
        //    }

        //    ap.ValidateAccountPrice();

        //    BillingItem bi = CalcBillingItem(ap,count, iu);
        //    decimal price = bi.ItemPrice;
        //    if (price > 0)
        //    {
        //        ValidateCredit(accountId, ap.Method, 1, bi.Units, ref price);
        //    }
        //    return bi;
        //}
        /*
        public static BillingItem CreateBillingItem(int accountId, MethodType method, UnitsItem ui, int count, int userId, bool validateCredit)
        {
            if (accountId <= 0)
            {
                throw new MsgException(AckStatus.BillingException, "Invalid Account Billing for method:" + method);
            }
            ActivePrice ap = new ActivePrice(accountId, method);
            if (ap == null || ap.IsEmpty || ap.BillingType == 0)
            {
                throw new MsgException(AckStatus.BillingException, accountId, "Invalid Billing for method:" + method);
            }

            ap.ValidateAccountPrice();
            BillingItem bi = new BillingItem(ap, ui, count,userId);
            if (validateCredit)
            {
                decimal price = bi.ItemPrice;
                if (price > 0)
                {
                    ValidateCredit(accountId, (MethodType)ap.MtId, count, bi.ItemUnits, ref price);
                }
            }
            return bi;
        }
          
        public static BillingItem CreateBillingItem(int accountId, MethodType method, string message, bool enableImageSize, int count, int userId, bool validateCredit)
        {
            return CreateBillingItem(accountId, method, new UnitsItem(message, enableImageSize), count, userId, validateCredit);
        }
          
        */
        public static BillingItem CreateBillingItem(int accountId, MethodType method, UnitsItem ui, int count, int userId, CreditMode creditMode)
        {

            if (accountId <= 0)
            {
                throw new MsgException(AckStatus.BillingException, "Invalid Account Billing for method:" + method);
            }
            ActivePrice ap = new ActivePrice(accountId, method);
            if (ap == null || ap.IsEmpty || ap.BillingType == 0)
            {
                throw new MsgException(AckStatus.BillingException, accountId, "Invalid Billing for method:" + method);
            }

            ap.ValidateAccountPrice();
            BillingItem bi = new BillingItem(ap, ui, count, userId);

            if (creditMode != CreditMode.None)
            {
                decimal price = bi.ItemPrice;
                if (price > 0)
                {
                    CreditState state = ValidateCreditState(accountId, method, count, bi.ItemUnits, creditMode, ref price);
                    CreditStateValidate(state, accountId);
                    //ValidateCredit(accountId, (MethodType)ap.MtId, count, bi.ItemUnits, ref price);
                }
            }
            return bi;
        }
        public static BillingItem CreateBillingItem(int accountId, MethodType method, string message, bool enableImageSize, int count, int userId, CreditMode creditMode, byte bunch)
        {
            return CreateBillingItem(accountId, method, new UnitsItem(message, enableImageSize,bunch), count, userId, creditMode);
        }

        #endregion
        
        #region Credit

        public static CreditState CheckCredit(int AccountId, MethodType Method, int Count, UnitsItem ui)
        {
            int units = CalcUnits(Method, 0, ui.Length, ui.IsLatinCharSet, ui.Concat,ui.Bunch);
            if (Count == 0)
            {
                return CreditState.InvalidItems;
            }
            //if (units == 0)
            //{
            //    throw new AppException(AckStatus.ArgumentException, "Units must be greater then zero");
            //}
            int totalUnits = Count * units;

            int res = DalRule.Instance.ValidateCredit(AccountId, (int)Method, totalUnits);
            return (CreditState)res;
           
        }

        public static bool ValidateCredit(int AccountId, MethodType Method, int Count, UnitsItem ui)
        {
            int units = CalcUnits(Method, 0, ui.Length, ui.IsLatinCharSet, ui.Concat, ui.Bunch);
            return ValidateCredit(AccountId,Method,Count,units);
        }

        public static bool ValidateCredit(int AccountId, MethodType Method, int Count, UnitsItem ui, ref decimal Price)
        {
            int units = CalcUnits(Method, 0, ui.Length, ui.IsLatinCharSet, ui.Concat,ui.Bunch);
            return ValidateCredit(AccountId, Method, Count, units, ref Price);
        }

        /// <summary>
        /// ValidateCredit
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="Method"></param>
        /// <param name="Count"></param>
        /// <param name="Units"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        public static bool ValidateCredit(int AccountId, MethodType Method, int Count, int Units)
        {
            if (Count == 0)
            {
                throw new AppException(AckStatus.BadDestination, "Invalid destination count");
            }
            if (Units == 0)
            {
                throw new AppException(AckStatus.ArgumentException, "Units must be greater then zero");
            }
            int totalUnits = Count * Units;
            int res = DalRule.Instance.ValidateCredit(AccountId, (int)Method, totalUnits);
            switch ((CreditState)res)
            {
                case CreditState.InvalidBillingPrice://-1
                    throw new AppException(AckStatus.BillingException, "Invalid Billing Price");
                case CreditState.NotEnoughCredit://0
                    throw new AppException(AckStatus.NotEnoughCredit, string.Format("Not Enough Credit, {0}", AccountId));
                case CreditState.OkAlertFlag://3
                    //ActiveSystemAlert.SendCreditAlert(AccountId, ViewConfig.CreditMessage);
                    RemoteApi.Instance.ExecuteSystemAlert(1, (int)AckStatus.WarningAlert, AccountId, ViewConfig.CreditMessage, 0, "BillingItem");
                    return true;
                default:
                    return true;
            }

        }
       
        /// <summary>
        /// ValidateCredit
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="Method"></param>
        /// <param name="Count"></param>
        /// <param name="Units"></param>
        /// <param name="instance"></param>
        /// <param name="Price"></param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        public static bool ValidateCredit(int AccountId, MethodType Method, int Count, int ItemUnits, ref decimal Price)
        {
            //if (Count == 0)
            //{
            //    throw new AppException(AckStatus.BadDestination, "Invalid destination count");
            //}
            //if (ItemUnits == 0)
            //{
            //    throw new AppException(AckStatus.ArgumentException, "Units must be greater then zero");
            //}
            //int totalUnits = Count * ItemUnits;
            //int res = 0;
            //decimal Credit = 0M;
            //DalRule.Instance.ValidateCredit(AccountId, (int)Method, totalUnits, ref Price, ref Credit, ref res);
            
            CreditState res=ValidateCreditState(AccountId, Method, Count, ItemUnits, CreditMode.Active, ref Price);
            switch ((CreditState)res)
            {
                case CreditState.InvalidItems://-1
                    throw new AppException(AckStatus.BadDestination, "Invalid destination count or Items units");
                case CreditState.InvalidBillingPrice://-1
                    throw new AppException(AckStatus.BillingException, "Invalid Billing Price");
                case CreditState.NotEnoughCredit://0
                    throw new AppException(AckStatus.NotEnoughCredit, string.Format("Not Enough Credit, {0}", AccountId));
                case CreditState.OkAlertFlag://3
                    RemoteApi.Instance.ExecuteSystemAlert(1, (int)AckStatus.WarningAlert, AccountId, ViewConfig.CreditMessage, 0, "BillingItem");
                    //ActiveSystemAlert.SendCreditAlert(AccountId, ViewConfig.CreditMessage);
                    return true;
                default:
                    return true;
            }
        }
        
        /// <summary>
        /// Validate Credit state
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="Method"></param>
        /// <param name="Count"></param>
        /// <param name="Units"></param>
        /// <param name="Mode"></param>
        /// <param name="instance"></param>
        /// <param name="Price"></param>
        /// <returns></returns>
        public static CreditState ValidateCreditState(int AccountId, MethodType Method, int Count, int ItemUnits, CreditMode Mode, ref decimal Price)
        {
            if (Count == 0 || ItemUnits == 0)
            {
                return CreditState.InvalidItems;
            }

            int totalUnits = Count * ItemUnits;
            int res = 0;
            decimal Credit = 0M;
            DalRule.Instance.ValidateCredit(AccountId, (int)Method, totalUnits, (int)Mode, ref Price, ref Credit, ref res);
            if ((CreditState)res == CreditState.OkAlertFlag)
            {
                RemoteApi.Instance.ExecuteSystemAlert(1, (int)AckStatus.WarningAlert, AccountId, ViewConfig.CreditMessage, 0, "BillingItem");
            }

            return (CreditState)res;

        }

        public static void CreditStateValidate(CreditState state, int AccountId)
        {
            switch (state)
            {
                case CreditState.InvalidItems:
                    throw new AppException(AckStatus.BadDestination, "Invalid destination count or Items units");
                case CreditState.InvalidBillingPrice:
                    throw new AppException(AckStatus.BillingException, "Invalid Billing Price");
                case CreditState.NotEnoughCredit:
                    throw new AppException(AckStatus.NotEnoughCredit, string.Format("Not Enough Credit, {0}", AccountId));
            }
        }

        #endregion

        #region calc static

        static int CalcUnits(int Quota, int bytes)
        {
            if (Quota <= 0)
            {
                return 1;
            }
            int kb = (int)((float)bytes / (float)1024);
            return (int)Math.Ceiling((decimal)((decimal)kb / (decimal)Quota));
        }

        static int CalcUnits(MethodType method, int Quota, int length, bool isLatin, bool concat, byte bunch)
        {

            if (!concat) return 1;
            if (MsgMethod.IsSms(method, true))
            {
                return OperatorsRules.GetBillingUnits(0, length, isLatin, concat, bunch);
            }
            //else if (MsgMethod.IsWap(method))
            //{
            //    return OperatorsRules.GetBillingUnits(0, length, isLatin, concat);
            //}
            else
            {
                return CalcUnits(Quota, length);
            }
        }

        #endregion

        #region BillingUnits
 
        public static int GetBillingUnits(int OperatorId, MethodType Method, UnitsItem ui)
        {

            int unit = 1;
            if (MsgMethod.IsSms(Method, true) && ui.Concat)
            {
                if (ui.Length <= 0)
                {
                    throw new AppException(AckStatus.InvalidContent, "Invalid message");
                }
                unit = OperatorsRules.GetBillingUnits(OperatorId, ui.Length, ui.IsLatinCharSet, ui.Concat, ui.Bunch);
            }
            else if (MsgMethod.IsMail(Method) )
            {
                if (ui.Length <= 0)
                {
                    throw new AppException(AckStatus.InvalidContent, "Invalid message");
                }
                unit = CalcUnits(0, ui.Length);
            }
            return unit;
        }

        public static int GetBillingUnits(int OperatorId, MethodType Method, string message, bool enableImageizeOrConcat, byte bunch)
        {
            UnitsItem ui = UnitsItem.CreateUnitsItem(Method, message, enableImageizeOrConcat, bunch);
 
            int unit = 1;
            if (MsgMethod.IsSms(Method, true) && ui.Concat)
            {
                if (ui.Length <= 0)
                {
                    throw new AppException(AckStatus.InvalidContent, "Invalid message");
                }
                unit = OperatorsRules.GetBillingUnits(OperatorId, ui.Length, ui.IsLatinCharSet, ui.Concat, ui.Bunch);
            }
            else if (MsgMethod.IsMail(Method))
            {
                if (ui.Length <= 0)
                {
                    throw new AppException(AckStatus.InvalidContent, "Invalid message");
                }
                unit = CalcUnits(0, ui.Length);
            }
            return unit;
        }

        public static int GetSMSBillingUnits(int OperatorId, string message, bool isSWP, bool enableImageizeOrConcat, byte bunch, out bool isLatin)
        {
            UnitsItem ui = new UnitsItem(message, isSWP? UnitsItem.SwpLinkLength: 0, enableImageizeOrConcat,bunch);

            isLatin = ui.IsLatinCharSet;

            int unit = 1;

            if (ui.Length <= 0)
            {
                throw new AppException(AckStatus.InvalidContent, "Invalid message");
            }
            unit = OperatorsRules.GetBillingUnits(OperatorId, ui.Length, ui.IsLatinCharSet, ui.Concat,bunch);

            return unit;
        }
        #endregion

        public static BillingItem BatchBillingItem(int accountId, MethodType method, string content, byte bunch)
        {

            if (content == null || content.Length == 0)
            {
                throw new ArgumentNullException("BatchBillingItem.content");
            }
            bool concat = MsgMethod.IsMail(method) ? false : true;
            return new BillingItem(accountId, method, new UnitsItem(method, content, concat, bunch), 1, 0);
        }

        public static BillingItem CampaignBillingItem(int accountId, MethodType method, string content, byte bunch)
        {

            if (content == null || content.Length == 0)
            {
                throw new ArgumentNullException("CampaignBillingItem.content");
            }
            bool concat = MsgMethod.IsMail(method) ? false : true;
            return new BillingItem(accountId, method, new UnitsItem(method, content, concat,bunch), 1, 0);
        }

        public static int CampaignBillingUnit(int accountId, MethodType method, string content, byte bunch)
        {
            BillingItem bi = CampaignBillingItem(accountId, method, content,bunch);
            return bi.ItemUnits;
        }

    }

    public class BillingRB
    {

        #region RB
        /*
        /// <summary>
        /// Validte Service Code
        /// </summary>
        /// <param name="priceCode"></param>
        /// <param name="SC"></param>
        /// <exception cref="AppException"></exception>
        /// <returns></returns>
        public static int ValidtePricingRB(string priceCode, string SC)
        {
            if (priceCode == null || !RemoteUtil.IsValidString(priceCode))
            {
                throw new AppException(AckStatus.InvalidServiceCode, "Invalid price code");
            }
            bool ok = DalRule.Instance.ValidatePricingRB(priceCode, SC);
            if (!ok)
            {
                throw new AppException(AckStatus.InvalidServiceCode, "Invalid price code");
            }
            return Types.ToInt(priceCode, 0);
        }

        public static int ValidtePricingRB(string priceCode)
        {
            if (priceCode == null || !RemoteUtil.IsValidString(priceCode))
            {
                throw new AppException(AckStatus.InvalidServiceCode, "Invalid price code");
            }
            bool ok = DalRule.Instance.ValidatePricingRB(priceCode);
            if (!ok)
            {
                throw new AppException(AckStatus.InvalidServiceCode, "Invalid price code");
            }
            return Types.ToInt(priceCode, 0);
        }
        */
        #endregion
    }

    public enum CharSet
    {
        Unicode=0,
        Ascii = 1
    }
   
    public class UnitsItem
    {
        #region const


        public const int SingleCellQuota = 66;
        public const int DoubleCellQuota = 132;

        //public const int ConcatLimit = 66;
        //public const int DefaultQuta = 250;
        public const int SwpLinkLength = 26;//http://m.ephone.org.il/123456789.wap
        
        public static int DefaultSmsUnitLength_He = 70;
        public static int DefaultSmsUnitLength_En = 160;


        #endregion

        #region Charset

        public bool IsLatinCharSet
        {
            get{return CharSet == CharSet.Ascii;}
        }

        public static bool IsLatin(CharSet charset)
        {
            return charset == CharSet.Ascii;
        }
        public static bool IsLatin(string s)
        {
            return GetCharSet(s) == CharSet.Ascii;
        }
        public static CharSet GetCharSet(bool isLatin)
        {
            return isLatin ? CharSet.Ascii : CharSet.Unicode;
        }
        public static CharSet GetCharSet(string s)
        {
            if (s == null)
                return CharSet.Ascii;
            byte[] message_byte_array = System.Text.Encoding.UTF8.GetBytes(s);

            foreach (byte b in message_byte_array)
            {
                if (b > 127)
                {
                    return CharSet.Unicode;
                }
            }
            return CharSet.Ascii;

        }
        #endregion
                
        #region members
        public readonly byte Bunch;
        public readonly bool Concat;
        public readonly CharSet CharSet;
        public readonly int Length;
        public readonly int ContentSize;
        //public readonly int Units;
        //public readonly string Method;
        #endregion
                
        #region ctor

        public UnitsItem(int length, bool islatin, bool concat, byte bunch)
        {
            Length = length;
            CharSet = GetCharSet(islatin);
            Concat = concat;
            ContentSize = 0;
            Bunch = bunch;
            //Method = MethodTypes.SMSMT;
            //Units = GetBillingUnits(0, length, islatin, concat);
        }
        public UnitsItem(int length, int contentSize, byte bunch)
        {
            Length = length;
            CharSet = CharSet.Unicode;
            Concat = true;
            ContentSize = contentSize;
            Bunch = bunch;
            //Method = MethodTypes.SMSMT;
            //Units = GetBillingUnits(0, length, islatin, concat);
        }

        public UnitsItem(MethodType method, string message, bool enableImageizeOrConcat, byte bunch)
        {
            if (MsgMethod.IsMail(method))
            {
                try
                {
                    int html_size = System.Text.Encoding.UTF8.GetBytes(message).Length;
                    int size = 0;

                    if (enableImageizeOrConcat)
                    {
                        string pattern = @"<img.*?>";

                        MatchCollection matchCol = Regex.Matches(message, pattern, RegexOptions.IgnoreCase);
                        foreach (Match m in matchCol)
                        {
                            string src = FormatImgSrc(m);
                            size += GetImageSize(src);
                        }
                    }
                    int images_size = size;

                    CharSet = CharSet.Unicode;
                    Length = html_size;
                    Concat = enableImageizeOrConcat;
                    ContentSize = size;
                    Bunch = bunch;
                }
                catch (Exception ex)
                {
                    //ApiLog.ErrorFormat("CalcMailSize Error:{0}", ex.Message);

                    throw new MsgException(AckStatus.BillingException, string.Format("Calc Message Size Error:{0}", ex.Message));
                }
            }
            else
            {
                CharSet = GetCharSet(message);
                Length = message.Length;
                Concat = enableImageizeOrConcat;
                ContentSize = 0;
            }
        }

        public UnitsItem(string message, bool enableImageize, byte bunch)
        {

            try
            {
                int size = 0;

                if (enableImageize)
                {
                    string pattern = @"<img.*?>";

                    MatchCollection matchCol = Regex.Matches(message, pattern, RegexOptions.IgnoreCase);
                    foreach (Match m in matchCol)
                    {
                        string src = FormatImgSrc(m);
                        size += GetImageSize(src);
                    }

                    int images_size = size;

                    Concat = enableImageize;

                }

                CharSet = GetCharSet(message);
                //CharSet = CharSet.Unicode;
                Length = message.Length;
                Concat = true;
                ContentSize = size;
                Bunch = bunch;
            }
            catch (Exception ex)
            {
                //ApiLog.ErrorFormat("CalcMailSize Error:{0}", ex.Message);

                throw new MsgException(AckStatus.BillingException, string.Format("Calc Message Size Error:{0}", ex.Message));
            }
        }

        public UnitsItem(string message, int addlength, bool concat, byte bunch)
        {
            CharSet = GetCharSet(message);
            Length = message.Length + addlength;
            Concat = concat;
            ContentSize = 0;
            Bunch = bunch;
            //Method = MethodTypes.SMSMT;
            //Units = GetBillingUnits(0, Length, IsLatin(CharSet), concat);
        }

       
        //public UnitsItem(string message, int addlength, bool concat, int contentSize, string method)
        //{
        //    CharSet = GetCharSet(message);
        //    Length = message.Length + addlength;
        //    Concat = concat;
        //    ContentSize = contentSize;
        //    //Method = method;
        //    //Units = CalcUnits(method, Length, IsLatin(CharSet), concat);
        //}
        #endregion

        #region Content size

        public static UnitsItem CreateUnitsItem(MethodType method, string message, bool enableImageizeOrConcat, byte bunch)
        {
            if (MsgMethod.IsMail(method))
                return CalcHtmlSize(message, enableImageizeOrConcat);
            return new UnitsItem(message, 0, enableImageizeOrConcat,bunch);
        }

        public static int GetImagesSize(string body)
        {
            int size = 0;
            string pattern = @"<img.*?>";

            MatchCollection matchCol = Regex.Matches(body, pattern, RegexOptions.IgnoreCase);
            foreach (Match m in matchCol)
            {
                string src = FormatImgSrc(m);
                size += GetImageSize(src);
            }
            return size;
        }
          

        public static UnitsItem CalcHtmlSize(string body, bool enableImageSize)
        {
            try
            {
                int html_size = System.Text.Encoding.UTF8.GetBytes(body).Length;
                int size = 0;

                if (enableImageSize)
                {
                    string pattern = @"<img.*?>";

                    MatchCollection matchCol = Regex.Matches(body, pattern, RegexOptions.IgnoreCase);
                    foreach (Match m in matchCol)
                    {
                        string src = FormatImgSrc(m);
                        size += GetImageSize(src);
                    }
                    //ApiLog.DebugFormat("CalcImageSize :{0}", size);
                }
                int images_size = size;

                return new UnitsItem(html_size, images_size,0);

                //if (accountId > 0)
                //{
                //    units = BillingItem.CalcBillingItem(accountId, "MALMT", 1, TotalSize).ItemUnits;
                //    //units = RemoteUtil.GetMailBillingUnits(accountId, TotalSize, ref item_price);
                //}
            }
            catch (Exception ex)
            {
                //ApiLog.ErrorFormat("CalcMailSize Error:{0}", ex.Message);

                throw new MsgException(AckStatus.BillingException, string.Format("Calc Message Size Error:{0}", ex.Message));
            }
          
        }



        static string FormatImgSrc(Match m)
        {
            string tag = m.ToString();
            //src(|\s)=(|\s)(\"|').*?(\"|')
            string pattern = "src(|\\s)=(\"|').*?(\"|')";

            Match msrc = Regex.Match(tag, pattern);
            if (msrc.Success)
            {
                return msrc.Value.Split('=')[1].Trim('"').ToLower();
            }
            return "";
        }

       

        static int GetImageSize(string src)
        {
            try
            {
                if (string.IsNullOrEmpty(src))
                    return 0;
                if (!src.StartsWith("http://"))
                    return 0;

                string url = src.Replace(ViewConfig.UploadVirtualPath, ViewConfig.UploadPath);
                url = url.Replace(ViewConfig.UploadVirtualLocalPath, ViewConfig.UploadPath);
                url = url.Replace("/", @"\");
                //ApiLog.DebugFormat("ImageUrl :{0}", url);

                if (System.IO.File.Exists(url))
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(url);
                    return (int)fi.Length;

                    //int bytes= System.IO.File.ReadAllBytes(url).Length;
                    //ApiLog.DebugFormat("ImageSize :{0}", bytes);
                    //return bytes;
                }
                return 0;
            }
            catch
            {
                return 0;
            }

        }

        #endregion

        #region BillingUnits


        public int GetSMSBillingUnits(byte bunch, int OperatorId = 0)
        {
            int unit = 1;

            if (this.Length <= 0)
            {
                throw new AppException(AckStatus.InvalidContent, "Invalid message length");
            }
            unit = OperatorsRules.GetBillingUnits(OperatorId, Length, IsLatinCharSet, Concat, bunch);

            return unit;
        }

        /*
        public static int GetBillingUnits(int operatorId, int msgLength, bool isLatin, bool concat)
        {
            int maxLength = OperatorsRules.GetTotalLengthByOperator(operatorId, isLatin);
            int maxDigits = OperatorsRules.GetMaxDigitByOperator(operatorId, isLatin);
            return GetBillingUnits(operatorId, msgLength, maxLength, maxDigits, concat);
        }

        public static int GetBillingUnits(int operatorId, int msgLength, int maxLength, int maxDigits, bool concat)
        {

            if (msgLength <= maxDigits || !concat)
                return 1;

            switch (operatorId)
            {
                case 0://default
                    if (msgLength >= maxLength)
                        return 3;
                    return (int)Math.Ceiling((decimal)((decimal)msgLength / (decimal)ConcatLimit));
                case 1://cellcom
                    if (msgLength >= maxLength)
                        return 3;
                    if (msgLength > 132)
                        return 3;
                    return 2;
                case 2://orage
                    if (msgLength >= maxLength)
                        return 3;
                    if (msgLength > 129)
                        return 3;
                    return 2;
                case 3://pelephone
                    //return 1;
                    if (msgLength >= maxLength)
                        return 3;
                    if (msgLength > 132)
                        return 3;
                    return 2;
                case 4://mirs
                    return 1;
                default:
                    return 1;
            }
        }
       
        /// <summary>
        /// Get BillingUnits
        /// </summary>
        /// <exception cref="AppException"></exception>
        /// <param name="OperatorId"></param>
        /// <param name="Method"></param>
        /// <param name="Message"></param>
        /// <param name="Concatenate"></param>
        /// <returns>units</returns>
        public static int GetBillingUnits(int OperatorId, string Method, string Message, int personalLength, bool Concatenate)
        {


            int unit = 1;
            if (MsgMethod.IsSMS(Method) && Concatenate)
            {
                if (Message == null || Message.Length <= 0)
                {
                    throw new AppException(AckStatus.InvalidContent, "Invalid message");
                }
                bool isLatin = IsLatin(Message);
                unit = GetBillingUnits(OperatorId, Message.Length + personalLength, isLatin, Concatenate);
            }

            return unit;
        }

        public static int GetBillingUnits(int OperatorId, string Method, int MessageLength, bool isLatin, bool Concatenate)
        {

            int unit = 1;
            if (MsgMethod.IsSMS(Method) && Concatenate)
            {
                if (MessageLength <= 0)
                {
                    throw new AppException(AckStatus.InvalidContent, "Invalid message");
                }
                unit = GetBillingUnits(OperatorId, MessageLength, isLatin, Concatenate);
            }

            return unit;
        }
        */
        #endregion

        public static byte GetBunch(int accountId)
        {
            using (DalRule dal = new DalRule())
            {
              return  dal.LookupAccountBunch(accountId);
            }
        }
    }
  
 
    public class OperatorsRules
    {
       
        public const int MaxSingleLimit = 70;
        public const int MaxDoubleLimit = 132;
        public const int MaxSingleLatinLimit = 160;
        public const int MaxConcatUnits = 12;

        public static int GetMessageLengthUnit(byte bunch)
        {
            return bunch==2 ? UnitsItem.DoubleCellQuota : UnitsItem.SingleCellQuota;
        }

        public static int GetBillingUnits(int operatorId, int msgLength, bool isLatin, bool concat, byte bunch)
        {
            int maxLength = OperatorsRules.GetTotalLengthByOperator(operatorId, isLatin);
            int maxSingleDigits = OperatorsRules.GetMaxSingleDigit(operatorId, isLatin, bunch);
            return GetBillingUnits(operatorId, msgLength, maxLength, maxSingleDigits, concat, bunch);
        }

        public static int GetBillingUnits(int operatorId, int msgLength, int maxLength, int maxSingleDigits, bool concat, byte bunch)
        {

            if (msgLength <= maxSingleDigits || !concat)
                return bunch==2 ? 2: 1;

            switch (operatorId)
            {
                case 1://cellcom
                case 2://orage
                case 3://pelephone
                case 0://default
                    if (msgLength >= maxLength)
                        return MaxConcatUnits;
                    return (int)Math.Ceiling((decimal)((decimal)msgLength / (decimal)GetMessageLengthUnit(bunch)));
                case 4://mirs
                    return bunch==2 ? 2 : 1;
                default:
                    return bunch==2 ? 2 : 1;
            }
        }

        public static int GetMaxSingleDigit(int operatorId, bool isLatin, byte bunch=1)
        {
            switch (operatorId)
            {
                case 0:
                    return isLatin ? MaxSingleLatinLimit : bunch ==2 ? MaxDoubleLimit : MaxSingleLimit;
                case 1://cellcom
                case 2://orage
                case 3://pelephone
                    return isLatin ? MaxSingleLatinLimit : bunch == 2 ? MaxDoubleLimit : MaxSingleLimit;
                case 4://mirs
                    return isLatin ? 140 : 70;
                default:
                    return isLatin ? MaxSingleLatinLimit : bunch == 2 ? MaxDoubleLimit : MaxSingleLimit;
            }
        }

        public static int GetTotalLengthByOperator(int operatorId, bool isLatin)
        {

            switch (operatorId)
            {
                case 0:
                    return isLatin ? 160 : 800;
                case 1://cellcom
                    return isLatin ? 160 : 800;
                case 2://orage
                    return isLatin ? 160 : 800;
                case 3://pelephone
                    return isLatin ? 160 : 800;
                case 4://mirs
                    return isLatin ? 160 : 132;
                default:
                    return 70;
            }
        }


    }
}
