using Nistec;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pro.Lib.Upload
{
    public enum UploadState
    {
        Ok = 0,
        WrongItem = 1,
        Duplicate = 2,
        FormatError = 3,
        PersonalError = 4,
        Unexpected = 5
    }

    public enum ContactsUploadMethod
    {
        Sync,
        QueueCommand,
        Auto,
        Preload,
        Stg,
        Integration
    }
    public struct UploadSumarize
    {
        public int Ok;
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
            WrongItem = 0;
            Duplicate = 0;
            FormatError = 0;
            PersonalError = 0;
            Unexpected = 0;
            UploadKey = "";
            WrongItems = null;
            PersonalDemo = "";
        }

        internal void Add(string message)
        {
            Message += message + ", ";
        }

        internal void Add(int state)
        {
            Add((UploadState)state);
        }

        internal void Add(UploadState state)
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
                default:
                    Ok++; break;
            }
        }
        internal void Sync(UploadSumarize us)
        {
            Ok += us.Ok;
            WrongItem += us.WrongItem;
            Duplicate += us.Duplicate;
            FormatError += us.FormatError;
            PersonalError += us.PersonalError;
            Unexpected += us.Unexpected;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", Message,
                "נקלטו: " + Ok.ToString() + " , ",
                WrongItem > 0 ? "נמענים שגויים: " + WrongItem.ToString() + ", " : "",
                Duplicate > 0 ? "נמענים כפולים: " + Duplicate.ToString() + ", " : "",
                FormatError > 0 ? "רשומות שגויות: " + FormatError.ToString() + ", " : "",
                PersonalError > 0 ? "רשומות לא פרסונאליות: " + PersonalError.ToString() + ", " : "",
                Unexpected > 0 ? "רשומות שגויות: " + Unexpected.ToString() : "");
        }
        public string ToHtml()
        {
            return string.Format("{0} {1} {2} {3} {4}", Message,
                "נקלטו: " + Ok.ToString() + " <br/>",
                WrongItem > 0 ? "נמענים שגויים: " + WrongItem.ToString() + "<br/>" : "",
                Duplicate > 0 ? "נמענים כפולים: " + Duplicate.ToString() + "<br/> " : "",
                FormatError > 0 ? "רשומות שגויות: " + FormatError.ToString() + "<br/>" : "",
                PersonalError > 0 ? "רשומות לא פרסונאליות: " + PersonalError.ToString() + "<br/>" : "",
                Unexpected > 0 ? "רשומות שגויות: " + Unexpected.ToString() : "");
        }
    }

    public class MembersUploadSumarize
    {
        public int Status;
        public int MembersUpdated;
        public int MemberInsterted;
        public int CategoryInserted;
        public string Reason;

        public MembersUploadSumarize(object value)
        {
            string result = value == null ? "" : value.ToString();

            if (string.IsNullOrEmpty(result))
            {
                Reason = "Invalid result";
                Status = -1;
            }
            else
            {
                string[] args = result.Split(';');
                foreach (string s in args)
                {
                    string[] arg = s.Split(':');
                    switch (arg[0])
                    {
                        case "Status":
                            Status = Types.ToInt(arg[1]);
                            break;
                        case "MembersUpdated":
                            MembersUpdated = Types.ToInt(arg[1]);
                            break;
                        case "MemberInsterted":
                            MemberInsterted = Types.ToInt(arg[1]);
                            break;
                        case "CategoryInserted":
                            CategoryInserted = Types.ToInt(arg[1]);
                            break;
                        case "Reason":
                            Reason = arg[1];
                            break;
                    }
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}", 
                "נקלטו: " + CategoryInserted.ToString(),
                "חברים שעודכנו: " + MembersUpdated.ToString(),
                "חברים חדשים: " + MemberInsterted.ToString(),
                "חברים שנקלטו לקבוצה: " + CategoryInserted.ToString(),
                "תאור: " + Reason );
        }
        public string ToHtml()
        {
            return string.Format("{0}<br/> {1}<br/> {2}<br/> {3}<br/> {4}",
                 "נקלטו: " + CategoryInserted.ToString(),
                 "חברים שעודכנו: " + MembersUpdated.ToString(),
                 "חברים חדשים: " + MemberInsterted.ToString(),
                 "חברים שנקלטו לקבוצה: " + CategoryInserted.ToString(),
                 "תאור: " + Reason);
        }
    }
}
