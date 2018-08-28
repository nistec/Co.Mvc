using System;
using System.Collections.Generic;
using System.Text;
using Nistec;

namespace Pro.Netcell.Api
{
    public struct CLI
    {
        #region members

 
        //public const string MobilePattern = @"^(05(0|2|4|7)(|-)[0-9]{7}|(|972)5(0|2|4|7)[0-9]{7})$";
        public const string MobilePattern = @"^(05(0|[2-9])(|-)[0-9]{7}|(|972)5(0|[2-9])[0-9]{7})$";
        public const string PhonePattern = @"(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$";
        public const string PhoneFreePattern = @"(|\()(18|17)00(|[\)\/\.-])[0-9]{3}(|[\)\/\.-])[0-9]{3}$";
        public const string PhoneAllPattern = @"(|\()(0|1|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$";
        public const string ShortCodePattern = @"^[0-9]{4}$";
        public const string ShortPattern = @"\*\d{4}$";

        /// <summary>
        /// OperatorId
        /// </summary>
        public readonly int OperatorId;
        /// <summary>
        /// Internal number like 527464292
        /// </summary>
        public readonly int Id;
        /// <summary>
        /// CellNumber like 972527464292
        /// </summary>
        public readonly string CellNumber;
        /// <summary>
        /// Country Code
        /// </summary>
        public readonly string CountryCode;
        /// <summary>
        /// Area Code
        /// </summary>
        public readonly string AreaCode;
        /// <summary>
        /// Is Valid cell number
        /// </summary>
        public readonly bool IsValid;
        /// <summary>
        /// Is International number
        /// </summary>
        public readonly bool IsInternational;
        /// <summary>
        /// Get the Local Number like 0527464292
        /// </summary>
        public string LocalNumber
        {
            get
            {
                if (!IsValid)
                    return string.Empty;
                return "0" + CellNumber.Substring(3, 9);
                //return AreaCode + Id; 
            }
        }

        /// <summary>
        /// Get the Number for search like 527464292
        /// </summary>
        public string SearchNumber
        {
            get
            {
                if (!IsValid)
                    return string.Empty;
                if (CellNumber.StartsWith("972"))
                    return CellNumber.Substring(3);

                return CellNumber.TrimStart('0').Replace("-","");

            }
        }

        #endregion

          /// <summary>
        /// CLI ctor
        /// </summary>
        /// <param name="phoneNumber"></param>
        public CLI(string phoneNumber)
        {
            CellNumber = FixNumber(phoneNumber);
            CountryCode = "972";
            AreaCode = "";
            IsInternational = false;
            OperatorId = GetDefaultOperator(CellNumber);
            Id = 0;
            IsValid = CellNumber != string.Empty;
        }

