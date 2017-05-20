﻿using Nistec.Channels;
using Nistec.Channels.RemoteCache;
using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Data.Entities.Config;
using Nistec.Generic;
using Nistec.Web.Controls;
using ProSystem.Data.Entities;
using ProSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProSystem.Data.Entities
{
    public class EntityEnum
    {


        static string GetKey(string TableName,string PropType,int AccountId)
        {
            return string.Format("{0}_{1}_{2}_{3}_{4}", Settings.ProjectName, TableName, PropType, AccountId, "Entity");
        }

        public static IEnumerable<T> ViewEntityList<T>(string TableName, string PropType, int AccountId) where T : IEntityEnum
        {
            string key = GetKey(TableName, PropType,AccountId);
            IEnumerable<T> list = null;

            if (EntityProCache.EnableCache)
                list = (IEnumerable<T>)WebCache.Get<List<T>>(key);// EntityProCache.CacheGet<List<T>>(key);
            if (list == null)
            {
                using (var db = DbContext.Create<DbSystem>())
                {
                    list = db.EntityItemList<T>(TableName, "PropType", PropType, "AccountId", AccountId);
                    if (EntityProCache.EnableCache && list != null)
                    {
                        //EntityProCache.CacheAdd(key,EntityProCache.GetSession(AccountId), (List<T>)list);
                        WebCache.Insert(key, (List<T>)list);
                    }
                }
            }

            return list;
        }

        public static int DoSaveProc(int PropId, string PropName, string PropType, int AccountId, UpdateCommandType command)
        {
            string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId, PropType);// GetKey("Enums", PropType, AccountId);
            WebCache.Remove(key);// EntityProCache.CacheRemove(key);
            using (var db = DbContext.Create<DbSystem>())
            {
                return db.ExecuteNonQuery("sp_Enums_Update", "Op", (int)command, "AccountId", AccountId, "PropType", PropType, "PropId", PropId, "PropName", PropName);
            }
        }


        public static int DoSave<T>(int PropId, string PropName, string PropType, int AccountId, UpdateCommandType command) where T : IEntityEnum
        {
            int result = 0;
            T newItem = Nistec.Runtime.ActivatorUtil.CreateInstance<T>();
            newItem.PropId = PropId;
            newItem.PropName = PropName;
            newItem.PropType = PropType;
            newItem.AccountId = AccountId;

            string TableName = newItem.MappingName();

            switch ((int)command)
            {
                case 0://insert
                    result = newItem.DoInsert<T>();
                    break;
                case 2://delete
                    result = newItem.DoDelete<T>();
                    break;
                default:
                    T current = newItem.Get<T>(PropId, PropType,AccountId);
                    result = current.DoUpdate(newItem);
                    break;
            }
            string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId, PropType);// GetKey(TableName, PropType, AccountId);
            WebCache.Remove(key);// EntityProCache.CacheRemove(key);

            return result;
        }

    }
}
