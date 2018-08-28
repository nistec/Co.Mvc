using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Web.Controls;
using ProNetcell.Data.Entities;
using Pro.Netcell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProNetcell.Data
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
            string key = WebCache.GetKey(Settings.ProjectName, EntityGroups.Contacts, accountId, 0, "DisplayList_" + type);
            return WebCache.GetOrCreateList(key, () => DisplayList(accountId, type), Settings.DefaultShortTTL );
        }

        public static IList<EntityListItem<int>> DisplayList(int accountId, string type)
        {
            switch(type)
            {
                case "member_display":
                    return EntityListContext<DbPro, int>.GetList("RecordId", "DisplayName", "vw_Member_Display", "AccountId", accountId);
                default:
                    return EntityListContext<DbPro, int>.GetList("Value", "Label", type, "AccountId", accountId);

            }
        }

        public static IList<EntityListItem<int>> DisplayListCache(string cacheGroups, string valueField, string displayField, string mappingName, int accountId)
        {
            string key = WebCache.GetKey(Settings.ProjectName, cacheGroups, accountId, 0, "DisplayList_" + mappingName);
            return WebCache.GetOrCreateList(key, () => DisplayList(valueField, displayField, mappingName,accountId), Settings.DefaultShortTTL);
        }
        public static IList<EntityListItem<int>> DisplayList(string valueField, string displayField, string mappingName, int accountId)
        {
            return EntityListContext<DbPro, int>.GetList(valueField, displayField, mappingName, "AccountId", accountId);
        }


        public static IEnumerable<EntityListItem<int>> ViewEntityList(string cacheGroups, string valueField, string displayField, string mappingName, int accountId) 
        {
            string key = WebCache.GetKey(Settings.ProjectName, cacheGroups, accountId, mappingName);
            IEnumerable<EntityListItem<int>> list = null;

            if (EntityPro.EnableCache)
                list = (IEnumerable<EntityListItem<int>>)WebCache.Get<List<EntityListItem<int>>>(key);
            if (list == null || list.Count() == 0)
            {
                list = EntityListContext<DbPro, int>.GetList(valueField, displayField, mappingName, "AccountId", accountId);
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
                list = EntityListContext<DbPro, int>.GetList(valueField, displayField, mappingName, "AccountId", accountId);
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
            string key = WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, accountId, "Enum_"+enumType.ToString());
            IEnumerable<EntityListItem<int>> list = null;

            if (EntityPro.EnableCache)
                list = (IEnumerable<EntityListItem<int>>)WebCache.Get<List<EntityListItem<int>>>(key);
            if (list == null || list.Count() == 0)
            {
                list = EntityListContext<DbPro, int>.GetList("PropId", "PropName", "Enum", "AccountId", accountId, "EnumType",enumType);
                if (EntityPro.EnableCache && list != null)
                {
                    //CacheAdd(key,GetSession(AccountId), (List<T>)list);
                    WebCache.Insert(key, (List<EntityListItem<int>>)list);
                }
            }

            return list;
        }

        //public static IList<EntityListItem<int>> Member_DisplayList(int accountId)
        //{
        //    return EntityListContext<DbPro, int>.GetList("RecordId", "DisplayName", "vw_Member_Display", "AccountId", accountId);
        //}

        public static string Member_Display(string field,params object[] keyvalueParameters)
        {
            return DbContext.Lookup<DbPro>(field, "vw_Member_Display", null, keyvalueParameters);
        }
        public static string UserProfile(string field, params object[] keyvalueParameters)
        {
            return DbContext.Lookup<DbPro>(field, "Ad_UserProfile", null, keyvalueParameters);
        }

    }
}
