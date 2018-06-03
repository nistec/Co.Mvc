#define LIVE//TEST//LIVE

using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Web.UI.WebControls;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Linq;
using MControl.Collections;
using MControl;
using Netcell.Data.Client;

using Netcell.Lib.Mobile;
using Netcell.Web;
using Netcell.Lib.View;
using Netcell.Data;
using Netcell.Remoting;
using MControl.Messaging;
using Netcell.Data.Db.Entities;
using MControl.Generic;
using Netcell;
using Netcell.Remoting.Extension;

namespace Netcell.Lib
{

   
    /// <summary>
    /// Summary description for Campaign.
    /// </summary>
    [Serializable]
    public class CampaignBuilder : ICampaign
    {
        #region structs
        
        public struct ItemEntry
        {
            public string Personal;
            public string Coupon;

            public static ItemEntry GetItemEntry(string value)
            {
                ItemEntry item = new ItemEntry();
                StringBuilder sb = new StringBuilder();
                string[] entry = value.Split(SplitKey);
                foreach (string s in entry)
                {
                    if (s.StartsWith(PersonalPrefix))
                    {
                        sb.AppendFormat("{0}{1}", s.Substring(2), SplitKey);
                    }
                    else if (s.StartsWith(CouponPrefix))
                    {
                        item.Coupon = string.Format("{0}", s.Substring(2));
                    }
                }
                item.Personal = sb.ToString();

                return item;
            }
        }

               
        #endregion

        #region consts
        public const string saveMessage = "הדיוור נשמר בהצלחה";
        public const string sentMessage = "הדיוור נשלח בהצלחה";
        public const string sentPenging = "הדיוור תוזמן בהצלחה";
        public const string notSent = "הדיוור לא נשלח";
        public const string haseError = "אירעה שגיאה,דיוור לא נשלח";

        public const string sentTest = "הההודעה נשלחה בהצלחה";
        const string activeName = "ActiveCampaign";
        const string DataKeyPrefix = "d";
        const string CellKeyPrefix = "s";
        const char SplitKey = ';';
        const string CouponPrefix = "cp:";
        const string PersonalPrefix = "p:";
        const int MaxValidTime = 2400;
        const int MinValidDiff = 60;
        const int MaxNotifyCells = 3;
        const string BarcodeSampleH = "BarcodeSample128H.jpg";
        const string BarcodeSampleV = "BarcodeSample128V.jpg";
        public const int SwpLinkLength = 42;
        public const string SwpLinkSample = "http://mobile.my-t.co.il/wp.aspx?p=123456789";
        public const int DefaultMaxItemsPerBatch = 10000;

       
           

        public enum Bits
        {
            IsContentDefine = 0,
            IsBarcodeDefine = 1,
            IsBatchDefine = 2,
            IsItemsDefine = 3,
            IsLoadFromDraft = 4,
            IsLoadFromTemplate = 5,
            //IsTemplate = 5,
            //IsPending = 6,
            //IsPersonal = 7

        }

        #endregion

        #region members

        public readonly int AccountId;
        public readonly MethodType Method;
        public readonly int UserId;
        protected bool IsPublishSync=false;
  
        protected bool IsRemote;
  
        public string HtmlDesign;

        protected int CampaignId;
        protected DalCampaign dalCamp;
        protected string Sender;
        protected int PriceCode;
        protected DeviceRule DeviceRule = 0;
        protected bool initilaized;
        protected CampaignSendType _SendType = CampaignSendType.Now;
        
        protected CampaignSendState state;
        protected DateTime DateToSend;
        protected string CampaignName;
        protected BillingType BillType;
        protected bool fixedInitilaized;
        protected FixedScheduler _FixedScheduler;
        protected ProductType ProductType;

        protected int SignCount;
        protected int AppId;
        protected bool IsActive;
        protected SendInterval SendInterval;
        protected SchedulerType SchedulerType;
        protected Lang lang= Lang.He;


        protected string ReplyTo;
        protected string Display;
        protected int DesignId;
 
        protected string Subject;
        protected string SmsMessage;
        protected string SessionId;
        protected string SiteName;

        protected int MaxItemsPerBatch;
        protected int MaxPersonalLength;
        protected bool IsPersonal;
        protected string PersonalFields;
        protected string PersonalDisplay;
        protected bool HasContentChanged;
        protected bool HasAlterChanged;

        //Lang Lang = Lang.He;//0=he
        //CampaignNotifyType NotifyType;
        //bool Concatenate;
        //Coupon _Coupon;
        string BarcodeValue;

        string[] NotifyCells;
        CampaignNotifyType NotifyType;

        DateTime ExpirationDate;
        string ValidTimeBegin;
        string ValidTimeEnd;
        protected BitArray _BitArray;
 
        protected bool IsPending;
        protected bool HasRemoveLink;
 
        protected int ItemUnits;
        protected int ItemSize;
        protected int TotalUnits;
        protected decimal Cost;
        protected decimal ItemPrice;
        WapItemsList _WapItems;

        CampaignFeatures features;
 
        protected ExecTypePublisher ExecPublisher;
        string _BatchArgs;
        protected int DestinationCount;
        //protected string DestinationPublishKey;
        //int BatchNext;
        Guid _PublishKey;
        BatchTypes BatchType;
        protected string UploadLocalPath;
        protected string UploadVirtualPath;
        protected string WebClientLocalPath;
        protected string WebClientVirtualPath;

        #endregion

        #region ctor

 
        delegate string CampaignInvokeDelegate();//MethodCategory pm);


        public string InvokeCampaignAsync()//MethodCategory pm)
        {
            CampaignInvokeDelegate d = new CampaignInvokeDelegate(InvokeCampaign);

            IAsyncResult ar = d.BeginInvoke( null, null);

            return d.EndInvoke(ar);
        }

        public static ICampaign Create(ICampaignDef campaign, int maxItemsPerBatch, int userId)
        {
            //if (campaign.Platform == PlatformType.Mail)
            //    return new CampaignMail(campaign, maxItemsPerBatch, userId);
            //else
                return new CampaignBuilder(campaign, maxItemsPerBatch, userId);
        }
  
        public CampaignBuilder(ICampaignDef campaign, int maxItemsPerBatch, int userId)
        {
            //IsValid = false;
            _PublishKey = GuidExtension.NewUuid();
            AccountId = campaign.CA;
            UserId = userId;
            MaxItemsPerBatch = maxItemsPerBatch>0?maxItemsPerBatch:DefaultMaxItemsPerBatch;
            CampaignName = campaign.CampaignName;
            CampaignId = campaign.CampaignId;
            Method = (MethodType)campaign.Method;
            SiteName = campaign.SiteName;
            SessionId = campaign.SessionId;

            if (AccountId <= 0)
            {
                throw new Exception("Invalid Account");
            }

            //ActiveAccountCreditInfo aci = new ActiveAccountCreditInfo(AccountId, method);

            initilaized = false;
            Sender = campaign.Sender;
            ReplyTo = campaign.ReplyTo;
            Display = campaign.DisplaySender;
            //ReminderFieldType = campaign.ReminderDateField;
            BillType = campaign.BillType;
            PriceCode = campaign.ServiceCode;
            Subject = campaign.Subject;
            ExpirationDate = Types.ToDateTime(campaign.Expiration, DateFormat, ApiUtil.MaxDate);
            DateToSend = DateTime.Now;
            //Method = MsgMethod.ToMethodType( method);
            SmsMessage = "";

            _BitArray = new BitArray(6, false);

            BillType = MsgMethod.ResolveBillingType(BillType);
            
            DesignId = campaign.DesignId;

            Features.Concatenate = campaign.Concatenate;
            //Features.NotifyOptions = CampaignNotifyType.None;

            NotifyType = CampaignNotifyType.None;

            IsPending = false;
            //IsPersonal = campaign.IsPersonal;
            HasRemoveLink = campaign.HasRemoveLink;// false;
            ValidTimeBegin = ViewConfig.DefaultValidTimeBegin;
            ValidTimeEnd = ViewConfig.DefaultValidTimeEnd;

            UploadLocalPath = campaign.UploadLocalPath;
            UploadVirtualPath = campaign.UploadVirtualPath;
            WebClientLocalPath = campaign.WebClientLocalPath;
            WebClientVirtualPath = campaign.WebClientVirtualPath;

            errs = new StringBuilder();// new List<string>();
            dalCamp = DalCampaign.Instance;
            dalCamp.AutoCloseConnection = false;
            

        }
        
