using Nistec;
using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Generic;
using Nistec.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ProSystem.Query
{
    
    public class QueryModel : QueryBase
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public int TotalRows { get; private set; }
        public string Layout { get; set; }
        public string Url { get; set; }

        public void SetTotalRows<T>(IList<T> list) where T : IEntityListItem
        {
            var row = list.FirstOrDefault<T>();
            TotalRows = row == null ? 0 : row.TotalRows;
        }

        NameValueArgs _Args;
        public NameValueArgs Args
        {
            get {
                if (_Args == null)
                    _Args = new NameValueArgs();
                return _Args; }
        }

        public object[] CreateParameters(System.Web.HttpRequestBase Request, bool enableSortAndFilter, object[] keyValueParameters)
        {
            if (enableSortAndFilter)
            {
                LoadSortAndFilter(Request);

                var filterArray = new object[] { "PageSize", PageSize, "PageNum", PageNum, "Filter", Filter, "Sort", Sort };
                var arr = new object[filterArray.Length + keyValueParameters.Length];
                filterArray.CopyTo(arr, 0);
                keyValueParameters.CopyTo(arr, filterArray.Length);

                return arr;
            }
            else
            {
                return keyValueParameters;
            }
        }

        public static object[] GetParameters(params object[] keyValueParameters)
        {
                  return keyValueParameters;
        }
 
        public static object[] GetParameters(System.Web.HttpRequestBase Request, bool enableSortAndFilter, params object[] keyValueParameters)
        {
            if (enableSortAndFilter)
            {
                var qf= GetSortAndFilter(Request);

                var filterArray = new object[] { "PageSize", qf.PageSize, "PageNum", qf.PageNum, "Filter", qf.Filter, "Sort", qf.Sort };
                var arr = new object[filterArray.Length + keyValueParameters.Length];
                filterArray.CopyTo(arr, 0);
                keyValueParameters.CopyTo(arr, filterArray.Length);

                return arr;
            }
            else
            {
                return keyValueParameters;
            }
        }

        public static QueryModel GetModel(System.Web.HttpRequestBase Request, params string[] keys)
        {
            var model=new QueryModel();
            model.PageSize = Types.ToInt(Request["pagesize"], 20);
            model.PageNum = Types.ToInt(Request["pagenum"]);
            model.AccountId = Types.ToInt(Request["AccountId"]);
            model.UserId= Types.ToInt(Request["UserId"]);

            foreach (string key in keys)
            {
                model.Args[key] = Request[key];
            }

            return model;
        }

        public static QueryModel GetSortAndFilter(System.Web.HttpRequestBase Request)
        {

            int pageSize = Types.ToInt(Request["pagesize"], 20);
            int pageNum = Types.ToInt(Request["pagenum"]);

            string sortfield = Request["sortdatafield"];
            string sortorder = Request["sortorder"] ?? "asc";

            string filter = null;
            int filterscount = Types.ToInt(Request["filterscount"]);
            if (filterscount > 0)
            {
                filter = BuildFilters(filterscount, Request.Params, "m.");
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = " and (" + filter;
                }
            }

            string sort = null;
            if (!string.IsNullOrEmpty(sortfield))
            {
                sort = sortfield + " " + sortorder;
            }

            //Sort = sort;
            //Filter = filter;

            return new QueryModel() { Filter = filter, Sort = sort, PageNum = pageNum, PageSize = pageSize };
        }
    }

    public abstract class QueryBase
    {
        public int PageSize { get; set; }
        public int PageNum { get; set; }
        public string Sort { get; set; }
        public string Filter { get; set; }

        protected void LoadSortAndFilter(System.Web.HttpRequestBase Request)
        {
            PageSize = Types.ToInt(Request["pagesize"],20);
            PageNum = Types.ToInt(Request["pagenum"]);

           string sortfield = Request["sortdatafield"];
            string sortorder = Request["sortorder"] ?? "asc";

            string filter = null;
            int filterscount = Types.ToInt(Request["filterscount"]);
            if (filterscount > 0)
            {
                filter = BuildFilters(filterscount, Request.Params, "m.");
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = " and (" + filter;
                }
            }

            string sort = null;
            if (!string.IsNullOrEmpty(sortfield))
            {
                sort = sortfield + " " + sortorder;
            }

            Sort = sort;
            Filter = filter;
         }
        internal static string BuildQuery(System.Collections.Specialized.NameValueCollection query, string prefix)
        {
            string queryString = ""
            + "  SELECT *, ROW_NUMBER() OVER (ORDER BY " + query.GetValues("sortdatafield")[0] + " "
            + query.GetValues("sortorder")[0].ToUpper() + ") as row FROM Customers "
            + " ";
            var filtersCount = int.Parse(query.GetValues("filterscount")[0]);
            var where = "";
            if (filtersCount > 0)
            {
                where += " WHERE (" + BuildFilters(filtersCount, query, prefix);
            }
            queryString += where;
            return queryString;
        }
        internal static string BuildFilters(int filtersCount, System.Collections.Specialized.NameValueCollection query, string prefix)
        {
            if (prefix == null)
                prefix = "";
            var tmpDataField = "";
            var where = "";
            var tmpFilterOperator = "";
            for (var i = 0; i < filtersCount; i += 1)
            {
                var filterValue = SqlFormatter.ValidateSqlInput(query.GetValues("filtervalue" + i)[0]);
                var filterCondition = query.GetValues("filtercondition" + i)[0];
                var filterDataField = prefix + query.GetValues("filterdatafield" + i)[0];
                var filterOperator = query.GetValues("filteroperator" + i)[0];

                if (tmpDataField == "")
                {
                    tmpDataField = filterDataField;
                }
                else if (tmpDataField != filterDataField)
                {
                    where += ") AND (";
                }
                else if (tmpDataField == filterDataField)
                {
                    if (tmpFilterOperator == "0")
                    {
                        where += " AND ";
                    }
                    else
                    {
                        where += " OR ";
                    }
                }
                // build the "WHERE" clause depending on the filter's condition, value and datafield.
                where += GetFilterCondition(filterCondition, filterDataField, filterValue);
                if (i == filtersCount - 1)
                {
                    where += ")";
                }
                tmpFilterOperator = filterOperator;
                tmpDataField = filterDataField;
            }
            return where;
        }
        internal static string GetFilterCondition(string filterCondition, string filterDataField, string filterValue)
        {
            switch (filterCondition)
            {
                case "NOT_EMPTY":
                case "NOT_NULL":
                    return " " + filterDataField + " NOT LIKE '" + "" + "'";
                case "EMPTY":
                case "NULL":
                    return " " + filterDataField + " LIKE '" + "" + "'";
                case "CONTAINS_CASE_SENSITIVE":
                    return " " + filterDataField + " LIKE '%" + filterValue + "%'" + " COLLATE SQL_Latin1_General_CP1_CS_AS";
                case "CONTAINS":
                    return " " + filterDataField + " LIKE '%" + filterValue + "%'";
                case "DOES_NOT_CONTAIN_CASE_SENSITIVE":
                    return " " + filterDataField + " NOT LIKE '%" + filterValue + "%'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                case "DOES_NOT_CONTAIN":
                    return " " + filterDataField + " NOT LIKE '%" + filterValue + "%'";
                case "EQUAL_CASE_SENSITIVE":
                    return " " + filterDataField + " = '" + filterValue + "'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                case "EQUAL":
                    return " " + filterDataField + " = '" + filterValue + "'";
                case "NOT_EQUAL_CASE_SENSITIVE":
                    return " BINARY " + filterDataField + " <> '" + filterValue + "'";
                case "NOT_EQUAL":
                    return " " + filterDataField + " <> '" + filterValue + "'";
                case "GREATER_THAN":
                    return " " + filterDataField + " > '" + filterValue + "'";
                case "LESS_THAN":
                    return " " + filterDataField + " < '" + filterValue + "'";
                case "GREATER_THAN_OR_EQUAL":
                    return " " + filterDataField + " >= '" + filterValue + "'";
                case "LESS_THAN_OR_EQUAL":
                    return " " + filterDataField + " <= '" + filterValue + "'";
                case "STARTS_WITH_CASE_SENSITIVE":
                    return " " + filterDataField + " LIKE '" + filterValue + "%'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                case "STARTS_WITH":
                    return " " + filterDataField + " LIKE '" + filterValue + "%'";
                case "ENDS_WITH_CASE_SENSITIVE":
                    return " " + filterDataField + " LIKE '%" + filterValue + "'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                case "ENDS_WITH":
                    return " " + filterDataField + " LIKE '%" + filterValue + "'";
            }
            return "";
        }


        protected int ToInt(HttpRequestBase Request, string field) {
            return Types.ToInt(Request[field]);
        }
        protected bool ToBool(HttpRequestBase Request, string field)
        {
            return Types.ToBool(Request[field],false);
        }

    }
}
