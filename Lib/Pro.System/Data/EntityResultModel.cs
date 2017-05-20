using Nistec.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSystem.Data
{
    /*
    public class EntityResultModel
    {
        //public ResultModel() { Target = "alert"; }

        //public ResultModel(int status)
        //{
        //    Target = "alert";
        //    Status = status;
        //    if (status == 401)
        //        Message = "משתמש אינו מורשה";
        //    else if (status == 0)
        //        Message = "לא עודכנו נתונים";
        //    else if (status > 0)
        //        Message = "עודכן בהצלחה";
        //    else if (status < 0)
        //        Message = "אירעה שגיאה, הנתונים לא עודכנו";
        //}

        //protected void Load(int status)
        //{
        //    Target = "alert";
        //    Status = status;
        //    if (status == 401)
        //        Message = "משתמש אינו מורשה";
        //    else if (status == 0)
        //        Message = "לא עודכנו נתונים";
        //    else if (status > 0)
        //        Message = "עודכן בהצלחה";
        //    else if (status < 0)
        //        Message = "אירעה שגיאה, הנתונים לא עודכנו";
        //}

        public int Status { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
        public int OutputId { get; set; }
        public string Args { get; set; }
        public string Target { get; set; }

        public static EntityResultModel Get(EntityCommandResult res, string action)
        {
            return EntityResultModel.Get(res.AffectedRecords,null, action, res.GetIdentityValue<int>());
        }
        public static EntityResultModel Get(int status, string method, string action, int outputIdentity=0)
        {
            string title = action ?? "";
            string message = ResultStatusModel.GetResult(status, method,action);
            //string buttonTrigger = "<input type=\"button\" id=\"btnTrigger\" value=\"המשך\"/>";
            string link = null;
            //if (status > 0)
            //{
            //    link = buttonTrigger;
            //}
            var model = new EntityResultModel() { Status = status, Title = title, Message = message, Link = link, OutputId = outputIdentity };
            return model;
        }
        public static EntityResultModel GetFormResult(int status, string action, string reason, int outputIdentity = 0)
        {
            string title = action ?? "";
            string message = ResultStatusModel.GetResult(status, null, action);
            if (reason != null)
                message = message + " - " + reason;
            //string buttonTrigger = "<input type=\"button\" id=\"btnTrigger\" value=\"המשך\"/>";
            string link = null;
            //if (status > 0)
            //{
            //    link = buttonTrigger;
            //}
            var model = new EntityResultModel() { Status = status, Title = title, Message = message, Link = link, OutputId = outputIdentity };
            return model;
        }
    }
    */
    /*
    public class EntityResultModel
    {

        public static EntityResultModel GetFormResult(int res, string action, string reason, int outputIdentity)
        {
            string title = "";
            string message = "";
            string buttonTrigger = "<input type=\"button\" id=\"btnTrigger\" value=\"המשך\"/>";
            string link = "";

            if (res > 1) res = 1;

            if (action == null)
            {
                switch (res)
                {
                    case 1: title = "עדכון נתונים"; message = "עודכן בהצלחה"; break;
                    case 0: title = "לא בוצע עדכון"; message = "לא נמצאו נתונים לעדכון"; break;
                    case -1: title = "אירעה שגיאה, לא בוצע עדכון."; message = reason; break;
                }

            }
            else
            {
                switch (res)
                {
                    case 1: title = string.Format("עדכון {0}", action); message = string.Format("{0} עודכן בהצלחה", action); break;
                    case 0: title = string.Format("{0} לא עודכן", action); message = string.Format("לא נמצאו נתונים לעדכון", action); break;
                    case -1: title = string.Format("אירעה שגיאה, {0} לא עודכן.", action); message = reason; break;
                }
            }
            if (res > 0)
            {
                link = buttonTrigger;
            }
            var model = new EntityResultModel() { Status = res, Title = title, Message = message, Link = link, OutputId = outputIdentity };
            return model;
        }

        public EntityResultModel() { Target = "alert"; }

        public EntityResultModel(int status)
        {
            Target = "alert";
            Status = status;
            if (status == 401)
                Message = "משתמש אינו מורשה";
            else if (status == 0)
                Message = "לא עודכנו נתונים";
            else if (status > 0)
                Message = "עודכן בהצלחה";
            else if (status < 0)
                Message = "אירעה שגיאה, הנתונים לא עודכנו";
        }

        public static EntityResultModel Get(EntityCommandResult res, string action)
        {
            return EntityResultModel.GetFormResult(res.AffectedRecords, action, null, res.GetIdentityValue<int>());
        }

        public int Status { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
        public int OutputId { get; set; }
        public string Args { get; set; }
        public string Target { get; set; }
    }
    */
}
