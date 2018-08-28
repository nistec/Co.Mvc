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
using Pro.Netcell.Query;

namespace ProNetcell.Data.Entities
{

    [Entity(EntityName = "MemberSignup", MappingName = "Members", ConnectionKey = "cnn_pro", EntityKey = new string[] { "MemberId,AccountId" })]
    public class SignupContext : EntityContext<MemberSignup>
    {
       

        #region ctor

        public SignupContext()
        {
        }

        public SignupContext(string MemberId,int AccountId)
            : base(MemberId, AccountId)
        {
        }

        public SignupContext(int RecordId)
            : base()
        {
            SetByParam("RecordId", RecordId);
        }

        #endregion

        #region update

        public static string CreateSignKey(int accountId)
        {
            return accountId.ToString() + "_" + Nistec.Generic.UUID.NewUuid().ToString().Replace("-","");
        }

        //public static int DoSave(MemberSignup v)
        //{

        //    var args = new object[]{
        //        "SignupId", 0
        //        ,"SignupStatus", 0//v.SignupStatus
        //        ,"AccountId", v.AccountId
        //        ,"MemberId", v.MemberId
        //        ,"LastName", v.LastName
        //        ,"FirstName", v.FirstName
        //        ,"Address", v.Address
        //        ,"City", v.City
        //        ,"CellPhone",v.CellPhone
        //        ,"Phone", v.Phone
        //        ,"Email", v.Email
        //        ,"Gender", v.Gender
        //        ,"Birthday", v.Birthday
        //        ,"Note", v.Note
        //        //,"JoiningDate", v.JoiningDate
        //        ,"Branch", v.Branch
        //        ,"ZipCode", v.ZipCode
        //        ,"ContactRule", 0
        //        ,"Categories", v.Categories
        //        ,"ExField1", v.ExField1
        //        ,"ExField2", v.ExField2
        //        ,"ExField3", v.ExField3
        //        ,"ExEnum1", v.ExEnum1
        //        ,"ExEnum2", v.ExEnum2
        //        ,"ExEnum3", v.ExEnum3
        //        ,"ExId", v.ExId
        //        ,"ReferralCode", v.ReferralCode
        //        ,"AutoCharge", v.AutoCharge
        //        ,"RegHostAddress", v.RegHostAddress
        //        ,"RegReferrer", v.RegReferrer
        //        ,"CreditCardOwner", v.CreditCardOwner
        //        ,"ConfirmAgreement", v.ConfirmAgreement
        //        ,"Campaign", 0//v.Campaign
        //        ,"SignKey", v.SignKey//CreateSignKey(v.AccountId) //v.SignKey
        //        ,"SignupOrder", v.SignupOrder
        //        ,"Price", v.Price
        //        ,"ItemId", v.ItemId
        //    };
        //    var parameters = DataParameter.GetSql(args);
        //    parameters[0].Direction = System.Data.ParameterDirection.Output;
        //    parameters[1].Direction = System.Data.ParameterDirection.Output;
        //    using (var db = DbContext.Create<DbPro>())
        //    {
        //        int res = db.ExecuteCommandNonQuery("sp_Member_Signup", parameters, System.Data.CommandType.StoredProcedure);
        //        v.SignupId = Types.ToInt(parameters[0].Value);
        //        int SignupStatus = Types.ToInt(parameters[1].Value);
        //        return SignupStatus;
        //    }
        //}

