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
    [EntityMapping("web_UserProfile")]
    public class UserView : EntityItem<DbPro>, IEntityPro
    {
        #region override

        public override string MappingName()
        {
            return "web_UserProfile";
        }

        public override EntityValidator Validate(UpdateCommandType commandType = UpdateCommandType.Update)
        {
            EntityValidator validator = new EntityValidator("משתמש", "he");
            if (commandType != UpdateCommandType.Delete)
                validator.Required(PropName, "שם משתמש");
            if (PropId == 0 && commandType != UpdateCommandType.Insert)
            {
                validator.Append("רשומה זו אינה ניתנת לעריכה");
            }
            return validator;
        }
        #endregion
       
        //public static IEnumerable<UserView> ViewList(int AccountId)
        //{
        //    return EntityPro.ViewEntityList<UserView>(EntityGroups.Enums, TableName, AccountId);
        //}
        public static void Refresh(int AccountId)
        {
            WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, AccountId, TableName));
        }

        public static UserView View(int UserId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.EntityItemGet<UserView>(TableName, "UserId", UserId);
        }

        public const string TableName = "web_UserProfile";
        public const string TagPropId = "קוד משתמש";
        public const string TagPropName = "שם משתמש";
        public const string TagPropTitle = "משתמש";

        #region properties

        [EntityProperty(EntityPropertyType.Identity, Column = "UserId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "UserName")]
        [Validator("שם משתמש", true)]
        public string PropName { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int AccountId { get; set; }

      
        public string DisplayName
        {
            get;
            set;
        }
        public DateTime Creation
        {
            get;
            set;
        }
        public string Phone
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }

        public int UserRole
        {
            get;
            set;
        }
     
        public string Lang
        {
            get;
            set;
        }
        public int Evaluation
        {
            get;
            set;
        }
        public bool IsBlocked
        {
            get;
            set;
        }
        #endregion

        public T Get<T>(int PropId) where T : IEntityItem
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemGet<T>(TableName, "UserId", PropId);
        }

        //public static int DoDelete(int PropId, int AccountId)
        //{
        //    int result = 0;
        //    result = db.EntityDelete(UserView.TableName, "UserId", PropId);
        //    WebCache.Remove(WebCache.GetKey(EntityPro.LibName, EntityGroups.Enums, AccountId, TableName));
        //    return result;
        //}
 
        public static int DoSave(UserView u)//, UpdateCommandType command)
        {
            int result = 0;
           
           string TableName = u.MappingName();
           UserView current = u.Get<UserView>(u.PropId);
           result = current.DoUpdate(u);

           WebCache.Remove(WebCache.GetKey(Settings.ProjectName, EntityGroups.Enums, u.AccountId, TableName));
            return result;
        }

    }
 
}
