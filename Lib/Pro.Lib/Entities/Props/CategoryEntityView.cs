using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Data;
using Nistec.Data;

namespace Pro.Data.Entities.Props
{
    [EntityMapping("Categories")]
    public class CategoryEntityView : EntityItem<DbPro>, IEntityPro
    {
        #region override

        public override string MappingName()
        {
            return "Categories";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("סווג", "he");
            validator.Required(PropName, "שם סווג");
            if (PropId == 0 && commandType != UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion

        public static IEnumerable<CategoryEntityView> ViewList(int AccountId)
        {
            return EntityPro.ViewEntityList<CategoryEntityView>(EntityGroups.Enums, TableName, AccountId);
        }

        public static CategoryEntityView View(int CategoryId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.EntityGet<CategoryEntityView>(TableName, "CategoryId", CategoryId);
        }

        public const string TableName = "Categories";
        public const string TagPropId = "קוד סווג";
        public const string TagPropName = "שם סווג";
        public const string TagPropTitle = "סווגים";

        #region properties

        [EntityProperty(EntityPropertyType.Identity, Column = "CategoryId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "CategoryName")]
        [Validator("שם סיווג", true)]
        public string PropName { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }

        #endregion

        public T Get<T>(int PropId) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbPro>())
            return db.EntityGet<T>(TableName, "CategoryId", PropId);
        }

    }

}
