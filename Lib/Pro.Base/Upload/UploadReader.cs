using Nistec;
using Nistec.Data;
using Nistec.Data.Factory;
using Nistec.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pro.Upload
{
    public enum ContactRule
    {
        None = 0,
        Cell = 1,
        Mail = 2,
        Both = 3
    }

    public static class UploadReader
    {
        public const string MobilePattern = @"^(05(0|[2-9])(|-)[0-9]{7}|(|972)5(0|[2-9])[0-9]{7})$";
        public const string PhonePattern = @"(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$";
        public const string PhoneFreePattern = @"(|\()(18|17)00(|[\)\/\.-])[0-9]{3}(|[\)\/\.-])[0-9]{3}$";
        public const string PhoneAllPattern = @"(|\()(0|1|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$";

        public static string EnsureLocalCli(string CellNumber, string defaultValue,int maxLength=10)
        {
            if (string.IsNullOrEmpty(CellNumber))
                return defaultValue;
            string cli = Regx.RegexReplace(CellNumber.Trim(), "[^0-9]", "");  
            cli = Regx.RegexReplace("^972", cli, "0");
            cli = Regx.RegexReplace("[^0-9]", cli, "");
            if (Regx.RegexValidate("^[1-9]", cli))
                cli = "0" + cli;
            if (cli.Length > maxLength)
                return defaultValue;
            return cli;
        }
        public static string EnsureEmail(string mail, string defaultValue)
        {
            if (string.IsNullOrEmpty(mail))
                return defaultValue;
            mail=mail.Trim();
            return (Nistec.Regx.IsEmail(mail)) ? mail : defaultValue;
        }
        public static ContactRule GetContactRule(string CellNumber, string Email)
        {
            bool isCli = ValidateMobile(CellNumber);
            bool isMail = ValidateEmail(Email);
            if (isCli && isMail)
            {
                return ContactRule.Both;
            }
            if (isCli)
            {
                return ContactRule.Cell;
            }
            if (isMail)
            {
                return ContactRule.Mail;
            }
            return ContactRule.None;
        }

        public static ContactRule ValidateContactRule(ref string CellNumber, ref  string Email)
        {
            if (CellNumber == null)
                CellNumber = "*";
            if (Email == null)
                Email = "*";

            bool isCli = ValidateMobile(CellNumber);
            bool isMail = ValidateEmail(Email);
            if (isCli && isMail)
            {
                CellNumber = EnsureLocalCli(CellNumber,"*");
                return ContactRule.Both;
            }
            if (isCli)
            {
                CellNumber = EnsureLocalCli(CellNumber, "*");
                Email = "*";
                return ContactRule.Cell;
            }
            if (isMail)
            {
                CellNumber = "*";
                return ContactRule.Mail;
            }
            return ContactRule.None;
        }


        //public static bool ValidatePhone(string phone, bool checkFree)
        //{
        //    bool ok = ValidatePhone(phone);
        //    if (!ok && checkFree)
        //        return ValidatePhoneFree(phone);
        //    return ok;
        //}
        public static bool ValidateMobile(string phone)
        {
            if (phone == null)
                return false;
            return Nistec.Regx.RegexValidateIgnoreCase(MobilePattern, phone);
        }
        public static bool ValidatePhoneAll(string phone)
        {
            if (phone == null)
                return false;
            return Nistec.Regx.RegexValidateIgnoreCase(PhoneAllPattern, phone);
        }
        public static bool ValidatePhone(string phone)
        {
            if (phone == null)
                return false;
            return Nistec.Regx.RegexValidateIgnoreCase(PhonePattern, phone);
        }
        public static bool ValidatePhoneFree(string phone)
        {
            if (phone == null)
                return false;
            return Nistec.Regx.RegexValidateIgnoreCase(PhoneFreePattern, phone);
        }
        public static bool ValidateEmail(string mail)
        {
            if (mail == null)
                return false;
            return Nistec.Regx.IsEmail(mail);
        }

        public static string GetIdentifier(int exType, int accountId, string memberId, string cellNumber, string email, string exId)
        {
            //0=MemberId,1=@CellNumber,2=@Email,3=@ExId

            switch (exType)
            {
                case 0://MemberId
                    if (string.IsNullOrEmpty(memberId))
                        throw new Exception("Invalid identifier (member ID)");
                    return string.Format("{0}_{1}_{2}", accountId, exType, memberId);
                case 1://CellNumber
                    if (string.IsNullOrEmpty(memberId))
                        throw new Exception("Invalid identifier (cell Number)");
                    return string.Format("{0}_{1}_{2}", accountId, exType, cellNumber);
                case 2://Email
                    if (string.IsNullOrEmpty(memberId))
                        throw new Exception("Invalid identifier (email)");
                    return string.Format("{0}_{1}_{2}", accountId, exType, email);
                case 3://ExId
                    if (string.IsNullOrEmpty(exId))
                        throw new Exception("Invalid identifier (ex ID)");
                    return string.Format("{0}_{1}_{2}", accountId, exType, exId);
                default:
                    throw new Exception("Identifier key type not supported " + exType.ToString());
            }
        }

        public static int ReadValidLookupField(DataRow dr, string field, int defaultValue, Dictionary<string, int> list)
        {
            try
            {

                string name = dr.Get<string>(field);
                if (name == null)
                    return defaultValue;
                if (Regx.IsInteger(name))
                    return Types.ToInt(name);
                var item = list.Where(k => k.Key == name);
                if (item == null)
                    return defaultValue;
                return item.FirstOrDefault().Value;
            }
            catch
            {
                return defaultValue;
            }
        }
        public static int ReadLookupField(DataRow dr, string field, int defaultValue, Dictionary<string,int> list)
        {
            try
            {
                
                if (!dr.Table.Columns.Contains(field))
                    return defaultValue;
                string name = dr.Get<string>(field);
                if (name == null)
                    return defaultValue;
                if (Regx.IsInteger(name))
                    return Types.ToInt(name);                
                var item = list.Where(k => k.Key == name);
                if (item == null)
                    return defaultValue;
                return item.FirstOrDefault().Value;
            }
            catch
            {
                return defaultValue;
            }
        }
        public static string ReadValidPhoneField(DataRow dr, string field, string defaultValue)
        {
            try
            {
                string o = dr.Get<string>(field, defaultValue);
                if (o == defaultValue)
                    return defaultValue;
                if (ValidatePhoneAll(o))
                    return o;
                return defaultValue;

            }
            catch
            {
                return defaultValue;
            }
        }
        public static string ReadPhoneField(DataRow dr, string field, string defaultValue)
        {
            try
            {
                if (!dr.Table.Columns.Contains(field))
                    return defaultValue;
                string o = dr.Get<string>(field, defaultValue);
                if (o == defaultValue)
                    return defaultValue;
                if (ValidatePhoneAll(o))
                    return o;
                return defaultValue;

            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ReadValidMobileField(DataRow dr, string field, string defaultValue)
        {
            try
            {
                string o = dr.Get<string>(field, defaultValue);
                if (o == defaultValue)
                    return defaultValue;
                return EnsureLocalCli(o, defaultValue, 10);

                //if (ValidateMobile(o))
                //    return o;
                //return defaultValue;

            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ReadMobileField(DataRow dr, string field, string defaultValue)
        {
            try
            {
                if (!dr.Table.Columns.Contains(field))
                    return defaultValue;
                string o = dr.Get<string>(field, defaultValue);
                if (o == defaultValue)
                    return defaultValue;
                return EnsureLocalCli(o, defaultValue, 10);

                //if (ValidateMobile(o))
                //    return o;
                //return defaultValue;

            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ReadValidEmailField(DataRow dr, string field, string defaultValue)
        {
            try
            {
                string o = dr.Get<string>(field, defaultValue);
                if (o == defaultValue)
                    return defaultValue;
                if (ValidateEmail(o))
                    return o;
                return defaultValue;

            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ReadEmailField(DataRow dr, string field, string defaultValue)
        {
            try
            {
                if (!dr.Table.Columns.Contains(field))
                    return defaultValue;
                string o = dr.Get<string>(field, defaultValue);
                if (o == defaultValue)
                    return defaultValue;
                if (ValidateEmail(o))
                    return o;
                return defaultValue;

            }
            catch
            {
                return defaultValue;
            }
        }
        public static object ReadValidStrDateField(IDictionary<string, object> dr, string field, object defaultValue)
        {
            try
            {
                object o = dr[field];
                if (o == null)
                    return defaultValue;
                string date = DateHelper.FormtDate(o.ToString());
                //string date = Types.FormatDate(o.ToString(), "dd/MM/yyyy", null);
                if (string.IsNullOrEmpty(date))
                {
                    double d = 0;
                    if (double.TryParse(o.ToString(), out d))
                    {
                        DateTime ndate = DateTime.FromOADate(Types.ToDouble(o, 0));
                        date = DateHelper.FormtDate(ndate);
                    }
                }
                return date;
            }
            catch
            {
                return defaultValue;
            }
        }
        public static object ReadValidStrDateField(DataRow dr, string field, object defaultValue)
        {
            try
            {
                object o = dr[field];
                if (o == null)
                    return defaultValue;
                string date = DateHelper.FormtDate(o.ToString());
                //string date = Types.FormatDate(o.ToString(), "dd/MM/yyyy", null);
                if (string.IsNullOrEmpty(date))
                {
                    double d = 0;
                    if (double.TryParse(o.ToString(), out d))
                    {
                        DateTime ndate = DateTime.FromOADate(Types.ToDouble(o, 0));
                        date = DateHelper.FormtDate(ndate);
                    }
                }
                return date;
            }
            catch
            {
                return defaultValue;
            }
        }
        public static object ReadStrDateField(DataRow dr, string field, object defaultValue)
        {
            try
            {
                if (!dr.Table.Columns.Contains(field))
                    return defaultValue;
                object o = dr[field];
                if (o == null)
                    return defaultValue;
                string date = DateHelper.FormtDate(o.ToString());
                //string date = Types.FormatDate(o.ToString(), "dd/MM/yyyy", null);
                if (string.IsNullOrEmpty(date))
                {
                    double d = 0;
                    if (double.TryParse(o.ToString(), out d))
                    {
                        DateTime ndate = DateTime.FromOADate(Types.ToDouble(o, 0));
                        date = DateHelper.FormtDate(ndate);
                    }
                }
                return date;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static DateTime? ReadNullableDateField(IDictionary<string, object> dr, string field, DateTime? defaultValue)
        {
            try
            {
                object o = dr[field];
                if (o == null)
                    return  defaultValue;
                return DateHelper.ToNullableDateTime(o.ToString(), defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }
        public static DateTime? ReadNullableDateField(DataRow dr, string field, DateTime? defaultValue)
        {
            try
            {
                object o = dr[field];
                if (o == null)
                    return defaultValue;
                return DateHelper.ToNullableDateTime(o.ToString(), defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static DateTime ReadValidDateField(IDictionary<string, object> dr, string field, DateTime defaultValue)
        {
            try
            {
                object o = dr[field];
                if (o == null)
                    return defaultValue;
                return DateHelper.ToDateTime(o.ToString(), defaultValue);

                //return Types.ToDateTime(o.ToString(), defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }
        public static DateTime ReadValidDateField(DataRow dr, string field, DateTime defaultValue)
        {
            try
            {
                object o = dr[field];
                if (o == null)
                    return defaultValue;
                return DateHelper.ToDateTime(o.ToString(), defaultValue);

                //return Types.ToDateTime(o.ToString(), defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }
        public static DateTime ReadDateField(DataRow dr, string field, DateTime defaultValue)
        {
            try
            {
                if (!dr.Table.Columns.Contains(field))
                    return defaultValue;
                object o = dr[field];
                if (o == null)
                    return defaultValue;
                return DateHelper.ToDateTime(o.ToString(), defaultValue);

                //return Types.ToDateTime(o.ToString(), defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ReadCommaListField(DataRow dr, string field, string defaultValue, int maxLength)
        {
            try
            {
                if (dr.Table.Columns.Contains(field))
                {
                    string value = dr.Get<string>(field, defaultValue);
                    if (string.IsNullOrEmpty(value))
                        return defaultValue;
                    if (value.Length > maxLength)
                        return defaultValue;
                    value = value.Replace(" ","").TrimEnd(',').TrimStart(',');
                    if (Regx.RegexValidateIgnoreCase(@"^[0-9,]+$", value))
                        return value;
                }
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }


        public static object ReadValidField(IDictionary<string, object> dr, string field, object defaultValue)
        {
            try
            {
                    return Types.NZ(dr[field], defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }
        public static object ReadValidField(DataRow dr, string field, object defaultValue)
        {
            try
            {
                    return Types.NZ(dr[field], defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }


        public static object ReadField(DataRow dr, string field, object defaultValue)
        {
            try
            {
                if (dr.Table.Columns.Contains(field))
                    return Types.NZ(dr[field], defaultValue);
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ReadValidTextField(IDictionary<string, object> dr, string field, string defaultValue, int maxLength)
        {
            try
            {
                string value = Types.NZ(dr[field], defaultValue);
                if (value.Length > maxLength)
                    return defaultValue;
                return value;
            }
            catch
            {
                return defaultValue;
            }
        }
        public static string ReadValidTextField(DataRow dr, string field, string defaultValue, int maxLength)
        {
            try
            {
                string value = dr.Get<string>(field, defaultValue);
                if (value.Length > maxLength)
                    return defaultValue;
                return value;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ReadTextField(DataRow dr, string field, string defaultValue, int maxLength)
        {
            try
            {
                if (dr.Table.Columns.Contains(field))
                {
                    string value = dr.Get<string>(field, defaultValue);
                    if (value.Length > maxLength)
                        return defaultValue;
                    return value;
                }
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ReadValidSexField(DataRow dr, string field, string defaultValue)
        {
            try
            {
                return ReadSexField(dr[field], defaultValue);

            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ReadSexField(DataRow dr, string field, string defaultValue)
        {
            try
            {
                if (!dr.Table.Columns.Contains(field))
                    return defaultValue;// "U";
                return ReadSexField(dr[field], defaultValue);

            }
            catch
            {
                return defaultValue;
            }
        }

        public static object ReadDateField(object value, object defaultValue)
        {
            try
            {
                if (value == null)
                    return defaultValue;
                return Types.FormatDate(value.ToString(), "d", null);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ReadSexField(object value, string defaultValue)
        {
            try
            {
                if (value == null)
                    return defaultValue;
                string val = Types.NZ(value, "");
                switch (val)
                {
                    case "זכר":
                    case "ז":
                    case "m":
                    case "M":
                        return "M";
                    case "נקבה":
                    case "נ":
                    case "f":
                    case "F":
                        return "F";
                    default:
                        return "U";
                }

            }
            catch
            {
                return defaultValue;
            }
        }

        public static object ReadValidBoolField(IDictionary<string, object> dr, string field, bool defaultValue)
        {
            try
            {

                string value = Types.NZ(dr[field], "");
                switch (value.ToLower())
                {
                    case "כן":
                    case "yes":
                    case "true":
                    case "ok":
                    case "on":
                        return true;
                    default:
                        return false;
                }

                //return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
        public static object ReadValidBoolField(DataRow dr, string field, bool defaultValue)
        {
            try
            {
 
                    string val = Types.NZ(dr[field], "");
                    switch (val.ToLower())
                    {
                        case "כן":
                        case "yes":
                        case "true":
                        case "ok":
                        case "on":
                            return true;
                        default:
                            return false;
                    }
            }
            catch
            {
                return defaultValue;
            }
        }

        public static object ReadBoolField(DataRow dr, string field, bool defaultValue)
        {
            try
            {
                if (dr.Table.Columns.Contains(field))
                {
                    string val = Types.NZ(dr[field], "");
                    switch (val.ToLower())
                    {
                        case "כן":
                        case "yes":
                        case "true":
                        case "ok":
                        case "on":
                            return true;
                        default:
                            return false;
                    }
                }

                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
        public static Dictionary<string, int> ReadLookup(IDbCmd cmd, string mappingName, string keyFieldName, string valueFieldName)
        {
            var dt = cmd.ExecuteDataTable(mappingName);
            return dt.ToDictionary<string, int>(keyFieldName, valueFieldName);
        }
        public static Dictionary<string, int> ReadLookup(IDbCmd cmd, string mappingName, string filter, string keyFieldName, string valueFieldName)
        {
            if (filter != null || filter != "")
                filter = " where " + filter;
            string cmdText = string.Format("select [{1}],[{2}] from [{0}]{3}", mappingName, keyFieldName, valueFieldName, filter);
            var dt = cmd.ExecuteDataTable(mappingName, cmdText, false);
            return dt.ToDictionary<string, int>(keyFieldName, valueFieldName);
        }
       
    }
   
}
