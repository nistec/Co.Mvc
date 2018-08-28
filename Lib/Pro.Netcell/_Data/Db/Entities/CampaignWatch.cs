using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using Nistec.Data;
using Nistec.Data.Entities;
//using Nistec.Sys;
using Netcell.Data.Client;
using Nistec.Data.Factory;
using Nistec;


namespace Netcell.Data.Db.Entities
{

    [Entity("CampaignWatch", "vw_Campaign_WatchEntity", "cnn_Netcell", EntityMode.Generic, "CampaignId")]
    public class CampaignWatch_Context : EntityContext<CampaignWatchEntity>
    {
       
        #region ctor
        public CampaignWatch_Context(int CampaignId)
            : base(CampaignId)
        {

        }
        public CampaignWatch_Context(CampaignWatchEntity item)
            : base(item)
        {

        }
        protected CampaignWatch_Context()
            : base()
        {
        }
        #endregion

        #region methods

        public static List<ContactEntity> GetCampaignsWatchListItems(int CampaignId, string DateField)
        {
            DataTable dt = GetCampaignsWatchItems(CampaignId, DateField);
            //return Nistec.Data.Entities.EntityFormatter.DataTableToEntity<ContactEntity>(dt);
            return dt.EntityList<ContactEntity>();
        }

        public static DataTable GetCampaignsWatchItems(int CampaignId, string DateField)
        {
            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.ExecuteCommand<DataTable>("sp_Campaigns_Watch_Items", DataParameter.GetSql("CampaignId", CampaignId, "DateField", DateField), CommandType.StoredProcedure);
            }
        }

        public static CampaignWatchEntity Get(int CampaignId)
        {
            using (CampaignWatch_Context context = new CampaignWatch_Context(CampaignId))
            {
                return context.Entity;
            }
        }

        //public static CampaignWatchEntity Get(int CampaignId)
        //{
        //    using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
        //    {
        //        return cmd.ExecuteCommand<DataTable>("SELECT * from  vw_Campaigns_Watch where CampaignId=@CampaignId", DataParameter.Get("CampaignId", CampaignId));
        //    }
        //}

        #endregion

    
    }

    public class CampaignWatchEntity : CampaignEntity
    {
        [EntityProperty(EntityPropertyType.Default, Caption = "סוג תאריך")]
        public int ReminderField { get; set; }
    }

    /*
     public class CampaignWatchEntity : IEntityItem
     {

         [EntityProperty(EntityPropertyType.Identity, Caption = "דיוור")]
         public int CampaignId
         {
             get;
             set;
         }
         [EntityProperty(EntityPropertyType.Default, Caption = "סוג הדיוור")]
         public int CampaignType//send type
         {
             get;
             set;
         }
         [EntityProperty(EntityPropertyType.Default, Caption = "שם הדיוור")]
         public string CampaignName
         {
             get;
             set;
         }

         [EntityProperty(EntityPropertyType.Default, Caption = "חשבון")]
         public int AccountId
         {
             get;
             set;
         }
         
         [EntityProperty(EntityPropertyType.Default, Caption = "נוצר ב")]
         public DateTime CreationDate
         {
             get;
             set;
         }

         [EntityProperty(EntityPropertyType.Default, Caption = "פריט")]
         public int MאIג
         {
             get;
             set;
         }
         [EntityProperty(EntityPropertyType.Default, Caption = "מדיה")]
         public int Platform
         {
             get;
             set;
         }

         [EntityProperty(EntityPropertyType.Default, Caption = "סוג תאריך")]
         public int ReminderField { get; set; }
     }
 */
}
