using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using ProNetcell.Data;
using Nistec.Data;
using Nistec;
using Pro.Netcell;
using System.Data;
using System.Data.SqlClient;
using Pro.Netcell.Query;
using Nistec.Web.Controls;
using System.Collections.Specialized;
using System.Web;
using Nistec.Serialization;

namespace ProNetcell.Data.Entities
{

    [Entity(EntityName = "ContactItem", MappingName = "Contacts", ConnectionKey = "cnn_pro", EntityKey = new string[] { "ContactId,AccountId" })]
    public class ContactsContext : EntityContext<ContactItem>
    {

        #region ctor

        public ContactsContext()
        {
        }

        public ContactsContext(string ContactId,int AccountId)
            : base(ContactId, AccountId)
        {
        }

        public ContactsContext(int ContactId)
            : base()
        {
            SetByParam("ContactId", ContactId);
        }

        #endregion

        #region update
        //ContactCategoryView
        public static int DoSave(ContactItem v, bool UpdateExists, string DataSource, DataSourceTypes DataSourceType)
        {

            var args = new object[] {
             "ContactId",v.ContactId
            ,"AccountId",v.AccountId
            ,"CellNumber",v.CellNumber
            ,"Email",v.Email
            ,"BirthDate",v.BirthDate
            ,"FirstName",v.FirstName
            ,"LastName",v.LastName
            ,"Address",v.Address
            ,"City",v.City
            ,"Country",v.Country
            ,"Sex",v.Sex
            ,"Details",v.Details
            ,"Phone1",v.Phone1
            ,"Company",v.Company
            ,"ContactRule",v.ContactRule
            ,"Registration",v.Registration
            ,"EnableNews",v.EnableNews
            ,"IsActive",v.IsActive
            ,"ExText1",v.ExText1
            ,"ExText2",v.ExText2
            ,"ExText3",v.ExText3
            ,"ExText4",v.ExText4
            ,"ExText5",v.ExText5
            ,"ExDate1",v.ExDate1
            ,"ExDate2",v.ExDate2
            ,"ExDate3",v.ExDate3
            ,"ExDate4",v.ExDate4
            ,"ExDate5",v.ExDate5
            ,"ExKey",v.ExKey
            ,"ExType",v.ExType
            ,"ExLang",v.ExLang
            ,"Identifier",v.Identifier
            ,"Segments",v.Segments
            ,"UpdateType",(int)ContactUpdateType.Update////tinyint = 0 -- 0=insert only,1= update full,2=update light,3=update datekey,4-sync,10,11,12=register
             //,"GroupName",v.GroupName   
                    };

            var parameters = DataParameter.GetSqlList(args);
            parameters[0].Direction = System.Data.ParameterDirection.InputOutput;
            //using (var db = DbContext.Create<DbNetcell>())
            //{
            //    var res = db.ExecuteCommandOutput("sp_Contact_Save_v1", parameters.ToArray(), System.Data.CommandType.StoredProcedure);
            //    v.ContactId = res.GetValue<int>("ContactId");
            //}

            using (var db = DbContext.Create<DbNetcell>())
            {
                int res = db.ExecuteCommandNonQuery("sp_Contacts_Items_Register", parameters.ToArray(), System.Data.CommandType.StoredProcedure);
                v.ContactId = Types.ToInt(parameters[0].Value);
                return res;
            }
        }

        public static int DoSave(string ContactId, int AccountId, ContactItem entity, UpdateCommandType commandType)
        {
            if (commandType == UpdateCommandType.Delete)
            {
                throw new ArgumentException("Delete not supported");
            }
                //using (ContactContext context = new ContactContext(ContactId, AccountId))
                //{
                //    return context.SaveChanges(commandType);
                //}

            EntityValidator.Validate(entity, "חבר", "he");

            if (commandType == UpdateCommandType.Insert)
                using (ContactsContext context = new ContactsContext())
                {
                    context.Set(entity);
                    return context.SaveChanges(commandType);
                }

            if (commandType == UpdateCommandType.Update)
                using (ContactsContext context = new ContactsContext(ContactId, AccountId))
                {
                    context.Set(entity);
                    return context.SaveChanges(commandType);
                }
            return 0;
        }

        //public static int DoDelete(string ContactId, int AccountId)
        //{
        //    var parameters = DataParameter.GetSqlWithDirection(
        //        "AccountId", AccountId, 0,
        //        "ContactId", ContactId, 0,
        //        "Result", 0, 2
        //        );
        //    int res= db.ExecuteNonQuery("sp_Contact_Remove", parameters, CommandType.StoredProcedure);
        //    return parameters.GetParameterValue<int>("Result");
        //}

