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
    [EntityMapping("PriceList")]
    public class PriceView : EntityItem<DbPro>, IEntityPro
    {
        #region override

        public override string MappingName()
        {
            return "PriceList";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("מחירון", "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, "שם מחירון");
            if (PropId == 0 && commandType != UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion
       
        public static IEnumerable<PriceView> ViewList(int AccountId)
        {
            return EntityLibPro.ViewEntityList<PriceView>(EntityGroups.Enums, TableName, AccountId);
        }

        public static PriceView View(int ItemId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<PriceView>(TableName, "ItemId", ItemId);
        }

        public const string TableName = "PriceList";
        public const string TagPropId = "קוד מחיר";
        public const string TagPropName = "שם פריט";
        public const string TagPropTitle = "מחירון";

        #region properties
	

        [EntityProperty(EntityPropertyType.Identity, Column = "ItemId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "ItemName")]
        [Validator("שם פריט",true)]
        public string PropName { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }
        [EntityProperty]
        public int Quota { get; set; }
        [EntityProperty]
        public decimal Price { get; set; }

        #endregion

        public T Get<T>(int PropId) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<T>(TableName, "ItemId", PropId);
        }

        public static int DoDelete(int PropId, int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                int result = 0;
                result = db.EntityItemDelete(PriceView.TableName, new object[] { "ItemId", PropId });
                WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, AccountId, TableName));
                return result;
            }
        }
 
        public static int DoSave(int PropId, string PropName, int Quota, decimal Price, int AccountId, UpdateCommandType command)
        {
            int result = 0;
            PriceView newItem = new PriceView()
            {
                PropId = PropId,
                PropName = PropName,
                Quota = Quota,
                Price=Price,
                AccountId = AccountId
            };
            string TableName = newItem.MappingName();

            switch ((int)command)
            {
                case 0://insert
                    result = newItem.DoInsert<PriceView>();
                    break;
                case 2://delete
                    result = newItem.DoDelete<PriceView>();
                    break;
                default:
                    PriceView current = newItem.Get<PriceView>(PropId);
                    result = current.DoUpdate(newItem);
                    break;
            }
            //EntityPro.CacheRemove(EntityPro.GetKey(TableName, AccountId));
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, AccountId, TableName));
            return result;
        }

    }
 
}
