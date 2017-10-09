using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Data;
using Nistec.Data;
using Nistec;
using Pro.Lib;
using System.Data;
using System.Data.SqlClient;
using Pro.Lib.Query;
using Nistec.Web.Controls;


namespace Pro.Data.Entities
{

    [Entity(EntityName = "MemberItem", MappingName = "Members", ConnectionKey = "cnn_pro", EntityKey = new string[] { "MemberId,AccountId" })]
    public class MemberContext : EntityContext<MemberItem>
    {

        #region ctor

        public MemberContext()
        {
        }

        public MemberContext(string MemberId,int AccountId)
            : base(MemberId, AccountId)
        {
        }

        public MemberContext(int RecordId)
            : base()
        {
            SetByParam("RecordId", RecordId);
        }

        #endregion

        #region update
        //MemberCategoryView
        public static int DoSave(MemberItem v, bool UpdateExists ,string DataSource,DataSourceTypes DataSourceType)
        {

            var args = new object[]{
                "RecordId", v.RecordId
                ,"AccountId", v.AccountId
                ,"MemberId", v.MemberId
                ,"LastName", v.LastName
                ,"FirstName", v.FirstName
                ,"Address", v.Address
                ,"City", v.City
                ,"CellPhone",v.CellPhone
                ,"Phone", v.Phone
                ,"Email", v.Email
                ,"Gender", v.Gender
                ,"Birthday", v.Birthday
                ,"Note", v.Note
                ,"Branch", v.Branch
                ,"ZipCode", v.ZipCode
                ,"ContactRule", 0
                ,"Categories", v.Categories
                ,"ExField1", v.ExField1
                ,"ExField2", v.ExField2
                ,"ExField3", v.ExField3
                ,"ExEnum1", v.ExEnum1
                ,"ExEnum2", v.ExEnum2
                ,"ExEnum3", v.ExEnum3
                ,"ExId", v.ExId
                //,"UpdateExists",true
                ,"UpdateType",(int)MemberUpdateType.Update//MemberContext.ReadMemberUpdateType(m.Body.UpdateType,MemberUpdateType.Sync)
                ,"EnableNews", v.EnableNews
                ,"DataSource", DataSource
                ,"DataSourceType",(int)DataSourceType// tinyint=0-- CoSystem = 0,Register = 1,FileSync = 2,ApiSync = 3
                ,"MemberType", GenericTypes.ReadEnum<MemberTypes>(v.MemberType, MemberTypes.Private) 
                ,"CompanyName", v.CompanyName
                ,"ExRef1", v.ExRef1
                ,"ExRef2", v.ExRef2
                ,"ExRef3", v.ExRef3

            };

            var parameters = DataParameter.GetSqlList(args);
            parameters[0].Direction = System.Data.ParameterDirection.InputOutput;
            //using (var db = DbContext.Create<DbPro>())
            //{
            //    var res = db.ExecuteCommandOutput("sp_Member_Save_v1", parameters.ToArray(), System.Data.CommandType.StoredProcedure);
            //    v.RecordId = res.GetValue<int>("RecordId");
            //}

            using (var db = DbContext.Create<DbPro>())
            {
                int res = db.ExecuteCommandNonQuery("sp_Member_Save_v1", parameters.ToArray(), System.Data.CommandType.StoredProcedure);
                v.RecordId = Types.ToInt(parameters[0].Value);
                return res;
            }
        }

        public static int DoSave(string MemberId, int AccountId, MemberItem entity, UpdateCommandType commandType)
        {
            if (commandType == UpdateCommandType.Delete)
            {
                throw new ArgumentException("Delete not supported");
            }
                //using (MemberContext context = new MemberContext(MemberId, AccountId))
                //{
                //    return context.SaveChanges(commandType);
                //}

            EntityValidator.Validate(entity, "חבר", "he");

            if (commandType == UpdateCommandType.Insert)
                using (MemberContext context = new MemberContext())
                {
                    context.Set(entity);
                    return context.SaveChanges(commandType);
                }

            if (commandType == UpdateCommandType.Update)
                using (MemberContext context = new MemberContext(MemberId, AccountId))
                {
                    context.Set(entity);
                    return context.SaveChanges(commandType);
                }
            return 0;
        }

        //public static int DoDelete(string MemberId, int AccountId)
        //{
        //    var parameters = DataParameter.GetSqlWithDirection(
        //        "AccountId", AccountId, 0,
        //        "MemberId", MemberId, 0,
        //        "Result", 0, 2
        //        );
        //    int res= db.ExecuteNonQuery("sp_Member_Remove", parameters, CommandType.StoredProcedure);
        //    return parameters.GetParameterValue<int>("Result");
        //}

