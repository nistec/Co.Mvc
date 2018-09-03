using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Pro.Lib.Upload
{
   
    public enum UploadState
    {
        Ok = 0,
        WrongItem = 1,
        Duplicate = 2,
        FormatError = 3,
        PersonalError = 4,
        Unexpected = 5,
        InvalidKey=6
    }

    public struct UploadSumarize
    {
        public bool IsAsync;
        public int Ok;
        public int InvalidKey;
        public int WrongItem;
        public int Duplicate;
        public int FormatError;
        public int PersonalError;
        public int Unexpected;
        public string Message;
        public string UploadKey;
        public DataTable WrongItems;
        public string PersonalDemo;
  
        public UploadSumarize(string message)
        {
            Message = message;
            Ok = 0;
            InvalidKey = 0;
            WrongItem = 0;
            Duplicate = 0;
            FormatError = 0;
            PersonalError = 0;
            Unexpected = 0;
            UploadKey = "";
            WrongItems = null;
            PersonalDemo = "";
            IsAsync = false;
        }

        public void Add(string message)
        {
            Message += message + ", ";
        }

        public void Add(int state)
        {
            Add((UploadState)state);
        }

        public void Add(UploadState state)
        {

            switch (state)
            {
                case UploadState.WrongItem:
                    WrongItem++; break;
                case UploadState.Duplicate:
                    Duplicate++; break;
                case UploadState.FormatError:
                    FormatError++; break;
                case UploadState.PersonalError:
                    PersonalError++; break;
                case UploadState.Unexpected:
                    Unexpected++; break;
                case UploadState.InvalidKey:
                    InvalidKey++; break;
                default:
                    Ok++; break;
            }
        }
        public void Sync(UploadSumarize us)
        {
            Ok += us.Ok;
            WrongItem += us.WrongItem;
            Duplicate += us.Duplicate;
            FormatError += us.FormatError;
            PersonalError += us.PersonalError;
            Unexpected += us.Unexpected;
            InvalidKey += us.InvalidKey;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5}", Message,
                "נקלטו: " + Ok.ToString() + " , ",
                WrongItem > 0 ? "נמענים שגויים: " + WrongItem.ToString() + ", " : "",
                Duplicate > 0 ? "נמענים כפולים: " + Duplicate.ToString() + ", " : "",
                FormatError > 0 ? "רשומות שגויות: " + FormatError.ToString() + ", " : "",
                PersonalError > 0 ? "רשומות לא פרסונאליות: " + PersonalError.ToString() + ", " : "",
                Unexpected > 0 ? "רשומות שגויות: " + Unexpected.ToString() : "",
                InvalidKey > 0 ? "מזהה חסר או שגוי: " + InvalidKey.ToString() : "",
                IsAsync ? "תהליך סנכרון הנתונים יסתיים בתוך מספר דקות! "  : "");
        }
        public string ToHtml()
        {
            return string.Format("{0} {1} {2} {3} {4}", Message,
                "נקלטו: " + Ok.ToString() + " <br/>",
                WrongItem > 0 ? "נמענים שגויים: " + WrongItem.ToString() + "<br/>" : "",
                Duplicate > 0 ? "נמענים כפולים: " + Duplicate.ToString() + "<br/> " : "",
                FormatError > 0 ? "רשומות שגויות: " + FormatError.ToString() + "<br/>" : "",
                PersonalError > 0 ? "רשומות לא פרסונאליות: " + PersonalError.ToString() + "<br/>" : "",
                Unexpected > 0 ? "רשומות שגויות: " + Unexpected.ToString() + "<br/>" : "",
                InvalidKey > 0 ? "מזהה חסר או שגוי: " + InvalidKey.ToString() + "<br/>" : "",
                IsAsync ? "תהליך סנכרון הנתונים יסתיים בתוך מספר דקות! "  : "");
        }
    }

}
