using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Web.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSystem.Data.Entities
{
   
    [EntityMapping("Accounts_Category")]
    public class AccountsCategoryView : EntityItem<DbSystem>, IEntityPro
    {
        #region override

        public override string MappingName()
        {
            return "Accounts_Category";
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

        public static IEnumerable<AccountsCategoryView> ViewList()
        {
            return EntityPro.ViewAdminEntityList<AccountsCategoryView>(EntityGroups.Admin, ViewName);
        }
        public static void Refresh(bool dbRefresh = false)
        {
            CacheRemove();
            if (dbRefresh)
            {
                DbSystemContext<AccountsCategoryView>.Refresh(EntityGroups.Admin, 0);

                //using(var db=DbContext.Create<DbPro>())
                //{
                //    db.ExecuteNonQuery("sp_Entity_Refresh","Entity",Lists.GetListsType(ListsTypes.Categories), "AccountId", AccountId);
                //}
            }
        }

        public static AccountsCategoryView View(int ItemId)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.EntityItemGet<AccountsCategoryView>(TableName, "AccountCategoryId", ItemId);
        }

        public const string ViewName = "Accounts_Category";
        public const string TableName = "Accounts_Category";
        public const string TagPropId = "קוד סיווג";
        public const string TagPropName = "שם סיווג";
        public const string TagPropTitle = "סווגים";

        #region properties


        [EntityProperty(EntityPropertyType.Identity, Column = "AccountCategoryId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "AccountCategoryName")]
        [Validator("שם סיווג", true)]
        public string PropName { get; set; }

        [EntityProperty(EntityPropertyType.NA)]
        public int AccountId { get; set; }

        //[EntityProperty(EntityPropertyType.Optional)]
        //public int MembersCount { get; set; }

        #endregion

        internal static void CacheRemove()
        {
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Admin, 0, ViewName));
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Admin, 0, TableName));

        }

        public T Get<T>(int PropId) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.EntityItemGet<T>(ViewName, "CategoryId", PropId);
        }

        public static int DoDelete(int PropId)
        {
            using (var db = DbContext.Create<DbSystem>())
            {
                int result = 0;
                result = db.EntityItemDelete(AccountsCategoryView.TableName, new object[] { "AccountCategoryId", PropId });
                CacheRemove();
                return result;
            }
        }

        public static int DoSave(int PropId, string PropName, int AccountId, UpdateCommandType command)
        {
            int result = 0;
            AccountsCategoryView newItem = new AccountsCategoryView()
            {
                PropId = PropId,
                PropName = PropName,
                AccountId = AccountId
            };
            string TableName = newItem.MappingName();

            switch ((int)command)
            {
                case 0://insert
                    result = newItem.DoInsert<AccountsCategoryView>();
                    break;
                case 2://delete
                    result = newItem.DoDelete<AccountsCategoryView>();
                    break;
                default:
                    AccountsCategoryView current = newItem.Get<AccountsCategoryView>(PropId);
                    result = current.DoUpdate(newItem);
                    break;
            }
            //EntityPro.CacheRemove(EntityPro.GetKey(TableName, AccountId));
            //WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, AccountId, ViewName));
            CacheRemove();
            return result;
        }

    }

}
