using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using Nistec.Channels.RemoteCache;
using Nistec.Web.Controls;
using ProSystem.Data;

namespace ProSystem.Data.Entities
{
     [EntityMapping("Task_Types")]
    public class TaskTypeEntity : EntityItem<DbSystem>, IEntityPro
    {
        #region override

        public override string MappingName()
        {
            return "Task_Types";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("סוג", "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, "סוג משימה");
            if (PropId == 0 && commandType != UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion


        public static IList<TaskTypeEntity> ViewList(int AccountId)
        {
            string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId, ViewName);
            return WebCache.GetOrCreateList<TaskTypeEntity>(key, () => ViewDbList(AccountId), EntityProCache.DefaultCacheTtl);
        }

        public static IList<TaskTypeEntity> ViewDbList(int AccountId)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.Query<TaskTypeEntity>("select * from vw_Task_Types where AccountId=@AccountId or AccountId=0", "AccountId", AccountId);

        }
        //public static IEnumerable<TaskTypeEntity> ViewItemList(int AccountId)
        //{
        //    return EntityProCache.ViewEntityList<TaskTypeEntity>(EntityCacheGroups.Enums, ViewName, AccountId);
        //}

        public static TaskTypeEntity View(int ItemId)
        {
            using (var db = DbContext.Create<DbSystem>())
            return db.EntityItemGet<TaskTypeEntity>(TableName, "TaskTypeId", ItemId);
        }

        public const string ViewName = "vw_Task_Types";
        public const string TableName = "Task_Types";
        public const string TagPropId = "קוד סוג";
        public const string TagPropName = "סוג משימה";
        public const string TagPropTitle = "סוג משימה";

        #region properties
      

        [EntityProperty(EntityPropertyType.Identity, Column = "TaskTypeId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "TaskTypeName")]
        [Validator("סוג משימה",true)]
        public string PropName { get; set; }

        [EntityProperty(EntityPropertyType.Key,Column = "AccountId")]
        public int AccountId { get; set; }
        [EntityProperty(EntityPropertyType.Optional)]
        public int MembersCount { get; set; }

        #endregion

        public T Get<T>(int PropId) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.EntityItemGet<T>(ViewName, "TaskTypeId", PropId);
        }

        public static int DoDelete(int PropId, int AccountId)
        {
            using (var db = DbContext.Create<DbSystem>())
            {
                int result = 0;
                result = db.EntityItemDelete(TaskTypeEntity.TableName, new object[] { "TaskTypeId", PropId });
                WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId, TableName));
                return result;
            }
        }
 
        public static int DoSave(int PropId, string PropName, int AccountId, UpdateCommandType command)
        {
            int result = 0;
            TaskTypeEntity newItem = new TaskTypeEntity()
            {
                PropId = PropId,
                PropName = PropName,
                AccountId = AccountId
            };
            string TableName = newItem.MappingName();

            switch ((int)command)
            {
                case 0://insert
                    result = newItem.DoInsert<TaskTypeEntity>();
                    break;
                case 2://delete
                    result = newItem.DoDelete<TaskTypeEntity>();
                    break;
                default:
                    TaskTypeEntity current = newItem.Get<TaskTypeEntity>(PropId);
                    result = current.DoUpdate(newItem);
                    break;
            }
            //EntityPro.CacheRemove(EntityPro.GetKey(TableName, AccountId));
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId, TableName));
            return result;
        }

    }
 
}
