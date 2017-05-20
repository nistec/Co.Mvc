using Pro.Data;
using Pro.Data.Entities;
using Pro.Mvc.Models;
using Nistec;
using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Web;
using Nistec.Web.Cms;
using Nistec.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pro.Lib;
using Pro.Data.Entities.Props;
using PropsEnum=Pro.Data.Entities.PropsEnum;
using Pro.Data.Registry;
using Nistec.Generic;
using Nistec.Runtime;
using ProSystem.Data.Entities;

namespace Pro.Mvc.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {

      

        [HttpPost]
        public JsonResult SetMngData(string value)
        {
            base.SetSignedUserData(value);
            return Json(null);
        }


        #region admin

        public ActionResult Manager()
        {
            if (IsAdmin())
                return RedirectToAction("Main", "Admin");
            return View();
        }
        public ActionResult LogonGrid()
        {
            return AuthenticateAdmin(null);
        }
        public ActionResult SysProperties()
        {
            return AuthenticateAdmin(null);
        }

        //public ActionResult UserInfo(int userId)
        //{
        //    var view =userId==0 ? new UsersView():UsersView.GetUsersView(userId);
        //    return View(view);
        //}
        public ActionResult UserPerms()
        {
            return AuthenticateAdmin(null);
        }
        //public ActionResult UsersList()
        //{
        //    var view = UsersView.GetUsersView();
        //    return AuthenticateAdmin(view);
        //}
        public ActionResult LogMonitor()
        {
            return AuthenticateAdmin(null);
        }
        [HttpPost]
        public JsonResult GetLogMonitor()
        {
            int PageSize = Types.ToInt(Request["pagesize"], 20);
            int PageNum = Types.ToInt(Request["pagenum"]);

            var list = DbLogs.ViewLog(PageNum);
            var row = list.FirstOrDefault<Dictionary<string,object>>();
            int totalRows = row == null ? 0 : row.Get<int>("TotalRows");
            return QueryPagerServer<Dictionary<string, object>>(list, totalRows, PageSize, PageNum);

        }


        [HttpPost]
        public ActionResult DoVirtualLoginSet()
        {
            int userId = GetUser();
            try
            {
                if(!IsAdminOrMaster())
                {
                    throw new Exception("User not allowed to DoVirtualLoginSet " + userId.ToString());
                }
                int accountId = Types.ToInt(Request["accountId"]);
                var profile = AdUserContext.VirtualUserSet(accountId, userId);
                var current = FormsAuth.GetCurrent();
                current.AccountId = accountId;
                current.AccountName = profile.AccountName;
                current.AccountCategory = profile.AccountCategory;
                //FormsAuth.Instance.SetAuthenticatedUserForRequest(current);
                FormsAuth.Instance.SignIn(current, false);
                return Json(new ResultModel() { Status = 1, Link = "/Home/Dashboard" });

                //return RedirectToAction("Dashboard", "Home");
            }
            catch (Exception ex)
            {
                return Json(new ResultModel() { Status = -1, Message = ex.Message });
                //return RedirectToIndex("The user name or password provided is incorrect.");// View(model);

            }
        }

        [HttpPost]
        public ActionResult DoVirtualLoginCancel()
        {
            int userId = GetUser();
            try
            {
                if (!IsAdminOrMaster())
                {
                    throw new Exception("User not allowed to DoVirtualLoginCancel " + userId.ToString());
                }
                var profile = AdUserContext.VirtualUserCancel(0, userId);
                var current = FormsAuth.GetCurrent();
                current.AccountId = profile.AccountId;
                current.AccountName = profile.AccountName;
                current.AccountCategory = profile.AccountCategory;
                FormsAuth.Instance.SignIn(current, false);
                return Json( new ResultModel() { Status = 1, Link = "/Admin/Main" });
                // RedirectToAction("Dashboard", "Home");
            }
            catch (Exception ex)
            {
                return Json(new ResultModel() { Status = -1,  Message = ex.Message });
                //return RedirectToIndex("The user name or password provided is incorrect.");// View(model);
            }
        }

        public ActionResult Logoff()
        {

            FormsAuth.Instance.SignOut();
            return View();
        }

        #endregion

        #region Cms
        /*
        public ActionResult CmsTree()
        {
            //var view = CmsPage.ViewPages(1);
            //return AuthenticateAdmin(view);

            return View();
        }

        public ActionResult CmsSite(string sid)
        {
            return View(sid);
        }
        
        [HttpPost]
        public JsonResult GetSiteView()
        {
            IList<CmsSiteView> list = Nistec.Web.Cms.CmsSiteView.View();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCmsPages(string SiteId)
        {
            if (SiteId == null || SiteId == "")
                return null;
            List<CmsPage> list = Nistec.Web.Cms.CmsPage.ViewPages(Nistec.Types.ToInt(SiteId));
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CmsPage(int pid)
        {
            ViewBag.PageName = Nistec.Web.Cms.CmsPage.LookupPageName(pid);
            return View(pid);
        }
        */

