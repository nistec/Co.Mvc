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
using Nistec.Generic;

namespace Netcell.Data.Db.Entities
{


    [Entity("Carrier_Rules", "Carrier_Rules", "cnn_Netcell", EntityMode.Generic, "AccountId,Platform,Charset,Country")]
    public class Carrier_Rules_Context : EntityContext<Carrier_Rules>
    {
        #region ctor
        public Carrier_Rules_Context(int AccountId,int Platform,int Charset,int Country)
            : base(AccountId, Platform, Charset, Country)
        {

        }
        public Carrier_Rules_Context(DataFilter filter)
            : base()
        {
            Init(filter);
        }
        public Carrier_Rules_Context(Carrier_Rules item)
            : base(item)
        {

        }
        protected Carrier_Rules_Context()
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

        public static Carrier_Rules Get(int AccountId, int Platform, int Charset, int Country)
        {
            using (Carrier_Rules_Context context = new Carrier_Rules_Context(AccountId, Platform, Charset, Country))
            {
                return context.Entity;
            }
        }

        public static Carrier_Rules GetRule(int AccountId, int Platform, int Charset, int Country)
        {
            using (Carrier_Rules_Context context = new Carrier_Rules_Context())
            {
                context.Set(@"select top 1 * from Carrier_Rules 
                    where (AccountId=@AccountId or AccountId=@AccountAlt) 
                    and Platform=@Platform 
                    and Charset=@Charset 
                    and Country=@Country 
                    order by AccountId desc",
                    DataParameter.GetSql("AccountId", AccountId, "AccountAlt", 0, "Platform", Platform, "Charset", Charset, "Country", Country),
                    CommandType.Text 
                    );
                return context.Entity;
            }

            //            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            //            {
            //                return cmd.ExecuteCommand<Carrier_Rules>(
            //                    @"select top 1 * from Carrier_Rules 
            //                    where (AccountId=@AccountId or AccountId=@AccountAlt) 
            //                    and Platform=@Platform 
            //                    and Charset=@Charset 
            //                    and Country=@Country 
            //                    order by AccountId desc", 
            //                    DataParameter.Get("AccountId",AccountId,"AccountAlt",0, "Platform",Platform, "Charset",Charset, "Country",Country)
            //                    );
            //            }
        }

        public static Carrier_Rules Get(DataRow dr)
        {
            using (Carrier_Rules_Context context = new Carrier_Rules_Context())
            {
                context.EntityRecord = new GenericRecord(dr);
                return context.Entity;
            }
        }

        public static IList<Carrier_Rules> GetListItems()
        {
            using (Carrier_Rules_Context context = new Carrier_Rules_Context())
            {
                return context.EntityList();
            }
        }

        public static DataTable GetList()
        {
            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.ExecuteCommand<DataTable>("select * from Carrier_Rules");
            }
        }
       
        public static Carrier_Rules Deserialize(string base64)
        {
            return Nistec.Serialization.BinarySerializer.DeserializeFromBase64<Carrier_Rules>(base64);
        }

        #endregion
    }

    [Serializable]
    public class Carrier_Rules : IEntityItem
    {

        public Carrier_Rules() { }

        public Carrier_Rules(DataRow dr)
        {
            AccountId = dr.Field<int>("AccountId");
            Platform = dr.Field<int>("Platform");
            Charset = dr.Field<int>("Charset");
            Country = dr.Field<int>("Country");
            MinUnitSize = dr.Field<int>("MinUnitSize");
            SizePerUnit = dr.Field<int>("SizePerUnit");
            MaxSize = dr.Field<int>("MaxSize");
        }

        #region Ex Properties

        public bool IsEmpty
        {
            get { return Types.IsEmpty(Platform); }
        }

        public string Key
        {
            get { return string.Format("{0}_{1}_{2}_{3}", AccountId, Platform, Charset, Country); }
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
        [EntityProperty(EntityPropertyType.Default, Caption = "קידוד")]
        public int Charset
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מדינה")]
        public int Country
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "ברוקר")]
        public int CarrierId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מינימום לחיוב")]
        public int MinUnitSize
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "יחס גודל ליח-חיוב")]
        public int SizePerUnit
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מקסימום גודל")]
        public int MaxSize
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
            return string.Format("Carrier_Rules Item AccountId:{0}, Platform:{1}, CarrierId:{2}", AccountId, Platform, CarrierId);
        }

        #endregion
    }

}
