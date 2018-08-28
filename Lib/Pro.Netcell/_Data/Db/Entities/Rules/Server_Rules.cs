using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Netcell.Data;
using Nistec;
using Netcell;
using Nistec.Data.Entities;
using Nistec.Data;
using Nistec.Data.Factory;
using Netcell.Data.Server;
using Nistec.Generic;

namespace Netcell.Data.Db.Entities
{


    [Entity("Server_Rules", "App_Servers_Rules", "cnn_Netcell", EntityMode.Generic, "AccountId,Platform")]
    public class Server_Rules_Context : EntityContext<Server_Rules>
    {
        #region ctor
        public Server_Rules_Context(int AccountId,int Platform)
            : base(AccountId, Platform)
        {

        }
        public Server_Rules_Context(DataFilter filter)
            : base()
        {
            Init(filter);
        }
        public Server_Rules_Context(Server_Rules item)
            : base(item)
        {

        }
        protected Server_Rules_Context()
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

        public static Server_Rules Get(int AccountId, int Platform)
        {
            using (Server_Rules_Context context = new Server_Rules_Context(AccountId, Platform))
            {
                return context.Entity;
            }
        }

        public static Server_Rules GetRule(int AccountId, int Platform)
        {
            using (Server_Rules_Context context = new Server_Rules_Context())
            {
                context.Set(@"select top 1 * from App_Servers_Rules 
                    where (AccountId=@AccountId or AccountId=@AccountAlt) 
                    and Platform=@Platform 
                    order by AccountId desc",
                    DataParameter.GetSql("AccountId", AccountId, "AccountAlt", 0, "Platform", Platform),
                    CommandType.Text 
                    );
                return context.Entity;
            }
        }

        public static int Lookup_Server_Rules(int AccountId, int Platform)
        {
            int server = 0;
            using (DalRule dal = new DalRule())
            {
                dal.Server_Rules(AccountId, Platform, ref server);
            }
            return server;
        }

        public static Server_Rules Get(DataRow dr)
        {
            using (Server_Rules_Context context = new Server_Rules_Context())
            {
                context.EntityRecord = new GenericRecord(dr);
                return context.Entity;
            }
        }

        public static IList<Server_Rules> GetListItems()
        {
            using (Server_Rules_Context context = new Server_Rules_Context())
            {
                return context.EntityList();
            }
        }

        public static DataTable GetList()
        {
            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.ExecuteCommand<DataTable>("select * from App_Servers_Rules");
            }
        }
       
        public static Server_Rules Deserialize(string base64)
        {
            return Nistec.Serialization.BinarySerializer.DeserializeFromBase64<Server_Rules>(base64);
        }

        #endregion
    }

    [Serializable]
    public class Server_Rules : IEntityItem
    {

        public Server_Rules() { }

        public Server_Rules(DataRow dr)
        {
            AccountId = dr.Field<int>("AccountId");
            Platform = dr.Field<int>("Platform");
            Server = dr.Field<int>("Server");
        }

        #region Ex Properties

        public bool IsEmpty
        {
            get { return Types.IsEmpty(AccountId); }
        }

        public string Key
        {
            get { return string.Format("{0}_{1}", AccountId, Platform); }
        }

        #endregion

        #region Properties

        [EntityProperty(EntityPropertyType.Key, Caption = "חשבון")]
        public int AccountId
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
        [EntityProperty(EntityPropertyType.Default, Caption = "שרת")]
        public int Server
        {
            get;
            set;
        }

        #endregion

        #region context
        #endregion

        #region methods

        public string Serialize()
        {
            return Nistec.Serialization.BinarySerializer.SerializeToBase64(this);
        }

        public string Print()
        {
            return string.Format("Server_Rules Item AccountId:{0}, Platform:{1}, Server:{2}", AccountId, Platform, Server);
        }

        #endregion
    }

}
