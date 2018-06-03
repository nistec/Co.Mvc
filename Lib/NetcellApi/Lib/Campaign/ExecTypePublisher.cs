using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netcell.Lib
{

    public enum ExecType
    {
        Draft = 0,
        DraftNew = 1,
        SaveNew = 2,
        //SaveNewAndSend = 3,
        Save = 4,
        //SaveAndSend = 5,
        Send = 6,
        SendNew = 7,
        Immediate = 8,
        Test = 9,
        TestNew = 10
    }

    public enum CampaignSendState
    {
        None = 0,
        Start = 1,
        HasError = 2,
        Saved = 3,
        SentNow = 4,
        SentBatch = 5,
        SentPending = 6,
        SentTest = 7,
        SentWatch = 8,
        SentFixed = 9
    }

  
    #region ExecType properties

    public class ExecTypePublisher
    {
        ExecType _ExecType;
  
        public ExecTypePublisher(ExecType execType)
        {
            _ExecType = execType;
        }
        public ExecTypePublisher(int execType)
        {
            _ExecType = (ExecType)execType;
        }
     
        public ExecTypePublisher(bool isSend, bool isSave, bool isNew, bool isDraft, bool isTest, int campaignId)
        {
           
            if (isTest)
            {
                if ((isDraft || isNew)&& campaignId==0)
                    _ExecType = ExecType.TestNew;
                else
                    _ExecType = ExecType.Test;
            }
            else
            {
                //if (isSend && isSave && isNew)
                //    _ExecType = ExecType.SaveNewAndSend;
                //else if (isSend && isSave && campaignId > 0)
                //    _ExecType = ExecType.SaveAndSend;
                if (isSave && isNew)
                    _ExecType = ExecType.SaveNew;
                else if (isSave && campaignId > 0)
                    _ExecType = ExecType.Save;
                else if (isSend)
                    _ExecType = campaignId > 0 ? ExecType.Send : ExecType.SendNew;
                else if (isDraft)
                    _ExecType = campaignId > 0 ? ExecType.Draft : ExecType.DraftNew;
                else
                    _ExecType = ExecType.Send;
            }
        }
        public bool ShouldUpdate()
        {
            switch (_ExecType)
            {
                case ExecType.Send:
                case ExecType.Save:
                //case ExecType.SaveAndSend:
                case ExecType.Draft:
                case ExecType.Test:
                    return true;
                default:
                    return false;
            }
        }

        public CampaignStatus GetStatus()
        {
            if (IsDraft)
                return CampaignStatus.Dtaft;
            else if (IsSaveNew)
                return CampaignStatus.Saved;
            else
                return CampaignStatus.Wait;
        }

        public bool IsSave
        {
            get
            {
                return _ExecType == ExecType.Save || _ExecType == ExecType.SaveNew /*|| _ExecType == ExecType.SaveAndSend || _ExecType == ExecType.SaveNewAndSend*/;
            }
        }

        public bool IsSaveNew
        {
            get
            {
                return _ExecType == ExecType.SaveNew /*|| _ExecType == ExecType.SaveAndSend || _ExecType == ExecType.SaveNewAndSend*/;
            }
        }
        //public bool IsSaveOnly
        //{
        //    get
        //    {
        //        return _ExecType == ExecType.Save || _ExecType == ExecType.SaveNew;
        //    }
        //}
        public bool IsDraft
        {
            get
            {
                return _ExecType == ExecType.Draft || _ExecType == ExecType.DraftNew;
            }
        }
        public bool IsEdited
        {
            get
            {
                return _ExecType == ExecType.Save;// || _ExecType == ExecType.SaveAndSend;
            }
        }

        public bool IsSend
        {
            get
            {
                return _ExecType == ExecType.Send || _ExecType == ExecType.SendNew /* || _ExecType == ExecType.SaveAndSend || _ExecType == ExecType.SaveNewAndSend*/;
            }
        }

        public bool IsTest
        {
            get
            {
                return _ExecType == ExecType.Test || _ExecType == ExecType.TestNew;
            }
        }

        public bool IsNew
        {
            get
            {
                return !ShouldUpdate();
            }
        }
        public void SetExecuteType(bool isSend, bool isSave, bool isNew, bool isDraft, bool isTest, int campaignId)
        {
            if (isTest)
            {
                if ((isDraft || isNew) && campaignId == 0)
                    _ExecType = ExecType.TestNew;
                else
                    _ExecType = ExecType.Test;
            }
            else
            {
                //if (isSend && isSave && isNew)
                //    _ExecType = ExecType.SaveNewAndSend;
                //else if (isSend && isSave && campaignId > 0)
                //    _ExecType = ExecType.SaveAndSend;
                if (isSave && isNew)
                    _ExecType = ExecType.SaveNew;
                else if (isSave && campaignId > 0)
                    _ExecType = ExecType.Save;
                else if (isSend)
                    _ExecType = campaignId > 0 ? ExecType.Send : ExecType.SendNew;
                else if (isDraft)
                    _ExecType = campaignId > 0 ? ExecType.Draft : ExecType.DraftNew;
            }

        }

        public ExecType ExecType
        {
            get { return _ExecType; }
            set
            {
                _ExecType = value;
            }
        }

        public bool IsImmediate
        {
            get { return _ExecType == ExecType.Immediate; }
        }
    }

    #endregion

    #region FixedCheduler
    /*
    public struct FixedScheduler
    {

        public string DaysInWeek;
        public string ExecTime;
        public DateTime FixedStart;
        public DateTime FixedEnd;

        public readonly bool fixedInitilaized;

        public FixedScheduler(DateTime fixedStart, DateTime fixedEnd, string execTime, ListItemCollection daysItems)
        {
            FixedStart = fixedStart;
            FixedEnd = fixedEnd;
            ExecTime = execTime;
            DaysInWeek = ActiveScheduler.DaysToString(daysItems);
            fixedInitilaized = true;
        }
        public FixedScheduler(ITiming timing)
        {
            FixedStart = timing.FixedStart;
            FixedEnd = timing.FixedEnd;
            ExecTime = timing.FixedExecTime;
            DaysInWeek = ActiveScheduler.DaysToString(timing.FixedDays);
            fixedInitilaized = true;
        }

        

        public bool Validate()
        {

            if (string.IsNullOrEmpty(ExecTime))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(DaysInWeek))
            {
                return false;
            }
            else if (FixedEnd < FixedStart)
            {
                return false;
            }
            return true;
        }
    }
    */
    #endregion

}
