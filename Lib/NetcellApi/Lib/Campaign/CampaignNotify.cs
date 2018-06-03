using Netcell;
using Nistec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netcell.Lib
{
    public class CampaignNotify
    {


        #region properties

        List<string> _NotifyItems;
        public List<string> NotifyItems
        {
            get
            {
                if (_NotifyItems == null)
                {
                    _NotifyItems = new List<string>();
                }
                return _NotifyItems;
            }
        }
        public int NotifyCount
        {
            get { return NotifyItems.Count; }
        }

        public PlatformType NotifyPlatform
        {
            get; set;
        }
        public CampaignNotifyType NotifyType
        {
            get; set;
        }

        public bool EnableReply
        {
            get; set;
        }
        public bool EnableOnBegin
        {
            get; set;
        }
        public bool EnableOnEnd
        {
            get; set;
        }


        bool IsMail
        {
            get { return NotifyPlatform == PlatformType.Mail; }
        }

        private bool ValidateNotifyCells(ref string result)
        {
            if (NotifyItems.Count == 0)
            {
                result = "ok";
                return true;
            }
            bool isMail = IsMail;
            foreach (var li in NotifyItems)
            {
                if (isMail)
                {
                    if (!Regx.IsEmail(li))
                    {
                        result = "אחד הנמענים לקבלת העדכונים אינו תקין או אינו תואם לאמצעי השליחה שסומן";
                        return false;
                    }
                }
                else
                {
                    if (!CLI.ValidateMobile(li))
                    {
                        result = "אחד הנמענים לקבלת העדכונים אינו תקין או אינו תואם לאמצעי השליחה שסומן";
                        return false;
                    }
                }
            }

            result = "ok";
            return true;
        }

        private void ValidateItem(string target)
        {

            if (IsMail)
            {
                if (!Regx.IsEmail(target))
                {
                    throw new Exception("דואר אלקטרוני אינו תקין");
                }
            }
            else
            {
                if (!CLI.ValidateMobile(target))
                {
                    throw new Exception("טלפון נייד אינו תקין");
                }
            }
        }


        #endregion

        //public string[] GetNotifyCells()
        //{
        //    return NotifyItems.ToArray();
        //}
        public static CampaignNotifyType GetNotifyType(bool onStart, bool onFinished, bool onReply)
        {

            //bool onStart = chkNotifyBegin.Checked;
            //bool onFinished = chkNotifyEnd.Checked;
            //bool onReply = chkNotifyReply.Checked;

            CampaignNotifyType notifyOptions = CampaignNotifyType.None;

            if (onStart && onFinished && onReply)
            {
                notifyOptions = CampaignNotifyType.BothAndReply;
            }
            else if (onStart && onReply)
            {
                notifyOptions = CampaignNotifyType.BeginAndReply;
            }
            else if (onStart && onFinished)
            {
                notifyOptions = CampaignNotifyType.Both;
            }
            else if (onStart)
            {
                notifyOptions = CampaignNotifyType.OnStart;
            }
            else if (onFinished)
            {
                notifyOptions = CampaignNotifyType.OnEnd;
            }
            else if (onReply)
            {
                notifyOptions = CampaignNotifyType.OnReplyOnly;
            }
            else
            {
                notifyOptions = CampaignNotifyType.None;
            }
            return notifyOptions;

        }

        public string NotifyTypeToString()
        {
            if (NotifyCount == 0)
                return "ללא";

            switch (NotifyType)
            {
                case CampaignNotifyType.None:
                    return "ללא";
                case CampaignNotifyType.OnStart:
                    return "בתחילת הקמפיין";
                case CampaignNotifyType.OnEnd:
                    return "בסיום הקמפיין";
                case CampaignNotifyType.Both:
                    return "בתחילה ובסיום הקמפיין";
                case CampaignNotifyType.BeginAndReply:
                    return "בתחילת כולל עדכונים";
                case CampaignNotifyType.BothAndReply:
                    return "בתחילה ובסיום הקמפיין כולל עדכונים";
                case CampaignNotifyType.OnReplyOnly:
                    return "עדכונים בלבד";
            }
            return "ללא";

        }

        public bool IsValid(ref StringBuilder sb, string format)
        {
            bool ok = false;
            string result = "";

            if (EnableReply ||
               EnableOnBegin ||
                EnableOnEnd)
            {
                if (NotifyItems.Count <= 0)
                {
                    sb.AppendFormat(format, "לא נמצאו נמענים לקבלת עדכונים");
                }
                else if (((int)NotifyPlatform) <= 0)
                {
                    sb.AppendFormat(format, "לא סומן אמצעי שליחה לקבלת עדכונים");
                }
                else if (!ValidateNotifyCells(ref result))
                {
                    sb.AppendFormat(format, result);
                }
                else
                {
                    ok = true;
                }
            }
            else if (NotifyItems.Count > 0)
            {
                sb.AppendFormat(format, "לא סומנו מצבים לקבלת עדכונים");
            }
            else
            {
                ok = true;
            }

            return ok;
        }

        public void Summarize(ref StringBuilder sb, string format)
        {
            sb.AppendFormat(format, "עדכונים:", NotifyTypeToString());
            foreach (var notifyCell in NotifyItems)
            {
                sb.AppendFormat(format, "עדכונים לנמען:", notifyCell);
            }

        }


        #region Notify

        //public void SetNotify(ICampaignNotify notify)
        //{
        //    //Features.NotifyOptions = notify.NotifyType;
        //    NotifyType = notify.NotifyType;
        //    NotifyCells = notify.GetNotifyCells();
        //    //ReplyTo = GetNotifyCells();
        //}

        public void SetNotify(CampaignNotifyType notifyType, string[] notifyCells)
        {
            //Features.NotifyOptions = notifyType;
            NotifyType = notifyType;
            NotifyItems.AddRange(notifyCells);
            //ReplyTo = GetNotifyCells();
        }

        protected string GetNotifyCells()
        {
            string cells = "";
            if (NotifyType != CampaignNotifyType.None)
            {
                foreach (string s in NotifyItems)
                {
                    cells += s + ";";
                }
            }
            cells = cells.TrimEnd(';');
            return cells;
        }
        #endregion

        /*
        public int ReloadCampaign(CampaignEntity item, ref StringBuilder sb)
        {
            if (item == null)
                return 0;
            try
            {


                CampaignNotifyType notifyType = (CampaignNotifyType)item.NotifyType;//.GetFeatures().NotifyOptions;
                ddlNotfyPlatform.Text = item.NotifyType.ToString();
                switch (notifyType)
                {
                    case CampaignNotifyType.Both:
                        chkNotifyBegin.Checked = true;
                        chkNotifyEnd.Checked = true;
                        break;
                    case CampaignNotifyType.OnEnd:
                        chkNotifyEnd.Checked = true;
                        break;
                    case CampaignNotifyType.OnStart:
                        chkNotifyBegin.Checked = true;
                        break;
                    case CampaignNotifyType.BeginAndReply:
                        chkNotifyBegin.Checked = true;
                        chkNotifyReply.Checked = true;
                        break;
                    case CampaignNotifyType.BothAndReply:
                        chkNotifyBegin.Checked = true;
                        chkNotifyEnd.Checked = true;
                        chkNotifyReply.Checked = true;
                        break;
                    case CampaignNotifyType.OnReplyOnly:
                        chkNotifyReply.Checked = true;
                        break;
                    case CampaignNotifyType.None:
                    default:
                        return 0;
                }

                string[] list = item.NotifyCells.Split(';');
                if (list != null)
                {
                    foreach (string s in list)
                    {
                        this.listNotify.Items.Add(s);
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                sb.AppendFormat("<h4>{0}<h4><p>{1}</p>", "שגיאה בשחזור התראות", ex.Message);
                return -1;
            }
        }
        */
    }
}
