using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Nistec;
using Nistec.Config;//.Configuration;
using Nistec.Generic;

namespace Netcell
{
    public static class ViewConfig
    {

        public const int ContactCapacity = 5000;
        public const int ContentCapacity = 500;

        public const string DefaultMailCharset = "windows-1255";

        #region Remote

        public static bool EnableRemote
        {
            get { return false;}// Types.ToBool(NetConfig.AppSettings["EnableRemote"], true); }
        }
        public static int MaxSyncPublish
        {
            get { return Types.ToInt(NetConfig.AppSettings["MaxSyncPublish"], 5000); }
        }
        #endregion

        #region sent const

        public const string SentID = "#id#";
        public const string PageID = "#pid#";
        public const string ShowUrl = "#showLinkClick#";
        public const string RemoveUrl = "#showRemoveClick#";

        #endregion

        #region methods
        public static string FormatConst(string s, string constatnt, string replacement)
        {
            return s.Replace(constatnt, replacement);
        }
        public static string FormatConst(string s, string constatnt, int replacement)
        {
            return s.Replace(constatnt, replacement.ToString());
        }
        public static string FormatSentID(string s, int sentId, int pageId)
        {
            return s.Replace(SentID, sentId.ToString()).Replace(PageID, pageId.ToString());
        }
        public static string FormatSentID(string s, int id)
        {
            return s.Replace(SentID, id.ToString());
        }
        public static string FormatShowUrl(string s, string showUrl, string removeUrl)
        {
            return s.Replace(ShowUrl, showUrl).Replace(RemoveUrl, removeUrl);
        }
        #endregion

        #region Consts
  
        public static string DefaultValidTimeBegin = "08:00";
        public static string DefaultValidTimeEnd = "22:00";
        public const string SwpLinkSample = "http://wap.my-t.co.il/wp.aspx?p=123456789";
        public const int LabOperator = 5;

        #endregion
 
        #region Domain


        public static int Server
        {
            get { return Types.ToInt(NetConfig.AppSettings["Server"], 0); }
        }

        public static bool EnableServerRules
        {
            get { return Types.ToBool(NetConfig.AppSettings["EnableServerRules"], false); }
        }
        
        public static string Domain
        {
            get { return NetConfig.AppSettings["Domain"]; }
        }
     
        #endregion

        #region Roots

        public static string ViewMobileRoot
        {
            get { return NetConfig.AppSettings["ViewMobileRoot"]; }
        }
       
        public static string ViewMailRoot
        {
            get { return NetConfig.AppSettings["ViewMailRoot"]; }
        }
        public static string ViewRoot
        {
            get { return NetConfig.AppSettings["ViewRoot"]; }
        }
        public static string EmailMobileRoot
        {
            get { return ViewMailRoot + "m/"; }
        }
        public static string ViewRespRoot
        {
            get { return NetConfig.AppSettings["ViewRespRoot"]; }
        }
        public static string TagViewRoot
        {
            get { return NetConfig.AppSettings["TagViewRoot"]; }
        }

        //public static string MailReportRoot
        //{
        //    get { return ViewMailRoot + "rpt/"; }
        //}

        public static string CmsSiteUrl
        {
            get { return TagViewRoot + "cms/"; }
        }
        public static string MobileSiteUrl
        {
            get { return ViewRoot + "mobi/"; }
        }
        #endregion

        #region Mobile View

        public static string MobileLinktUrl
        {
            get { return ViewMobileRoot + "link.aspx"; }
        }
        public static string MobileConfirmUrl
        {
            get { return ViewMobileRoot + "confirm/"; }
        }
        public static string MobileRemoveUrl
        {
            get { return ViewMobileRoot + "remove/"; }
        }
        public static string MobileCatalogUrl
        {
            get { return ViewRoot + "catalog/"; }
        }
        public static string MobileRemovePattern
        {
            get { return ViewMobileRoot + "remove/{0}.htm"; }//id
        }
        public static string MobileUrlPattern
        {
            get { return ViewRoot + "m/{0}.htm"; }//wap
        }
        public static string MobileThanksUrl
        {
            get { return ViewMobileRoot + "Thanks.aspx"; }
        }
        public static int MobileMaxWidth
        {
            get { return Types.ToInt(NetConfig.AppSettings["MobileMaxWidth"], 0); }
        }

        //public static string MobilePreviewUrlPattern
        //{
        //    get { return MobileRoot + "{0}.mpv"; }//i
        //}

        #endregion

        #region Mail View

        //public static string MailLinkUrl
        //{
        //    get { return ViewMailRoot + "link/"; }
        //}

        //public static string MailLinkPattern
        //{
        //    get { return ViewMailRoot + "link/{0}/{1}/{2}.htm"; }//?id={0}&linkId={1}&href={2}
        //}