        //remote
        internal CampaignBuilder(int campaignId, string campaignName, int accountId, MethodType method, int maxItemsPerBatch, string dateToSend, int userId)
            : this(campaignId, campaignName, accountId, method, BillingType.CB, 0, maxItemsPerBatch, dateToSend,userId)
        {

        }
        //remote
        internal CampaignBuilder(int campaignId, string campaignName, int accountId, MethodType method, BillingType billingType, int priceCode, int maxItemsPerBatch, string dateToSend, int userId)
        {
            _PublishKey = GuidExtension.NewUuid();
            IsRemote = true;
            //IsValid = false;
            AccountId = MControl.Types.ToInt(accountId, 0);
            MaxItemsPerBatch = maxItemsPerBatch > 0 ? maxItemsPerBatch : CampaignBuilder.DefaultMaxItemsPerBatch;
            CampaignName = campaignName;
            CampaignId = campaignId;
            if (AccountId <= 0)
            {
                throw new Exception("Invalid Account");
            }

            UserId = userId;
            IsPending = false;
            DateToSend = DateTime.Now;
            if (!string.IsNullOrEmpty(dateToSend))
            {
                DateToSend = DateHelper.ToDateTime(dateToSend);
                IsPending = (DateToSend > DateTime.Now.AddMinutes(2));
            }
            
       
            //ActiveAccountCreditInfo aci = new ActiveAccountCreditInfo(AccountId, method.ToString());// MsgMethod.MediaTypeToMethodMT(mediaType).ToString());

            initilaized = false;
            BillType = billingType;
            PriceCode = priceCode;
            ExpirationDate = ApiUtil.MaxDate;
           
            //MediaType = mediaType;
            Method = method;
            SmsMessage = "";

            _BitArray = new BitArray(6, false);

            BillType = MsgMethod.ResolveBillingType(BillType);


            Features.Concatenate = true;
            //Features.NotifyOptions = CampaignNotifyType.None;
            NotifyType = CampaignNotifyType.None;
            
            IsPersonal = false;
            HasRemoveLink = false;
            ValidTimeBegin = ViewConfig.DefaultValidTimeBegin;
            ValidTimeEnd = ViewConfig.DefaultValidTimeEnd;
            errs = new StringBuilder();// new List<string>();
            dalCamp = DalCampaign.Instance;
            dalCamp.AutoCloseConnection = false;

        }
        
        ~CampaignBuilder()
        {
            Dispose();
        }

        public void Dispose()
        {
            CampaignName = null;
            //BillingType = null;
            errs = null;


            Sender = null;
            ReplyTo = null;
            Display = null;
            //BillingType = null;
            Subject = null;
            SmsMessage = null;
            PersonalFields = null;
            PersonalDisplay = null;
            //SiteUrl = null;
            NotifyCells = null;
            ValidTimeBegin = null;
            ValidTimeEnd = null;
            //DateFieldName = null;
            //UploadFilesPath = null;
            //ContentPath = null;
            UploadLocalPath = null;
            UploadVirtualPath = null;
            WebClientLocalPath = null;
            WebClientVirtualPath = null;
            BarcodeValue = null;
           

            //_Coupon = null;

            //if (_Destinations != null)
            //{
            //    _Destinations.Clear();
            //    _Destinations = null;
            //}
            if (_WapItems != null)
            {
                _WapItems.Clear();
                _WapItems = null;
            }
            if (dalCamp != null)
            {
                dalCamp.AutoCloseConnection = true;
                dalCamp.Dispose();
            }
        }

         #endregion

        #region ExecType properties

        public void SetExecuteType(bool isSend, bool isSave, bool isNew, bool isDraft, bool isTest, int campaignId)
        {
            if (_SendType == CampaignSendType.Fixed)
            {
                isSave = true;
                if (string.IsNullOrEmpty(CampaignName))
                    CampaignName = "Fixed no name";
            }
            ExecPublisher = new ExecTypePublisher(isSend, isSave, isNew, isDraft,isTest, campaignId);
        }

        #endregion

        #region properties

        public CampaignFeatures Features
        {
            get
            {
                if (features == null)
                    features = new CampaignFeatures(null);
                return features;
            }
        }

        public Guid PublishKey
        {
            get { return _PublishKey; }
        }

        public CampaignSendState State
        {
            get { return state; }
        }

        public string StateDescription
        {
            get 
            {
                switch (State)
                {
                    case CampaignSendState.HasError:
                        return ErrorMessage;
                    case CampaignSendState.Saved:
                        return saveMessage;
                    case CampaignSendState.SentBatch:
                    case CampaignSendState.SentNow:
                    case CampaignSendState.SentPending:
                       return sentMessage;
                    default:
                       return ErrorMessage;
                }
             }
        }
        public int CID
        {
            get { return CampaignId; }
        }
        //public int BatchId
        //{
        //    get
        //    {
        //        //if (BatchList.Count > 0)
        //        //    return BatchList[0].BatchIndex;
        //        return BatchNext;
        //    }
        //}
        public bool Pending
        {
            get { return IsPending; }
        }


        private int PersonalLength
        {
            get
            {
                if (MaxPersonalLength > 0)
                    return MaxPersonalLength;
                if (IsPersonal)
                    return string.IsNullOrEmpty(PersonalDisplay) ? 1 : PersonalDisplay.Length;
                return 0;
            }
        }

        private int DateIndex
        {
            get
            {
                
                return MControl.Types.DateTimeToInt(DateTime.Now);
            }
        }
        #endregion

        #region private methods

        private decimal GetUnitPrice()
        {
            if (DestinationCount <= 0)
                return 0;
            return Cost / DestinationCount;
        }

        //tversion
        private int NextBatch(int batchCount, DateTime execTime, int batchIndex,int batchRange)
        {
            //return dalCamp.GetNextBatch(CampaignId, Method== MethodType.MALMT);

            int batchId = 0;

            using (DalController dal = new DalController())
            {
                dal.Trans_Batch_Insert(ref batchId, AccountId, CampaignId, batchCount, 0, execTime,batchIndex,batchRange, (int)BatchType, UserId, 0, (int)MsgMethod.GetPlatform(Method), (int)Method, Cost, PublishKey.ToString());
            }
            return batchId;

            //return Counters.BatchId(CampaignId, AccountId, (int)Method,(int) BillType);
        }

         private string GetPersonalField(string value)
        {
            int i = value.IndexOf(SplitKey);
            i = value.IndexOf(SplitKey, i + 1);

            return value.Substring(i + 1).TrimEnd(';');
        }


        private string DeserializeRow(DataRow dr, int count)
        {
            StringBuilder sb = new StringBuilder();
            object o_date = null;

            if (count > 1)
                sb.AppendFormat("{0};", dr["FirstName"]);
            if (count > 2)
                sb.AppendFormat("{0};", dr["LastName"]);
            if (count > 3)
            {
                DateTime user_date = Types.NZ(dr["BirthDate"], ApiUtil.NullDate);
                if (user_date > ApiUtil.NullDate)
                    o_date = user_date;
                sb.AppendFormat("{0};", o_date);
            }
            if (count > 4)
                sb.AppendFormat("{0};", dr["Address"]);
            if (count > 5)
                sb.AppendFormat("{0};", dr["Email"]);
            if (count > 6)
                sb.AppendFormat("{0}{1};", CouponPrefix, dr["Coupon"]);


            return sb.ToString();
        }

 
        public virtual bool ValidateCredit()
        {

            if (ItemUnits <= 0)
            {
                //ItemUnits = 1;

                throw new Exception("Units Billing error");
            }
            CreditStatus cs = ActiveCredit.GetCreditAndPrice(AccountId, Method, DestinationCount, ItemUnits);
            if (cs == null)
            {
                throw new Exception("Billing error");
            }
            Cost = cs.TotalCost;
            ItemPrice = cs.ItemPrice;
            TotalUnits = cs.Units;
            return cs.HasCredit;
         }

        #endregion
               
        #region WapItems


        public WapItemsList WapItems
        {
            get
            {
                if (_WapItems == null)
                {
                    _WapItems = (WapItemsList)ApiCache.GetWapContent(SessionId, CampaignId);//ApiCache.Get(/*SiteName,*/ SessionId, WapContent);
                    if (_WapItems == null)
                    {
                        _WapItems = new WapItemsList();
                    }
                }
                return _WapItems;
            }
            set { _WapItems = value; }
        }

