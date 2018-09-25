using Nistec;
using Nistec.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Pro.Query
{
  
    public abstract class QueryBase
    {
        public int PageSize { get; set; }
        public int PageNum { get; set; }
        public string Sort { get; set; }
        public string Filter { get; set; }

        protected void ValidateFilter(string filter)
        {
            if (Regx.RegexValidateIgnoreCase("[;:=<>^{}\\[\\]]", filter))
                throw new ArgumentException("filter not allowed: " +filter);
            if (Regx.RegexValidateIgnoreCase("(0x|exe|update|insert|delete|drop|grant|\\sor\\s|\\sand\\s)", filter))
                throw new ArgumentException("filter not allowed: " + filter);
        }
 
        protected void LoadSortAndFilter(System.Web.HttpRequestBase Request, string tablePrefix)
        {
            PageSize = Types.ToInt(Request["pagesize"], 20);
            PageNum = Types.ToInt(Request["pagenum"]);

            string sortfield = Request["sortdatafield"];
            string sortorder = Request["sortorder"] ?? "asc";

            string filter = null;
            int filterscount = Types.ToInt(Request["filterscount"]);
            if (filterscount > 0)
            {
                filter = BuildFilters(filterscount, Request.Params, tablePrefix);
                if (!string.IsNullOrEmpty(filter))
                {
                    ValidateFilter(filter);

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


        protected int ToInt(HttpRequestBase Request, string field)
        {
            return Types.ToInt(Request[field]);
        }
        protected bool ToBool(HttpRequestBase Request, string field)
        {
            return Types.ToBool(Request[field], false);
        }

    }
    /*
    public abstract class QueryBase
    {
        public int PageSize { get; set; }
        public int PageNum { get; set; }
         public string Sort { get; set; }
        public string Filter { get; set; }

        protected void LoadSortAndFilter(System.Web.HttpRequestBase Request, string tablePrefix)
        {
            PageSize = Types.ToInt(Request["pagesize"],20);
            PageNum = Types.ToInt(Request["pagenum"]);

           string sortfield = Request["sortdatafield"];
            string sortorder = Request["sortorder"] ?? "asc";

            string filter = null;
            int filterscount = Types.ToInt(Request["filterscount"]);
            if (filterscount > 0)
            {
                filter = BuildFilters(filterscount, Request.Params, tablePrefix);// "m.");
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
        protected string BuildQuery(System.Collections.Specialized.NameValueCollection query, string prefix)
        {
            string queryString = ""
            + "  SELECT *, ROW_NUMBER() OVER (ORDER BY " + query.GetValues("sortdatafield")[0] + " "
            + query.GetValues("sortorder")[0].ToUpper() + ") as row FROM Customers "
            + " ";
            var filtersCount = int.Parse(query.GetValues("filterscount")[0]);
            var where = "";
            if (filtersCount > 0)
            {
                where += " WHERE (" + this.BuildFilters(filtersCount, query, prefix);
            }
            queryString += where;
            return queryString;
        }
        protected string BuildFilters(int filtersCount, System.Collections.Specialized.NameValueCollection query, string prefix)
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
                where += this.GetFilterCondition(filterCondition, filterDataField, filterValue);
                if (i == filtersCount - 1)
                {
                    where += ")";
                }
                tmpFilterOperator = filterOperator;
                tmpDataField = filterDataField;
            }
            return where;
        }
        protected string GetFilterCondition(string filterCondition, string filterDataField, string filterValue)
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

    }
    */
}
