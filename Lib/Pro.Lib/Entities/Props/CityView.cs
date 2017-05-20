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
    public class CityRegionView : CityView
    {
        public static IEnumerable<CityRegionView> ViewCityRegion(int accountId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<CityRegionView>("vw_CityRegion", "AccountId", accountId);
        }

        [EntityProperty]
        public string RegionName { get; set; }
    }

    [EntityMapping("Cities")]
    public class CityView : EntityItem<DbPro>, IEntityPro
    {
        #region override

        public override string MappingName()
        {
            return "Cities";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("יישוב", "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, "שם יישוב");
            if (PropId == 0 && commandType != UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion
       
        public static IEnumerable<CityView> ViewList(int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.Query<CityView>("select * from " + TableName + " where AccountId=0 or AccountId=@AccountId", "AccountId",AccountId);
            //return EntityPro.ViewEntityList<CityView>(EntityGroups.Enums, TableName, AccountId);
        }

        public static CityView View(int CityId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<CityView>(TableName, "CityId", CityId);
        }

        public const string TableName = "Cities";
        public const string TagPropId = "קוד יישוב";
        public const string TagPropName = "שם יישוב";
        public const string TagPropTitle = "יישובים";

        #region properties

        [EntityProperty(EntityPropertyType.Identity, Column = "CityId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "CityName")]
        [Validator("שם יישוב",true)]
        public string PropName { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }
        [EntityProperty]
        public int RegionId { get; set; }

        #endregion

        public T Get<T>(int PropId) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<T>(TableName, "CityId", PropId);
        }

        public static int DoDelete(int PropId, int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                int result = 0;
                result = db.EntityItemDelete(CityView.TableName, new object[] { "CityId", PropId });
                WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, AccountId, TableName));
                return result;
            }
        }

        public static int DoSave(int PropId, string PropName, int RegionId, int AccountId, UpdateCommandType command)
        {
            int result = 0;
            CityView newItem = new CityView()
            {
                PropId = PropId,
                PropName = PropName,
                RegionId = RegionId,
                AccountId = AccountId
            };
            string TableName = newItem.MappingName();

            switch ((int)command)
            {
                case 0://insert
                    result = newItem.DoInsert<CityView>();
                    break;
                case 2://delete
                    result = newItem.DoDelete<CityView>();
                    break;
                default:
                    CityView current = newItem.Get<CityView>(PropId);
                    result = current.DoUpdate(newItem);
                    break;
            }
            //EntityPro.CacheRemove(EntityPro.GetKey(TableName, AccountId));
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, AccountId, TableName));
            return result;
        }

    }
 
}
