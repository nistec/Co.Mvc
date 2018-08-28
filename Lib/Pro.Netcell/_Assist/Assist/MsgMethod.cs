using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Nistec.Generic;
using Nistec;

namespace Netcell
{
    //public class MediaTypes
    //{
    //    public const string BIN = "BIN";
    //    public const string SMS = "SMS";
    //    public const string SWP = "SWP";
    //    public const string WAP = "WAP";
    //    public const string MMS = "MMS";
    //    public const string MAL = "MAL";
    //}

    public enum NotifyActionType
    {
        Begin,
        End,
        Statistic
    }

    public class NotifyTemplateTypes
    {
        public const int MailNotifyMessage = 1;
        public const int CellNotifyMessage = 2;
        
        public const int CampaignCellNotifyBegin = 3;
        public const int CampaignCellNotifyEnd = 4;
        public const int CampaignMailNotifyBegin = 5;
        public const int CampaignMailNotifyEnd = 6;

        //public const int CampaignSmsNotifyBegin = 5;
        //public const int CampaignSmsNotifyEnd = 6;

        //public static NotifyActionType GetNotifyActionType()
        //{

        //}

        public static int GetCampaignNotifyTemplateId(PlatformType pmt, NotifyActionType actionMode)
        {
   
            switch (pmt)
            {
                case PlatformType.Mail:
                    return actionMode == NotifyActionType.Begin ? CampaignMailNotifyBegin : actionMode == NotifyActionType.End ? CampaignMailNotifyEnd : MailNotifyMessage;
                case PlatformType.Cell:
                    return actionMode == NotifyActionType.Begin ? CampaignCellNotifyBegin : actionMode == NotifyActionType.End ? CampaignCellNotifyEnd : CellNotifyMessage;
            }
            return 0;
        }

        public static PlatformType GetCampaignNotifyPlatform(string target)
        {

            if (target == null)
                return PlatformType.NA;
            if (target.Contains('@'))
                return PlatformType.Mail;
            return PlatformType.Cell;
        }

    }

    public class QueueLabel
    {
        public const string CB = "CB";
        public const string RB = "RB";
        public const string CB_SMSMT_TEXT = "CB|SMSMT|TEXT";
        public const string CB_SMSEX_TEXT = "CB|SMSEX|TEXT";
        public const string CB_SMSHL_TEXT = "CB|SMSHL|TEXT";
        public const string CB_SMSWP_TEXT = "CB|SMSWP|TEXT";
        public const string CB_MALMT_HTML = "CB|MALMT|HTML";
        public const string CB_MALWP_HTML = "CB|MALWP|HTML";

       
        public static void ParseLabel(string label,out BillingType bt,out MethodType mt,out BodyFormat bf)
        {
           switch(label.ToUpper())
           {
               case "CB|SMSMT|TEXT":
                   bt = BillingType.CB;
                   mt = MethodType.SMSMT;
                   bf = BodyFormat.Text;
                   break;
               case "CB|SMSEX|TEXT":
                   bt = BillingType.CB;
                   mt = MethodType.SMSEX;
                   bf = BodyFormat.Text;
                   break;
               case "CB|SMSHL|TEXT":
                   bt = BillingType.CB;
                   mt = MethodType.SMSHL;
                   bf = BodyFormat.Text;
                   break;
               case "CB|SMSWP|TEXT":
                   bt = BillingType.CB;
                   mt = MethodType.SMSWP;
                   bf = BodyFormat.Text;
                   break;
               case "CB|MALMT|HTML":
                   bt = BillingType.CB;
                   mt = MethodType.MALMT;
                   bf = BodyFormat.Html;
                   break;
               case "CB|MALWP|HTML":
                   bt = BillingType.CB;
                   mt = MethodType.MALWP;
                   bf = BodyFormat.Html;
                   break;
               case "CB":
                   bt = BillingType.CB;
                   mt = MethodType.SMSMT;
                   bf = BodyFormat.Xml;
                   break;
               case "RB":
                   bt = BillingType.RB;
                   mt = MethodType.SMSMT;
                   bf = BodyFormat.Xml;
                   break;
               default:
                   throw new NetException(AckStatus.NotSupportedException, "QueueLabel not supported " + label);
           }
        }
    }

    public enum BodyFormat
    {
        Xml,
        Text,
        Html
    }

    public enum MethodType
    {
        NA=0,
        SMSMT = 1,
        SMSEX = 2,
        SMSHL = 3,

        //WAPMT = 2,
        //MMSMT = 3,
        //BINMT = 4,
        MALMT = 5,
        //SMSPS = 6,
        SMSWP = 7,
        //IMOBI = 8,
        //IMINI = 9,
        //IMBLG = 10,
        //RENTM = 11,
        SMSMO=12,
        MALWP=13
    }

    //public enum MediaType
    //{
    //    NA = 0,
    //    SMS = 1,
    //    WAP = 2,
    //    MMS = 3,
    //    BIN = 4,
    //    MAL = 5,
    //    SWP = 7,
    //}

