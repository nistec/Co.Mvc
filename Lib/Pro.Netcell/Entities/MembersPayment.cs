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
using Nistec.Generic;
using System.Web;
using Pro.Netcell.Query;

namespace ProNetcell.Data.Entities
{

    [Entity(EntityName = "PaymentView", MappingName = "Payment", ConnectionKey = "cnn_pro", EntityKey = new string[] { "PayId" })]
    public class PaymentContext : EntityContext<PaymentView>
    {

        #region ctor

        public PaymentContext()
        {
        }

        public PaymentContext(int PayId)
            : base(PayId)
        {
        }

        #endregion


        #region static

   
        public static decimal LookupItemPrice(int ItemId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.QueryScalar<decimal>("select Price from PriceList where ItemId=@ItemId", 0, "ItemId", ItemId);
        }

        #endregion

        #region list query


        public static IEnumerable<PaymentReportView> ListQueryView(int QueryType, int PageSize, int PageNum, int AccountId,
           string MemberId = null,
           string Name = null,
           string Items = null,
            //int ValidityRemain = -1,
           int Campaign = -1,
           DateTime? SignupDateFrom = null,
           DateTime? SignupDateTo = null,
           decimal PriceFrom = -1,
           decimal PriceTo = -1,
           int ContactRule = 0)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteList<PaymentReportView>("sp_Payment_Query", "QueryType", QueryType, "PageSize", PageSize, "PageNum", PageNum, "AccountId", AccountId,
                    "MemberId", MemberId,
                    "Name", Name,
                    "Items", Items,
                    //"ValidityRemain", ValidityRemain,
                    "Campaign", Campaign,
                    "SignupDateFrom", SignupDateFrom,
                    "SignupDateTo", SignupDateTo,
                    "PriceFrom", PriceFrom,
                    "PriceTo", PriceTo,
                    "ContactRule", ContactRule);
            }
        }


        public static IEnumerable<PaymentReportView> ListQueryView(PaymentQuery q)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteList<PaymentReportView>("sp_Payment_Query", "QueryType", q.QueryType, "PageSize", q.PageSize, "PageNum", q.PageNum, "AccountId", q.AccountId,
                    "MemberId", q.MemberId,
                    "Name", q.Name,
                    "Items", q.Items,
                    //"ValidityRemain", q.ValidityRemain,
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

        public static DataTable ListQueryDataView(PaymentQuery q)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                return db.ExecuteCommand<DataTable>("sp_Payment_Query", DataParameter.GetSql("QueryType", q.QueryType, "PageSize", q.PageSize, "PageNum", q.PageNum, "AccountId", q.AccountId,
                    "MemberId", q.MemberId,
                    "Name", q.Name,
                    "Items", q.Items,
                    //"ValidityRemain", q.ValidityRemain,
                    "Campaign", q.Campaign,
                    "SignupDateFrom", q.SignupDateFrom,
                    "SignupDateTo", q.SignupDateTo,
                    "PriceFrom", q.PriceFrom,
                    "PriceTo", q.PriceTo,
                    "ContactRule", q.ContactRule), CommandType.StoredProcedure);
            }
        }

        //public static int ListQueryTargetsView(PaymentQuery q)
        //{
        //    return db.ExecuteScalar<int>("sp_Signup_Query_Targets", 0, "QueryType", q.QueryType, "AccountId", q.AccountId, "UserId", q.UserId,
        //    "MemberId", q.MemberId,
        //    "Name", q.Name,
        //    "Items", q.Items,
        //    //"ValidityRemain", q.ValidityRemain,
        //    "Campaign", q.Campaign,
        //    "SignupDateFrom", q.SignupDateFrom,
        //    "SignupDateTo", q.SignupDateTo,
        //    "PriceFrom", q.PriceFrom,
        //    "PriceTo", q.PriceTo);
        //    //"ContactRule", q.ContactRule);
        //}

        public static string UnpayedView(
            int AccountId,
            int CampaignId)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                string res = db.ExecuteJson("sp_UnPayed_Members", "Mode", "get", "AccountId", AccountId, "CampaignId", CampaignId);
                return res;
            }
        }

        public static int UnpayedRemove(
            int AccountId,
            int CampaignId)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                int res = db.ExecuteNonQuery("sp_UnPayed_Members", "Mode", "del", "AccountId", AccountId, "CampaignId", CampaignId);
                return res;
            }
        }

        #endregion

    }

    public class PaymentReportView : IEntityItem
    {
        public int TotalRows { get; set; }

        public int PayId { get; set; }
        public decimal Payed { get; set; }
        public string ConfirmationCode { get; set; }
        public string Response { get; set; }
        public DateTime PayedDate { get; set; }
        public int Qty { get; set; }
        public int SignupId { get; set; }
        public int SignupOrder { get; set; }
        public int TransIndex { get; set; }
        public DateTime SignupDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int ValidityMonth { get; set; }
        public string CreditCardOwner { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Campaign { get; set; }
        public string CampaignName { get; set; }
        public string MemberId { get; set; }

        public string MemberName { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
    }

/*
    public class PaymentAccountView : IEntityItem
    {
        public int PayId { get; set; }
        public decimal Payed { get; set; }
        public DateTime Creation { get; set; }
        public string TransIndex { get; set; }
        public string ConfirmationCode { get; set; }
        
        public int AccountId { get; set; }
        public string MemberId { get; set; }
        public string MemberName { get; set; }
        public string CellPhone { get; set; }
        public DateTime SignupDate { get; set; }
        public int ValidityMonth { get; set; }
        public bool CreditCardOwner { get; set; }
        public DateTime ExpirationDate { get; set; }
        //public int TotalRows { get; set; }

    }
*/
    public class PaymentView : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Identity)]
        public int PayId { get; set; }
        public decimal Payed { get; set; }
        public int Qty { get; set; }
        public DateTime Creation { get; set; }
        public string TransIndex { get; set; }
        public string ConfirmationCode { get; set; }
        public DateTime SignupDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int ValidityMonth { get; set; }
        public int MemberRecord { get; set; }
        public string CreditCardOwner { get; set; }
        public string ItemName { get; set; }
        public string CampaignName { get; set; }

    }
