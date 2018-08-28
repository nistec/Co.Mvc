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
    [Entity("Campaigns_Links", "Campaigns_Links", "cnn_Netcell", EntityMode.Multi, "CampaignId", EntitySourceType = EntitySourceType.Table)]
    public class Campaigns_Links: EntityTable
    {
 
        public Campaigns_Links(int campaignId)
            : base(campaignId)
        {
            //using (DalCampaign dal = new DalCampaign())
            //{
            //    DataTable table = dal.Campaigns_LinksWithSchema(campaignId);
            //    base.Init(table);
            //}
        }


        public void LoadItem(int CampaignId, int LinkId, string DisplayText, string Link, int ConfirmStatus, int DesignId, int LinkType)
        {
            if (LinkId <= 0)
            {
                LinkId = GetMaxLinkId();
            }
            this.EntityDataSource.BeginEdit();
            this.EntityDataSource.LoadDataRow(new object[] { CampaignId, LinkId, DisplayText, Link, ConfirmStatus, DesignId, LinkType }, false);
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


        //public LinkViewItems ToLinkViewItems()
        //{
        //    if (IsEmpty)
        //    {
        //        return null;
        //    }
        //    LinkViewItems items = new LinkViewItems();
        //    Position = 0;
        //    for (int i = 0; i < Count; i++)
        //    {
        //        Position = i;
        //        LinkView item = ToLinkView();
        //        if (item.Index > 0)
        //        {
        //            items.Add(item);
        //        }
        //    }
        //    Position = 0;
        //    return items;
        //}

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
            DataTable table = new DataTable("CampaignsLink");
            table.Columns.Add("CampaignId", typeof(int));
            table.Columns.Add("LinkId", typeof(short));
            table.Columns.Add("DisplayText");
            table.Columns.Add("Link");
            table.Columns.Add("ConfirmStatus", typeof(short));
            table.Columns.Add("DesignId", typeof(int));
            table.Columns.Add("LinkType", typeof(short));
            return table.Clone();
        }

          #endregion

         //public int GetMaxIndex()
         //{
         //    if (MaxIndex <= 0 || IsEmpty)
         //        return 0;

         //    Position = 0;
         //    for (int i = 0; i < Count; i++)
         //    {
         //        Position = i;
         //        MaxIndex = Math.Max(MaxIndex, LinkId);
         //    }
         //    Position = 0;
         //    return MaxIndex;
         //}

        //public void CampaignLink_Update(LinkViewItems items)
        // {
        //     if (IsEmpty)
        //         return;

        //    GetMaxIndex();
        //    int added = 0;
        //     if (((CampaignId > 0) && (items != null)) && (items.Count != 0))
        //     {
        //         DataTable table = CampaignLinkSchema();
        //         if (table != null)
        //         {
        //              foreach (LinkView item in items)
        //             {
        //                 if (item.Index>MaxIndex)
        //                 {
        //                     table.Rows.Add(new object[] { CampaignId, item.Index, item.Title, item.Href, item.Value, item.DesignId, (int)item.LinkType });
        //                     added++;
        //                 }
        //             }
        //             if (added > 0)
        //             {
        //                 DalCampaign.Instance.InsertTable(table, "Campaigns_Links");
        //             }
        //         }
        //     }
        // }

        //public LinkView ToLinkView()
        //{
        //    return new LinkView(Link, DisplayText, LinkId, ConfirmStatus,DesignId);
        //}

        #region properties

        [EntityProperty(EntityPropertyType.Key)]
        public int CampaignId
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