    public class MsgMethod
    {
        public const int MaxSmsMessage = 800;

  

        public static MethodCategory ToMethodCategory(MethodType mt)
        {

            if (IsMail(mt))
                return MethodCategory.MAL;
            if (IsSwp(mt) || IsHl(mt))
                return MethodCategory.SWP;
            if (IsSms(mt,false))
                return MethodCategory.SMS;

            return MethodCategory.NA;
        }

       

        public static MethodType GetBillingMethod(MethodType method)
        {
            return ((method == MethodType.SMSWP || method == MethodType.SMSHL) ? MethodType.SMSMT : method);
        }

        public static int GetBillingMethod(int method)
        {
            return ((method == (int)MethodType.SMSWP || method == (int)MethodType.SMSHL) ? 1 : method);
        }

        public static string GetBillingMethod(string method)
        {
            return ((method == "SMSWP" || method == "SMSHL") ? "SMSMT" : method);
        }

        public static int GetMethodRule(string method)
        {
            switch (method)
            {
                case "ALL":
                    return 0;

                case "SMSMT":
                case "SMSPS":
                case "SMSWP":
                case "SMSHL":
                    return 1;

                case "SMSEX":
                    return 2;

                case "MMSMT":
                    return 3;

                case "BINMT":
                    return 4;
            }
            return 0;
        }
        
        public static PreviewMode GetPreviewMode(MethodType method)
        {
            return  MsgMethod.IsSms(method,false) ? PreviewMode.Sms : PreviewMode.Html;
        }
        public static PreviewMode GetPreviewMode(string method)
        {
            return MsgMethod.IsSms(method,false) ? PreviewMode.Sms : PreviewMode.Html;
        }
        public static PlatformType GetPlatform(MethodCategory category)
        {
            return (category== MethodCategory.MAL ? PlatformType.Mail : PlatformType.Cell);
        }

        public static PlatformType GetPlatform(MethodType method)
        {
            return (IsMail(method) ? PlatformType.Mail : PlatformType.Cell);
        }

        public static PlatformType GetPlatform(int method)
        {
            return (IsMail((MethodType)method) ? PlatformType.Mail : PlatformType.Cell);
        }

        public static PlatformType GetPlatform(string method)
        {
            return (IsMail(method) ? PlatformType.Mail : PlatformType.Cell);
        }

        public static PlatformType ResolvePlatform(string method)
        {
            return (IsMail(Resolve(method)) ? PlatformType.Mail : PlatformType.Cell);
        }

        public static PlatformType ParsePlatform(string platform)
        {
           return Nistec.Generic.EnumExtension.Parse<PlatformType>(platform, PlatformType.NA);
        }

  

        public static TransModule GetTransModule(MethodType method, bool isTrans)
        {
            if (isTrans)
                return TransModule.TransCB;
            return (IsMail(method) ? TransModule.CampaignMail : TransModule.CmapaignCell);
        }

        public static bool IsCell(MethodType method)
        {
            return (method == MethodType.SMSMT || method == MethodType.SMSWP);
        }

        public static bool IsMail(MethodType method, bool enableMwp = true)
        {
            if (enableMwp)
                return (method == MethodType.MALMT || method == MethodType.MALWP);
            return (method == MethodType.MALMT);
        }
        public static bool IsMail(string method, bool enableMwp = true)
        {
            return IsMail(EnumExtension.Parse<MethodType>(method, MethodType.NA), enableMwp);
        }
        //public static bool IsMal(MethodType method, bool enableMwp=false)
        //{
        //    return (method == MethodType.MALMT);
        //}
        //public static bool IsMal(string method, bool enableMwp = false)
        //{
        //    return IsMal(EnumExtension.Parse<MethodType>(method, MethodType.NA), enableMwp);
        //}
        public static bool IsMwp(MethodType method)
        {
            return (method == MethodType.MALWP);
        }

        public static bool IsMwp(string method)
        {
            return IsMwp(EnumExtension.Parse<MethodType>(method, MethodType.NA));
        }

 
        public static bool IsMMS(int method)
        {
            return (method == 3);
        }

        public static bool IsMO(MethodType method)
        {
            return (method == MethodType.SMSMO);
        }

        public static bool IsMO(string method)
        {
            switch (method)
            {
                case "MO":
                case "MB":
                case "SMSMO":
                //case "SMSMB":
                //case "BINMB":
                //case "WAPMB":
                //case "MMSMB":
                    return true;
            }
            return false;
        }

        //public static bool IsSmsCategory(MethodType method)
        //{
        //    switch (method)
        //    {
        //        case MethodType.SMSMT:
        //        case MethodType.SMSWP:
        //        case MethodType.SMSPS:
        //            return true;
        //    }
        //    return false;
        //}
        //public static bool IsSmsCategory(string method)
        //{
        //    return IsSmsCategory(EnumExtension.Parse<MethodType>(method, MethodType.NA));
        //}

