using Nistec.Web.Controls;
using ProSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProSystem
{
    public class Settings
    {
        public const string ProjectName = "Co";
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
