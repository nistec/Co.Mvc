using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using MControl;
using Nistec;

namespace Netcell.Lib
{


    #region BatchArgs


    public struct BatchArgs
    {
        //public bool IsMultiBatch;
        public BatchTypes BatchType;
        public int BatchValue;
        public int Delay;
        public int DelayMode;
        public bool[] Days;
        public int MaxItemsPerBatch;
        public int UserId;
        public decimal UnitPrice;
        public string PublishKey;
        bool initilaized;

        public BatchArgs(string args, int MaxBatchItems=5000)
        {
            //BatchArgs = string.Format("{0};{1};{2};{3};{4}", IsMultiBatch ? 1 : 0, batchValue, delay, delayMode,MaxItemsPerBatch, ApiUtil.BoolToArgs(days, '|'));

            if (string.IsNullOrEmpty(args))
            {
                BatchType = BatchTypes.Single;
                BatchValue = 0;
                Delay = 0;
                DelayMode = 0;
                MaxItemsPerBatch = MaxBatchItems;
                Days = new bool[] { true, true, true, true, true, true, true };
                UserId = 0;
                UnitPrice = 0;
                PublishKey = null;
            }
            else
            {
                int maxBatchItems = MaxBatchItems;
                string[] items = args.Split(';');
                int len = items.Length;
                BatchType = (BatchTypes)((int)len == 0 ? 0 : Types.ToInt(items[0], 0));
                //IsMultiBatch =len==0?false: Types.ToInt(items[0], 0) == 1;
                BatchValue = len < 1 ? 0 : Types.ToInt(items[1], 0);
                Delay = len < 2 ? 0 : Types.ToInt(items[2], 0);
                DelayMode = len < 3 ? 0 : Types.ToInt(items[3], 0);
                MaxItemsPerBatch = len < 4 ? maxBatchItems : Types.ToInt(items[4], maxBatchItems);
                Days = new bool[7];
                UserId = len < 6 ? 0 : Types.ToInt(items[6], 0);
                UnitPrice = len < 7 ? 0 : Types.ToDecimal(items[7], 0);
                PublishKey = len < 8 ? null : items[8];
                if (len < 5)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        Days[i] = true;
                    }
                }
                else
                {
                    string[] sdays = items[5].Split('|');
                    if (sdays.Length >= 7)
                    {
                        bool[] days = new bool[sdays.Length];
                        for (int i = 0; i < 7; i++)
                        {
                            days[i] = Types.ToInt(sdays[i], 0) == 1;
                        }
                        Days = days;
                    }
                }
            }
            initilaized = true;
        }

        public bool IsEmpty
        {
            get { return !initilaized; }
        }

        public static string BatchArgsFormat(BatchTypes batchType, int batchValue, int delay, int delayMode, int maxItemsPerBatch, bool[] days, int userId,decimal unitPrice, Guid publishKey)
        {
            return string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", (int)batchType, batchValue, delay, delayMode, maxItemsPerBatch, ApiUtil.BoolToArgs(days, '|'), userId, unitPrice, publishKey);
        }
        public static string BatchArgsFormat(BatchTypes batchType, int batchValue, int maxItemsPerBatch, int userId, decimal unitPrice, Guid publishKey)
        {
            return string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", (int)batchType, batchValue, 0, 0, maxItemsPerBatch, "1|1|1|1|1|1|1", userId, unitPrice, publishKey);
        }
    }

    #endregion


    #region Batch item

    [Serializable]
    public class BatchItem
    {
        #region members
        public readonly int BatchType;
        public readonly int BatchDelayMode;
        public readonly int BatchValue;
        public readonly int BatchDelay;
        public readonly int BatchSaprate;
        public readonly bool[] BatchDays;

        //ListItemCollection BatchDays;
        #endregion

        #region ctor

        public BatchItem(bool IsOneBatch, int batchValue, int delay, int delayMode, bool[] daysItems)
        {
            BatchType = IsOneBatch ? 0 : 1;
            BatchDelayMode = delayMode;
            BatchValue = batchValue;
            BatchDelay = delay;
            BatchDays = daysItems;
        }

        //public BatchItem(bool IsOneBatch, int batchValue, int delay, int delayMode, ListItemCollection daysItems)
        //{
        //    BatchType = IsOneBatch ? 0 : 1;
        //    BatchDelayMode = delayMode;
        //    BatchValue = batchValue;
        //    BatchDelay = delay;
        //    //BatchDays = daysItems;

        //    bool[] days = new bool[7];
        //    for (int d = 0; d < 7; d++)
        //    {
        //        days[d] = daysItems[d].Selected;
        //    }
        //    BatchDays = days;

        //}
        
        public BatchItem(int campainId)
        {

            if (campainId <= 0)
            {
                throw new Exception("לא נמצאו נתונים");
            }
            DataRow dr = null;//TODO Dal.DB.Site.Campaigns(campainId);
            if (dr == null)
            {
                return;// throw new Exception("לא נמצאו נתונים");
            }

            BatchType = Types.ToInt(dr["BatchType"], 0);
            if (BatchType == 0)
            {
                return;
            }
            BatchDelayMode = Types.ToInt(dr["BatchDelayMode"], 0);
            BatchValue = Types.ToInt(dr["BatchValue"], 0);
            BatchDelay = Types.ToInt(dr["BatchDelay"], 0);
            BatchSaprate = Types.ToInt(dr["BatchSaprate"], 0);

            BatchDays = new bool[7];

            string[] list = Types.NZ(dr["BatchDayes"], "").Split(';');
            if (list != null || list.Length < 7)
            {
                return;
            }

            for (int i = 0; i < 7; i++)
            {
                if (Types.ToInt(list[i], 0) > 0)
                    BatchDays[i] = true;
                else
                    BatchDays[i] = false;
            }

        }
        
        #endregion

        #region static

       
         
        public static int CalcBatcheCount(int BatchCount, CampaignSendType sendType, int DestinationCount, int MaxItemsPerBatch)
        {
            if (BatchCount > 0)
            {
                return BatchCount;
            }
            if (sendType != CampaignSendType.Watch && DestinationCount > MaxItemsPerBatch)
            {
                BatchCount = (int)Math.Ceiling((decimal)((decimal)DestinationCount / (decimal)MaxItemsPerBatch));
                if (BatchCount == 0)
                {
                    throw new Exception("נתוני חלוקה למנות אינם תקינים");
                }

            }
            return 1;
        }

        public static decimal CalcBatchePrice(int BatchCount, int DestinationCount, decimal Cost)
        {
            if (BatchCount > 0)
            {
                return Cost;
            }

            return Cost / DestinationCount * BatchCount;
        }

        #endregion

        #region public methods

        public bool IsValidDays()
        {
            if (BatchDays == null || BatchDays.Length < 7)
                return false;
            int validDays = 0;
            for (int d = 0; d < 7; d++)
            {
                validDays += BatchDays[d] ? 1 : 0;
            }
            return validDays > 0;
        }

        #endregion

        #region Batches
      
        private DateTime GetBatcheTime(DateTime timeStart, TimeSpan timeBegin, TimeSpan timeEnd, int delay, int delayMode, bool[] days, int index, bool isTimeBegin)
        {
            DateTime newTime = isTimeBegin ? timeStart : delayMode == 0 ? timeStart.AddMinutes(delay) : timeStart.AddHours(delay);
            DayOfWeek day = (DayOfWeek)newTime.DayOfWeek;
            TimeSpan spn = new TimeSpan(newTime.Hour, newTime.Minute, 0);

            if (!days[(int)day])
            {
                newTime = newTime.AddDays(1);
                newTime = new DateTime(newTime.Year, newTime.Month, newTime.Day, timeBegin.Hours, timeBegin.Minutes, timeBegin.Seconds);
                return GetBatcheTime(newTime, timeBegin, timeEnd, delay, delayMode, days, index + 1, true);
            }
            else if (spn < timeBegin)
            {
                return new DateTime(newTime.Year, newTime.Month, newTime.Day, timeBegin.Hours, timeBegin.Minutes, timeBegin.Seconds);
            }
            else if (spn > timeEnd)
            {
                return GetBatcheTime(newTime, timeBegin, timeEnd, delay, delayMode, days, index + 1, false);
            }
            else if (index > 60)
            {
                return newTime;
            }
            else
            {
                return newTime;
            }
        }

        #endregion

    }
    #endregion

   

    
}
