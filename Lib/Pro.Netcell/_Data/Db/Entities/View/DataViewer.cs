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

    [Entity("DataViewer", "Data_Content", "cnn_Netcell", EntityMode.Generic, "SourceId")]
    public class DataViewer_Context : EntityContext<DataViewer>
    {
      
        #region ctor

        public DataViewer_Context(DataViewer item)
            : base(item)
        {

        }
        protected DataViewer_Context()
            : base()
        {
        }

        public DataViewer_Context(int SentId, int SrcId, int Version, int Platform, bool IsPreview, bool IsMobile, string DeviceName)
        {
            var parameters = DataParameter.GetSql("SentId", SentId, "SrcId", SrcId, "Version", Version, "Platform", Platform, "IsPreview", IsPreview, "IsMobile", IsMobile, "DeviceName", DeviceName);
            
            base.Init("sp_Data_View",parameters, System.Data.CommandType.StoredProcedure);
        }

        #endregion
       
    }

    public class DataViewer : IEntityItem
    {

        

        #region Properties

        [EntityProperty(EntityPropertyType.Key)]
        public int SrcId { get; set; }

        [EntityProperty(EntityPropertyType.Key)]
        public int Version { get; set; }

        //[EntityProperty(EntityPropertyType.Key)]
        //public int Version { get; set; }

        [EntityProperty(EntityPropertyType.Key)]
        public int SentId { get; set; }

        [EntityProperty(EntityPropertyType.Key)]
        public int Platform { get; set; }

        //[EntityProperty(EntityPropertyType.Default)]
        //public bool IsPreview { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public string Body { get; set; }
        [EntityProperty(EntityPropertyType.Default)]
        public string Src { get; set; }
        [EntityProperty(EntityPropertyType.Default)]
        public string Title { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public bool IsRtl { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public int PagesCount { get; set; }
        //[EntityProperty(EntityPropertyType.Default)]
        //public int MaxWidth { get; set; }

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


        //[EntityProperty(EntityPropertyType.Default)]
        //public virtual DateTime ExpirationDate { get; set; }


        ////default barcode value
        //[EntityProperty(EntityPropertyType.Default)]
        //public virtual string BarcodeValue { get; set; }

        [EntityProperty(EntityPropertyType.NA)]
        public string Dir
        {
            get { return IsRtl ? "rtl" : "ltr"; }
        }

        #endregion
   
        #region methods

        //public string GetBody()
        //{
        //    return Body;
        //}

        public string GetPageBody(int pageId)
        {
            return ParseInnerBody(Body, pageId);
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