        [HttpGet]
        public ActionResult CmsContentGrid()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CmsClearAllCache()
        {
            string action = "ניקוי זכרון";
            try
            {
                int AccountId = GetAccountId();
                int res = CmsContext.ClearCacheAll();
                return Json(GetFormResult(res, action, null, 0), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

       
        
        #endregion

        #region Accounts property

        public ActionResult Accounts()
        {
            if (!IsAdmin())
            {
                return RedirectToIndex("User not alllowed for this method");
            }
            return View();
        }

        public ActionResult DefAccount()
        {
            if (!IsAdmin())
            {
               return RedirectToIndex("User not alllowed for this method");
            }
            return View();
        }

        [HttpPost]
        public JsonResult DefAccountUpdate()
        //public JsonResult DefAccountUpdate(int PropId, string PropName, 
        //   string  SmsSender,
        //   string  MailSender,
        //   int AuthUser,
        //   int AuthAccount,
        //   bool EnableSms,
        //   bool EnableMail,
        //   string  Path,
        //   string  SignupPage,
        //   bool EnableInputBuilder,
        //   bool BlockCms,
        //   int SignupOption,
        //   string Design,
        //   bool EnableSignupCredit,
        //   string CreditTerminal,
        //   string RecieptEvent,
        //   string RecieptAddress,
        //    int command)
        {
 
           
            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {
                if (!IsAdmin())
                {
                    throw new Exception("User not alllowed for this method");
                }
                int command = Types.ToInt(Request["command"]);

                AccountsView v = new AccountsView()
                {
                    PropId = Types.ToInt(Request["PropId"]),
                    PropName = Request["PropName"],
                    SmsSender = Request["SmsSender"],
                    MailSender = Request["MailSender"],
                    AuthUser = Types.ToInt(Request["AuthUser"]),
                    AuthAccount = Types.ToInt(Request["AuthAccount"]),
                    EnableSms = Types.ToBool(Request["EnableSms"],false),
                    EnableMail = Types.ToBool(Request["EnableMail"],false),
                    Path = Request["Path"],
                    SignupPage = Request["SignupPage"],
                    EnableInputBuilder = Types.ToBool(Request["EnableInputBuilder"],false),
                    BlockCms=Types.ToBool(Request["BlockCms"],false),
                    //SignupOption=Types.ToInt(Request["SignupOption"]),
                    Design=Request["Design"]
                    //EnableSignupCredit=Types.ToBool(Request["EnableSignupCredit"],false),
                    //CreditTerminal= HttpUtility.UrlDecode( Request["CreditTerminal"]),
                    //RecieptEvent = Request["RecieptEvent"],
                    //RecieptAddress = Request["RecieptAddress"]
                };

                result = AccountsView.DoSave(v, (UpdateCommandType)command);
                rm = new ResultModel(result);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                result = -1;
                rm = new ResultModel()
                {
                    Status = result,
                    Message = err
                };
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }
               [HttpPost]
        public JsonResult DefAccountUpdateCredit()
        {
            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {
                if (!IsAdmin())
                {
                    throw new Exception("User not alllowed for this method");
                }
                AccountsCreditView v = new AccountsCreditView()
                {
                    PropId = Types.ToInt(Request["PropId"]),
                    //PropName = Request["PropName"],
                    SignupOption=Types.ToInt(Request["SignupOption"]),
                    EnableSignupCredit=Types.ToBool(Request["EnableSignupCredit"],false),
                    CreditTerminal= HttpUtility.UrlDecode( Request["CreditTerminal"]),
                    RecieptEvent = Request["RecieptEvent"],
                    RecieptAddress = Request["RecieptAddress"]
                };

                result = AccountsCreditView.DoSave(v);
                rm = new ResultModel(result);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                result = -1;
                rm = new ResultModel()
                {
                    Status = result,
                    Message = err
                };
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public JsonResult GetAccountView()
        {
            int accountId = GetAccountId();
            var list = AccountsView.ViewList();
            //var list = EntityPro.ViewEntityList<AccountsView>(EntityGroups.Enums, AccountsView.TableName, accountId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult GetAccountsList()
        {
            //int accountId = GetAccountId();
            //AdContext<AdAccount> context = new AdContext<AdAccount>(accountId);
            //var list = context.GetList();
            //return Json(list, JsonRequestBehavior.AllowGet);

            var list = AccountsView.AccountsList();
            return GetJsonResult(list);
        }
        #endregion

        #region Users def

        public ActionResult UsersDef()
        {
            //var list = DbPro.Instance.GetEntityList<AreaView>(AreaView.MappingName, null);
            return View();
        }
        [HttpPost]
        public JsonResult GetUsersRoles()
        {
            //var list = DbPro.Instance.EntityGetList<UserRoles>(UserRoles.MappingName, null);
            int accountId = GetAccountId();
            int userId = GetUser();
            var list = AdUserContext.GetUsersRoleView(accountId, userId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAdminUsersRoles()
        {
            int accountId = Types.ToInt(Request["accountId"]);
            int userId = GetUser();
            var list = AdUserContext.GetUsersRoleView(accountId, userId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAdminUsersProfile()
        {
            int accountId = Types.ToInt(Request["accountId"]);
            int userId = GetUser();
            //var list = DbPro.Instance.QueryEntityList<UserProfileView>(UserProfileView.MappingName, "AccountId", accountId);
            var list = AdUserContext.GetUsersView(accountId, userId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetUsersProfile()
        {
            int accountId = GetAccountId();
            int userId = GetUser();
            //var list = DbPro.Instance.QueryEntityList<UserProfileView>(UserProfileView.MappingName, "AccountId", accountId);
            var list = AdUserContext.GetUsersView(accountId, userId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult GetUps(int UserId)
        //{
        //    using (var db = DbContext.Create<DbPro>())
        //    {
        //        var entity = db.EntityItemGet<UserMembership>(UserMembership.MappingName, "UserId", UserId);
        //        return Json(entity, JsonRequestBehavior.AllowGet);
        //    }
        //}

        // Insert=0,Update=1,Delete=2,StoredProcedure=3
        [HttpPost]
        public JsonResult UserDefUpdate(int UserId, string UserName, int UserRole, string Email, string Phone, int AccountId, string Lang, int Evaluation, bool IsBlocked, string DisplayName)//UserProfile user, int command)
        {
            UserResult result;
            try
            {
                //AccountId = GetAccountId();
                UserProfile view = UserProfile.Get(UserId);
                AccountId = view.AccountId;
                UserProfile newItem = new UserProfile()
                {
                    AccountId = AccountId,
                    Creation = DateTime.Now,
                    DisplayName = DisplayName,
                    Email = Email,
                    Evaluation = Evaluation,
                    IsBlocked = IsBlocked,
                    Lang = Lang,
                    Phone = Phone,
                    UserId = UserId,
                    UserName = UserName,
                    UserRole = UserRole
                };
                
                if (IsAdminOrManager() == false)
                {
                    newItem.UserRole = view.UserRole;
                    newItem.IsBlocked = view.IsBlocked;
                    newItem.AccountId = view.AccountId;
                }
                result = view.Update(newItem);
                SetResult(result);
            }
            catch (Exception)
            {
                result = new UserResult() { Status = (int)AuthState.Failed };
                SetResult(result);
            }
            return Json(result);
        }

        // Insert=0,Update=1,Delete=2,StoredProcedure=3
        [HttpPost]
        public JsonResult UserDefDelete(int UserId)
        {
            UserResult result;
            try
            {
                UserProfile view = UserProfile.Get(UserId);
                if (view.UserRole < 5)
                {
                    result = new UserResult() { Status = (int)AuthState.UnAuthorized };
                }
                else
                {
                    result = view.Delete();
                }
                SetResult(result);
            }
            catch (Exception)
            {
                result = new UserResult() { Status = (int)AuthState.Failed };
                SetResult(result);
            }
            return Json(result);
        }

        // Insert=0,Update=1,Delete=2,StoredProcedure=3
        [HttpPost]
        public JsonResult UserDefRegister(string UserName, int UserRole, string Email, string Phone, int AccountId, string Lang, int Evaluation, bool IsBlocked, string DisplayName, string Password)//(UserRegister user)
        {
            UserResult result;
            try
            {
                //AccountId = GetAccountId();
                if (IsAdminOrManager() ==false)
                {
                    result = new UserResult() { Status = (int)AuthState.UnAuthorized };
                }
                else
                {
                    var user = new UserRegister(UserName, UserRole, Email, Phone, AccountId, Lang, Evaluation, IsBlocked, DisplayName, Password);
                    result = Authorizer.Register(user);
                }
                SetResult(result);
            }
            catch (Exception)
            {
                result = new UserResult() { Status = (int)AuthState.Failed };
                SetResult(result);
            }
            return Json(result);
        }

        //// Insert=0,Update=1,Delete=2,StoredProcedure=3
        //[HttpPost]
        //public JsonResult UserUpsUpdate(int UserId, string Ups, int command)
        //{
        //    UserResult result=null;
        //    try
        //    {
        //        if (command == 0 || command == 2)
        //        {
        //            if (IsAdminOrManager() == false)
        //            {
        //                result = new UserResult() { Status = (int)AuthState.UnAuthorized };
        //            }
        //        }
        //        else
        //        {
        //            UserMembership newItem = new UserMembership()
        //            {
        //                Password = Ups,
        //                UserId = UserId,
        //                PasswordSalt=EncryptPass(Ups),
        //                CreateDate = DateTime.Now
        //            };
        //            switch (command)
        //            {
        //                case 0://insert
        //                    result = newItem.Insert(); break;
        //                case 2://delete
        //                    result = newItem.Delete(); break;
        //                default:
        //                    UserMembership view = UserMembership.Get(UserId);
        //                    result = view.Update(newItem);
        //                    break;
        //            }
        //        }
        //        //UserMembership view = command == 0 ? new UserMembership() : UserMembership.Get(UserId);
        //        //int result = view.Update(newItem, (Nistec.Data.UpdateCommandType)command);
        //    }
        //    catch (Exception)
        //    {
        //        result = new UserResult() { Status = (int)AuthState.Failed };
        //        SetResult(result);
        //    }
        //    return Json(result);
        //}

        #endregion

        #region Def Entity Enum

        public ActionResult DefEntityEnum(string entity)
        {
            switch (entity)
            {
                case "status":
                    ViewBag.TagPropId = StatusView.TagPropId;
                    ViewBag.TagPropName = StatusView.TagPropName;
                    ViewBag.TagPropTitle = StatusView.TagPropTitle;
                    break;
                case "role":
                    ViewBag.TagPropId = RoleView.TagPropId;
                    ViewBag.TagPropName = RoleView.TagPropName;
                    ViewBag.TagPropTitle = RoleView.TagPropTitle;
                    break;
                //case "category":
                //    ViewBag.TagPropId = CategoryView.TagPropId;
                //    ViewBag.TagPropName = CategoryView.TagPropName;
                //    ViewBag.TagPropTitle = CategoryView.TagPropTitle;
                //    break;
                //case "region":
                //    ViewBag.TagPropId = RegionView.TagPropId;
                //    ViewBag.TagPropName = RegionView.TagPropName;
                //    ViewBag.TagPropTitle = RegionView.TagPropTitle;
                //    break;
            }

            return View();
        }


        [HttpPost]
        public JsonResult DefEntityEnumUpdate(int PropId, string PropName, string PropType, int command)
        {
            //ValidateAdmin();
            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {

                switch (PropType)
                {
                    case "status":
                        result = PropsEnum.EntityEnum.DoSaveProc(PropId, PropName, PropsEnum.StatusView.EnumType, accountId, (UpdateCommandType)command);
                        break;
                    case "role":
                        result = PropsEnum.EntityEnum.DoSaveProc(PropId, PropName, PropsEnum.RoleView.EnumType, accountId, (UpdateCommandType)command);
                        break;
                    //case "category":
                    //    result = EntityEnum.DoSaveProc(PropId, PropName, CategoryView.EnumType, accountId, (UpdateCommandType)command);
                    //    break;
                    //case "region":
                    //    result = EntityEnum.DoSaveProc(PropId, PropName, RegionView.EnumType, accountId, (UpdateCommandType)command);
                    //    break;
                }
                rm = new ResultModel(result);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                result = -1;
                rm = new ResultModel()
                {
                    Status = result,
                    Message = err
                };
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }


        #endregion



 
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
           
            var user = FormsAuth.DoSignInUser(model.UserName, model.Password, model.RememberMe, true);
            if (user != null || user.IsAdmin)
            {
                return RedirectToLocal(returnUrl);
            }
            // If we got this far, something failed, redisplay form
            //ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return RedirectToIndex("The user name or password provided is incorrect.");// View(model);

            //return Index("The user name or password provided is incorrect.");
        }

        protected override ActionResult RedirectToIndex(string message)
        {

            ViewBag.Message = message;

            ModelState.AddModelError("ErrorMessage", "The user name or password provided is incorrect.");

            return RedirectToAction("Manager", "Admin", ModelState);//new { ErrorMessage = message });

        }

        protected override ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Main", "Admin");
            }
        }

        public ActionResult Main()
        {
            return Authenticate(null);
        }

      
    }
}