        public static string MailLinkUrl
        {
            get { return ViewMailRoot + "link.aspx"; }
        }

        public static string MailConfirmPattern
        {
            get { return ViewMailRoot + "confirm/{0}/{1}/{2}.htm"; }//?id={0}&linkId={1}&a=[status={2}&designId={3}]
        }

        public static string MailQuizPattern
        {
            get { return ViewMailRoot + "quiz/{0}.htm"; }//id
        }

        public static string MailRemovePattern
        {
            get { return ViewMailRoot + "remove/{0}.htm"; }//id
        }

        public static string MailShowPattern
        {
            get { return ViewMailRoot + "show/{0}.htm"; }//id
        }
        public static string MailImageFooterPattern
        {
            get { return ViewMailRoot + "footer/{0}.gif"; }//id
        }

        public static string MailCampaignStatisticPattern
        {
            get { return ViewRespRoot + "csum/{0}.htm"; }//id
        }

        #endregion

        #region EMobileRoot

        public static string EMobileRemoveUrl
        {
            get { return EmailMobileRoot + "remove/"; }
        }

        public static string EMobileShowPatternRelative
        {
            get { return "~/em/show/{0}.htm"; }
        }
        public static string EMobileRemovePatternRelative
        {
            get { return "~/em/remove/{0}.htm"; }//id
        }

        public static string EMobileShowPattern
        {
            get { return EmailMobileRoot + "show/{0}.htm"; }
        }

        public static string EMobileRemovePattern
        {
            get { return EmailMobileRoot + "remove/{0}.htm"; }//id
        }


        #endregion

        #region Common
        //& &amp; 
        //< &lt; 
        //> &gt; 
        //" &quot; 
        //' &apos; 

        public static string IntegrationWsUrl
        {
            get { return NetConfig.AppSettings["IntegrationWsUrl"]; }
        }

        public static string BaseContentPath
        {
            get { return NetConfig.AppSettings["BaseContentPath"]; }
        }
         public static string UploadPath
        {
            get { return NetConfig.AppSettings["UploadPath"]; }
        }
        public static string UploadVirtualPath
        {
            get { return NetConfig.AppSettings["UploadVirtualPath"]; }
        }
        public static string UploadVirtualLocalPath
        {
            get { return NetConfig.AppSettings["UploadVirtualLocalPath"]; }
        }
       
        public static string ViewErrPageUrl
        {
            get { return NetConfig.AppSettings["ViewErrPageUrl"]; }
        }
        public static string ViewEphoneUrl
        {
            get { return NetConfig.AppSettings["ViewEphoneUrl"]; }
        }

        public static int MaxBatchItems
        {
            get { return Types.ToInt(NetConfig.AppSettings["MaxBatchItems"], 10000); }
        }
        public static string TemplatesVirtualLocalPath
        {
            get { return NetConfig.AppSettings["TemplatesVirtualLocalPath"]; }
        }
        #endregion
       
        #region Smtp / Mailer

        public static MailerProxyMode MailerProxyMode
        {
            get { return (MailerProxyMode)Types.ToInt(NetConfig.AppSettings["MailerProxyMode"], 0); }
        }
        
        public static string SmtpClient
        {
            get { return NetConfig.AppSettings["SmtpClient"]; }
        }

        public static string MailSentBy
        {
            get { return NetConfig.AppSettings["ViewMailSentBy"]; }
        }
        public static bool MailEnableImageSize
        {
            get { return Types.ToBool( NetConfig.AppSettings["ViewMailEnableImageSize"],false); }
        }
        
        public static string MailRemoveText
        {
            get { return NetConfig.AppSettings["ViewMailRemoveText"]; }
        }
        public static string MailSender
        {
            get { return NetConfig.AppSettings["ViewMailSender"]; }
        }
        //public static string MailDisplayName
        //{
        //    get { return NetConfig.AppSettings["ViewMailDisplayName"]; }
        //}
        public static string MailHost
        {
            get { return NetConfig.AppSettings["ViewMailHost"]; }
        }
        public static string MailImageFooter
        {
            get { return NetConfig.AppSettings["ViewMailImageFooter"]; }
        }
        public static string MailCssLink
        {
            get { return NetConfig.AppSettings["ViewMailCssLink"]; }
        }
        public static bool ViewMailTarget
        {
            get { return Types.ToBool(NetConfig.AppSettings["ViewMailTarget"], false); }
        }
        #endregion

        #region Netcell Format

        public static string WapUploadPathFormat(int accountId)
        {
            return UploadPath + accountId.ToString() + "\\Wap\\";
        }

        public static string WapVirtualPathFormat(int accountId)
        {
            return UploadVirtualPath + accountId.ToString() + "/Wap/";
        }

