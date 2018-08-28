using Nistec;
using Nistec.Data;
using Nistec.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Pro.Netcell.Query
{
    [Serializable]
    public class MemberQuery:QueryBase
    {

        public string Serialize()
        {
           var ser= BinarySerializer.SerializeToBase64(this);
           return ser;
        }

        public static MemberQuery Deserialize(string ser)
        {
            return BinarySerializer.DeserializeFromBase64<MemberQuery>(ser);
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
        public MemberQuery()
        {
            MemberId = null;
            ExId = null;
            ExType = 0;
            CellPhone = null;
            Name = null;
            Address = null;
            City = null;
            Category = null;
            Region = 0;
            //Place = null;
            Branch = null;
            //Status = -1;
            ExEnum1 = 0;
            ExEnum2 = 0;
            ExEnum3 = 0;
            BirthdayMonth = 0;
            JoinedFrom = 0;
            JoinedTo = 0;
            AgeFrom = 0;
            AgeTo = 0;
            ContactRule = 0;
        }

        public MemberQuery(HttpRequestBase Request)
        {

            QueryType = QueryType = Types.ToInt(Request["QueryType"]);
            AccountId = Types.ToInt(Request["AccountId"]);
            //ExType = Types.ToInt(Request["ExType"]);
            
            //if (QueryType == 1)
            //{
                MemberId = Request["MemberId"];
                ExId = Request["ExId"];
                Email = Request["Email"];
                CellPhone = Request["CellPhone"];
                Category = Request["Category"];
                Branch = Request["Branch"];
            //}
            //else
            //{
                
                Name = Request["Name"];
                Address = Request["Address"];
                City = Request["City"];
                Region = Types.ToInt(Request["Region"]);
                Category = Request["Category"];
                Branch = Request["Branch"];
                ExEnum1 = Types.ToInt(Request["ExEnum1"]);
                ExEnum2 = Types.ToInt(Request["ExEnum2"]);
                ExEnum3 = Types.ToInt(Request["ExEnum3"]);
                BirthdayMonth = Types.ToInt(Request["BirthdayMonth"]);
                JoinedFrom = Types.ToInt(Request["JoinedFrom"]);
                JoinedTo = Types.ToInt(Request["JoinedTo"]);
                AgeFrom = Types.ToInt(Request["AgeFrom"]);
                AgeTo = Types.ToInt(Request["AgeTo"]);
                ContactRule = Types.ToInt(Request["ContactRule"]);

                //signup query
                Items = Request["Items"];
                ReferralCode = Request["ReferralCode"];
                ValidityRemain = Types.ToInt(Request["ValidityRemain"], 0);
                Campaign = Types.ToInt(Request["Campaign"], 0);
                SignupDateFrom = Types.ToNullableDate(Request["SignupDateFrom"]);
                SignupDateTo = Types.ToNullableDate(Request["SignupDateTo"]);
                PriceFrom = Types.ToDecimal(Request["PriceFrom"], 0);
                PriceTo = Types.ToDecimal(Request["PriceTo"], 0);
                HasAutoCharge = Types.ToInt(Request["HasAutoCharge"]);// == "on";
                HasPayment = Types.ToInt(Request["HasPayment"]);// == "on";
                HasSignup = Types.ToInt(Request["HasSignup"]);// == "on";

            //}
            LoadSortAndFilter(Request);
        }

        public  MemberQuery(NameValueCollection Request, int queryType)
        {
            if (Request.Count == 0)
                return;

            //string query_type = Request["query_type"];
            QueryType = queryType;// Types.ToInt(Request["QueryType"]);
            //ExType = Types.ToInt(Request["ExType"]);

            if (queryType == 1)
            {
                MemberId = Request["MemberId"];
                ExId = Request["ExId"];
                Email = Request["Email"];
                CellPhone = Request["CellPhone"];
                Category = Request["Category"];
                Branch = Request["Branch"];
            }
            else
            {
                MemberId = Types.NZorEmpty(Request["MemberId"], null);
                if (MemberId != null)
                {
                    QueryType = 1;
                    return;
                }

                ExId = Types.NZorEmpty(Request["ExId"], null);
                if (ExId != null)
                {
                    QueryType = 1;
                    return;
                }

                CellPhone = Types.NZorEmpty(Request["CellPhone"], null);
                if (CellPhone != null)
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

                string listRegion = Request["Region"];
                bool allRegion = Request["allRegion"] == "on";
                if (!allRegion)
                    Region = Types.ToInt(listRegion);

                string listCity = Request["City"];
                bool allCity = Request["allCity"] == "on";
                if (!allCity)
                    City = listCity;

                string listCategory = Request["Category"];
                bool allCategory = Request["allCategory"] == "on";
                if (!allCategory)
                    Category = listCategory;

                Address = Types.NZorEmpty(Request["Address"], null);

                bool allBirthday = Request["allBirthday"] == "on";
                if (allBirthday)
                    BirthdayMonth = DateTime.Now.Month;

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

                AgeFrom = Types.ToInt(Request["AgeFrom"]);
                AgeTo = Types.ToInt(Request["AgeTo"]);


                //signup query
                string listItems = Request["Items"];
                bool allItems = Request["allItems"] == "on";
                if (!allItems)
                    Items = listItems;
                ReferralCode = Request["ReferralCode"];

                string val = "";
                val = Request["ValidityRemain"];
                ValidityRemain = (val == "") ? 0 : Types.ToInt(Request["ValidityRemain"], 0);
                val = Request["Campaign"];
                Campaign = (val == "") ? 0 : Types.ToInt(Request["Campaign"], 0);

                SignupDateFrom = Types.ToNullableDate(Request["SignupDateFrom"]);
                SignupDateTo = Types.ToNullableDate(Request["SignupDateTo"]);

                PriceFrom = Types.ToDecimal(Request["PriceFrom"], 0);
                PriceTo = Types.ToDecimal(Request["PriceTo"], 0);

                HasAutoCharge = Types.ToInt(Request["HasAutoCharge"]);// == "on";
                HasPayment = Types.ToInt(Request["HasPayment"]);// == "on";
                HasSignup = Types.ToInt(Request["HasSignup"]);// == "on";
            }

        }
/*
        public static MemberQuery Create(int pagesize, int pagenum,
            int QueryType,
            int accountId,
            int userId,
            string MemberId,
            string ExId,
            string CellPhone,
            string Email,
            string Name,
            string Address,
            string City,
            string Category,
            int Region,
            //string Place,
            string Branch,
            int ExEnum1,
            int ExEnum2,
            int ExEnum3,
            //int Status,
            int BirthdayMonth,
            int JoinedFrom,
            int JoinedTo,
            int AgeFrom,
            int AgeTo,
            int ContactRule)
        {

            MemberQuery query = new MemberQuery()
            {
                AccountId = accountId,
                UserId=userId,
                Address = Address,
                AgeFrom = AgeFrom,
                AgeTo = AgeTo,
                BirthdayMonth = BirthdayMonth,
                Branch = Branch,
                Category = Category,
                City = City,
                ContactRule = ContactRule,
                JoinedFrom = JoinedFrom,
                JoinedTo = JoinedTo,
                MemberId = MemberId,
                ExId=ExId,
                CellPhone=CellPhone,
                Email=Email,
                Name = Name,
                //Place = Place,
                QueryType = QueryType,
                Region = Region,
                ExEnum1=ExEnum1,
                ExEnum2=ExEnum2,
                ExEnum3=ExEnum3,
                //Status = Status,
                PageNum = pagenum,
                PageSize = pagesize
            };
            query.Normelize();
            return query;
        }

 
*/
        //public MemberQuery(HttpRequestBase Request)
        //{
        //    string query_type = Request.Form["query_type"];

        //    MemberId = Types.NZorEmpty(Request.Form["memID"], null);
        //    if (MemberId != null)
        //        return;

        //    string listBranch = Request.Form["listBranch"];
        //    bool allBranch = Request.Form["allBranch"]=="on";
        //    if (!allBranch)
        //        Branch = listBranch;

        //    string listRegion = Request.Form["listRegion"];
        //    bool allRegion = Request.Form["allRegion"]=="on";
        //    if (!allRegion)
        //        Region = listRegion;

        //    string listCity = Request.Form["listCity"];
        //    bool allCity = Request.Form["allCity"]=="on";
        //    if (!allCity)
        //        City = listCity;

        //    string listCategory = Request.Form["listCategory"];
        //    bool allCategory = Request.Form["allCategory"]=="on";
        //    if (!allCategory)
        //        Category = listCategory;

        //    string listPlace = Request.Form["listPlace"];
        //    bool allPlace = Request.Form["allPlace"]=="on";
        //    if (!allPlace)
        //        Place = listPlace;
        //    Status = -1;
        //    int listStatus = Types.ToInt(Request.Form["listStatus"], -1);
        //    bool allStatus = Request.Form["allStatus"]=="on";
        //    if (!allStatus)
        //        Status = listStatus;


        //    string Address = Types.NZorEmpty(Request.Form["address"], null);
            
        //    bool allBirthday = Request.Form["allBirthday"]=="on";
        //    if (allBirthday)
        //        BirthdayMonth = DateTime.Now.Month;

        //    int monthlyTime = Types.ToInt(Request.Form["monthlyTime"]);
        //    if (monthlyTime > 0)
        //    {
        //        JoinedFrom = monthlyTime;
        //        JoinedTo = 0;
        //    }
        //    bool allCell = Request.Form["allCell"]=="on";
        //    bool allEmail = Request.Form["allEmail"]=="on";

        //    if (allCell && allEmail)
        //        ContactRule = 3;
        //    if (allCell)
        //        ContactRule = 1;
        //    if (allEmail)
        //        ContactRule = 2;
        //    else
        //        ContactRule = 0;

        //    AgeFrom = Types.ToInt(Request.Form["ageFrom"]);
        //    AgeTo = Types.ToInt(Request.Form["ageTo"]);

        //}

        public void Normelize()
        {
            if (MemberId == "")
                MemberId = null;
            if (ExId == "")
                ExId = null;
            if (CellPhone == "")
                CellPhone = null;
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
            if (Branch == "")
                Branch = null;

            MemberId = SqlFormatter.ValidateSqlInput(MemberId);
            Name = SqlFormatter.ValidateSqlInput(Name);
            Address = SqlFormatter.ValidateSqlInput(Address);
            //Filter = SqlFormatter.ValidateSqlInput(Filter);

            if (!string.IsNullOrEmpty(City))
                Region = 0;

            //signup query
            Items = SqlFormatter.ValidateSqlInput(Items);
            if (ReferralCode == "")
                ReferralCode = null;
        }

        public string MemberId{ get; set; }
        public string ExId { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Name{ get; set; }
        public string Address{ get; set; }
        public string City{ get; set; }
        public string Category{ get; set; }
        public int Region{ get; set; }
        public string Branch { get; set; }

        //public int Status{ get; set; }
        //public string Place{ get; set; }
        
        public int ExEnum1 { get; set; }
        public int ExEnum2 { get; set; }
        public int ExEnum3 { get; set; }

        public int BirthdayMonth{ get; set; }
        public int JoinedFrom{ get; set; }
        public int JoinedTo{ get; set; }
        public int AgeFrom{ get; set; }
        public int AgeTo{ get; set; }
        public int ContactRule{ get; set; }
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
 
    }
}
