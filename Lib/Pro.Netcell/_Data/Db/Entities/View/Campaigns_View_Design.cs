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

namespace Netcell.Data.Db.Entities
{

    [Entity("Campaigns_View_Design", "Campaigns_View_Design", "cnn_Netcell", EntityMode.Generic, "DesignId")]
    public class Campaigns_View_Design_Context : EntityContext<Campaigns_View_Design>
    {
 
        #region Ctor

        public Campaigns_View_Design_Context()
            : base()
        {

        }
        public Campaigns_View_Design_Context(int DesignId)
            : base(DesignId)
        {

        }

        public Campaigns_View_Design_Context(Campaigns_View_Design item)
            : base(item)
        {

        }
        #endregion

        #region methods

        public static int Insert(Campaigns_View_Design item)
        {
            using (Campaigns_View_Design_Context context = new Campaigns_View_Design_Context(item))
            {
                return context.SaveChanges(UpdateCommandType.Insert);
            }
        }

        public static Campaigns_View_Design Get(int designId)
        {
            using (Campaigns_View_Design_Context context = new Campaigns_View_Design_Context(designId))
            {
                return context.Entity;
            }
        }

        public static Campaigns_View_Design GetOrCreate(int designId)
        {
            if (designId > 0)
            {
                using (Campaigns_View_Design_Context context = new Campaigns_View_Design_Context(designId))
                {
                    return context.Entity;
                }
            }
            return new Campaigns_View_Design();
        }

        public static int DeleteDesign(int DesignId)
        {
            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.ExecuteNonQuery("delete from Campaigns_View_Design where DesignId=@DesignId", DataParameter.GetSql("DesignId", DesignId));
            }
        }

        #endregion
 
    }

    public class Campaigns_View_Design : IEntityItem
    {
        #region Properties

        [EntityProperty(EntityPropertyType.NA)]
        public bool IsEmpty
        {
            get { return Types.IsEmpty(DesignId); }
        }

        [EntityProperty(EntityPropertyType.Identity)]
        public int DesignId{get;set;}

        [EntityProperty(EntityPropertyType.Default)]
        public int AccountId{get;set;}
        
        [EntityProperty(EntityPropertyType.Default)]
        public string DesignName{get;set;}
        
        [EntityProperty(EntityPropertyType.Default)]
        public string Header{get;set;}
        

        [EntityProperty(EntityPropertyType.Default)]
        public string Footer{get;set;}
        
        #endregion
    }

}
