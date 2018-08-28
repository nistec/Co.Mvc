using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Netcell.Data.Client;
using Netcell.Data;
using Nistec.Data.Entities;
using Netcell;

namespace Netcell.Data.Db.Entities
{


    public class TransItems : Nistec.Data.Entities.EntityView//, ITransItem
    {
 
        #region Ctor

        //public TransItems(int batchId, int accountId, PlatformType platform)
        //{
        //     try
        //    {
        //        DataTable dt = null;
        //        using (DalController dal = new DalController())
        //        {
        //            dt = dal.Batch_Items_ToSend(batchId, accountId, (int)platform);
        //        }

        //        if (dt != null)
        //        {
        //            base.Init(dt);
        //            base.View.Sort = "Target";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new EntityException("Could Not load CampaignItems:" + ex.Message);
        //    }
        //}
        public TransItems(int batchId, int accountId, PlatformType platform, int batchType)
        {
            try
            {
                DataTable dt = null;
                using (DalController dal = new DalController())
                {
                    dt = dal.Batch_Items_ToSend(batchId, accountId, (int)platform, batchType);
                }

                if (dt != null)
                {
                    base.Init(dt);
                    base.View.Sort = "Target";
                }
            }
            catch (Exception ex)
            {
                throw new EntityException("Could Not load CampaignItems:" + ex.Message);
            }
        }

        #endregion

        public int CampaignId
        {
            get { return base.GetValue<int>(); }
        }
        public int MessageId
        {
            get { return base.GetValue<int>(); }
        }
        public int BatchId
        {
            get { return base.GetValue<int>(); }
        }
        public DateTime SendTime
        {
            get { return base.GetValue<DateTime>(); }
        }
        public string Coupon
        {
            get { return base.GetValue<string>(); }
        }
        public string Personal
        {
            get { return base.GetValue<string>(); }
        }
        public int State //Status
        {
            get { return base.GetValue<int>(); }
        }
        public string Sender
        {
            get { return base.GetValue<string>(); }
        }

        public string Target
        {
            get { return base.GetValue<string>(); }
        }
        public int GroupId
        {
            get { return base.GetValue<int>(); }
        }

        //public int RegisterId
        //{
        //    get { return base.GetValue<int>("Retry"); }
        //}

        //public int SessionId
        //{
        //    get
        //    {
        //        if (ProductType == 8)//session
        //            return base.GetValue<int>("SessionId");
        //        return 0;
        //    }
        //}
        //public string ItemText
        //{
        //    get
        //    {
        //        if (ProductType == 5)//trivia
        //            return base.GetValue<string>("ItemText");
        //        return "";
        //    }
        //}
        public string GetSender(string defaultSender)
        {
            return string.IsNullOrEmpty(Sender) ? defaultSender : Sender;
        }
    }
}