//    public class PaymentItem : IEntityItem
//    {

//        [EntityProperty(EntityPropertyType.Identity)]
//        public int PayId { get; set; }

//        [EntityProperty(EntityPropertyType.View)]
//        public DateTime Creation { get; set; }
//        public string Ccno { get; set; }
//        public string SignKey { get; set; }
//        public string ResponseText { get; set; }// full text
        
//        //========================================

//        public string Response { get; set; }//Response
//        public int SignId { get; set; }//trid
//        public int ExpireMonth { get; set; }//expmonth
//        public string Contact { get; set; }//contact
//        public string ID { get; set; }//myid
//        public string Email { get; set; }//email
//        public int ExpireYear { get; set; }//expyear
//        public string Terminal { get; set; }//supplier
//        public decimal Payed { get; set; }//sum
//        public string benid { get; set; }//benid
//        public string Phone { get; set; }//phone
//        public string TransMode { get; set; }//tranmode
//        public string ConfirmationCode { get; set; }//ConfirmationCode
//        public string Token { get; set; }//TranzilaTK
//        public string TransIndex { get; set; }//index
                


//        /*
//        Response=000
//        &o_tranmode=AK
//        &trid=50
//        &trBgColor=
//        &expmonth=10
//        &contact=nissim
//        &myid=054649967
//        &email=nissim%40myt.com
//        &currency=1
//        &nologo=1
//        &expyear=17
//        &supplier=baityehudi
//        &sum=1.00
//        &benid=5pb423r0odqe2kcvo40ku1bvm7
//        &o_cred_type=
//        &lang=il
//        &phone=0527464292
//        &o_npay=
//        &tranmode=AK
//        &ConfirmationCode=0000000
//        &cardtype=2
//        &cardissuer=6
//        &cardaquirer=6
//        &index=5
//        &Tempref=02720001
//        &TranzilaTK=W2e44ed3a9737dc2322
//        &ccno=
//        */
//        public string ToHtml()
//        {
//            return EntityProperties.ToHtmlTable<PaymentItem>(this, null, null, true);
//        }

//    }
///*
///
/*
    public class MemberPayment : PaymentItem
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
            return EntityProperties.ToHtmlTable<MemberPayment>(this, null, null, true);
        }


    }

*/
}
