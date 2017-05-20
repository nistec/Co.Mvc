using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSystem.Data
{
    public static class SystemStatusMessage
    {
        //public static class TaskCompleted
        public static string Get(int res)
        {
            switch (res)
            {
                case 0:
                    return "לא נמצאו\\עודכנו נתונים";
                case 1:
                    return "עודכן\\הסתיים בהצלחה";
                //Ad            1000
                case -1001:
                    return "";
                //Task          1100
                case -1101:
                    return "נשארו משימות פתוחות במעקב זמנים, לא ניתן לסגור";
                case -1102:
                    return "נשארו משימות פתוחות במעקב ביצוע, לא ניתן לסגור";
                case -1103:
                    return "לא ניתן לאתחל משימה שאותחלה";
                case -1104:
                    return "משימה סגורה, לא ניתן לשנות סטאטוס";
                //project       1150
                case -1151:
                    return "";
                //Form          1200
                case -1201:
                    return "";
                default:
                    if (res < 0)
                        return "אירעה תקלה בילתי צפויה";
                    return "לא ידוע";
            }
        }

    }
}
