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

    [Entity("Data_View_Context", "Data_Content", "cnn_Netcell", EntityMode.Generic, "SrcId,Version")]
    public class Data_View_Context : EntityContext<Data_View_Item>
    {
        #region Ctor

        public Data_View_Context(int SrcId, int Version)
            : base(SrcId, Version)
        {

        }

        #endregion

        public static Data_View_Item Get(int SrcId,int Version)
        {
            using (Data_View_Context context = new Data_View_Context(SrcId,Version))
            {
                return context.Entity;
            }
        }

        public static string Lookup_Content_Body(int SrcId, int Version)
        {
            using (var cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.LookupQuery<string>("Body", "Data_Content", "SrcId=@SrcId and Version=@Version", "", new object[] { SrcId, Version });
            }
        }
        public static string Lookup_MessageText(int BatchId)
        {
            using (var cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.LookupQuery<string>("Message", "Trans_Batch_Content", "BatchId=@BatchId", "", new object[] { BatchId });
            }
        }

        public static bool Lookup_HasContent(int SrcId, int Version)
        {
            using (var cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.LookupQuery<string>("Body", "Data_Content", "SrcId=@SrcId and Version=@Version", "", new object[] { SrcId, Version }).Length > 0;
            }
        }

        public static bool Lookup_HasMessage(int BatchId)
        {
            using (var cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.LookupQuery<string>("Message", "Trans_Batch_Content", "BatchId=@BatchId", "", new object[] { BatchId }).Length > 0;
            }
        }
    }
    public class Data_View_Item : IEntityItem
    {
        #region Properties
        //	v.Html,v.Css,v.Title,v.MaxWidth, @CId as CampaignId,@Personal as Personal,@AccountId as AccountId,@PersonalDisplay as PersonalDisplay, @Target as [Target],


        [EntityProperty(EntityPropertyType.Key)]
        public int SrcId { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int Version { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public string Body { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public string Src { get; set; }

        //[EntityProperty(EntityPropertyType.Default)]
        //public string Html { get; set; }

        //[EntityProperty(EntityPropertyType.Default)]
        //public string Preview { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public string Css { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public string Title { get; set; }

        [EntityProperty(EntityPropertyType.Default)]
        public string Sender { get; set; }

        //[EntityProperty(EntityPropertyType.Default)]
        //public int MaxWidth { get; set; }

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
            return DataViewer.ParseInnerBody(Body, pageId);
        }

        #endregion
    }
}
