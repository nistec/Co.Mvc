using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Data;
using Nistec.Data;

namespace Pro.Data.Entities.Props
{
    [EntityMapping("Region")]
    public class RegionView : EntityItem<DbPro>, IEntityPro
    {
        #region override

        public override string MappingName()
        {
            return "Region";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("אזור", "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, "שם אזור");
            if (PropId == 0 && commandType!= UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion

        public static IEnumerable<RegionView> ViewList(int AccountId)
        {
            return EntityLibPro.ViewEntityList<RegionView>(EntityGroups.Enums, TableName, 0);
        }

        public static RegionView View(int RegionId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<RegionView>(TableName, "RegionId", RegionId);
        }

        public const string TableName = "Region";
        public const string TagPropId = "קוד אזור";
        public const string TagPropName = "שם אזור";
        public const string TagPropTitle = "אזורים";

        #region properties

        [EntityProperty(EntityPropertyType.Identity, Column = "RegionId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "RegionName")]
        [Validator("שם אזור", true)]
        public string PropName { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }

        #endregion

        public T Get<T>(int PropId) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<T>(TableName, "RegionId", PropId);
        }

    }

}
