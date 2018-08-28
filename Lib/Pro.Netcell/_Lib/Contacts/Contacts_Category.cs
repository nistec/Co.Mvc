using Netcell.Data.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netcell.Lib
{
    public class Contacts_Category
    {

        public static ContactKeyType GetKeyType(int accountId)
        {
            int keyType= DalContacts.Instance.Lookup_ContactKeyType(accountId);
            return (ContactKeyType) keyType;
        }

        #region properties
        public int CategoryId { get; set; }
        //public int AccountCategory { get; set; }
        public int ExType { get; set; }
        public string ExText1 { get; set; }
        public string ExText2 { get; set; }
        public string ExText3 { get; set; }
        public string ExText4 { get; set; }
        public string ExText5 { get; set; }
        public string ExDate1 { get; set; }
        public string ExDate2 { get; set; }
        public string ExDate3 { get; set; }
        public string ExDate4 { get; set; }
        public string ExDate5 { get; set; }
        public string ExKey { get; set; }
        #endregion

    }
}
