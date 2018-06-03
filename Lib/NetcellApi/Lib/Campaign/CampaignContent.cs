using Netcell.Data.Db;
using Netcell.Data.Entities;
using Netcell.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netcell.Lib
{
    public class CampaignContent //:ICampaignContent
    {

        #region reload and update

        public void ReloadCampaign(int campaignId)
        {
            CampaignEntity item = CampaignEntity_Context.Get(campaignId);
            if (item.IsEmpty)
                return;
            ReloadCampaign(item);
        }

        public void ReloadCampaign(CampaignEntity item)
        {
            try
            {
                CampaignId = item.CampaignId;
                this.Message = item.MessageText;
                CampaignProductType = item.CampaignProductType;
                //chkConcatenate.Checked = item.GetFeatures().Concatenate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int DoUpdate(bool isTemplate)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                bool ok = IsValid(ref sb, "{0}", false);
                if (!ok)
                {
                    UpdateResult = sb.ToString();
                    return -1;
                }
                int accountId = AccountId;
                int res = DalCampaign.Instance.Campaigns_Message_Update(CampaignId, GetContent(), null);
                if (res < 0)
                {
                    throw new Exception("התוכן לא עודכן");
                }
                if (isTemplate)
                {
                    //string message = txtMessage.Text;
                    //string preview = message.Length < 25 ? message : message.Substring(0, 25);
                    //ucMessageTemplate.SaveNewMessage(accountId, message, IsPersonal, preview);

                }
                //JS.Alert(this.Page, "התוכן עודכן בהצלחה");

                return res;
            }
            catch (Exception ex)
            {
                UpdateResult = ex.Message;
                return -1;
            }
        }

        public int SaveTemplate(bool isTemplate)
        {
            int res = 0;
            try
            {
                StringBuilder sb = new StringBuilder();
                bool ok = IsValid(ref sb, "{0}", false);
                if (!ok)
                {
                    UpdateResult = sb.ToString();
                    return -1;
                }
                int accountId = AccountId;

                if (isTemplate)
                {
                    //string message = Message;
                    //string preview = message.Length < 25 ? message : message.Substring(0, 25);
                    //ucMessageTemplate.SaveNewMessage(accountId, message, IsPersonal, preview);
                    //chkMessageTemplate.Checked = false;
                    res = 1;
                }
                //JS.Alert(this.Page, "התוכן עודכן בהצלחה");

                return res;
            }
            catch (Exception ex)
            {
                UpdateResult = ex.Message;
                return -1;
            }
        }


        #endregion


        #region Proprties

        public string UpdateResult
        {
            get; private set;
        }

        public bool HasChanges
        {
            get { return true; }
        }

        public bool IsReload
        {
            get;
            private set;
        }
        public PublishType PublishType
        {
            get; set;
        }
        public string Message
        {
            get; set;
        }
        public string HtmlContent
        {
            get; set;
        }

        public int CampaignProductType
        {
            get; set;
        }
        protected string PersonalDisplay
        {
            get; set;
        }
        protected string PersonalFields
        {
            get; set;
        }
        public bool IsPersonal
        {
            get; set;

        }
        public int CampaignId
        {
            get; set;
        }
        public int AccountId
        {
            get; set;
        }
        #endregion

        #region editor

        public string GetContent()
        {
            return Message;
        }

        public string GetContentPreview(string personal, string PersonalDisplay)
        {
            return RemoteUtil.BuildMessage(Message, personal, PersonalDisplay);
        }

        public string Header
        {
            get { return ""; }
        }

        public string Footer
        {
            get { return ""; }
        }

        #region properties

        //public string BillingType
        //{
        //    get { return spnBillingType.InnerText; }
        //    private set { spnBillingType.InnerText = value; }
        //}

        public bool IsConcatenate
        {
            get; set;
        }

        public bool IsLatin
        {
            get; set;
        }

        public int ContentSize
        {
            get { return Message == null ? 0 : Message.Length; }
        }

        public int ContentUnits
        {
            get;
            private set;
        }

        public void ClearContent()
        {
            Message = "";
        }
        #endregion

        public bool IsValid(ref StringBuilder sb, string format, bool isSWP)
        {
            int state = 0;


            if (ContentSize <= 0)
            {
                sb.AppendFormat(format, "חסר נוסח ההודעה");
                state = -1;
            }
            else if (IsPersonal && !HasPersonalRemarks)
            {
                sb.AppendFormat(format, "לא נמצאה התייחסות לשדות פרסונאלים בנוסח ההודעה");
                state = -1;
            }
            else if (!IsPersonal && HasPersonalRemarks)
            {
                sb.AppendFormat(format, "נוסח ההודעה כולל שדות פרסונאלים אך הדיוור לא סומן כפרסונאלי");
                state = -1;
            }
            else
            {
                if (isSWP)
                {
                    if (!Message.Contains("@@"))
                    {
                        sb.AppendFormat(format, "חסר @@ לקביעת מיקום הלינק בהודעה מקדימה");
                        state = -1;
                    }
                    if (!this.IsConcatenate)
                    {
                        sb.AppendFormat(format, "חובה לסמן שרשור בשליחת מולטימדיה");
                    }
                }

                IsLatin = RemoteUtil.IsLatin(GetContent());
                int lang_unit = IsLatin ? UnitsItem.DefaultSmsUnitLength_En : UnitsItem.DefaultSmsUnitLength_He;
                if (!IsConcatenate && ContentSize > lang_unit)
                {
                    sb.AppendFormat(format, "נוסח ההודעה מכיל מספר תוים גדול מהמותר");
                    state = -1;
                }
            }

            if (ContentSize > 0)
            {
                int units = 0;
                bool isLatin = false;
                try
                {
                    units = BillingItem.GetSMSBillingUnits(0, GetContent(), isSWP, IsConcatenate, UnitsItem.GetBunch(this.AccountId), out isLatin);
                }
                catch (Exception)
                {
                    sb.AppendFormat(format, "שגיאה בחישוב יחידות חיוב, אנא פנה לתמיכה");
                }
            }

            return state == 0;
        }

      
        public void Summarize(ref StringBuilder sb, string formatsum, int accountId, bool isSWP)
        {
            string message = GetContent();
            int messageLength = ContentSize;
            //int personalLength = PersonalDisplay.Length;

            //IsLatin = RemoteUtil.IsLatin(message);
            //int lang_unit = IsLatin ? ViewConfig.DefaultSmsUnitLength_En : ViewConfig.DefaultSmsUnitLength_He;
            //ContentUnits = (int)Math.Ceiling((decimal)((decimal)messageLength / (decimal)lang_unit));

            bool isLatin = false;
            int units = 0;
            if (ContentSize > 0)
            {
                try
                {
                    units = BillingItem.GetSMSBillingUnits(0, message, isSWP, IsConcatenate, UnitsItem.GetBunch(accountId), out isLatin);
                }
                catch (Exception)
                {
                    //JS.ShowMsg(this.Page, ex.Message, "שגיאה");
                }
            }
            IsLatin = isLatin;
            ContentUnits = units;
            //if (ContentUnits <= 0)
            //    ContentUnits = 1;

            sb.AppendFormat(formatsum, "שפה:", this.IsLatin ? "לטינית" : "עברית");
            if (isSWP)
            {
                sb.AppendFormat(formatsum, "מספר תוים משוער כולל לינק:", this.ContentSize);
                sb.AppendFormat(formatsum, "יחידות חיוב משוער כולל לינק :", this.ContentUnits);
            }
            else
            {
                sb.AppendFormat(formatsum, "שרשור:", this.IsConcatenate ? "כן" : "לא");
                sb.AppendFormat(formatsum, "מספר תוים:", this.ContentSize);
                sb.AppendFormat(formatsum, "יחידות חיוב משוער :", this.ContentUnits);
            }
        }

        #endregion

        #region personal fields


        public bool HasPersonalRemarks
        {
            get
            {
                return ApiUtil.HasPersonalRemark(GetContent());
            }
        }


        #endregion
    }
}