        protected void InvokeWapContent(bool isPreview = false)
        {
            try
            {


                MobileViewBuilder mv = new MobileViewBuilder(CampaignId, DesignId, AccountId);
                mv.PersonalArgs = ViewUtil.CreatePersonalArgs(PersonalDisplay);
                mv.LoadAccountFeatures(AccountId, lang);
                mv.ExtractView(WapItems, false, true);
                mv.SaveContent(isPreview);//!shouldUpdate);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            _BitArray[(int)Bits.IsContentDefine] = true;
        }

        private void ClearWapContents(string uploadFilesPath)
        {
            try
            {
                //for (int i = 0; i < WapItems.Count; i++)
                foreach (WapItem item in WapItems.GetListItems())
                {
                    switch (item.WapItemType)
                    {
                        case WapItemType.Css:
                        case WapItemType.Barcode:
                        case WapItemType.Html:
                        case WapItemType.Confirm:
                        case WapItemType.Command:
                        case WapItemType.Holder:
                        case WapItemType.Link:
                            break;
                        default:
                            //WapItem item = WapItems[i];// GetWapItem(WapItemsOrder[i]);
                            if (item != null && !string.IsNullOrEmpty(item.FileName))
                            {
                                //System.IO.File.Delete(item.Filename.Replace("~",".."));
                                IOhelper.RemoveFile(uploadFilesPath, item.FileName);
                            }
                            break;
                    }
                }
            }
            catch { }
        }


        public static bool SaveWapContent(WapItemsList wapItems, ICampaignDef def, int accountId, int maxWidth, string dir, string personalDispaly, bool isPreview = false)
        {
            try
            {
                MobileViewBuilder mv = new MobileViewBuilder(def.CampaignId, def.DesignId, accountId);
                mv.PersonalArgs = ViewUtil.CreatePersonalArgs(personalDispaly);
                mv.LoadAccountFeatures(accountId, Lang.He);
                mv.ExtractView(wapItems, false, dir == "rtl");
                mv.SaveContent(isPreview);//!shouldUpdate);
                return true;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return false;
            }
        }

        #endregion
 
        #region Content
         
        protected virtual int InvokeMailContent(bool isPreview=false)
        {
            string html = ApiCache.GetMailContent(SessionId, CampaignId);//..GetAsString(/*SiteName, */SessionId, MailContent);
            if (string.IsNullOrEmpty(html))
            {
                throw new ArgumentException("Invalid Mail body");
            }
            return InvokeMailContent(html, isPreview);
        }

        protected virtual int InvokeMailContent(string html, bool isPreview = false)
        {
            int saveState = 0;
            bool shouldUpdate = ExecPublisher.ShouldUpdate();
            //MessageText = "";
            //ReplyTo = GetNotifyCells();

            MailViewBuilder mv = new MailViewBuilder(CampaignId, AccountId, html, ItemSize, ItemUnits);
            mv.PersonalArgs = ViewUtil.CreatePersonalArgs(PersonalDisplay);
            mv.LoadAccountFeatures(AccountId, Sender, MsgMethod.IsMwp(Method),lang);
            //mv.CampaignId = CampaignId;
            //mv.EnableMailMobile = MsgMethod.IsMwp(Method);
            mv.ExtractView(UploadLocalPath, UploadVirtualPath, WebClientLocalPath, WebClientVirtualPath, ProductType == ProductType.Quiz);
            mv.SaveContent(isPreview);//!shouldUpdate);

            return saveState;
        }

        protected virtual int InvokeMailPreview()
        {
            string html = ApiCache.GetMailContent(SessionId,CampaignId);//.GetAsString(/*SiteName,*/ SessionId, MailContent);
            if(string.IsNullOrEmpty(html))
            {
                throw new ArgumentException("Invalid Mail body");
            }
            int saveState = 0;
            bool shouldUpdate = ExecPublisher.ShouldUpdate();
            //ReplyTo = GetNotifyCells();

            MailViewBuilder mv = new MailViewBuilder(CampaignId, AccountId, html, ItemSize, ItemUnits);
            mv.PersonalArgs = ViewUtil.CreatePersonalArgs(PersonalDisplay);
            mv.LoadAccountFeatures(AccountId, Sender, MsgMethod.IsMwp(Method), lang);
            mv.ExtractView(UploadLocalPath, UploadVirtualPath, WebClientLocalPath, WebClientVirtualPath, ProductType == ProductType.Quiz);
            mv.SaveContent(true);//!shouldUpdate);
            return saveState;
        }

        public static bool SaveMailContent(ICampaignContent content, ICampaignDef def, int accountId, string personalDisplay, Lang lang,bool isPreview=false)
        {
            try
            {
                MailViewBuilder mv = new MailViewBuilder(def.CampaignId,accountId, content);
                mv.PersonalArgs = ViewUtil.CreatePersonalArgs(personalDisplay);
                mv.LoadAccountFeatures(accountId, def.Sender, MsgMethod.IsMwp(def.Method), lang);
                //mv.CampaignId = def.CampaignId;
                //mv.EnableMailMobile = def.IsSWP;
                mv.ExtractView(def.GetCampaignPaths());
                mv.SaveContent(isPreview);// (false);
                return true;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return false;
            }
        }

        public static bool SaveSmsContent(string message, ICampaignDef def)
        {
            int res = 0;
            try
            {
                using (CampaignEntity_Context context = new CampaignEntity_Context(def.CampaignId))
                {
                    context.Entity.MessageText = message;
                    res=context.SaveChanges();
                }
                
                return res>0;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return false;
            }
        }
        #endregion
          
        #region Destinations
        /*
        Dictionary<string, DestinationItem> _Destinations;
        protected ICollection Destinations
        {
            get
            {
                if (_Destinations == null)
                {
                    Hashtable h = (Hashtable)MControl.Caching.Remote.RemoteCacheClient.Instance.GetItem(DestinationKey, null);
                    if (h == null)
                        return null;
                    SetDestination(h.Values);
                    // _Destinations = new Dictionary<string, DestinationItem>();
                }
                return _Destinations.Values;
            }
        }

        protected void SetDestination(ICollection items)
        {
            _Destinations = new Dictionary<string, DestinationItem>();
            foreach (object o in items)
            {
                DestinationItem item = (DestinationItem)o;
                _Destinations[item.Target] = item;
            }
            DestinationCount = _Destinations.Count;
        }

        */

        //protected virtual ICollection GetDestinations()
        //{
        //    RenderDestinationPublishKey();

        //    Hashtable h = (Hashtable)MControl.Caching.Remote.RemoteCacheClient.Instance.GetItem(DestinationPublishKey, null);
        //    if (h == null || h.Count==0)
        //    {
        //        throw new MsgException(AckStatus.BadDestination, "No destination found");
        //    }
        //    Dictionary<string, DestinationItem>  _Destinations = new Dictionary<string, DestinationItem>();
        //    foreach (object o in h.Values)
        //    {
        //        DestinationItem item = (DestinationItem)o;
        //        _Destinations[item.Target] = item;
        //    }

        //    return _Destinations.Values;
        //}


        #endregion

        #region Notify
        
         public void SetNotify(ICampaignNotify notify)
        {
            //Features.NotifyOptions = notify.NotifyType;
            NotifyType = notify.NotifyType;
            NotifyCells = notify.GetNotifyCells();
            //ReplyTo = GetNotifyCells();
        }
       
        public void SetNotify(CampaignNotifyType notifyType, string[] notifyCells)
        {
            //Features.NotifyOptions = notifyType;
            NotifyType = notifyType;
            NotifyCells = notifyCells;
            //ReplyTo = GetNotifyCells();
        }

        protected string GetNotifyCells()
        {
            string cells = "";
            if (NotifyType != CampaignNotifyType.None)
            {
                foreach (string s in NotifyCells)
                {
                    cells += s + ";";
                }
            }
            cells = cells.TrimEnd(';');
            return cells;
        }
        #endregion

        #region Coupon

        public void SetBarcode(Barcode c)
        {
            //_Coupon = c;
            if (c != null )//&& c.CouponType == Barcode.eCouponType.Barcode)
            {
                Features.CouponSource = (CouponSource)(int)c.BarcodeSource;
                Features.CouponType = (CouponType)(int)c.BarcodeType;
                BarcodeValue = c.BarcodeValue;
                //WapItems.Add(new WapItem("Barcode", "../img/Barcode.jpg", "", WapItemType.Barcode, c.FormId));
            }
            else
            {
                WapItems.Remove(WapItemType.Barcode);
            }
        }

        #endregion

        #region Timing

        public static DateTime DefaultExpiration
        {
            get { return DateTime.Now.AddYears(1); }
        }

        public static System.Globalization.DateTimeFormatInfo DateFormat
        {
            //get{return new System.Globalization.DateTimeFormatInfo;}
            get { return new System.Globalization.CultureInfo("he-IL", false).DateTimeFormat; }
        }
        public void SetValidTime(ITiming timing)
        {
            //TimingType = timingType;
            BatchType = BatchTypes.Auto;
            DateToSend = Types.ToDateTime(timing.GetValidTimeToSend(), DateFormat, DateTime.Now);
            IsPending = DateToSend > DateTime.Now.AddMinutes(2);
            if (timing.GetTimingType() == 1)
            {
                if (DateToSend < DateTime.Now)
                {
                    throw new Exception("מועד תזמון אינו תקין");
                }
                IsPending = true;
            }

            if (!string.IsNullOrEmpty(timing.ValidTimeBegin))
                ValidTimeBegin = timing.ValidTimeBegin;
            if (!string.IsNullOrEmpty(timing.ValidTimeEnd))
                ValidTimeEnd = timing.ValidTimeEnd;

            switch (_SendType)
            {
                case CampaignSendType.Watch:
                case CampaignSendType.Fixed:
                    _FixedScheduler = new FixedScheduler(timing);
                    fixedInitilaized = true;
                    break;
                case CampaignSendType.Batches:
                    SetBatches(timing);
                    break;
            }
        }

        void SetBatches(ITiming timing)
        {
            bool[] days = new bool[7];
            int validDays = 0;
            for (int d = 0; d < 7; d++)
            {
                days[d] = timing.BatchDays[d].Selected;
                validDays += days[d] ? 1 : 0;
            }
            if (validDays == 0)
            {
                throw new Exception("לא סומנו ימים לחלוקת המנות");
            }
            BatchType = timing.IsMultiBatch ? BatchTypes.Multi : BatchTypes.Single;
            _BatchArgs = BatchArgs.BatchArgsFormat(timing.IsMultiBatch ? BatchTypes.Multi : BatchTypes.Single, timing.BatchValue, timing.BatchDelay, timing.BatchDelayMode, MaxItemsPerBatch, days, UserId, GetUnitPrice(),PublishKey);

        }
        
 
        #endregion

        #region Validate

        public AckStatus CampaignAckStatus()
        {
            switch (state)
            {
                case CampaignSendState.HasError:
                    return AckStatus.ApplicationException;
                case CampaignSendState.None:
                    return AckStatus.None;
                default:
                    return AckStatus.Ok;
            }
        }

        //List<string> errs;
        protected StringBuilder errs;

        public string ErrorMessage
        {
            get
            {
                if (errs == null)
                    return "";
                return errs.ToString();
            }
        }

        //public static bool ValidtePricingRB(int priceCode, string SC)
        //{
        //    try
        //    {
        //       return BillingRB.ValidtePricingRB(priceCode.ToString(), SC)>0;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public virtual bool Validate()
        {
            bool isValid = false;

            //errs.Clear();
            errs = new StringBuilder();

            if (Method == MethodType.MALMT || Method == MethodType.MALWP)
            {
                //if (string.IsNullOrEmpty(MessageText))
                //    errs.Append("חסר נוסח ההודעה");
                if (string.IsNullOrEmpty(Sender))
                    errs.AppendLine("חסר מאת");
                if (!MControl.Regx.RegexValidate("^[a-zA-Z0-9-_.]+$", Sender))
                    errs.AppendLine("שם השולח אינו תקין נדרש אותיות באנגלית ו או מספרים ו או (_ - .)");
                if (string.IsNullOrEmpty(Subject))
                    errs.AppendLine("חסר נושא ההודעה");

                if (Method == MethodType.MALWP && CampaignId == 0 && HasAlterChanged == false)
                    errs.AppendLine("לא נמצא תוכן מולטימדיה, נא לסמן שמירת שינויים במולטימדיה");
            }
            //else if (Method == MethodType.WAPMT)
            //{
            //    if (string.IsNullOrEmpty(SmsMessage))
            //        errs.AppendLine("חסר הודעה מקדימה");
            //    if (string.IsNullOrEmpty(Sender))
            //        errs.AppendLine("חסר מאת");
            //}
            else if (Method == MethodType.SMSWP)
            {
                if (string.IsNullOrEmpty(SmsMessage))
                    errs.AppendLine("חסר הודעה מקדימה");
                if (string.IsNullOrEmpty(Sender))
                    errs.AppendLine("חסר מאת");
                if (CampaignId == 0 && HasAlterChanged == false)
                    errs.AppendLine("לא נמצא תוכן מולטימדיה, נא לסמן שמירת שינויים במולטימדיה");
            }
            else
            {
                if (string.IsNullOrEmpty(SmsMessage))
                    errs.AppendLine("חסר נוסח ההודעה");
                if (string.IsNullOrEmpty(Sender))
                    errs.AppendLine("חסר מאת");
            }


            if (DestinationCount == 0)
                errs.AppendLine("חסר נמענים");
            if (!ValidateTimeBeginAndEnd())//credit, priceUnit))
                errs.AppendLine("זמני שליחה אינם תקניים");
            if (_SendType == CampaignSendType.Fixed && !_FixedScheduler.Validate())//credit, priceUnit))
                errs.AppendLine("זמני שליחה למצב קבוע אינם תקניים");
            //if (Coupon.CouponType != Coupon.eCouponType.None && string.IsNullOrEmpty(Coupon.BarcodeValue))
            //    errs.Append(isHtml ? string.Format("<li><span style='font-weight: bold;color: red'>{0}</span></li>", "הגדרות קופון אינן תקינות") : "הגדרות קופון אינן תקינות");
#if(RB)
            if (BillType == BillingType.RB)
            {
                if (PriceCode <= 0)
                    errs.AppendLine("חסר קוד מחיר להודעת RB");
                if (!ValidtePricingRB(PriceCode, Sender))
                    errs.AppendLine("קוד המחיר אינו תואם למספר השולח, יש להשתמש בקוד מקוצר");
            }
            else
            {
                if (!ExecPublisher.IsSaveOnly)
                {
                    if (!ValidateCredit())
                        errs.AppendLine("חסר אשראי לשליחה");
                }
            }
#else
            if (!ExecPublisher.IsSave)
            {
                if (!ValidateCredit())
                    errs.AppendLine("חסר אשראי לשליחה");
            }

#endif
       
            if (ExecPublisher.IsSave && string.IsNullOrEmpty(CampaignName))
                errs.AppendLine("חסר שם דיוור");

            isValid = errs.Length == 0;

            return isValid;
        }

        public virtual bool ValidateSend()
        {
            bool isValid = false;

            //errs.Clear();
            errs = new StringBuilder();

            if (Method == MethodType.MALMT || Method == MethodType.MALWP)
            {
                if (!MControl.Regx.RegexValidate("^[a-zA-Z0-9-_.]+$", Sender))
                    errs.AppendLine("שם השולח אינו תקין נדרש אותיות באנגלית ו או מספרים ו או (_ - .)");
                if (string.IsNullOrEmpty(Subject))
                    errs.AppendLine("חסר נושא ההודעה");
            }

            if (string.IsNullOrEmpty(Sender))
                errs.AppendLine("חסר מאת");
            if (DestinationCount == 0)
                errs.AppendLine("חסר נמענים");
            if (!ValidateTimeBeginAndEnd())//credit, priceUnit))
                errs.AppendLine("זמני שליחה אינם תקניים");
            if (_SendType == CampaignSendType.Fixed && !_FixedScheduler.Validate())//credit, priceUnit))
                errs.AppendLine("זמני שליחה למצב קבוע אינם תקניים");
            //if (Coupon.CouponType != Coupon.eCouponType.None && string.IsNullOrEmpty(Coupon.BarcodeValue))
            //    errs.Append(isHtml ? string.Format("<li><span style='font-weight: bold;color: red'>{0}</span></li>", "הגדרות קופון אינן תקינות") : "הגדרות קופון אינן תקינות");

            if (!ValidateCredit())
                errs.AppendLine("חסר אשראי לשליחה");

            isValid = errs.Length == 0;

            return isValid;
        }

        public string WrappedErrorMessage(string err)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<p style='font-weight: bold'>הערות</p>");
            sb.AppendFormat("<p>{0}</p>", HtmlHelper.CleanHtml(err));
            return sb.ToString();

            //sb.Append("<p style='font-weight: bold;color: red'>הערות</p>");
            //sb.Append("<ul>");
            //foreach (string s in errs)
            //{
            //    sb.Append(s);
            //}
            //sb.Append(errs.ToString());
            //if (isHtml)
            //{
            //    sb.Append("</ul>");
            //}
            //msg = sb.ToString();
        }

