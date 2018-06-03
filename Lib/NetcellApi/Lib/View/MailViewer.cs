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
   

    #region MailViewer

    public class MailViewer : CampaignViewer
    {

        public static CampaignViewer Get(int messageId, int version, IDeviceInfo device)
        {
            using (CampaignViewer_Context view = new CampaignViewer_Context(messageId, 0, (int)PlatformType.Mail, version, device.IsMobileDevice, device.DeviceId))
            {
                if (view.IsEmpty)
                {
                    throw new MsgException(AckStatus.MobileException, string.Format("MailViewer.Invalid data for : messageId:{0}, campaignId:{1}, platform:{2}, version:{3}, IsMobile:{4}, DeviceNam:{5}", messageId, 0, PlatformType.Mail, version, device.IsMobileDevice, device.DeviceId));
                }
                return view.Entity;
            }
        }

        public static CampaignViewer Get(int messageId, int campaignId, PlatformType platform, int version, IDeviceInfo device)
        {
            using (CampaignViewer_Context view = new CampaignViewer_Context(messageId, campaignId, (int)platform, version, device.IsMobileDevice, device.DeviceId))
            {
                if (view.IsEmpty)
                {
                    throw new MsgException(AckStatus.MobileException, string.Format("MailViewer.Invalid data for : messageId:{0}, campaignId:{1}, platform:{2}, version:{3}, IsMobile:{4}, DeviceNam:{5}", messageId, campaignId, platform, version, device.IsMobileDevice, device.DeviceId));
                }
                return view.Entity;
            }
        }

        public static CampaignViewer Get(int messageId, int campaignId, PlatformType platform, int version, bool IsMobile, string DeviceName)
        {
            using (CampaignViewer_Context view = new CampaignViewer_Context(messageId, campaignId, (int)platform, version, IsMobile, DeviceName))
            {
                if (view.IsEmpty)
                {
                    throw new MsgException(AckStatus.MobileException, string.Format("MailViewer.Invalid data for : messageId:{0}, campaignId:{1}, platform:{2}, version:{3}, IsMobile:{4}, DeviceNam:{5}", messageId, campaignId, platform, version, IsMobile, DeviceName));
                }
                return view.Entity;
            }
        }

        
        #region static

        public static CampaignViewer Instance(System.Web.HttpRequest request)//, bool isMobileDevice)
        {
            int Id = 0;
            int CampaignId = 0;
            int EnableMobile = 0;
            int EnableTransfer = 0;
            bool IsMobile = false;
            string Browser = "NA";
            string Personal = "";
            string Barcode = "";
            string Target = "";
            int Version = VersionUtil.VersionView;
            bool IsPreview = false;
            //pageType = MailPageType.NA;

            try
            {

                string strid = request.QueryString[0];//["id"];

                if (string.IsNullOrEmpty(strid))
                {
                    throw new Exception("Invalid RequestMailParam");
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
                            CampaignId = Types.ToInt(args.Get("cid"), 0);
                            Version = VersionUtil.VersionView;
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

                IsMobile = Netcell.Web.BrowserHelper.GetBrowser(request, out Browser) == BrowserType.Mobile;

                //return new MailViewer(Id, CampaignId, PlatformType.Mail, Version, IsMobile, Browser);

               return MailViewer.Get(Id, CampaignId, PlatformType.Mail, Version, IsMobile, Browser);

            }
            catch (Exception ex)
            {
                Netcell.Log.ErrorFormat("RequestMailParam Error:{0}", ex.Message);
                throw ex;
            }
        }

        #endregion

        #region ctor
        /*
        public MailViewer(int messageId, int version, IDeviceInfo device)
            : this(messageId, 0, PlatformType.Mail, version, device.IsMobileDevice, device.ModelName)
        {
        }

        public MailViewer(int messageId, int campaignId, PlatformType platform, int version, IDeviceInfo device)
            : this(messageId, campaignId, platform, version, device.IsMobileDevice, device.ModelName)
        {
        }
        public MailViewer(int messageId, int campaignId, PlatformType platform, int version, bool IsMobile, string DeviceName)
            : base(messageId, campaignId, (int)platform, version, IsMobile, DeviceName)
        {
            if (IsEmpty)
            {
                string msg= string.Format("MailViewer.Invalid data for : messageId:{0}, campaignId:{1}, platform:{2}, version:{3}, IsMobile:{4}, DeviceNam:{5}", messageId, campaignId, platform, version, IsMobile, DeviceName);

                throw new MsgException(AckStatus.WebException, msg);
            }
        }
       */
 
        #endregion

        public static string RenderView(RequestMailShow rp)
        {
            CampaignViewer cv = MailViewer.Get(rp.Id, rp.CampaignId, PlatformType.Mail, rp.Version, rp.IsMobile, rp.Browser);
            
            int id = cv.SentId;
            int accountId = cv.AccountId;
            int version = cv.Version;
            string target = cv.Target;
            string barcode = cv.Coupon;
            string personal = cv.Personal;
            int campaignId = cv.CampaignId;
            string personalDisplay = cv.PersonalDisplay;

            if (rp.HasTarget)
            {
                personal = rp.Personal;
                campaignId = rp.CampaignId;
                target = rp.Target;

            }

            string encid = MControl.Sys.RquestQuery.EncryptEx32(id.ToString());

            string mail_item = cv.GetHtml();// Html;
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


    #endregion

  


}