        public static int DoDelete(int RecordId, int AccountId)
        {
            var parameters = DataParameter.GetSqlWithDirection(
                "AccountId", AccountId, 0,
                "RecordId", RecordId, 0,
                "Result", 0, 2
                );
            using (var db = DbContext.Create<DbPro>())
            {
                int res = db.ExecuteCommandNonQuery("sp_Member_Remove_v1", parameters, CommandType.StoredProcedure);
                return parameters.GetParameterValue<int>("Result");
            }
        }

        public static int DeleteMembersByCategory( int AccountId, int Category)
        {
            //var parameters = DataParameter.GetSqlList("AccountId", AccountId, "Category", Category);
            //DataParameter.AddOutputParameter(parameters, "Result", SqlDbType.Int, 4);
            //using (var db = DbContext.Create<DbPro>())
            //{
            //    db.ExecuteCommandNonQuery("sp_Members_Delete", parameters.ToArray(), CommandType.StoredProcedure);
            //    var result = Types.ToInt(parameters[2].Value);
            //    return result;
            //}

            using (var db = DbContext.Create<DbPro>())
            {
                var result = db.ExecuteReturnValue("sp_Members_Delete", 0, "AccountId", AccountId, "Category", Category);//, "Result", 0);
                return result;

            }
        }
        #endregion

        #region static

        public static MemberItem Get(int RecordId)
        {
            using (MemberContext context = new MemberContext(RecordId))
            {
                return context.Entity;
            }
        }

        public static MemberItem Get(string MemberId,int AccountId)
        {
            using (MemberContext context = new MemberContext(MemberId,AccountId))
            {
                return context.Entity;
            }
        }

        public static MemberInfo GetInfo(int RecordId, int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<MemberInfo>("vw_Members ", "RecordId", RecordId, "AccountId", AccountId);
        }

        //MemberCategoryView
        public static MemberItem ViewOrNewMemberItem(int RecordId, int AccountId)
        {
            if (RecordId > 0)
            {
                using (var db = DbContext.Create<DbPro>())
                    return db.ExecuteSingle<MemberItem>("sp_Member_View ", "RId", RecordId, "AccountId", AccountId);
            }
            return new MemberItem() { AccountId = AccountId };
        }

        public static string ViewMember(int RId, int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.ExecuteJsonRecord("sp_Member_View ", "RId", RId, "AccountId", AccountId);
        }

        //public static List<MemberItem> GetItems()
        //{
        //    using (MemberContext context = new MemberContext())
        //    {
        //        return context.EntityList();
        //    }
        //}

        public static IEnumerable<MemberItem> View()
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<MemberItem>(MappingName, null);
        }
        //public static IEnumerable<MemberItem> ViewByType(int MemberType)
        //{
        //    return db.EntityGetList<MemberItem>(MappingName, "MemberType=@MemberType", MemberType);
        //}
        //public static MemberItem Get(int MemberId)
        //{
        //    if (MemberId == 0)
        //        return new MemberItem();
        //    return db.EntityItemGet<MemberItem>(MappingName, "MemberId", MemberId);
        //}
        public const string MappingName = "Members";

        

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

        public static MemberUpdateType ReadMemberUpdateType(int value, MemberUpdateType defaultValue)
        {
            if (Enum.IsDefined(typeof(MemberUpdateType), value))
                return (MemberUpdateType)value;
            return defaultValue;
            //Nistec.GenericTypes.ConvertEnum<MemberUpdateType>()
        }

        #endregion

        #region list

        public static IEnumerable<MemberListView> ListView(int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<MemberListView>("vw_Members", "AccountId", AccountId);
        }

        //public static IEnumerable<MemberListView> View(int QueryType, int PageSize, int PageNum, int AccountId)
        //{
        //    return db.ExecuteQuery<MemberListView>("sp_Members_Query_Paging", "QueryType", QueryType, "@PageSize", PageSize, "PageNum", PageNum, "AccountId", AccountId);
        //}

        public static IEnumerable<MemberListView> ListQueryView(int QueryType, int PageSize, int PageNum, int AccountId,
           string MemberId = null,
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
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteList<MemberListView>("sp_Members_Query_v1", "QueryType", QueryType, "@PageSize", PageSize, "PageNum", PageNum, "AccountId", AccountId,
                    "MemberId", MemberId,
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

        public static IEnumerable<MemberListView> ListQueryView(MemberQuery q)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteList<MemberListView>("sp_Members_Query_v2", "QueryType", q.QueryType, "PageSize", q.PageSize, "PageNum", q.PageNum, "AccountId", q.AccountId,
                    "MemberId", q.MemberId,
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
                    "Filter", q.Filter,
                    "Items", q.Items,
                    "ReferralCode",q.ReferralCode,
                    "ValidityRemain", q.ValidityRemain,
                    "Campaign", q.Campaign,
                    "SignupDateFrom", q.SignupDateFrom,
                    "SignupDateTo", q.SignupDateTo,
                    "PriceFrom", q.PriceFrom,
                    "PriceTo", q.PriceTo,
                    "HasSignup", q.HasSignup,
                    "HasPayment", q.HasPayment,
                    "HasAutoCharge", q.HasAutoCharge
                    );
            }
        }

