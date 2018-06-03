using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using Netcell.Lib;
using Netcell.Remoting;
using Netcell.Web;
using Netcell.Data.Entities;
using Netcell.Data;
using Netcell;

namespace Netcell.Lib.View
{

    public struct RequestMailDataShow
    {
        public int Id;
        public int SrcId;
        public int Version;
        public int EnableMobile;//0=no;1=yes
        public int EnableTransfer;//0=no;1=yes
        public bool IsMobile;
        public string Browser;
        public string Personal;
        public string Barcode;
        public string Target;
        //public int Version;//0=messageid,1=sentid,2=transid,3=fullArgs
        public bool IsPreview;

        public static string CreateEncParam(int id, int SrcId, int version, bool enableMobile, bool enableTransfer, string target, string personal, string barcode)
        {
            string strid = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", VersionUtil.GetIdArg(id), VersionUtil.GetBatchArg(SrcId), VersionUtil.GetVersionArg(version), VersionUtil.GetTargetArg(target), VersionUtil.GetPersonalArg(personal), VersionUtil.GetMobileArg(enableMobile), VersionUtil.GetTransferArg(enableTransfer), VersionUtil.GetBarcodeArg(barcode));
            return MControl.Sys.RquestQuery.EncryptEx32(strid);
        }

        public static string CreateShowUrl(int id, int BatchId, bool enableMobile, bool enableTransfer, string target, string personal, string barcode, bool isDemo)
        {
            int version = (int)VersionUtil.VersionView;//.GetShowVersion(id, campaignId, target, isDemo);
            string strid = CreateEncParam(id, BatchId, version, enableMobile, enableTransfer, target, personal, barcode);
            return string.Format(ViewConfig.MailShowPattern, strid);
        }

        #region mobile

        //public bool ShouldRedirectToMobile
        //{
        //    get { return IsMobile && EnableMobile > 0; }
        //}

        //public string MobileUrl
        //{
        //    get { return string.Format(ViewConfig.EMobileShowPattern, Id, SrcId); }
        //}
        //public string MobileUrlRelative
        //{
        //    get { return string.Format(ViewConfig.EMobileShowPatternRelative, Id, SrcId); }
        //}
        public bool HasTarget
        {
            get { return !string.IsNullOrEmpty(Target); }
        }
        #endregion

        public RequestMailDataShow(System.Web.HttpRequest request)
        {
            Id = 0;
            SrcId = 0;
            EnableMobile = 0;
            EnableTransfer = 0;
            IsMobile = false;
            Browser = "NA";
            Personal = "";
            Barcode = "";
            Target = "";
            Version = 0;//VersionUtil.VersionView;
            IsPreview = false;
            //pageType = MailPageType.NA;

            string strid = request.QueryString[0];//["id"];
            ParseRequestMailShow(strid);
            IsMobile = Netcell.Web.BrowserHelper.GetBrowser(request, out Browser) == BrowserType.Mobile;
        }

        public RequestMailDataShow(string strid, bool isMobile)
        {
            Id = 0;
            SrcId = 0;
            EnableMobile = 0;
            EnableTransfer = 0;
            IsMobile = false;
            Browser = "NA";
            Personal = "";
            Barcode = "";
            Target = "";
            Version = 0;//VersionUtil.VersionView;
            IsPreview = false;
            //pageType = MailPageType.NA;

            ParseRequestMailShow(strid);
            IsMobile = isMobile;
        }

