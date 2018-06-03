using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pro.Mvc.Models
{
    public class ResultTaskModel : ResultModel<Guid>
    {
        public ResultTaskModel() :base(){}
        public ResultTaskModel(int status) : base(status) { }
    }
    public class ResultModel : ResultModel<int>
    {
        public ResultModel():base() { }
         public ResultModel(int status):base(status){}
    }
    public class ResultModel<T>
    {
        public ResultModel() { Target = "alert"; }

        public ResultModel(int status)
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

        protected void Load(int status)
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

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        //public ResultModel(int status,string action)
        //{
        //    Target = "alert";
        //    Status = status;

        //    switch(action)
        //    {
        //        case "signup":
        //            if (status == -1)
        //                Message = "משתמש אינו מורשה"; 
        //            else if (status == 0)
        //                Message = "לא עודכנו נתונים";
        //            else if (status > 0)
        //                Message = "עודכן בהצלחה";
        //            else if (status < 0)
        //                Message = "אירעה שגיאה, הנתונים לא עודכנו";
        //            break;
        //        default:
        //            if (status == 401)
        //                Message = "משתמש אינו מורשה";
        //            else if (status == 0)
        //                Message = "לא עודכנו נתונים";
        //            else if (status > 0)
        //                Message = "עודכן בהצלחה";
        //            else if (status < 0)
        //                Message = "אירעה שגיאה, הנתונים לא עודכנו";
        //            break;
        //    }
           
        //}

        public int Status { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
        public T OutputId { get; set; }
        public string Args { get; set; }
        public string Target { get; set; }


        public static ResultModel<T> GetFormResult(int res, string action, string reason, T outputIdentity)
        {
            string title = "";
            string message = "";
            //string buttonTrigger = "<input type=\"button\" id=\"btnTrigger\" value=\"המשך\"/>";
            string link = null;

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
            //if (res > 0)
            //{
            //    link = buttonTrigger;
            //}
            var model = new ResultModel<T>() { Status = res, Title = title, Message = message, Link = link, OutputId = outputIdentity };
            return model;
        }
    }

    public class MediaModel
    {   
        public int AccountId { get; set; }
        public string Folder { get; set; }
        public string BaseUrl { get; set; }
        //public int BuildingId { get; set; }
        //public int UnitId { get; set; }
        //public string PropertyType { get; set; }
    }

    public class TaskInfoModel: InfoModel<Guid>
    {
    }
    public class InfoModel : InfoModel<int>
    {
    }
    public class InfoModel<T>
    {
        public T Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
    }
    public class ContentModel
    {
        public static ContentModel Get(string content)
        {
            return new ContentModel() { Content=content };
        }
         public string Content { get; set; }
    }
    public class UploadFileModel
    {
        public int AccountId { get; set; }
        public int FileId { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }

    }

    public class Employee
    {
        //public int Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public DateTime BirthDate { get; set; }
    }

   
}