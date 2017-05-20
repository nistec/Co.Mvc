using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSystem
{
    public class ResultStatusModel
    {

        public static string GetResult(int status, string method, string action = null)
        {
            //const string defaultResult = "ok";

            switch (method)
            {
                case "TaskCompleted":
                    if (status == -1)
                        return "נמצאו משימות שטרם הסתיימו במעקב זמנים";
                    if (status == -2)
                        return "נמצאו משימות שטרם הסתיימו במעקב ביצוע";
                    break;
            }

            if (status == 401)
                return "משתמש אינו מורשה";
            if (action == null)
            {
                if (status == 0)
                    return "לא עודכנו נתונים";
                else if (status > 0)
                    return "עודכן בהצלחה";
                else if (status < 0)
                    return "אירעה שגיאה, הנתונים לא עודכנו";
            }
            else
            {
                if (status == 0)
                    return string.Format("{0} לא עודכנו נתונים", action);
                else if (status > 0)
                    return string.Format("{0} עודכן בהצלחה", action);
                else if (status < 0)
                    return string.Format("אירעה שגיאה, {0} הנתונים לא עודכנו.", action);
            }
            return "NA";

        }
    }
}