        protected void ValidateInvoke()
        {
            if (!initilaized)
            {
                throw new Exception("Campaign not initilaized!!!");
            }
            if (_SendType == CampaignSendType.Fixed && !fixedInitilaized)
            {
                throw new Exception("Campaign Scheduler not initilaized!!!");
            }
            if (_SendType == CampaignSendType.Pending && !IsPending)
            {
                throw new Exception("Campaign send time should be pending!!!");
            }
        }

        //public bool Validate(string message, string sender,bool isHtml)
        //{
        //    MessageText = message;
        //    Sender = sender;
        //    return Validate();
        //}

        public bool ValidateTimeBeginAndEnd()
        {
            return (MControl.Strings.StringTimeToTimeSpan(ValidTimeBegin) <= MControl.Strings.StringTimeToTimeSpan(ValidTimeEnd));
        }

         public static bool ValidateSend(bool isHtml, CampaignEntity campaign,string Sender, int DestinationCount, ref string err)// MethodType Method, string Sender, string MessageText, string PromoMessage, int DestinationCount, int AccountId, bool Concat ,ref string err)
         {
             //errs.Clear();
             StringBuilder errs = new StringBuilder();
             string message = "";
             try
             {
                 campaign.ValidateContent();
             }
             catch
             {
                 errs.Append(isHtml ? string.Format("<li><span style='font-weight: bold;color: red'>{0}</span></li>", "חסר תוכן המסר") : "חסר תוכן המסר");

             }
             if (campaign.MtId == (int)MethodType.MALMT || campaign.MtId == (int)MethodType.MALWP)
             {
                  if (!campaign.IsValidContent())
                     errs.Append(isHtml ? string.Format("<li><span style='font-weight: bold;color: red'>{0}</span></li>", "חסר תוכן המסר") : "חסר תוכן המסר");
                 if (string.IsNullOrEmpty(Sender))
                     errs.Append(isHtml ? string.Format("<li><span style='font-weight: bold;color: red'>{0}</span></li>", "חסר מאת") : "חסר מאת");
                 if (!MControl.Regx.RegexValidate("^[a-zA-Z0-9-_.]+$", Sender))
                     errs.Append(isHtml ? string.Format("<li><span style='font-weight: bold;color: red'>{0}</span></li>", "שם השולח אינו תקין נדרש אותיות באנגלית ו או מספרים ו או (_ - .)") : "שם השולח אינו תקין נדרש אותיות באנגלית ו או מספרים ו או (_ - .)");
                 if (string.IsNullOrEmpty(campaign.CampaignPromo))
                     errs.Append(isHtml ? string.Format("<li><span style='font-weight: bold;color: red'>{0}</span></li>", "חסר נושא ההודעה") : "חסר נושא ההודעה");
             }
             else if (campaign.MtId == (int)MethodType.SMSWP)
             {
                 message = campaign.CampaignPromo;
                 if (string.IsNullOrEmpty(message))
                     errs.Append(isHtml ? string.Format("<li><span style='font-weight: bold;color: red'>{0}</span></li>", "חסר נוסח המסר") : "חסר נוסח המסר");
                 if (string.IsNullOrEmpty(Sender))
                     errs.Append(isHtml ? string.Format("<li><span style='font-weight: bold;color: red'>{0}</span></li>", "חסר מאת") : "חסר מאת");
             }
             else
             {
                 message = campaign.MessageText;
                 if (string.IsNullOrEmpty(message))
                     errs.Append(isHtml ? string.Format("<li><span style='font-weight: bold;color: red'>{0}</span></li>", "חסר נוסח המסר") : "חסר נוסח המסר");
                 if (string.IsNullOrEmpty(Sender))
                     errs.Append(isHtml ? string.Format("<li><span style='font-weight: bold;color: red'>{0}</span></li>", "חסר מאת") : "חסר מאת");
             }


             if (DestinationCount == 0)
                 errs.Append(isHtml ? string.Format("<li><span style='font-weight: bold;color: red'>{0}</span></li>", "חסר נמענים") : "חסר נמענים");

             CampaignFeatures features = campaign.GetFeatures();

             //int ItemUnit = ActiveCredit.CalcBillingUnits(campaign.AccountId, campaign.Method, message, campaign.FeaturesItem.Concatenate);

             //CreditStatus cs = ActiveCredit.GetCreditAndPrice(campaign.AccountId, campaign.Method, DestinationCount, ItemUnit);

             CreditStatus cs = ActiveCredit.GetCreditAndPrice(campaign.AccountId, (MethodType)campaign.MtId, DestinationCount, UnitsItem.CreateUnitsItem((MethodType)campaign.MtId, message, features.Concatenate, UnitsItem.GetBunch(campaign.AccountId)));

             if (!cs.HasCredit)
                 errs.Append(isHtml ? string.Format("<li><span style='font-weight: bold;color: red'>{0}</span></li>", "חסר אשראי לשליחה") : "חסר אשראי לשליחה");

             err = errs.ToString();

             return errs.Length == 0;
         }
        #endregion