        public static DataTable ListQueryDataView(MemberQuery q)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteCommand<DataTable>("sp_Members_Query_v2", DataParameter.GetSql("QueryType", q.QueryType, "PageSize", q.PageSize, "PageNum", q.PageNum, "AccountId", q.AccountId,
                    "MemberId", q.MemberId,
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
                    "Sort", null,
                    "Filter", null,
                    
                    "Items", q.Items,
                    "ReferralCode",q.ReferralCode,
                    "ValidityRemain", q.ValidityRemain,
                    "Campaign", q.Campaign,
                    "SignupDateFrom", q.SignupDateFrom,
                    "SignupDateTo", q.SignupDateTo,
                    "PriceFrom", q.PriceFrom,
                    "PriceTo", q.PriceTo,
                    "HasSignup", q.HasSignup,
                    "HasPayment", q.HasPayment,
                    "HasAutoCharge", q.HasAutoCharge
                    ), CommandType.StoredProcedure);
            }
        }

        public static int ListQueryTargetsView(MemberQuery q)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteScalar<int>("sp_Members_Query_Targets_v2", 0, "QueryType", q.QueryType, "AccountId", q.AccountId, "UserId", q.UserId,
                    "MemberId", q.MemberId,
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

                    //signup query
                    "Items", q.Items,
                    "ReferralCode",q.ReferralCode,
                    "ValidityRemain", q.ValidityRemain,
                    "Campaign", q.Campaign,
                    "SignupDateFrom", q.SignupDateFrom,
                    "SignupDateTo", q.SignupDateTo,
                    "PriceFrom", q.PriceFrom,
                    "PriceTo", q.PriceTo,
                    "HasSignup", q.HasSignup,
                    "HasPayment", q.HasPayment,
                    "HasAutoCharge", q.HasAutoCharge

                    );
                //"ContactRule", q.ContactRule);
            }
        }

