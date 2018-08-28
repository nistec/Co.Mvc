using Nistec;
using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Serialization;
using ProNetcell.Data;
using ProNetcell.Data.Entities.Props;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Pro.Netcell.Query
{

#if(false)
    [Serializable]
    public class CategoryMergeQuery//:QueryBase
    {

        public string Serialize()
        {
            var ser = BinarySerializer.SerializeToBase64(this);
            return ser;
        }

        public static CategoryMergeQuery Deserialize(string ser)
        {
            return BinarySerializer.DeserializeFromBase64<CategoryMergeQuery>(ser);
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

        public CategoryMergeQuery()
        {
            //MemberId = null;
            //ExId = null;
            //CellPhone = null;
            //Name = null;
            //Address = null;
            City = null;
            Category = null;
            //Region = 0;
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

        public CategoryMergeQuery(HttpRequestBase Request)
        {
            CategoryTo = Request["newCategory"];

            CategoryId = Types.ToInt(Request["CategoryId"]);
            MergeType = Types.ToInt(Request["MergeType"]);

            QueryType = Types.ToInt(Request["QueryType"]);
            AccountId = Types.ToInt(Request["AccountId"]);
            //MemberId = Request["MemberId"];
            //ExId = Request["ExId"];
            //Email = Request["Email"];
            //CellPhone = Request["CellPhone"];
            //Name = Request["Name"];
            //Address = Request["Address"];
            City = Request["City"];
            Region = Request["Region"];
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

            //LoadSortAndFilter(Request);
        }

        public CategoryMergeQuery(NameValueCollection Request)
        {
            CategoryTo = Request["newCategory"];

            CategoryId = Types.ToInt(Request["CategoryId"]);

            string query_type = Request["query_type"];

            MergeType = Types.ToInt(Request["MergeType"]);


            //MemberId = Types.NZorEmpty(Request["MemberId"], null);
            //if (MemberId != null)
            //    return;

            //CellPhone = Types.NZorEmpty(Request["CellPhone"], null);
            //if (CellPhone != null)
            //    return;

            //Email = Types.NZorEmpty(Request["Email"], null);
            //if (Email != null)
            //    return;

            string listBranch = Request["Branch"];
            bool allBranch = Request["allBranch"] == "on";
            if (!allBranch)
                Branch = listBranch;

            string listRegion = Request["Region"];
            bool allRegion = Request["allRegion"] == "on";
            if (!allRegion)
                Region = null;// Types.ToInt(listRegion);

            string listCity = Request["City"];
            bool allCity = Request["allCity"] == "on";
            if (!allCity)
                City = listCity;

            string listCategory = Request["Category"];
            bool allCategory = Request["allCategory"] == "on";
            if (!allCategory)
                Category = listCategory;

            //Address = Types.NZorEmpty(Request["address"], null);

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
            if (CategoryTo == "")
                CategoryTo = null;
            if (City == "")
                City = null;
            if (Category == "")
                Category = null;
            if (Region == "")
                Region = null;
            //if (Place == "")
            //    Place = null;
            if (Branch == "")
                Branch = null;
            if (ExField1 == "")
                ExField1 = null;
            if (ExField2 == "")
                ExField2 = null;
            if (ExField3 == "")
                ExField3 = null;
            if (EvText1 == "")
                EvText1 = null;
            if (EvText2 == "")
                EvText2 = null;
            if (EvText3 == "")
                EvText3 = null;

            if (!string.IsNullOrEmpty(City))
                Region = null;

        }

        //public string MemberId{ get; set; }
        //public string ExId { get; set; }
        //public string CellPhone { get; set; }
        //public string Email { get; set; }
        public string CategoryTo { get; set; }
        public int MergeType { get; set; }
        public string City { get; set; }
        public string Category { get; set; }
        public string Region { get; set; }
        public string Branch { get; set; }

        //public int Status{ get; set; }
        //public string Place{ get; set; }

        public int ExEnum1 { get; set; }
        public int ExEnum2 { get; set; }
        public int ExEnum3 { get; set; }
        public string ExField1 { get; set; }
        public string ExField2 { get; set; }
        public string ExField3 { get; set; }
        public int BirthdayMonth { get; set; }
        public int JoinedFrom { get; set; }
        public int JoinedTo { get; set; }
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public int ContactRule { get; set; }
        //public int QueryType;
        //--signup
        public DateTime? ExpirationDate { get; set; }
        public decimal? Price { get; set; }

        //--action
        public DateTime? EvDate1 { get; set; }
        public DateTime? EvDate2 { get; set; }
        public DateTime? EvDate3 { get; set; }
        public string EvText1 { get; set; }
        public string EvText2 { get; set; }
        public string EvText3 { get; set; }


        //--options
        public bool IsClean { get; set; }
        public bool IsDebug { get; set; }

        public int CategoryId { get; set; }


        public int ExecuteMerge()//0=mergeTo,1=MergeAppend,2=mergeRemove,3=mergeDelete
        {
            //select count(*) as TotalRows from [dbo].[Members_Categories] where AccountId=@AccountId and PropId=@CategoryId

            string categoryTo = CategoryId.ToString();
            if (MergeType == 1)
            {
                categoryTo = CategoryTo;
            }
            else if (CategoryId <= 0)
            {
                throw new ArgumentException("Invalid category to...");
            }

            string Place = null;
            int Status = 0;
            int TotalRows = 0;
/*
MergeType tinyint=0,--0=mergeTo,1=MergeAppend,2=mergeRemove,3=mergeDelete
CategoryTo nvarchar(50),
UserId int,
AccountId int,
City varchar(500)=null,
Category varchar(500)=null,
Region int=0,
Branch varchar(500)=null,
ExEnum1 int=0,
ExEnum2 int=0,
ExEnum3 int=0,
BirthdayMonth int=0,
JoinedFrom int=0,
JoinedTo int=0,
AgeFrom int=0,
AgeTo int=0,
ContactRule int=0,
Filter varchar(1000)=null,
//--signup query
Items varchar(500)=null,
ReferralCode varchar(50)=null,
ValidityRemain int=0,
Campaign int=0,
SignupDateFrom datetime=null,
SignupDateTo datetime=null,
PriceFrom decimal(9,2)=0,
PriceTo decimal(9,2)=0,
HasSignup tinyint=0,
HasPayment tinyint=0,
HasAutoCharge tinyint=0,
ExpirationDateFrom date=null,
ExpirationDateTo date=null,
//--action
EvDate1 date=null,
EvDate2 date=null,
EvDate3 date=null,
EvText1 nvarchar(250)=null,
EvText2 nvarchar(250)=null,
EvText3 nvarchar(250)=null,
//--options
IsClean bit=0,
IsDebug bit=1
*/

            using (var db = DbContext.Create<DbPro>())
            {
                TotalRows = db.ExecuteScalar<int>("sp_Category_Merge_v1", 0, "MergeType", MergeType, "CategoryTo", categoryTo, "UserId", UserId, "AccountId", AccountId,
                    "City", City,
                    "Category", Category,
                    "Region", Region,
                    "Branch", Branch,
                    //"ExField1", ExField1,
                    //"ExField2", ExField2,
                    //"ExField3", ExField3,
                    "ExEnum1", ExEnum1,
                    "ExEnum2", ExEnum2,
                    "ExEnum3", ExEnum3,
                    "BirthdayMonth", BirthdayMonth,
                    "JoinedFrom", JoinedFrom,
                    "JoinedTo", JoinedTo,
                    "AgeFrom", AgeFrom,
                    "AgeTo", AgeTo,
                    "ExpirationDate", ExpirationDate,
                    "Price", Price,
                    "EvDate1", EvDate1,
                    "EvDate2", EvDate2,
                    "EvDate3", EvDate3,
                    "EvText1", EvText1,
                    "EvText2", EvText2,
                    "EvText3", EvText3,
                    "IsClean", IsClean,
                    "IsDebug", IsDebug
                    );


            }

            CategoryView.Refresh(AccountId);

            return TotalRows;
        }

    }
#endif

    [Serializable]
    public class CategoryMergeQuery//:QueryBase
    {

        public string Serialize()
        {
           var ser= BinarySerializer.SerializeToBase64(this);
           return ser;
        }

        public static CategoryMergeQuery Deserialize(string ser)
        {
            return BinarySerializer.DeserializeFromBase64<CategoryMergeQuery>(ser);
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

        public CategoryMergeQuery()
        {
            //MemberId = null;
            //ExId = null;
            //CellPhone = null;
            //Name = null;
            //Address = null;
            City = null;
            Category = null;
            //Region = 0;
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

        public CategoryMergeQuery(HttpRequestBase Request)
        {
            CategoryTo = Request["newCategory"];

            CategoryId = Types.ToInt(Request["CategoryId"]);
            MergeType = Types.ToInt(Request["MergeType"]);
            
            QueryType = Types.ToInt(Request["QueryType"]);
            AccountId = Types.ToInt(Request["AccountId"]);
            //MemberId = Request["MemberId"];
            //ExId = Request["ExId"];
            //Email = Request["Email"];
            //CellPhone = Request["CellPhone"];
            //Name = Request["Name"];
            //Address = Request["Address"];
            City = Request["City"];
            Region = Request["Region"];
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

            //LoadSortAndFilter(Request);
        }

        public CategoryMergeQuery(NameValueCollection Request)
        {
            CategoryTo = Request["newCategory"];

            CategoryId = Types.ToInt(Request["CategoryId"]);

            string query_type = Request["query_type"];

            MergeType = Types.ToInt(Request["MergeType"]);
            

            //MemberId = Types.NZorEmpty(Request["MemberId"], null);
            //if (MemberId != null)
            //    return;

            //CellPhone = Types.NZorEmpty(Request["CellPhone"], null);
            //if (CellPhone != null)
            //    return;

            //Email = Types.NZorEmpty(Request["Email"], null);
            //if (Email != null)
            //    return;

            string listBranch = Request["Branch"];
            bool allBranch = Request["allBranch"] == "on";
            if (!allBranch)
                Branch = listBranch;

            string listRegion = Request["Region"];
            bool allRegion = Request["allRegion"] == "on";
            if (!allRegion)
                Region = null;// Types.ToInt(listRegion);

            string listCity = Request["City"];
            bool allCity = Request["allCity"] == "on";
            if (!allCity)
                City = listCity;

            string listCategory = Request["Category"];
            bool allCategory = Request["allCategory"] == "on";
            if (!allCategory)
                Category = listCategory;

            //Address = Types.NZorEmpty(Request["address"], null);

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
            if (CategoryTo == "")
                CategoryTo = null;
            if (City == "")
                City = null;
            if (Category == "")
                Category = null;
            if (Region == "")
                Region = null;
            //if (Place == "")
            //    Place = null;
            if (Branch == "")
                Branch = null;
            if (ExField1 == "")
                ExField1 = null;
            if (ExField2 == "")
                ExField2 = null;
            if (ExField3 == "")
                ExField3 = null;
            if (EvText1 == "")
                EvText1 = null;
            if (EvText2 == "")
                EvText2 = null;
            if (EvText3 == "")
                EvText3 = null;

            if (!string.IsNullOrEmpty(City))
                Region = null;

        }

        //public string MemberId{ get; set; }
        //public string ExId { get; set; }
        //public string CellPhone { get; set; }
        //public string Email { get; set; }
        public string CategoryTo{ get; set; }
        public int MergeType{ get; set; }
        public string City{ get; set; }
        public string Category{ get; set; }
        public string Region{ get; set; }
        public string Branch { get; set; }

        //public int Status{ get; set; }
        //public string Place{ get; set; }
        
        public int ExEnum1 { get; set; }
        public int ExEnum2 { get; set; }
        public int ExEnum3 { get; set; }
        public string ExField1 { get; set; }
        public string ExField2 { get; set; }
        public string ExField3 { get; set; }
        public int BirthdayMonth{ get; set; }
        public int JoinedFrom{ get; set; }
        public int JoinedTo{ get; set; }
        public int AgeFrom{ get; set; }
        public int AgeTo{ get; set; }
        public int ContactRule{ get; set; }
        //public int QueryType;
        //--signup
        public DateTime? ExpirationDate { get; set; }
        public decimal? Price { get; set; }

        //--action
        public DateTime? EvDate1 { get; set; }
        public DateTime? EvDate2 { get; set; }
        public DateTime? EvDate3 { get; set; }
        public string EvText1 { get; set; }
        public string EvText2 { get; set; }
        public string EvText3 { get; set; }


        //--options
        public bool IsClean { get; set; }
        public bool IsDebug { get; set; }
        
        public int CategoryId { get; set; }


        public int ExecuteMerge( )//0=mergeTo,1=MergeAppend,2=mergeRemove,3=mergeDelete
        {
            //select count(*) as TotalRows from [dbo].[Members_Categories] where AccountId=@AccountId and PropId=@CategoryId

            string categoryTo = CategoryId.ToString();
            if(MergeType==1)
            {
                categoryTo = CategoryTo;
            }
            else if (CategoryId <= 0)
            {
                throw new ArgumentException("Invalid category to...");
            }

            string Place = null;
            int Status = 0;
            int TotalRows = 0;
            using (var db = DbContext.Create<DbPro>())
            {
                TotalRows = db.ExecuteScalar<int>("sp_Category_Merge", 0, "MergeType", MergeType, "CategoryTo", categoryTo, "QueryType", QueryType, "AccountId", AccountId,
                    "City", City,
                    "Category", Category,
                    "Region", Region,
                    "Place", Place,
                    "Branch", Branch,
                    "ExField1", ExField1,
                    "ExField2", ExField2,
                    "ExField3", ExField3,
                    "ExEnum1", ExEnum1,
                    "ExEnum2", ExEnum2,
                    "ExEnum3", ExEnum3,
                    "Status", Status,
                    "BirthdayMonth", BirthdayMonth,
                    "JoinedFrom", JoinedFrom,
                    "JoinedTo", JoinedTo,
                    "AgeFrom", AgeFrom,
                    "AgeTo", AgeTo,
                    "ExpirationDate", ExpirationDate,
                    "Price", Price,
                    "EvDate1", EvDate1,
                    "EvDate2", EvDate2,
                    "EvDate3", EvDate3,
                    "EvText1", EvText1,
                    "EvText2", EvText2,
                    "EvText3", EvText3,
                    "IsClean", IsClean,
                    "IsDebug", IsDebug
                    );

               
            }

            CategoryView.Refresh(AccountId);

            return TotalRows;
        }

    }
}
