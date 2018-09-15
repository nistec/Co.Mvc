using Nistec;
using Nistec.Data;
using Nistec.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Pro.Lib.Query
{
    [Serializable]
    public class PaymentQuery:QueryBase
    {
        public string HTitle { get; set; }
        public string Serialize()
        {
           var ser= BinarySerializer.SerializeToBase64(this);
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
            MemberId = null;
            CellPhone = null;
            Name = null;
            Items = null;
            //ValidityRemain = -1;
            Campaign = -1;
            SignupDateFrom = null;
            SignupDateTo = null;
            PriceFrom = 0;
            PriceTo = 0;
            ContactRule = 0;
        }

        public PaymentQuery(NameValueCollection Request)
        {
            //string query_type = Request["query_type"];

             QueryType = Types.ToInt(Request["QueryType"]);

            //string op = Request["op"];
            //if(op=="/Crm/PaymentsExport")
            //{
            //    if(QueryType==1)
            //        QueryType=21;
            //    else
            //        QueryType=20;
            //}
            //else //if(op=="/Crm/PaymentsReport")
            //{
            //     if(QueryType==1)
            //         HTitle = "פירוט תשלומים שנכשלו";
            //    else
            //         HTitle = "פירוט תשלומים שהתקבלו";
                
            //}   


             if (QueryType==0)
                 HTitle = "פירוט תשלומים שהתקבלו";
             else if (QueryType == 1)
                 HTitle = "פירוט תשלומים שנכשלו";
             else if (QueryType == 2)
                 HTitle = "פירוט תשלומים כפולים";

            MemberId = Types.NZorEmpty(Request["MemberId"], null);
            if (MemberId != null)
                return;

            CellPhone = Types.NZorEmpty(Request["CellPhone"], null);
            if (CellPhone != null)
                return;

            string listItems = Request["Items"];
            bool allItems = Request["allItems"] == "on";
            if (!allItems)
                Items = listItems;

            string val = "";
            //val = Request["ValidityRemain"];
            //ValidityRemain = (val == "") ? -1 : Types.ToInt(Request["ValidityRemain"], 0);
            val = Request["Campaign"];
            Campaign = (val == "") ? -1 : Types.ToInt(Request["Campaign"], 0);


            SignupDateFrom = Types.ToNullableDate(Request["SignupDateFrom"]);
            SignupDateTo = Types.ToNullableDate(Request["SignupDateTo"]);


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

            PriceFrom = Types.ToDecimal(Request["PriceFrom"], 0);
            PriceTo = Types.ToDecimal(Request["PriceTo"], 0);


        }

        public PaymentQuery(HttpRequestBase Request)
        {
            QueryType = Types.ToInt(Request["QueryType"]);

            //string op = Request["op"];
            //if (op == "/Crm/PaymentsExport")
            //{
            //    if (QueryType == 1)
            //        QueryType = 21;
            //    else
            //        QueryType = 20;
            //}
            //else //if(op=="/Crm/PaymentsReport")
            //{
            //    if (QueryType == 1)
            //        HTitle = "פירוט תשלומים שנכשלו";
            //    else
            //        HTitle = "פירוט תשלומים שהתקבלו";

            //}   

            AccountId = Types.ToInt(Request["AccountId"]);
            MemberId = Request["MemberId"];
            CellPhone = Request["CellPhone"];
            Name = Request["Name"];
            Items = Request["Items"];
            //ValidityRemain =Types.ToInt( Request["ValidityRemain"]);
            Campaign = Types.ToInt(Request["Campaign"], 0);
            SignupDateFrom = Types.ToNullableDate(Request["SignupDateFrom"]);
            SignupDateTo = Types.ToNullableDate(Request["SignupDateTo"]);
            PriceFrom = Types.ToDecimal(Request["PriceFrom"], 0);
            PriceTo = Types.ToDecimal(Request["PriceTo"], 0);
            ContactRule = Types.ToInt(Request["ContactRule"]);

            LoadSortAndFilter(Request,"");
        }

        public void Normelize()
        {
            if (MemberId == "")
                MemberId = null;
            if (CellPhone == "")
                CellPhone = null;
            if (Name == "")
                Name = null;
            if (Items == "")
                Items = null;
            

            MemberId = SqlFormatter.ValidateSqlInput(MemberId);
            Name = SqlFormatter.ValidateSqlInput(Name);
            Items = SqlFormatter.ValidateSqlInput(Items);
            CellPhone = SqlFormatter.ValidateSqlInput(CellPhone);
        }

        public string MemberId{ get; set; }
        public string CellPhone { get; set; }
        public string Name{ get; set; }
        public string Items{ get; set; }
        //public int ValidityRemain { get; set; }
        public int Campaign { get; set; }
        public DateTime? SignupDateFrom { get; set; }
        public DateTime? SignupDateTo { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        public int ContactRule{ get; set; }
       
    }
}