        public static int DoDelete(int ContactId, int AccountId)
        {
            var parameters = DataParameter.GetSqlWithDirection(
                "AccountId", AccountId, 0,
                "ContactId", ContactId, 0,
                "Result", 0, 2
                );
            using (var db = DbContext.Create<DbNetcell>())
            {
                int res = db.ExecuteCommandNonQuery("sp_Contact_Remove_v1", parameters, CommandType.StoredProcedure);
                return parameters.GetParameterValue<int>("Result");
            }
        }

        public static int DeleteContactsByCategory( int AccountId, int Category)
        {
            //var parameters = DataParameter.GetSqlList("AccountId", AccountId, "Category", Category);
            //DataParameter.AddOutputParameter(parameters, "Result", SqlDbType.Int, 4);
            //using (var db = DbContext.Create<DbNetcell>())
            //{
            //    db.ExecuteCommandNonQuery("sp_Contacts_Delete", parameters.ToArray(), CommandType.StoredProcedure);
            //    var result = Types.ToInt(parameters[2].Value);
            //    return result;
            //}

            using (var db = DbContext.Create<DbNetcell>())
            {
                var result = db.ExecuteReturnValue("sp_Contacts_Delete", 0, "AccountId", AccountId, "Category", Category);//, "Result", 0);
                return result;

            }
        }
        #endregion

        #region static

        public static ContactItem Get(int ContactId)
        {
            using (ContactsContext context = new ContactsContext(ContactId))
            {
                return context.Entity;
            }
        }

        public static ContactItem Get(string ContactId,int AccountId)
        {
            using (ContactsContext context = new ContactsContext(ContactId,AccountId))
            {
                return context.Entity;
            }
        }

        public static ContactInfo GetInfo(int ContactId, int AccountId)
        {
            using (var db = DbContext.Create<DbNetcell>())
                return db.ExecuteSingle<ContactInfo>("sp_Contact_View ", "ContactId", ContactId, "AccountId", AccountId, "SourceType", 1);
            //return db.EntityItemGet<ContactInfo>("vw_Contacts ", "ContactId", ContactId, "AccountId", AccountId);
        }

        //ContactCategoryView
        public static ContactItem ViewOrNewContactItem(int ContactId, int AccountId)
        {
            if (ContactId > 0)
            {
                using (var db = DbContext.Create<DbNetcell>())
                    return db.ExecuteSingle<ContactItem>("sp_Contact_View ", "ContactId", ContactId, "AccountId", AccountId, "SourceType", 1);
            }
            return new ContactItem() { AccountId = AccountId };
        }

        public static string ViewContact(int ContactId, int AccountId)
        {
            using (var db = DbContext.Create<DbNetcell>())
            return db.ExecuteJsonRecord("sp_Contact_View ", "ContactId", ContactId, "AccountId", AccountId);
        }

        //public static List<ContactItem> GetItems()
        //{
        //    using (ContactContext context = new ContactContext())
        //    {
        //        return context.EntityList();
        //    }
        //}

        public static IEnumerable<ContactItem> View()
        {
            using (var db = DbContext.Create<DbNetcell>())
                return db.EntityItemList<ContactItem>(MappingName, null);
        }
        //public static IEnumerable<ContactItem> ViewByType(int ContactType)
        //{
        //    return db.EntityGetList<ContactItem>(MappingName, "ContactType=@ContactType", ContactType);
        //}
        //public static ContactItem Get(int ContactId)
        //{
        //    if (ContactId == 0)
        //        return new ContactItem();
        //    return db.EntityItemGet<ContactItem>(MappingName, "ContactId", ContactId);
        //}
        public const string MappingName = "Contacts";

        

        public static string ReadGender(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return "U";
                switch (value)
                {
                    case "זכר":
                    case "ז":
                    case "m":
                    case "M":
                        return "M";
                    case "נקבה":
                    case "נ":
                    case "f":
                    case "F":
                        return "F";
                    default:
                        return "U";
                }

            }
            catch
            {
                return "U";
            }
        }

        public static ContactUpdateType ReadContactUpdateType(int value, ContactUpdateType defaultValue)
        {
            if (Enum.IsDefined(typeof(ContactUpdateType), value))
                return (ContactUpdateType)value;
            return defaultValue;
            //Nistec.GenericTypes.ConvertEnum<ContactUpdateType>()
        }

        #endregion

        #region list

        public static IEnumerable<ContactListView> ListView(int AccountId)
        {
            using (var db = DbContext.Create<DbNetcell>())
                return db.EntityItemList<ContactListView>("vw_Contacts", "AccountId", AccountId);
        }

        //public static IEnumerable<ContactListView> View(int QueryType, int PageSize, int PageNum, int AccountId)
        //{
        //    return db.ExecuteQuery<ContactListView>("sp_Contacts_Query_Paging", "QueryType", QueryType, "@PageSize", PageSize, "PageNum", PageNum, "AccountId", AccountId);
        //}

