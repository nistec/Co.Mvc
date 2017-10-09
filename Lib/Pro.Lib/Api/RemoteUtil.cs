using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using Nistec;

namespace Pro.Lib.Api
{
   
    
    public static class RemoteUtil
    {
        public const int ConcatLimit = 0x42;
        public const string Version = "Netcell 4.1";

        public static System.Globalization.DateTimeFormatInfo DateFormat
        {
            //get{return new System.Globalization.DateTimeFormatInfo;}
            get { return new System.Globalization.CultureInfo("he-IL", false).DateTimeFormat; }
        }

       
        public static string FormatMessage(string message, string personal)
        {
            string str2;
            if (string.IsNullOrEmpty(message))
            {
                return message;
            }
            if (string.IsNullOrEmpty(personal))
            {
                return message;
            }
            string str = message;
            string[] strArray = SplitToArray(personal, ';', true, true, new string[] { " " });
            if (strArray.Length <= 0)
            {
                return message;
            }
            try
            {
                str2 = string.Format(message, (object[])strArray);
            }
            catch
            {
                throw new ApiException(AckStatus.ArgumentException, "Message not match to personal attributes");
            }
            return str2;
        }

        public static string FormatMessage(string message, string[] personal)
        {
            string str;
            if (string.IsNullOrEmpty(message))
            {
                return message;
            }
            if ((personal == null) || (personal.Length == 0))
            {
                return message;
            }
            try
            {
                str = string.Format(message, (object[])personal);
            }
            catch
            {
                throw new AppException(AckStatus.ArgumentException, "Message not match to personal attributes");
            }
            return str;
        }
        

