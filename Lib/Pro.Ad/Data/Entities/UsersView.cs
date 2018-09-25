using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Web.Security;
using Nistec.Web.Controls;
using Nistec.Data;
using ProSystem.Data;
using ProSystem.Data.Entities;
using Pro.Data;
using Pro.Data.Entities;

namespace ProAd.Data.Entities
{
    public class UserContext<T> : EntityContext<DbSystem, T> where T : IEntityItem
    {
        public static void Refresh(int AccountId)
        {
            DbContextCache.Remove<T>(Settings.ProjectName, EntityCacheGroups.System, AccountId, 0);
        }
        
        public UserContext(int userId)
        {
            if (userId > 0)
                CacheKey = DbContextCache.GetKey<T>(Settings.ProjectName, EntityCacheGroups.System, 0, userId);
        }
        public IList<T> GetList()
        {
            //int ttl = 3;
            return DbContextCache.EntityList<DbSystem, T>(CacheKey, null);
        }
        public IList<T> GetList(int UserId)
        {
            //int ttl = 3;
            return DbContextCache.EntityList<DbSystem, T>(CacheKey, new object[] { "UserId", UserId });
        }
        protected override void OnChanged(ProcedureType commandType)
        {
            DbContextCache.Remove(CacheKey);
        }
        public FormResult GetFormResult(EntityCommandResult res, string reason)
        {
            return FormResult.Get(res, this.EntityName, reason);
        }
      

        #region UserTeam

        //public static IEnumerable<UserTeamProfile> GetUserTeamList(int AccountId, int TeamId)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //        return db.ExecuteList<UserTeamProfile>("vw_UserTeam", "AccountId", AccountId, "TeamId", TeamId);
        //}

        #endregion
    }

    public class AdUserContext 
    {
         #region static

        public static UserProfileView VirtualUserSet(int accountId, int userId)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteSingle<UserProfileView>("sp_Ad_UserVirtual", "Mode", "set", "AccountId", accountId, "UserId", userId);
        }
        public static UserProfileView VirtualUserCancel(int accountId, int userId)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteSingle<UserProfileView>("sp_Ad_UserVirtual", "Mode", "cancel", "AccountId", accountId, "UserId", userId);
        }

        public static IEnumerable<UserProfileView> GetUsersView(int accountId, int userId)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteList<UserProfileView>("sp_Ad_GetUsers", "AccountId", accountId, "UserId", userId);
        }

        public static IEnumerable<UserRoles> GetUsersRoleView(int accountId, int userId)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteList<UserRoles>("sp_Ad_GetUsersRole", "AccountId", accountId, "UserId", userId);
        }
        public static string LookupUserName(int userId)
        {
            if (userId <= 0)
                return "";
            var TableName = "Ad_UserProfile";// EntityMappingAttribute.View<T>();
            using (var db = DbContext.Create<DbSystem>())
            {
                return db.QueryScalar<string>("select UserName from " + TableName + " where UserId=@UserId", "", "UserId", userId);
            }
        }

        #endregion

        public static IEnumerable<Dictionary<string, object>> GeUsersInTeam(int accountId, int userId)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.QueryDictionary("vw_Ad_UsersInTeam", "AccountId", accountId, "UserId", userId);
        }
    }

    [EntityMapping("vw_UserTeam",ProcListView = "sp_UsersOrTeam")]
    public class UserTeamProfile : IEntityItem
    {
        public int UserTeamId
        {
            get;
            set;
        }
        public string DisplayName
        {
            get;
            set;
        }
        public int AccountId
        {
            get;
            set;
        }
         [EntityProperty(EntityPropertyType.Key)]
        public int UserId
        {
            get;
            set;
        }
         public bool IsTeam
        {
            get;
            set;
        }
       
    }

    public class SigendUserEx : Nistec.Web.Security.SignedUser
    {

        public int ExType
        {
            get
            {
                return base.GetDataValue<int>("ExType");
            }
        }

        public void LoadClaims()
        {
            var claims=AdContext.ListAdPermsItem(UserId);
            Claims = new Nistec.Generic.NameValueArgs(claims);
        }


    }



    /*
        [EntityMapping("web_UserProfile")]
        public class UserProfile : IEntityItem
        {
            //const string TableName = "web_UserProfile";
            //public static IEnumerable<UsersView> GetUsersView()
            //{
            //    return db.EntityGetList<UsersView>(TableName, null);
            //}


            //public static UsersView GetUsersView(int userId)
            //{
            //    return db.EntityItemGet<UsersView>(TableName, userId);
            //}



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
     */
}
