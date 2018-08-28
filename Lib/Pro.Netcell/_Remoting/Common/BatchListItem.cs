using Netcell.Data.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netcell.Remoting
{
   
    public class BatchListItem
    {
        public const int DefaultBatchValue = 999999999;

        public readonly DateTime SendTime;
        public readonly int BatchValue;
        public readonly int BatchId;
        public readonly int BatchIndex;
        public readonly int BatchRange;
        public readonly decimal BatchPrice;
        public readonly Guid PublishKey;

        public BatchListItem(DateTime SendTime, int BatchValue, int BatchId, int BatchIndex, int BatchRange, decimal BatchPrice, Guid PublishKey)
        {
            this.SendTime = SendTime;
            this.BatchValue = BatchValue;
            this.BatchId = BatchId;
            this.BatchIndex = BatchIndex;
            this.BatchRange = BatchRange;
            this.BatchPrice = BatchPrice;
            this.PublishKey = PublishKey;
        }
        public BatchListItem(DateTime SendTime, decimal BatchPrice, Guid PublishKey)
        {
            this.SendTime = SendTime;
            this.BatchValue = DefaultBatchValue;
            this.BatchId = 0;
            this.BatchIndex = 0;
            this.BatchRange = 0;
            this.BatchPrice = BatchPrice;
            this.PublishKey = PublishKey;
        }
        //public BatchListItem()
        //{
        //    this.SendTime = DateTime.Now;
        //    this.BatchValue = DefaultBatchValue;
        //    this.BatchId = 0;
        //    this.BatchIndex = 0;
        //    this.BatchRange = 0;
        //    this.BatchPrice = 0;
        //}
        public override string ToString()
        {
            return string.Format("Time={0} Value={1}", SendTime, BatchValue);
        }

        public Trans_Batch ToTransBatch(int accountId, int userId, DateTime batchTime, BatchTypes batchType, MethodType method, int server)
        {
            return new Trans_Batch()
            {
                AccountId = accountId,
                BatchCount = BatchValue,
                BatchId = BatchId,
                BatchIndex = BatchIndex,
                BatchRange = BatchRange,
                BatchStatus = 0,
                BatchTime = batchTime,
                BatchType = (int)batchType,
                CampaignId = 0,
                Creation = DateTime.Now,
                DefaultPrice = BatchPrice,
                MtId = (int)method,
                Platform = (int)MsgMethod.GetPlatform(method),
                PublishKey = PublishKey.ToString(),
                Server = server,
                UserId = userId

            };

        }
    }
}
