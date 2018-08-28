using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using ProNetcell.Data;
using Nistec.Data;
using System.Data;
using Pro.Netcell.Sender;

namespace ProNetcell.Data.Entities
{
    public class TargetListView : TargetView
    {
 

        public static IEnumerable<TargetListView> ViewList(int PageSize, int PageNum, int AccountId, int UserId, string targetFilter, string personalFilter)
        {
            using (var db=DbContext.Create<DbPro>())
            return db.ExecuteList<TargetListView>("sp_Targets_View", "@PageSize", PageSize, "PageNum", PageNum, "AccountId", AccountId, "UserId", UserId, "TargetFilter", targetFilter, "PersonalFilter", personalFilter);
        }
        public int TotalRows { get; set; }
    }

    public class TargetView : IEntityItem
    {

        public static IList<TargetView>   ViewList(int AccountId,int UserId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<TargetView>("Targets", "AccountId", AccountId, "UserId", UserId);
        }
 
        public static IList<TargetView> ViewSmsListByCategories(int AccountId, string Categories, bool isAll)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.ExecuteList<TargetView>("sp_Targets_ViewByCategories", "TotalCount", 0,"AccountId", AccountId, "Categories", Categories, "Platform", 1, "IsCount", false);
        }
        public static IList<TargetView> ViewEmailListByCategories(int AccountId, string Categories, bool isAll)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.ExecuteList<TargetView>("sp_Targets_ViewByCategories", "TotalCount", 0, "AccountId", AccountId, "Categories", Categories, "Platform", 2, "IsCount", false);
        }


        //public static IList<TargetView> ViewSmsListByCategory(int AccountId, int CategoryId, bool isAll)
        //{
        //    using (var db = DbContext.Create<DbPro>())
        //        return db.ExecuteList<TargetView>("sp_Targets_ViewByCategory", "AccountId", AccountId, "CategoryId", CategoryId, "Platform", 1, "IsAll", isAll, "IsCount", false, "TotalCount", 0);
        //}
        //public static IList<TargetView> ViewEmailListByCategory(int AccountId, int CategoryId, bool isAll)
        //{
        //    using (var db = DbContext.Create<DbPro>())
        //        return db.ExecuteList<TargetView>("sp_Targets_ViewByCategory", "AccountId", AccountId, "CategoryId", CategoryId, "Platform", 2, "IsAll", isAll, "IsCount", false, "TotalCount", 0);
        //}

        public static int ViewSmsCountByCategory(int AccountId, int CategoryId, bool isAll)
        {
            var parameters = DataParameter.GetSqlWithDirection(
                "AccountId", AccountId, 0, "CategoryId", CategoryId, 0, "Platform", 1, 0, "IsAll", isAll, 0, "IsCount", true, 0, "TotalCount", 0, 2);
            using (var db = DbContext.Create<DbPro>())
            {
                var res = db.ExecuteCommandNonQuery("sp_Targets_ViewByCategory", parameters, CommandType.StoredProcedure);
                return parameters.GetParameterValue<int>("TotalCount");
            }
        }
        public static int ViewEmailCountByCategory(int AccountId, int CategoryId, bool isAll)
        {
            var parameters = DataParameter.GetSqlWithDirection(
                "AccountId", AccountId, 0, "CategoryId", CategoryId, 0, "Platform", 2, 0, "IsAll", isAll, 0, "IsCount", true, 0, "TotalCount", 0, 2);
            using (var db = DbContext.Create<DbPro>())
            {
                var res = db.ExecuteCommandNonQuery("sp_Targets_ViewByCategory", parameters, CommandType.StoredProcedure);
                return parameters.GetParameterValue<int>("TotalCount");
            }
        }
        public static int ViewSmsCountByCategory(int AccountId, string Categories, bool isAll)
        {
            if (isAll)
                Categories = "-ALL-";

            var parameters = DataParameter.GetSqlWithDirection(
                "TotalCount", 0, 2, "AccountId", AccountId, 0, "Categories", Categories, 0, "Platform", 1, 0, "IsCount", true, 0);
            using (var db = DbContext.Create<DbPro>())
            {
                var res = db.ExecuteCommandNonQuery("sp_Targets_ViewByCategories", parameters, CommandType.StoredProcedure);
                return parameters.GetParameterValue<int>("TotalCount");
            }
        }
        public static int ViewEmailCountByCategory(int AccountId, string Categories, bool isAll)
        {
            if (isAll)
                Categories = "-ALL-";
            var parameters = DataParameter.GetSqlWithDirection(
                "TotalCount", 0, 2, "AccountId", AccountId, 0, "Categories", Categories, 0, "Platform", 1, 0, "IsCount", true, 0);
            using (var db = DbContext.Create<DbPro>())
            {
                var res = db.ExecuteCommandNonQuery("sp_Targets_ViewByCategories", parameters, CommandType.StoredProcedure);
                return parameters.GetParameterValue<int>("TotalCount");
            }
        }



        public static DataTable ViewData(int AccountId, int UserId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.ExecuteCommand<DataTable>("select Target,Personal from Targets where AccountId=@AccountId and UserId=@UserId", DataParameter.GetSql("AccountId",AccountId,"UserId",UserId));
        }

        public static string ToJson(IEnumerable<TargetView> targets, bool isPersonal)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var t in targets)
            {
                sb.AppendFormat(t.ToJson(isPersonal));
            }
            string items = sb.ToString().TrimEnd(',');

            return "[" + items + "]";
        }

        #region properties

        public string Target { get; set; }
        public string Personal { get; set; }
        public int AccountId { get; set; }
        //public DateTime Creation { get; set; }
        //public int UserId { get; set; }
        public int Id { get; set; }


        #endregion

        public string ToJson(bool isPersonal)
        {
            const string ToPersonal = "\"Personal\":\"{1}\",\"To\":\"{0}\"";
            const string To = "\"To\":\"{0}\"";
            if (isPersonal)
                return "{" + string.Format(ToPersonal, Target, Personal) + "},";
            else
                return "{" + string.Format(To, Target) + "},";
        }
    }
}
