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
using Netcell.Data;
using Nistec.Data.Factory;

namespace Netcell.Data.Client
{

    public class DalWap : DbCommand
    {
        // Methods
        public DalWap()
            : base(DBRule.CnnNetcell)
        {
        }

        // Properties
        public static DalWap Instance
        {
            get
            {
                return new DalWap();
            }
        }
        //[DBCommand("Select * from Campaigns_Wap where CampaignId=@CampaignId Order By PageId")]
        //public DataTable Campagins_Wap(int CampaignId)
        //{
        //    return (DataTable)base.Execute(new object[] { CampaignId });
        //}

        //[DBCommand("Select * from Campaigns_Wap where CampaignId=@CampaignId and PageId=@PageId")]
        //public DataRow Campagins_WapItem(int CampaignId, int PageId)
        //{
        //    return (DataRow)base.Execute(new object[] { CampaignId, PageId });
        //}

        [DBCommand("Select * from vw_Campaign_DataItem where Id=@Id")]
        public DataRow Campaign_DataItem(int Id)
        {
            return (DataRow)base.Execute(new object[] { Id });
        }

        [DBCommand(DBCommandType.Delete, "Campaigns_View_Design")]
        public int Campaigns_Design_Delete([DbField(DalParamType.Key)] int DesignId)
        {
            return (int)base.Execute(new object[] { DesignId });
        }

        [DBCommand(DBCommandType.Insert, "Campaigns_View_Design")]
        public int Campaigns_Design_Insert
            (
            [DbField(DalParamType.Identity)] ref int DesignId, 
            [DbField] string DesignName, 
            [DbField] int AccountId, 
            [DbField] string Body
            )
        {
            object[] objArray = new object[] { (int)DesignId, DesignName, AccountId, Body };
            object obj2 = base.Execute(objArray);
            DesignId = Types.ToInt(objArray[0], 0);
            return Types.ToInt(obj2, 0);
        }

        

        [DBCommand("Select * from Campaigns_View_Design where DesignId=@DesignId")]
        public DataRow Campaigns_View_Design(int DesignId)
        {
            return (DataRow)base.Execute(new object[] { DesignId });
        }

        [DBCommand("SELECT DesignId,DesignName from [Campaigns_View_Design] where (AccountId=0 or  AccountId=@AccountId)")]
        public DataTable Campaigns_View_Design_List(int AccountId)
        {
            return (DataTable)base.Execute(new object[] { AccountId });
        }

       
        public int Campaigns_Wap_Duplicate(string DesignName, int DesignId)
        {
            string str = string.Format("insert into [Campaigns_View_Design](DesignName,AccountId,Header,Footer) select N'{0}',AccountId,Header,Footer from [Campaigns_View_Design] where (DesignId={1})", DesignName, DesignId);
            return base.ExecuteNonQuery(str);
        }

        //[DBCommand(DBCommandType.StoredProcedure, "sp_Campaign_Wap_Render")]
        //public DataRow Campaigns_Wap_Item_Render([DbField] int SentId, [DbField] int PageId, [DbField] string UA, [DbField] bool IsMobile, [DbField] int Version)
        //{
        //    return (DataRow)base.Execute(new object[] { SentId, PageId, UA, IsMobile, Version });
        //}

        //[DBCommand(DBCommandType.StoredProcedure, "sp_CampaignsWapRender")]
        //public DataRow CampaignsWapRender(int Id)
        //{
        //    return (DataRow)base.Execute(new object[] { Id });
        //}

       
        public int GetDesignId(int campaignId)
        {
            return base.Dlookup<int>("DesignId", "Campaigns", "CampaignId=" + campaignId.ToString(), 0);
        }

        public string Lookup_Campaigns_View_Design(int DesignId)
        {
            return base.Dlookup<string>("Body", "Campaigns_View_Design", "DesignId=" + DesignId.ToString(), null);
        }

   
        //[DBCommand(DBCommandType.StoredProcedure, "sp_Wap_ContentRender")]
        //public DataTable Wap_ContentRender([DbField] int RegisterId, [DbField] int ItemCode, [DbField] string UA, [DbField] bool IsMobile)
        //{
        //    return (DataTable)base.Execute(new object[] { RegisterId, ItemCode, UA, IsMobile });
        //}

        //[DBCommand(DBCommandType.StoredProcedure, "sp_Wap_Session_Render")]
        //public DataRow Wap_SessionRender([DbField] int RegisterId, [DbField] int Sign, [DbField] int Session, [DbField] string UA, [DbField] bool IsMobile)
        //{
        //    return (DataRow)base.Execute(new object[] { RegisterId, Sign, Session, UA, IsMobile });
        //}

        //[DBCommand(DBCommandType.StoredProcedure, "sp_Campaigns_WapTrans_Render")]
        //public DataRow Wap_TransItem_Render([DbField] int Id, [DbField] int PageId, [DbField] string UA, [DbField] bool IsMobile)
        //{
        //    return (DataRow)base.Execute(new object[] { Id, PageId, UA, IsMobile });
        //}

        //[DBCommand(DBCommandType.StoredProcedure, "sp_WapTransRender")]
        //public DataRow WapTrans([DbField(DalParamType.Key)] int ItemId)
        //{
        //    return (DataRow)base.Execute(new object[] { ItemId });
        //}

 
    }
    

}