        //public static string DefaultSmsSender
        //{
        //    get { return "0527464292"; }
        //}
        //public static string NetcellAuoth
        //{
        //    get { return Instance["NetcellAuoth"]; }
        //}
        //public static string NetcellUrl
        //{
        //    get { return Instance["NetcellUrl"]; }
        //}

        #endregion

        #region System


        public static bool EnableMultipleQueue
        {
            get { return Types.ToBool(NetConfig.AppSettings["EnableMultipleQueue"], false); }
        }

        public static int MinQueuesCount
        {
            get { return Types.ToInt(NetConfig.AppSettings["MinQueuesCount"], 30); }
        }

        public static int SystemAccount
        {
            get { return Types.ToInt(NetConfig.AppSettings["SystemAccount"], 1); }
        }
        public static string SystemCli
        {
            get { return NetConfig.AppSettings["SystemCli"]; }
        }
        public static string SystemMail
        {
            get { return NetConfig.AppSettings["SystemMail"]; }
        }
        public static string SystemSender
        {
            get { return NetConfig.AppSettings["SystemSender"]; }
        }
        public static string CreditMessage
        {
            get { return NetConfig.AppSettings["CreditMessage"]; }
        }
        public static string SystemSmsUrl
        {
            get { return NetConfig.AppSettings["SystemSmsUrl"]; }
        }

        public static string SystemMailUser
        {
            get { return NetConfig.AppSettings["SystemMailUser"]; }
        }
        public static string SystemMailPass
        {
            get { return NetConfig.AppSettings["SystemMailPass"]; }
        }
        public static string SystemMailHost
        {
            get { return NetConfig.AppSettings["SystemMailHost"]; }
        }
        public static string SystemMailDisplayName
        {
            get { return NetConfig.AppSettings["SystemMailDisplayName"]; }
        }
        public static string SystemMailFrom
        {
            get { return NetConfig.AppSettings["SystemMailFrom"]; }
        }

        public static string SystemAlarmUrl
        {
            get { return NetConfig.AppSettings["SystemAlarmUrl"]; }
        }
        #endregion

        #region Mail Server

        public static string Mail_server
        {
            get { return NetConfig.AppSettings["Mail_server"]; }
        }
        public static int Mail_port
        {
            get { return Types.ToInt(NetConfig.AppSettings["Mail_port"], 2525); }
        }
        public static string Mail_user
        {
            get { return NetConfig.AppSettings["Mail_user"]; }
        }
        public static string Mail_pwd
        {
            get { return NetConfig.AppSettings["Mail_pwd"]; }
        }
        public static string Mail_ReturnPath
        {
            get { return NetConfig.AppSettings["Mail_ReturnPath"]; }
        }
        public static string Mail_NotifyUrl
        {
            get { return NetConfig.AppSettings["Mail_NotifyUrl"]; }
        }
        public static string Mail_TransferEncoding
        {
            get { return NetConfig.AppSettings["Mail_TransferEncoding"]; }
        }
        //public static int MinChunkItems
        //{
        //    get { return Types.ToInt(NetConfig.AppSettings["MinChunkItems"], 1); }
        //}
        public static int MaxItemsPerChunk
        {
            get { return Types.ToInt(NetConfig.AppSettings["MaxItemsPerChunk"], 10); }
        }
        public static int Mail_MaxQuickItems
        {
            get { return Types.ToInt(NetConfig.AppSettings["Mail_MaxQuickItems"], 10); }
        }
        public static string Mail_Charset
        {
            get { return NetConfig.AppSettings["Mail_Charset"]; }
        }
        #endregion

        #region Netcell Controller

        public static string Queue_Bulk
        {
            get { return NetConfig.AppSettings["Queue_Bulk"]; }
        }
        public static string Queue_MO
        {
            get { return NetConfig.AppSettings["Queue_MO"]; }
        }
        public static string Queue_RB
        {
            get { return NetConfig.AppSettings["Queue_RB"]; }
        }

        //public static string App_Servers
        //{
        //    get { return NetConfig.AppSettings["App_Servers"]; }
        //}

        #endregion

        #region Netcell Sender

        public static int Sender_MaxConnection
        {
            get { return Types.ToInt(NetConfig.AppSettings["Sender_MaxConnection"], 30); }
        }
        public static int Sender_MaxCapacity
        {
            get { return Types.ToInt(NetConfig.AppSettings["Sender_MaxCapacity"], 50000); }
        }
        //public static int Sender_Default
        //{
        //    get { return Types.ToInt(NetConfig.AppSettings["Sender_Default"], 0); }
        //}
        //public static string Sender_DefaultQueue
        //{
        //    get { return NetConfig.AppSettings["Sender_DefaultQueue"]; }
        //}
        #endregion

        #region View

        public static string PowerdByLink
        {
            get { return string.Format("<p class=\"powerd-by\">{0}</p>", ViewConfig.MailSentBy); }
        }


        #endregion


    }
}
