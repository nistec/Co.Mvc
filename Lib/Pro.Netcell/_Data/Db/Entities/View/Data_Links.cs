using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using System.Data.Linq;
using Netcell.Data.Client;
using Nistec.Data.Entities;
using Nistec.Data;
using Nistec;

namespace Netcell.Data.Db.Entities
{
    [Entity("Data_Links", "Data_Links", "cnn_Netcell", EntityMode.Multi, "SrcId,Version", EntitySourceType = EntitySourceType.Table)]
    public class Data_Links: EntityTable
    {

        public Data_Links(int srcId, int version)
            : base(srcId, version)
        {
            //using (DalCampaign dal = new DalCampaign())
            //{
            //    DataTable table = dal.Campaigns_LinksWithSchema(campaignId);
            //    base.Init(table);
            //}
        }


        public void LoadItem(int srcId, int version,int LinkId, string DisplayText, string Link, int ConfirmStatus, int DesignId, int LinkType)
        {
            if (LinkId <= 0)
            {
                LinkId = GetMaxLinkId();
            }
            this.EntityDataSource.BeginEdit();
            this.EntityDataSource.LoadDataRow(new object[] { srcId, version, LinkId, DisplayText, Link, ConfirmStatus, DesignId, LinkType }, false);
            this.EntityDataSource.EndEdit();
        }

        public string LookupHref(int linkId)
        {
            string href = this.EntityDataSource.AsEnumerable()
                           .Where(row => row.Get<int>("LinkId",-1) == linkId)
                           .Select(row => row["Link"]).ToString();

            return href;
        }

        int LookupMaxLinkId()
        {
            if (this.EntityDataSource.IsEmpty)
                return 0;

            int maxLinkId = 0;
            foreach (DataRow dr in this.EntityDataSource.Rows)
            {
                int linkId = dr.Get<int>("LinkId");
                maxLinkId = Math.Max(maxLinkId, linkId);
            }
            return maxLinkId;

        }

        public int GetMaxLinkId()
        {
            if (this.EntityDataSource.IsEmpty)
                return 0;
            int maxLinkId = Types.ToInt(this.EntityDataSource.Compute("max(LinkId)", string.Empty), 0);
            return maxLinkId;
        }

        public override int SaveChanges()
        {
            if (this.EntityDataSource.IsEmpty)
                return 0;
            return base.SaveChanges();
        }
 
        #region static

        //public static LinkViewItems GetCampaignLinks(int campaignId)
        //{
        //    CampaignLinks links = new CampaignLinks(campaignId);
        //    if (links.IsEmpty)
        //    {
        //        return null;
        //    }
        //    return links.ToLinkViewItems();

        //}

        public static DataTable CampaignLinkSchema()
        {
            DataTable table = new DataTable("DataLink");
            table.Columns.Add("SrcId", typeof(int));
            table.Columns.Add("Version", typeof(int));
            table.Columns.Add("LinkId", typeof(short));
            table.Columns.Add("DisplayText");
            table.Columns.Add("Link");
            table.Columns.Add("ConfirmStatus", typeof(short));
            table.Columns.Add("DesignId", typeof(int));
            table.Columns.Add("LinkType", typeof(short));
            return table.Clone();
        }

          #endregion

        #region properties

        [EntityProperty(EntityPropertyType.Key)]
        public int SrcId
        {
            get { return base.GetValue<int>(); }
            set { base.SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Key)]
        public int Version
        {
            get { return base.GetValue<int>(); }
            set { base.SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Key)]
        public int LinkId
        {
            get { return base.GetValue<int>(); }
            set { base.SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string Link
        {
            get { return base.GetValue<string>(); }
            set { base.SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string DisplayText
        {
            get { return base.GetValue<string>(); }
            set { base.SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int ConfirmStatus
        {
            get { return base.GetValue<int>(); }
            set { base.SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int DesignId
        {
            get { return base.GetValue<int>(); }
            set { base.SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int LinkType
        {
            get { return base.GetValue<int>(); }
            set { base.SetValue(value); }
        }
        #endregion
    }
}
