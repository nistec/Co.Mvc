using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


namespace Netcell
{

    /// <summary>
    /// Summary description for RM
    /// </summary>
    public class RM
    {
        #region static
        /*
        public static string GetHelpInfo(string name)
        {
            string text = null;// Resources.Dictionary.ResourceManager.GetString(name);
            if (text == null)
                return name;
            return text;
        }
        public static void ShowInfo(Page p, string caption, string key, params object[] args)
        {
            string msg = GetString(key, args);
            JS.ShowInfo(p, caption, msg, true);
        }
        public static void ShowInfo(Page p, string caption, string key)
        {
            string msg = GetString(key);
            JS.ShowInfo(p, caption, msg, true);
        }

        public static void ShowWarning(Page p, string caption, string key, params object[] args)
        {
            string msg = GetString(key, args);
            JS.ShowWarning(p, caption, msg, true);
        }
        public static void ShowWarning(Page p, string caption, string key)
        {
            string msg = GetString(key);
            JS.ShowWarning(p, caption, msg, true);
        }
        public static void ShowError(Page p, string caption, string key, params object[] args)
        {
            string msg = GetString(key, args);
            JS.ShowError(p, caption, msg);
        }
        public static void ShowError(Page p, string caption, string key)
        {
            string msg = GetString(key);
            JS.ShowError(p, caption, msg);
        }
        */

        public static string GetString(string key, params object[] args)
        {
            //string lang = "he";
            //object o= HttpContext.GetLocalResourceObject("Dictionary.resx", key, new System.Globalization.CultureInfo(lang));
            try
            {
                object o = HttpContext.GetGlobalResourceObject("Dictionary", key);

                if (o == null)
                    return string.Format(o.ToString(), args);

                return string.Format(o.ToString(), args);
            }
            catch
            {
                return key;
            }
        }

        public static string GetString(string key)
        {
            try
            {
                //string lang = "he";
                //object o= HttpContext.GetLocalResourceObject("Dictionary.resx", key, new System.Globalization.CultureInfo(lang));
                object o = HttpContext.GetGlobalResourceObject("Dictionary", key);

                //object o = HttpContext.GetLocalResourceObject("lang", key);

                if (o == null)
                    return key;

                return o.ToString();
            }
            catch
            {
                return key;
            }
        }

        public static string GetLocalString(string key, params object[] args)
        {
            try
            {
                //string lang = "he";
                //object o= HttpContext.GetLocalResourceObject("Dictionary.resx", key, new System.Globalization.CultureInfo(lang));

                object o = HttpContext.GetLocalResourceObject("Lang.resex", key);

                if (o == null)
                    return string.Format(o.ToString(), args);

                return string.Format(o.ToString(), args);
            }
            catch
            {
                return key;
            }
        }

        public static string GetLocalString(string key)
        {
            try
            {
                //string lang = "he";
                //object o= HttpContext.GetLocalResourceObject("Dictionary.resx", key, new System.Globalization.CultureInfo(lang));
                object o = HttpContext.GetLocalResourceObject("Lang.resex", key);

                //object o = HttpContext.GetLocalResourceObject("lang", key);

                if (o == null)
                    return key;

                return o.ToString();
            }
            catch
            {
                return key;
            }
        }
        #endregion

        #region const label

        public const string MainDomain = "my-t.co.il";
        public const string FileTypeNotSupported = "FileTypeNotSupported";
        public const string IvalidFile = "IvalidFile";
        public const string NotEnoughCredit = "NotEnoughCredit";
        public const string UploadResult = "ValidNumbers:{0},InavlidNumbers:{1},DuplicateNumbers:{2}";

        public const string CreditError = "CreditError";
        public const string DuplicateTargets = "DuplicateTargets";
        public const string EmptyGroupTargets = "EmptyGroupTargets";
        public const string AccessDenied = "Access denied!!!";

        //public const string SiteName = "EPHONE";
        //public const string CookiName = "EPHONE";
        public const string CompanyName = "My-T Interactive";

        #endregion

        #region const messages
        //logoff
        public const string logoff_evaluated = "תקופת הנסיון הסתיימה, אנא פנה לתמיכה";
        public const string logoff_logout = "עקב חוסר פעילות נותקת מהמערכת   <br/><br/>";
        public const string logoff_relog="כניסה מחודשת";
        public const string logoff_thanks_a_site_company = "Thank You For Using {0} <br/><br/> {1}";

        public const string registration_title_a_site = "Message from {0} registration";

        #endregion


        public static string FormatString(string strconst,params object[] args)
        {
            return string.Format(strconst,args);
        }

        public static string FormatStringSite(string strconst)
        {
            return string.Format(strconst, WebConfig.SiteName);
        }
    }

}