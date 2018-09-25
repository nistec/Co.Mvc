using Nistec.Data; 
using Nistec.Data.Entities;
using Nistec.Web.Controls;
using Pro;
using Pro.Data;
using Pro.Data.Entities;
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
            string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Task.ToString(), accountId, 0, "DisplayList_" + type);
            return WebCache.GetOrCreateList(key, () => DisplayList(accountId, type),Settings.DefaultShortTTL);
        }

        public static IList<EntityListItem<int>> DisplayList(int accountId, string type)
        {
            switch (type)
            {
                case "tags":
                    return EntityListContext<DbSystem, int>.GetList("Tag", "Tag", "Tags", "AccountId", accountId);
                case "lu_Project":
                case "project_name":
                    return EntityListContext<DbSystem, int>.GetList("ProjectId", "ProjectName", "Project", "AccountId", accountId);
                default:
                    return EntityListContext<DbSystem, int>.GetList("Value", "Label", type, "AccountId", accountId);
            }
        }

        public static IList<EntityListItem<int>> DisplayListCache(string cacheGroups, string valueField, string displayField, string mappingName, int accountId)
        {
            string key = WebCache.GetKey(Settings.ProjectName, cacheGroups, accountId, 0, "DisplayList_" + mappingName);
            return WebCache.GetOrCreateList(key, () => DisplayList(valueField, displayField, mappingName, accountId), Settings.DefaultShortTTL);
        }
        public static IList<EntityListItem<int>> DisplayList(string valueField, string displayField, string mappingName, int accountId)
        {
            return EntityListContext<DbSystem, int>.GetList(valueField, displayField, mappingName, "AccountId", accountId);
        }
   
        public static IEnumerable<EntityListItem<int>> ViewEntityList(string cacheGroups, string valueField, string displayField, string mappingName, int accountId)
        {
            string key = WebCache.GetKey(Settings.ProjectName, cacheGroups, accountId, mappingName);
            IEnumerable<EntityListItem<int>> list = null;

            if (EntityPro.EnableCache)
                list = (IEnumerable<EntityListItem<int>>)WebCache.Get<List<EntityListItem<int>>>(key);
            if (list == null || list.Count() == 0)
            {
                list = EntityListContext<DbSystem, int>.GetList(valueField, displayField, mappingName, "AccountId", accountId);
                if (EntityPro.EnableCache && list != null)
                {
                    //CacheAdd(key,GetSession(AccountId), (List<T>)list);
                    WebCache.Insert(key, (List<EntityListItem<int>>)list);
                }
            }

            return list;
        }

        public static IEnumerable<EntityListItem<int>> ViewEnumList(string cacheGroups, string valueField, string displayField, string mappingName, int accountId, int enumType)
        {
            string key = WebCache.GetKey(Settings.ProjectName, cacheGroups, accountId, mappingName + "_" + enumType.ToString());
            IEnumerable<EntityListItem<int>> list = null;

            if (EntityPro.EnableCache)
                list = (IEnumerable<EntityListItem<int>>)WebCache.Get<List<EntityListItem<int>>>(key);
            if (list == null || list.Count() == 0)
            {
                list = EntityListContext<DbSystem, int>.GetList(valueField, displayField, mappingName, "AccountId", accountId);
                if (EntityPro.EnableCache && list != null)
                {
                    //CacheAdd(key,GetSession(AccountId), (List<T>)list);
                    WebCache.Insert(key, (List<EntityListItem<int>>)list);
                }
            }

            return list;
        }

        public static IEnumerable<EntityListItem<int>> ViewEnumList(int accountId, int enumType)
        {
            string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, accountId, "Enum_" + enumType.ToString());
            IEnumerable<EntityListItem<int>> list = null;

            if (EntityPro.EnableCache)
                list = (IEnumerable<EntityListItem<int>>)WebCache.Get<List<EntityListItem<int>>>(key);
            if (list == null || list.Count() == 0)
            {
                list = EntityListContext<DbSystem, int>.GetList("PropId", "PropName", "Enum", "AccountId", accountId, "EnumType", enumType);
                if (EntityPro.EnableCache && list != null)
                {
                    //CacheAdd(key,GetSession(AccountId), (List<T>)list);
                    WebCache.Insert(key, (List<EntityListItem<int>>)list);
                }
            }

            return list;
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
