using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Data;
using Nistec.Data;
using Nistec.Channels.RemoteCache;
using Nistec.Web.Controls;
using Pro.Lib;

namespace Pro.Data.Entities.Props
{
    [EntityMapping("Branch")]
    public class BranchView : EntityItem<DbPro>, IEntityPro
    {
        #region override

        public override string MappingName()
        {
            return "Branch";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("סניף", "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, "שם סניף");
            if (commandType!= UpdateCommandType.Insert && PropId == 0)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion

        public static void Refresh(int AccountId)
        {
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, AccountId, TableName));
        }
        public static IEnumerable<BranchView> ViewList(int AccountId)
        {
            return EntityPro.ViewEntityList<BranchView>(EntityGroups.Enums, TableName, AccountId);
        }

        public static BranchView View(int PropId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<BranchView>(TableName, "PropId", PropId);
        }


        public const string TableName = "Branch";
        public const string TagPropId = "קוד סניף";
        public const string TagPropName = "שם סניף";
        public const string TagPropTitle = "סניפים";


        #region properties

        [EntityProperty(EntityPropertyType.Identity, Column = "BranchId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "BranchName")]
        [Validator("שם סניף", true)]
        public string PropName { get; set; }

        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }

        #endregion

        public T Get<T>(int PropId) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<T>(TableName, "BranchId", PropId);
        }

    }

}
