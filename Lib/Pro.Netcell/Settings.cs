using Nistec.Web.Controls;
using ProNetcell.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProNetcell
{
    public class Settings
    {
        public const string AppName = "co";
        public const string ProjectName = "Party";
        //public const int AccountId = 3;
        public const string ChoosAll = "===בחר הכל===";

        public const int DefaultShortTTL = 1;
        public const int DefaultLongTTL = 60;
        public static object NullableToValue(DateTime? item) 
        {
            if (item.HasValue)
                return item.Value;
            return null;
        }
        public static object NullableToValue(int? item)
        {
            if (item.HasValue)
                return item.Value;
            return null;
        }

    }
}
