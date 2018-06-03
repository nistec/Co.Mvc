using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using Netcell.Lib;
using Netcell.Remoting;
using Netcell.Lib.View;
using Netcell.Web;
using Netcell.Data.Entities;
using Netcell.Data;
using Netcell;

namespace Netcell.Lib.View
{

    public class MailView : IDisposable
    {
        #region consts

        //internal const string cnShowUrl = "#showLinkClick#";
        //internal const string cnRemoveUrl = "#showRemoveClick#";
        //internal const string cnShowText = "#showLinkText#";
        //internal const string cnRemoveText = "#showRemoveText#";

        #endregion

        #region ctor

        public MailView()
        {

        }


        public MailView(int campaignId, bool isPreview = false)
        {
            CampaignId = campaignId;
            Campaign_View cv = Campaign_View_Mail_Context.Get(campaignId);
            if (isPreview)
                m_html = cv.Preview;
            else
                m_html = cv.Html;
            m_units = cv.Units;
            m_size = cv.Size;
            _IsPreview = isPreview;
        }

        public MailView(IMailView view)
        {
            m_html = view.Body;
            m_units = view.Units;
            m_size = view.Size;
        }

        public MailView(string html, int size, int units)
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
        bool _IsPreview = false;
        public bool IsPreview
        {
            get { return _IsPreview; }
            //set;
        }

        public int CampaignId
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

        public static string BuildMailView(string html, bool enableShow, bool enableRemove, string removeText, bool enableSentBy, bool enableAccountDetails, string accountDetails)
        {

            string image_footer = string.Format(ViewConfig.MailImageFooter, LinkView.SentID);

            string showLink = enableShow ? string.Format("<div style=\"width:100%;display:block;text-align:center;\"><div><a href=\"{0}\" title=\"show\">{1}</a></div></div>", ViewUtil.cnShowUrl, ViewUtil.cnShowText /*AcFeatures.MailShowText*/) : "";

            //add header 
            string account_sender = enableAccountDetails ? string.Format("<p dir='rtl' style=\"width:100%;background:#f0f0f0;border-top:1px solid #999999;margin-top:8px;\"><span style=\"font-size: 10pt;\"><strong>{0}</strong></span></p>", accountDetails) : "";

            //add remove link
            string removeLink = enableRemove ? string.Format("<p><a href=\"{0}\" title=\"remove from list\">{1}</a></p> ", ViewUtil.cnRemoveUrl, ViewUtil.cnRemoveText) : "";

            //add Sent by link
            string sentByLink = enableSentBy ? string.Format("<p class=\"powerd-by\">{0}</p>", ViewConfig.MailSentBy) : "";

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
            return VersionUtil.GetCampaignVersion(CampaignId, ViewConfig.ViewMailTarget, IsPreview, true);
        }

        public string ImparsonateView(int id, int version, bool enableShow, string mailShowText, bool enableRemove, string mailRemoveText, bool enableMobile, bool enableMailTransfer, string target, string barcode, string personal)//, string[] personalDisplay)
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
                string showId = RequestMailShow.CreateEncParam(id, CampaignId, version, enableMobile, enableMailTransfer, target, personal, barcode);
                mail_item = mail_item.Replace(LinkView.ShowID, showId);
            }
            else
            {
                mail_item = mail_item.Replace(ViewUtil.cnShowText, "");
            }
            if (enableRemove)
            {
                mail_item = mail_item.Replace(ViewUtil.cnRemoveText, mailRemoveText);
                string removeId = RequestMailRemove.CreateEncParam(id, AccountId, version, enableMobile, enableMailTransfer, target);
                mail_item = mail_item.Replace(LinkView.RemoveID, removeId);
            }
            else
            {
                mail_item = mail_item.Replace(ViewUtil.cnRemoveText, "");
            }

