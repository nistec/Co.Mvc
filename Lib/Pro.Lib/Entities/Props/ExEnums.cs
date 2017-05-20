using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Data;
using Nistec.Data;

namespace Pro.Data.Entities.Props
{

    public class EnumView : EntityItem<DbPro>, IEntityProEnum
    {
        #region entity settings

        public const string TableName = "Enum";

        internal string TagPropId = "קוד";
        internal string TagPropName = "שם";
        internal string TagPropTitle = "";

        void SetEntity(int AccountId)
        {
            if (TagPropTitle == "")
            {
                var fields = MembersFieldsContext.GetMembersFields(AccountId);
                TagPropTitle = fields.ExEnum1;
                TagPropName = "שם " + TagPropTitle;
                TagPropId = "קוד " + TagPropTitle;
            }
        }

        #endregion

        #region override


        public override string MappingName()
        {
            return "Enum";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            SetEntity(AccountId);

            EntityValidator validator = new EntityValidator(TagPropTitle, "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, TagPropName);
            if (PropId == 0 && commandType != UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion

        #region static
        public static IEnumerable<EnumView> ViewList(int AccountId)
        {
            return EntityPro.ViewEntityList<EnumView>(EntityGroups.Enums, TableName, AccountId);
        }

        public static EnumView View(int PropId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<EnumView>(TableName, "PropId", PropId);
        }
        #endregion

        #region properties

        [EntityProperty(EntityPropertyType.Identity)]
        public int PropId { get; set; }

        [Validator("תאור", true)]
        public string PropName { get; set; }
        [Validator("מספר חשבון", true)]
        public int AccountId { get; set; }
        public int EnumType { get; set; }
        #endregion

        public T Get<T>(int PropId) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<T>(TableName, "PropId", PropId);
        }

    }
#if(false)
    //====================================================== Enum 1

    public class Enum1View : EntityItem<DbPro>, IEntityPro
    {
        #region entity settings

        public const string TableName = "Enum1";

        internal string TagPropId = "קוד";
        internal string TagPropName = "שם";
        internal string TagPropTitle = "";

        void SetEntity(int AccountId)
        {
            if (TagPropTitle == "")
            {
                var fields = MembersFieldsContext.GetMembersFields(AccountId);
                TagPropTitle = fields.ExEnum1;
                TagPropName = "שם " + TagPropTitle;
                TagPropId = "קוד " + TagPropTitle;
            }
        }

        #endregion

        #region override


        public override string MappingName()
        {
            return "Enum1";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            SetEntity(AccountId);

            EntityValidator validator = new EntityValidator(TagPropTitle, "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, TagPropName);
            if (PropId == 0 && commandType != UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion

        #region static
        public static IEnumerable<Enum1View> ViewList(int AccountId)
        {
            return EntityPro.ViewEntityList<Enum1View>(EntityGroups.Enums, TableName, AccountId);
        }

        public static Enum1View View(int PropId)
        {
            return db.EntityItemGet<Enum1View>(TableName, "PropId", PropId);
        }
        #endregion

        #region properties

        [EntityProperty(EntityPropertyType.Identity)]
        public int PropId { get; set; }

        [Validator("תאור", true)]
        public string PropName { get; set; }
        [Validator("מספר חשבון", true)]
        public int AccountId { get; set; }

        #endregion

        public T Get<T>(int PropId) where T : IEntityItem
        {
            return db.EntityItemGet<T>(TableName, "PropId", PropId);
        }

    }

    //====================================================== Enum 2

    public class Enum2View : EntityItem<DbPro>, IEntityPro
    {
        #region entity settings

        public const string TableName = "Enum2";

        internal string TagPropId = "קוד";
        internal string TagPropName = "שם";
        internal string TagPropTitle = "";

        void SetEntity(int AccountId)
        {
            if (TagPropTitle == "")
            {
                var fields = MembersFieldsContext.GetMembersFields(AccountId);
                TagPropTitle = fields.ExEnum2;
                TagPropName = "שם " + TagPropTitle;
                TagPropId = "קוד " + TagPropTitle;
            }
        }

        #endregion

        #region override


        public override string MappingName()
        {
            return "Enum2";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            SetEntity(AccountId);

            EntityValidator validator = new EntityValidator(TagPropTitle, "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, TagPropName);
            if (PropId == 0 && commandType != UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion

        #region static
        public static IEnumerable<Enum2View> ViewList(int AccountId)
        {
            return EntityPro.ViewEntityList<Enum2View>(EntityGroups.Enums, TableName, AccountId);
        }

        public static Enum2View View(int PropId)
        {
            return db.EntityItemGet<Enum2View>(TableName, "PropId", PropId);
        }
        #endregion

        #region properties

        [EntityProperty(EntityPropertyType.Identity)]
        public int PropId { get; set; }

        [Validator("תאור", true)]
        public string PropName { get; set; }
        [Validator("מספר חשבון", true)]
        public int AccountId { get; set; }

        #endregion

        public T Get<T>(int PropId) where T : IEntityItem
        {
            return db.EntityItemGet<T>(TableName, "PropId", PropId);
        }

    }

    //====================================================== Enum 3

    public class Enum3View : EntityItem<DbPro>, IEntityPro
    {
        #region entity settings

        public const string TableName = "Enum3";

        internal string TagPropId = "קוד";
        internal string TagPropName = "שם";
        internal string TagPropTitle = "";

        void SetEntity(int AccountId)
        {
            if (TagPropTitle == "")
            {
                var fields = MembersFieldsContext.GetMembersFields(AccountId);
                TagPropTitle = fields.ExEnum3;
                TagPropName = "שם " + TagPropTitle;
                TagPropId = "קוד " + TagPropTitle;
            }
        }

        #endregion

        #region override


        public override string MappingName()
        {
            return "Enum3";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            SetEntity(AccountId);

            EntityValidator validator = new EntityValidator(TagPropTitle, "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, TagPropName);
            if (PropId == 0 && commandType != UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion

        #region static
        public static IEnumerable<Enum3View> ViewList(int AccountId)
        {
            return EntityPro.ViewEntityList<Enum3View>(EntityGroups.Enums, TableName, AccountId);
        }

        public static Enum3View View(int PropId)
        {
            return db.EntityItemGet<Enum3View>(TableName, "PropId", PropId);
        }
        #endregion

        #region properties

        [EntityProperty(EntityPropertyType.Identity)]
        public int PropId { get; set; }

        [Validator("תאור", true)]
        public string PropName { get; set; }
        [Validator("מספר חשבון", true)]
        public int AccountId { get; set; }

        #endregion

        public T Get<T>(int PropId) where T : IEntityItem
        {
            return db.EntityItemGet<T>(TableName, "PropId", PropId);
        }

    }

#endif

}