        public static string BuildMessage(string message, string personal, string[] args)
        {
            string str2;
            if (string.IsNullOrEmpty(message))
            {
                return message;
            }
            if (string.IsNullOrEmpty(personal))
            {
                return message;
            }
            if ((args == null) || (args.Length == 0))
            {
                if (HasPersonalRemark(message, true, false))
                {
                    return FormatMessage(message, personal);
                }
                return message;
            }
            string str = message;
            string[] strArray = SplitToArray(personal, ';', true, true, new string[] { " " });
            if (strArray.Length < args.Length)
            {
                return message;
            }
            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (!string.IsNullOrEmpty(args[i]))
                    {
                        message = message.Replace(args[i], strArray[i]);
                    }
                }
                if (HasPersonalRemark(message, true, false))
                {
                    return FormatMessage(message, strArray);
                }
                str2 = message;
            }
            catch
            {
                throw new AppException(AckStatus.ArgumentException, "Message not match to personal attributes args");
            }
            return str2;
        }

        public static string BuildMessage(string message, string personal, string personalDisplay)
        {
            if (string.IsNullOrEmpty(personalDisplay))
            {
                return message;
            }
            return BuildMessage(message, personal, SplitToArray(personalDisplay, ';', true, true, null));
        }

        public static string BuildSmsCBQs(string User, string Password, string Sender, string Data, string Target)
        {
            return string.Format("Method=SMSMT&Billing=CB&User={0}&Password={1}&Sender={2}&Data={3}&Target={4}", new object[] { User, Password, Sender, Data, Target });
        }

        
        public static string NormelaizeXml(string xml)
        {
             Regex regex = new Regex(@">\s*<");
             xml = regex.Replace(xml, "><");
            return xml.Replace("\r\n", "").Replace("\n","").Trim();
        }

        public static bool ParseKeyValue(string msg, out string key, out string value)
        {
            key = msg;
            value = msg;
            try
            {
                const string pattern = @"^\s*(?<Keyword>\w+)\s*(?<Value>.*)";
                Regex regex = new Regex(pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
                // Replace invalid characters with empty strings.
                //string cleanText= regex.Replace(Txt, @"[^\w\.@-]", ""); 

                Match m = regex.Match(msg);

                if (m.Success)
                {
                    key = m.Groups[1].Value;
                    value = m.Groups[2].Value;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static int GetPriority(string queue)
        {
            if (!string.IsNullOrEmpty(queue))
            {
                if (queue.Contains("_Quick"))
                {
                    return 1;// Priority.Medium;
                }
                if (queue.Contains("_MO"))
                {
                    return 1;// Priority.Medium;
                }
            }
            return 0;
        }

        public static int GetPriority(string Method, string billingType)
        {
            if (Method == "SMSPS")
            {
                return 2;// Priority.High;
            }
            if (MsgMethod.IsMO(Method))
            {
                return 2;// Priority.High;
            }
            if (billingType == "RB")
            {
                return 1;// Priority.Medium;
            }
            return 0;// Priority.Normal;
        }

        public static string GetRandom(int length)
        {
            Random random = new Random();
            string str = random.Next(0x5f5e100, 0x3b9ac9ff).ToString();
            if ((length > 2) && (length < 9))
            {
                return str.Substring(0, length);
            }
            return str;
        }

        public static bool HasPersonalRemark(string content, bool format, bool fields)
        {
            if (format && fields)
            {
                return Regx.RegexValidateIgnoreCase(@"({\d}|\[.*\])", content);
            }
            if (format)
            {
                return Regx.RegexValidateIgnoreCase(@"{\d}", content);
            }
            return (fields && Regx.RegexValidateIgnoreCase(@"\[.*\]", content));
        }

        public static bool IsAlphaNumeric(params string[] expression)
        {
            Regex regex = new Regex("^[a-zA-Z0-9]+$");
            foreach (string str in expression)
            {
                if (!regex.Match(str).Success)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool IsValidCliSender(string sender)
        {
            bool ok = CLI.ValidatePhone(sender, true);
            if (!ok)
                ok = Regx.RegexValidateIgnoreCase(CLI.PhoneAllPattern, sender);
            return ok;
        }
        public static bool IsDateTime(params string[] expression)
        {
            Regex regex = new Regex(@"^((\d{1,2})[\s\.\/-](\d{1,2})[\s\.\/-](\d{4})[\s]|(\d{4})[\-](\d{1,2})[\-](\d{1,2})[\sT])(\d{1,2})[\:](\d{1,2})$");
            foreach (string str in expression)
            {
                if (!regex.Match(str).Success)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsLatin(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            foreach (byte num in bytes)
            {
                if (num > 0x7f)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsPending(string time)
        {
            return string.IsNullOrEmpty(time) ? false : (Types.ToDateTime(time, DateTime.Now) > DateTime.Now.AddMinutes(3.0));
        }
        public static bool IsPending(DateTime? time)
        {
            return time == null ? false : (time.Value > DateTime.Now.AddMinutes(3.0));
        }
        public static bool IsSqlInjectionExists(string valueToCheck)
        {
            bool flag = true;
            flag &= Regex.Match(valueToCheck, @"/(\%27)|(\')|(\-\-)|(\%23)|(#)/ix").Success;
            flag &= Regex.Match(valueToCheck, @"/((\%3D)|(=))[^\n]*((\%27)|(\')|(\-\-)|(\%3B)|(;))/i").Success;
            flag &= Regex.Match(valueToCheck, @"/\w*((\%27)|(\'))((\%6F)|o|(\%4F))((\%72)|r|(\%52))/ix ").Success;
            flag &= Regex.Match(valueToCheck, @"/((\%27)|(\'))union/ix").Success;
            return (flag & Regex.Match(valueToCheck, @"exec(\s|\+)+(s|x)p\w+/ix").Success);
        }

        public static bool IsValidString(string s)
        {
            if (s.IndexOfAny(new char[] { 
                '[', ']', '(', ')', '{', '}', '|', '<', '>', '!', '=', ';', ':', '&', '?', '*', 
                '%', '&', '+', ' ', '\''
             }) > 0)
            {
                return false;
            }
            if (s.ToLower().Contains("delete"))
            {
                return false;
            }
            if (s.ToLower().Contains("insert"))
            {
                return false;
            }
            if (s.ToLower().Contains("select"))
            {
                return false;
            }
            if (s.ToLower().Contains("from"))
            {
                return false;
            }
            if (s.ToLower().Contains("script"))
            {
                return false;
            }
            return true;
        }

        public static bool RegexMatch(string pattern, string expression)
        {
            Regex regex = new Regex(pattern);
            return regex.Match(expression).Success;
        }

        public static bool RegexMatch(string pattern, params string[] expression)
        {
            Regex regex = new Regex(pattern);
            foreach (string str in expression)
            {
                if (!regex.Match(str).Success)
                {
                    return false;
                }
            }
            return true;
        }

        public static string[] SplitToArray(string s, char spliter)
        {
            if (s == null)
            {
                return null;
            }
            return s.Split(new char[] { spliter });
        }

        public static string[] SplitToArray(string s, char spliter, bool trimStart, bool trimEnd, params string[] strIfEmpty)
        {
            if (string.IsNullOrEmpty(s))
            {
                return strIfEmpty;
            }
            if (trimStart)
            {
                s = s.TrimStart(new char[] { spliter });
            }
            if (trimEnd)
            {
                s = s.TrimEnd(new char[] { spliter });
            }
            return s.Split(new char[] { spliter });
        }

        public static Dictionary<string, string> SplitToDictionary(string s, char spliter, char innerspliter)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (s != null)
            {
                string[] strArray = SplitToArray(s.TrimEnd(new char[] { spliter }), spliter);
                foreach (string str in strArray)
                {
                    string[] strArray2 = SplitToArray(str, innerspliter);
                    if ((strArray2 != null) && (strArray2.Length == 2))
                    {
                        dictionary[strArray2[0]] = strArray2[1];
                    }
                }
            }
            return dictionary;
        }


    }

}