        public static int DoSave (MemberSignup v)
        {

            var args = new object[]{
                "SignupId", 0
                ,"SignupStatus", 0//v.SignupStatus
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
                ,"MemberType", v.MemberType
                ,"CompanyName", v.CompanyName
                ,"ReferralCode", v.ReferralCode
                ,"AutoCharge", v.AutoCharge
                ,"RegHostAddress", v.RegHostAddress
                ,"RegReferrer", v.RegReferrer
                ,"CreditCardOwner", v.CreditCardOwner
                ,"ConfirmAgreement", v.ConfirmAgreement
                ,"Campaign", 0//v.Campaign
                ,"SignKey", v.SignKey//CreateSignKey(v.AccountId) //v.SignKey
                ,"SignupOrder", v.SignupOrder
                ,"Price", v.Price
                ,"ItemId", v.ItemId
            };
            var parameters = DataParameter.GetSql(args);
            parameters[0].Direction = System.Data.ParameterDirection.Output;
            parameters[1].Direction = System.Data.ParameterDirection.Output;
            using (var db = DbContext.Create<DbPro>())
            {
                int res = db.ExecuteCommandNonQuery("sp_Member_Signup_v1", parameters, System.Data.CommandType.StoredProcedure);
                v.SignupId = Types.ToInt(parameters[0].Value);
                int SignupStatus = Types.ToInt(parameters[1].Value);
                return SignupStatus;
            }
        }

        #endregion

        #region static


        public static IEnumerable<SignupPaymentView> MemberSignupHistory(int MemberRecord)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<SignupPaymentView>("vw_Signup_Validity", "MemberRecord", MemberRecord);//("vw_Signup_Payment", "MemberRecord", MemberRecord);
        }

        //public static IEnumerable<SignupView> MemberSignupHistory(int MemberRecord)
        //{
        //    using (var db = DbContext.Create<DbPro>())
        //        return db.EntityItemList<SignupView>("vw_Signup_View", "MemberRecord", MemberRecord);
        //}

        public static IEnumerable<SignupAccountView> SignupViewByAccount(int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<SignupAccountView>("vw_Account_Signup_View", "AccountId", AccountId);
        }

        public static IEnumerable<SignupAccountView> SignupViewByMemnber(string MemberId, int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<SignupAccountView>("vw_Signup_View", "MemberId", MemberId, "AccountId", AccountId);
        }

        public static int Member_Signup_Validation(
            int AccountId,
            string MemberId,
            string CellPhone,
            string Email,
            string ExId)
        {

            var args = new object[]{
                "SignupStatus", 0
                ,"AccountId", AccountId
                ,"MemberId", MemberId
                ,"CellPhone", CellPhone
                ,"Email", Email
                ,"ExId", ExId
            };
            var parameters = DataParameter.GetSql(args);
            parameters[0].Direction = System.Data.ParameterDirection.Output;
            using (var db = DbContext.Create<DbPro>())
            {
                int res = db.ExecuteCommandNonQuery("sp_Member_Signup_Validation", parameters, System.Data.CommandType.StoredProcedure);
                int SignupStatus = Types.ToInt(parameters[0].Value);
                return SignupStatus;
            }
        }
        #endregion

        #region list query

        public static IEnumerable<SignupAccountView> ListView(int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<SignupAccountView>("vw_Account_Signup_View", "AccountId", AccountId);
        }

