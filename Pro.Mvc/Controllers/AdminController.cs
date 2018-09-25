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
using ProSystem.Query;
using System.Data;
using Nistec.Web.Controls;
using ProSystem.Data;
using ProAd.Data.Entities;
using ProAd.Query;
using Pro.Query;

namespace Pro.Mvc.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {

      

        //[HttpPost]
        //public JsonResult SetMngData(string value)
        //{
        //    base.SetSignedUserData(value);
        //    return Json(null);
        //}


        #region admin

        public ActionResult Manager()
        {
            if (!IsSignedUser(UserRole.SubAdmin, false)) //(IsAdmin())
                return RedirectToAction("Co", "Admin");
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
            try { 
            var su = GetSignedUser(true);

            int PageSize = Types.ToInt(Request["pagesize"], 20);
            int PageNum = Types.ToInt(Request["pagenum"]);

            string QueryType = Types.NZ( Request["type"],"co");

            var list = DbLogs.ViewLog(QueryType,PageNum);
            var row = list.FirstOrDefault<Dictionary<string,object>>();
            int totalRows = row == null ? 0 : row.Get<int>("TotalRows");
                return QueryPagerServer<Dictionary<string, object>>(list, totalRows, su.UserId);// PageSize, PageNum);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
        }


        [HttpPost]
        public ActionResult DoVirtualLoginSet()
        {
            int userId = GetUser();
            try
            {
                if (!IsSignedUser(UserRole.System, false)) //(!IsAdminOrMaster())
                {
                    throw new Exception("User not allowed to DoVirtualLoginSet " + userId.ToString());
                }
                int accountId = Types.ToInt(Request["accountId"]);
                var profile = AdUserContext.VirtualUserSet(accountId, userId);
                var current = FormsAuth.GetCurrent();
                current.AccountId = accountId;
                current.AccountName = profile.AccountName;
                current.AccountCategory = profile.AccountCategory;
                current.SetUserDataEx();
                //FormsAuth.Instance.SetAuthenticatedUserForRequest(current);
                FormsAuth.Instance.SignIn(current, false);
                return Json(new ResultModel() { Status = 1, Link = "/Co/Dashboard" });

                //return RedirectToAction("Dashboard", "Co");
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
                if (!IsSignedUser(UserRole.System, false)) //(!IsAdminOrMaster())
                {
                    throw new Exception("User not allowed to DoVirtualLoginCancel " + userId.ToString());
                }
                var profile = AdUserContext.VirtualUserCancel(0, userId);
                var current = FormsAuth.GetCurrent();
                current.AccountId = profile.AccountId;
                current.AccountName = profile.AccountName;
                current.AccountCategory = profile.AccountCategory;
                current.SetUserDataEx();
                FormsAuth.Instance.SignIn(current, false);
                return Json( new ResultModel() { Status = 1, Link = "/Admin/Main" });
                // RedirectToAction("Dashboard", "Co");
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

        #region Accounts property _legacy

        //public ActionResult Accounts()
        //{
        //    if (!IsAdmin())
        //    {
        //        return RedirectToIndex("User not alllowed for this method");
        //    }
        //    return View(true);
        //}

        public ActionResult DefAccount()
        {
            if (!IsSignedUser(UserRole.SubAdmin, false)) //(!IsAdmin())
                {
               return RedirectToIndex("User not alllowed for this method");
            }
            return View(true);
        }

        [HttpPost]
        public JsonResult DefAccountUpdate()
        {
 
           
            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {
                if (!IsSignedUser(UserRole.SubAdmin, false)) //(!IsAdmin())
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
                if (!IsSignedUser(UserRole.SubAdmin, false)) //(!IsAdmin())
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

        #region Users def _legacy

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
                
                if (!IsSignedUser(Nistec.Web.Security.UserRole.System, false)) //(IsAdminOrManager() == false)
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
                if (!IsSignedUser(Nistec.Web.Security.UserRole.System, false)) //(IsAdminOrManager() ==false)
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

        #region AdUsers

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
        public ActionResult AdUserDefUpdate()
        {
            string action = "עדכון משתמש";
            AdContext<AdUserProfile> context = null;
            try
            {
                var su = GetSignedUser(UserRole.SubAdmin, true);

                ValidateUpdate(su.UserId, "AdUserDefUpdate");
                bool isResetPass = Types.ToBool(Request.Form["IsResetPass"], false);
                int accountId = 0;// su.AccountId;
                context = new AdContext<AdUserProfile>(accountId);
                context.Set(Request.Form);
                //context.Current.AccountId = accountId;
                var res = context.SaveChanges(false);
                if (isResetPass)
                {
                    var status = ForgotUserPassword(context.Current);
                    bool isok = false;
                    string msg = StatusDesc.GetMembershipStatus("MembershipStatus", status, ref isok);
                    if (status != 10)
                    {
                        ViewBag.Message = msg;
                        return Json(FormResult.Get(-1, action, msg), JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(context.GetFormResult(res, null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }
        protected int ForgotUserPassword(AdUserProfile model)
        {
            //string token = Guid.NewGuid().ToString().Replace("-", "");
            //string ConfirmUrl = "https://co.my-t.co.il/account/resetpassword?token=" + token;
            //string AppUrl = "https://co.my-t.co.il/";
            //string MailSenderFrom = "my-t <co@mytdocs.com>";
            //string Subject = "Reset your Co password";
            //string profile_name = "coProfile";
            //var status = UserMembership.ForgotPassword(model.Email, token, ConfirmUrl, AppUrl, MailSenderFrom, Subject, profile_name);

            var status = UserMembership.SendResetToken(model.Email, ProSettings.AppId);
            return status;
        }

        [HttpPost]
        public ActionResult AdUserDefInsert()
        {
            string action = "הוספת משתמש חדש";
            AdContext<AdUserProfile> context = null;
            try
            {

                var su = GetSignedUser(UserRole.SubAdmin ,true);

                ValidateUpdate(su.UserId, "AdUserDefInsert");

                int accountId = 0;// su.AccountId;

                context = new AdContext<AdUserProfile>(accountId);
                context.Set(Request.Form);
                context.Validate(ProcedureType.Insert);
                var cur = context.Current;
                var res = context.Upsert(UpsertType.Insert, ReturnValueType.ReturnValue, 
                    "DisplayName", cur.DisplayName,
                    "Email", cur.Email,
                    "Phone", cur.Phone,
                    "UserName", cur.UserName,
                    "UserRole", cur.UserRole,
                    "AccountId", cur.AccountId,
                    "Lang", cur.Lang,
                    "Evaluation", cur.Evaluation,
                    "IsBlocked", cur.IsBlocked,
                    "Password", Guid.NewGuid().ToString().Substring(0, 8),
                    "Profession", 0,
                    "AppId", ProSettings.AppId);
                int status = res.GetReturnValue();
                var reslult = StatusDesc.GetMembershipResult(action, status);
                return Json(reslult, JsonRequestBehavior.AllowGet);
                //return Json(context.GetFormResult(res, Message), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AdUserDefDelete(int AccountId,int UserId)
        {
            string action = "מחיקת משתמש";
            AdContext<AdUserProfile> context = null;
            try
            {
                var su = GetSignedUser(UserRole.SubAdmin, true);
                               
                ValidateDelete(su.UserId, "AdUserDefDelete");

                int accountId = 0;// su.AccountId;
                context = new AdContext<AdUserProfile>(accountId);
                //context.Set(Request.Form);
                //context.Validate(ProcedureType.Delete);
                //var cur = context.Current;
                //var res = context.Delete("UserId", UserId, "AccountId", accountId, "AssignBy", user.UserId);
                var status = context.DeleteReturnValue(-1, "UserId", UserId, "AccountId", AccountId, "AssignBy", su.UserId);
                var reslult = StatusDesc.GetAuthResult(action, status);
                return Json(reslult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }
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


        #region Accounts targets
        /*

        [HttpPost]
        public JsonResult GetAccountsTargets(
           int QueryType,
           int AccountId,
           int UserId,
           string IdNumber,
           string Mobile,
           string Email,
           string AccountName,
           string Address,
           string City,
           string Category,
           string Branch,
           int JoinedFrom,
           int JoinedTo)
        {
            ResultModel model = null;
            try
            {

                string key = string.Format("GetTargets_{0}_{1}", AccountId, UserId);
                CacheRemove(key);

                AccountsQuery query = new AccountsQuery()
                {
                    AccountId = AccountId,
                    UserId = UserId,
                    Address = Address,
                    Branch = Branch,
                    Category = Category,
                    City = City,
                    //ContactRule = ContactRule,
                    JoinedFrom = JoinedFrom,
                    JoinedTo = JoinedTo,
                    IdNumber = IdNumber,
                    Mobile = Mobile,
                    Email = Email,
                    AccountName = AccountName,
                    //Place = Place,
                    QueryType = QueryType,
                    PageNum = 0,
                    PageSize = 999999999
                };
                query.Normelize();
                var totalRows = AccountsContext.ListQueryTargetsView(query);
                ViewBag.TargetsCount = totalRows;

                model = new ResultModel() { Status = totalRows, Title = "איתור נמענים", Message = string.Format("אותרו {0} נמענים", totalRows), Link = null, OutputId = totalRows };
            }
            catch (Exception ex)
            {
                model = new ResultModel() { Status = -1, Title = "איתור נמענים", Message = "Error: " + ex.Message, Link = null, OutputId = 0 };
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public ActionResult MailTargets()
        {
            AccountsQuery query = new AccountsQuery(Request.Form, 22);
            query.AccountId = GetAccountId();
            query.UserId = GetUser();
            query.QueryType = 22;
            return View(query);
        }

        public ActionResult MembersExport()
        {
            AccountsQuery query = new AccountsQuery(Request.Form, 20);
            query.AccountId = GetAccountId();
            query.UserId = GetUser();
            query.QueryType = 20;
            query.Normelize();
            var d = MemberContext.ListQueryDataView(query);
            return CsvActionResult.ExportToCsv(d, "Targets");

            //return View(query);
        }
        [HttpPost]
        public ActionResult DoMembersExport(string q)
        {
            if (q != null)
            {
                AccountsQuery query = AccountsQuery.Deserialize(q);
                ViewBag.MemberQuery = null;
                var d = MemberContext.ListQueryDataView(query);
                return CsvActionResult.ExportToCsv(d, "Targets");
            }
            else
            {
                ViewBag.Message = "אירעה שגיאה לא הופקו נתונים לקובץ";
                return null;
            }
        }

        public ActionResult ExportTargets()
        {
            int uid = GetUser();
            int accountId = GetAccountId();
            var d = TargetView.ViewData(accountId, uid);
            return CsvActionResult.ExportToCsv(d, "Targets");
        }

        protected DataTable GetTargetsData()
        {
            int uid = GetUser();
            int accountId = GetAccountId();
            return TargetView.ViewData(accountId, uid);
        }
        protected IList<TargetView> GetTargetsList()
        {
            int uid = GetUser();
            int accountId = GetAccountId();
            return TargetView.ViewList(accountId, uid);
        }

        protected string GetTargetsJson(bool isPrsonal)
        {
            int uid = GetUser();
            int accountId = GetAccountId();
            var list = TargetView.ViewList(accountId, uid);
            return TargetView.ToJson(list, isPrsonal);
        }

        */
        #endregion

        #region Accounts

        [HttpGet]
        public ActionResult Accounts()
        {
            AccountsQuery query = new AccountsQuery(Request.QueryString, 1);
            var su = GetSignedUser(false);
            if (su == null)
            {
                return RedirectToLogin();
            }
            if (!su.IsAdmin)
            {
                return RedirectToIndex("User not alllowed for this method");
            }

            //query.AccountId = su.AccountId;
            query.UserId = su.UserId;
            //query.ExType = su.GetDataValue<int>("ExType");
            //query.QueryType = 1;
            return View(true, query);
        }

        [HttpPost]
        [ActionName("Accounts")]
        public ActionResult AccountsPost()
        {
            AccountsQuery query = new AccountsQuery(Request.Form, 0);
            var su = GetSignedUser(false);
            if (su == null)
            {
                return RedirectToLogin();
            }
            if (!su.IsAdmin)
            {
                return RedirectToIndex("User not alllowed for this method");
            }
            //query.AccountId = su.AccountId;
            query.UserId = su.UserId;
            //query.ExType = su.GetDataValue<int>("ExType");
            //query.QueryType = 0;
            return View(true, query);
        }
        /*
        [HttpGet]
        public ActionResult AccountsUpload()
        {
            return View(true);
        }

        [HttpGet]
        public ActionResult AccountsRemove()
        {
            return View(true);
        }

        [HttpGet]
        public ActionResult AccountsAdd()
        {
            return View(true);
        }
        [HttpGet]
        public ActionResult AccountsEdit()
        {
            return View(true);
        }
      */

        public ActionResult AccountsQuery()
        {
            if (!IsSignedUser(UserRole.SubAdmin, false)) //(!IsAdmin())
            {
                return RedirectToIndex("User not alllowed for this method");
            }
            return View(true);
        }

        public ActionResult _AccountsQuery()
        {
            if (!IsSignedUser(UserRole.SubAdmin, false)) //(!IsAdmin())
            {
                return RedirectToIndex("User not alllowed for this method");
            }
            return PartialView(true);
        }

        public JsonResult GetAccountsGrid()
        {
            try
            {
                var su = GetSignedUser(true);
                if (!su.IsAdmin)
                {
                    throw new Exception("User not alllowed for this method");
                }
                AccountsQuery query = new AccountsQuery(Request);
                query.Normelize();

               
                //query.AccountId = su.AccountId;

                var list = AdContext.ListQueryView(query);
                return QueryPagerServer<AdAccountView>(list, su.UserId);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(GetSupportErrMessage("רשימת לקוחות"), JsonRequestBehavior.AllowGet);

            }
        }


        #endregion

        #region Accounts edit
        /*
        [HttpGet]
        public ActionResult _AccountsAdd()
        {
            return PartialView(true, "_AccountsEdit", new EditModel() { Option = "a" });
        }
        [HttpGet]
        public ActionResult _AccountsEdit(int id)
        {
            return PartialView(true, "_AccountsEdit", new EditModel() { Id = id, Option = "e" });
        }
        [HttpGet]
        public ActionResult _AccountsView(int id)
        {
            return PartialView("_AccountsEdit", new EditModel() { Id = id, Option = "g" });
        }
        */

        [HttpGet]
        public ActionResult _AccountEdit(int id)
        {
            return PartialView(true,"AccountEdit", new EditModel() { Id = id, Option = "e", Args= "~/Views/Shared/_ViewIframe.cshtml" });
        }

        [HttpGet]
        public ActionResult AccountEdit(int id)
        {
            return View(true, "AccountEdit", new EditModel() { Id = id, Option = "e",  Args= "~/Views/Shared/_ViewAdmin.cshtml" });
        }

        [HttpPost]
        public JsonResult GetAccountsEdit()
        {

            try
            {
                var su = GetSignedUser(true);
                if (!su.IsAdmin)
                {
                    throw new Exception("User not alllowed for this method");
                }
                int id = Types.ToInt(Request["id"]);

                var item = AdContext<AdAccount>.Get().Get("AccountId",id);
                return Json(item);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(GetSupportErrMessage("עריכת לקוח"), JsonRequestBehavior.AllowGet);
            }
        }
        

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult AccountUpdate()
        {
            int res = 0;
            string action = "הגדרת לקוח";
            AdAccount a = null;
            ResultModel model = null;
            try
            {
                var su = GetSignedUser(true);
                if (su==null || !su.IsAdmin)
                {
                    throw new Exception ("User not alllowed for this method");
                }
                a = EntityContext.Create<AdAccount>(Request.Form);
                //a.AccountId = usr.AccountId;
                //var exType = usr.GetDataValue<int>("ExType");
                EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת לקוח", "he");//, new object[] { "@ExType", exType });
                if (!validator.IsValid)
                {
                    return Json(GetFormResult(-1, action, validator.Result, 0), JsonRequestBehavior.AllowGet);

                }

                res = AdContext.DoSave(a,true);//, true, "", DataSourceTypes.CoSystem);
                if (res <=0)
                    model = new ResultModel() { Message = "הלקוח לא עודכן", Status = -1, Title = action };
                else
                    model = GetFormResult(res, action, "הלקוח עודכן", a.AccountId);

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(GetFormResult(-1, action, "אירעה שגיאה, הלקוח לא עודכן", 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult AccountDelete()
        {
            //MemberCategoryView
            int res = 0;
            string action = "הסרת לקוח";
            MemberItem a = null;
            try
            {
                var su = GetSignedUser(true);
                if (su == null || !su.IsAdmin)
                {
                    throw new Exception("User not alllowed for this method");
                }

                int AccountId = Types.ToInt(Request["AccountId"]);
                //int accountId = GetAccountId();
                res = -2;// AdContext.DoDelete(AccountId, accountId);
                string message = "";
                switch (res)
                {
                    case -2:
                        message = "אירעה שגיאה (Invalid Arguments Account) אנא פנה לתמיכה"; break;
                    case -1:
                        message = "הלקוח אינו קיים"; break;
                    case 1:
                        message = "הלקוח הוסר"; break;
                    default:
                        message = "הלקוח לא הוסר"; break;
                }

                var model = new ResultModel() { Status = res, Title = "הסרת לקוח", Message = message, Link = null, OutputId = 0 };

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Accounts Nested

        [HttpPost]
        public JsonResult GetAccountsCategories()
        {
            int accountId = GetAccountId();
            var list = AccountsCategoryView.ViewList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteAccountCategories(int propId)
        {
            int res = 0;
            int accountId = GetAccountId();
            ResultModel model = null;
            try
            {
                res = AccountsCategoryView.DoDelete(propId);
                model = new ResultModel() { Status = res, Title = "עדכון סווג", Message = "סווג הוסר למנוי " + propId, Link = null, OutputId = propId };
            }
            catch (Exception ex)
            {
                model = new ResultModel() { Status = res, Title = "עדכון סווג", Message = ex.Message, Link = null, OutputId = propId };
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //// [ValidateAntiForgeryToken]
        //public JsonResult UpdateAccountCategories()
        //{
        //    int res = 0;
        //    int accountId = GetAccountId();
        //    string action = "הגדרת סווג";
        //    try
        //    {
        //        int rcdid = Types.ToInt(Request.Form["AccountRecord"]);
        //        string proptypes = Request.Form["Categories"];
        //        if (rcdid <= 0)
        //        {
        //            return Json(GetFormResult(-1, action, "נדרש ת.ז", 0), JsonRequestBehavior.AllowGet);
        //        }

        //        res = AccountsCategoryView..AddCategory(rcdid, proptypes, accountId);
        //        return Json(GetFormResult(res, action, null, 0), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
        //    }
        //}

        #endregion

        #region Accounts_Label

        [HttpPost]
        public JsonResult GetAccountsLabel(int id)
        {
            int currentAccount = GetAccountId();
            var list = AdContext<Accounts_Label>.GetEntityList("AccountId", id);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult UpsertAccountsLabels(string olabel,string nlabel, string val)
        {
            //int accountId = GetAccountId();
            string action = "עדכון פרטים נוספים";
            try
            {
                var su= GetSignedUser(UserRole.System, true);

                var a = EntityContext.Create<Accounts_Label>(Request.Form);
                EntityValidator validator = EntityValidator.ValidateEntity(a, action, "he",a.LabelId==0? EntityOperation.Insert: EntityOperation.Update );
                if (!validator.IsValid)
                {
                    return Json(GetFormResult(-1, action, validator.Result, 0), JsonRequestBehavior.AllowGet);
                }
                
                var context = new AdContext<Accounts_Label>(su.AccountId);
                var res = context.Upsert(UpsertType.Upsert, ReturnValueType.ReturnValue, "LabelId", a.LabelId, "AccountId",a.AccountId,"Label",a.Label,"Val",a.Val);
                return Json(res.GetResult(action), JsonRequestBehavior.AllowGet);
                //return Json(FormResult.Get(res,action), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult DeleteAccountsLabels(int AccountId, long LabelId)//string Label)
        {
            string action = "מחיקת פרטים נוספים";
            try
            {
                var su = GetSignedUser(UserRole.SubAdmin, true);
                var context = new AdContext<Accounts_Label>(su.AccountId);
                var res = context.Delete("AccountId", AccountId, "LabelId", LabelId);

                return Json(FormResult.Get(res, action), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Accounts property

        [HttpPost]
        public JsonResult AccountProperty_Get(int id)
        {
            int accountId = GetAccountId();
            var list = AccountsView.ViewList();
            //var list = EntityPro.ViewEntityList<AccountsView>(EntityGroups.Enums, AccountsView.TableName, accountId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AccountProperty_Update()
        {


            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {
                if (!IsSignedUser(UserRole.SubAdmin, false)) //(!IsAdmin())
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
                    EnableSms = Types.ToBool(Request["EnableSms"], false),
                    EnableMail = Types.ToBool(Request["EnableMail"], false),
                    Path = Request["Path"],
                    SignupPage = Request["SignupPage"],
                    EnableInputBuilder = Types.ToBool(Request["EnableInputBuilder"], false),
                    BlockCms = Types.ToBool(Request["BlockCms"], false),
                    //SignupOption=Types.ToInt(Request["SignupOption"]),
                    Design = Request["Design"]
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

        #endregion

        #region Accounts Clearing

        [HttpPost]
        public JsonResult AccountClearing_Get(int id)
        {
            int accountId = GetAccountId();
            var list = AccountsView.ViewList();
            //var list = EntityPro.ViewEntityList<AccountsView>(EntityGroups.Enums, AccountsView.TableName, accountId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AccountClearing_Update()
        {
            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {
                if (!IsSignedUser(UserRole.SubAdmin, false)) //(!IsAdmin())
                {
                    throw new Exception("User not alllowed for this method");
                }
                AccountsCreditView v = new AccountsCreditView()
                {
                    PropId = Types.ToInt(Request["PropId"]),
                    //PropName = Request["PropName"],
                    SignupOption = Types.ToInt(Request["SignupOption"]),
                    EnableSignupCredit = Types.ToBool(Request["EnableSignupCredit"], false),
                    CreditTerminal = HttpUtility.UrlDecode(Request["CreditTerminal"]),
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

        #endregion

        #region Payments

        public ActionResult PaymentsQuery()
        {
            return View(true);
        }
        public ActionResult Payments()
        {
            PaymentQuery query = new PaymentQuery(Request.Form);
            return View(true, query);
        }

        public JsonResult GetPaymentsGrid()
        {
            try
            {
                PaymentQuery query = new PaymentQuery(Request);
                var su = GetSignedUser(true);
                query.AccountId = su.AccountId;

                query.Normelize();
                IEnumerable<PaymentsView> list = AccountPaymentContext.ListQueryView(query);
                var row = list.FirstOrDefault<PaymentsView>();
                int totalRows = row == null ? 0 : row.TotalRows;
                return QueryPagerServer<PaymentsView>(list, totalRows, su.UserId);

            }
            catch (Exception ex)
            {
                TraceHelper.Log("Co", "GetPaymentsGrid", ex.Message, Request, -1);
                return null;
            }
        }
        public ActionResult PaymentsExport()
        {
            PaymentQuery query = new PaymentQuery(Request);
            query.AccountId = GetAccountId();
            query.UserId = GetUser();
            query.Normelize();
            var d = AccountPaymentContext.ListQueryDataView(query);
            return CsvActionResult.ExportToCsv(d, "PaymentTargets");

            //return View(query);
        }

        #endregion

        #region Credit

        public ActionResult CreditReport()
        {
            return View(true, new QueryModel() { Layout="_ViewAdmin", Url= "/Admin/GetCreditsGrid" });
        }
        //public ActionResult Payments()
        //{
        //    PaymentQuery query = new PaymentQuery(Request.Form);
        //    return View(true, query);
        //}

        public ContentResult GetCreditsGrid()
        {
            try
            {
                var su = GetSignedUser(UserRole.SubAdmin,true);

                var model=QueryModel.GetModel(Request, "ActionType", "DateFrom", "DateTo");

                var result = AdContext.CreditReportView(model);

                //var result = AdContext.CreditReportJson(model);
               
                return new ContentResult() { Content = result.ToJson() };
            }
            catch (Exception ex)
            {
                TraceHelper.Log("Admin", "GetCreditsGrid", ex.Message, Request, -1);
                return null;
            }
        }
        public ActionResult CreditReportExport()
        {
            var su = GetSignedUser(UserRole.SubAdmin, true);

            var model = QueryModel.GetModel(Request, "ActionType", "DateFrom", "DateTo");
            var dt = AdContext.CreditReportTabel(model);
            return CsvActionResult.ExportToCsv(dt, "CreditReport");

            //return View(query);
        }

        #endregion

        #region Login

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
                return RedirectToAction("Co", "Admin");
            }
        }

        public ActionResult Main()
        {
            return View(true);
        }
        #endregion

    }
}
