using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netcell.Web;
using Netcell.Lib;
using Netcell.Remoting;
using Netcell.Data;
using Netcell;

namespace Netcell.Lib.View
{

    public enum MailPageType
    {
        NA,
        Show,
        Remove,
        Link,
        Confirm,
        Footer,
        Thanks
    }

    #region RequestMailShow

    public struct RequestMailShow
    {
        public int Id;
        public int CampaignId;
        public int EnableMobile;//0=no;1=yes
        public int EnableTransfer;//0=no;1=yes
        public bool IsMobile;
        public string Browser;
        public string Personal;
        public string Barcode;
        public string Target;
        public int Version;//0=messageid,1=sentid,2=transid,3=fullArgs
        public bool IsPreview;

        public static string CreateEncParam(int id, int campaignId, int version, bool enableMobile, bool enableTransfer, string target, string personal,string barcode)
        {
            string strid = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", VersionUtil.GetIdArg(id), VersionUtil.GetCampaignArg(campaignId), VersionUtil.GetVersionArg(version), VersionUtil.GetTargetArg(target), VersionUtil.GetPersonalArg(personal), VersionUtil.GetMobileArg(enableMobile), VersionUtil.GetTransferArg(enableTransfer), VersionUtil.GetBarcodeArg(barcode));
            return MControl.Sys.RquestQuery.EncryptEx32(strid);
        }

        public static string CreateShowUrl(int id, int campaignId, bool enableMobile, bool enableTransfer, string target, string personal, string barcode, bool isDemo)
        {
            int version = (int)VersionUtil.VersionView;//.GetShowVersion(id, campaignId, target, isDemo);
            string strid = CreateEncParam(id, campaignId, version, enableMobile, enableTransfer, target, personal,barcode);
            return string.Format(ViewConfig.MailShowPattern, strid);
        }

        #region mobile

        public bool ShouldRedirectToMobile
        {
            get { return IsMobile && EnableMobile > 0; }
        }

        public string MobileUrl
        {
            get { return string.Format(ViewConfig.EMobileShowPattern, Id, CampaignId); }
        }
        public string MobileUrlRelative
        {
            get { return string.Format(ViewConfig.EMobileShowPatternRelative, Id, CampaignId); }
        }
        public bool HasTarget
        {
            get { return !string.IsNullOrEmpty(Target); }
        }
        #endregion

        public RequestMailShow(System.Web.HttpRequest request)
         {
             Id = 0;
             CampaignId = 0;
             EnableMobile = 0;
             EnableTransfer = 0;
             IsMobile = false;
             Browser = "NA";
             Personal = "";
             Barcode = "";
             Target = "";
             Version = VersionUtil.VersionView;
             IsPreview = false;
             //pageType = MailPageType.NA;

             string strid = request.QueryString[0];//["id"];
             ParseRequestMailShow(strid);
             IsMobile = Netcell.Web.BrowserHelper.GetBrowser(request, out Browser) == BrowserType.Mobile;
        }

        public RequestMailShow(string strid, bool isMobile)
        {
            Id = 0;
            CampaignId = 0;
            EnableMobile = 0;
            EnableTransfer = 0;
            IsMobile = false;
            Browser = "NA";
            Personal = "";
            Barcode = "";
            Target = "";
            Version = VersionUtil.VersionView;
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
                    throw new Exception("Invalid RequestMailParam");
                }