        #region Preview

        public string GetValidateMessage(bool isHtml)
        {
            string msg = "תקין";
            if (errs.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                if (isHtml)
                {
                    sb.Append("<p style='font-weight: bold'>הערות</p>");
                    sb.Append("<ul>");
                }
                //sb.Append("<p style='font-weight: bold;color: red'>הערות</p>");
                //sb.Append("<ul>");
                //foreach (string s in errs)
                //{
                //    sb.Append(s);
                //}
                sb.Append(errs.ToString());
                if (isHtml)
                {
                    sb.Append("</ul>");
                }
                msg = sb.ToString();
            }
            return msg;
        }
      
        #endregion

        #region public methods

 
        public string GetTimeToSend()
        {
            if (IsPending)
                return DateToSend.ToString();
            return "Now";
        }

        public string GetExpirationDate()
        {
            if (ExpirationDate != ApiUtil.MaxDate)
                return ExpirationDate.ToString();
            return "None";
        }


        public bool ValidateValidTime(string timeBegin, string timeEnd)
        {
            int validTimeBegin = MControl.Strings.StringTimeToInt(timeBegin);
            int validTimeEnd = MControl.Strings.StringTimeToInt(timeEnd);


            if (validTimeEnd <= validTimeBegin)
            {
                throw new Exception("זמני שליחה אינם תקינים");
            }
            if ((validTimeEnd - validTimeBegin) < MinValidDiff)
            {
                throw new Exception("טווח זמני השליחה קצר");
            }
            ValidTimeBegin = timeBegin;
            ValidTimeEnd = timeEnd;
            return true;
        }

 
        //protected void InvokeTemplate(int CampaignId)
        //{
        //    //int templateId=0;
        //    //DalCampaign.Instance.Campaigns_Template_Insert(ref templateId,CampaignId,CampaignName);
        //    if (CampaignName != null)
        //        dalCamp.Campaigns_Template_Save(CampaignId, CampaignName);
        //}

        public override string ToString()
        {
            return RM.GetString(RM.UploadResult, ExecPublisher);
        }

        private static void InvokeConfirm(int campaignId, string msg)
        {
            
            if (!msg.Contains(LinkPrefix.ConfirmRef))
                return;
            int len = LinkPrefix.ConfirmRef.Length;
            int count = 0;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            int start = 0;
            while (start >= 0)
            {
                start = msg.IndexOf(LinkPrefix.ConfirmRef, start);
                if (start == -1)
                    break;
                start += len;
                string val = msg.Substring(start, 1);
                start++;
                int beginText = msg.IndexOf(">", start);
                beginText++;
                int endText = msg.IndexOf("<", beginText);
                string text = msg.Substring(beginText, endText - beginText);
                dict[val] = text;
                count++;
                start = endText + 1;
            }
            if (count != dict.Count)
            {
                throw new Exception("There is duplicate values in confirms");
            }
            DataTable dt = GetConfirmSchema();
            foreach (KeyValuePair<string, string> d in dict)
            {
                dt.Rows.Add(campaignId, d.Key, d.Value);
            }
            DalCampaign.Instance.InsertTable(dt, "Campaigns_Confirm");
        }

        public static DataTable GetConfirmSchema()
        {
            DataTable dt = new DataTable("Campaign_Confirm");
            dt.Columns.Add("CampaignId", typeof(Int32));
            dt.Columns.Add("ConfirmStatus", typeof(Int16));
            dt.Columns.Add("DisplayText");
            return dt.Clone();
        }
        #endregion

        #region Invoke Campaign
              
        
        //private void RenderDestinationPublishKey()
        //{
        //    try
        //    {

        //        //DestinationPublishKey = ApiCache.GetCacheKey(/*SiteName,*/ ApiCache.ActiveDestination, SessionId);


        //        /*
        //        DestinationPublishKey = ApiCache.PublishPrefix + Guid.NewGuid().ToString();
        //        ApiCache.Copy(SiteName, SessionId, ApiCache.ActiveDestination, DestinationPublishKey, 30);
                
        //         Hashtable h = (Hashtable)MControl.Caching.Remote.RemoteCacheClient.Instance.Fetch(DestinationPublishKey);

        //        if (h == null || h.Count == 0)
        //        {
        //            throw new Exception("RenderDestinationPublishKey: Destination Not found, campaignId:" + DestinationPublishKey);
        //        }
        //        */
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new MsgException(AckStatus.CacheException, "Could not copy destination to campaign publisher, " + ex.Message);
        //    }


        // }

        protected void SetContentProperties(ICampaignContent content, ICampaignContent altcontent)
        {
            if (MsgMethod.IsSms(Method, false))
            {
                SmsMessage = content.GetContent();
                ItemUnits = content.ContentUnits;
                ItemSize = content.ContentSize;
                HasContentChanged = content.HasChanges;
            }
            else if (MsgMethod.IsSwp(Method))
            {
                SmsMessage = content.GetContent();
                ItemUnits = content.ContentUnits;
                ItemSize = content.ContentSize;
                HasContentChanged = content.HasChanges;
                if (altcontent != null)
                    HasAlterChanged = altcontent.HasChanges;
            }
            else if (MsgMethod.IsMwp(Method))
            {
                ItemUnits = content.ContentUnits + altcontent.ContentUnits;
                ItemSize = content.ContentSize + altcontent.ContentSize;
                HasContentChanged = content.HasChanges;
                if (altcontent != null)
                    HasAlterChanged = altcontent.HasChanges;
            }
            else if (MsgMethod.IsMail(Method, false))
            {
                ItemUnits = content.ContentUnits;
                ItemSize = content.ContentSize;
                HasContentChanged = content.HasChanges;
            }
        }

        public virtual void InitCampaign(IDestList dest, ICampaignContent content, ICampaignContent altcontent, CampaignSendType sendType, bool isPublish)
        {
            //IsPublishSync = isPublish;
            IsPersonal = dest.IsPersonal;
     
            PersonalDisplay = dest.PersonalDisplay;
            PersonalFields = dest.PersonalFields;

            //switch (Method)
            //{
            //    case MethodType.SMSMT:
            //        //SmsMessage = altcontent.GetContent(); break;
            //    case MethodType.SMSWP:
            //    case MethodType.WAPMT:
            //        //SmsMessage = PromoMessage;
            //        SmsMessage = altcontent.GetContent(); 
            //        break;


            //}

            SetContentProperties(content, altcontent);

            //if (MsgMethod.IsSms(Method, false))
            //{
            //    SmsMessage = content.GetContent();
            //    ItemUnits = content.ContentUnits;
            //    ItemSize = content.ContentSize;
            //    HasContentChanged = content.HasChanges;
            //}
            //else if (MsgMethod.IsSwp(Method))
            //{
            //    SmsMessage = content.GetContent();
            //    ItemUnits = content.ContentUnits;
            //    ItemSize = content.ContentSize;
            //    HasContentChanged = content.HasChanges;
            //    HasAlterChanged = altcontent.HasChanges;
            //}
            //else if (MsgMethod.IsMwp(Method))
            //{
            //    ItemUnits = content.ContentUnits + altcontent.ContentUnits;
            //    ItemSize = content.ContentSize + altcontent.ContentSize;
            //    HasContentChanged = content.HasChanges;
            //    HasAlterChanged = altcontent.HasChanges;
            //}
            //else if (MsgMethod.IsMail(Method,false))
            //{
            //    ItemUnits = content.ContentUnits;
            //    ItemSize = content.ContentSize;
            //    HasContentChanged = content.HasChanges;
            //}

            //DesignHeader = content.Header;
            //DesignFooter = content.Footer;
            
            //MessageText = content.Content;
            
            
            DestinationCount = dest.ItemsCount;
            MaxPersonalLength = dest.GetMaxPersonalLength();
            //DestinationKey = dest.DestinationKey;

            _SendType = sendType;

            initilaized = true;
        }

