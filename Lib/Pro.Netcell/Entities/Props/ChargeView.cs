using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using ProNetcell.Data;
using Nistec.Data;

namespace ProNetcell.Data.Entities.Props
{
    [EntityMapping("Charge")]
    public class ChargeView : EntityItem<DbPro>, IEntityPro
    {
        #region override

        public override string MappingName()
        {
            return "Charge";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("חיוב", "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, "שם חיוב");
            if (PropId == 0 && commandType != UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion

        public static IEnumerable<ChargeView> ViewList(int AccountId)
        {
            return EntityPro.ViewEntityList<ChargeView>(EntityGroups.Enums, TableName, AccountId);
        }

        public static ChargeView View(int ChargeId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<ChargeView>(TableName, "ChargeId", ChargeId);
        }

        public const string TableName = "Charge";
        public const string TagPropId = "קוד חיוב";
        public const string TagPropName = "שם חיוב";
        public const string TagPropTitle = "סוגי חיוב";

        #region properties

        [EntityProperty(EntityPropertyType.Identity, Column = "ChargeId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "ChargeName")]
        [Validator("שם חיוב", true)]
        public string PropName { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }

        #endregion

        public T Get<T>(int PropId) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<T>(TableName, "ChargeId", PropId);
        }

    }

}
