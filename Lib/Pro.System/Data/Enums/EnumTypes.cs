using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using Nistec.Channels.RemoteCache;
using Nistec.Web.Controls;
using ProSystem.Data;
using ProSystem.Data.Entities;

namespace ProSystem.Data.Enums
{
     [EntityMapping("Enums_Types")]
    public class EnumTypes : EntityItem<DbSystem>, IEntityPro
    {
        #region override

        public override string MappingName()
        {
            return "Enums_Types";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("סוג", "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, "סוג");
            if (PropId == 0 && commandType != UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion


        public static IList<EnumTypes> ViewList(int AccountId, string TaskMode)
        {
            string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId,0, ViewName, TaskMode);
            return WebCache.GetOrCreateList<EnumTypes>(key, () => ViewDbList(AccountId, TaskMode), EntityProCache.DefaultCacheTtl);
        }

        public static IList<EnumTypes> ViewDbList(int AccountId, string TypeModel)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.Query<EnumTypes>("select * from Enums_Types where (AccountId=@AccountId or AccountId=0) and (TypeModel=@TypeModel or TypeModel='A')", "AccountId", AccountId, "TypeModel", TypeModel);

        }
        //public static IEnumerable<EnumTypes> ViewItemList(int AccountId)
        //{
        //    return EntityProCache.ViewEntityList<EnumTypes>(EntityCacheGroups.Enums, ViewName, AccountId);
        //}

        public static EnumTypes View(int ItemId)
        {
            using (var db = DbContext.Create<DbSystem>())
            return db.EntityItemGet<EnumTypes>(TableName, "TypeId", ItemId);
        }

        public const string ViewName = "Enums_Types";
        public const string TableName = "Enums_Types";
        public const string TagPropId = "קוד סוג";
        public const string TagPropName = "סוג";
        public const string TagPropTitle = "סוג";

        #region properties

        [EntityProperty(EntityPropertyType.Identity, Column = "TypeId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "TypeName")]
        [Validator("סוג",true)]
        public string PropName { get; set; }

        [EntityProperty(EntityPropertyType.Key,Column = "AccountId")]
        public int AccountId { get; set; }

        [EntityProperty(Column = "TypeModel")]
        [Validator("מודל", true)]
        public string TyprModel { get; set; }


        [EntityProperty(EntityPropertyType.Optional)]
        public int MembersCount { get; set; }

        #endregion

        public T Get<T>(int PropId) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.EntityItemGet<T>(ViewName, "TypeId", PropId);
        }

        public static int DoDelete(int PropId, int AccountId)
        {
            using (var db = DbContext.Create<DbSystem>())
            {
                int result = 0;
                result = db.EntityItemDelete(EnumTypes.TableName, new object[] { "TypeId", PropId });
                WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId, TableName));
                return result;
            }
        }
 
        public static int DoSave(int PropId, string PropName, int AccountId, string TyprModel, UpdateCommandType command)
        {
            int result = 0;
            EnumTypes newItem = new EnumTypes()
            {
                PropId = PropId,
                PropName = PropName,
                AccountId = AccountId,
                TyprModel = TyprModel
            };
            string TableName = newItem.MappingName();

            switch ((int)command)
            {
                case 0://insert
                    result = newItem.DoInsert<EnumTypes>();
                    break;
                case 2://delete
                    result = newItem.DoDelete<EnumTypes>();
                    break;
                default:
                    EnumTypes current = newItem.Get<EnumTypes>(PropId);
                    result = current.DoUpdate(newItem);
                    break;
            }
            //EntityPro.CacheRemove(EntityPro.GetKey(TableName, AccountId));
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId,0, TableName, TyprModel));
            return result;
        }

    }
}