        void ParseRequestMailShow(string strid)
        {
            //Id = 0;
            //CampaignId = 0;
            //EnableMobile = 0;
            //EnableTransfer = 0;
            //IsMobile = false;
            //Browser = "NA";
            //Personal = "";
            //Barcode = "";
            //Target = "";
            //Version =VersionUtil.VersionView;
            //IsPreview = false;


            try
            {

                //string strid = request.QueryString[0];//["id"];

                if (string.IsNullOrEmpty(strid))
                {
                    throw new Exception("Invalid RequestMailBatchParam");
                }

                if (int.TryParse(strid, out Id))
                {
                    Version = VersionUtil.VersionView;//.MessageId;
                }
                else
                {

                    //Netcell.Log.DebugFormat("RequestMailShow qs:{0}", strid);
                    string encid = null;
                    try
                    {
                        if (strid.StartsWith("0X"))
                            encid = MControl.Sys.RquestQuery.DecryptEx32(strid);
                        else
                            encid = MControl.Sys.RquestQuery.DecryptQueryOptional(strid);
                    }
                    catch
                    {
                        throw new MsgException(AckStatus.WebException, "RequestMailBatchShow DecryptEx32 error:" + strid);
                    }
                    //id = Types.ToInt(encid, 0);
                    if (!int.TryParse(encid, out Id))
                    {
                        CommandArgs args = CommandArgs.ParseQueryString(encid);

                        Version = Types.ToInt(args[VersionUtil.TagVersion], 0);

                        if (args.ContainsKey("id"))
                        {
                            //"id={0}&cid={1}&acc={2}&v={3}&em={4}&et={5}"
                            Id = Types.ToInt(args.Get("id"), 0);
                            SrcId = Types.ToInt(args.Get("cid"), 0);
                            Version = Types.ToInt(args.Get("v"), 0);
                            // Version = VersionUtil.VersionView;//.FullArgs;
                        }
                        else
                        {
                            Id = Types.ToInt(args.Get(VersionUtil.TagId), 0);

                            SrcId = Types.ToInt(args.Get(VersionUtil.TagCampaignId), 0);
                            Version = Types.ToInt(args.Get(VersionUtil.TagVersion), 0);
                            Target = args.Get(VersionUtil.TagTarget);
                            Personal = args.Get(VersionUtil.TagPersonal);
                            Barcode = args.Get(VersionUtil.TagBarcode);
                            IsPreview = Version == VersionUtil.VersionPreview;
                            EnableMobile = Types.ToInt(args.Get(VersionUtil.TagEnableMobile), 0);
                            EnableTransfer = Types.ToInt(args.Get(VersionUtil.TagEnableTransfer), 0);
                        }
                        //VersionUtil.ParseVersion(Version, out EnableMobile, out EnableTransfer, out IsDemo);

                    }

                }

                if (Id <= 0)
                {
                    throw new Exception("Invalid mail Batch item:" + strid);
                }

                //IsMobile = request.Browser.IsMobileDevice;

                //IsMobile = Netcell.Web.BrowserHelper.GetBrowser(request, out Browser) == BrowserType.Mobile;



            }
            catch (Exception ex)
            {
                Netcell.Log.ErrorFormat("RequestMailBatchParam Error:{0}", ex.Message);
                throw ex;
            }
        }

        public string Print()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            sb.AppendFormat("Id {0}\rn", Id);
            sb.AppendFormat("SrcId {0}\rn", SrcId);
            sb.AppendFormat("EnableMobile {0}\rn", EnableMobile);
            sb.AppendFormat("EnableTransfer {0}\rn", EnableTransfer);
            sb.AppendFormat("IsMobile {0}\rn", IsMobile);
            sb.AppendFormat("Browser {0}\rn", Browser);
            sb.AppendFormat("Personal {0}\rn", Personal);
            sb.AppendFormat("Barcode {0}\rn", Barcode);
            sb.AppendFormat("Target {0}\rn", Target);
            sb.AppendFormat("Version {0}\rn", Version);
            sb.AppendFormat("IsPreview {0}\rn", IsPreview);
            return sb.ToString();
        }

