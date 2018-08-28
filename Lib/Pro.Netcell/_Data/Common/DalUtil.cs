using System;
using System.Collections.Generic;
using System.Text;
using Nistec.Data.Factory;
using System.Data.SqlClient;
using Nistec;

namespace Netcell.Data.Common
{

    public static class DalUtil
    {
        public const string MobilePattern = @"^(05(0|[2-9])(|-)[0-9]{7}|(|972)5(0|[2-9])[0-9]{7})$";
        public const string PhonePattern = @"(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$";
        public const string PhoneFreePattern = @"(|\()(18|17)00(|[\)\/\.-])[0-9]{3}(|[\)\/\.-])[0-9]{3}$";
        public const string PhoneAllPattern = @"(|\()(0|1|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$";
        public const string ShortCodePattern = @"^[0-9]{4}$";

        public const string DefaultMailCharset = "windows-1255";

        public static string GetValidValue(string value, string defaultValue, string regex)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            if (Nistec.Regx.RegexValidateIgnoreCase(regex, value))
                return value;
            return defaultValue;
        }

        public static string GetValidValue(string value, string defaultValue)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            return value;
        }

        public static string GetValidStringDate(string date, string defaultValue)
        {
            if (string.IsNullOrEmpty(date))
                return defaultValue;
            if (Nistec.Regx.RegexValidateIgnoreCase(@"^\d{2}(/|\.|-)\d{2}(/|\.|-)\d{4}\z", date))
                return date;
            return defaultValue;
        }

        public static bool[] ArgsToBitArray(string args)
        {
            string cleanArgs = args.Replace(" ", "");
            bool[] bargs = new bool[cleanArgs.Length];
            int i = 0;
            foreach (char c in cleanArgs.ToCharArray())
            {
                bargs[i] = (c == '1') ? true : false;
                i++;
            }
            return bargs;
        }

        public static string DateTimeToIndexQuery(string field, DateTime dateFrom, DateTime dateTo)
        {
            return string.Format("{0} between {1} and {2}", field, Types.DateTimeToInt(dateFrom), Types.DateTimeToInt(dateTo));
        }

        public static string GetMonthSplit(string field, string valueField, string prefix)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("COALESCE (SUM(CASE WHEN month({0}) =1 THEN {1} END), 0) AS {2}1,", field, valueField, prefix);
            sb.AppendFormat("COALESCE (SUM(CASE WHEN Month({0}) =2 THEN {1} END), 0) AS {2}2, ", field, valueField, prefix);
            sb.AppendFormat("COALESCE (SUM(CASE WHEN Month({0}) =3 THEN {1} END), 0) AS {2}3, ", field, valueField, prefix);
            sb.AppendFormat("COALESCE (SUM(CASE WHEN Month({0}) =4 THEN {1} END), 0) AS {2}4,", field, valueField, prefix);
            sb.AppendFormat("COALESCE (SUM(CASE WHEN month({0}) =5 THEN {1} END), 0) AS {2}5, ", field, valueField, prefix);
            sb.AppendFormat("COALESCE (SUM(CASE WHEN Month({0}) =6 THEN {1} END), 0) AS {2}6, ", field, valueField, prefix);
            sb.AppendFormat("COALESCE (SUM(CASE WHEN Month({0}) =7 THEN {1} END), 0) AS {2}7, ", field, valueField, prefix);
            sb.AppendFormat("COALESCE (SUM(CASE WHEN Month({0}) =8 THEN {1} END), 0) AS {2}8,", field, valueField, prefix);
            sb.AppendFormat("COALESCE (SUM(CASE WHEN month({0}) =9 THEN {1} END), 0) AS {2}9, ", field, valueField, prefix);
            sb.AppendFormat("COALESCE (SUM(CASE WHEN Month({0}) =10 THEN {1} END), 0) AS {2}10, ", field, valueField, prefix);
            sb.AppendFormat("COALESCE (SUM(CASE WHEN Month({0}) =11 THEN {1} END), 0) AS {2}11, ", field, valueField, prefix);
            sb.AppendFormat("COALESCE (SUM(CASE WHEN Month({0}) =12 THEN {1} END), 0) AS {2}12 ", field, valueField, prefix);
            return sb.ToString();
        }

        public static string GetQuarterSplit(string field, string valueField, string prefix)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("COALESCE (SUM(CASE WHEN month({0}) BETWEEN 1 AND 3 THEN {1} END), 0) AS {2}1,", field, valueField, prefix);
            sb.AppendFormat("COALESCE (SUM(CASE WHEN Month({0}) BETWEEN 4 AND 6 THEN {1} END), 0) AS {2}2,", field, valueField, prefix);
            sb.AppendFormat("COALESCE (SUM(CASE WHEN Month({0}) BETWEEN 7 AND 8 THEN {1} END), 0) AS {2}3,", field, valueField, prefix);
            sb.AppendFormat("COALESCE (SUM(CASE WHEN Month({0}) BETWEEN 9 AND 12 THEN {1} END), 0) AS {2}4 ", field, valueField, prefix);
            return sb.ToString();
        }

        public static SqlParameter[] CreateParameters(string[] keys, object[] values)
        {
            if(keys==null || values==null)
            {
                throw new ArgumentNullException("CreateParameters.Keys or Values");
            }
             if(keys.Length != values.Length)
            {
                throw new ArgumentException("CreateParameters.Keys or Values length not equal");
            }
            List<SqlParameter> parameters = new List<SqlParameter>();

            for (int i = 0; i < keys.Length; i++)
            {
                parameters.Add(new SqlParameter(keys[i],values[i]));
            }
            return parameters.ToArray();
        }
    }

  
}