        public static IEnumerable<SignupReportView> ListQueryView(int QueryType, int PageSize, int PageNum, int AccountId,
           string MemberId = null,
           string Name = null,
           string Items = null,
           int ValidityRemain = -1,
           int Campaign = -1,
           DateTime? SignupDateFrom = null,
           DateTime? SignupDateTo = null,
           decimal PriceFrom = -1,
           decimal PriceTo = -1,
           int ContactRule = 0)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteList<SignupReportView>("sp_Signup_Query", "QueryType", QueryType, "PageSize", PageSize, "PageNum", PageNum, "AccountId", AccountId,
                    "MemberId", MemberId,
                    "Name", Name,
                    "Items", Items,
                    "ValidityRemain", ValidityRemain,
                    "Campaign", Campaign,
                    "SignupDateFrom", SignupDateFrom,
                    "SignupDateTo", SignupDateTo,
                    "PriceFrom", PriceFrom,
                    "PriceTo", PriceTo,
                    "ContactRule", ContactRule);
            }
        }


        public static IEnumerable<SignupReportView> ListQueryView(SignupQuery q)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteList<SignupReportView>("sp_Signup_Query", "QueryType", q.QueryType, "PageSize", q.PageSize, "PageNum", q.PageNum, "AccountId", q.AccountId,
                    "MemberId", q.MemberId,
                    "Name", q.Name,
                    "Items", q.Items,
                    "ValidityRemain", q.ValidityRemain,
                    "Campaign", q.Campaign,
                    "SignupDateFrom", q.SignupDateFrom,
                    "SignupDateTo", q.SignupDateTo,
                    "PriceFrom", q.PriceFrom,
                    "PriceTo", q.PriceTo,
                    "ContactRule", q.ContactRule,
                    "Sort", q.Sort,
                    "Filter", q.Filter);
            }
        }

        public static DataTable ListQueryDataView(SignupQuery q)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteCommand<DataTable>("sp_Signup_Query", DataParameter.GetSql("QueryType", q.QueryType, "PageSize", q.PageSize, "PageNum", q.PageNum, "AccountId", q.AccountId,
                    "MemberId", q.MemberId,
                    "Name", q.Name,
                    "Items", q.Items,
                    "ValidityRemain", q.ValidityRemain,
                    "Campaign", q.Campaign,
                    "SignupDateFrom", q.SignupDateFrom,
                    "SignupDateTo", q.SignupDateTo,
                    "PriceFrom", q.PriceFrom,
                    "PriceTo", q.PriceTo,
                    "ContactRule", q.ContactRule), CommandType.StoredProcedure);
            }
        }

        public static int ListQueryTargetsView(SignupQuery q)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteScalar<int>("sp_Signup_Query_Targets", 0, "QueryType", q.QueryType, "AccountId", q.AccountId, "UserId", q.UserId,
                "MemberId", q.MemberId,
                "Name", q.Name,
                "Items", q.Items,
                "ValidityRemain", q.ValidityRemain,
                "Campaign", q.Campaign,
                "SignupDateFrom", q.SignupDateFrom,
                "SignupDateTo", q.SignupDateTo,
                "PriceFrom", q.PriceFrom,
                "PriceTo", q.PriceTo);
                //"ContactRule", q.ContactRule);
            }
        }

        #endregion

        public static DataSet Registry_Info(int AccountId, string Id)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.ExecuteDataSet("sp_Registry_Info", "AccountId", AccountId, "Id", Id);
        }

        public static int GetMemberRecord(int AccountId, string Id)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.ExecuteScalar<int>("sp_Member_Record_Get",0, "AccountId", AccountId, "Id", Id);
        }
        public static int GetMemberRecord(string Folder, string Id)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.ExecuteScalar<int>("sp_Member_Record_Get", 0, "Folder", Folder, "Id", Id);
        }
    }

    public class SignupReportView : SignupAccountView
    {
        public int TotalRows { get; set; }

        public int? PayId { get; set; }
        public decimal? Payed { get; set; }
        public string ConfirmationCode { get; set; }
        public string Token { get; set; }
        public DateTime? PayedDate { get; set; }

    }

    

    public class SignupAccountView : SignupItem
    {
        public string MemberId { get; set; }
        public string MemberName { get; set; }
        public string CellPhone { get; set; }
        public string CampaignName { get; set; }
        public string ItemName { get; set; }
    }

    public class SignupPaymentView : SignupItem
    {
        public string CampaignName { get; set; }
        public string ItemName { get; set; }
        public int PayId { get; set; }
        public decimal Payed { get; set; }
        public DateTime PayedDate { get; set; }
        public bool PaymentOwner { get; set; }
    }
    public class SignupView : SignupItem
    {
        public string CampaignName { get; set; }
        public string ItemName { get; set; }
    }

    public class SignupItem : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Identity)]
        public int SignupId { get; set; }
        [Validator(Required = true, MinValue = "2000-01-01", Name = "תאריך הצטרפות")]
        public DateTime SignupDate { get; set; }
        public string ReferralCode { get; set; }
        public bool AutoCharge { get; set; }
        public string RegHostAddress { get; set; }
        public string RegReferrer { get; set; }
        public string CreditCardOwner { get; set; }
        public bool ConfirmAgreement { get; set; }
        public string SignKey { get; set; }
        public decimal Price { get; set; }
        public int ItemId { get; set; }
        //public int PayId { get; set; }

        [EntityProperty(EntityPropertyType.View)]
        public int ValidityMonth { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public int MemberRecord { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public int Campaign { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public int SignupOrder { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime ExpirationDate { get; set; }
    }

    public class MemberSignup : MemberItem
    {
        public string Categories { get; set; }

        [EntityProperty(EntityPropertyType.Identity)]
        public int SignupId { get; set; }
        [Validator(Required = true, MinValue = "2000-01-01", Name = "תאריך הצטרפות")]
        public DateTime SignupDate { get; set; }
        public string ReferralCode { get; set; }
        public bool AutoCharge { get; set; }
        public string RegHostAddress { get; set; }
        public string RegReferrer { get; set; }
        public string CreditCardOwner { get; set; }
        public bool ConfirmAgreement { get; set; }
        public string SignKey { get; set; }
        public decimal Price { get; set; }
        public int ItemId { get; set; }
        //public int PayId { get; set; }

        [EntityProperty(EntityPropertyType.View)]
        public int ValidityMonth { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public int MemberRecord { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public int Campaign { get; set; }
        //[EntityProperty(EntityPropertyType.View)]
        public int SignupOrder { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime ExpirationDate { get; set; }
    }

/*
    public class SignupItem : IEntityItem
    {
      
        [EntityProperty(EntityPropertyType.Identity)]
        public int SignupId	{ get; set; }
        [Validator(Required = true, MinValue = "2000-01-01", Name = "תאריך הצטרפות")]
        public DateTime SignupDate { get; set; }
        public string ReferralCode { get; set; }
        public bool AutoCharge { get; set; }
        public string RegHostAddress { get; set; }
        public string RegReferrer { get; set; }
        public bool CreditCardOwner { get; set; }
        public bool ConfirmAgreement { get; set; }
        public string SignKey { get; set; }
        public decimal Price { get; set; }
        public int ItemId { get; set; }
        public int PayId { get; set; }

        [EntityProperty(EntityPropertyType.View)]
        public int ValidityMonth { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public int MemberRecord { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public int Campaign { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public int SignupOrder { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime ExpirationDate { get; set; }
        
        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<SignupItem>(this, null, null, true);
        }
   
    }

    public class MemberSignup : SignupItem
    {

        [EntityProperty(EntityPropertyType.Key)]
        [Validator(Required = true, Name = "תעודת זהות")]
        public string MemberId { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        [Validator(Required = true, Name = "חשבון")]
        public int AccountId { get; set; }
        [Validator(Required = true, Name = "שם פרטי")]
        public string LastName { get; set; }
        [Validator(Required = true)]
        public string FirstName { get; set; }
        //public string FatherName { get; set; }
        public string Address { get; set; }
        public int City { get; set; }
        
        //public int BirthDateYear { get; set; }
        //public int ChargeType { get; set; }
        //public int Branch { get; set; }
        public string CellPhone { get; set; }
        //public string Phone { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        //public int Region { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string Note { get; set; }

        //[EntityProperty(EntityPropertyType.NA)]
        //public string Categories { get; set; }

        //[Validator(Required = true, MinValue = "2000-01-01", Name = "תאריך הצטרפות")]
        //public DateTime JoiningDate { get; set; }
        //public string Fax { get; set; }
        //public string WorkPhone { get; set; }
        public string ZipCode { get; set; }

        public string HebrewBirthday { get; set; }
        

        //[EntityProperty(EntityPropertyType.View)]
        //public DateTime LastUpdate { get; set; }

        //[EntityProperty(EntityPropertyType.Identity)]
        //public int RecordId	{ get; set; }

        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<MemberSignup>(this, null, null, true);
        }


    }

*/
}
