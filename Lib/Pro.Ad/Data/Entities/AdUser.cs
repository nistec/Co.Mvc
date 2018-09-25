using Nistec;
using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Serialization;
using Nistec.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace ProAd.Data.Entities
{
    public class AdUser : UserProfile, IUser, ISignedUser
    {
        internal const string SessionKey = "SignedUser";


        #region static

        public static AuthState DoSignIn(string loginName, string pass, bool createPersistentCookie, string HostClient = null, string HostReferrer = null, string AppName = null, bool? IsMobile = null)
        {
            try
            {
                var auth = FormsAuth.Instance;
                var user = Authorizer.Login<AdUser>(loginName, pass, HostClient, HostReferrer, AppName, IsMobile);
                if (user == null)
                {
                    throw new SecurityException(AuthState.UnAuthorized, "User not Authenticated");
                }
                if (!user.IsAuthenticated)
                {
                    throw new SecurityException(AuthState.UnAuthorized, "User not Authenticated");
                }
                //user.Data = UserDataContext.GetUserDataEx(user.AccountId,user.UserId);
                user.SetUserDataEx();
                auth.SignIn(user, createPersistentCookie);

                user.StateDescription = "Succeeded";
                user.IsMobile = IsMobile.Value;
                user.LoadDataAndClaims();

                return (AuthState)user.State;//. IsAuthenticated;
            }
            catch (SecurityException sex)
            {
                TraceHelper.Log("SignIn", "Error", sex.Message, HostClient, HostReferrer, -1);
                return (AuthState)sex.ErrorCode;
            }
            catch (Exception ex)
            {
                TraceHelper.Log("SignIn", "Error", "SignIn error: " + ex.Message, HostClient, HostReferrer, -1);
                return AuthState.Failed;
            }
        }

        public static AdUser Get(HttpContextBase context)
        {
            if (context == null || !context.Request.IsAuthenticated || !(context.User.Identity is FormsIdentity))
            {
                return NotAuthrized(AuthState.UnAuthorized, "Http error: Invalid HttpContext");
            }
            AdUser signedUser = null;
            //SignedUser signedUser = (SignedUser)context.Session[SignedUser.SessionKey];
            //if(signedUser!=null)
            //{
            //    return signedUser;
            //}
            var formsIdentity = (FormsIdentity)context.User.Identity;
            signedUser = new AdUser(formsIdentity);
            if (signedUser.IsAuthenticated == false || signedUser.IsBlocked)
            {
                //Log.Fatal("User not Authenticated");
                return NotAuthrized(AuthState.UnAuthorized, "Authenticatation error: User not Authenticated");
            }
            signedUser.State = (int)AuthState.Succeeded;
            signedUser.IsMobile = DeviceHelper.IsMobile(context.Request);
            //context.Session[SignedUser.SessionKey] = signedUser;
            return signedUser;
        }

        public static AdUser Get(HttpContextBase context, UserRole role)
        {

            if (context == null || !context.Request.IsAuthenticated || !(context.User.Identity is FormsIdentity))
            {
                return NotAuthrized(AuthState.UnAuthorized, "Http error: Invalid HttpContext");
            }
            AdUser signedUser = null;
            //SignedUser signedUser = (SignedUser)context.Session[SignedUser.SessionKey];
            //if(signedUser!=null)
            //{
            //    return signedUser;
            //}
            var formsIdentity = (FormsIdentity)context.User.Identity;
            signedUser = new AdUser(formsIdentity);
            if (signedUser.IsAuthenticated == false || signedUser.IsBlocked)
            {
                //Log.Fatal("User not Authenticated");
                return NotAuthrized(AuthState.UnAuthorized, "Authenticatation error: User not Authenticated");
            }
            if (signedUser.UserRole < (int)role)
            {
                //Log.Fatal("User not Authenticated");
                return NotAuthrized(AuthState.UnAuthorized, "Authenticatation error: Access is denied");
            }
            signedUser.State = (int)AuthState.Succeeded;
            signedUser.IsMobile = DeviceHelper.IsMobile(context.Request);
            //context.Session[SignedUser.SessionKey] = signedUser;
            return signedUser;
        }

        public static string GetJson(HttpContextBase context)
        {
            if (context == null || !context.Request.IsAuthenticated || !(context.User.Identity is FormsIdentity))
            {
                return JsonSerializer.ConvertToJson(new object[] { "state", AuthState.UnAuthorized, "desc", "Http error: Invalid HttpContext" }, null);
            }
            string userData = null;
            var formsIdentity = (FormsIdentity)context.User.Identity;
            if (formsIdentity != null)
            {
                userData = formsIdentity.Ticket.UserData;
            }
            if (userData == null)
            {
                return JsonSerializer.ConvertToJson(new object[] { "state", AuthState.UnAuthorized, "desc", "FormsIdentity error: Invalid User Data" }, null);
            }

            return UserProfile.UserDataToJson(userData);
        }

        internal static AdUser NotAuthrized(AuthState state, string desc)
        {
            return new AdUser() { State = (int)state, StateDescription = desc };
        }
        #endregion

        #region ctor
        public AdUser() { }

        public AdUser(FormsIdentity identity)
            : base(identity)
        {

        }
        #endregion

        #region properties
        [EntityProperty]
        public int State { get; set; }
        [EntityProperty(EntityPropertyType.NA)]
        public string StateDescription { get; set; }
        [EntityProperty]
        public int EvaluationDays { get; set; }
        [EntityProperty(EntityPropertyType.NA)]
        public bool IsMobile { get; set; }
        [EntityProperty(EntityPropertyType.NA)]
        public int ExType { get; set; }

        //[EntityProperty]
        //public int ApplicationId { get; set; }

        //public AuthState AuthState
        //{
        //    get { return (AuthState)State; }
        //}

        #endregion

        #region read/write perms
        //v-e-r-x-m
        //view-edit-remove-export-management
        /*
        public bool AllowEdit
        {
            get { return UserRole >= 1; }
        }
        public bool AllowAdd
        {
            get { return UserRole >= 1; }
        }
        public bool AllowExport
        {
            get { return UserRole >= 5; }
        }
        public bool AllowDelete
        {
            get { return UserRole >= 5; }
        }
        public string AllowEditClass
        {
            get { return AllowEdit ? "" : "item-pasive"; }
        }
        public string AllowAddClass
        {
            get { return AllowAdd ? "" : "item-pasive"; }
        }
        public string AllowExportClass
        {
            get { return AllowExport ? "" : "item-pasive"; }
        }
        public string AllowDeleteClass
        {
            get { return AllowDelete ? "" : "item-pasive"; }
        }
        //public bool IsManager
        //{
        //    get { return UserRole >= 5; }
        //}
        */

        //public PermsValue GetDefaultPerms()
        //{
        //    //UserRole role= Nistec.Web.Security.UserRole.User

        //    if (UserRole >= 5) //UserRole.Manager
        //        return PermsValue.FullControl;
        //    if (UserRole == 2)  //UserRole.Super
        //        return PermsValue.Modify;
        //    //if (UserRole > 1)
        //    //    return PermsValue.Add;
        //    //if (UserRole >= 1)
        //    //    return PermsValue.Write;
        //    if (UserRole == 1) //UserRole.User
        //        return PermsValue.Read;

        //    return PermsValue.None;
        //}

        #endregion

        public void LoadDataAndClaims()
        {
            //DefaultRule=(int) GetDefaultPerms();
            ExType = base.GetDataValue<int>("ExType");
            //EvaluationDays = base.GetDataValue<int>("EvaluationDays");
            LoadClaims();
        }
        public void LoadClaims()
        {
            var claims = AdContext.ListAdPermsItem(UserId);
            if (claims != null)
                Claims = new Nistec.Generic.NameValueArgs(claims);
        }

        public PermsValue GetPems(string itemName)
        {
            if (Claims == null)
                return DefaultRule;
            string val;
            if (Claims.TryGetValue(itemName, out val))
            {
                return (PermsValue)Types.ToInt(val);
            }
            return DefaultRule;
        }
    }
}
