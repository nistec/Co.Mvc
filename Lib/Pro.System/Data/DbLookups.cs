using Nistec.Data; 
using Nistec.Data.Entities;
using Nistec.Web.Controls;
using ProSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSystem.Data
{
    public class SystemLookups
    {
        public static IEnumerable<EntityListItem<int>> Autocomplete(int accountId, string type, string serach)
        {
           var list= DisplayListCache(accountId, type);
           if (list == null || list.Count == 0)
               return null;
           return list.Where(p => p.Label.StartsWith(serach));
        }

        public static IList<EntityListItem<int>> DisplayListCache(int accountId, string type)
        {
            string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Task, accountId, 0, "DisplayList_" + type);
            return WebCache.GetOrCreateList(key, () => DisplayList(accountId, type),Settings.DefaultShortTTL);
        }
        public static IList<EntityListItem<int>> DisplayList(int accountId, string type)
        {
            switch (type)
            {
                case "tags":
                    return EntityListContext<DbSystem, int>.GetList("Tag", "Tag", "Tags", "AccountId", accountId);
                case "project_name":
                    return EntityListContext<DbSystem, int>.GetList("ProjectId", "ProjectName", "Project", "AccountId", accountId);
                default:
                    return null;
            }
        }

        public static string Project(string field, params object[] keyvalueParameters)
        {
            return DbContext.Lookup<DbSystem>(field, "Project", null, keyvalueParameters);
        }
        public static string Tags(string field, params object[] keyvalueParameters)
        {
            return DbContext.Lookup<DbSystem>(field, "Tags", null, keyvalueParameters);
        }

        //public static string GetProjectName(params object[] keyvalueParameters)
        //{
        //    return DbContext.Lookup<DbSystem>("ProjectName", "Project", null, keyvalueParameters);
        //}


    }
}
