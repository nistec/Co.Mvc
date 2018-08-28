using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Nistec;

namespace Netcell
{
    public class WebConfig
    {

        public static string BasePath
        {
            get { return ConfigurationManager.AppSettings["BasePath"]; }
        }
 
        public static bool EnableApi
        {
            get { return Types.ToBool(ConfigurationManager.AppSettings["EnableApi"], true); }
        }
        public static bool IsAdmin
        {
            get { return Types.ToBool(ConfigurationManager.AppSettings["IsAdmin"], false); }
        }
        public static bool IsWl
        {
            get { return Types.ToBool(ConfigurationManager.AppSettings["IsWl"], false); }
        }
        public static bool IsCampaignPublish
        {
            get { return Types.ToBool(ConfigurationManager.AppSettings["IsCampaignPublish"], false); }
        }
        public static string SiteName
        {
            get { return ConfigurationManager.AppSettings["SiteName"]; }
        }
        public static string CookiName
        {
            get { return ConfigurationManager.AppSettings["CookiName"]; }
        }
        public static string OwnerName
        {
            get { return ConfigurationManager.AppSettings["OwnerName"]; }
        }
        public static string UploadFilesPath
        {
            get { return ConfigurationManager.AppSettings["UploadFilesPath"]; }
        }

        public static int MinContactsUploadAsync
        {
            get { return Types.ToInt(ConfigurationManager.AppSettings["MinContactsUploadAsync"], 100); }
        }
        public static int MaxItemsPerBatch
        {
            get { return Types.ToInt(ConfigurationManager.AppSettings["MaxItemsPerBatch"], 10000); }
        }

        public static string WebClientLocalPath
        {
            get { return ConfigurationManager.AppSettings["WebClientLocalPath"]; }
        }
        public static string WebClientVirtualPath
        {
            get { return ConfigurationManager.AppSettings["WebClientVirtualPath"]; }
        }
        public static int WapMaxWidth
        {
            get { return Types.ToInt(ConfigurationManager.AppSettings["WapMaxWidth"], 0); }
        }

        public static string Menu
        {
            get { return ConfigurationManager.AppSettings["Menu"]; }
        }
        public static int WlId
        {
            get { return Types.ToInt(ConfigurationManager.AppSettings["WlId"],0); }
        }

        public static int GetWlId(string wl)
        {
            return Types.ToInt(ConfigurationManager.AppSettings["wlid_" + wl], 0);
        }

        #region uploads

        public static int UploadMaxSizeImages
        {
            get { return Types.ToInt(ConfigurationManager.AppSettings["UploadMaxSizeImages"], 512000); }
        }
        public static int UploadMaxSizeFiles
        {
            get { return Types.ToInt(ConfigurationManager.AppSettings["UploadMaxSizeFiles"], 716800); }
        }
        public static int UploadMaxSizeMedia
        {
            get { return Types.ToInt(ConfigurationManager.AppSettings["UploadMaxSizeMedia"], 716800); }
        }
        public static int UploadMaxSizeTemplate
        {
            get { return Types.ToInt(ConfigurationManager.AppSettings["UploadMaxSizeTemplate"], 716800); }
        }
        public static int UploadMaxSizeMobileImage
        {
            get { return Types.ToInt(ConfigurationManager.AppSettings["UploadMaxSizeMobileImage"], 512000); }
        }
        public static int UploadMaxSizeMobileVideo
        {
            get { return Types.ToInt(ConfigurationManager.AppSettings["UploadMaxSizeMobileVideo"], 716800); }
        }
        public static int UploadMaxSizeMobileTemplate
        {
            get { return Types.ToInt(ConfigurationManager.AppSettings["UploadMaxSizeMobileTemplate"], 716800); }
        }
        #endregion
    }
}