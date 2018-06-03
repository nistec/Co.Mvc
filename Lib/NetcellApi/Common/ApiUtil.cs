using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Netcell.Remoting;
using Nistec;
using Nistec.Generic;

namespace Netcell
{
  
    public class ApiUtil
    {
        public static DateTime NullDate = new DateTime(1900, 1, 1);
        public static DateTime MaxDate = new DateTime(2900, 12, 31);

        public static string FormatString(string s, int length)
        {
            if (s == null || s.Length < length)
                return s;
            return s.Substring(0, length);
        }

        public static string FixDateString(string date)
        {

            if (string.IsNullOrEmpty(date))
                return null;

            if (date.Length < 8)
                return null;

            string[] args = date.Split('/', '.', '-');
            if (args.Length < 3)
                return null;

            int day = Types.ToInt(args[0], 0);
            int month = Types.ToInt(args[1], 0);
            int year = Types.ToInt(args[2], 0);

            if (day <= 0 || day > 31)
                return null;
            if (month <= 0 || month > 12)
                return null;
            if (year < 1900 || year > 2900)
                return null;

            string d = (day < 10 ? "0" : "") + day.ToString();
            string m = (month < 10 ? "0" : "") + month.ToString();

            return string.Format("{0}/{1}/{2}", d, m, year);


        }

     

        public static CampaignSendType ResolveCampaignSendType(int type)
        {
            if (type == 10)
                return CampaignSendType.Fixed;
            else if (type == 11)
                return CampaignSendType.Manual;
            return (CampaignSendType)type;

        }

        public static bool[] ArgsToBool(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            bool[] b = new bool[value.Length];
            int i = 0;
            foreach (char ch in value.ToCharArray())
            {
                int c=Types.ToInt(ch.ToString(),0);
                b[i]=c>0;
                i++;
            }
            return b;
        }
       
        public static string BoolToArgs(bool[] b, char splitter)
        {
            if (b == null || b.Length == 0)
                return null;
            List<bool> list = new List<bool>(b);
            StringBuilder sb = new StringBuilder();
            foreach (bool i in b)
            {
                sb.AppendFormat("{0}{1}", i ? 1 : 0, splitter);
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        public static int[] ArgsToInt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            int[] b = new int[value.Length];
            int i = 0;
            foreach (char ch in value.ToCharArray())
            {
                b[i] = Types.ToInt(ch.ToString(), 0);
                i++;
            }
            return b;
        }
        public static List<int> StrArrayToInt(string[] strArray, int minItems, int maxItems)
        {
            if (strArray == null || strArray.Length < minItems)
            {
                throw new Exception("strArray is less then  minItems");
            }
            List<int> result = new List<int>();
            int i = 0;
            foreach (string s in strArray)
            {
                if (i >= maxItems)
                    break;
                result.Add(Types.ToInt(s, 0));
            }

            return result;
        }
        public static int CharArrayToInt(char[] charArray)
        {
            const int maxItems=9;
            if (charArray == null || charArray.Length > maxItems)
            {
                throw new Exception("charArray length is greater then max bits");
            }
            string bitsStr = "1";
            foreach (char c in charArray)
            {
                bitsStr += c.ToString();
            }
            for (int i = 9; i > charArray.Length; i--)
            {
                bitsStr += "0";
            }
            return Convert.ToInt32(bitsStr);
        }

        public static string CharArrayToString(char[] charArray)
        {
            if (charArray == null)
            {
                throw new ArgumentNullException("charArray");
            }
            StringBuilder sb = new StringBuilder();
            foreach (char c in charArray)
            {
               sb.Append( c.ToString());
            }
            return sb.ToString();
        }

        public static int CountChars(string value, char c)
        {
            int count = 0;
            foreach (char ch in value)
            {
                if (ch.Equals(c))
                {
                    count++;
                }
            }
            return count;
        }

        public static string GetValidValue(string value, string defaultValue, string regex)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            if (Regx.RegexValidateIgnoreCase(regex, value))
                return value;
            return defaultValue;
        }

        public static string GetValidStringDate(string date, string defaultValue)
        {
            if (string.IsNullOrEmpty(date))
                return defaultValue;
            if (Regx.RegexValidateIgnoreCase(@"^\d{2}(/|\.|-)\d{2}(/|\.|-)\d{4}\z", date))
                return date;
            return defaultValue;
        }

        public static string GetValidSender(string sender, string defaultValue)
        {
            if (string.IsNullOrEmpty(sender))
                return defaultValue;
            if (!ValidateSender(sender))
                return defaultValue;
            return sender;
        }

