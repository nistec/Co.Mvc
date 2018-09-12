using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using Nistec;
using System.Data;
using Nistec.Generic;
using System.Web;
using ProSystem.Query;
using Nistec.Serialization;
using System.Collections.Specialized;
using ProSystem.Data;

namespace ProAd.Data.Entities
{

    [Entity(EntityName = "PaymentView", MappingName = "Payment", ConnectionKey = "netcell_system", EntityKey = new string[] { "PayId" })]
    public class AccountPaymentContext : EntityContext<PaymentItem>
    {

        #region ctor

        public AccountPaymentContext()
        {
        }

        public AccountPaymentContext(int PayId)
            : base(PayId)
        {
        }

        #endregion

        #region list query


        public static IEnumerable<PaymentsView> ListQueryView(int QueryType, int PageSize, int PageNum, int AccountId,
           bool IsFailure=false,
           DateTime? SignupDateFrom = null,
           DateTime? SignupDateTo = null,
           decimal PriceFrom = -1,
           decimal PriceTo = -1,
           string Sort=null,
           string Filter=null)
        {
            using (var db = DbContext.Create<DbSystem>())
            {
                return db.ExecuteList<PaymentsView>("sp_Accounts_Payment_Query", 
                    "QueryType", QueryType, 
                    "PageSize", PageSize, 
                    "PageNum", PageNum, 
                    "AccountId", AccountId,
                    "IsFailure", IsFailure,
                    "SignupDateFrom", SignupDateFrom,
                    "SignupDateTo", SignupDateTo,
                    "PriceFrom", PriceFrom,
                    "PriceTo", PriceTo,
                    "Sort", Sort,
                    "Filter", Filter);
            }
        }

       
        public static IEnumerable<PaymentsView> ListQueryView(PaymentQuery q)
        {
            using (var db = DbContext.Create<DbSystem>())
            {
                return db.ExecuteList<PaymentsView>("sp_Accounts_Payment_Query", "QueryType", q.QueryType, "PageSize", q.PageSize, "PageNum", q.PageNum, "AccountId", q.AccountId,
                    "IsFailure", q.IsFailure,
                    "SignupDateFrom", q.SignupDateFrom,
                    "SignupDateTo", q.SignupDateTo,
                    "PriceFrom", q.PriceFrom,
                    "PriceTo", q.PriceTo,
                    "Sort", q.Sort,
                    "Filter", q.Filter);
            }
        }
        
       public static DataTable ListQueryDataView(PaymentQuery q)
       {
           using (var db = DbContext.Create<DbSystem>())
           {
               return db.ExecuteCommand<DataTable>("sp_Payment_Query", DataParameter.GetSql("QueryType", q.QueryType, "PageSize", q.PageSize, "PageNum", q.PageNum, "AccountId", q.AccountId,
                   "IsFailure", q.IsFailure,
                   "SignupDateFrom", q.SignupDateFrom,
                   "SignupDateTo", q.SignupDateTo,
                   "PriceFrom", q.PriceFrom,
                   "PriceTo", q.PriceTo,
                   "Sort", q.Sort,
                   "Filter", q.Filter), CommandType.StoredProcedure);
           }
       }
       

        //public static string UnpayedView(
        //    int AccountId,
        //    int CampaignId)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //    {
        //        string res = db.ExecuteJson("sp_UnPayed_Members", "Mode", "get", "AccountId", AccountId, "CampaignId", CampaignId);
        //        return res;
        //    }
        //}

        //public static int UnpayedRemove(
        //    int AccountId,
        //    int CampaignId)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //    {
        //        int res = db.ExecuteNonQuery("sp_UnPayed_Members", "Mode", "del", "AccountId", AccountId, "CampaignId", CampaignId);
        //        return res;
        //    }
        //}

        #endregion

    }

    public class PaymentsView : PaymentItem
    {
        public int TotalRows { get; set; }
    }

    public class PaymentItem : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Identity)]
        public int PayId { get; set; }
        public decimal Payed { get; set; }
        public int Qty { get; set; }
        public DateTime Creation { get; set; }
        public string TransIndex { get; set; }
        public string ConfirmationCode { get; set; }
        public string Ccno { get; set; }
        public string Response { get; set; }

        public string Contact { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Token { get; set; }
        public string ResponseText { get; set; }
        public string Terminal { get; set; }
        public string TransMode { get; set; }
        public string ExpDate { get; set; }
        public int ChargeMode { get; set; }

        public string ID { get; set; }
        public int AccountId { get; set; }
        public int ChargeId { get; set; }
        public int State { get; set; }
        public bool IsFailure { get; set; }

    }


    [Serializable]
    public class PaymentQuery : QueryBase
    {
        public string HTitle { get; set; }
        public string Serialize()
        {
            var ser = BinarySerializer.SerializeToBase64(this);
            return ser;
        }

        public static PaymentQuery Deserialize(string ser)
        {
            return BinarySerializer.DeserializeFromBase64<PaymentQuery>(ser);
        }

        public int QueryType { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set; }

        public PaymentQuery()
        {
            AccountId = 0;
            SignupDateFrom = null;
            SignupDateTo = null;
            PriceFrom = 0;
            PriceTo = 0;
            IsFailure = false;
        }

        public PaymentQuery(NameValueCollection Request)
        {
            //string query_type = Request["query_type"];

            QueryType = Types.ToInt(Request["QueryType"]);
        

            if (QueryType == 0)
                HTitle = "פירוט תשלומים שהתקבלו";
            else if (QueryType == 1)
                HTitle = "פירוט תשלומים שנכשלו";
            //else if (QueryType == 2)
            //    HTitle = "פירוט תשלומים כפולים";

            AccountId = Types.ToInt(Request["AccountId"]);
            SignupDateFrom = Types.ToNullableDate(Request["SignupDateFrom"]);
            SignupDateTo = Types.ToNullableDate(Request["SignupDateTo"]);
            PriceFrom = Types.ToDecimal(Request["PriceFrom"], 0);
            PriceTo = Types.ToDecimal(Request["PriceTo"], 0);
            IsFailure = Types.ToBool(Request["IsFailure"], false);

        }

        public PaymentQuery(HttpRequestBase Request)
        {
            QueryType = Types.ToInt(Request["QueryType"]);

            AccountId = Types.ToInt(Request["AccountId"]);
            SignupDateFrom = Types.ToNullableDate(Request["SignupDateFrom"]);
            SignupDateTo = Types.ToNullableDate(Request["SignupDateTo"]);
            PriceFrom = Types.ToDecimal(Request["PriceFrom"], 0);
            PriceTo = Types.ToDecimal(Request["PriceTo"], 0);
            IsFailure = Types.ToBool(Request["IsFailure"],false);

            LoadSortAndFilter(Request);
        }

        public void Normelize()
        {
           
        }

        public DateTime? SignupDateFrom { get; set; }
        public DateTime? SignupDateTo { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        public bool IsFailure { get; set; }

    }

}