        public static IEnumerable<ContactListView> ListQueryView(int QueryType, int PageSize, int PageNum, int AccountId,
           string ContactId = null,
            string ExId = null,
            string CellPhone = null,
            string Email = null,
           string Name = null,
           string Address = null,
           string City = null,
           string Category = null,
           string Region = null,
            //string Place = null,
           string Branch = null,
            //int Status = -1,
            int ExEnum1 = 0,
            int ExEnum2 = 0,
            int ExEnum3 = 0,
           int BirthdayMonth = 0,
           int JoinedFrom = 0,
           int JoinedTo = 0,
           int AgeFrom = 0,
           int AgeTo = 0,
           int ContactRule = 0)
        {
            using (var db = DbContext.Create<DbNetcell>())
            {
                return db.ExecuteList<ContactListView>("sp_Contacts_Query_v1", "QueryType", QueryType, "PageSize", PageSize, "PageNum", PageNum, "AccountId", AccountId,
                    "ContactId", ContactId,
                    "ExId", ExId,
                    "CellPhone", CellPhone,
                    "Email", Email,
                    "Name", Name,
                    "Address", Address,
                    "City", City,
                    "Category", Category,
                    "Region", Region,
                    //"Place", q.Place,
                    "Branch", Branch,
                    "ExEnum1", ExEnum1,
                    "ExEnum2", ExEnum2,
                    "ExEnum3", ExEnum3,
                    //"Status", q.Status,
                    "BirthdayMonth", BirthdayMonth,
                    "JoinedFrom", JoinedFrom,
                    "JoinedTo", JoinedTo,
                    "AgeFrom", AgeFrom,
                    "AgeTo", AgeTo,
                    "ContactRule", ContactRule);
            }
        }
 


        //Items = Request["Items"];
        //        ValidityRemain = Types.ToInt(Request["ValidityRemain"], 0);
        //        Campaign = Types.ToInt(Request["Campaign"], 0);
        //        SignupDateFrom = Types.ToNullableDate(Request["SignupDateFrom"]);
        //        SignupDateTo = Types.ToNullableDate(Request["SignupDateTo"]);
        //        PriceFrom = Types.ToDecimal(Request["PriceFrom"], 0);
        //        PriceTo = Types.ToDecimal(Request["PriceTo"], 0);
        //        HasAutoCharge = Request["HasAutoCharge"] == "on";
        //        HasPayment = Request["HasPayment"] == "on";
        //        HasSignup = Request["HasSignup"] == "on";

        public static IEnumerable<ContactListView> ListQueryView(ContactQuery q)
        {
            using (var db = DbContext.Create<DbNetcell>())
            {

                return db.ExecuteList<ContactListView>("sp_Contacts_Query", "QueryType", q.QueryType, "PageSize", q.PageSize, "PageNum", q.PageNum, "AccountId", q.AccountId,
                    "ContactId", q.ContactId,
                    "ExKey", q.ExKey,
                    "CellNumber", q.CellNumber,
                    "Email", q.Email,
                    "Name", q.Name,
                    "Address", q.Address,
                    "City", q.City,
                    "Category", q.Category,
                    "ExText1", q.ExText1,
                    "ExText2", q.ExText2,
                    "ExText3", q.ExText3,
                    "ExDate1", q.ExDate1,
                    "ExDate2", q.ExDate2,
                    "ExDate3", q.ExDate3,
                    "BirthdayMonth", q.BirthdayMonth,
                    "JoinedDateFrom", q.JoinedDateFrom,
                    "JoinedDateTo", q.JoinedDateTo,
                    "AgeFrom", q.AgeFrom,
                    "AgeTo", q.AgeTo,
                    "ContactRule", q.ContactRule,
                    "Sort", q.Sort,
                    "Filter", q.Filter
                    );
            }
        }

        public static DataTable ListQueryDataView(ContactQuery q)
        {
            using (var db = DbContext.Create<DbNetcell>())
            {
                return db.ExecuteCommand<DataTable>("sp_Contacts_Query", DataParameter.GetSql("QueryType", q.QueryType, "PageSize", q.PageSize, "PageNum", q.PageNum, "AccountId", q.AccountId,
                    "ContactId", q.ContactId,
                    "ExKey", q.ExKey,
                    "CellNumber", q.CellNumber,
                    "Email", q.Email,
                    "Name", q.Name,
                    "Address", q.Address,
                    "City", q.City,
                    "Category", q.Category,
                    "ExText1", q.ExText1,
                    "ExText2", q.ExText2,
                    "ExText3", q.ExText3,
                    "ExDate1", q.ExDate1,
                    "ExDate2", q.ExDate2,
                    "ExDate3", q.ExDate3,
                    "BirthdayMonth", q.BirthdayMonth,
                    "JoinedDateFrom", q.JoinedDateFrom,
                    "JoinedDateTo", q.JoinedDateTo,
                    "AgeFrom", q.AgeFrom,
                    "AgeTo", q.AgeTo,
                    "ContactRule", q.ContactRule,
                    "Sort", q.Sort,
                    "Filter", q.Filter
                    ), CommandType.StoredProcedure);
            }
        }

