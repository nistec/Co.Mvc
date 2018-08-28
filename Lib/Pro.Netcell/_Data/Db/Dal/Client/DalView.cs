using System;
using System.Data;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using Nistec.Data;
using Nistec.Data.SqlClient;
using System.Data.SqlClient;

using Nistec;
using System.Text;
using Nistec.Data.Factory;

namespace Netcell.Data.Client
{


    public class DalView : Nistec.Data.SqlClient.DbCommand
    {
        #region ctor

        public DalView()
            : base(Netcell.Data.DBRule.CnnNetcell)
        {

        }

        public static DalView Instance
        {
            get { return new DalView(); }
        }
        #endregion

        #region mail
        /*
        [DBCommand(DBCommandType.StoredProcedure, "sp_View_MailShow")]
        public DataRow View_MailShow
            (
            [DbField()]int SentId,
            [DbField()]int CampaignId,
            [DbField()]int Version,
            [DbField()]bool IsMobile,
            [DbField(50)]string DeviceName
            )
        {
            return (DataRow)base.Execute(new object[] { SentId, CampaignId, Version, IsMobile, DeviceName });
        }
        */


        [DBCommand(DBCommandType.StoredProcedure, "sp_Data_View")]
        public DataRow Data_View
            (
            [DbField()]int SentId,
            [DbField()]int SrcId,
            [DbField()]int Version,
            [DbField()]int Platform,
            [DbField()]bool IsPreview,
            [DbField()]bool IsMobile,
            [DbField(250)]string DeviceName
            )
        {
            return (DataRow)base.Execute(new object[] { SentId, SrcId, Version, Platform, IsPreview, IsMobile, DeviceName });
        }



        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_View_Preview_b")]
        public DataRow Campaign_Preview
            (
            [DbField()]int CampaignId,
            [DbField()]int MessageId,
            [DbField()]int Platform,
            [DbField()]int Mode
            )
        {
            return (DataRow)base.Execute(new object[] { CampaignId, MessageId, Platform, Mode });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_View_b")]
        public DataRow Campaign_View
            (
            [DbField()]int SentId,
            [DbField()]int CampaignId,
            [DbField()]int Platform,
            [DbField()]int Version,
            [DbField()]bool IsMobile,
            [DbField(50)]string DeviceName
            )
        {
            return (DataRow)base.Execute(new object[] { SentId, CampaignId, Platform,Version, IsMobile, DeviceName });
        }

 
        #endregion

        [DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Reply_Confirm")]
        public object Campaign_Confirm_Reply([DbField] int SentId, [DbField] int LinkId, [DbField] int ConfirmStatus, [DbField] int Platform, [DbField] int Version)
        {
            return base.Execute(new object[] { SentId, LinkId, ConfirmStatus, Platform, Version });
        }

        #region Design

        [DBCommand(DBCommandType.Delete, "Campaigns_View_Design")]
        public int Campaigns_Design_Delete([DbField(DalParamType.Key)] int DesignId)
        {
            return (int)base.Execute(new object[] { DesignId });
        }

        #endregion

        #region Catalog

        [DBCommand("SELECT CatalogId,CatalogName from [Catalogs] where (AccountId=0 or  AccountId=@AccountId) and (Platform=@Platform or Platform=0)")]
        public DataTable Catalogs_List(int AccountId, int Platform)
        {
            return (DataTable)base.Execute(new object[] { AccountId, Platform });
        }

        [DBCommand("SELECT CatalogId,CatalogPageId,Title from [Catalogs_Pages] where CatalogId=@CatalogId")]
        public DataTable Catalogs_Pages_List(int CatalogId)
        {
            return (DataTable)base.Execute(new object[] { CatalogId });
        }

        [DBCommand("SELECT CatalogId,CatalogPageId,Title from [vw_Catalog_PagesList] where AccountId=@AccountId and PageCategory=@PageCategory and Platform=@Platform")]
        public DataTable Catalogs_Pages_List(int AccountId,int PageCategory, int Platform)
        {
            return (DataTable)base.Execute(new object[] { AccountId, PageCategory, Platform });
        }

        [DBCommand("SELECT * from [Catalogs_Pages] where CatalogPageId=@CatalogPageId")]
        public DataRow Catalogs_Page(int CatalogPageId)
        {
            return (DataRow)base.Execute(new object[] { CatalogPageId });
        }
        [DBCommand(DBCommandType.Delete,"Catalogs_Pages")]
        public int Catalogs_Page_Delete([DbField(DalParamType.Key)]int CatalogPageId)
        {
            return (int)base.Execute(CatalogPageId);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Catalog_Delete")]
        public int Catalogs_Delete([DbField(DalParamType.Key)]int CatalogId)
        {
            return (int)base.Execute(CatalogId);
        }

        [DBCommand(DBCommandType.Insert, "Catalogs")]
        public int Catalogs_Add
            (
              [DbField(DalParamType.Identity)]ref int CatalogId,
              [DbField]string CatalogName,
              [DbField]int AccountId,
              [DbField]int Platform
            )
        {
            object[] values = new object[] { CatalogId, CatalogName, AccountId, Platform };
            int res = (int)base.Execute(values);
            CatalogId = Types.ToInt(values[0], 0);
            return res;
        }
        #endregion

        #region Sites

        [DBCommand("SELECT * from [Sites] where SiteId=@SiteId")]
        public DataRow Sites(int SiteId)
        {
            return (DataRow)base.Execute(new object[] { SiteId});
        }


        [DBCommand("SELECT SiteId,SiteName from [Sites] where (AccountId=0 or  AccountId=@AccountId) and (Platform=@Platform or Platform=0)")]
        public DataTable Sites_List(int AccountId, int Platform)
        {
            return (DataTable)base.Execute(new object[] { AccountId, Platform });
        }

        [DBCommand("SELECT SiteId,SitePageId,Title from [Sites_Pages] where SiteId=@SiteId")]
        public DataTable Sites_Pages_List(int SiteId)
        {
            return (DataTable)base.Execute(new object[] { SiteId });
        }

        [DBCommand("SELECT SiteId,SitePageId,Title from [vw_Sites_PagesList] where AccountId=@AccountId and PageCategory=@PageCategory and Platform=@Platform")]
        public DataTable Sites_Pages_List(int AccountId, int PageCategory, int Platform)
        {
            return (DataTable)base.Execute(new object[] { AccountId, PageCategory, Platform });
        }
        
        [DBCommand("SELECT * from [Sites_Pages] where SiteId=@SiteId")]
        public DataTable Sites_Pages(int SiteId)
        {
            return (DataTable)base.Execute(new object[] { SiteId });
        }

        [DBCommand("SELECT * from [Sites_Pages] where SitePageId=@SitePageId")]
        public DataRow Sites_Page(int SitePageId)
        {
            return (DataRow)base.Execute(new object[] { SitePageId });
        }
        [DBCommand("SELECT * from [Sites_Pages] where SiteId=@SiteId and PageId=@PageId")]
        public DataRow Sites_Page(int SiteId, int PageId)
        {
            return (DataRow)base.Execute(new object[] { SiteId, PageId });
        }
        [DBCommand(DBCommandType.Delete, "Sites_Pages")]
        public int Sites_Page_Delete([DbField(DalParamType.Key)]int SitePageId)
        {
            return (int)base.Execute(SitePageId);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Sites_Delete")]
        public int Sites_Delete([DbField]int SiteId)
        {
            return (int)base.Execute(SiteId);
        }

        [DBCommand(DBCommandType.Insert, "Sites")]
        public int Sites_Add
            (
              [DbField(DalParamType.Identity)]ref int SiteId,
              [DbField]string SiteName,
              [DbField]int AccountId,
              [DbField]int Platform,
              [DbField]string SiteTitle
            )
        {
            object[] values = new object[] { SiteId, SiteName, AccountId, Platform, SiteTitle };
            int res = (int)base.Execute(values);
            SiteId = Types.ToInt(values[0], 0);
            return res;
        }
        #endregion

        #region global design

        [DBCommand("SELECT DesignId,DesignName,Preview from [Global_Design] where (AccountId=0 or  AccountId=@AccountId) and DesignType=@DesignType")]
        public DataTable GlobalDesign_List(int AccountId, int DesignType)
        {
            return (DataTable)base.Execute(new object[] { AccountId, DesignType });
        }

        [DBCommand("SELECT * from [Global_Design] where DesignId=@DesignId")]
        public DataRow GlobalDesign(int DesignId)
        {
            return (DataRow)base.Execute(new object[] { DesignId });
        }

        #endregion
    }
}