        public static bool IsSms(MethodType method, bool enableSwp)
        {
            if (enableSwp)
                return (method == MethodType.SMSMT || method == MethodType.SMSWP || method == MethodType.SMSEX || method == MethodType.SMSHL);
            return method == MethodType.SMSMT ;
        }
        public static bool IsSms(string method, bool enableSwp)
        {
            return IsSms(EnumExtension.Parse<MethodType>(method, MethodType.NA), enableSwp);
        }
        public static string ValidteSms(string method)
        {
            if (method == null)
                return MethodTypes.SMSMT;
            method = method.ToUpper();
            if (method == MethodTypes.SMSMT || method == MethodTypes.SMSWP || method == MethodTypes.SMSEX || method == MethodTypes.SMSHL)
                return method;
            else
                return MethodTypes.SMSMT;
        }
        public static string ValidteMail(string method)
        {
            if (method == null)
                return MethodTypes.MALMT;
            method = method.ToUpper();
            if (method == MethodTypes.MALMT || method == MethodTypes.MALWP)
                return method;
            else
                return MethodTypes.MALMT;
        }
        public static bool IsHl(MethodType method)
        {
            return method == MethodType.SMSHL;
        }
        public static bool IsSwp(MethodType method)
        {
            return method == MethodType.SMSWP;
        }
        public static bool IsSwp(string method)
        {
            return IsSwp(EnumExtension.Parse<MethodType>(method, MethodType.NA));
        }

        
        public static string MethodDir(string method)
        {
            return (IsMO(method) ? "MO" : "MT");
        }
        public static string MethodDir(MethodType method)
        {
            return (IsMO(method) ? "MO" : "MT");
        }
        public static string Resolve(string method)
        {
            if ( method == null)
            {
                method = "";
            }
            //method = method.ToUpper();
            switch (method)
            {
                case "":
                case "NA":
                case "ALL":
                    return "NA";

                case "MO":
                    return "SMSMO";

                case "SMS":
                    return "SMSMT";

                case "Cell":
                case "SWP":
                    return "SMSWP";

                //case "WAP":
                //    return "WAPMT";

                case "Mail":
                case "MAL":
                    return "MALMT";

                //case "MMS":
                //    return "MMSMT";

                //case "BIN":
                //    return "BINMT";
            }
            if (Types.IsNumber(method))
                return ToString(Types.ToInt(method));
            return method.ToUpperInvariant();
        }

        public static BillingType ResolveBillingType(BillingType billing)
        {
            if (billing == BillingType.NA)
            {
                return BillingType.CB;
            }
            return billing;
        }

        public static BillingType ToBillingType(string billing)
        {
            if (string.IsNullOrEmpty(billing))
            {
                return BillingType.CB;
            }
            return (BillingType)Enum.Parse(typeof(BillingType), billing, true);
        }

        public static MethodType KnownSendTypes(string  method)
        {
       
            if ( method == null)
            {
                method = "";
            }
            method = method.ToUpper();
            switch (method)
            {
                case "SMSMT":
                    return MethodType.SMSMT;
                case "SMSEX":
                    return MethodType.SMSEX;
                case "SMSHL":
                    return MethodType.SMSHL;
                case "SMSWP":
                    return MethodType.SMSWP;
                case "MALMT":
                    return MethodType.MALMT;
                case "MALWP":
                    return MethodType.MALWP;
                default:
                    return MethodType.NA;
            }
        }

        public static MethodType ToMethodType(int method)
        {
            return (MethodType)method;
        }
        
        static MethodType ParseMethodType(string method)
        {
            return (MethodType)Types.ParseEnum(typeof(MethodType), method, (int)MethodType.NA);
        }

        public static MethodType ToMethodType(string method)
        {
            return ToMethodType(method,true);
        }
        public static MethodType ToMethodType(string method, bool resolve)
        {
            if (resolve)
            {
                method = Resolve(method);
            }
            //return (MethodType)Enum.Parse(typeof(MethodType), method, true);
            return ParseMethodType(method);
        }

        public static string ToString(int method)
        {
            return ((MethodType)method).ToString();
        }

        // Properties
        //private static int[] SmsRange
        //{
        //    get
        //    {
        //        return new int[] { 1, 6 };
        //    }
        //}

        //private static int[] SmsWpRange
        //{
        //    get
        //    {
        //        return new int[] { 1, 6, 7 };
        //    }
        //}

        //private static int[] WapRange
        //{
        //    get
        //    {
        //        return new int[] { 2, 7 };
        //    }
        //}

        // Nested Types
        private class MethodTypes
        {
            // Fields
            //public const string BINMT = "BINMT";
            public const string MALMT = "MALMT";
            public const string MALWP = "MALWP";
            public const string MMSMT = "MMSMT";
            public const string NA = "NA";
            public const string SMSMO = "SMSMO";
            public const string SMSMT = "SMSMT";
            public const string SMSWP = "SMSWP";
            public const string SMSEX = "SMSEX";
            public const string SMSHL = "SMSHL";
            //public const string SMSPS = "SMSPS";
            //public const string WAPMT = "WAPMT";
        }
    }

}
