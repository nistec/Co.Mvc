using Netcell;
using Netcell.Data.Db;
using Netcell.Data.Entities;
using Netcell.Data.Rules;
using Netcell.Remoting;
using Nistec;
using Nistec.Generic;
using Nistec.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netcell.Lib
{
    public class CampaignPublisher :IDisposable
    {

        enum PublishState
        {
            None = 0,
            Draft = 1,
            Fixed = 2,
            Watch = 3,
            Pending = 4,
            Batches = 5,
            Saved = 6,
            SendNow = 9,
        }

        bool EnableServerRules;
        BatchTypes BatchType;
        protected int ItemUnits;
        protected int ItemSize;
        protected int TotalUnits;
        protected decimal Cost;
        protected decimal ItemPrice;

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
        #endregion


        #region members
        bool initilaized;
        DalCampaign dalCamp;
        MethodType Method;
        string Sender;
        int CampaignId;
        string CampaignName;
        int MaxItemsPerBatch;
        int MaxPersonalLength;
        //int DestinationCount;
        int AccountId;
        BillingType BillingType;
        string CouponValue;
        DateTime DateToSend;
        DateTime ExpirationDate;
        string ValidTimeBegin;
        string ValidTimeEnd;

        CampaignSendType _SendType = CampaignSendType.Now;
        ExecTypePublisher _ExecPublisher;
        ExecType _ExecType;
      
        Guid _PublishKey;


        public PlatformType Platform
        {
            get;
            set;
        }
        public bool IsPending
        {
            get;
            set;
        }

        public bool IsRenderBatch
        {
            get; internal set;
        }

        //public int BatchId
        //{
        //    get; internal set;
        //}
        public int UserId
        {
            get; internal set;
        }
        public decimal UnitPrice
        {
            get; internal set;
        }

        #endregion


        public bool IsPersonal { get; set; }
        public string PersonalFields { get; set; }
        public string PersonalDisplay { get; set; }

        public CampaignEntity Campaign { get; set; }
        public CampaignSchedule Schedule { get; set; }
        public CampaignNotify Notify { get; set; }
        public TargetItemList TargetList { get; set; }
        public int DestinationCount { get { return TargetList == null ? 0 : TargetList.Count; } }

        public CampaignContent Content { get; private set; }

        public string SmsMessage { get; set; }
        public string HtmlContent { get; set; }
        public string Subject { get; set; }
        public bool HasContentChanged { get; private set; }

       
        public CampaignPublisher(string campaignName, int accountId, MethodType method, BillingType billingType, int maxItemsPerBatch, DateTime? dateToSend, int userId)
        {
            _PublishKey = UUID.NewUuid();
            AccountId = accountId;
            MaxItemsPerBatch = maxItemsPerBatch > 0 ? maxItemsPerBatch : DefaultMaxItemsPerBatch;
            CampaignName = campaignName;
            CampaignId = 0;
            if (AccountId <= 0)
            {
                throw new Exception("Invalid Account");
            }

            UserId = userId;
            IsPending = false;
            DateToSend = dateToSend==null? DateTime.Now: dateToSend.Value;
            IsPending = (DateToSend > DateTime.Now.AddMinutes(2));

            //ActiveAccountCreditInfo aci = new ActiveAccountCreditInfo(AccountId, method.ToString());// MsgMethod.MediaTypeToMethodMT(mediaType).ToString());

            BillingType = billingType;
            BillingType = MsgMethod.ResolveBillingType(BillingType);
            ExpirationDate = ApiUtil.MaxDate;
            Method = method;
            SmsMessage = "";

            IsPersonal = false;
            ValidTimeBegin = ViewConfig.DefaultValidTimeBegin;
            ValidTimeEnd = ViewConfig.DefaultValidTimeEnd;
            errs = new StringBuilder();// new List<string>();
            dalCamp = DalCampaign.Instance;
            dalCamp.AutoCloseConnection = false;

        }

        public virtual void InitCampaign(bool isPersonal,string personalDisplay, string personalFields, string subject, TargetItemList destinations, CampaignContent content, CampaignSendType sendType, bool isPublish)
        {
            //IsPublishSync = isPublish;
            IsPersonal = isPersonal;
            PersonalDisplay = personalDisplay;
            PersonalFields = personalFields;

            SmsMessage = content.Message;
            HtmlContent = content.HtmlContent;
            Subject = subject;

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
                HtmlContent = content.HtmlContent;
                ItemUnits = content.ContentUnits;
                ItemSize = content.ContentSize;
                HasContentChanged = content.HasChanges;
            }
            //else if (MsgMethod.IsMwp(Method))
            //{
            //    ItemUnits = content.ContentUnits + altcontent.ContentUnits;
            //    ItemSize = content.ContentSize + altcontent.ContentSize;
            //    HasContentChanged = content.HasChanges;
            //    HasAlterChanged = altcontent.HasChanges;
            //}
            else if (MsgMethod.IsMail(Method, false))
            {
                HtmlContent = content.HtmlContent;
                ItemUnits = content.ContentUnits;
                ItemSize = content.ContentSize;
                HasContentChanged = content.HasChanges;
            }

            //DesignHeader = content.Header;
            //DesignFooter = content.Footer;

            TargetList = destinations;
            MaxPersonalLength = destinations.GetMaxPersonalLength();
            //DestinationKey = dest.DestinationKey;

            _SendType = sendType;

            initilaized = true;
        }
        
        #region Ctor
        /*
                public CampaignPublisher(int campaignId, ExecType execType, int userId, Guid publishKey)
                    : this(campaignId, execType, null, null, null)
                {
                    UserId = userId;
                }

                public CampaignPublisher(int campaignId, string sender, string dateToSend, int userId, Guid publishKey)
                {
                    UserId = userId;
                    _PublishKey = publishKey;
                    InitCampaignPublisher(campaignId);

                    if (!string.IsNullOrEmpty(sender))
                    {
                        Sender = sender;
                    }
                    ExecType execType = ExecType.Immediate;

                    if (!string.IsNullOrEmpty(dateToSend))
                    {
                        DateToSend = DateHelper.ToDateTime(dateToSend);
                        if (DateToSend > DateTime.Now.AddMinutes(2))
                            execType = ExecType.Send;
                    }
                    SetExecPublisher(execType);
                    MaxItemsPerBatch = CampaignBuilder.DefaultMaxItemsPerBatch;
                }

                public CampaignPublisher(int campaignId, ExecType execType, string sender, string coupon, string batchArgs)
                {

                    InitCampaignPublisher(campaignId);
                    if (!string.IsNullOrEmpty(sender))
                    {
                        Sender = sender;
                    }
                    CouponValue = coupon;

                    SetExecPublisher(execType);

                    _BatchArgs = new BatchArgs(batchArgs);
                    UserId = _BatchArgs.UserId;
                    _UnitPrice = _BatchArgs.UnitPrice;
                    _PublishKey = Types.ToGuid(_BatchArgs.PublishKey);

                    MaxItemsPerBatch = _BatchArgs.MaxItemsPerBatch;

                    //errs = new StringBuilder();
                    //dalCamp = DalCampaign.Instance;
                    //dalCamp.AutoCloseConnection = false;

                }

                private void InitCampaignPublisher(int campaignId)
                {
                    CampaignId = campaignId;

                    //init
                    using (CampaignEntity_Context context = new CampaignEntity_Context(campaignId))
                    {
                        if (context.IsEmpty)
                        {
                            throw new Exception("Invalid campaign to publish");
                        }
                        CampaignEntity campaign = context.Entity;
                        Method = (MethodType)campaign.MtId;
                        _SendType = (CampaignSendType)campaign.CampaignType;
                        Sender = campaign.Sender;
                        ValidTimeBegin = campaign.GetValidTimeBegin();
                        ValidTimeEnd = campaign.GetValidTimeEnd();
                        DateToSend = campaign.DateToSend;
                        Platform = (PlatformType)campaign.Platform;
                        AccountId = campaign.AccountId;
                        //campaign.Dispose();
                    }

                    errs = new StringBuilder();
                    dalCamp = DalCampaign.Instance;
                    dalCamp.AutoCloseConnection = false;
                }

                private void SetExecPublisher(ExecType execType)
                {
                    _ExecPublisher = new ExecTypePublisher(execType);
                    if (_ExecPublisher.IsImmediate)
                    {
                        DateToSend = DateTime.Now;
                        IsPending = false;
                    }
                    else
                    {
                        IsPending = DateToSend > DateTime.Now.AddMinutes(2);
                    }
                }
                */

        ~CampaignPublisher()
        {
            Dispose();
        }

        public void Dispose()
        {
            errs = null;
            Sender = null;
            ValidTimeBegin = null;
            ValidTimeEnd = null;
            CouponValue = null;

            if (TargetList != null)
            {
                TargetList.Clear();
                TargetList = null;
            }

            if (dalCamp != null)
            {
                dalCamp.AutoCloseConnection = true;
                dalCamp.Dispose();
            }
        }


        #endregion

        #region private methods

        private decimal GetUnitPrice()
        {
            int count = DestinationCount;
            if (count <= 0)
                return 0;
            return Cost / count;
        }

        //tversion
        //private int NextBatch(int batchCount, DateTime execTime, int batchIndex, int batchRange)
        //{
        //    //return dalCamp.GetNextBatch(CampaignId, Method== MethodType.MALMT);

        //    int batchId = 0;

        //    using (DalController dal = new DalController())
        //    {
        //        dal.Trans_Batch_Insert(ref batchId, AccountId, CampaignId, batchCount, 0, execTime, batchIndex, batchRange, (int)BatchType, UserId, 0, (int)MsgMethod.GetPlatform(Method), (int)Method, Cost, PublishKey.ToString());
        //    }
        //    return batchId;

        //    //return Counters.BatchId(CampaignId, AccountId, (int)Method,(int) BillType);
        //}

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

    
        #region Content

        public void SetContent(CampaignContent content)
        {
            Content = content;
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
                HtmlContent = content.HtmlContent;
            }
            else if (MsgMethod.IsMwp(Method))
            {
                ItemUnits = content.ContentUnits;
                ItemSize = content.ContentSize;
                HasContentChanged = content.HasChanges;
                HtmlContent = content.HtmlContent;
            }
            else if (MsgMethod.IsMail(Method, false))
            {
                ItemUnits = content.ContentUnits;
                ItemSize = content.ContentSize;
                HasContentChanged = content.HasChanges;
                HtmlContent = content.HtmlContent;
            }
        }

        /*
        protected virtual int InvokeMailContent(bool isPreview = false)
        {
            string html = ApiCache.GetMailContent(SessionId, CampaignId);//..GetAsString(SessionId, MailContent);
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
            mv.LoadAccountFeatures(AccountId, Sender, MsgMethod.IsMwp(Method), lang);
            //mv.CampaignId = CampaignId;
            //mv.EnableMailMobile = MsgMethod.IsMwp(Method);
            mv.ExtractView(UploadLocalPath, UploadVirtualPath, WebClientLocalPath, WebClientVirtualPath, ProductType == ProductType.Quiz);
            mv.SaveContent(isPreview);//!shouldUpdate);

            return saveState;
        }

        protected virtual int InvokeMailPreview()
        {
            string html = ApiCache.GetMailContent(SessionId, CampaignId);//.GetAsString( SessionId, MailContent);
            if (string.IsNullOrEmpty(html))
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

        public static bool SaveMailContent(ICampaignContent content, ICampaignDef def, int accountId, string personalDisplay, Lang lang, bool isPreview = false)
        {
            try
            {
                MailViewBuilder mv = new MailViewBuilder(def.CampaignId, accountId, content);
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
*/

        public static bool SaveSmsContent(string message, ICampaignEntity def)
        {
            int res = 0;
            try
            {
                using (CampaignEntity_Context context = new CampaignEntity_Context(def.CampaignId))
                {
                    context.Entity.MessageText = message;
                    res = context.SaveChanges();
                }

                return res > 0;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return false;
            }
        }

        #endregion

        #region Coupon

        //public void SetBarcode(Barcode c)
        //{
        //    //_Coupon = c;
        //    if (c != null)//&& c.CouponType == Barcode.eCouponType.Barcode)
        //    {
        //        Features.CouponSource = (CouponSource)(int)c.BarcodeSource;
        //        Features.CouponType = (CouponType)(int)c.BarcodeType;
        //        BarcodeValue = c.BarcodeValue;
        //        //WapItems.Add(new WapItem("Barcode", "../img/Barcode.jpg", "", WapItemType.Barcode, c.FormId));
        //    }
        //    else
        //    {
        //        WapItems.Remove(WapItemType.Barcode);
        //    }
        //}

        #endregion

        #region Timing
        /*
            public static DateTime DefaultExpiration
            {
                get { return DateTime.Now.AddYears(1); }
            }

            public static System.Globalization.DateTimeFormatInfo DateFormat
            {
                //get{return new System.Globalization.DateTimeFormatInfo;}
                get { return new System.Globalization.CultureInfo("he-IL", false).DateTimeFormat; }
            }
            public void SetValidTime(CampaignSchedule timing)
            {
                //TimingType = timingType;
                BatchType = BatchTypes.Auto;
                DateToSend = Types.ToDateTime(timing.GetValidTimeToSend(), DateFormat, DateTime.Now);
                IsPending = DateToSend > DateTime.Now.AddMinutes(2);
                if (timing.SendType== CampaignSendType.Now)
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
                    //case CampaignSendType.Fixed:
                    //    _FixedScheduler = new FixedScheduler(timing);
                    //    fixedInitilaized = true;
                    //    break;
                    case CampaignSendType.Batches:
                        SetBatches(timing);
                        break;
                }
            }

            void SetBatches(CampaignSchedule timing)
            {
                bool[] days = new bool[7];
                int validDays = 0;
                for (int d = 0; d < 7; d++)
                {
                    days[d] = timing.BatchDays[d];//.BatchDays[d].Selected;
                    validDays += days[d] ? 1 : 0;
                }
                if (validDays == 0)
                {
                    throw new Exception("לא סומנו ימים לחלוקת המנות");
                }
                BatchType = timing.IsMultiBatch ? BatchTypes.Multi : BatchTypes.Single;
                _BatchArgs = BatchArgs.BatchArgsFormat(timing.IsMultiBatch ? BatchTypes.Multi : BatchTypes.Single, timing.BatchValue, timing.BatchDelay, timing.BatchDelayMode, MaxItemsPerBatch, days, UserId, GetUnitPrice(), PublishKey);

            }
    */

        #endregion

        #region Validataion

      
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

        public virtual bool Validate()
        {
            bool isValid = false;

            if (!initilaized)
            {
                throw new Exception("Campaign not initilaized!!!");
            }

            //errs.Clear();
            errs = new StringBuilder();

            if (Method == MethodType.MALMT || Method == MethodType.MALWP)
            {
                //if (string.IsNullOrEmpty(MessageText))
                //    errs.Append("חסר נוסח ההודעה");
                if (string.IsNullOrEmpty(Sender))
                    errs.AppendLine("חסר מאת");
                if (!Regx.RegexValidate("^[a-zA-Z0-9-_.]+$", Sender))
                    errs.AppendLine("שם השולח אינו תקין נדרש אותיות באנגלית ו או מספרים ו או (_ - .)");
                if (string.IsNullOrEmpty(Subject))
                    errs.AppendLine("חסר נושא ההודעה");
                if (string.IsNullOrEmpty(HtmlContent))
                    errs.AppendLine("לא נמצא תוכן מולטימדיה");
            }
            else if (Method == MethodType.SMSWP)
            {
                if (string.IsNullOrEmpty(SmsMessage))
                    errs.AppendLine("חסר הודעה מקדימה");
                if (string.IsNullOrEmpty(Sender))
                    errs.AppendLine("חסר מאת");
                if (string.IsNullOrEmpty(HtmlContent))
                    errs.AppendLine("לא נמצא תוכן מולטימדיה");
            }
            else
            {
                if (string.IsNullOrEmpty(SmsMessage))
                    errs.AppendLine("חסר נוסח ההודעה");
                if (string.IsNullOrEmpty(Sender))
                    errs.AppendLine("חסר מאת");
            }


            if (string.IsNullOrEmpty(CampaignName))
                errs.AppendLine("חסר שם דיוור");

            if(_ExecType == ExecType.Send || _ExecType== ExecType.SendNew)
            {

                if (DestinationCount == 0)
                    errs.AppendLine("חסר נמענים");
                if (!ValidateTimeBeginAndEnd())//credit, priceUnit))
                    errs.AppendLine("זמני שליחה אינם תקניים");
                if (!ValidateCredit())
                    errs.AppendLine("חסר אשראי לשליחה");


                if (!initilaized)
                {
                    throw new Exception("Campaign not initilaized!!!");
                }
                if (_SendType == CampaignSendType.Fixed)// && !fixedInitilaized)
                {
                    throw new Exception("Campaign Fixed Scheduler not supported!!!");
                }
                if (_SendType == CampaignSendType.Pending && !IsPending)
                {
                    throw new Exception("Campaign send time should be pending!!!");
                }

            }

            isValid = errs.Length == 0;

            return isValid;
        }

        public bool ValidateTimeBeginAndEnd()
        {
            return (Strings.StringTimeToTimeSpan(ValidTimeBegin) <= Strings.StringTimeToTimeSpan(ValidTimeEnd));
        }
        #endregion

        #region Execute

        /*
        delegate int CampaignPublisherDelegate(ICollection items);


        public int ExecuteAsync(ICollection items)
        {
            CampaignPublisherDelegate d = new CampaignPublisherDelegate(Execute);

            IAsyncResult ar = d.BeginInvoke(items, null, null);
            while (!ar.IsCompleted)
            {
                System.Threading.Thread.Sleep(10);
            }
            return d.EndInvoke(ar);
        }
        */

        //protected virtual int AddCampaign()
        //{
        //    bool shouldUpdate = ExecPublisher.ShouldUpdate();
        //    int res = 0;
        //    int status = (int)ExecPublisher.GetStatus();// ? 1 : 0;
        //    PlatformType platform = MsgMethod.GetPlatform(Method);
        //    int batchCount = 0;// BatchNext;

        //    //string replyTo = ReplyTo;
        //    string notifyCells = GetNotifyCells();
        //    if (shouldUpdate)
        //    {
        //        res = dalCamp.Campaigns_Update(CampaignId, CampaignName, (int)_SendType, Subject, AccountId, DateToSend, ExpirationDate, DestinationCount, status, DateIndex, SmsMessage, BarcodeValue, Strings.StringTimeToInt(ValidTimeBegin), Strings.StringTimeToInt(ValidTimeEnd), (int)NotifyType, batchCount, ExecPublisher.IsDraft, this.Sender, notifyCells, PersonalLength, DesignId, Display, ReplyTo, (int)BillType, PersonalFields, PersonalDisplay, (int)Method, Features.ToString(), (int)platform);
        //        //DeleteDraft(CampaignId);
        //    }
        //    else
        //    {
        //        res = dalCamp.Campaigns_Insert(ref CampaignId, CampaignName, (int)_SendType, Subject, AccountId, DateToSend, ExpirationDate, DestinationCount, status, DateIndex, SmsMessage, BarcodeValue, Strings.StringTimeToInt(ValidTimeBegin), Strings.StringTimeToInt(ValidTimeEnd), (int)NotifyType, batchCount, ExecPublisher.IsDraft, this.Sender, notifyCells, PersonalLength, DesignId, Display, ReplyTo, (int)BillType, PersonalFields, PersonalDisplay, (int)Method, (int)ProductType, Features.ToString(), (int)platform);
        //    }
        //    if (CampaignId <= 0)
        //    {
        //        throw new Exception("אירעה שגיאה ביצירת הקמפיין,הקמפיין לא נשלח");
        //    }

        //    return res;
        //}

        public CampaignSendState Execute(ExecType execType)
        {
            return ExecuteInternal(execType);
        }

        public CampaignSendState Execute(ExecType execType,List<TargetItem> items)
        {
            TargetList = new TargetItemList(items);
            return ExecuteInternal(execType);
        }

        private CampaignSendState ExecuteInternal(ExecType execType)
        {
            _ExecType = execType;

            if(!Validate())
                return CampaignSendState.HasError;

            ExecTypePublisher execPublisher = new ExecTypePublisher(execType);

            if (execPublisher.IsTest)
            {
                return CampaignSendState.SentTest;// RenderCampaign();
            }
            if (execPublisher.IsDraft)
            {
                return  CampaignSendState.Saved;
            }

            if (_SendType == CampaignSendType.Watch)
            {
                return CampaignSendState.SentWatch;
            }
            if (_SendType == CampaignSendType.Fixed)
            {
                RenderTargets(true);
                return CampaignSendState.SentFixed;
            }
            if (_ExecPublisher.IsSave)
            {
                return CampaignSendState.Saved;
            }


            //if (_BatchArgs.BatchType == BatchTypes.Multi)
            //    SetBatches(_BatchArgs.BatchType, _BatchArgs.BatchValue, _BatchArgs.Delay, _BatchArgs.DelayMode, _BatchArgs.Days);
            //else
            //    SetBatches();

            SetBatches();

            return RenderCampaign();

        }

        #endregion

        #region Targets


        private int RenderTargets(bool isAsync)
        {

            if (TargetList.Count <= 0)
            {
                //throw new Exception("Invalid Targets");
                return 0;
            }
            int index = 0;
            try
            {
                string table = "Campaigns_Targets";
                DataTable dt = DalUpload.CampaignTargets_Schema();

                object[] values = null;
                int destCount = DestinationCount;

                foreach (var item in TargetList)
                {
                    values = new object[] { CampaignId, item.Target, index, ApiUtil.FormatString(item.Personal, 250), ApiUtil.FormatString(item.Coupon, 50), item.Sender, item.GroupId, item.ContactId, item.Prefix, item.Date };
                    dt.Rows.Add(values);
                    index++;
                }

                dalCamp.Campaigns_Targets_Delete(CampaignId);
                dalCamp.InsertTable(dt, table);
                //RenderAsyncDestination(dt, table,isAsync);

            }
            catch (Exception ex)
            {
                new MsgException(AckStatus.SqlException, "Error RenderTargets " + ex.Message);
            }
            return index;
        }

        #endregion

        #region Batches

        List<BatchListItem> _BatchList;

        List<BatchListItem> BatchList
        {
            get
            {
                if (_BatchList == null)
                {
                    _BatchList = new List<BatchListItem>();
                }
                return _BatchList;
            }
        }


        void SetBatches(BatchTypes batchTypes, int batchValue, int delay, int delayMode)
        {
            SetBatches(batchTypes, batchValue, delay, delayMode, new bool[] { true, true, true, true, true, true, true });
        }

        void SetBatches(BatchTypes batchTypes, int batchValue, int delay, int delayMode, bool[] days)
        {
            BatchList.Clear();
            int ValidNumbers = DestinationCount;
            decimal batchPrice = ValidNumbers * UnitPrice;

            if (batchTypes == BatchTypes.Single)
            {
                //tversion batchOffset = NextBatch();

                int batchId = Counters.BatchId(AccountId, CampaignId, ValidNumbers, BatchTypes.Single, UserId, Platform, (int)Method, batchPrice, _PublishKey.ToString());

                BatchListItem cb = new BatchListItem(DateToSend, ValidNumbers, batchId, 0, 0, batchPrice, _PublishKey);
                //cb.SendTime = DateToSend;
                //cb.BatchValue = ValidNumbers;
                //cb.BatchId = NextBatch(); //tversion batchOffset;
                BatchList.Add(cb);

                return;
            }
            DateTime timeStart = DateToSend;
            DayOfWeek day = (DayOfWeek)timeStart.DayOfWeek;
            if (days == null || days.Length < 7)
            {
                days = new bool[] { true, true, true, true, true, true, true };
                //throw new Exception("לא סומנו ימים לחלוקת המנות");
            }

            int totalCount = ValidNumbers;
            if (totalCount == 0)
            {
                throw new Exception("לא סומנו נמענים לשליחה");
            }
            TimeSpan timeBegin = Strings.StringTimeToTimeSpan(ValidTimeBegin);
            TimeSpan timeEnd = Strings.StringTimeToTimeSpan(ValidTimeEnd);
            int BatchCount = (int)Math.Ceiling((decimal)((decimal)totalCount / (decimal)batchValue));
            if (BatchCount == 0)
            {
                throw new Exception("נתוני חלוקה למנות אינם תקינים");
            }


            //tversion batchOffset = NextBatch();

            int batchVal = totalCount / BatchCount;
            int sum = 0;
            DateTime newTime = timeStart;
            batchPrice = batchVal * UnitPrice;

            for (int i = 0; i < BatchCount; i++)
            {
                newTime = GetBatcheTime(newTime, timeBegin, timeEnd, delay, delayMode, days, 0, i == 0);
                if (i == BatchCount - 1)
                {
                    batchVal = totalCount - sum;
                }
                sum += batchVal;

                int batchId = Counters.BatchId(AccountId, CampaignId, batchVal, newTime, i, BatchCount, BatchTypes.Multi, UserId, Platform, (int)Method, batchPrice, _PublishKey.ToString());

                BatchListItem cb = new BatchListItem(newTime, batchVal, batchId, i, BatchCount, batchPrice, _PublishKey);

                BatchList.Add(cb);
                //Console.WriteLine(batchTimes[batchTimes.Count - 1].ToString());
            }

            //MaxBatchIndex = dalCamp.GetNextBatch(CampaignId, batchOffset + BatchCount);// batchOffset + BatchCount;
        }

        void SetBatches()
        {
            if (BatchList.Count > 0)
            {
                return;
            }

            BatchList.Clear();
            int ValidNumbers = DestinationCount;
            decimal batchPrice = ValidNumbers * UnitPrice;

            if(Schedule.SendType== CampaignSendType.Batches)
            {
                SetBatches(BatchTypes.Scheduled, Schedule.BatchValue,Schedule.BatchDelay,Schedule.BatchDelayMode,Schedule.BatchDays.ToArray());
            }
            else if (_SendType != CampaignSendType.Watch && ValidNumbers > MaxItemsPerBatch)
            {
                SetBatches(BatchTypes.Auto, MaxItemsPerBatch, 10, 0);
            }
            else
            {
                int batchId = Counters.BatchId(AccountId, CampaignId, ValidNumbers, DateToSend, 0, 0, BatchTypes.Auto, UserId, Platform, (int)Method, batchPrice, _PublishKey.ToString());

                BatchListItem cb = new BatchListItem(DateToSend, ValidNumbers, batchId, 0, 0, batchPrice, _PublishKey);

                BatchList.Add(cb);
            }
        }

        DateTime GetBatcheTime(DateTime timeStart, TimeSpan timeBegin, TimeSpan timeEnd, int delay, int delayMode, bool[] days, int index, bool isTimeBegin)
        {
            DateTime newTime = isTimeBegin ? timeStart : delayMode == 0 ? timeStart.AddMinutes(delay) : timeStart.AddHours(delay);
            DayOfWeek day = (DayOfWeek)newTime.DayOfWeek;
            TimeSpan spn = new TimeSpan(newTime.Hour, newTime.Minute, 0);

            if (!days[(int)day])
            {
                newTime = newTime.AddDays(1);
                newTime = new DateTime(newTime.Year, newTime.Month, newTime.Day, timeBegin.Hours, timeBegin.Minutes, timeBegin.Seconds);
                return GetBatcheTime(newTime, timeBegin, timeEnd, delay, delayMode, days, index + 1, true);
            }
            else if (spn < timeBegin)
            {
                return new DateTime(newTime.Year, newTime.Month, newTime.Day, timeBegin.Hours, timeBegin.Minutes, timeBegin.Seconds);
            }
            else if (spn > timeEnd)
            {
                return GetBatcheTime(newTime, timeBegin, timeEnd, delay, delayMode, days, index + 1, false);
            }
            else if (index > 60)
            {
                return newTime;
            }
            else
            {
                return newTime;
            }
        }

        //tversion
        //private int NextBatch()
        //{
        //    //return dalCamp.GetNextBatch(CampaignId, Method == MethodType.MALMT);

        //    return Counters.BatchId(CampaignId);
        //}

        #endregion

        #region Render

        protected int RenderDestination()
        {
            if (BatchList.Count <= 0)
            {
                throw new Exception("Invalid Destination BatchList");
            }
            return BatchRender.RenderDestination(CampaignId, TargetList, BatchList);
        }


        private CampaignSendState RenderCampaign()
        {

            CampaignSendState state = CampaignSendState.None;
 
            RenderDestination();

            dalCamp.Campaigns_UpdateStatus(0, CampaignId);

            int batchCount = 0;

            if (_SendType == CampaignSendType.Fixed)
            {
                // do nothing
                return CampaignSendState.SentFixed;
            }
            else if (BatchList.Count > 1)
            {
                batchCount=RenderBatch(PublishState.Batches);
                return batchCount > 0 ? CampaignSendState.SentBatch : CampaignSendState.None;
            }
            else if (IsPending)
            {
                batchCount = RenderBatch(PublishState.Pending);
                return batchCount > 0 ? CampaignSendState.SentPending: CampaignSendState.None;
            }
            else if (BatchList.Count > 0)
            {
                //BatchId = BatchList[0].BatchId;
                batchCount = RenderBatch(PublishState.SendNow);
                return batchCount > 0 ? CampaignSendState.SentNow : CampaignSendState.None;
            }
            
            return CampaignSendState.None;
        }

        private int RenderBatch(PublishState state)
        {
            int server = 0;// ViewConfig.Server;

            if (EnableServerRules)
            {
                server = Server_Rules_Context.Lookup_Server_Rules(AccountId, (int)Platform);
            }

            if (BatchList.Count <= 0)
            {
                SetBatches();
            }
            int batchCount = BatchList.Count;
            int batchState = _SendType == CampaignSendType.Fixed ? 11 : 0;
            bool isPending = state == PublishState.Pending || state == PublishState.Batches;
            int units = 0;
            if (batchCount > 0)
            {
                SchedulerItemType schedulerType = isPending ? SchedulerItemType.Scheduled : SchedulerItemType.Executed;

                using (DalController dal = new DalController())
                {
                    dal.AutoCloseConnection = false;

                    foreach (BatchListItem cb in BatchList)
                    {
                        dal.Scheduler_Enqueue(CampaignId, (int)schedulerType, cb.BatchValue, cb.BatchIndex, cb.BatchRange, cb.BatchPrice, "Batch", cb.SendTime, AccountId, cb.BatchId, cb.PublishKey.ToString(), UserId, server, (int)Method, units);
                    }
                }
            }
            //if (BatchList.Count > 0)
            //    BatchId = BatchList[0].BatchId;
            return batchCount;
        }


        //private int RenderScheduledBatch(int batchId, DateTime SendTime)
        //{
        //    int server = 0;// ViewConfig.Server;

        //    int batchCount = _BatchArgs.BatchValue;

        //    decimal BatchPrice = batchCount * _BatchArgs.UnitPrice;
        //    int units = 0;

        //    using (DalController dal = new DalController())
        //    {
        //        server = 0;// appservers.NextServerId();
        //        dal.Scheduler_Enqueue(CampaignId, (int)SchedulerItemType.Scheduled, batchCount, 0, 0, BatchPrice, "Batch", SendTime, AccountId, batchId, _BatchArgs.PublishKey, _BatchArgs.UserId, server, (int)Method, units);
        //    }

        //    BatchId = batchId;
        //    return batchCount;
        //}

        #endregion

        #region Remote Exec 
        /*
        public int RemoteExec(int batchId, int userId)
        {
            try
            {
                RemoteApi api = new RemoteApi();
                return api.ExecuteCampaign(CampaignId, batchId, userId);
            }
            catch (Exception ex)
            {
                MsgException.Trace(AckStatus.NetworkError, "RemoteExec Campaign Error: " + ex.Message);

                return -1;
            }
        }
    */
        #endregion

        #region static
        /*
        public static int PendingCampaign(int campaignId, ExecType execType, string sender, string coupon, string batchArgs, string destKey)
        {
            Netlog.DebugFormat("PendingCampaign: {0}, DestKey:{1}", campaignId, destKey);
            //CacheItem item = null;
            //bool begin_publishe = false;

            try
            {

                if (campaignId <= 0)
                {
                    throw new MsgException(AckStatus.ApplicationException, "PendingCampaign Error: Campaign Not found, campaignId:" + campaignId.ToString());
                }

                Hashtable h = ApiCache.GetAltDestination(destKey, campaignId, 0);// (Hashtable)RemoteSession.Instance(destKey).Get(ApiCache.ActiveDestination);

                if (h == null)
                {
                    throw new MsgException(AckStatus.FatalException, "PendingCampaign Error: Destination Not found, campaignId:" + destKey);
                }

                //MControl.Caching.Remote.RemoteCacheClient.Instance.RemoveItem(destKey);

                //begin_publishe = true;

                using (CampaignPublisher publisher = new CampaignPublisher(campaignId, execType, sender, coupon, batchArgs))
                {

                    publisher.IsPending = true;

                    CampaignPublisher.PublishState state = (CampaignPublisher.PublishState)publisher.Execute(h.Values);
                }

                TraceAsync.Execute("publish-dest", destKey, "", campaignId, (int)AckStatus.Ok, AckStatus.Ok.ToString(), "Ok", ViewConfig.Server);
                //Counters.PublishComment(destKey, campaignId, (int)AckStatus.Ok, AckStatus.Ok.ToString(), "Ok", ViewConfig.Server);

                return 0;
            }
            catch (MsgException mex)
            {
                Counters.PublishComment(destKey, campaignId, (int)mex.Status, mex.Status.ToString(), mex.Message, ViewConfig.Server);
                return -1;
            }
            catch (Exception ex)
            {

                Counters.PublishComment(destKey, campaignId, (int)AckStatus.UnExpectedError, AckStatus.UnExpectedError.ToString(), ex.Message, ViewConfig.Server);

                //if (begin_publishe)
                //{
                //    MControl.Caching.Remote.RemoteCacheClient.Instance.AddItem(item);
                //}
                Log.ErrorFormat("PendingCampaign error: {0}", ex.Message);
                Log.ErrorFormat("PendingCampaign errorTrace: {0}", ex.StackTrace);

                return -1;
            }
            finally
            {
                ApiCache.RemoveAltDestination(destKey);
                //RemoteSession.Instance(destKey).RemoveItem(ApiCache.ActiveDestination);
            }
        }

        public static int RenderTargets(int campaignId, ExecType execType, int userId, string destKey)
        {
            Log.DebugFormat("RenderTargetsCampaign: {0}, DestKey:{1}", campaignId, destKey);
            //CacheItem item = null;
            //bool begin_publishe = false;
            try
            {
                if (campaignId <= 0)
                {
                    throw new MsgException(AckStatus.ApplicationException, "RenderTargetsCampaign Error: Campaign Not found, campaignId:" + campaignId.ToString());
                }

                //item = MControl.Caching.Remote.RemoteCacheClient.Instance.FetchItem(destKey);
                //if (item == null)
                //{
                //    throw new MsgException(AckStatus.BadDestination, "No targets found in cache");
                //}
                //Hashtable h = (Hashtable)item.DeserializeValue();
                Hashtable h = ApiCache.GetAltDestination(destKey, campaignId, 0);// (Hashtable)RemoteSession.Instance(destKey).Get(ApiCache.ActiveDestination);

                if (h == null)
                {
                    throw new MsgException(AckStatus.FatalException, "RenderTargetsCampaign Error: Destination Not found, campaignId:" + destKey);
                }

                //MControl.Caching.Remote.RemoteCacheClient.Instance.RemoveItem(destKey);
                //begin_publishe = true;

                Guid publishKey = Counters.ParsePublishKey(destKey);

                using (CampaignPublisher publisher = new CampaignPublisher(campaignId, execType, userId, publishKey))
                {
                    CampaignPublisher.PublishState state = (CampaignPublisher.PublishState)publisher.Execute(h.Values);
                }
                Counters.PublishComment(destKey, campaignId, (int)AckStatus.Ok, AckStatus.Ok.ToString(), "Ok", ViewConfig.Server);
                return 0;
            }
            catch (MsgException mex)
            {
                Counters.PublishComment(destKey, campaignId, (int)mex.Status, mex.Status.ToString(), mex.Message, ViewConfig.Server);
                return -1;
            }
            catch (Exception ex)
            {
                Counters.PublishComment(destKey, campaignId, (int)AckStatus.UnExpectedError, AckStatus.UnExpectedError.ToString(), ex.Message, ViewConfig.Server);

                //if (begin_publishe)
                //{
                //    MControl.Caching.Remote.RemoteCacheClient.Instance.AddItem(item);
                //}
                Log.ErrorFormat("RenderTargets error: {0}", ex.Message);
                Log.ErrorFormat("RenderTargets errorTrace: {0}", ex.StackTrace);
                return -1;
            }
            finally
            {
                ApiCache.RemoveAltDestination(destKey);
                // RemoteSession.Instance(destKey).RemoveItem(ApiCache.ActiveDestination);
            }
        }
        */
        #endregion

        #region publish sync
        /*
        static object oLock = new object();

        public int PublishDestination(int campaignId, int execType, string sender, string coupon, string batchArgs, string destKey)
        {
            Log.DebugFormat("Remote ExecuteCampaign: {0}, DestKey:{1}", campaignId, destKey);

            Hashtable h = null;

            try
            {
                if (campaignId <= 0)
                {
                    throw new MsgException(AckStatus.ApplicationException, "Remote Error: Campaign Not found, campaignId:" + campaignId.ToString());
                }
                lock (oLock)
                {
                    h = ApiCache.GetAltDestination(destKey, campaignId, 0);

                    if (h == null || h.Count == 0)
                    {
                        throw new MsgException(AckStatus.FatalException, "Remote Error: Destination Not found, campaignId:" + destKey);
                    }

                    //MControl.Caching.Remote.RemoteCacheClient.Instance.RemoveItem(destKey);
                }

                int res = Execute(h.Values);

                Counters.PublishComment(destKey, campaignId, (int)AckStatus.Ok, AckStatus.Ok.ToString(), "Ok", ViewConfig.Server);

                return res;
            }
            catch (MsgException mex)
            {
                //UpdateBatchState(batchId, campaignId, ViewConfig.Server, mex.Status);
                Counters.PublishComment(destKey, campaignId, (int)mex.Status, mex.Status.ToString(), mex.Message, ViewConfig.Server);
                return -1;
            }
            catch (Exception ex)
            {
                Counters.PublishComment(destKey, campaignId, (int)AckStatus.UnExpectedError, AckStatus.UnExpectedError.ToString(), ex.Message, ViewConfig.Server);

                Log.ErrorFormat("Remote ExecuteCampaign error: {0}", ex.Message);
                Log.ErrorFormat("Remote ExecuteCampaign errorTrace: {0}", ex.StackTrace);
                //UpdateBatchState(batchId, campaignId, ViewConfig.Server, AckStatus.UnExpectedError);
                return -1;
            }
            finally
            {
                ApiCache.RemoveAltDestination(destKey);
                //MControl.Caching.Remote.RemoteSession.Instance(destKey).RemoveItem(ApiCache.ActiveDestination);
            }
        }
        */

        #endregion

    }
}