        /// <summary>
        /// CLI ctor
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="isInternational"></param>
        public CLI(string phoneNumber, bool isInternational)
        {
            OperatorId = -1;
            Id = 0;
            IsValid = false;

            //972524377192
            //string[] tokennumber = phoneNumber.Replace("+", "").Split('-');
            AreaCode = "";
            CountryCode = "";
            CellNumber = "";
            IsInternational = false;
            try
            {

                CellNumber = FixNumber(phoneNumber);
                if (string.IsNullOrEmpty(CellNumber))
                {
                    goto label_exit;
                }
               
                SetCountryAndAreaCode(CellNumber, ref CountryCode, ref AreaCode, ref Id, ref IsInternational);

                //CellId = CLI.GetCliId(fixnumber);
                //if (CellId > 0)
                //{
                //    CodeId = Types.ToInt(areacode,0);
                //    OperatorId = CLIRemote.GetOperatorId(CellId);// CLICache.CliList.GetOperatorId(CellId);
                //}

                //TODO MNP
                //OperatorId = CLIRemote.GetOperatorId(CellNumber);// CLICache.CliList.GetOperatorId(CellId);

                if (OperatorId <= 0)
                {
                    OperatorId = CLI.GetDefaultOperator(CellNumber);
                }

            label_exit:

                IsValid = OperatorId > 0;

                //CellNumber = fixnumber;
                //CountryCode = countrycode;
                //AreaCode = areacode;
                //IsInternational = isInternational;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        #region static methods

        public static bool ValidatePhone(string phone, bool checkFree)
        {
            bool ok = ValidatePhone(phone);
            if (!ok && checkFree)
                return ValidatePhoneFree(phone);
            return ok;
        }

        public static bool ValidatePhone(string phone)
        {
            return Regx.RegexValidateIgnoreCase(PhonePattern, phone);
        }
        public static bool ValidatePhoneFree(string phone)
        {
            return Regx.RegexValidateIgnoreCase(PhoneFreePattern, phone);
        }
        public static bool ValidatePhoneCall(string link, bool checkFreeCall)
        {
            bool ok = ValidatePhoneCall(link);
            if (!ok && checkFreeCall)
                return ValidatePhoneCallFree(link);
            return ok;
        }

        static string GetCliTel(string link)
        {
            if (!link.StartsWith("tel:"))
                return "tel:" + link;
            return link;
        }

        public static bool ValidatePhoneCall(string link)
        {
            string pattern = @"tel:(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$";
            return Regx.RegexValidateIgnoreCase(pattern, GetCliTel(link));
        }
        public static bool ValidatePhoneCallFree(string link)
        {
            string pattern = @"tel:(|\()(18|17)00(|[\)\/\.-])[0-9]{3}(|[\)\/\.-])[0-9]{3}$";
            return Regx.RegexValidateIgnoreCase(pattern, GetCliTel(link));
        }

        public static bool ValidatePhoneCallShort(string link)
        {
            string pattern = @"tel:\*\d{4}|(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$";
            return Regx.RegexValidateIgnoreCase(pattern, GetCliTel(link));
        }

        public static bool ValidatePhoneCallAll(string link)
        {
            string pattern = @"tel:\*\d{4}|(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})|(|\()(18|17)00(|[\)\/\.-])[0-9]{3}(|[\)\/\.-])[0-9]{3}$";
            return Regx.RegexValidateIgnoreCase(pattern, GetCliTel(link));
        }

        public static bool ValidateMobile(string cli)
        {
              return Regx.RegexValidateIgnoreCase(MobilePattern, cli);
        }
        public static string  CleanCli(string cli)
        {
            return Regx.RegexReplace(@"[^0-9]", cli, "");
        }

     
        #endregion

        #region static methods

        static void SetCountryAndAreaCode(string fixNumber, ref string countryCode, ref string areaCode, ref int Id, ref bool IsInternational)
        {
            string Prefix = fixNumber.Substring(0, 5);
            IsInternational = false;
            switch (Prefix)
            {
                case "97250":
                case "97252":
                case "97253":
                case "97254":
                case "97255":
                case "97256":
                case "97257":
                case "97258":
                case "97259":
                    countryCode = Prefix.Substring(0, 3);
                    areaCode = "0"+ Prefix.Substring(3, 2);
                    break;
                //case "97250":
                //    countryCode = "972";
                //    areaCode = "050";
                //    break;
                // case "97252":
                //    countryCode = "972";
                //    areaCode = "052";
                //    break;
                //case "97254":
                //    countryCode = "972";
                //    areaCode = "054";
                //    break;
                //case "97257":
                //    countryCode = "972";
                //    areaCode = "057";
                //    break;
                default:
                    if (Prefix.Substring(0, 2).Equals("00"))
                    {
                        IsInternational = true;
                        //TODO FIX THIS

                        //if (Prefix.Substring(0, 4).Equals("0044"))
                        //{
                        //        countryCode = "44";
                        //        areaCode = "";
                        //        break;
                        //}
                    }
                    break;
            }
            Id = Types.ToInt(fixNumber.Substring(3,9), -1);

        }


        /// <summary>
        /// Get fixed cell number,if is not valid return string.Empty
        /// </summary>
        /// <param name="cellNumber"></param>
        /// <returns>if not valid return String.Empty else return Cli </returns>
        public static string FixNumber(string cellNumber)
        {
            if(string.IsNullOrEmpty(cellNumber))
            {
                return string.Empty;
            }
            string CleanNumber = CleanCli( cellNumber);
            if (!ValidateMobile(CleanNumber))
                return string.Empty;
            return Regx.RegexReplace(@"^(0|972|)", CleanNumber, "972");



         }


        
        /// <summary>
        /// Get fixed cell number,if is not valid return false else return true
        /// </summary>
        /// <param name="cellNumber"></param>
        /// <returns>if not valid return String.Empty else return Cli </returns>
        public static bool TryParse(ref string cellNumber)
        {
            if (string.IsNullOrEmpty(cellNumber))
            {
                return false;
            }
            string CleanNumber = CleanCli(cellNumber);
            if (!ValidateMobile(CleanNumber))
                return false;
            cellNumber = Regx.RegexReplace(@"^(0|972|)", CleanNumber, "972");
            return true;

        }

        /// <summary>
         /// Get CliId as internal number like 527464292
        /// </summary>
        /// <param name="cellNumber"></param>
        /// <returns></returns>
        public static int GetCliId(string cellNumber)
        {
            if (string.IsNullOrEmpty(cellNumber))
                return -1;
            if (cellNumber.StartsWith("972"))
                return Types.ToInt(cellNumber.Substring(3, 9), -1);
            if (cellNumber.StartsWith("05") && cellNumber.Length == 10)
                return Types.ToInt(cellNumber.Substring(1, 9), -1);
            if (cellNumber.Length == 9)
                return Types.ToInt(cellNumber, -1);
            return -1;
        }

        /// <summary>
        /// Get ContactId as combination of key and cellNumber
        /// </summary>
        /// <param name="cellNumber"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long GetContactId(string cellNumber,string key)
        {
            int cellId = GetCliId(cellNumber);
            if (cellId<=0)
                return -1;

            string s = "00000000000000000";
            string cellkey = key + s.Substring(key.Length);
            long id = long.Parse(cellkey);
            return id + cellId;
        }

        /// <summary>
        /// Get Default Operator for fixed number
        /// </summary>
        /// <param name="fixNumber"></param>
        /// <returns></returns>
        static public int GetDefaultOperator(string fixNumber)
        {
            string Prefix = (fixNumber==null || fixNumber.Length<5)?"": fixNumber.Substring(0, 5);

            switch (Prefix)
            {
                case "":
                    return 0;
                case "97250":
                    return 3;
                case "97252":
                    return 1;
                case "97254":
                    return 2;
                case "97257":
                    return 4;
        
                case "97253":
                    return 15;
                case "97255":
                    return 16;
                case "97256":
                    return 17;
                case "97258":
                    return 18;
                case "97259":
                    return 19;
                 default:
                    if(Prefix.Substring(0,2).Equals("00"))
                    return 0;
                return 0;

            }
        }

       
        /// <summary>
        /// Get Default Operator Prefix like "97252"
        /// </summary>
        /// <param name="fixNumber"></param>
        /// <returns></returns>
        static public string GetOperatorPrefix(string cellNumber)
        {
            //string Prefix = fixNumber.Substring(0, 5);

            switch (cellNumber.Length)
            {
                case 9://527464292
                    return "972" + cellNumber.Substring(0, 2);
                case 10://0527464292
                    return "972" + cellNumber.Substring(1, 2);
                case 12://972527464292
                    return cellNumber.Substring(0, 5);
                default:
                    return string.Empty;
            }

        }

        /// <summary>
        /// Get Operator from prefix
        /// </summary>
        /// <param name="fixNumber"></param>
        /// <returns></returns>
        static public int PrefixToOperator(string prefix,int defaultOperator)
        {
            switch (prefix)
            {
                case "050":
                case "97250":
                    return 3;
                case "052":
                case "97252":
                    return 1;
                case "054":
                case "97254":
                    return 2;
                case "057":
                case "97257":
                    return 4;
                case "053":
                case "97253":
                    return 15;
                case "055":
                case "97255":
                    return 16;
                case "056":
                case "97256":
                    return 17;
                case "058":
                case "97258":
                    return 18;
                case "059":
                case "97259":
                    return 19;
                default:
                    return defaultOperator;
            }
        }


        #endregion
    }

}
