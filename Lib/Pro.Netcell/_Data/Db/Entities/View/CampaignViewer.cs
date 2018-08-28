using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using System.Text.RegularExpressions;
using Netcell.Data.Client;
using Nistec.Data.Factory;
using Nistec;
using Netcell;

namespace Netcell.Data.Db.Entities
{
  
    [Entity("CampaignView", "Campaigns_View", "cnn_Netcell", EntityMode.Generic, "CampaignId")]
    public class CampaignViewer_Context : EntityContext<CampaignViewer>
    {
      
        #region ctor

        public CampaignViewer_Context(CampaignViewer item)
            : base(item)
        {

        }
        protected CampaignViewer_Context()
            : base()
        {
        }
              
        public CampaignViewer_Context(int SentId, int CampaignId, int Platform, int Version, bool IsMobile, string DeviceName)
        {
            var parameters=DataParameter.GetSql("SentId",SentId,"CampaignId", CampaignId, "Platform",Platform, "Version",Version, "IsMobile",IsMobile, "DeviceName",DeviceName );
            
            base.Init("sp_Campaign_View_b",parameters, System.Data.CommandType.StoredProcedure);
        }

        #endregion
       
    }

    public class CampaignViewer : IEntityItem
    {

        // v.Html,
        //v.Body,
        //v.Css,
        //v.Title,
        //v.MaxWidth,
        //v.IsRtl,
        //v.PagesCount, 
        //@CId as CampaignId,
        //@Personal as Personal,
        //@AccountId as AccountId,
        //@PersonalDisplay as PersonalDisplay, 
        //@Target as [Target],
		//'' as Header,
        //'' as Footer,
        //@Coupon as Coupon,
        //@ExpirationDate as ExpirationDate,
        //@BarcodeValue as BarcodeValue
	

        #region Properties
        //	v.Html,v.Css,v.Title,v.MaxWidth, @CId as CampaignId,@Personal as Personal,@AccountId as AccountId,@PersonalDisplay as PersonalDisplay, @Target as [Target],


        [EntityProperty(EntityPropertyType.Key)]
        public int CampaignId { get; set; }

        [EntityProperty(EntityPropertyType.Key)]
        public int Version { get; set; }

        [EntityProperty(EntityPropertyType.Key)]
        public int SentId { get; set; }

        [EntityProperty(EntityPropertyType.Key)]
        public int Platform { get; set; }

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

        //  [EntityProperty(EntityPropertyType.Default)]
        //public int State
        //{
        //    get { return base.GetValue<int>(); }
        //}

        [EntityProperty(EntityPropertyType.Default)]
        public bool IsRtl { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public int PagesCount { get; set; }


        #endregion

        #region Campaign property


        [EntityProperty(EntityPropertyType.Default)]
        public int AccountId { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public virtual string Personal { get; set; }


        [EntityProperty(EntityPropertyType.Default)]
        public virtual string PersonalDisplay { get; set; }


        [EntityProperty(EntityPropertyType.Default)]
        public virtual string Target { get; set; }

        //barcode item
        [EntityProperty(EntityPropertyType.Default)]
        public virtual string Coupon { get; set; }


        [EntityProperty(EntityPropertyType.Default)]
        public virtual DateTime ExpirationDate { get; set; }


        //default barcode value
        [EntityProperty(EntityPropertyType.Default)]
        public virtual string BarcodeValue { get; set; }


        //[EntityProperty(EntityPropertyType.Default)]
        //public string RemoveText
        //{
        //    get { return base.GetValue<string>(); }
        //}

        [EntityProperty(EntityPropertyType.NA)]
        public string Dir
        {
            get { return IsRtl ? "rtl" : "ltr"; }
        }

        #endregion

        #region Optional properties

        //  [EntityProperty(EntityPropertyType.Default)]
        //public string Header
        //{
        //    get
        //    {
        //        if (Version == 2)
        //            return base.GetValue<string>();
        //        return "";
        //    }
        //}

        //  [EntityProperty(EntityPropertyType.Default)]
        //public string Footer
        //{
        //    get
        //    {
        //        if (Version == 2)
        //            return base.GetValue<string>();
        //        return "";
        //    }
        //}
        #endregion

        #region methods

        public string GetBody()
        {
            if (Version == (int)VersionView.Preview)//demo
                return Preview;
            return Body;
        }

        public string GetHtml()
        {
            if (Version == (int)VersionView.Preview)//demo
                return Preview;
            return Html;
        }

        public string GetPreview()
        {
            if (Version == (int)VersionView.Preview)//demo
                return Preview;
            return Html;
        }

        public string GetPageBody(int pageId)
        {
            return ParseInnerBody(Body, pageId);
        }

        public string GetBarcode()
        {
            return Types.NzOr(Coupon, BarcodeValue);
        }


        #endregion

        #region static methods

        public static string ParseInnerBody(string html, int page)
        {
            if (string.IsNullOrEmpty(html))
            {
                return "";
            }
            string pattern = string.Format(@"<!--wp#{0}-->((.)|(\s))*?<!--/wp#{0}-->", page);
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match mBody = regex.Match(html);
            if (mBody.Success)
            {
                string body = mBody.ToString();
                body = Regex.Replace(body, "<!--wp#" + page.ToString() + "-->", "", RegexOptions.IgnoreCase);
                body = Regex.Replace(body, "<!--/wp#" + page.ToString() + "-->", "", RegexOptions.IgnoreCase);

                return body.Replace("\r\n", "").Replace("\n", "").Trim();
            }
            return null;
        }


        #endregion
    }

}
