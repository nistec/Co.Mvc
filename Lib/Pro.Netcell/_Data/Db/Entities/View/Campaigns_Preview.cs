using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using System.Text.RegularExpressions;

namespace Netcell.Data.Db.Entities
{

    [Entity("Campaigns_Preview", "sp_Campaign_View_Preview_b", "cnn_Netcell", EntityMode.Generic, "CampaignId,MessageId,Platform,Mode", EntitySourceType = EntitySourceType.Procedure)]
    public class Campaigns_Preview : ActiveEntity
    {

        #region Ctor

        public Campaigns_Preview()
            : base()
        {

        }
        public Campaigns_Preview(int campaignId, int messageId, int platform, int mode)
            : base(campaignId, messageId, platform, mode)
        {

        }

        #endregion
        
        #region Properties

        [EntityProperty(EntityPropertyType.Default)]
        public int CampaignId
        {
            get { return base.GetValue<int>(); }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public int AccountId
        {
            get { return base.GetValue<int>(); }
        }
        [EntityProperty(EntityPropertyType.Default)]
         public int MaxWidth
        {
           get { return base.GetValue<int>(); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string Html
        {
            get { return base.GetValue<string>(); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string Css
        {
              get { return base.GetValue<string>(); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string Title
        {
             get { return base.GetValue<string>(); }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public string Personal
        {
             get { return base.GetValue<string>(); }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public string PersonalDisplay
        {
             get { return base.GetValue<string>(); }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public bool IsRtl
        {
            get { return base.GetValue<bool>(); }
        }

        //public string DesignHtml
        //{
        //    get { return base.GetValue<string>("DesignHtml"); }
        //}
        //public int DesignId
        //{
        //    get { return base.GetValue<int>("DesignId"); }
        //}

       
        [EntityProperty(EntityPropertyType.Default)]
        public int MtId
        {
             get { return base.GetValue<int>(); }
        }
        #endregion

    }

}