        public static bool ValidateSender(string sender)
        {
            bool ok = CLI.ValidatePhoneCallAll(sender);// CLI.ValidatePhone(sender, true);
            if (!ok)
            ok= Regx.RegexValidateIgnoreCase(CLI.ShortCodePattern, sender);
            return ok;
        }
        public static bool ValidateMailSender(string sender)
        {
            return Regx.RegexValidate("^[a-zA-Z0-9-_.]+$", sender);
        }
        public static bool ValidateUrl(string url)
        {
            string pattern=@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)? ";
           return Regx.RegexValidateIgnoreCase(pattern,url);
        }
        public static bool ValidateMailSubject(string subject)
        {
            return Regx.RegexValidate("^[a-zA-Z0-9\u0590-\u05FF-_,.()\\[\\]\\s]+$", subject);
        }
        public static bool ValidateMailDisplay(string display)
        {
            return Regx.RegexValidate("^[a-zA-Z0-9\u0590-\u05FF-_,.()\\s]+$", display);
        }

        /// <summary>
        /// Validate Url
        /// </summary>
        /// <param name="prefix">http:// or other types or combination (http|https|ftp)://</param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool ValidateUrl(string prefix, string url)
        {
            string pattern = prefix + @"([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)? ";
            return Regx.RegexValidateIgnoreCase(pattern, url);
        }
        public static bool ValidatePhoneCall(string link,bool checkFreeCall)
        {
            bool ok=ValidatePhoneCall(link);
            if(ok && checkFreeCall)
                return ValidatePhoneCallFree(link);
            return ok;
        }

        public static bool ValidatePhoneCall(string link)
        {
            string pattern = @"tel:(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$";
            return Regx.RegexValidateIgnoreCase(pattern, link);
        }
        public static bool ValidatePhoneCallFree(string link)
        {
            string pattern = @"tel:(|\()(18|17)00(|[\)\/\.-])[0-9]{3}(|[\)\/\.-])[0-9]{3}$";
            return Regx.RegexValidateIgnoreCase(pattern, link);
        }
        public static bool ValidatePhone(string cli)
        {
            string pattern = @"(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$";
            return Regx.RegexValidateIgnoreCase(pattern, cli);
        }
        public static bool ValidatePhoneFree(string cli)
        {
            string pattern = @"(|\()(18|17)00(|[\)\/\.-])[0-9]{3}(|[\)\/\.-])[0-9]{3}$";
            return Regx.RegexValidateIgnoreCase(pattern, cli);
        }
        public static string AckTrans(int transId, string status, string description)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(@"<?xml version='1.0' encoding='utf-8' ?>");
            sb.Append("<RESULT>");
            sb.AppendFormat("<TransId>{0}</TransId>", transId);
            //sb.AppendFormat("<Code>{0}</Code>", (int)status);
            sb.AppendFormat("<Status>{0}</Status>", status.ToString());
            sb.AppendFormat("<Description>{0}</Description>", description);
            sb.Append("</RESULT>");

