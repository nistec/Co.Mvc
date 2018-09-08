using Nistec.Data.Entities;
using Nistec.Web.Controls;
using ProNetcell.Data;
using ProNetcell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProNetcell.Data.Entities
{
    public enum ListsTypes
    {

        //Accounts=1,
        //Users=2,
        Categories=3,
        Branch=4,
        Cities=5,
        Design=6
    }

    public class Lists
    {
        public static string GetListsType(ListsTypes entity)
        {
            switch (entity)
            {
                //case ListsTypes.Users:
                //    return "user";
                //case ListsTypes.Accounts:
                //    return "account";
                case ListsTypes.Branch:
                    return "branch";
                case ListsTypes.Cities:
                    return "city";
                case ListsTypes.Categories:
                    return "category";
                //case "place":
                //    return ListsTypes.Branch;
                //case "charge":
                //    return ListsTypes.Branch;
                //case "status":
                //    return ListsTypes.Branch;
                //case "role":
                //    return ListsTypes.Branch;
                //case "exenum1":
                //    return ListsTypes.Branch;
                //case "exenum2":
                //    return ListsTypes.Branch;
                //case "exenum3":
                //    return ListsTypes.Branch;
                default:
                    return null;
            }

        }

        public static ListsTypes GetListsType(string entity)
        {
            switch (entity)
            {
                //case "user":
                //    return ListsTypes.Users;
                //case "account":
                //    return ListsTypes.Accounts;
                case "branch":
                    return ListsTypes.Branch;
                case "city":
                    return ListsTypes.Cities;
                case "category":
                    return ListsTypes.Categories;
                //case "region":
                //    return ListsTypes.Branch;
                //case "place":
                //    return ListsTypes.Branch;
                //case "charge":
                //    return ListsTypes.Branch;
                //case "status":
                //    return ListsTypes.Branch;
                //case "role":
                //    return ListsTypes.Branch;
                //case "exenum1":
                //    return ListsTypes.Branch;
                //case "exenum2":
                //    return ListsTypes.Branch;
                //case "exenum3":
                //    return ListsTypes.Branch;
                default:
                    return (ListsTypes)0;
            }

        }

        public static string GetList(ListsTypes type)
        {
            using (var db = DbContext.Create<DbNetcell>())
            return db.ExecuteJson("sp_GetLists", "ListType", (int)type);
        }

        public static IList<T> GetList<T>(ListsTypes type)
        {
            using (var db = DbContext.Create<DbNetcell>())
                return db.ExecuteList<T>("sp_GetLists", "ListType", (int)type);
        }

 
    }
}