                if (int.TryParse(strid, out Id))
                {
                    Version = VersionUtil.VersionView;//.MessageId;
                }
                else
                {

                    //Netcell.Log.DebugFormat("RequestMailShow qs:{0}", strid);
                    string encid =null;
                    try
                    {
                        if (strid.StartsWith("0X"))
                            encid = MControl.Sys.RquestQuery.DecryptEx32(strid);
                        else
                            encid = MControl.Sys.RquestQuery.DecryptQueryOptional(strid);
                    }
                    catch
                    {
                        throw new MsgException(AckStatus.WebException, "RequestMailShow DecryptEx32 error:" + strid);
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
                            CampaignId = Types.ToInt(args.Get("cid"), 0);
                            Version = VersionUtil.VersionView;//.FullArgs;
                        }
                        else
                        {
                            Id = Types.ToInt(args.Get(VersionUtil.TagId), 0);

                            CampaignId = Types.ToInt(args.Get(VersionUtil.TagCampaignId), 0);
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
                    throw new Exception("Invalid mail item:" + strid);
                }

                //IsMobile = request.Browser.IsMobileDevice;

                //IsMobile = Netcell.Web.BrowserHelper.GetBrowser(request, out Browser) == BrowserType.Mobile;



            }
            catch (Exception ex)
            {
                Netcell.Log.ErrorFormat("RequestMailParam Error:{0}", ex.Message);
                throw ex;
            }
        }

        public string Print()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            sb.AppendFormat("Id {0}\rn", Id);
            sb.AppendFormat("CampaignId {0}\rn", CampaignId);
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

    

    #endregion

    #region RequestMailRemove

    public struct RequestMailRemove
    {
        public int Id;
        public int AccountId;
        public int EnableMobile;//0=no;1=yes
        public int EnableTransfer;//0=no;1=yes
        public bool IsMobile;
        public string Browser;
        public string Target;//target or personal
        public int Version;//0=messageid,1=sentid,2=transid,3=fullArgs
        public bool IsPreview;

        public static string CreateEncParam(int id, int accountId, int version, bool enableMobile, bool enableTransfer, string target)
        {
            string strid = string.Format("{0}{1}{2}{3}{4}{5}", VersionUtil.GetIdArg(id), VersionUtil.GetAccountArg(accountId), VersionUtil.GetVersionArg(version), VersionUtil.GetTargetArg(target), VersionUtil.GetMobileArg(enableMobile), VersionUtil.GetTransferArg(enableTransfer));
            return MControl.Sys.RquestQuery.EncryptEx32(strid);
        }

        public static string CreateRemoveUrl(int id, int accountId, bool enableMobile, bool enableTransfer, string target, bool isDemo)
        {
            int version = (int)VersionUtil.VersionView;//.GetRemoveVersion(id, accountId, target, isDemo);

            string strid = CreateEncParam(id, accountId, version,enableMobile, enableTransfer, target);
            return string.Format(ViewConfig.MailRemovePattern, strid);
        }

        #region mobile

        public bool ShouldRedirectToMobile
        {
            get { return IsMobile && EnableMobile > 0; }
        }

        public string MobileUrl
        {
            get { return string.Format(ViewConfig.EMobileRemovePattern, Id); }
        }

        public bool HasTarget
        {
            get { return !string.IsNullOrEmpty(Target); }
        }
        #endregion

        #region ctor

        public RequestMailRemove(System.Web.HttpRequest request)
        {
            Id = 0;
            AccountId = 0;
            EnableMobile = 0;
            EnableTransfer = 0;
            IsMobile = false;
            Browser = "NA";
            Target = "";
            Version = 0;
            IsPreview = false;

            string strid = request.QueryString[0];//["id"];
            ParseRequestMailRemove(strid);
            IsMobile = Netcell.Web.BrowserHelper.GetBrowser(request, out Browser) == BrowserType.Mobile;
        }
/*
        public RequestMailRemove(string strid)
        {
            Id = 0;
            AccountId = 0;
            EnableMobile = 0;
            EnableTransfer = 0;
            IsMobile = false;
            Browser = "NA";
            Target = "";
            Version = 0;
            IsPreview = false;

            try
            {

                //string strid = request.QueryString[0];//["id"];

                if (string.IsNullOrEmpty(strid))
                {
                    throw new Exception("Invalid RequestMailParam");
                }


                if (int.TryParse(strid, out Id))
                {
                    Version = VersionUtil.VersionView;// MailVersion.MessageId;
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
                            AccountId = Types.ToInt(args.Get("acc"), 0);
                        }
                        else
                        {

                            Id = Types.ToInt(args[VersionUtil.TagId], 0);
                            AccountId = Types.ToInt(args.Get(VersionUtil.TagAccountId), 0);
                            Target = args.Get(VersionUtil.TagTarget);
                            Version = Types.ToInt(args[VersionUtil.TagVersion], 0);


                            IsPreview = Version == VersionUtil.VersionPreview;
                            EnableMobile = Types.ToInt(args.Get(VersionUtil.TagEnableMobile), 0);
                            EnableTransfer = Types.ToInt(args.Get(VersionUtil.TagEnableTransfer), 0);
                        }
                        //VersionUtil.ParseVersion(Version, out EnableMobile, out EnableTransfer, out IsDemo);


                    }

                }

                if (Id <= 0)
                {
                    throw new Exception("Invalid mail item:" + strid);
                }
                //IsMobile = request.Browser.IsMobileDevice;

                IsMobile = Netcell.Web.BrowserHelper.GetBrowser(request, out Browser) == BrowserType.Mobile;

            }
            catch (Exception ex)
            {
                Netcell.Log.ErrorFormat("RequestMailRemoveParam Error:{0}", ex.Message);
                throw ex;
            }
        }
*/

