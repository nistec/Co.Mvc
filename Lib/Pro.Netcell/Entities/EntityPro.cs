using Nistec;
using Nistec.Channels;
using Nistec.Channels.RemoteCache;
using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Data.Entities.Config;
using Nistec.Generic;
using Nistec.Serialization;
using Nistec.Web.Controls;
using ProNetcell.Data;
using ProNetcell.Data.Entities;
using ProNetcell.Data.Entities.Props;
using Pro.Netcell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace ProNetcell.Data.Entities
{

    public static class EntityGroups
    {
        public const string Enums = "Enums";
        public const string Contacts = "Contacts";
        public const string Settings = "Settings";
        public const string Reports = "Reports";
        public const string Registry = "Registry";
    }

    //public enum EntityGroups2
    //{
    //    Enums,
    //    Reports,
    //    Registry,
    //}
    public class EntityPro
    {

        public static void RefreshList(ListsTypes type, int accountId)
        {

            switch (type)
            {
                //case ListsTypes.Accounts:
                //    AccountsView.Refresh(accountId);
                //    break;
                case ListsTypes.Branch:
                    BranchView.Refresh(accountId);
                    break;
                case ListsTypes.Categories:
                    CategoryView.Refresh(accountId);
                    break;
                //case ListsTypes.Users:
                //    UserView.Refresh(accountId);
                //    break;
            }

        }
        internal static string GetSession(int AccountId)
        {
            return string.Format("{0}_{1}", Settings.ProjectName, AccountId);
        }
        //internal static string GetKey(string TableName, int AccountId)
        //{
        //    return string.Format("{0}_{1}_{2}_{3}", EntityPro.LibName, TableName, AccountId, "Entity");
        //}

        public static string CacheKey(string group, int AccountId, string TableName)
        {
            return WebCache.GetKey(Settings.ProjectName, group, AccountId, TableName);
        }
        public static string CacheGroupKey(string group, int AccountId)
        {
            return WebCache.GetGroupKey(Settings.ProjectName, group, AccountId);
        }
        public static T CacheGet<T>(string key)
        {
            return WebCache.Get<T>(key);
        }

        public static T CacheGetOrCreate<T>(string key, Func<T> function,int expirationMinutes=0)
        {
            return WebCache.GetOrCreate<T>(key, function,expirationMinutes);
        }

        public static void CacheAdd(string key, object value, int expirationMinutes = 0)
        {
            WebCache.Insert(key, value, expirationMinutes);
        }
        //public static void CacheAdd(string key, object value, int expirationMimute)
        //{
        //    WebCache.Insert(key, value, expirationMimute);
        //}
        public static void CacheDelete(string key)
        {
            WebCache.Remove(key);
        }
   

#if (CacheApi)
        public static string CacheGetJson(string key)
        {
            return CacheApi.Get(EntityPro.CacheProtocol).GetJson(key, JsonFormat.Indented);
        }
        public static T CacheGet<T>(string key)
        {
            return CacheApi.Get(EntityPro.CacheProtocol).GetValue<T>(key);
        }
        public static void CacheAdd(string key, string session, object value)
        {
            CacheApi.Get(EntityPro.CacheProtocol).AddItem(key, value, session, EntityPro.CacheTimeout);
        }
        public static void CacheRemove(string key)
        {
            CacheApi.Get(EntityPro.CacheProtocol).RemoveItem(key);
        }
        public static void CacheReset(int AccountId)
        {
            string session = GetSession(AccountId);
            CacheApi.Get(EntityPro.CacheProtocol).RemoveCacheSessionItems(session);
        }
#else
        /*
        public static string CacheGetJson(string key)
        {
            object o = HttpContext.Current.Cache[key];
            return o == null ? null : Nistec.Serialization.JsonSerializer.Serialize(o, true);
            //return CacheApi.Get(EntityPro.CacheProtocol).GetJson(key, JsonFormat.Indented);
        }
        public static T CacheGet<T>(string key)
        {
            object o = HttpContext.Current.Cache[key];
            return GenericTypes.Cast<T>(o);
            //return CacheApi.Get(EntityPro.CacheProtocol).GetValue<T>(key);
        }

        public static void CacheAdd(string key, object value)
        {
            
            HttpContext.Current.Cache.Add(key, value, null, DateTime.Now.AddMinutes(EntityPro.CacheTimeout), TimeSpan.Zero, CacheItemPriority.Default, null);

            //CacheApi.Get(EntityPro.CacheProtocol).AddItem(key, value, session, EntityPro.CacheTimeout);
        }
        public static void CacheRemove(string key)
        {
            HttpContext.Current.Cache.Remove(key);
            //CacheApi.Get(EntityPro.CacheProtocol).RemoveItem(key);
        }
        //public static void CacheReset(int AccountId)
        //{
        //    string session = GetSession(AccountId);
        //    CacheApi.Get(EntityPro.CacheProtocol).RemoveCacheSessionItems(session);
        //}
         */
#endif
        public static bool EnableCache
        {
            get 
            {
                return EntityConfig.Settings.EntityCache.Enable;
            }
        }

        static int _CacheTimeout;
        public static int CacheTimeout
        {
            get
            {

                if (_CacheTimeout == 0)
                {
                    _CacheTimeout = EntityConfig.Settings.EntityCache.Timeout;
                }
                return _CacheTimeout;
            }
        }

        static NetProtocol _CacheProtocol = 0;
        public static NetProtocol CacheProtocol
        {
            get
            {

                if (_CacheProtocol == 0)
                {
                    var protocl = EntityConfig.Settings.EntityCache.Protocol;
                    if (protocl == "tcp")
                        _CacheProtocol = NetProtocol.Tcp;
                    else
                        _CacheProtocol = NetProtocol.Pipe;
                }
                return _CacheProtocol;
            }
        }

        public static IEnumerable<T> ViewEntityList<T>(string GroupName,string TableName, int AccountId) where T : IEntityPro
        {
            string key = WebCache.GetKey(Settings.ProjectName, GroupName, AccountId, TableName);// GetKey(TableName, AccountId);
            IEnumerable<T> list = null;

            if (EntityPro.EnableCache)
                list = (IEnumerable<T>)WebCache.Get<List<T>>(key);
            if (list == null || list.Count()==0)
            {
                using (var db = DbContext.Create<DbPro>())
                {
                    list = db.EntityItemList<T>(TableName, "AccountId", AccountId);
                }
                if (EntityPro.EnableCache && list != null)
                {
                    //CacheAdd(key,GetSession(AccountId), (List<T>)list);
                    WebCache.Insert(key, (List<T>)list);
                }
            }

            return list;
        }

        public static IEnumerable<T> ViewEntityList<T>(string GroupName, string TableName, int AccountId,int EnumType) where T : IEntityProEnum
        {
            string EntiryName = TableName + EnumType.ToString();
            string key = WebCache.GetKey(Settings.ProjectName, GroupName, AccountId, EntiryName);// GetKey(TableName, AccountId);
            IEnumerable<T> list = null;

            if (EntityPro.EnableCache)
                list = (IEnumerable<T>)WebCache.Get<List<T>>(key);
            if (list == null || list.Count() == 0)
            {
                using (var db = DbContext.Create<DbPro>())
                {
                    list = db.EntityItemList<T>(TableName, "AccountId", AccountId, "EnumType", EnumType);
                }
                if (EntityPro.EnableCache && list != null)
                {
                    //CacheAdd(key,GetSession(AccountId), (List<T>)list);
                    WebCache.Insert(key, (List<T>)list);
                }
            }

            return list;
        }

        public static int DoSave<T>(int PropId, string PropName, int AccountId, UpdateCommandType command) where T : IEntityPro
        {
            int result = 0;
            T newItem = Nistec.Runtime.ActivatorUtil.CreateInstance<T>();
            if (command != UpdateCommandType.Insert)
                newItem.PropId = PropId;
            newItem.PropName = PropName;
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
                    T current = newItem.Get<T>(PropId);
                    result = current.DoUpdate(newItem);
                    break;
            }


            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, AccountId, TableName));// CacheRemove(GetKey(TableName, AccountId));

            return result;
        }

        public static int DoSave<T>(int PropId, string PropName, int AccountId, int EnumType, UpdateCommandType command) where T : IEntityProEnum
        {
            int result = 0;
            T newItem = Nistec.Runtime.ActivatorUtil.CreateInstance<T>();
            if (command != UpdateCommandType.Insert)
                newItem.PropId = PropId;
            newItem.PropName = PropName;
            newItem.AccountId = AccountId;
            newItem.EnumType = EnumType;
            string EntiryName = newItem.MappingName() + EnumType.ToString();

            switch ((int)command)
            {
                case 0://insert
                    result = newItem.DoInsert<T>();
                    break;
                case 2://delete
                    result = newItem.DoDelete<T>();
                    break;
                default:
                    T current = newItem.Get<T>(PropId);
                    result = current.DoUpdate(newItem);
                    break;
            }

            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, AccountId, EntiryName));// CacheRemove(GetKey(TableName, AccountId));

            return result;
        }
        public static int DoDelete(string TableName, string PropType, int PropId, int Replacement, int AccountId)
        {
            var parameters = DataParameter.GetSqlList("PropType", "campaign", "PropId", PropId, "Replacement", 0, "AccountId", AccountId);
            DataParameter.AddOutputParameter(parameters, "Result", System.Data.SqlDbType.Int, 4);
            using (var db = DbContext.Create<DbPro>())
            {
                db.ExecuteCommandNonQuery("sp_Property_Remove", parameters.ToArray(), System.Data.CommandType.StoredProcedure);
            }
            int result = Types.ToInt(parameters[4].Value);
            if (result > 0)
            {
                WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, AccountId, TableName));
            }
            return result;
        }

        public static int DoDelete(string PropType, int PropId, int Replacement, int AccountId)
        {

            var parameters = DataParameter.GetSqlList("PropType", PropType, "PropId", PropId, "Replacement", Replacement, "AccountId", AccountId);
            DataParameter.AddOutputParameter(parameters, "Result", System.Data.SqlDbType.Int, 4);
            using (var db = DbContext.Create<DbPro>())
            {
                db.ExecuteCommandNonQuery("sp_Property_Remove", parameters.ToArray(), System.Data.CommandType.StoredProcedure);
            }
            int result = Types.ToInt( parameters[4].Value);

            //CacheRemove(GetKey(GetTableName(PropType), AccountId));
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, AccountId, GetTableName(PropType)));
            return result;

        }

        public static string GetTableName(string entity)
        {
            switch (entity)
            {
                case "branch":
                    return BranchView.TableName;
                case "charge":
                    return ChargeView.TableName;
                case "city":
                    return CityView.TableName;
                //case "place":
                //    return PlaceView.TableName;
                case "region":
                    return RegionView.TableName;
                case "category":
                    return CategoryView.TableName;
                case "status":
                    return StatusView.TableName;
                case "role":
                    return RoleView.TableName;
                case "exenum1":
                    return EnumView.TableName + "1";
                case "exenum2":
                    return EnumView.TableName + "2";
                case "exenum3":
                    return EnumView.TableName + "3";
                default:
                    return "";
            }

        }
    }
}