        public static int ListQueryTargetsView(ContactQuery q)
        {
            using (var db = DbContext.Create<DbNetcell>())
            {
                return db.ExecuteScalar<int>("sp_Contacts_Query_Targets_v2", 0, "QueryType", q.QueryType, "AccountId", q.AccountId, "UserId", q.UserId,
                    "ContactId", q.ContactId,
                    "ExKey", q.ExKey,
                    "CellNumber", q.CellNumber,
                    "Email", q.Email,
                    "Name", q.Name,
                    "Address", q.Address,
                    "City", q.City,
                    "Category", q.Category,
                    "ExText1", q.ExText1,
                    "ExText2", q.ExText2,
                    "ExText3", q.ExText3,
                    "ExDate1", q.ExDate1,
                    "ExDate2", q.ExDate2,
                    "ExDate3", q.ExDate3,
                    "BirthdayMonth", q.BirthdayMonth,
                    "JoinedDateFrom", q.JoinedDateFrom,
                    "JoinedDateTo", q.JoinedDateTo,
                    "AgeFrom", q.AgeFrom,
                    "AgeTo", q.AgeTo,
                    "ContactRule", q.ContactRule,
                    "Sort", q.Sort,
                    "Filter", q.Filter
                    );
                //"ContactRule", q.ContactRule);
            }
        }

#if(false)
        public static IEnumerable<ContactListView> ListQueryView(ContactQuery q)
        {
            using (var db = DbContext.Create<DbNetcell>())
            {
                return db.ExecuteList<ContactListView>("sp_Contacts_Query_v1", "QueryType", q.QueryType, "PageSize", q.PageSize, "PageNum", q.PageNum, "AccountId", q.AccountId,
                    "ContactId", q.ContactId,
                    "ExId", q.ExId,
                    "CellPhone", q.CellPhone,
                    "Email", q.Email,
                    "Name", q.Name,
                    "Address", q.Address,
                    "City", q.City,
                    "Category", q.Category,
                    "Region", q.Region,
                    //"Place", q.Place,
                    "Branch", q.Branch,
                    "ExEnum1", q.ExEnum1,
                    "ExEnum2", q.ExEnum2,
                    "ExEnum3", q.ExEnum3,
                    //"Status", q.Status,
                    "BirthdayMonth", q.BirthdayMonth,
                    "JoinedFrom", q.JoinedFrom,
                    "JoinedTo", q.JoinedTo,
                    "AgeFrom", q.AgeFrom,
                    "AgeTo", q.AgeTo,
                    "ContactRule", q.ContactRule,
                    "Sort", q.Sort,
                    "Filter", q.Filter);
            }
        }

        public static DataTable ListQueryDataView(ContactQuery q)
        {
            using (var db = DbContext.Create<DbNetcell>())
            {
                return db.ExecuteCommand<DataTable>("sp_Contacts_Query_v1", DataParameter.GetSql("QueryType", q.QueryType, "PageSize", q.PageSize, "PageNum", q.PageNum, "AccountId", q.AccountId,
                    "ContactId", q.ContactId,
                    "ExId", q.ExId,
                    "CellPhone", q.CellPhone,
                    "Email", q.Email,
                    "Name", q.Name,
                    "Address", q.Address,
                    "City", q.City,
                    "Category", q.Category,
                    "Region", q.Region,
                    //"Place", q.Place,
                    "Branch", q.Branch,
                    "ExEnum1", q.ExEnum1,
                    "ExEnum2", q.ExEnum2,
                    "ExEnum3", q.ExEnum3,
                    //"Status", q.Status,
                    "BirthdayMonth", q.BirthdayMonth,
                    "JoinedFrom", q.JoinedFrom,
                    "JoinedTo", q.JoinedTo,
                    "AgeFrom", q.AgeFrom,
                    "AgeTo", q.AgeTo,
                    "ContactRule", q.ContactRule), CommandType.StoredProcedure);
            }
        }

