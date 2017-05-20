using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Data;
using Nistec.Data;

namespace Pro.Data.Entities.Props
{
    [EntityMapping("Status")]
    public class StatusView : EntityItem<DbPro>, IEntityPro
    {
        #region override

        public override string MappingName()
        {
             return "Status";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("סטאטוס", "he");
            validator.Required(PropName, "שם סטאטוס");
            if (PropId == 0 && commandType != UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion

        public static IEnumerable<StatusView> View()
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<StatusView>(TableName, null);
        }

        public static StatusView View(int StatusId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<StatusView>(TableName, "StatusId", StatusId);
        }

        public const string TableName = "Status";
        public const string TagPropId = "קוד סטאטוס";
        public const string TagPropName = "שם סטאטוס";
        public const string TagPropTitle = "סטאטוס";

        #region properties

        [EntityProperty(EntityPropertyType.Identity, Column = "StatusId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "StatusName")]
        [Validator("שם סטאטוס", true)]
        public string PropName { get; set; }
        public int AccountId { get; set; }
        #endregion

        public T Get<T>(int PropId) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<T>(TableName, "PropId", PropId);
        }

    }
 
}