            return mail_item;
        }

        public void FormatMailShowRemoveLinks(bool enableShow, string MailShowText, bool enableRemove, string MailRemoveText)
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

        public static string Preview(int campaignId)
        {
            //    MailView item = new MailView();
            //    return item.ExecutePreview(campaignId);
            //}

            //public string ExecutePreview(int campaignId)
            //{

            //IsBrowser = true;
            try
            {

                if (campaignId <= 0)
                {
                    throw new Exception("Invalid mail item");
                }
                DataRow dr = null;

                using (DalView instance = new DalView())
                {
                    instance.Campaign_Preview(campaignId, 0, (int)PlatformType.Mail, 0);
                }
                if (dr == null)
                {
                    throw new Exception("Invalid mail data item for campaign:" + campaignId.ToString());
                }

                string html = Types.NZ(dr["Html"], "");
                string personal = Types.NZ(dr["Personal"], "");
                int CampaignId = Types.ToInt(dr["CampaignId"], 0);
                int AccountId = Types.ToInt(dr["AccountId"], 0);
                string PersonalDisplay = Types.NZ(dr["PersonalDisplay"], "");
                string Target = Types.NZ(dr["Target"], "");

                //Mail_body = RemoteUtil.BuildMessage(mail_body, personal);

                if (string.IsNullOrEmpty(html))
                {
                    throw new AppException(AckStatus.InvalidContent, "Invalid html body for campaign:" + CampaignId.ToString());

                }
                html = BuildMailView(html, false, false, "", false, false, "");

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
    public class MailViewBuilder : MailView, IDisposable
    {

        #region ctor


        public MailViewBuilder(int campaignId,int accountId, int userId, string body, bool enableImageSize)
        {
            CampaignId = campaignId;
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


        public MailViewBuilder(int campaignId, int accountId, string html, int item_size, int item_units)
        {
            CampaignId = campaignId;
            AccountId = accountId;
            m_body = Netcell.Xhtml.XUtil.FormatXhtml(html, Xhtml.XUtil.MatchEntitiesFormat.Html, false);
            this.m_size = item_size;
            this.m_units = item_units;
        }

        public MailViewBuilder(int campaignId, int accountId, ICampaignContent content)
        {
            CampaignId = campaignId;
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

        public void ExtractView(CampaignPath path)
        {
            ExtractView(path.UploadLocalPath,path.UploadVirtualPath,path.WebClientLocalPath, path.WebClientVirtualPath,path.IsQuiz);
        }

        public void ExtractView(string uploadLocalPath, string uploadVirtualPath,
                      string webClientLocalPath, string webClientVirtualPath, bool isQuiz)
        {
            ValidateInitilaized();

            string body = Body;

            LinkViewBuilder lbuilder = new LinkViewBuilder();

            LinkViewItems links = lbuilder.BuildLinks(CampaignId, body, 0, false, isQuiz, false, true);

            m_body = lbuilder.Body;

            string html = m_body;

            html = LinkViewBuilder.RenderLinks(links, html, 0, false, isQuiz);

            html = Regex.Replace(html, "=\"" + uploadLocalPath, "=\"" + uploadVirtualPath, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            html = Regex.Replace(html, "=\"" + webClientLocalPath, "=\"" + webClientVirtualPath, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            //html = Xhtml.XUtil.FormatXhtml(html, Netcell.Xhtml.XUtil.MatchEntitiesFormat.Html, false);
            
            //html = m_html.Replace(ViewUtil.cnRemoveText, MailRemoveText);
            //html = m_html.Replace(ViewUtil.cnShowText, MailShowText);

            m_html = MailView.BuildMailView(html, EnableMailShow, EnableMailRemove,MailRemoveText,  true, EnableAccountDetailsTag, AccountDetails);

        }
        #endregion

        #region SaveContent

        public int SaveContent(bool isPreview = false)
        {
            ValidateInitilaized();
            bool isExists = true;
            Campaign_View_Mail_Context context = new  Campaign_View_Mail_Context(CampaignId);

            Campaign_View cv = context.Entity;

            if (context.IsEmpty)
            {
                isExists = false;
            }
            
            //Campaigns_View cv = new Campaigns_View(CampaignId);
            cv.CampaignId = CampaignId;
            cv.PlatformView = (int)PlatformType.Mail;
            if (isPreview)
            {
                cv.Preview = Html;
                if (!isExists)
                {
                    cv.Body = Body;
                    cv.Html = Html;
                }
            }
            else
            {
                cv.Body = Body;
                cv.Html = Html;
            }
            cv.Size = Size;
            cv.Units = Units;
            cv.Modified = DateTime.Now;

            return context.SaveChanges(isExists ? MControl.Data.UpdateCommandType.Update : MControl.Data.UpdateCommandType.Insert);

            //return cv.UpdateChanges(!cv.IsExists);
        }

        //public int SavePreview()
        //{
        //    ValidateInitilaized();

        //    bool isExists = true;
        //    Campaign_View_Mail_Context context = new Campaign_View_Mail_Context(CampaignId);

        //    Campaign_View cv = context.Entity;

        //    if (context.IsEmpty)
        //    {
        //        isExists = false;
        //        cv.CampaignId = CampaignId;
        //    }

        //    //Campaigns_View cv = new Campaigns_View(CampaignId);
        //    //cv.CampaignId = CampaignId;
        //    cv.PlatformView = (int)PlatformType.Mail;
        //    cv.Preview = Html;
        //    //cv.Size = Size;
        //    //cv.Units = Units;
        //    //return cv.UpdateChanges(!cv.IsExists);

        //    return context.SaveChanges(isExists ? MControl.Data.UpdateCommandType.Update : MControl.Data.UpdateCommandType.Insert);

        //}
        #endregion
    }

}
