using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netcell.Data.Server;
using Nistec;
using Netcell;


namespace Netcell.Data
{
    public class Counters:IDisposable
    {
        static object m_lock = new object();
        DalController dalControl;

        public Counters()
        {
            dalControl = new DalController();
            dalControl.AutoCloseConnection = false;
        }

        public void Dispose()
        {
            if (dalControl != null)
            {
                dalControl.Dispose();
            }
        }

        public static void MessageId(int count, ref int MinId, ref int MaxId)
        {
            int trys = 0;
            bool ok=false;

            lock (m_lock)
            {
                while (!ok)
                {
                    try
                    {
                        using (DalController dal = new DalController())
                        {
                            if (count <= 0)
                                count = 1;
                            dal.Counter_New(2, count, ref MinId, ref MaxId);
                        }
                        ok = true;
                    }
                    catch (Exception ex)
                    {
                        trys++;
                        if (trys > 2)
                        {
                            throw ex;
                        }
                    }
                }
                
            }
        }

        public static int BatchId(int accountId, int campaignId, int batchCount, DateTime execTime, int batchIndex, int batchRange, BatchTypes batchType, int userId, PlatformType platform, int mtid, decimal price, string publishKey)
        {
            int batchId = 0;

            using (DalController dal = new DalController())
            {
                dal.Trans_Batch_Insert(ref batchId, accountId, campaignId, batchCount, 0, execTime, batchIndex, batchRange, (int)batchType, userId, 0, (int)platform, (int)mtid, price, publishKey);
            }
            return batchId;
        }

        public static int BatchId(int accountId, int campaignId, int batchCount, BatchTypes batchType, int userId, PlatformType platform, int mtid, decimal price, string publishKey)
        {
            int batchId = 0;

            using (DalController dal = new DalController())
            {
                dal.Trans_Batch_Insert(ref batchId, accountId, campaignId, batchCount, batchType, userId, 0, platform, (int)mtid, price, publishKey);
            }
            return batchId;
        }

        public int AddBatch(int accountId, int campaignId, int batchCount, DateTime execTime, int batchIndex, int batchRange, BatchTypes batchType, int userId, PlatformType platform, int mtid, decimal price, string publishKey)
        {
            int batchId = 0;
            dalControl.Trans_Batch_Insert(ref batchId, accountId, campaignId, batchCount, 0, execTime, batchIndex, batchRange, (int)batchType, userId, 0, (int)platform, (int)mtid, price, publishKey);
            return batchId;
        }

        public int AddBatch(int accountId, int campaignId, int batchCount, BatchTypes batchType, int userId, PlatformType platform, int mtid, decimal price, string publishKey)
        {
            int batchId = 0;

            dalControl.Trans_Batch_Insert(ref batchId, accountId, campaignId, batchCount, batchType, userId, 0, platform, (int)mtid, price, publishKey);

            return batchId;
        }


        public int AddPublishComment(
              string PublishKey,
              string SessionId,
              int ItemId,
              int AckStatus,
              string State,
              string Comment,
              int Server)
        {

            return dalControl.Publish_Comments_Insert(PublishKey, SessionId, ItemId, AckStatus, State, Comment, Server);

        }

        public static int PublishComment(
              string PublishKey,
              string SessionId,
              int ItemId,
              int AckStatus,
              string State,
              string Comment,
              int Server)
        {
            using (DalController dal = new DalController())
            {
                return dal.Publish_Comments_Insert(PublishKey, SessionId, ItemId, AckStatus, State, Comment, Server);
            }
        }

        public static int PublishComment(
             string altKey,
             int ItemId,
             int AckStatus,
             string State,
             string Comment, int Server)
        {
            if (altKey == null)
            {
                return -1;
            }
            string PublishKey = null;
            string SessionId = null;

            if (ParseAltKey(altKey, ref SessionId, ref PublishKey))
            {

                return PublishComment(PublishKey, SessionId, ItemId, AckStatus, State, Comment, Server);
            }
            return 0;
        }

        public static bool ParseAltKey(string altKey, ref string sessionId, ref string publishKey)
        {
            if (string.IsNullOrEmpty(altKey))
                return false;
            return Nistec.Generic.GenericArgs.SplitArgs<string, string>(altKey, '$', ref sessionId, ref publishKey);
        }

        public static Guid ParsePublishKey(string altKey)
        {
            if (string.IsNullOrEmpty(altKey))
                return Guid.Empty;
            string sessionId = null;
            string publishKey = null;
            if (Counters.ParseAltKey(altKey, ref sessionId, ref publishKey))
                return Types.ToGuid(publishKey);
            return Guid.Empty;
        }

     
        public int AddMessage
            (
             int CampaignId,
             int BatchId,
             DateTime SendTime,
             string Target,
             int State,
             int Retry,
             int GroupId,
             string Coupon,
             string Personal,
             string Sender,
             int ItemIndex,
             int BatchIndex,
             int Platform,
             int ContactId)
        {

  
            int minId = 0;
            int maxId = 0;
            Counters.MessageId(1, ref minId, ref maxId);

            int MessageId = maxId;

            dalControl.Trans_Items_Insert(MessageId, CampaignId, BatchId, SendTime, Target.Trim(), State, GroupId, Coupon, Personal, Sender, ItemIndex, Platform,ContactId);
            return MessageId;
        }

        public int AddMessage
            (
             int CampaignId,
             int BatchId,
             DateTime SendTime,
             string Target,
             string Personal,
             string Sender,
             int Platform,
            int ContactId)
        {

            int minId = 0;
            int maxId = 0;
            Counters.MessageId(1, ref minId, ref maxId);

            int MessageId = maxId;

            dalControl.Trans_Items_Insert(MessageId, CampaignId, BatchId, SendTime, Target.Trim(), 0, 0, null, Personal, Sender, 0, Platform, ContactId);
            return MessageId;
        }

        public static int MessageRB()
        {
            throw new Exception("Conter MessageRB not supported");
        }

        public static int WapId()
        {
            lock (m_lock)
            {
                using (DalRule dal = new DalRule())
                {
                    int id = 0;
                    dal.Counter_New(6,1,ref id);
                    return id;
                }
            }
        }
    }
}