        public virtual void InitCampaignContent(ICampaignContent content, ICampaignContent altcontent, bool isPersonal, string personalDisplay, string personalFields)
        {
            IsPersonal = isPersonal;

            PersonalDisplay = personalDisplay;
            PersonalFields = personalFields;

            SetContentProperties(content, altcontent);

            //if (MsgMethod.IsSms(Method, false))
            //{
            //    SmsMessage = content.GetContent();
            //    ItemUnits = content.ContentUnits;
            //    ItemSize = content.ContentSize;
            //    HasContentChanged = content.HasChanges;
            //}
            //else if (MsgMethod.IsSwp(Method))
            //{
            //    SmsMessage = content.GetContent();
            //    ItemUnits = content.ContentUnits;
            //    ItemSize = content.ContentSize;
            //    HasContentChanged = content.HasChanges;
            //    HasAlterChanged = altcontent.HasChanges;
            //}
            //else if (MsgMethod.IsMwp(Method))
            //{
            //    ItemUnits = content.ContentUnits + altcontent.ContentUnits;
            //    ItemSize = content.ContentSize + altcontent.ContentSize;
            //    HasContentChanged = content.HasChanges;
            //    HasAlterChanged = altcontent.HasChanges;
            //}
            //else if (MsgMethod.IsMail(Method, false))
            //{
            //    ItemUnits = content.ContentUnits;
            //    ItemSize = content.ContentSize;
            //    HasContentChanged = content.HasChanges;
            //}
           
            _SendType = CampaignSendType.Manual;

            initilaized = true;
        }

        public virtual void InitCampaignSend(IDestList dest, CampaignSendType sendType, string contentBill, bool isPublish)
        {
            //IsPublishSync = isPublish;
            IsPersonal = dest.IsPersonal;

            PersonalDisplay = dest.PersonalDisplay;
            PersonalFields = dest.PersonalFields;

            BillingItem bi = BillingItem.CampaignBillingItem(AccountId, Method, contentBill, UnitsItem.GetBunch(AccountId));

            ItemUnits = bi.ItemUnits;
            ItemSize = bi.ItemSize;
           
            DestinationCount = dest.ItemsCount;
            MaxPersonalLength = dest.GetMaxPersonalLength();
            _SendType = sendType;

            initilaized = true;
        }

        protected void InvokeContent(bool isPreview=false)
        {
            if (MsgMethod.IsSwp(Method) && HasAlterChanged)
            {
                InvokeWapContent(isPreview);
            }
            else if (MsgMethod.IsMail(Method))
            {
                if (HasContentChanged)
                {
                    InvokeMailContent(isPreview);
                }
                if (MsgMethod.IsMwp(Method) && HasAlterChanged)
                {
                    InvokeWapContent(isPreview);
                }
            }
        }

        public virtual string InvokeCampaignTest(string dest)
        {
            state = CampaignSendState.Start;
            int res = 0;
            try
            {
                ValidateInvoke();

                AddCampaign();

                InvokeContent(true);
                
                BatchType = BatchTypes.Preview;

                //BatchNext = NextBatch(1,DateTime.Now,0,0);
                 int batchId = NextBatch(1, DateTime.Now, 0, 0);

                _BatchArgs = BatchArgs.BatchArgsFormat(BatchTypes.Preview,1, MaxItemsPerBatch, UserId, GetUnitPrice(),PublishKey);

                DestinationItem item = new DestinationItem(dest, "", MsgMethod.GetPlatform(Method));
                
                using (CampaignPublisher pub = new CampaignPublisher(CampaignId, ExecPublisher.ExecType, Sender, BarcodeValue, _BatchArgs))
                {
                    // pub.IsRenderBatch = true;
                    res = pub.Execute(item, batchId, false);
                }

                state =(res>0) ? CampaignSendState.SentTest: CampaignSendState.HasError;

                return sentTest;

            }
            catch (Exception ex)
            {
                state = CampaignSendState.HasError;
                return ex.Message;
            }
            finally
            {
                if (dalCamp != null)
                {
                    dalCamp.AutoCloseConnection = true;
                }
            }
        }

        public virtual string InvokeCampaign()
        {
            state = CampaignSendState.Start;
            try
            {
                ValidateInvoke();
      
                if (!ExecPublisher.IsDraft &&  !Validate())
                {
                    state = CampaignSendState.HasError;
                    return ErrorMessage;
                }
                //if (!IsValid)
                //{
                //    if (!Validate())
                //    {
                //        state = CampaignSendState.HasError;
                //        return GetValidateMessage(false);
                //    }
                //}

                AddCampaign();

                InvokeContent();

                //RenderDestination();

                if (_SendType == CampaignSendType.Fixed && ExecPublisher.IsDraft==false)
                {
                    ActiveScheduler.InvokeScheduler(CampaignId, AccountId, SchedulerDataSource.Fixed, SchedulerType.D, _FixedScheduler);
                }
                return RenderCampaign();

            }
            catch (Exception ex)
            {
                state = CampaignSendState.HasError;
                Log.Exception("InvokeCampaign error ", ex);
                return ex.Message;
            }
            finally
            {
                if (dalCamp != null)
                {
                    dalCamp.AutoCloseConnection = true;
                }
            }
        }

        public virtual string InvokeSend()
        {
            state = CampaignSendState.Start;
            try
            {
                ValidateInvoke();

                
                if (CampaignId <= 0)
                {
                    throw new Exception("אירעה שגיאה ביצירת הקמפיין,הקמפיין לא נשלח");
                }

                int status = 0;
                int batchCount = 0;// BatchNext;
                //string replyTo = ReplyTo;
                string notifyCells = GetNotifyCells();
                int res = dalCamp.Campaigns_UpdateSend(CampaignId, (int)_SendType, Subject, DateToSend, ExpirationDate, DestinationCount, status, DateIndex, MControl.Strings.StringTimeToInt(ValidTimeBegin), MControl.Strings.StringTimeToInt(ValidTimeEnd), batchCount, this.Sender, notifyCells, PersonalLength, Display,ReplyTo,(int)NotifyType);

                if (_SendType == CampaignSendType.Fixed && ExecPublisher.IsDraft == false)
                {
                    ActiveScheduler.InvokeScheduler(CampaignId, AccountId, SchedulerDataSource.Fixed, SchedulerType.D, _FixedScheduler);
                }

                //BatchNext = NextBatch(batchCount,DateToSend,);// dalCamp.Lookup_NextBatch(CampaignId, isMail);

                //dalCamp.Campaigns_UpdateStatus(0, CampaignId);
                if (_SendType == CampaignSendType.Fixed || _SendType == CampaignSendType.Watch)
                {
                    state = CampaignSendState.SentPending;
                }
                else if (IsPending)
                {
                    state = CampaignSendState.SentPending;
                }
                else
                {

                    if (BatchType == BatchTypes.Multi || DestinationCount > MaxItemsPerBatch)
                    {
                        state = CampaignSendState.SentBatch;
                    }
                    else
                    {
                        state = CampaignSendState.SentNow;
                    }
                }

                //if (IsPublishSync)
                //{
                //    return InvokeCampaignSync();
                //}


                if (IsRemote)
                {
                    InvokeRemotePublish();
                }
                else
                {
                    string altKey = ApiCache.CreateAltDestination(SessionId,PublishKey, CampaignId, true);

                    RenderCampaignAsync(new InvokePublishDelegate(AsyncCampaignWorker), altKey);
                }

               
                return sentMessage;

            }
            catch (MsgException mex)
            {
                state = CampaignSendState.HasError;
                if (mex.Status == AckStatus.CacheException)
                {
                    return "הנמענים לא נקלטו, אנא נסה שנית או פנה לתמיכה";
                }
                return mex.Message;
            }
            catch (Exception ex)
            {
                state = CampaignSendState.HasError;
                Log.Exception("InvokeSend error ", ex);
                return ex.Message;
            }
            finally
            {
                if (dalCamp != null)
                {
                    dalCamp.AutoCloseConnection = true;
                }
            }
        }

