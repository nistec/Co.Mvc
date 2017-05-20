using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Data;
using Nistec.Data;

namespace Pro.Data.Entities.Props
{
    [EntityMapping("Roles")]
    public class RoleView : EntityItem<DbPro>, IEntityPro
    {
        #region override

        public override string MappingName()
        {
             return "Roles";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("תפקיד", "he");
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
            return EntityPro.ViewEntityList<RoleView>(EntityGroups.Enums, TableName, AccountId);
        }

        public static RoleView View(int RegionId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<RoleView>(TableName, "RegionId", RegionId);
        }
                

        public const string TableName = "Roles";
        public const string TagPropId = "קוד תפקיד";
        public const string TagPropName = "שם תפקיד";
        public const string TagPropTitle = "תפקידים";

        #region properties

        [EntityProperty(EntityPropertyType.Identity, Column = "RoleId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "RoleName")]
        [Validator("שם תפקיד",true)]
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
