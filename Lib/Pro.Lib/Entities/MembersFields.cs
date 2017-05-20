using Nistec.Data.Entities;
using Pro.Data;
using Pro.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pro.Data.Entities
{
    public class MembersFieldsContext
    {
        const string TableName = "Members_Fields";
        public static string LookupLabelField(int AccountId, string field)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.QueryScalar<string>("select " + field + " from [" + TableName + "] where AccountId=@AccountId", null, "AccountId", AccountId);
        }
        public static string LookupLabelExId(int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.QueryScalar<string>("select ExId from [" + TableName + "] where AccountId=@AccountId", null, "AccountId", AccountId);
        }
        public static int LookupLabelExType(int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.QueryScalar<int>("select ExType from [" + TableName + "] where AccountId=@AccountId", 0, "AccountId", AccountId);
        }

        public static MembersFields MembersFieldView(int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.QuerySingle<MembersFields>("select * from [" + TableName + "] where AccountId=@AccountId", "AccountId", AccountId);
        }

        public static MembersFields GetMembersFields(int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                MembersFields mf = EntityPro.CacheGetOrCreate(EntityPro.CacheKey(EntityGroups.Enums, AccountId, TableName), () => MembersFieldView(AccountId));
                return mf;
            }
        }
         public static IList<MembersEnumFields> MembersEnumFieldView(int AccountId, string FieldType)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.ExecuteList<MembersEnumFields>("sp_Members_Fields", "AccountId", AccountId, "FieldType", FieldType);
        }
         public static IList<MembersEnumFields> GetMembersEnumFields(int AccountId)
         {
             using (var db = DbContext.Create<DbPro>())
             {
                 IList<MembersEnumFields> mf = EntityPro.CacheGetOrCreate(EntityPro.CacheKey(EntityGroups.Enums, AccountId, TableName), () => MembersEnumFieldView(AccountId, "entityenum"));
                 return mf;
             }
        }

    }
    
    public class MembersEnumFields : IEntityItem
    {
        //public int AccountId { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string PropName { get; set; }
        public string PropId { get; set; }
    }
    public class MembersFields : IEntityItem
    {
        public int AccountId { get; set; }
        public int ExType { get; set; }
        public string ExId { get; set; }
        public string ExField1 { get; set; }
        public string ExField2 { get; set; }
        public string ExField3 { get; set; }
        public string ExEnum1 { get; set; }
        public string ExEnum2 { get; set; }
        public string ExEnum3 { get; set; }
        public string ExDate1 { get; set; }
        public string ExDate2 { get; set; }
        public string ExDate3 { get; set; }
        public string ExText1 { get; set; }
        public string ExText2 { get; set; }
        public string ExText3 { get; set; }


    }
}