        public static CampaignSendState InvokeCampaignBatchItems(int campaignId, bool isMail, string sender, int itemsCount, int maxItemPerBatch, string destKey, int userId,decimal unitPrice)
        {
            CampaignSendState state = CampaignSendState.None;

            CampaignEntity campaign = CampaignEntity_Context.Get(campaignId);
            if (campaign.IsEmpty)
            {
                throw new Exception("Invalid campaign");
            }
            string coupon = "";

            string err = "";

            if (!ValidateSend(false, campaign, sender, itemsCount, ref err))
            {
                throw new AppException(err);
            }
            Guid publishKey = Counters.ParsePublishKey(destKey);

            try
            {
                 RemoteApi.Instance.PublishCampaign(
                     campaignId, (int)ExecType.Immediate, sender, coupon, BatchArgs.BatchArgsFormat(BatchTypes.Single,0, maxItemPerBatch, userId, unitPrice,publishKey), destKey);
                if (itemsCount > maxItemPerBatch)
                    state = CampaignSendState.SentBatch;
                else
                    state = CampaignSendState.SentNow;
            }
            catch
            {
                state = CampaignSendState.HasError;
            }

            if (state == CampaignSendState.HasError)
            {
                CampaignPublisher.PendingCampaign(campaignId, ExecType.Immediate, sender, coupon, BatchArgs.BatchArgsFormat( BatchTypes.Scheduled,0, maxItemPerBatch, userId, unitPrice,publishKey), destKey);
                state = CampaignSendState.SentPending;
            }

            //RemoteApi.RemoteClient.PublishCampaign(
            // campaignId, campaign.Method, (int)campaign.CampaignType, (int)ExecType.Send, sender, coupon, campaign.GetValidTimeBegin(), campaign.GetValidTimeEnd(), BatchArgs.BatchArgsFormat(0,maxItemPerBatch), destinationKey);

            //return itemsCount;

            return state;
        }


        #endregion
  
        #region Render methods


        protected string RenderCampaign()
        {
            bool isMail = MsgMethod.IsMail(Method);


            if (ExecPublisher.IsDraft)
            {
                state = CampaignSendState.Saved;
                return saveMessage;
            }
            else if (ExecPublisher.IsSave)
            {

                state = CampaignSendState.Saved;

                return saveMessage;
            }

            //send campaign:

            //BatchNext = NextBatch();

            //dalCamp.Campaigns_UpdateStatus(0, CampaignId);
            if (_SendType == CampaignSendType.Fixed || _SendType == CampaignSendType.Watch)
            {
                state = CampaignSendState.SentPending;
            }
            else if (IsPending)
            {
                state = CampaignSendState.SentPending;
            }
            else
            {

                if (BatchType == BatchTypes.Multi || DestinationCount > MaxItemsPerBatch)
                {
                    state = CampaignSendState.SentBatch;
                }
                else
                {
                    state = CampaignSendState.SentNow;
                }
            }


            //if (IsPublishSync)
            //{
            //    return InvokeCampaignSync();
            //}

            if (IsRemote)
            {
                InvokeRemotePublish();
            }

 
            try
            {
                string altKey = ApiCache.CreateAltDestination(SessionId,PublishKey, CampaignId, true);
                RenderCampaignAsync(new InvokePublishDelegate(AsyncCampaignWorker), altKey);
            }
            catch (MsgException mex)
            {
                state = CampaignSendState.HasError;
                if (mex.Status == AckStatus.CacheException)
                {
                    return "הנמענים לא נקלטו, אנא נסה שנית או פנה לתמיכה";
                }
                return mex.Message;
            }
            catch (Exception)
            {
                InvokeRemotePublish();
            }

            //RemoteApi.RemoteClient.PublishCampaign(
            //CampaignId, Method.ToString(),(int) _SendType, (int)ExecPublisher.ExecType, Sender, BarcodeValue, ValidTimeBegin, ValidTimeEnd, m_instance, DestinationKey);

            return sentMessage;
        }
/*
        protected string InvokeCampaignSync()
        {

            CampaignPublisher.PublishState publishState = CampaignPublisher.PublishState.None;
            int batchId = 0;
            int execResult = 0;
            decimal unitPrice=0;
            try
            {
                bool enableRemote = ViewConfig.EnableRemote;

                if (string.IsNullOrEmpty(_BatchArgs))
                    _BatchArgs = BatchArgs.BatchArgsFormat(BatchTypes.Auto, DestinationCount, MaxItemsPerBatch, UserId, GetUnitPrice());

                //   lock (oLock)
                //{

                Hashtable h = ApiCache.GetActiveDestination(SessionId, CampaignId);

                if (h == null || h.Count == 0)
                {
                    throw new MsgException(AckStatus.CacheException, "Remote Error: Destination Not found, campaignId:" + SessionId);
                }

                if (enableRemote && h.Count > ViewConfig.MaxSyncPublish)
                {
                    ApiCache.SetAltDestination(h, SessionId, CampaignId);

                    try
                    {
                        RemoteApi.Instance.PublishCampaign(
                            CampaignId, (int)ExecPublisher.ExecType, Sender, BarcodeValue, _BatchArgs, SessionId);

                        state = CampaignSendState.SentPending;
                        return sentPenging;
                    }
                    catch (Exception)
                    {
                        //MsgException.Trace(AckStatus.NetworkError, "InvokeCampaignPublish Exception:" + ex.Message);
                        state = CampaignSendState.HasError;
                        return haseError;
                    }
                }

 
                using (CampaignPublisher publisher = new CampaignPublisher(CampaignId, ExecPublisher.ExecType, Sender, BarcodeValue, _BatchArgs))
                {
                    publisher.IsRenderBatch = true;
                    if (!enableRemote)
                    {
                        publisher.IsPending = true;
                    }
                    publishState = (CampaignPublisher.PublishState)publisher.Execute(h.Values);
                    batchId = publisher.BatchId;
                    unitPrice=publisher.UnitPrice;
                    
                }

                if (enableRemote && publishState == CampaignPublisher.PublishState.SendNow && batchId > 0)
                {
                    try
                    {
                        execResult = RemoteApi.Instance.ExecuteCampaign(CampaignId, batchId, UserId);
                        if (execResult > 0)
                        {
                            state = CampaignSendState.SentNow;
                            return sentMessage;
                        }
                    }
                    catch (Exception)
                    {
                        //has_error = true;
                        //MsgException.Trace(AckStatus.NetworkError, "InvokeCampaignPublish Exception:" + ex.Message);

                        using (DalController dal = new DalController())
                        {
                            dal.Campaign_Batch_Enqueue(batchId, ItemUnits, ItemPrice,"publisher pending");
                        }

                        //System.Threading.Thread.Sleep(60000);
                        state = CampaignSendState.SentPending;
                        return sentPenging;
                    }
                }
                if (publishState == CampaignPublisher.PublishState.Pending || publishState == CampaignPublisher.PublishState.Batches)
                {
                    state = CampaignSendState.SentPending;
                    return sentPenging;
                }
            }
            catch (MsgException mex)
            {
                if (mex.Status == AckStatus.CacheException)
                {
                    state = CampaignSendState.HasError;
                    return haseError;
                }
                if (batchId == 0)
                {
                    state = CampaignSendState.None;
                    return notSent;
                }
            }
            catch (Exception)
            {
                state = CampaignSendState.HasError;
                return haseError;
            }

            if (batchId == 0)
            {
                state = CampaignSendState.None;
                return notSent;
            }

            state = CampaignSendState.SentPending;
            return sentPenging;
        }
    */
        //int RenderBilling()
        //{
        //    int billingId = 0;
        //    PlatformType pm = Method == MethodType.MALMT ? PlatformType.Mail : PlatformType.Cell;
        //    DalAccounts.Instance.Account_Billing(ref billingId, AccountId, CampaignId, 0, Cost, ItemPrice, 5, UserId, "Render Campaign Billing", TotalUnits, (int)pm);
        //    return billingId;
        //}

        #region InvokePublish

        //delegate void CampaignPublisherDelegate();

        //void PublishCampaignAsync(string altKey)
        //{
        //    InvokePublishDelegate d = null;
        //    if (IsRemote)
        //    {
        //        d = new InvokePublishDelegate(InvokeRemotePublish);
        //        // RenderCampaignAsync(new InvokePublishDelegate(InvokeRemotePublish), null);
        //    }
        //    else
        //    {
        //        d = new InvokePublishDelegate(InvokeCampaignPublish);
        //        //RenderCampaignAsync(new InvokePublishDelegate(InvokeCampaignPublish), null);
        //        //InvokeRemotePublish();
        //    }

        //    IAsyncResult ar = d.BeginInvoke(altKey,null, null);
        //    while (!ar.IsCompleted)
        //    {
        //        System.Threading.Thread.Sleep(10);
        //    }
        //    d.EndInvoke(ar);

        //}

        //void InvokeRemotePublishAsync()
        //{
        //    InvokePublishDelegate d = new InvokePublishDelegate(InvokeRemotePublish);
        //    IAsyncResult ar = d.BeginInvoke(null, null);
        //    while (!ar.IsCompleted)
        //    {
        //        System.Threading.Thread.Sleep(10);
        //    }
        //    d.EndInvoke(ar);
        //}

        //void InvokeCampaignPublish(string altKey)
        //{
        //    if (string.IsNullOrEmpty(_BatchArgs))
        //        _BatchArgs = BatchArgs.BatchArgsFormat(DestinationCount, MaxItemsPerBatch, UserId,GetUnitPrice());