        public RequestMailRemove(string strid, bool isMobile)
        {
            Id = 0;
            AccountId = 0;
            EnableMobile = 0;
            EnableTransfer = 0;
            IsMobile = false;
            Browser = "NA";
            Target = "";
            Version = 0;
            IsPreview = false;

            ParseRequestMailRemove(strid);
            IsMobile = isMobile;
        }

        void ParseRequestMailRemove(string strid)
        {
            //Id = 0;
            //AccountId = 0;
            //EnableMobile = 0;
            //EnableTransfer = 0;
            //IsMobile = false;
            //Browser = "NA";
            //Target = "";
            //Version = 0;
            //IsPreview = false;

            try
            {

                //string strid = request.QueryString[0];//["id"];

                if (string.IsNullOrEmpty(strid))
                {
                    throw new Exception("Invalid RequestMailParam");
                }


                if (int.TryParse(strid, out Id))
                {
                    Version = VersionUtil.VersionView;// MailVersion.MessageId;
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
                            AccountId = Types.ToInt(args.Get("acc"), 0);
                        }
                        else
                        {

                            Id = Types.ToInt(args[VersionUtil.TagId], 0);
                            AccountId = Types.ToInt(args.Get(VersionUtil.TagAccountId), 0);
                            Target = args.Get(VersionUtil.TagTarget);
                            Version = Types.ToInt(args[VersionUtil.TagVersion], 0);


                            IsPreview = Version == VersionUtil.VersionPreview;
                            EnableMobile = Types.ToInt(args.Get(VersionUtil.TagEnableMobile), 0);
                            EnableTransfer = Types.ToInt(args.Get(VersionUtil.TagEnableTransfer), 0);
                        }
                        //VersionUtil.ParseVersion(Version, out EnableMobile, out EnableTransfer, out IsDemo);


                    }

                }

                if (Id <= 0)
                {
                    throw new Exception("Invalid mail item:" + strid);
                }
                //IsMobile = request.Browser.IsMobileDevice;

                //IsMobile = Netcell.Web.BrowserHelper.GetBrowser(request, out Browser) == BrowserType.Mobile;

            }
            catch (Exception ex)
            {
                Netcell.Log.ErrorFormat("RequestMailRemoveParam Error:{0}", ex.Message);
                throw ex;
            }
        }

        #endregion

        //public int Reply_Remove(int Status, PlatformType platform, bool IsAll)
        //{

        //    return DalCampaign.Instance.View_Remove(Id, Status, (int)platform, IsAll, (int)Version,AccountId,Target);
        //}


        public int DoRemove(PlatformType platform, int Status, bool IsAll)
        {

            return DalCampaign.Instance.View_Remove(Id, Status, (int)platform, IsAll, (int)Version, AccountId, Target);
        }
    }

    #endregion

    
}