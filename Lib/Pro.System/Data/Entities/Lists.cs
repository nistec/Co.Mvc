using Nistec.Data.Entities;
using Nistec.Web.Controls;
using Pro;
using Pro.Data;
using Pro.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProSystem.Data.Entities
{
    public enum SystemListsTypes
    {

        Ad_Account = 1,
        Ad_UserProfile = 2,
        Ad_Team = 3,
        Task_Types = 4,
        Ticket_Types = 5,
        Topic_Types = 6,
        Doc_Types = 7,
        Tags = 8,
        Folders = 9,
        Admin_Ad_Team=10,
        Labels=11
    }

    [EntityMapping(ProcListView = "sp_GetListsSelect")]
    public class SelectItem : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Key)]
        public int id { get; set; }
        public string text { get; set; }
    }

    public class Lists<T> where T : IEntityItem
    {

        public static IList<T> Get_List(int AccountId,int UserId, object[] keyValueParameters, int ttl)
        {
            string key = DbContextCache.GetKey<T>(Settings.ProjectName, EntityCacheGroups.Enums, AccountId, UserId);
            return DbContextCache.EntityList<DbSystem, T>(key, ttl, keyValueParameters);
        }
        public static IList<T> Get_List(int UserId, object[] keyValueParameters, string excludeKeys, int ttl) 
        {
            string key = DbContextCache.GetKey<T>(Settings.ProjectName, EntityCacheGroups.Enums, UserId, keyValueParameters, excludeKeys);
            return DbContextCache.EntityList<DbSystem, T>(key, ttl, keyValueParameters);
        }
        public static IList<T> Get_List()
        {
            var context = new DbSystemContext<T>(EntityCacheGroups.Enums,0,0);
            return context.ExecOrViewList();

            //string key = EntityProCache.CacheKey(EntityCacheGroups.Enums, 0, tableName);
            //return EntityProCache.CacheGetOrCreate(key, () => Get_List_Internal(tableName));
        }

        //public static IList<T> Get_List(string tableName) 
        //{
        //    string key = EntityProCache.CacheKey(EntityCacheGroups.Enums, 0, tableName);
        //    return EntityProCache.CacheGetOrCreate(key, () => Get_List_Internal(tableName));
        //}
        //public static IList<T> Get_List_Internal(string tableName)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //        return db.EntityItemList<T>(tableName);
        //}

    }
    public class SystemLists
    {

        public static string GetList(SystemListsTypes type, int AccountId)
        {
            using (var db = DbContext.Create<DbSystem>())
            return db.ExecuteJson("sp_GetLists", "ListType", (int)type, "AccountId", AccountId);
        }

        public static IList<T> GetList<T>(SystemListsTypes type, int AccountId)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteList<T>("sp_GetLists", "ListType", (int)type, "AccountId", AccountId);
        }

        public static IList<SelectItem> ExecListSelect(int AccountId, SystemListsTypes type, bool enableCache = true)
        {
            if (enableCache)
            {
                string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId, 0, type.ToString());
                return WebCache.GetOrCreateList<SelectItem>(key, () =>
                {
                    var context = new DbSystemContext<SelectItem>(EntityCacheGroups.Enums, AccountId, 0);
                    return context.ExecList("AccountId", AccountId, "ListType", (int)type);
                }, EntityPro.DefaultCacheTtl);
            }
            else
            {
                var context = new DbSystemContext<SelectItem>(EntityCacheGroups.Enums, AccountId, 0);
                return context.ExecList("AccountId", AccountId, "ListType", (int)type);
            }
        }

        public static string ExecListTags(int AccountId, SystemListsTypes type, bool enableCache = true)
        {
            if (enableCache)
            {
                string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId, 0, type.ToString());
                return WebCache.GetOrCreateJson(key, () =>
                {
                    var context = DbContext.Get<DbSystem>();
                    return context.ExecuteJsonArray("sp_GetListsTags", "AccountId", AccountId, "ListType", (int)type);
                }, EntityPro.DefaultCacheTtl);
            }
            else
            {
                var context = DbContext.Get<DbSystem>();
                return context.ExecuteJsonArray("sp_GetListsTags", "AccountId", AccountId, "ListType", (int)type);
            }
        }

    }
}
