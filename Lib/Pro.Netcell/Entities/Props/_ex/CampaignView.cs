using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using ProNetcell.Data;
using Nistec.Data;
using Nistec.Channels.RemoteCache;
using Nistec.Web.Controls;
using Nistec;
using Pro.Netcell;

namespace ProNetcell.Data.Entities.Props
{
    [EntityMapping("Campaigns")]
    public class CampaignView : EntityItem<DbPro>, IEntityPro
    {
        #region override

        public override string MappingName()
        {
            return "Campaigns";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("קמפיין", "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, "שם קמפיין");
            if (PropId == 0 && commandType != UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion

        public static IEnumerable<CampaignView> ViewList(int AccountId)
        {
            return EntityPro.ViewEntityList<CampaignView>(EntityGroups.Enums, TableName, AccountId);
        }

        public static CampaignView View(int CampaignId)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<CampaignView>(TableName, "CampaignId", CampaignId);
        }

        public const string TableName = "Campaigns";
        public const string TagPropId = "קוד קמפיין";
        public const string TagPropName = "שם קמפיין";
        public const string TagPropTitle = "קמפיין";

        #region properties


        [EntityProperty(EntityPropertyType.Identity, Column = "CampaignId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "CampaignName")]
        [Validator("שם קמפיין", true)]
        public string PropName { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }
        public bool IsActive { get; set; }
        public int ValidityMonth { get; set; }
        public int ValidDiff { get; set; }
        public int DefaultCategory { get; set; }
        public bool IsSignupCredit { get; set; }
        public DateTime? ValidityDate { get; set; }

        #endregion

        public T Get<T>(int PropId) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<T>(TableName, "CampaignId", PropId);
        }
        //public static int DoDelete(int PropId, int AccountId)
        //{
        //    var parameters = DataParameter.GetSqlList("PropType", "campaign", "PropId", PropId, "Replacement", 0, "AccountId", AccountId);
        //    DataParameter.AddOutputParameter(parameters, "Result", System.Data.SqlDbType.Int, 4);
        //    db.ExecuteNonQuery("sp_Property_Remove", parameters.ToArray(), System.Data.CommandType.StoredProcedure);
        //    int result = Types.ToInt(parameters[4].Value);
        //    if(result>0)
        //    {
        //        WebCache.Remove(WebCache.GetKey(EntityPro.LibName, EntityGroups.Enums, AccountId, TableName));
        //    }
        //    return result;
        //}
        public static int DoDelete(int PropId, int AccountId)
        {
            return EntityPro.DoDelete(TableName, "campaign", PropId,0, AccountId);
            //int result = 0;
            //result = db.EntityDelete(CampaignView.TableName, "CampaignId", PropId);
            //WebCache.Remove(WebCache.GetKey(EntityPro.LibName, EntityGroups.Enums, AccountId, TableName));
            //return result;
        }
        public static int DoSave(int PropId, string PropName, bool IsActive, int ValidityMonth, int ValidDiff, DateTime? ValidityDate, int DefaultCategory, bool IsSignupCredit, int AccountId, UpdateCommandType command)
        {
            if (command == UpdateCommandType.Delete)
            {
                throw new ArgumentException("Delete command not supported in this method");
            }

            int result = 0;

            CampaignView newItem = new CampaignView()
            {
                PropId = PropId,
                PropName = PropName,
                IsActive = IsActive,
                ValidityMonth = ValidityMonth,
                ValidDiff = ValidDiff,
                DefaultCategory = DefaultCategory,
                IsSignupCredit=IsSignupCredit,
                AccountId = AccountId,
                ValidityDate=ValidityDate
            };
            string TableName = newItem.MappingName();

            var args = new object[]{
                "AccountId",AccountId,
                "CampaignId",PropId,
                "CampaignName",PropName,
                "IsActive",IsActive,
                "ValidityMonth",ValidityMonth,
                "ValidDiff",ValidDiff,
                "DefaultCategory",DefaultCategory,
                "IsSignupCredit",IsSignupCredit,
                "ValidityDate",Settings.NullableToValue(ValidityDate)
            };

            using (var db = DbContext.Create<DbPro>())
            {
                result = db.ExecuteNonQuery("sp_Campaign_Save", args);

                //switch ((int)command)
                //{
                //    case 0://insert
                //        result = newItem.DoInsert<CampaignView>();
                //        break;
                //    case 2://delete
                //        result = newItem.DoDelete<CampaignView>();
                //        break;
                //    default:
                //        CampaignView current = newItem.Get<CampaignView>(PropId);
                //        result = current.DoUpdate(newItem);
                //        break;
                //}

                //EntityPro.CacheRemove(EntityPro.GetKey(TableName, AccountId));
                WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, AccountId, TableName));
                return result;
            }
        }

    }
}