#if(false)
        public static IEnumerable<MemberListView> ListQueryView(MemberQuery q)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteList<MemberListView>("sp_Members_Query_v1", "QueryType", q.QueryType, "PageSize", q.PageSize, "PageNum", q.PageNum, "AccountId", q.AccountId,
                    "MemberId", q.MemberId,
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

        public static DataTable ListQueryDataView(MemberQuery q)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteCommand<DataTable>("sp_Members_Query_v1", DataParameter.GetSql("QueryType", q.QueryType, "PageSize", q.PageSize, "PageNum", q.PageNum, "AccountId", q.AccountId,
                    "MemberId", q.MemberId,
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

        public static int ListQueryTargetsView(MemberQuery q)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteScalar<int>("sp_Members_Query_Targets", 0, "QueryType", q.QueryType, "AccountId", q.AccountId, "UserId", q.UserId,
                    "MemberId", q.MemberId,
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

    public class MemberContext<T> : EntityContext<DbPro, T> where T : IEntityItem
    {
        public static void Refresh(int AccountId)
        {
            DbContextCache.Remove<T>(Settings.ProjectName, EntityCacheGroups.Members, AccountId, 0);
        }
        public MemberContext(int accountId)
        {
            if (accountId > 0)
                CacheKey = DbContextCache.GetKey<T>(Settings.ProjectName, EntityCacheGroups.Members, accountId, 0);
        }
        public IList<T> GetList()
        {
            return DbContextCache.EntityList<DbPro, T>(CacheKey, null);
        }
        public IList<T> GetList(int accountId)
        {
            return DbContextCache.EntityList<DbPro, T>(CacheKey, new object[] { "AccountId", accountId });
        }
        protected override void OnChanged(UpdateCommandType commandType)
        {
            DbContextCache.Remove(CacheKey);
        }
        public FormResult GetFormResult(EntityCommandResult res, string reason)
        {
            return FormResult.Get(res, this.EntityName, reason);//  res.AffectedRecords, this.EntityName, reason, res.GetIdentityValue<int>());
        }

    }

    public class MemberListView : IEntityItem
    {

        //public const string MappingName = "vw_Members";


        [EntityProperty(EntityPropertyType.Key)]
        public string Identifier { get; set; }
        public string ExId { get; set; }
        public string MemberId { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }
        public string Address { get; set; }
        public DateTime JoiningDate { get; set; }
        public string CellPhone { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Birthday { get; set; }
        public string Note { get; set; }
        [EntityProperty(EntityPropertyType.Identity)]
        public int RecordId { get; set; }

        #region view
        
        public string MemberName { get; set; }
        public string CityName { get; set; }
        public string BranchName { get; set; }
 
        [EntityProperty(EntityPropertyType.View)]
        public DateTime LastUpdate { get; set; }
        public string RegionName { get; set; }
        public string GenderName { get; set; }
        public string ExEnumName1 { get; set; }
        public string ExEnumName2 { get; set; }
        public string ExEnumName3 { get; set; }
        
        public int TotalRows { get; set; }

        #endregion
    }

    //public class MemberCategoryView : MemberItem
    //{
    //    public string Categories { get; set; }

    //    //public SqlParameter[] ToProc()
    //    //{
    //    //    var args = new object[]{
    //    //        "RecordId", RecordId
    //    //        ,"MemberId", MemberId
    //    //        ,"AccountId", AccountId
    //    //        ,"LastName", LastName
    //    //        ,"FirstName", FirstName
    //    //        //,"FatherName", v.FatherName
    //    //        ,"Address", Address
    //    //        ,"City", City
                
    //    //        //,"BirthDateYear", v.BirthDateYear
    //    //        //,"ChargeType", v.ChargeType
    //    //        ,"CellPhone",CellPhone
    //    //        ,"Phone", Phone
    //    //        ,"Email", Email
    //    //        //,"Status", v.Status
    //    //        //,"Region", v.Region
    //    //        ,"Gender", Gender
    //    //        ,"Birthday", Birthday
    //    //        ,"Note", Note
    //    //        //,"Fax", v.Fax
    //    //        //,"WorkPhone", v.WorkPhone
    //    //        ,"JoiningDate", JoiningDate
    //    //        ,"Branch", Branch
    //    //        ,"ZipCode", ZipCode
    //    //        ,"ContactRule", 0
    //    //        ,"Categories", Categories
    //    //        ,"ExField1", ExField1
    //    //        ,"ExField2", ExField2
    //    //        ,"ExField3", ExField3
    //    //        ,"ExEnum1", ExEnum1
    //    //        ,"ExEnum2", ExEnum2
    //    //        ,"ExEnum3", ExEnum3
    //    //        ,"ExId", ExId
    //    //    };
    //    //    var parameters = DataParameter.GetSql(args);
    //    //    parameters[0].Direction = System.Data.ParameterDirection.InputOutput;
    //    //    return parameters;
    //    //}
    //}


    [EntityMapping("vw_Members")]
    public class MemberInfo : MemberItem
    {
        public string CityName { get; set; }
        public string BranchName { get; set; }
        public string GenderName { get; set; }
        public string ExEnumName1 { get; set; }
        public string ExEnumName2 { get; set; }
        public string ExEnumName3 { get; set; }
    }

     [EntityMapping("vw_Member_Display")]
    public class MemberDisplay : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Identity)]
        public int RecordId { get; set; }
        public int AccountId { get; set; }
        public string DisplayName { get; set; }
    }

     [EntityMapping("Members")]
    public class MemberItem : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Key)]
        public string Identifier { get; set; }
     
        //[Validator(RequiredVar="@ExType=0", Name="תעודת זהות")]
        public string MemberId { get; set; }
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
        public int RecordId	{ get; set; }

        public string ExField1 { get; set; }
        public string ExField2 { get; set; }
        public string ExField3 { get; set; }

        public int ExEnum1 { get; set; }
        public int ExEnum2 { get; set; }
        public int ExEnum3 { get; set; }
        public bool? EnableNews { get; set; }

        public string DataSource { get; set; }
        public int DataSourceType { get; set; }
        public int MemberType { get; set; }
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
                if (MemberType == 1)
                    return CompanyName;
                return FirstName + " " + LastName;
            }
        }

        public string MemberName { get { return FirstName + " " + LastName; } }
        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<MemberItem>(this,null, null, true);
        }
        public Dictionary<string,object> ToDictionary()
        {
            return EntityDataExtension.EntityToDictionary(this,true,false); //..<MemberItem>(this, null, null, true);
        }
    
    }

     public enum MemberUpdateType
     {
         InsertOnly = 0,
         Sync = 1,
         Update = 2
     }
     public enum MemberTypes
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
     public enum MemberKeyType
     {
         None = -1, MemberId = 0, Cell = 1, Mail = 2, Target = 3, Key = 4
     }
}
