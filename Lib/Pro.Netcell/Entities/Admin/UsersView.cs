using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Web.Security;

namespace Pro.Data.Entities
{
     [EntityMapping("web_UserProfile")]
    public class UsersView : IEntityItem
    {
        const string TableName = "web_UserProfile";

        public static UserProfileView VirtualUserSet(int accountId, int userId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.ExecuteSingle<UserProfileView>("websp_UserVirtual","Mode","set", "AccountId", accountId, "UserId", userId);
        }
        public static UserProfileView VirtualUserCancel(int accountId, int userId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.ExecuteSingle<UserProfileView>("websp_UserVirtual", "Mode", "cancel", "AccountId", accountId, "UserId", userId);
        }

        public static IEnumerable<UserProfileView> GetUsersView(int accountId, int userId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.ExecuteList<UserProfileView>("websp_GetUsers", "AccountId", accountId, "UserId", userId);
        }

        public static IEnumerable<UserRoles> GetUsersRoleView(int accountId, int userId)
        {
            using (var db = DbContext.Create<DbPro>())
            return db.ExecuteList<UserRoles>("websp_GetUsersRole", "AccountId", accountId, "UserId", userId);
        }
        

        //public static IEnumerable<UsersView> GetUsersView()
        //{
        //    return db.EntityGetList<UsersView>(TableName, null);
        //}


        //public static UsersView GetUsersView(int userId)
        //{
        //    return db.EntityItemGet<UsersView>(TableName, userId);
        //}

        public static string LookupUserName(int userId)
        {
            if (userId <= 0)
                return "";
            using (var db = DbContext.Create<DbPro>())
            return db.QueryScalar<string>("select UserName from " + TableName + " where UserId=@UserId", "", "UserId", userId);
        }

        #region properties

        [EntityProperty(EntityPropertyType.Identity)]
        public int UserId
        {
            get;
            set;
        }
        public string UserName
        {
            get;
            set;
        }
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
            public int AccountId
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
      
    }
 
}
