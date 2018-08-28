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
    #region enums
    public enum RegistryActionType
    {
        Register=0,
        Block=1
    }

    public enum EnableNewsState
    {
        NoSet=0,
        Enable=1,
        Disable=2
    }
    #endregion

    [Entity("Registry_Items", "Registry_Items", "cnn_Netcell", EntityMode.Generic, "RegisterId")]
    public class Registry_Items_Context : EntityContext<Registry_Items>
    {
        #region ctor
        public Registry_Items_Context(int RegisterId)
            : base(RegisterId)
        {

        }

        public Registry_Items_Context(Registry_Items item)
            : base(item)
        {

        }
        private Registry_Items_Context()
            : base()
        {
        }
        #endregion

        #region binding

        protected override void EntityBind()
        {
            //base.EntityDb.EntityCulture = Netcell.Data.DB.NetcellDB.GetCulture();
            //If EntityAttribute not define you can initilaize the entity here
            //base.InitEntity<AdventureWorks>("Contact", "Person.Contact", EntityKeys.Get("ContactID"));
        }

        #endregion

        #region methods
        
        public static int InsertItem(int formId, string name, string cli, string email, string company, string details, EnableNewsState enableNews, RegistryActionType actionType, string args)
        {
            int registerId = 0;
            using (DalRegistry dal = new DalRegistry())
            {
                int res = dal.Registry_Items_Insert(ref registerId, formId, name, cli, email, company, details, (int)enableNews, (int)actionType, args);
            }
            return registerId;
        }

        public static int Insert(Registry_Items item)
        {
            using (Registry_Items_Context context = new Registry_Items_Context(item))
            {
               return context.SaveChanges(UpdateCommandType.Insert);
            }
        }

        public static Registry_Items Get(int registerId)
        {
            using (Registry_Items_Context context = new Registry_Items_Context(registerId))
            {
                return context.Entity;
            }
        }
        
        public static List<Registry_Items> GetListItems(int formId)
        {
            using (Registry_Items_Context context = new Registry_Items_Context())
            {
                return context.EntityList(DataFilter.Get("FormId=@FormId", formId));
            }
        }

        public static DataTable GetList(int formId)
        {
            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.ExecuteCommand<DataTable>("select * from Registry_Items where FormId=@FormId", DataParameter.GetSql("FormId", formId));
            }
        }
        #endregion
    }

    public class Registry_Items : IEntityItem
    {

        //==============================================

        public static Registry_Items Get(Dictionary<string, string> dic, int formId)
        {
            Registry_Items ri = new Registry_Items()
            {
                FormId = formId,
                Name = dic["Name"],
                Cli = dic["Cli"],
                Email = dic["Email"],
                Company = dic["Company"],
                Details = dic["Details"],
                EnableNews = (EnableNewsState)Types.ToInt(dic["EnableNews"]),
                ActionType = Types.ToInt(dic["ActionType"]),
                Args = dic["Args"],
                Creation = DateTime.Now,
                LastUpdate = DateTime.Now
            };
            return ri;
        }

        #region Properties

        [EntityProperty(EntityPropertyType.Identity, Caption = "רשומה")]
        public int RegisterId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מס-טופס")]
        public int FormId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "שם")]
        public string Name
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "טלםון")]
        public string Cli
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "דואל")]
        public string Email
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "חברה\\ארגון")]
        public string Company
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "פרטים")]
        public string Details
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "נוצר ב")]
        public DateTime Creation
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "עודכן ב")]
        public DateTime LastUpdate
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "האם לאפשר דיוור")]
        public EnableNewsState EnableNews
        {
            get;
            set;
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "סוג-פעולה")]
        public int ActionType
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "ארגומנטים")]
        public string Args
        {
            get;
            set;
        }

        [EntityProperty(EntityPropertyType.Default, Caption = "סטטוס")]
        public int Status
        {
            get;
            set;
        }

        #endregion

        #region methods

        public string ToHtml(string formName, string powerdBy, string formTitle)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("<p>{0}</p>", formTitle);
            sb.Append("<ul>");
            sb.AppendFormat("<li>Form: {0}</li>", formName);
            sb.AppendFormat("<li>Name: {0}</li>", Name);
            sb.AppendFormat("<li>Cli: {0}</li>", Cli);
            sb.AppendFormat("<li>Email: {0}</li>", Email);
            sb.AppendFormat("<li>Company: {0}</li>", Company);
            sb.AppendFormat("<li>Details: {0}</li>", Details);
            sb.AppendFormat("<li>EnableNews: {0}</li>", EnableNews);
            sb.AppendFormat("<li>ActionType: {0}</li>", ActionType);
            sb.AppendFormat("<li>Args: {0}</li>", Args);
            sb.Append("</ul>");
            sb.Append(powerdBy);
            return sb.ToString();
        }

        #endregion

        #region context

        public static EntityDbContext GetDb()
        {
            return EntityDbContext.Get<NetcellDB>("Registry_Items", EntityKeys.Get("RegisterId"));
        }

        public static int Insert(Registry_Items item)
        {
            using (EntityContext<Registry_Items> context = new EntityContext<Registry_Items>(item))
            {
                context.EntityDb = GetDb();
                //context.SetDb<NetcellDB>("Registry_Items", EntitySourceType.Table, EntityKeys.Get("RegisterId"));
                return context.SaveChanges(UpdateCommandType.Insert);
            }
        }

        public static Registry_Items Get(int registerId)
        {
            using (EntityContext<Registry_Items> context = new EntityContext<Registry_Items>())
            {
                context.EntityDb = GetDb();
                //context.Set(registerId);
                //context.SetDb<NetcellDB>("Registry_Items", EntitySourceType.Table, EntityKeys.Get("RegisterId"));
                return context.Entity;
            }
        }

        #endregion
    }
 
}
