using Nistec;
using Nistec.Data.Entities;
using Nistec.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pro.Query
{

    public enum PermisLevel
    {
        none = 0,
        read = 1,
        write2,
        delete = 4,
        export = 8
    }

    public class QueryModel : QueryBase
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public int TotalRows { get; private set; }
        public string Layout { get; set; }
        public string Url { get; set; }

        //v-e-r-x-m
        //view-edit-remove-export-management

        //r-w-d-x
        //read-writ-delete-export
        string _Rules = "r-w-d-x-";
        public string Rules { get { return _Rules; } }
        public void SetRules(int UserRole)
        {
            bool AllowRead = UserRole > 0;
            bool AllowEdit = UserRole >= 1;
            bool AllowDelete = UserRole >= 5;
            bool AllowExport = UserRole >= 5;

            _Rules = string.Format("{0}{1}{2}{3}",AllowRead ? "r-" : "-", AllowEdit ? "w-" : "-", AllowDelete ? "d-" : "-", AllowExport ? "x-" : "-");
        }

        //public bool AllowRead { get; set; }
        //public bool AllowEdit { get; set; }
        //public bool AllowDelete { get; set; }
        //public bool AllowExport { get; set; }
     

        public void SetTotalRows<T>(IList<T> list) where T : IEntityListItem
        {
            var row = list.FirstOrDefault<T>();
            TotalRows = row == null ? 0 : row.TotalRows;
        }

        NameValueArgs _Args;
        public NameValueArgs Args
        {
            get
            {
                if (_Args == null)
                    _Args = new NameValueArgs();
                return _Args;
            }
        }

        public object[] CreateParameters(System.Web.HttpRequestBase Request, bool enableSortAndFilter, object[] keyValueParameters)
        {
            if (enableSortAndFilter)
            {
                LoadSortAndFilter(Request,"");

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
                var qf = GetSortAndFilter(Request);

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
            var model = new QueryModel();
            model.PageSize = Types.ToInt(Request["pagesize"], 20);
            model.PageNum = Types.ToInt(Request["pagenum"]);
            model.AccountId = Types.ToInt(Request["AccountId"]);
            model.UserId = Types.ToInt(Request["UserId"]);

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
}
