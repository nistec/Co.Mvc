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
    [EntityMapping("Categories", "vw_Categories")]
    public class CategoryView : EntityItem<DbPro>, IEntityPro
    {
        #region override

        public override string MappingName()
        {
            return "Categories";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("סיווג", "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, "שם סיווג");
            if (PropId == 0 && commandType != UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion

        public static IEnumerable<CategoryView> ViewList(int AccountId)
        {
            return EntityLibPro.ViewEntityList<CategoryView>(EntityGroups.Enums, ViewName, AccountId);
        }
        public static void Refresh(int AccountId, bool dbRefresh=false)
        {
            CacheRemove(AccountId);
            if(dbRefresh)
            {
                //ListsTypes.Categories,
                DbProContext<CategoryView>.DbRefresh(AccountId);

                //using(var db=DbContext.Create<DbPro>())
                //{
                //    db.ExecuteNonQuery("sp_Entity_Refresh","Entity",Lists.GetListsType(ListsTypes.Categories), "AccountId", AccountId);
                //}
            }
        }

        public static CategoryView View(int ItemId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<CategoryView>(TableName, "CategoryId", ItemId);
        }

        public const string ViewName = "vw_Categories";
        public const string TableName = "Categories";
        public const string TagPropId = "קוד סיווג";
        public const string TagPropName = "שם סיווג";
        public const string TagPropTitle = "סווגים";

        #region properties


        [EntityProperty(EntityPropertyType.Identity, Column = "CategoryId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "CategoryName")]
        [Validator("שם סיווג",true)]
        public string PropName { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }
        [EntityProperty(EntityPropertyType.Optional)]
        public int MembersCount { get; set; }

        #endregion

        internal static void CacheRemove(int AccountId)
        {
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, AccountId, ViewName));
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, AccountId, TableName));

        }

        public T Get<T>(int PropId) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<T>(ViewName, "CategoryId", PropId);
        }

        public static int DoDelete(int PropId, int AccountId)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                int result = 0;
                result = db.EntityItemDelete(CategoryView.TableName,new object[]{"CategoryId", PropId});
                CacheRemove(AccountId);
                return result;
            }
        }
 
        public static int DoSave(int PropId, string PropName, int AccountId, UpdateCommandType command)
        {
            int result = 0;
            CategoryView newItem = new CategoryView()
            {
                PropId = PropId,
                PropName = PropName,
                AccountId = AccountId
            };
            string TableName = newItem.MappingName();

            switch ((int)command)
            {
                case 0://insert
                    result = newItem.DoInsert<CategoryView>();
                    break;
                case 2://delete
                    result = newItem.DoDelete<CategoryView>();
                    break;
                default:
                    CategoryView current = newItem.Get<CategoryView>(PropId);
                    result = current.DoUpdate(newItem);
                    break;
            }
            //EntityPro.CacheRemove(EntityPro.GetKey(TableName, AccountId));
            //WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, AccountId, ViewName));
            CacheRemove(AccountId);
            return result;
        }

    }
 
}
