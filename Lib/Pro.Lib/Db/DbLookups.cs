using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Web.Controls;
using Pro.Data.Entities;
using Pro.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pro.Data
{
    public class DbLookups
    {

        public static IEnumerable<EntityListItem<int>> Autocomplete(int accountId, string type, string serach)
        {
            var list = DisplayListCache(accountId, type);
            if (list == null || list.Count == 0)
                return null;
            return list.Where(p => p.Label.StartsWith(serach));
        }

        public static IList<EntityListItem<int>> DisplayListCache(int accountId, string type)
        {
            string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Members, accountId, 0, "DisplayList_" + type);
            return WebCache.GetOrCreateList(key, () => DisplayList(accountId, type), Settings.DefaultShortTTL );
        }

        public static IList<EntityListItem<int>> DisplayList(int accountId, string type)
        {
            switch(type)
            {
                case "member_display":
                    return EntityListContext<DbPro, int>.GetList("RecordId", "DisplayName", "vw_Member_Display", "AccountId", accountId);
                default:
                    return null;
            }
        }


        //public static IList<EntityListItem<int>> Member_DisplayList(int accountId)
        //{
        //    return EntityListContext<DbPro, int>.GetList("RecordId", "DisplayName", "vw_Member_Display", "AccountId", accountId);
        //}

        public static string Member_Display(string field,params object[] keyvalueParameters)
        {
            return DbContext.Lookup<DbPro>(field, "vw_Member_Display", null, keyvalueParameters);
        }

       
    }
}
