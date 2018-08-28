using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using System.Text.RegularExpressions;
using Netcell.Data.Client;
using Nistec.Data.Factory;

namespace Netcell.Data.Db.Entities
{

    [Entity("Campaign_View_Mobile", "Campaigns_View_Mobile", "cnn_Netcell", EntityMode.Generic, "CampaignId")]
    public class Campaign_View_Mobile_Context : EntityContext<Campaign_View>
    {
        //public readonly bool IsExists = false;

        #region Ctor

        public Campaign_View_Mobile_Context(int campaignId)
            : base(campaignId)
        {
            //if (IsEmpty)
            //{
            //    //CampaignId = campaignId;
            //    IsExists = false;
            //}
            //else
            //    IsExists = true;
        }

        #endregion

        public static Campaign_View Get(int CampaignId)
        {
            using (Campaign_View_Mobile_Context context = new Campaign_View_Mobile_Context(CampaignId))
            {
                return context.Entity;
            }
        }

        public static bool Lookup_HasContent(int CampaignId)
        {
            using (var cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.LookupQuery<string>("Html", "Campaigns_View_Mobile", "CampaignId=@CampaignId", "", new object[] { CampaignId }).Length > 0;
            }
        }
    }

    [Entity("Campaign_View_Mail", "Campaigns_View", "cnn_Netcell", EntityMode.Generic, "CampaignId")]
    public class Campaign_View_Mail_Context : EntityContext<Campaign_View>
    {
        #region Ctor

        public Campaign_View_Mail_Context(int CampaignId)
            : base(CampaignId)
        {

        }

        #endregion

        public static Campaign_View Get(int CampaignId)
        {
            using (Campaign_View_Mail_Context context = new Campaign_View_Mail_Context(CampaignId))
            {
                return context.Entity;
            }
        }

        public static string Lookup_Mail_Body(int CampaignId)
        {
            using (var cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.LookupQuery<string>("Body", "Campaigns_View", "CampaignId=@CampaignId", "", new object[] { CampaignId });
            }
        }

        public static bool Lookup_HasContent(int CampaignId)
        {
            using (var cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.LookupQuery<string>("Html", "Campaigns_View", "CampaignId=@CampaignId", "", new object[] { CampaignId }).Length > 0;
            }
        }
    }

    public class Campaign_View : IEntityItem
    {
        #region Properties
        //	v.Html,v.Css,v.Title,v.MaxWidth, @CId as CampaignId,@Personal as Personal,@AccountId as AccountId,@PersonalDisplay as PersonalDisplay, @Target as [Target],


        [EntityProperty(EntityPropertyType.Key)]
        public int CampaignId { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public string Body { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public string Html { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public string Preview { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public string Css { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public string Title { get; set; }


        [EntityProperty(EntityPropertyType.Default)]
        public int MaxWidth { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public int PlatformView { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public int Units { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public int Size { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public int State { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public bool IsRtl { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public int PagesCount { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public DateTime Modified { get; set; }
        

        #endregion

        #region Methods

        public string GetPageBody(int pageId)
        {
            return CampaignViewer.ParseInnerBody(Body, pageId);
        }

        #endregion
    }
}
