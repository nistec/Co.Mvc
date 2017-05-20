using Nistec;
using Nistec.Data;
using Nistec.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Pro.Lib
{
    [Serializable]
    public class PaymentQuery
    {

        public string Serialize()
        {
           var ser= BinarySerializer.SerializeToBase64(this);
           return ser;
        }

        public static PaymentQuery Deserialize(string ser)
        {
            return BinarySerializer.DeserializeFromBase64<PaymentQuery>(ser);
        }

        public int PageSize { get; set; }
        public int PageNum{ get; set; }

        //public string Sortfield;
        //public string Sortorder;

        //public string FilterValue;
        //public string FilterCondition;
        //public string FilterDataField;
        //public string FilterOperator;

        public string Sort { get; set; }
        public string Filter { get; set; }


        public int QueryType { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set; }

        public PaymentQuery()
        {
            MemberId = null;
            CellPhone = null;
            Name = null;
            Address = null;
            City = null;
            Category = null;
            Region = null;
            Place = null;
            Branch = null;
            Status = -1;
            BirthdayMonth = 0;
            JoinedFrom = 0;
            JoinedTo = 0;
            AgeFrom = 0;
            AgeTo = 0;
            ContactRule = 0;
        }


        public static PaymentQuery Create(int pagesize, int pagenum,
            int QueryType,
            int accountId,
            int userId,
            string MemberId,
            string CellPhone,
            string Name,
            string Address,
            string City,
            string Category,
            string Region,
            string Place,
            string Branch,
            int Status,
            int BirthdayMonth,
            int JoinedFrom,
            int JoinedTo,
            int AgeFrom,
            int AgeTo,
            int ContactRule)
        {

            PaymentQuery query = new PaymentQuery()
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
                CellPhone=CellPhone,
                Name = Name,
                Place = Place,
                QueryType = QueryType,
                Region = Region,
                Status = Status,
                PageNum = pagenum,
                PageSize = pagesize
            };
            query.Normelize();
            return query;
        }

        public PaymentQuery(NameValueCollection Request)
        {
            string query_type = Request["query_type"];

            MemberId = Types.NZorEmpty(Request["memID"], null);
            if (MemberId != null)
                return;

            CellPhone = Types.NZorEmpty(Request["CellPhone"], null);
            if (CellPhone != null)
                return;

            string listBranch = Request["Branch"];
            bool allBranch = Request["allBranch"] == "on";
            if (!allBranch)
                Branch = listBranch;

            string listRegion = Request["Region"];
            bool allRegion = Request["allRegion"] == "on";
            if (!allRegion)
                Region = listRegion;

            string listCity = Request["City"];
            bool allCity = Request["allCity"] == "on";
            if (!allCity)
                City = listCity;

            string listCategory = Request["Category"];
            bool allCategory = Request["allCategory"] == "on";
            if (!allCategory)
                Category = listCategory;

            string listPlace = Request["Place"];
            bool allPlace = Request["allPlace"] == "on";
            if (!allPlace)
                Place = listPlace;
            Status = -1;
            int listStatus = Types.ToInt(Request["Status"], -1);
            bool allStatus = Request["allStatus"] == "on";
            if (!allStatus)
                Status = listStatus;


            string Address = Types.NZorEmpty(Request["address"], null);

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

            AgeFrom = Types.ToInt(Request["ageFrom"]);
            AgeTo = Types.ToInt(Request["ageTo"]);

        }
  
        public void Normelize()
        {
            if (MemberId == "")
                MemberId = null;
            if (CellPhone == "")
                CellPhone = null;
            if (Name == "")
                Name = null;
            if (Address == "")
                Address = null;
            if (City == "")
                City = null;
            if (Category == "")
                Category = null;
            if (Region == "")
                Region = null;
            if (Place == "")
                Place = null;
            if (Branch == "")
                Branch = null;

            MemberId = SqlFormatter.ValidateSqlInput(MemberId);
            Name = SqlFormatter.ValidateSqlInput(Name);
            Address = SqlFormatter.ValidateSqlInput(Address);
            //Filter = SqlFormatter.ValidateSqlInput(Filter);
        }

         { name: 'AccountId', type: 'number' },
               { name: 'MemberId', type: 'string' },
               { name: 'MemberName', type: 'string' },
               { name: 'CellPhone', type: 'string' },
               
               { name: 'PayId', type: 'number' },
               { name: 'Payed', type: 'number' },
               { name: 'Creation', type: 'date' },
               { name: 'TransIndex', type: 'string' },
               { name: 'ConfirmationCode', type: 'string' },

               { name: 'SignupDate', type: 'date' },
               { name: 'ValidityMonth', type: 'number' },
               { name: 'CreditCardOwner', type: 'bool' },
               { name: 'ExpirationDate', type: 'date' },
               { name: 'TotalRows', type: 'number' }

        public string MemberId{ get; set; }
        public string CellPhone { get; set; }
        public string MemberName{ get; set; }

      
    }
}
