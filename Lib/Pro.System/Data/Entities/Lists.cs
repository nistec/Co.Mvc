using Nistec.Data.Entities;
using Nistec.Web.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProSystem.Data.Entities
{
    public enum ListsTypes
    {

        Accounts=1,
        Users=2,
        Categories=3,
        Branch=4,
        Cities=5,
        Design=6
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
            return context.GetList();

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
    public class Lists
    {

        public static string GetList(ListsTypes type)
        {
            using (var db = DbContext.Create<DbSystem>())
            return db.ExecuteJson("sp_GetLists", "ListType", (int)type);
        }

        public static IList<T> GetList<T>(ListsTypes type)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteList<T>("sp_GetLists", "ListType", (int)type);
        }

    }
}