        //    //RenderDestinationPublishKey();

        //   // string altKey = ApiCache.CreateAltDestination(SessionId, CampaignId, true);


        //    bool has_error = false;

        //    try
        //    {
        //        RemoteApi.Instance.PublishCampaign(
        //            CampaignId, (int)ExecPublisher.ExecType, Sender, BarcodeValue, _BatchArgs, altKey);
        //    }
        //    catch (Exception ex)
        //    {
        //        has_error = true;
        //        MsgException.Trace(AckStatus.NetworkError, "InvokeCampaignPublish Exception:" + ex.Message);
        //    }

        //    if (has_error)
        //    {
        //        if (ExecPublisher.IsDraft)
        //        {
        //            CampaignPublisher.RenderTargets(CampaignId, ExecPublisher.ExecType, UserId, SessionId);
        //        }
        //        else
        //        {
        //            CampaignPublisher.PendingCampaign(CampaignId, ExecPublisher.ExecType, Sender, BarcodeValue, _BatchArgs, SessionId);
        //        }
        //    }

        //    if (state == CampaignSendState.SentPending || state == CampaignSendState.SentBatch)
        //    {
        //        //RenderBilling();
        //    }
        //    //RemoteApi.RemoteClient.PublishCampaign(
        //    //        CampaignId, Method.ToString(), (int)_SendType, (int)ExecPublisher.ExecType, Sender, BarcodeValue, ValidTimeBegin, ValidTimeEnd, _BatchArgs, m_instance, DestinationKey);
        //}

        #endregion

        #region InvokeRemote

        void InvokeRemotePublish()
        {
            if (string.IsNullOrEmpty(_BatchArgs))
                _BatchArgs = BatchArgs.BatchArgsFormat( BatchTypes.Auto,DestinationCount, MaxItemsPerBatch, UserId, GetUnitPrice(),PublishKey);

            Hashtable h = (Hashtable)MControl.Caching.Remote.RemoteSession.Instance(SessionId).Get(ApiCache.ActiveDestination);
            if (h == null || h.Count == 0)
            {
                throw new MsgException(AckStatus.BadDestination, "No destination found");
            }
            
            try
            {
                using (CampaignPublisher pub = new CampaignPublisher(CampaignId, ExecPublisher.ExecType, Sender, BarcodeValue, _BatchArgs))
                {
                    pub.IsRenderBatch = true;
                    pub.Execute(h.Values);
                }
            }
            catch (Exception)
            {
                //MControl.Caching.Remote.RemoteSession.Instance(SessionId).RemoveItem(ApiCache.ActiveDestination);
            }
        }

        //int InvokeRemoteTest(string testDestination)
        //{
        //    int res = 0;
        //    try
        //    {

        //        BatchNext = NextBatch();

        //        _BatchArgs = BatchArgs.BatchArgsFormat(1, MaxItemsPerBatch, UserId, GetUnitPrice());

        //        DestinationItem item = new DestinationItem(testDestination, "", MsgMethod.GetPlatform(Method));


        //        using (CampaignPublisher pub = new CampaignPublisher(CampaignId, ExecPublisher.ExecType, Sender, BarcodeValue, _BatchArgs))
        //        {
        //            // pub.IsRenderBatch = true;
        //            res = pub.Execute(item, BatchId, false);
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        //MsgException.Trace(AckStatus.NetworkError, "RemoteExec Campaign Error: " + ex.Message);
        //        res = -1;
        //    }

        //    return res;
        //}

        #endregion
        
        #region Invoke async

        delegate void InvokePublishDelegate(string altKey);
 
        void AsyncCampaignWorker(string altKey)
        {
            if (string.IsNullOrEmpty(_BatchArgs))
                _BatchArgs = BatchArgs.BatchArgsFormat(BatchTypes.Auto, DestinationCount, MaxItemsPerBatch, UserId, GetUnitPrice(),PublishKey);

              bool has_error = false;

            try
            {
                RemoteApi.Instance.PublishCampaign(
                    CampaignId, (int)ExecPublisher.ExecType, Sender, BarcodeValue, _BatchArgs, altKey);
            }
            catch (Exception ex)
            {
                has_error = true;
                MsgException.Trace(AckStatus.NetworkError,AccountId, "InvokeDynamicAsync Exception:" + ex.Message);
            }

            if (has_error)
            {
                if (ExecPublisher.IsSend || ExecPublisher.IsImmediate )
                {
                    CampaignPublisher.PendingCampaign(CampaignId, ExecPublisher.ExecType, Sender, BarcodeValue, _BatchArgs, altKey);
                }
            }

            //if (state == CampaignSendState.SentPending || state == CampaignSendState.SentBatch)
            //{
            //    //RenderBilling();
            //}
        }


        /// <summary>
        /// Delegate to wrap another delegate and its arguments
        /// </summary>
        delegate void DelegateWrapper(Delegate d, object[] args);

        /// <summary>
        /// An instance of DelegateWrapper which calls InvokeWrappedDelegate,
        /// which in turn calls the DynamicInvoke method of the wrapped
        /// delegate.
        /// </summary>
        static DelegateWrapper wrapperInstance = new DelegateWrapper(InvokeWrappedDelegate);

        /// <summary>
        /// Callback used to call <code>EndInvoke</code> on the asynchronously
        /// invoked DelegateWrapper.
        /// </summary>
        static AsyncCallback callback = new AsyncCallback(EndWrapperInvoke);

        /// <summary>
        /// Executes the specified delegate with the specified arguments
        /// asynchronously on a thread pool thread.
        /// </summary>
        void RenderCampaignAsync(Delegate d, params object[] args)
        {
            // Invoke the wrapper asynchronously, which will then
            // execute the wrapped delegate synchronously (in the
            // thread pool thread)
            wrapperInstance.BeginInvoke(d, args, callback, null);
        }

        /// <summary>
        /// Invokes the wrapped delegate synchronously
        /// </summary>
        static void InvokeWrappedDelegate(Delegate d, object[] args)
        {
            d.DynamicInvoke(args);
        }

        /// <summary>
        /// Calls EndInvoke on the wrapper and Close on the resulting WaitHandle
        /// to prevent resource leaks.
        /// </summary>
        static void EndWrapperInvoke(IAsyncResult ar)
        {
            wrapperInstance.EndInvoke(ar);
            ar.AsyncWaitHandle.Close();
        }

        #endregion

  
        protected virtual int AddCampaign()
        {
            bool shouldUpdate = ExecPublisher.ShouldUpdate();
            int res = 0;
            int status = (int)ExecPublisher.GetStatus();// ? 1 : 0;
            PlatformType platform = MsgMethod.GetPlatform(Method);
            int batchCount = 0;// BatchNext;

            //string replyTo = ReplyTo;
            string notifyCells = GetNotifyCells();
            if (shouldUpdate)
            {
                res = dalCamp.Campaigns_Update(CampaignId, CampaignName, (int)_SendType, Subject, AccountId, DateToSend, ExpirationDate, DestinationCount, status, DateIndex, SmsMessage, BarcodeValue, MControl.Strings.StringTimeToInt(ValidTimeBegin), MControl.Strings.StringTimeToInt(ValidTimeEnd), (int)NotifyType, batchCount, ExecPublisher.IsDraft, this.Sender, notifyCells, PersonalLength, DesignId, Display,ReplyTo, (int)BillType, PersonalFields, PersonalDisplay, (int)Method, Features.ToString(), (int)platform);
                //DeleteDraft(CampaignId);
            }
            else
            {
                res = dalCamp.Campaigns_Insert(ref CampaignId, CampaignName, (int)_SendType, Subject, AccountId, DateToSend, ExpirationDate, DestinationCount, status, DateIndex, SmsMessage, BarcodeValue, MControl.Strings.StringTimeToInt(ValidTimeBegin), MControl.Strings.StringTimeToInt(ValidTimeEnd), (int)NotifyType, batchCount, ExecPublisher.IsDraft, this.Sender, notifyCells, PersonalLength, DesignId, Display, ReplyTo, (int)BillType, PersonalFields, PersonalDisplay, (int)Method, (int)ProductType, Features.ToString(), (int)platform);
            }
            if (CampaignId <= 0)
            {
                throw new Exception("אירעה שגיאה ביצירת הקמפיין,הקמפיין לא נשלח");
            }


#if(RB)
            if (BillType == BillingType.RB)
            {
                if (shouldUpdate)//(IsEdited)
                    res = dalCamp.Campaigns_RB_Update(CampaignId, PriceCode, (int)DeviceRule, AppId, /*(int)ProductType,*/ SignCount, 0, (int)SendInterval);
                else
                    res = dalCamp.Campaigns_RB_Insert(CampaignId, PriceCode, (int)DeviceRule, AppId, /*(int)ProductType,*/ SignCount, 0, (int)SendInterval);
            }
#endif
            return res;
        }
              
     

        //protected void DeleteDraft(int campaignId)
        //{
        //    if (campaignId > 0)
        //    {
        //        dalCamp.Campaigns_Draft_Delete(CampaignId);
        //    }
        //}

        #endregion
    }
}