        //public string RenderView()
        //{
        //    MailView mailItem = new MailView(CampaignId);
        //    string html = mailItem.ExecuteShow(this);
        //    return html;
        //}
    }
   
    public class MailDataViewer : DataViewer
    {

        //public static DataViewer Get(int messageId, int version, IDeviceInfo device)
        //{
        //    using (DataViewer_Context view = new DataViewer_Context(messageId, 0, (int)PlatformType.Mail, version, device.IsMobileDevice, device.DeviceId))
        //    {
        //        if (view.IsEmpty)
        //        {
        //            throw new MsgException(AckStatus.MobileException, string.Format("MailDataViewer.Invalid data for : messageId:{0}, BatchId:{1}, platform:{2}, version:{3}, IsMobile:{4}, DeviceNam:{5}", messageId, 0, PlatformType.Mail, version, device.IsMobileDevice, device.DeviceId));
        //        }
        //        return view.Entity;
        //    }
        //}

        //public static DataViewer Get(int messageId, int BatchId, PlatformType platform, bool isPreview, IDeviceInfo device)
        //{
        //    using (DataViewer_Context view = new DataViewer_Context(messageId, BatchId, 1, (int)platform, isPreview, device.IsMobileDevice, device.DeviceId))
        //    {
        //        if (view.IsEmpty)
        //        {
        //            throw new MsgException(AckStatus.MobileException, string.Format("MailDataViewer.Invalid data for : messageId:{0}, BatchId:{1}, platform:{2}, isPreview:{3}, IsMobile:{4}, DeviceNam:{5}", messageId, BatchId, platform, isPreview, device.IsMobileDevice, device.DeviceId));
        //        }
        //        return view.Entity;
        //    }
        //}

        //public static DataViewer Get(int messageId, int BatchId, PlatformType platform, bool isPreview, bool IsMobile, string DeviceName)
        //{
        //    using (DataViewer_Context view = new DataViewer_Context(messageId, BatchId,1, (int)platform, isPreview, IsMobile, DeviceName))
        //    {
        //        if (view.IsEmpty)
        //        {
        //            throw new MsgException(AckStatus.MobileException, string.Format("MailDataViewer.Invalid data for : messageId:{0}, BatchId:{1}, platform:{2}, isPreview:{3}, IsMobile:{4}, DeviceNam:{5}", messageId, BatchId, platform, isPreview, IsMobile, DeviceName));
        //        }
        //        return view.Entity;
        //    }
        //}

        public static DataViewer Get(int messageId, int SrcId, int Version, PlatformType platform, bool isPreview, bool IsMobile, string DeviceName)
        {
            using (DataViewer_Context view = new DataViewer_Context(messageId, SrcId, Version, (int)platform, isPreview, IsMobile, DeviceName))
            {
                if (view.IsEmpty)
                {
                    throw new MsgException(AckStatus.MobileException, string.Format("MailDataViewer.Invalid data for : messageId:{0}, SrcId:{1}, Version:{2}, platform:{3}, isPreview:{4}, IsMobile:{5}, DeviceNam:{6}", messageId, SrcId, Version, platform, isPreview, IsMobile, DeviceName));
                }
                return view.Entity;
            }
        }
        #region static

        public static DataViewer Instance(System.Web.HttpRequest request)//, bool isMobileDevice)
        {
            int Id = 0;
            int SrcId = 0;
            int EnableMobile = 0;
            int EnableTransfer = 0;
            bool IsMobile = false;
            string Browser = "NA";
            string Personal = "";
            string Barcode = "";
            string Target = "";
            int Version = 0;
            //int Version = VersionUtil.VersionView;
            bool IsPreview = false;
            //pageType = MailPageType.NA;

            try
            {

                string strid = request.QueryString[0];//["id"];

                if (string.IsNullOrEmpty(strid))
                {
                    throw new Exception("Invalid RequestMailBatchParam");
                }

                if (int.TryParse(strid, out Id))
                {
                    Version = VersionUtil.VersionView;
                }
                else
                {


                    string encid = MControl.Sys.RquestQuery.DecryptEx32(strid);
                    //id = Types.ToInt(encid, 0);
                    if (!int.TryParse(encid, out Id))
                    {
                        CommandArgs args = CommandArgs.ParseQueryString(encid);

                        Version = Types.ToInt(args[VersionUtil.TagVersion], 0);

                        if (args.ContainsKey("id"))
                        {
                            //"id={0}&cid={1}&acc={2}&v={3}&em={4}&et={5}"
                            Id = Types.ToInt(args.Get("id"), 0);
                            SrcId = Types.ToInt(args.Get("cid"), 0);
                            Version = Types.ToInt(args.Get(VersionUtil.TagVersion), 0);
                            //Version = VersionUtil.VersionView;
                        }
                        else
                        {
                            Id = Types.ToInt(args.Get(VersionUtil.TagId), 0);

                            SrcId = Types.ToInt(args.Get(VersionUtil.TagCampaignId), 0);
                            Version = Types.ToInt(args.Get(VersionUtil.TagVersion), 0);
                            Target = args.Get(VersionUtil.TagTarget);
                            Personal = args.Get(VersionUtil.TagPersonal);
                            Barcode = args.Get(VersionUtil.TagBarcode);
                            IsPreview = Version == VersionUtil.VersionPreview;
                            EnableMobile = Types.ToInt(args.Get(VersionUtil.TagEnableMobile), 0);
                            EnableTransfer = Types.ToInt(args.Get(VersionUtil.TagEnableTransfer), 0);
                        }
                    }

                }

                if (Id <= 0)
                {
                    throw new Exception("Invalid mail Batch item:" + strid);
                }

                //IsMobile = request.Browser.IsMobileDevice;

                IsMobile = Netcell.Web.BrowserHelper.GetBrowser(request, out Browser) == BrowserType.Mobile;

                //return new MailViewer(Id, CampaignId, PlatformType.Mail, Version, IsMobile, Browser);

                return MailDataViewer.Get(Id, SrcId, Version, PlatformType.Mail, IsPreview, IsMobile, Browser);

            }
            catch (Exception ex)
            {
                Netcell.Log.ErrorFormat("RequestMailBatchParam Error:{0}", ex.Message);
                throw ex;
            }
        }

        #endregion


        public static string RenderView(RequestMailDataShow rp)
        {
            DataViewer cv = MailDataViewer.Get(rp.Id, rp.SrcId, rp.Version, PlatformType.Mail, rp.IsPreview, rp.IsMobile, rp.Browser);

            int id = cv.SentId;
            int accountId = cv.AccountId;
            int version = cv.Version;
            string target = cv.Target;
            string barcode = cv.Coupon;
            string personal = cv.Personal;
            int srcId = cv.SrcId;
            string personalDisplay = cv.PersonalDisplay;

            if (rp.HasTarget)
            {
                personal = rp.Personal;
                srcId = rp.SrcId;
                target = rp.Target;

            }

            string encid = MControl.Sys.RquestQuery.EncryptEx32(id.ToString());

            string mail_item = cv.GetBody();// Html;
            string[] personalArgs = ViewUtil.CreatePersonalArgs(personalDisplay);

            if (!string.IsNullOrEmpty(personal) && personalArgs != null && personalArgs.Length > 0)
            {
                mail_item = RemoteUtil.BuildMessage(mail_item, personal, personalArgs);
            }

            mail_item = mail_item.Replace(LinkView.SentID, encid);

            //string showId = RequestMailShow.CreateEncParam(id, campaignId, version, rp.EnableMobile==1, rp.EnableTransfer==1, target, personal, barcode);
            //mail_item = mail_item.Replace(LinkView.ShowID, showId);

            mail_item = mail_item.Replace(ViewUtil.cnShowText, "");

            string removeId = RequestMailRemove.CreateEncParam(id, accountId, version, rp.EnableMobile == 1, rp.EnableTransfer == 1, target);
            mail_item = mail_item.Replace(LinkView.RemoveID, removeId);

            //just in case
            mail_item = mail_item.Replace(ViewUtil.cnRemoveText, ViewUtil.DefaultRemoveText);

            return mail_item;
        }

    }

     public class MailDataItem: IDisposable
    {
        #region consts

        //internal const string cnShowUrl = "#showLinkClick#";
        //internal const string cnRemoveUrl = "#showRemoveClick#";
        //internal const string cnShowText = "#showLinkText#";
        //internal const string cnRemoveText = "#showRemoveText#";

        #endregion

        #region ctor

        public MailDataItem()
        {
            
        }


        public MailDataItem(int SrcId, int Version, bool isPreview = false)
        {
            this.SrcId = SrcId;
            this.Version = Version;

            Data_View_Item cv = Data_View_Context.Get(SrcId, Version);
            if (isPreview)
                m_html = cv.Body;
            else
                m_html = cv.Body;
            m_units = cv.Units;
            m_size = cv.Size;
            _IsPreview = isPreview;
        }
        
        public MailDataItem(IMailView view)
        {
            m_html = view.Body;
            m_units = view.Units;
            m_size = view.Size;
        }

        public MailDataItem(string html, int size, int units)
        {
            m_html = html;
            m_units = units;
            m_size = size;
        }

        public virtual void Dispose()
        {
            m_html = null;
        }
        #endregion

        #region Properties

        public string MailAdvTagText
        {
            get;
            set;
        }
        public string[] PersonalArgs
        {
            get;
            set;
        }
        bool _IsPreview=false;
        public bool IsPreview
        {
            get { return _IsPreview; }
            //set;
        }

        public int SrcId
        {
            get;
            set;
        }
        public int Version
        {
            get;
            set;
        }
        public int AccountId
        {
            get;
            set;
        }
 
        protected int m_units = 1;
        public int Units
        {
            get { return m_units <= 0 ? 1 : m_units; }
        }

        protected int m_size;
        public int Size
        {
            get { return m_size; }
        }
        protected string m_html;
        public string Html
        {
            get { return m_html; }
         }
        #endregion

        #region static

        public static string BuildMailView(string html, bool enableShow, bool enableRemove,string removeText, bool enableSentBy, bool enableAccountDetails, string accountDetails)
        {

            string image_footer = string.Format(ViewConfig.MailImageFooter, LinkView.SentID);

            string showLink = enableShow ? string.Format("<div style=\"width:100%;display:block;text-align:center;\"><div><a href=\"{0}\" title=\"show\">{1}</a></div></div>", ViewUtil.cnShowUrl, ViewUtil.cnShowText /*AcFeatures.MailShowText*/) : "";

            //add header 
            string account_sender = enableAccountDetails ? string.Format("<p dir='rtl' style=\"width:100%;background:#f0f0f0;border-top:1px solid #999999;margin-top:8px;\"><span style=\"font-size: 10pt;\"><strong>{0}</strong></span></p>", accountDetails) : "";

            //add remove link
            string removeLink = enableRemove ? string.Format("<p><a href=\"{0}\" title=\"remove from list\">{1}</a></p> ", ViewUtil.cnRemoveUrl, ViewUtil.cnRemoveText) : "";

            //add Sent by link
            string sentByLink =enableSentBy? string.Format("<p class=\"powerd-by\">{0}</p>", ViewConfig.MailSentBy):"";

            string mailerDetails = string.Concat("<center>", account_sender, image_footer, removeLink, sentByLink, "</center>");

            //return Netcell.Xhtml.XUtil.FormatXhtml(html, Netcell.Xhtml.XUtil.MatchEntitiesFormat.Html, false);

            html = Netcell.Xhtml.XUtil.FormatMail(html, showLink, mailerDetails, Netcell.Xhtml.XUtil.MatchEntitiesFormat.Html, false);

            if (enableRemove)
            {
                string removeUrl = string.Format(ViewConfig.MailRemovePattern, LinkView.RemoveID);
                html = html.Replace(ViewUtil.cnRemoveUrl, removeUrl);
                html = html.Replace(ViewUtil.cnRemoveText, removeText);
            }
            else
            {
                html = html.Replace(ViewUtil.cnRemoveText, "");
            }

            //set show link
            if (enableShow)
            {
                string showUrl = string.Format(ViewConfig.MailShowPattern, LinkView.ShowID);
                html = html.Replace(ViewUtil.cnShowUrl, showUrl);
            }
            else
            {
                html = html.Replace(ViewUtil.cnShowText, "");
            }


            html = html.Replace(LinkPrefix.LinkRef, ViewConfig.MailLinkUrl);
            html = html.Replace("&amp;", "&");

            return html;
        }
        #endregion

        #region Imparsonate

        public string CreateMailSubject(string subject, string personal)
        {
            if (!string.IsNullOrEmpty(MailAdvTagText))
                subject = subject + MailAdvTagText;
            return RemoteUtil.BuildMessage(subject, personal, PersonalArgs);
        }
        public int GetCampaignVersion()
        {
            return VersionUtil.GetCampaignVersion(SrcId, ViewConfig.ViewMailTarget, IsPreview, true);
        }

        public string ImparsonateView(int id, bool enableShow, string mailShowText, bool enableRemove, string mailRemoveText ,bool enableMobile, bool enableMailTransfer, string target, string barcode, string personal)//, string[] personalDisplay)
        {
            //bool enableMobile = EnableMailMobile;

            string encid = MControl.Sys.RquestQuery.EncryptEx32(id.ToString());

            string mail_item = m_html;
            string[] personalArgs = PersonalArgs;

            if (!string.IsNullOrEmpty(personal) && personalArgs != null && personalArgs.Length > 0)
                mail_item = RemoteUtil.BuildMessage(m_html, personal, personalArgs);
            else
                mail_item = m_html;

            mail_item = mail_item.Replace(LinkView.SentID, encid);

            if (enableShow)
            {
                mail_item = mail_item.Replace(ViewUtil.cnShowText, mailShowText);
                string showId = RequestMailShow.CreateEncParam(id, SrcId, Version, enableMobile, enableMailTransfer, target, personal, barcode);
                mail_item = mail_item.Replace(LinkView.ShowID, showId);
            }
            else
            {
                mail_item = mail_item.Replace(ViewUtil.cnShowText, "");
            }
            if (enableRemove)
            {
                mail_item = mail_item.Replace(ViewUtil.cnRemoveText, mailRemoveText);
                string removeId = RequestMailRemove.CreateEncParam(id, AccountId, Version, enableMobile, enableMailTransfer, target);
                mail_item = mail_item.Replace(LinkView.RemoveID, removeId);
            }
            else
            {
                mail_item = mail_item.Replace(ViewUtil.cnRemoveText, "");
            }
 
            return mail_item;
        }
        
        public void FormatMailShowRemoveLinks(bool enableShow,string MailShowText, bool enableRemove, string MailRemoveText)
        {
            //set remove link
            if (enableRemove)
            {
                m_html = m_html.Replace(ViewUtil.cnRemoveText, MailRemoveText);
            }
            else
            {
                m_html = m_html.Replace(ViewUtil.cnRemoveText, "");
            }

            //set show link
            if (enableShow)
            {
                m_html = m_html.Replace(ViewUtil.cnShowText, MailShowText);
            }
            else
            {
                m_html = m_html.Replace(ViewUtil.cnShowText, "");
            }
        }


        #endregion

        #region Show
        /*
        public string ExecuteShow(RequestMailShow request)
        {
            CampaignViewer cv = null;
            try
            {

                cv = new CampaignViewer(request.Id, request.CampaignId, (int)PlatformType.Mail, (int)request.Version, request.ShouldRedirectToMobile, request.Browser);
                if (cv.IsEmpty)
                {
                    throw new MsgException(AckStatus.WebException, "Invalid mail show data item:" + request.Id.ToString());
                }

                //bool isQuiz = false;
                string personal = request.Personal;
                string barcode = request.Barcode;
                string target = request.Target;
                int CampaignId = request.CampaignId;

                int AccountId = cv.AccountId;
                //m_rawBody = cv.Body;
                //string html = cv.Html;
                m_html = cv.Html;

                AccountsFeatures acFeatures = AccountsFeatures.Instanse(AccountId);
                
                //if (request.ShouldRedirectToMobile)
                //{
                //    //string html = LinkViewBuilder.RenderLinks(CampaignId, m_rawBody, 0, false, isQuiz);
                //    html = cv.Html;
                //    m_html = BuildCampaignMobile(html, acFeatures.EnableMailRemove);
                //}
                //else
                //{
                //    m_html = html;// Types.NZ(dr["Html"], "");
                //}


                string PersonalDisplay = cv.PersonalDisplay;

                if (!request.HasTarget)//request.Version != MailVersion.FullArgs)
                {
                    personal = cv.Personal;
                    CampaignId = cv.CampaignId;
                    target = cv.Target;

                }

                //if (cv.Version != VersionUtil.MailDemo && string.IsNullOrEmpty(m_html))
                //{
                //    html = LinkViewBuilder.RenderLinks(CampaignId, m_rawBody, 0, false, isQuiz);

                //    m_html = BuildCampaignHtml(html, false, acFeatures.EnableMailRemove);

                //}

                if (string.IsNullOrEmpty(m_html))
                {
                    throw new AppException(AckStatus.InvalidContent, "Invalid html body for campaign:" + CampaignId.ToString());
                }


                //FormatMailShowRemoveLinks(false, acFeatures.EnableMailRemove);

                PersonalArgs = CreatePersonalArgs(PersonalDisplay);

                m_html = ImparsonateView(request.Id, (int)request.Version, request.EnableMobile == 1, request.EnableTransfer == 1, request.Target, barcode, personal);

                return Html;
            }
            catch (MsgException mex)
            {
                throw mex;
            }
            catch (Exception ex)
            {
                ApiLog.ErrorFormat("MailView Error:{0}", ex.Message);
                throw ex;
            }
            finally
            {
                if (cv != null)
                {
                    cv.Dispose();
                }
            }
        }
        */
        #endregion

        #region Preview

        public static string Preview(int SrcId, int Version)
        {
            //IsBrowser = true;
            try
            {

                if (SrcId <= 0)
                {
                    throw new Exception("Invalid mail item");
                }
                DataRow dr = null;
                
                using (DalView instance = new DalView())
                {
                    instance.Data_View(0,SrcId, Version, (int)PlatformType.Mail,true,false,"");
                }
                if (dr == null)
                {
                    throw new Exception("Invalid mail data item for SrcId:" + SrcId.ToString()+", ");
                }

                string html= Types.NZ(dr["Html"], "");
                string personal = Types.NZ(dr["Personal"], "");
                int srcId = Types.ToInt(dr["SrcId"], 0);
                int version = Types.ToInt(dr["Version"], 0);
                int AccountId = Types.ToInt(dr["AccountId"], 0);
                string PersonalDisplay = Types.NZ(dr["PersonalDisplay"], "");
                string Target = Types.NZ(dr["Target"], "");

                //Mail_body = RemoteUtil.BuildMessage(mail_body, personal);

                if (string.IsNullOrEmpty(html))
                {
                    throw new AppException(AckStatus.InvalidContent, "Invalid html body for srcId:" + srcId.ToString());

                }
                html = BuildMailView(html, false, false,"", false, false, "");

                if (!string.IsNullOrEmpty(PersonalDisplay))
                {
                    string[] personalArgs = ViewUtil.CreatePersonalArgs(PersonalDisplay);
                    html = RemoteUtil.BuildMessage(html, personal, personalArgs);
                }

                int id = 0;
                string encid = id.ToString();// MControl.Sys.RquestQuery.EncryptQueryArgs(id.ToString());
                html = html.Replace(LinkView.SentID, encid);

                
                return HtmlHelper.FormatHtmlPreview(html);
            }
            catch (Exception ex)
            {
                Netcell.Log.ErrorFormat("MailPreview Error:{0}", ex.Message);
                throw ex;
            }
        }

        #endregion

    }


    [Serializable]
    public class MailDataViewBuilder : MailDataItem, IDisposable
    {

        #region ctor


        public MailDataViewBuilder(int srcId, int version, int accountId, int userId, string body, bool enableImageSize)
        {
            SrcId = srcId;
            Version = version;
            AccountId = accountId;
            m_body = Netcell.Xhtml.XUtil.FormatXhtml(body, Xhtml.XUtil.MatchEntitiesFormat.Html, false);
            MethodType method = MethodType.MALMT;
            this.m_modeState = MailMode.Mail;

            UnitsItem ui = UnitsItem.CalcHtmlSize(body, enableImageSize);
            this.m_size = ui.Length;
            //this.images_size = ui.ContentSize;
            this.m_units = 1;

            if (accountId > 0)
            {
                this.m_units = BillingItem.CreateBillingItem(accountId, method, ui, 1, userId, CreditMode.None).ItemUnits;
                //units = RemoteUtil.GetMailBillingUnits(accountId, TotalSize, ref item_price);
            }
            //CalcMailSize(accountId, mail);
        }


        public MailDataViewBuilder(int srcId, int version, int accountId, string html, int item_size, int item_units)
        {
            SrcId = srcId;
            Version = version;
            AccountId = accountId;
            m_body = Netcell.Xhtml.XUtil.FormatXhtml(html, Xhtml.XUtil.MatchEntitiesFormat.Html, false);
            this.m_size = item_size;
            this.m_units = item_units;
        }

        public MailDataViewBuilder(int srcId, int version, int accountId, ICampaignContent content)
        {
            SrcId = srcId;
            Version = version;
            AccountId = accountId;
            m_body = Netcell.Xhtml.XUtil.FormatXhtml(content.GetContent(), Xhtml.XUtil.MatchEntitiesFormat.Html, false);
            this.m_size = content.ContentSize;
            this.m_units = content.ContentUnits;
        }

        public override void Dispose()
        {
            base.Dispose();
            //m_html = null;
            m_body = null;
            AccountDetails = null;
            MailSender = null;
            //if (m_acFeatures != null)
            //{
            //    m_acFeatures.Dispose();
            //}
        }

        #endregion

        #region properties

        protected MailMode m_modeState;
        public MailMode ModeState
        {
            get { return m_modeState; }
        }

        //protected string m_html;
        //public string Html
        //{
        //    get { return m_html; }
        //}

        protected string m_body;
        public string Body
        {
            get { return m_body; }
        }


        #endregion

        #region Account features
         
        private bool EnableMailShow;
        private bool EnableMailRemove;
        private string MailShowText;
        private string MailRemoveText;
        private bool EnableAccountDetailsTag;

        private string AccountDetails;
        private string MailSender;
        private bool EnableMailMobile;
        private Lang MailLang;

        private bool m_Initilaized = false;
        public bool Initilaized
        {
            get { return m_Initilaized; }
        }

        internal void ValidateInitilaized()
        {
            if (!Initilaized)
            {
                throw new MsgException(AckStatus.UnExpectedError, "Mail account features not Initilaized");
            }
            if(string.IsNullOrEmpty(Body))
            {
                throw new MsgException(AckStatus.InvalidContent, "Mail Body is empty");
            }
        }

        public void LoadAccountFeatures(int accountId, string Sender, bool enableMobile, Lang lang)
        {
            AccountsFeatures acFeatures = AccountsFeatures.Instanse(accountId);
            LoadAccountFeatures(acFeatures, Sender, enableMobile, lang);
        }

        public void LoadAccountFeatures(AccountsFeatures acFeatures, string Sender, bool enableMobile, Lang lang)
        {
            //AccountId = accountId;
            MailLang = lang;
            EnableMailMobile = enableMobile;
            string maSender = Sender;
            if (!MControl.Regx.IsEmail(maSender))
            {
                maSender = string.Format(ViewConfig.MailSender, Sender);
                if (!MControl.Regx.IsEmail(maSender))
                {
                   //throw 

                }
            }

            MailSender = maSender;
            EnableMailRemove = acFeatures.EnableMailRemove;
            EnableMailShow = acFeatures.EnableMailShow;
            MailRemoveText = acFeatures.MailRemoveText;
            MailShowText = acFeatures.GetShowText(enableMobile);//MailShowText;

            EnableAccountDetailsTag = acFeatures.EnableAccountDetailsTag;
            AccountDetails = acFeatures.AccountDetailsTagText;
            if (acFeatures.EnableAccountDetailsTag && string.IsNullOrEmpty(AccountDetails))
            {
                AccountDetails = (string)DalCampaign.Instance.Accounts_Details(AccountId);
            }

            m_Initilaized = true;
        }




        #endregion

        #region ExtractView

        //public void ExtractView(CampaignPath path)
        //{
        //    ExtractView(path.UploadLocalPath,path.UploadVirtualPath,path.WebClientLocalPath, path.WebClientVirtualPath,path.IsQuiz);
        //}

        public void ExtractView(string uploadLocalPath, string uploadVirtualPath,
                      string webClientLocalPath, string webClientVirtualPath)
        {
            ValidateInitilaized();

            string body = Body;

            LinkViewBuilder lbuilder = new LinkViewBuilder();

            LinkViewItems links = lbuilder.BuildLinks(SrcId,Version, body, 0, false, false, true);

            m_body = lbuilder.Body;

            string html = m_body;

            html = LinkViewBuilder.RenderLinks(links, html, 0);

            html = Regex.Replace(html, "=\"" + uploadLocalPath, "=\"" + uploadVirtualPath, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            html = Regex.Replace(html, "=\"" + webClientLocalPath, "=\"" + webClientVirtualPath, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            //html = Xhtml.XUtil.FormatXhtml(html, Netcell.Xhtml.XUtil.MatchEntitiesFormat.Html, false);
            
            //html = m_html.Replace(ViewUtil.cnRemoveText, MailRemoveText);
            //html = m_html.Replace(ViewUtil.cnShowText, MailShowText);

            m_html = MailDataItem.BuildMailView(html, EnableMailShow, EnableMailRemove,MailRemoveText,  true, EnableAccountDetailsTag, AccountDetails);

        }
        #endregion

        #region SaveContent

        public int SaveContent(bool isPreview = false)
        {
            ValidateInitilaized();
            bool isExists = true;
            Data_View_Context context = new Data_View_Context(SrcId,Version);

            Data_View_Item cv = context.Entity;

            if (context.IsEmpty)
            {
                isExists = false;
            }
            
            //Campaigns_View cv = new Campaigns_View(CampaignId);
            cv.SrcId = SrcId;
            cv.Version = Version;

            cv.PlatformView = (int)PlatformType.Mail;
            if (isPreview)
            {
                cv.Body = Html;
                if (!isExists)
                {
                    cv.Body = Body;
                    //cv.Html = Html;
                }
            }
            else
            {
                cv.Body = Body;
                //cv.Html = Html;
            }
            cv.Size = Size;
            cv.Units = Units;
            cv.Modified = DateTime.Now;

            return context.SaveChanges(isExists ? MControl.Data.UpdateCommandType.Update : MControl.Data.UpdateCommandType.Insert);

            //return cv.UpdateChanges(!cv.IsExists);
        }

        #endregion
    }

}