        public static int ListQueryTargetsView(ContactQuery q)
        {
            using (var db = DbContext.Create<DbNetcell>())
            {
                return db.ExecuteScalar<int>("sp_Contacts_Query_Targets", 0, "QueryType", q.QueryType, "AccountId", q.AccountId, "UserId", q.UserId,
                    "ContactId", q.ContactId,
                    "ExId", q.ExId,
                    "CellPhone", q.CellPhone,
                    "Email", q.Email,
                    "Name", q.Name,
                    "Address", q.Address,
                    "City", q.City,
                    "Category", q.Category,
                    "Region", q.Region,
                    //"Place", q.Place,
                    "Branch", q.Branch,
                    "ExEnum1", q.ExEnum1,
                    "ExEnum2", q.ExEnum2,
                    "ExEnum3", q.ExEnum3,
                    //"Status", q.Status,
                    "BirthdayMonth", q.BirthdayMonth,
                    "JoinedFrom", q.JoinedFrom,
                    "JoinedTo", q.JoinedTo,
                    "AgeFrom", q.AgeFrom,
                    "AgeTo", q.AgeTo);
                //"ContactRule", q.ContactRule);
            }
        }
#endif

        #endregion
    }

    public class ContactsContext<T> : EntityContext<DbNetcell, T> where T : IEntityItem
    {
        public static void Refresh(int AccountId)
        {
            DbContextCache.Remove<T>(Settings.ProjectName, EntityGroups.Contacts, AccountId, 0);
        }
        public ContactsContext(int accountId)
        {
            if (accountId > 0)
                CacheKey = DbContextCache.GetKey<T>(Settings.ProjectName, EntityGroups.Contacts, accountId, 0);
        }
        public IList<T> GetList()
        {
            return DbContextCache.EntityList<DbNetcell, T>(CacheKey, null);
        }
        public IList<T> GetList(int accountId)
        {
            return DbContextCache.EntityList<DbNetcell, T>(CacheKey, new object[] { "AccountId", accountId });
        }
        //protected override ContactIde void OnChanged(ProcedureType commandType)
        //{
        //    DbContextCache.Remove(CacheKey);
        //}

        protected override void OnChanged(ProcedureType commandType)
        {
            DbContextCache.Remove(CacheKey);
        }
        public FormResult GetFormResult(EntityCommandResult res, string reason)
        {
            return FormResult.Get(res, this.EntityName, reason);//  res.AffectedRecords, this.EntityName, reason, res.GetIdentityValue<int>());
        }

    }

    public class ContactListView : ContactItem, IEntityItem, IEntityListItem
    {
        public int TotalRows { get; set; }

        //public const string MappingName = "vw_Contacts";


        //[EntityProperty(EntityPropertyType.Key)]
        //public string Identifier { get; set; }
        //public string ExId { get; set; }
        //public string ContactId { get; set; }
        //[EntityProperty(EntityPropertyType.Key)]
        //public int AccountId { get; set; }
        //public string Address { get; set; }
        //public DateTime JoiningDate { get; set; }
        //public string CellPhone { get; set; }
        //public string Phone { get; set; }
        //public string Email { get; set; }
        //public string Birthday { get; set; }
        //public string Note { get; set; }
        //[EntityProperty(EntityPropertyType.Identity)]
        //public int ContactId { get; set; }


        #region view
        //public string CompanyName { get; set; }

        //public string ContactName { get; set; }
        //public string CityName { get; set; }
        //public string BranchName { get; set; }

        //[EntityProperty(EntityPropertyType.View)]
        //public DateTime LastUpdate { get; set; }
        //public string RegionName { get; set; }
        //public string GenderName { get; set; }
        //public string ExEnumName1 { get; set; }
        //public string ExEnumName2 { get; set; }
        //public string ExEnumName3 { get; set; }

        #endregion
    }


    [EntityMapping("vw_Contacts")]
    public class ContactInfo : ContactItem
    {
        //public string CityName { get; set; }
        //public string BranchName { get; set; }
        //public string GenderName { get; set; }
        //public string ExEnumName1 { get; set; }
        //public string ExEnumName2 { get; set; }
        //public string ExEnumName3 { get; set; }
    }

     [EntityMapping("vw_Contact_Display")]
    public class ContactDisplay : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Identity)]
        public int ContactId { get; set; }
        public int AccountId { get; set; }
        public string DisplayName { get; set; }
    }

    [EntityMapping("Contacts")]
    public class ContactItem : Netcell.Lib.ContactItem//, IEntityItem
    {

        [EntityProperty(EntityPropertyType.Optional)]
        public string Categories { get; set; }
                
        public string Name { get { return FirstName + " " + LastName; } }
        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<ContactItem>(this, null, null, true);
        }
        public Dictionary<string, object> ToDictionary()
        {
            return EntityDataExtension.EntityToDictionary(this, true, false); //..<ContactItem>(this, null, null, true);
        }
    }


        /*
         [EntityMapping("Contacts")]
        public class ContactItem : IEntityItem
        {
            [EntityProperty(EntityPropertyType.Key)]
            public string Identifier { get; set; }

            //[Validator(RequiredVar="@ExType=0", Name="תעודת זהות")]
            public string ContactId { get; set; }
            [EntityProperty(EntityPropertyType.Key)]
            [Validator(Required = true,Name="חשבון")]
            public int AccountId { get; set; }
            [Validator(Required = true,Name="שם פרטי")]
            public string LastName { get; set; }
            [Validator(Required = true)]
            public string FirstName { get; set; }
            //[Validator(RequiredVar = "@ExType=3")]
            public string ExId { get; set; }
            public string Address { get; set; }
            public int City { get; set; }
            public int Branch { get; set; }
            //[Validator(RequiredVar = "@ExType=1", Name = "טלפון נייד")]
            public string CellPhone { get; set; }
            public string Phone { get; set; }
            //[Validator(RequiredVar = "@ExType=2", Name = "דואל")]
            public string Email { get; set; }
            public string Gender { get; set; }
            public string Birthday { get; set; }
            public string Note { get; set; }

            [Validator(Required = true, MinValue = "2000-01-01", Name = "תאריך הצטרפות")]
            public DateTime JoiningDate { get; set; }
            public string ZipCode { get; set; }

            [EntityProperty(EntityPropertyType.View)]
            public DateTime LastUpdate { get; set; }

            [EntityProperty(EntityPropertyType.Identity)]
            public int ContactId	{ get; set; }

            public string ExField1 { get; set; }
            public string ExField2 { get; set; }
            public string ExField3 { get; set; }

            public int ExEnum1 { get; set; }
            public int ExEnum2 { get; set; }
            public int ExEnum3 { get; set; }
            public bool? EnableNews { get; set; }

            public string DataSource { get; set; }
            public int DataSourceType { get; set; }
            public int ContactType { get; set; }
            public string CompanyName { get; set; }
            public int ExRef1 { get; set; }
            public int ExRef2 { get; set; }
            public int ExRef3 { get; set; }

            [EntityProperty(EntityPropertyType.Optional)]
            public string Categories { get; set; }

            public string DisplayName
            {
                get
                {
                    if (ContactType == 1)
                        return CompanyName;
                    return FirstName + " " + LastName;
                }
            }

            public string ContactName { get { return FirstName + " " + LastName; } }
            public string ToHtml()
            {
                return EntityProperties.ToHtmlTable<ContactItem>(this,null, null, true);
            }
            public Dictionary<string,object> ToDictionary()
            {
                return EntityDataExtension.EntityToDictionary(this,true,false); //..<ContactItem>(this, null, null, true);
            }

        }
        */
        public enum ContactUpdateType
     {
         InsertOnly = 0,
         Sync = 1,
         Update = 2
     }
     public enum ContactTypes
     {
         Private = 0,
         Business = 1
     }

     public enum DataSourceTypes
     {
         CoSystem = 0, Register = 1, FileSync = 2, ApiSync = 3
     }
     public enum EnableNewsState
     {
         NotSet = -1, Disable = 0, Enable = 1
     }
     public enum PlatformType
     {
         NA = 0, Cell = 1, Mail = 2
     }
     public enum ContactKeyType
     {
         None = -1, ContactId = 0, Cell = 1, Mail = 2, Target = 3, Key = 4
     }



    [Serializable]
    public class ContactQuery : QueryBase
    {

        public string Serialize()
        {
            var ser = BinarySerializer.SerializeToBase64(this);
            return ser;
        }

        public static ContactQuery Deserialize(string ser)
        {
            return BinarySerializer.DeserializeFromBase64<ContactQuery>(ser);
        }

        public int QueryType { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public int ExType { get; set; }
        public ContactQuery()
        {
            ContactId = 0;
            ExKey =null;
            ExType = 0;
            CellNumber = null;
            Name = null;
            Address = null;
            City = null;
            Category = null;
            //Region = 0;
            //Place = null;
            //Branch = null;
            //Status = -1;
            ExText1 = null;
            ExText2 = null;
            ExText3 = null;
            ExDate1 = null;
            ExDate2 = null;
            ExDate3 = null;
            BirthdayMonth = 0;
            JoinedDateFrom = null;
            JoinedDateTo = null;
            AgeFrom = 0;
            AgeTo = 0;
            ContactRule = 0;
        }

        public ContactQuery(HttpRequestBase Request)
        {

            QueryType = QueryType = Types.ToInt(Request["QueryType"]);
            AccountId = Types.ToInt(Request["AccountId"]);
            //ExType = Types.ToInt(Request["ExType"]);

            //if (QueryType == 1)
            //{
            ContactId = Types.ToInt(Request["ContactId"]);
            ExKey = Types.NZorEmpty(Request["ExKey"]);
            Email = Types.NZorEmpty(Request["Email"]);
            CellNumber = Types.NZorEmpty(Request["CellNumber"]);
            Category = Types.NZorEmpty(Request["Category"]);
            //Branch = Request["Branch"];
            //}
            //else
            //{

            Name = Types.NZorEmpty(Request["Name"]);
            Address = Types.NZorEmpty(Request["Address"]);
            City = Types.NZorEmpty(Request["City"]);
            //Region = Types.ToInt(Request["Region"]);
            //Branch = Request["Branch"];
            ExText1 = Types.NZorEmpty(Request["ExText1"]);
            ExText2 = Types.NZorEmpty(Request["ExText2"]);
            ExText3 = Types.NZorEmpty(Request["ExText3"]);
            ExDate1 = Types.NZorEmpty(Request["ExDate1"]);
            ExDate2 = Types.NZorEmpty(Request["ExDate2"]);
            ExDate3 = Types.NZorEmpty(Request["ExDate3"]);

            BirthdayMonth = Types.ToInt(Request["BirthdayMonth"]);
            DateType = Types.ToInt(Request["DateType"]);
            JoinedDateFrom = Types.ToNullableDateIso(Request["JoinedDateFrom"]);
            JoinedDateTo = Types.ToNullableDateIso(Request["JoinedDateTo"]);
            AgeFrom = Types.ToInt(Request["AgeFrom"]);
            AgeTo = Types.ToInt(Request["AgeTo"]);
            ContactRule = Types.ToInt(Request["ContactRule"]);

            //signup query
            //Items = Request["Items"];
            //ReferralCode = Request["ReferralCode"];
            //ValidityRemain = Types.ToInt(Request["ValidityRemain"], 0);
            //Campaign = Types.ToInt(Request["Campaign"], 0);
            //SignupDateFrom = Types.ToNullableDate(Request["SignupDateFrom"]);
            //SignupDateTo = Types.ToNullableDate(Request["SignupDateTo"]);
            //PriceFrom = Types.ToDecimal(Request["PriceFrom"], 0);
            //PriceTo = Types.ToDecimal(Request["PriceTo"], 0);
            //HasAutoCharge = Types.ToInt(Request["HasAutoCharge"]);// == "on";
            //HasPayment = Types.ToInt(Request["HasPayment"]);// == "on";
            //HasSignup = Types.ToInt(Request["HasSignup"]);// == "on";

            //}
            LoadSortAndFilter(Request);
        }

        public ContactQuery(NameValueCollection Request, int queryType)
        {

            QueryType = queryType;

            if (Request.Count == 0)
                return;

            //string query_type = Request["query_type"];
            // Types.ToInt(Request["QueryType"]);
            //ExType = Types.ToInt(Request["ExType"]);

            if (queryType == 1)
            {
                ContactId = Types.ToInt(Request["ContactId"]);
                ExKey = Types.NZorEmpty(Request["ExKey"]);
                Email = Types.NZorEmpty(Request["Email"]);
                CellNumber = Types.NZorEmpty(Request["CellNumber"]);
                Category = Types.NZorEmpty(Request["Category"]);
                //Branch = Request["Branch"];
            }
            else
            {
                ContactId = Types.ToInt(Request["ContactId"], 0);
                if (ContactId != 0)
                {
                    QueryType = 1;
                    return;
                }

                ExKey = Types.NZorEmpty(Request["ExKey"], null);
                if (ExKey != null)
                {
                    QueryType = 1;
                    return;
                }

                CellNumber = Types.NZorEmpty(Request["CellNumber"], null);
                if (CellNumber != null)
                {
                    QueryType = 1;
                    return;
                }

                Email = Types.NZorEmpty(Request["Email"], null);
                if (Email != null)
                {
                    QueryType = 1;
                    return;
                }

                //string listBranch = Request["Branch"];
                //bool allBranch = Request["allBranch"] == "on";
                //if (!allBranch)
                //    Branch = listBranch;

                //string listRegion = Request["Region"];
                //bool allRegion = Request["allRegion"] == "on";
                //if (!allRegion)
                //    Region = Types.ToInt(listRegion);

                //string listCity = Request["City"];
                //bool allCity = Request["allCity"] == "on";
                //if (!allCity)
                //    City = listCity;

                string listCategory = Request["Category"];
                bool allCategory = Request["allCategory"] == "on";
                if (!allCategory)
                    Category = listCategory;

                City = Types.NZorEmpty(Request["City"], null);
                Address = Types.NZorEmpty(Request["Address"], null);

                bool allBirthday = Request["allBirthday"] == "on";
                if (allBirthday)
                    BirthdayMonth = DateTime.Now.Month;



                //int monthlyTime = Types.ToInt(Request["monthlyTime"]);
                //if (monthlyTime > 0)
                //{
                //    JoinedFrom = monthlyTime;
                //    JoinedTo = 0;
                //}
                bool allCell = Request["allCell"] == "on";
                bool allEmail = Request["allEmail"] == "on";

                if (allCell && allEmail)
                    ContactRule = 3;
                if (allCell)
                    ContactRule = 1;
                if (allEmail)
                    ContactRule = 2;
                else
                    ContactRule = 0;

                AgeFrom = Types.ToInt(Request["AgeFrom"]);
                AgeTo = Types.ToInt(Request["AgeTo"]);

                DateType = Types.ToInt(Request["DateType"]);
                JoinedDateFrom = Types.ToNullableDateIso(Request["JoinedDateFrom"]);
                JoinedDateTo = Types.ToNullableDateIso(Request["JoinedDateTo"]);

                //signup query
                //string listItems = Request["Items"];
                //bool allItems = Request["allItems"] == "on";
                //if (!allItems)
                //    Items = listItems;
                //ReferralCode = Request["ReferralCode"];

                //string val = "";
                //val = Request["ValidityRemain"];
                //ValidityRemain = (val == "") ? 0 : Types.ToInt(Request["ValidityRemain"], 0);
                //val = Request["Campaign"];
                //Campaign = (val == "") ? 0 : Types.ToInt(Request["Campaign"], 0);

                //SignupDateFrom = Types.ToNullableDate(Request["SignupDateFrom"]);
                //SignupDateTo = Types.ToNullableDate(Request["SignupDateTo"]);

                //PriceFrom = Types.ToDecimal(Request["PriceFrom"], 0);
                //PriceTo = Types.ToDecimal(Request["PriceTo"], 0);

                //HasAutoCharge = Types.ToInt(Request["HasAutoCharge"]);// == "on";
                //HasPayment = Types.ToInt(Request["HasPayment"]);// == "on";
                //HasSignup = Types.ToInt(Request["HasSignup"]);// == "on";
            }

        }


        public void Normelize()
        {
 
            if (ExKey == "")
                ExKey = null;
            if (CellNumber == "")
                CellNumber = null;
            if (Email == "")
                Email = null;
            if (Name == "")
                Name = null;
            if (Address == "")
                Address = null;
            if (City == "")
                City = null;
            if (Category == "")
                Category = null;
            //if (Region == "")
            //    Region = null;
            //if (Place == "")
            //    Place = null;
            //if (Branch == "")
            //    Branch = null;

            //ContactId = SqlFormatter.ValidateSqlInput(ContactId);
            Name = SqlFormatter.ValidateSqlInput(Name);
            Address = SqlFormatter.ValidateSqlInput(Address);
            //Filter = SqlFormatter.ValidateSqlInput(Filter);

            //if (!string.IsNullOrEmpty(City))
            //    Region = 0;

            //signup query
            //Items = SqlFormatter.ValidateSqlInput(Items);
            //if (ReferralCode == "")
            //    ReferralCode = null;
        }


//public int QueryType { get; set; }
//public int PageSize { get; set; }
//public int PageNum i{get;set;}
//public int AccountId i{get;set;}
public int ContactId { get; set; }
//--public string MembeContactId { get; set; }
public string ExKey { get; set; }
public string CellNumber { get; set; }
public string Email { get; set; }
public string Name { get; set; }
public string Address { get; set; }
public string City { get; set; }
public string Category { get; set; }
//public int Region { get; set; }
//public string Branch { get; set; }
public string ExText1 { get; set; }
public string ExText2 { get; set; }
public string ExText3 { get; set; }
public string ExDate1 { get; set; }
public string ExDate2 { get; set; }
public string ExDate3 { get; set; }

public int BirthdayMonth { get; set; }
public int DateType { get; set; }//--0=nome,1=[CreationDate], 2=[RegisterDate]
public DateTime? JoinedDateFrom { get; set; }
public DateTime? JoinedDateTo { get; set; }
public int AgeFrom { get; set; }
public int AgeTo { get; set; }
public int ContactRule { get; set; }
public string Sort { get; set; }
public string Filter { get; set; }

        /*

        public string ContactId { get; set; }
        public string ExId { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Category { get; set; }
        public int Region { get; set; }
        public string Branch { get; set; }

        //public int Status{ get; set; }
        //public string Place{ get; set; }

        public int ExEnum1 { get; set; }
        public int ExEnum2 { get; set; }
        public int ExEnum3 { get; set; }

        public int BirthdayMonth { get; set; }
        public int JoinedFrom { get; set; }
        public int JoinedTo { get; set; }
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public int ContactRule { get; set; }
        //public int QueryType;

        //signup query
        public string Items { get; set; }
        public string ReferralCode { get; set; }
        public int ValidityRemain { get; set; }
        public int Campaign { get; set; }
        public DateTime? SignupDateFrom { get; set; }
        public DateTime? SignupDateTo { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        public int HasSignup { get; set; }
        public int HasPayment { get; set; }
        public int HasAutoCharge { get; set; }
        */
    }
}
