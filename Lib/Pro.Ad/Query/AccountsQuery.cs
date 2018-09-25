using Nistec;
using Nistec.Data;
using Nistec.Serialization;
using Pro.Query;
using ProSystem.Query;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace ProAd.Query
{
    [Serializable]
    public class AccountsQuery:QueryBase
    {

        public string Serialize()
        {
           var ser= BinarySerializer.SerializeToBase64(this);
           return ser;
        }

        public static AccountsQuery Deserialize(string ser)
        {
            return BinarySerializer.DeserializeFromBase64<AccountsQuery>(ser);
        }

        //public string Sortfield;
        //public string Sortorder;

        //public string FilterValue;
        //public string FilterCondition;
        //public string FilterDataField;
        //public string FilterOperator;
        public int QueryType { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public int ExType { get; set; }
        public AccountsQuery()
        {
            IdNumber = null;
            Mobile = null;
            AccountName = null;
            Address = null;
            City = null;
            Category = null;
            Branch = null;
            JoinedFrom = 0;
            JoinedTo = 0;
            ContactRule = 0;
        }

        public AccountsQuery(HttpRequestBase Request)
        {

            QueryType = QueryType = Types.ToInt(Request["QueryType"]);
            AccountId = Types.ToInt(Request["AccountId"]);
            //ExType = Types.ToInt(Request["ExType"]);

            IdNumber = Request["IdNumber"];
            Email = Request["Email"];
            Mobile = Request["Mobile"];
            Category = Request["Category"];
            Branch = Request["Branch"];


            AccountName = Request["AccountName"];
            Address = Request["Address"];
            City = Request["City"];
            Category = Request["Category"];
            Branch = Request["Branch"];
            JoinedFrom = Types.ToInt(Request["JoinedFrom"]);
            JoinedTo = Types.ToInt(Request["JoinedTo"]);
            ContactRule = Types.ToInt(Request["ContactRule"]);


            LoadSortAndFilter(Request,"");
        }

        public AccountsQuery(NameValueCollection Request, int queryType)
        {
            if (Request.Count == 0)
                return;

            //string query_type = Request["query_type"];
            QueryType = queryType;// Types.ToInt(Request["QueryType"]);
            //ExType = Types.ToInt(Request["ExType"]);

            if (queryType == 1)
            {
                IdNumber = Request["IdNumber"];
                Email = Request["Email"];
                Mobile = Request["Mobile"];
                Category = Request["Category"];
                Branch = Request["Branch"];
            }
            else
            {
                IdNumber = Types.NZorEmpty(Request["IdNumber"], null);
                if (IdNumber != null)
                {
                    QueryType = 1;
                    return;
                }

                Mobile = Types.NZorEmpty(Request["Mobile"], null);
                if (Mobile != null)
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

                string listBranch = Request["Branch"];
                bool allBranch = Request["allBranch"] == "on";
                if (!allBranch)
                    Branch = listBranch;

                string listCity = Request["City"];
                bool allCity = Request["allCity"] == "on";
                if (!allCity)
                    City = listCity;

                string listCategory = Request["Category"];
                bool allCategory = Request["allCategory"] == "on";
                if (!allCategory)
                    Category = listCategory;

                Address = Types.NZorEmpty(Request["Address"], null);

                int monthlyTime = Types.ToInt(Request["monthlyTime"]);
                if (monthlyTime > 0)
                {
                    JoinedFrom = monthlyTime;
                    JoinedTo = 0;
                }
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
               
            }

        }

        public void Normelize()
        {
            if (IdNumber == "")
                IdNumber = null;
            if (Mobile == "")
                Mobile = null;
            if (Email == "")
                Email = null;
            if (AccountName == "")
                AccountName = null;
            if (Address == "")
                Address = null;
            if (City == "")
                City = null;
            if (Category == "")
                Category = null;

            if (Branch == "")
                Branch = null;

            IdNumber = SqlFormatter.ValidateSqlInput(IdNumber);
            AccountName = SqlFormatter.ValidateSqlInput(AccountName);
            Address = SqlFormatter.ValidateSqlInput(Address);
            //Filter = SqlFormatter.ValidateSqlInput(Filter);

           
        }

       
            //public int AccountId { get; set; }
            public int ParentId { get; set; }
            public string AccountName { get; set; }
            public string ContactName { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string ZipCode { get; set; }
            public string Phone { get; set; }
            public string Fax { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public string IdNumber { get; set; }
            public int Country { get; set; }
            public int OwnerId { get; set; }
            public int AccType { get; set; }
            public int AccountCategory { get; set; }
            public string Details { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreationDate { get; set; }
            public DateTime? LastUpdate { get; set; }
            public string Path { get; set; }
            public string Branch { get; set; }


        public string Category { get; set; }
        public int JoinedFrom{ get; set; }
        public int JoinedTo{ get; set; }
        public int ContactRule{ get; set; }
        
 
    }
}
