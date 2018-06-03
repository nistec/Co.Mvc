using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using Nistec.Channels.RemoteCache;

namespace ProSystem.Data.Entities
{
    public abstract class EntityEnumView : EntityItem<DbSystem>, IEntityEnum
    {
        public const string TableName = "Enums";
 
        #region properties

        [EntityProperty(EntityPropertyType.Key, Column = "PropId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "PropName")]
         public string PropName { get; set; }

        [EntityProperty(EntityPropertyType.Key, Column = "PropType")]
        public string PropType { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }

        #endregion

        #region override

        public override string MappingName()
        {
            return "Enums";
        }

        //public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        //{
        //    EntityValidator validator = new EntityValidator("סניף", "he");
        //    validator.Required(PropName, "שם סטאטוס");
        //    if (PropId == 0)
        //    {
        //        validator.Append("רשומה זו אינה ניתנת לעריכה");
        //    }
        //    return validator;
        //}
        #endregion

        public T Get<T>(int PropId, string PropType, int AccountId) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbSystem>())
            return db.EntityItemGet<T>(TableName, "PropId", PropId, "PropType", PropType, "AccountId", AccountId);
        }

    }
    public class StatusView : EntityEnumView
    {
        #region override
        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("סטאטוס", "he");
            validator.Required(PropName, "שם סטאטוס");
            if (PropId == 0)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion

        public static IEnumerable<StatusView> ViewList(int AccountId)
        {
            return EntityEnum.ViewEntityList<StatusView>(TableName, EnumType,AccountId);
        }

        public static StatusView View(int PropId,int AccountId)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.EntityItemGet<StatusView>(TableName, PropId, EnumType, AccountId);
        }

        public const string EnumType = "Status";
        public const string TagPropId = "קוד סטאטוס";
        public const string TagPropName = "שם סטאטוס";
        public const string TagPropTitle = "סטאטוס";

    }
    /*
    public class CategoryView : EntityEnumView
    {
        #region override
        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("קטגוריה", "he");
            validator.Required(PropName, "שם קטגוריה");
            if (PropId == 0)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion

        public static IEnumerable<CategoryView> ViewList(int AccountId)
        {
            return EntityEnum.ViewEntityList<CategoryView>(TableName, EnumType,AccountId);
        }

        public static CategoryView View(int PropId, int AccountId)
        {
            return db.QueryEntity<CategoryView>(TableName, "PropId", PropId, "EnumType", EnumType, "AccountId", AccountId);
        }

        public const string EnumType = "Category";
        public const string TagPropId = "קוד קטגוריה";
        public const string TagPropName = "שם קטגוריה";
        public const string TagPropTitle = "קטגוריה";

    }
    */
    /*
    public class RegionView : EntityEnumView
    {
        #region override
        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("מחוז", "he");
            validator.Required(PropName, "שם מחוז");
            if (PropId == 0)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion

        public static IEnumerable<RegionView> ViewList(int AccountId)
        {
            return EntityEnum.ViewEntityList<RegionView>(TableName, EnumType,AccountId);
        }

        public static RegionView View(int PropId, int AccountId)
        {
            return db.QueryEntity<RegionView>(TableName, "PropId", PropId, "EnumType", EnumType, "AccountId", AccountId);
        }

        public const string EnumType = "Category";
        public const string TagPropId = "קוד מחוז";
        public const string TagPropName = "שם מחוז";
        public const string TagPropTitle = "מחוז";

    }
    */
    public class RoleView : EntityEnumView
    {
        #region override
        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("תפקיד", "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, "שם תפקיד");
            if (PropId == 0 && commandType != UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion

        public static IEnumerable<RoleView> ViewList(int AccountId)
        {
            return EntityEnum.ViewEntityList<RoleView>(TableName, EnumType, AccountId);
        }

        public static RoleView View(int PropId, int AccountId)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.EntityItemGet<RoleView>(TableName, PropId, EnumType, AccountId);
        }

        public const string EnumType = "Role";
        public const string TagPropId = "קוד תפקיד";
        public const string TagPropName = "שם תפקיד";
        public const string TagPropTitle = "תפקיד";

    }
}