            return sb.ToString();
        }
        public static string AckService(string Service, string status, string description)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(@"<?xml version='1.0' encoding='utf-8' ?>");
            sb.Append("<RESULT>");
            sb.AppendFormat("<Service>{0}</Service>", Service);
            //sb.AppendFormat("<Code>{0}</Code>", (int)status);
            sb.AppendFormat("<Status>{0}</Status>", status);
            sb.AppendFormat("<Description>{0}</Description>", description);
            sb.Append("</RESULT>");

            return sb.ToString();
        }

        public static bool ValidatePersonalMessage(string message, string personal)
        {
            if (string.IsNullOrEmpty(message))
            {
                return false;
            }
            if (string.IsNullOrEmpty(personal))
            {
                return false;
            }
            string result = message;
            string[] attrib = personal.TrimStart(';').TrimEnd(';').Split(';');
            int attribCount = attrib.Length;
            int remarkCount = PersonalRemarkCount(message);
            if (attribCount <= 0)
            {
                return false;
            }
            try
            {
                string s = string.Format(message, attrib);
                return true;
            }
            catch //(Exception ex)
            {
                return false;
            }
        }
        public static bool HasPersonalRemark(string content)
        {

            return Regx.RegexValidateIgnoreCase("({\\d}|\\[.*\\])", content);
           // return MControl.Regx.RegexValidateIgnoreCase("{[0-9]}", content) || (content.Contains("[") && content.Contains("]"));

        }
        public static int PersonalRemarkCount(string content)
        {

            return Regx.RegexMatchesCount("({\\d}|\\[.*\\])", content);
            //return MControl.Regx.RegexMatchesCount("{[0-9]}", content);

        }
        public static string CleanPersonalRemark(string content)
        {

            return Regx.RegexReplace("{\\d}", content, " ");

        }

  /*  
        /// <summary>
        /// GetColor
        /// </summary>
        /// <param name="color"></param>
        /// <param name="DefaultColor"></param>
        /// <returns></returns>
        public static System.Drawing.Color GetColor(string color, string DefaultColor)
        {
            if (string.IsNullOrEmpty(color))
            {
                if (string.IsNullOrEmpty(DefaultColor))
                    return System.Drawing.Color.Empty;
                color = DefaultColor;
            }
            if (color.StartsWith("#"))
                return MControl.Drawing.ColorUtils.StringToColor(color);
            if (char.IsNumber(color, 0))
                return MControl.Drawing.ColorUtils.StringToColor("#" + color);
            if (MControl.Drawing.ColorUtils.KnownColorNames.Contains(color))
            {
                return System.Drawing.Color.FromName(color);
            }
            return MControl.Drawing.ColorUtils.StringToColor("#" + color);
        }
*/
        public static DateTime GetExpiration(string date, int addMonth)
        {
            return Types.ToDateTime(date, DateTime.Now).AddMonths(addMonth);
        }

        #region formt date

        public static string FormatIntDate(int date)
        {
            string strdate = date.ToString();
            if (date == 0)
                return "";
            if (strdate.Length < 8)
                return "";
            return strdate.Substring(6, 2) + "/" + strdate.Substring(4, 2) + "/" + strdate.Substring(0, 4);
        }
        public static string FormatIntTime(int time, string defaultValue)
        {
            string strdate = time.ToString();
            if (time == 0)
                return defaultValue;
            if (strdate.Length == 3)
                return "0" + strdate.Substring(0, 1) + ":" + strdate.Substring(1, 2);
            if (strdate.Length < 4)
                return defaultValue;
            return strdate.Substring(0, 2) + ":" + strdate.Substring(2, 2);
        }
        public static DateTime IntDateToDate(int date, DateTime defaultValue)
        {
            string strdate = date.ToString();
            if (date == 0)
                return defaultValue;
            if (strdate.Length < 8)
                return defaultValue;
            strdate= strdate.Substring(0, 4) + "-" + strdate.Substring(4, 2) + "-" + strdate.Substring(6,2);
            return Types.ToDateTime(strdate, defaultValue);
        }

        public static int DateStringToInt(string strdate, bool isoformat,bool ismonthday)
        {
            //00/00/0000
            string intdate = "";
            if (string.IsNullOrEmpty(strdate))
                return 0;
            if (strdate.Length < 10)
                return 0;
            if (isoformat)
            {
                if (ismonthday)
                    intdate = strdate.Substring(6, 2) + strdate.Substring(8, 2);
                else
                    intdate = strdate.Substring(0, 4) + strdate.Substring(6, 2) + strdate.Substring(8, 2);
            }
            else
            {
                if (ismonthday)
                    intdate =  strdate.Substring(3, 2) + strdate.Substring(0, 2);
                else
                    intdate = strdate.Substring(6, 4) + strdate.Substring(3, 2) + strdate.Substring(0, 2);
            }
            return Types.ToInt(intdate, 0);
        }

        public static bool CompareMonthDay(string strdate,int month, int day, bool isoformat)
        {
            int intdate = DateStringToInt(strdate,isoformat,true);
            if (intdate == 0)
                return false;
            string str = month.ToString() + day.ToString();
            return intdate == Types.ToInt(str, 0);
        }
        #endregion

        public static int GetRandom(int max)
        {
            Random rand = new Random();
            return rand.Next(max);
        }

        public static string BuildPersonalList(string personalDisplay)
        {
            if (string.IsNullOrEmpty(personalDisplay))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            string[] list = personalDisplay.Split(';');

            if (list.Length > 0)
            {
                sb.Append("{");
                sb.AppendFormat("'{0}':'{0}',", "פרסונאלי");
                foreach (string s in list)
                {
                    sb.AppendFormat("'{0}':'{0}',", s);
                }
                if (sb.Length > 1)
                    sb.Remove(sb.Length - 1, 1);
                sb.Append("}");

            }
            return sb.ToString();

        }
/*
        public static NameValueArgs BuildPersonalListItems(string personalDisplay)
        {
            if (string.IsNullOrEmpty(personalDisplay))
            {
                return null;
            }
            //List<System.Web.UI.WebControls.ListItem> items = new List<System.Web.UI.WebControls.ListItem>();
            NameValueArgs items = new NameValueArgs();
            string[] list = personalDisplay.Split(';');

            if (list.Length > 0)
            {
                items.Add(new System.Web.UI.WebControls.ListItem("פרסונאלי", "פרסונאלי"));
                foreach (string s in list)
                {
                    items.Add(new System.Web.UI.WebControls.ListItem(s,s));
                }

            }
            return items.ToArray();

        }
*/
    }
}
