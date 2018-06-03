using Netcell;
using Nistec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netcell.Lib
{
    public class CampaignSchedule
    {

        public CampaignSendType SendType { get; set; }

        //    public int GetTimingType()
        //{

        //}
        //public string GetValidTimeToSend()
        //{

        //}

        public string ValidTimeBegin { get; set; }
        public string ValidTimeEnd { get; set; }

        public DateTime FixedStart { get; set; }
        public DateTime FixedEnd { get; set; }
        public string FixedExecTime { get; set; }
        public IList<bool> FixedDays { get; set; }

        public bool IsMultiBatch { get; set; }
        public int BatchValue { get; set; }
        public int BatchDelay { get; set; }
        public int BatchDelayMode { get; set; }
        public IList<bool> BatchDays { get; set; }

        //public bool IsValid(ref StringBuilder err, string format)
        //{

        //}
        //public void Summarize(ref StringBuilder sb, string format, int validItems)
        //{

        //}

        string intToDay(int i)
        {
            switch (i)
            {
                case 0:
                    return "א";
                case 1:
                    return "ב";
                case 2:
                    return "ג";
                case 3:
                    return "ד";
                case 4:
                    return "ה";
                case 5:
                    return "ו";
                case 6:
                    return "ש";
                default:
                    return "";
            }
        }

        /*
        //public void UpdateActiveCampaign(CampaignView ac)
        //{
        //    ac.SetValidTime(ddTimeBegin.Text, ddTimeEnd.Text);
        //    ac.SetBatches(this.rbOneBatch.Checked, this.txtBatchValue.Text, txtBatchDelay.Text, ddlBatchDelayMode.SelectedValue, chkBatchDays.Items);
        //}

        public ListItemCollection FixedDaysR
        {
            get { return listFixDaysR.Items; }
        }
        public DateTime FixedStartR
        {
            get { return DateHelper.ToDateTime(dpFixedStartR.Value); }
        }
        public DateTime FixedEndR
        {
            get { return DateHelper.ToDateTime(dpFixedEndR.Value); }
        }
        public string FixedExecTimeR
        {
            get { return txtTimeR.Text; }
        }
        */
        #region fixed properties
        /*
        public CampaignSendType SendType
        {
            get
            {
                if (pnlReminder.Visible)
                {
                    return CampaignSendType.Watch;
                }
                string g = cur_group.Value;
                if (g == "1")//(rbImmidiate.Checked)
                {
                    return CampaignSendType.Now;
                }
                else if (g == "2")//(rbPending.Checked)
                {
                    return CampaignSendType.Pending;
                }
                else if (g == "3")//(rbFixed.Checked)
                {
                    return CampaignSendType.Fixed;
                }
                else if (g == "4")//(rbMultipleBatch.Checked)
                {
                    return CampaignSendType.Batches;
                }
                return CampaignSendType.Now;
            }
        }
        public ListItemCollection FixedDays
        {
            get { return listFixDays.Items; }
        }
        public DateTime FixedStart
        {
            get { return DateHelper.ToDateTime(dpFixedStart.Value); }
        }
        public DateTime FixedEnd
        {
            get { return DateHelper.ToDateTime(dpFixedEnd.Value); }
        }
        public string FixedExecTime
        {
            get { return txtTime.Text; }
        }
        */
        #endregion

        #region Editor properties
        /*

        public bool IsMultiBatch
        {
            get { return cur_group.Value == "4"; }
            private set {  cur_group.Value = "4"; }
        }

        public string ValidTimeBegin
        {
            get { return txtTimeBegin.Text; }
            private set { txtTimeBegin.Text = value; }
        }
        public string ValidTimeEnd
        {
            get { return txtTimeEnd.Text; }
            private set { txtTimeEnd.Text = value; }
        }

        public int BatchValue
        {
            get { return MControl.Types.ToInt(txtBatchValue.Text, 0); }
            private set { txtBatchValue.Text = value.ToString(); }
        }
        public int BatchDelay
        {
            get { return MControl.Types.ToInt(txtBatchDelay.Text, 0); }
            private set { txtBatchDelay.Text = value.ToString(); }
        }
        public int BatchDelayMode
        {
            get { return MControl.Types.ToInt(ddlBatchDelayMode.Text, 0); }
            private set { ddlBatchDelayMode.SelectedIndex = value == 0 ? 0 : 1; }
        }
        public ListItemCollection BatchDays
        {
            get { return chkBatchDays.Items; }
        }
        */
        public int GetBatchCount(int totalCount)
        {
            if (SendType != CampaignSendType.Batches)//(!rbMultipleBatch.Checked)
                return 1;
            int batchCount = BatchValue;
            if (batchCount <= 0)
                batchCount = 1;
            int count = (int)Math.Ceiling((decimal)((decimal)totalCount / (decimal)batchCount));
            if (count <= 0)
            {
                return 0;// ("נתוני חלוקה למנות אינם תקינים");
            }
            return count;//.ToString();
        }



        #endregion

        #region tab Pending

        //public bool IsPending
        //{
        //    get { return cur_group.Value == "1"/*rbPending.Checked*/; }
        //}

        //protected void rdo_CheckedChanged(object sender, EventArgs e)
        //{
        //    System.Web.UI.WebControls.RadioButton rdb = (System.Web.UI.WebControls.RadioButton)sender;
        //    //CampaignView ac = Sessions.GetActiveCampaign(this);

        //    if (rdb.ClientID.EndsWith("rbImmidiate"))
        //    {
        //        //case "rbImmidiate":
        //        pnlPending.Visible = false;
        //        pnlFixed.Visible = false;
        //        pnlBatch.Visible = false;
        //        //ac.IsPending = false;
        //    }
        //    else if (rdb.ClientID.EndsWith("rbPending"))
        //    {
        //        //case "rbPending":
        //        pnlPending.Visible = true;
        //        pnlFixed.Visible = false;
        //        pnlBatch.Visible = false;
        //    }
        //    else if (rdb.ClientID.EndsWith("rbFixed"))
        //    {
        //        //case "btnFixAdd":
        //        pnlPending.Visible = false;
        //        pnlFixed.Visible = true;
        //        pnlBatch.Visible = false;
        //    }
        //    else if (rdb.ClientID.EndsWith("rbMultipleBatch"))
        //    {
        //        //case "btnFixAdd":
        //        pnlPending.Visible = false;
        //        pnlFixed.Visible = false;
        //        pnlBatch.Visible = true;
        //    }


        //    //this.pnlTime.Refresh();
        //    updatePanel1.Update();
        //}


        #endregion


        #region timing properties

        public DateTime PendingTime
        {
            get;set;
        }

        public bool IsValidPending()
        {
            return PendingTime > DateTime.Now;
        }


        //public string GetTimeToSend()
        //{
        //    if (pnlReminder.Visible)
        //    {
        //        return "שליחה קבועה בשעה: " + txtTimeR.Text;
        //    }
        //    string g = cur_group.Value;
        //    if (g == "1")//(rbImmidiate.Checked)
        //    {
        //        return "מיידי";
        //    }
        //    else if (g == "2")//(rbPending.Checked)
        //    {
        //        if (!DateHelper.IsDateTime(PendingTimeText))//(!MControl.Info.IsDateTime(datePending.Text))
        //            return "תאריך לשליחה אינו תקין";
        //        return PendingTimeText;
        //    }
        //    else if (g == "3")//(rbFixed.Checked)
        //    {
        //        return "שליחה קבוע בשעה: " + txtTime.Text;
        //    }
        //    else if (g == "4")//(rbMultipleBatch.Checked)
        //    {
        //        return "חלוקה למנות";
        //    }
        //    return "";
        //}

        //public int GetTimingType()
        //{
        //    if (pnlReminder.Visible)
        //    {
        //        return 4;
        //    }
        //    string g = cur_group.Value;

        //    if (g == "1")//(rbImmidiate.Checked)
        //    {
        //        return 0;
        //    }
        //    else if (g == "2")//(rbPending.Checked)
        //    {
        //        return 1;
        //    }
        //    else if (g == "3")//(rbFixed.Checked)
        //    {
        //        return 2;
        //    }
        //    else if (g == "4")//(rbMultipleBatch.Checked)
        //    {
        //        return 3;
        //    }
        //    return 0;
        //}

        //public string GetValidTimeToSend()
        //{
        //    if (cur_group.Value == "2")//(rbPending.Checked)
        //    {
        //        return PendingTime.ToString("s");
        //    }
        //    return "";
        //}


        #endregion



        //public bool IsValid(ref StringBuilder sb, string format)
        //{

        //    bool ok = false;

        //    if (pnlReminder.Visible)
        //    {

        //        if (!MControl.Info.IsValidTime(txtTimeR.Text))
        //        {
        //            sb.AppendFormat(format, "שעת השליחה במצב קבוע אינה תקינה");
        //        }
        //        else if (ActiveScheduler.DaysCount(listFixDaysR.Items) == 0)
        //        {
        //            sb.AppendFormat(format, "לא סומנו ימים לשליחה במצב קבוע");
        //        }
        //        else if (!DateHelper.IsDateTime(dpFixedStartR.Value) || !DateHelper.IsDateTime(dpFixedEndR.Value))
        //        {
        //            sb.AppendFormat(format, "מועדי התחלה וסיום במצב קבוע אינם תקינים");
        //        }
        //        else if (DateHelper.ToDateTime(dpFixedEndR.Value) < DateHelper.ToDateTime(dpFixedStartR.Value))
        //        {
        //            sb.AppendFormat(format, "מועד התחלה גדול ממועד הסיום במצב קבוע");
        //        }
        //        else
        //        {
        //            ok = true;
        //        }
        //        return ok;
        //    }


        //    string g = cur_group.Value;
        //    if (g == "1")//rbImmidiate.Checked)
        //    {
        //        return true;
        //    }
        //    else if (g == "2")//(rbPending.Checked)
        //    {
        //        if (!MControl.Info.IsValidTime(txtPendingTime.Text))
        //        {
        //            sb.AppendFormat(format, " השעה לשליחה  במצב תזמון אינה תקינה");
        //            return false;
        //        }
        //        else if (!IsValidPending())//(MControl.Info.IsDateTime(datePending.Text))
        //        {
        //            sb.AppendFormat(format, "תאריך לשליחה אינו תקין");
        //        }
        //        else if (PendingTime < DateTime.Now)
        //        {
        //            sb.AppendFormat(format, "מועד לשליחה בזמן עבר");
        //        }
        //        else
        //        {
        //            ok = true;
        //        }
        //    }
        //    else if (g == "3")//(rbFixed.Checked)
        //    {
        //        if (!Types.IsValidTime(txtTime.Text))
        //        {
        //            sb.AppendFormat(format, "שעת השליחה במצב קבוע אינה תקינה");
        //        }
        //        else if (ActiveScheduler.DaysCount(listFixDays.Items) == 0)
        //        {
        //            sb.AppendFormat(format, "לא סומנו ימים לשליחה במצב קבוע");
        //        }
        //        else if (!DateHelper.IsDateTime(dpFixedStart.Value) || !DateHelper.IsDateTime(dpFixedEnd.Value))
        //        {
        //            sb.AppendFormat(format, "מועדי התחלה וסיום במצב קבוע אינם תקינים");
        //        }
        //        else if (DateHelper.ToDateTime(dpFixedEnd.Value) < DateHelper.ToDateTime(dpFixedStart.Value))
        //        {
        //            sb.AppendFormat(format, "מועד התחלה גדול ממועד הסיום במצב קבוע");
        //        }
        //        else
        //        {
        //            ok = true;
        //        }

        //    }
        //    else if (g == "4")//(rbMultipleBatch.Checked)
        //    {
        //        if (BatchValue <= 0)
        //        {
        //            sb.AppendFormat(format, "מספר המנות אינו תקין");
        //        }
        //        else if (ActiveScheduler.DaysCount(chkBatchDays.Items) == 0)
        //        {
        //            sb.AppendFormat(format, "לא סומנו ימים לשליחה בחלוקה המנות");
        //        }
        //        else
        //        {
        //            ok = true;
        //        }
        //    }

        //    if (!(Strings.StringTimeToTimeSpan(ValidTimeBegin) <= Strings.StringTimeToTimeSpan(ValidTimeEnd)))
        //    {
        //        sb.AppendFormat(format, "זמני שליחה אינם תקינים");
        //        ok = false;
        //    }


        //    return ok;

        //}
        //public void Summarize(ref StringBuilder sb, string format, int validItems)
        //{
        //    int batchCount = GetBatchCount(validItems);

        //    sb.AppendFormat(format, "שעת התחלה:", ValidTimeBegin);
        //    sb.AppendFormat(format, "שעת סיום:", ValidTimeEnd);
        //    sb.AppendFormat(format, "מספר מנות:", batchCount);

        //    if (batchCount > 1)
        //    {
        //        // BatchItems bi=BatchItems.Create(validItems,this.,ValidTimeBegin,ValidTimeEnd,false,BatchValue,BatchDelay,BatchDelayMode,BatchDays);

        //        //foreach (CampaignBatch cb in BatchList)
        //        //{
        //        //    sb.AppendFormat(formatsum, "מנה:", cb.ToString());
        //        //}
        //    }
        //    else
        //    {
        //        sb.AppendFormat(format, "מועד לשליחה:", GetTimeToSend());
        //    }
        //    //return batchCount;
        //}



    }
}
