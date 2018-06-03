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


        public static IList<TaskTypeEntity> ViewList(int AccountId, string TaskMode)
        {
            string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId,0, ViewName, TaskMode);
            return WebCache.GetOrCreateList<TaskTypeEntity>(key, () => ViewDbList(AccountId, TaskMode), EntityProCache.DefaultCacheTtl);
        }

        public static IList<TaskTypeEntity> ViewDbList(int AccountId, string TaskModel)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.Query<TaskTypeEntity>("select * from vw_Task_Types where (AccountId=@AccountId or AccountId=0) and (TaskModel=@TaskModel or TaskModel='A')", "AccountId", AccountId, "TaskModel", TaskModel);

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


        public const string TagPropNameTopic = "סוג סוגיה";
        public const string TagPropNameTicket = "סוג כרטיס";
        public const string TagPropNameDoc = "סוג מסמך";

        #region properties


        [EntityProperty(EntityPropertyType.Identity, Column = "TaskTypeId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "TaskTypeName")]
        [Validator("סוג משימה",true)]
        public string PropName { get; set; }

        [EntityProperty(EntityPropertyType.Key,Column = "AccountId")]
        public int AccountId { get; set; }

        [EntityProperty(Column = "TaskModel")]
        [Validator("מודל", true)]
        public string TaskModel { get; set; }


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
 
        public static int DoSave(int PropId, string PropName, int AccountId, string TaskModel, UpdateCommandType command)
        {
            int result = 0;
            TaskTypeEntity newItem = new TaskTypeEntity()
            {
                PropId = PropId,
                PropName = PropName,
                AccountId = AccountId,
                TaskModel=TaskModel
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
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId,0, TableName,TaskModel));
            return result;
        }

    }

    /*
    [EntityMapping("Tags")]
    public class TagsEntity : EntityItem<DbSystem>, IEntityPro
    {
        #region override

        public override string MappingName()
        {
            return "Tags";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("סוג", "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, "תגית");
            if (PropId == 0 && commandType != UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion


        public static IList<TagsEntity> ViewList(int AccountId, string TaskMode)
        {
            string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId, 0, ViewName, TaskMode);
            return WebCache.GetOrCreateList<TagsEntity>(key, () => ViewDbList(AccountId, TaskMode), EntityProCache.DefaultCacheTtl);
        }

        public static IList<TagsEntity> ViewDbList(int AccountId, string TaskModel)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.Query<TagsEntity>("select * from Tags where AccountId=@AccountId", "AccountId", AccountId);

        }
       
        public static TagsEntity View(string tag)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.EntityItemGet<TagsEntity>(TableName, "Tag", tag);
        }

        public const string ViewName = "Tags";
        public const string TableName = "Tags";
        public const string TagPropId = "תגית";
        public const string TagPropName = "תגית";
        public const string TagPropTitle = "תגית";

        #region properties


        //[EntityProperty(EntityPropertyType.Identity, Column = "TaskTypeId")]
        //public int PropId { get; set; }

        [EntityProperty(Column = "Tag")]
        [Validator("תגית", true)]
        public string PropName { get; set; }

        [EntityProperty(EntityPropertyType.Key, Column = "AccountId")]
        public int AccountId { get; set; }

        [EntityProperty(Column = "SourceType")]
        [Validator("מודל", true)]
        public string SourceType { get; set; }

        #endregion

        public T Get<T>(string PropName) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.EntityItemGet<T>(ViewName, "Tag", PropName);
        }

        public static int DoDelete(string PropName, int AccountId)
        {
            using (var db = DbContext.Create<DbSystem>())
            {
                int result = 0;
                result = db.EntityItemDelete(TagsEntity.TableName, new object[] { "Tag", PropName });
                WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId, TableName));
                return result;
            }
        }

        public static int DoSave(string PropName, int AccountId, string SourceType, UpdateCommandType command)
        {
            int result = 0;
            TagsEntity newItem = new TagsEntity()
            {
                //PropId = PropId,
                PropName = PropName,
                AccountId = AccountId,
                SourceType = SourceType
            };
            string TableName = newItem.MappingName();

            switch ((int)command)
            {
                case 0://insert
                    result = newItem.DoInsert<TagsEntity>();
                    break;
                case 2://delete
                    result = newItem.DoDelete<TagsEntity>();
                    break;
                default:
                    TagsEntity current = newItem.Get<TagsEntity>(PropName);
                    result = current.DoUpdate(newItem);
                    break;
            }
            //EntityPro.CacheRemove(EntityPro.GetKey(TableName, AccountId));
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId, 0, TableName, SourceType));
            return result;
        }

    }

    */
}
