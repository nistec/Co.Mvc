using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Data;
using Nistec.Data;

namespace Pro.Data.Entities.Props
{
    [EntityMapping("PlaceOfBirth")]
    public class PlaceView : EntityItem<DbPro>, IEntityPro
    {
        #region override

        public override string MappingName()
        {
            return "PlaceOfBirth";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("ארץ מוצא", "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, "שם ארץ מוצא");
            if (PropId == 0 && commandType!= UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion

        public static IEnumerable<PlaceView> ViewList(int AccountId)
        {
            return EntityPro.ViewEntityList<PlaceView>(EntityGroups.Enums, TableName, AccountId);
        }

        public static PlaceView View(int PlaceId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<PlaceView>(TableName, "PlaceId", PlaceId);
        }

        public const string TableName = "PlaceOfBirth";
        public const string TagPropId = "קוד ארץ";
        public const string TagPropName = "שם ארץ";
        public const string TagPropTitle = "ארץ מוצא";

        #region properties

        [EntityProperty(EntityPropertyType.Identity, Column = "PlaceId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "PlaceName")]
        [Validator("שם ארץ מוצא", true)]
        public string PropName { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }

        #endregion

        public T Get<T>(int PropId) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<T>(TableName, "PlaceId", PropId);
        }

    }
 
}
