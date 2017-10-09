#define DEBUG

using Pro.Mvc.Models;
using Nistec;
using Nistec.Web.Cms;
using Nistec.Web.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Pro.Data.Entities;
using Pro.Lib;
using Nistec.Data;
using Nistec.Runtime;
using Pro.Data;
using System.Threading.Tasks;

namespace Pro.Mvc.Controllers
{
    public abstract class BaseController : Controller
    {
 

        #region override
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Request.IsAuthenticated)
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }
        }

        protected SignedUser LoadUserInfo()
        {
            //var signedUser = (SignedUser)ViewBag.UserInfo;
            //if (signedUser != null)
            //    return signedUser;

            var signedUser = SignedUser.Get(Request.RequestContext.HttpContext);
            if (signedUser != null && signedUser.IsAuthenticated && signedUser.IsBlocked == false)
            {
                ViewBag.UserInfo = signedUser;
                
                //ViewBag.UserId = signedUser.UserId;
                ViewBag.UserName = signedUser.UserName;
                ViewBag.UserRole = signedUser.UserRole;
                //ViewBag.AccountId = signedUser.AccountId;
                //ViewBag.DisplayName = signedUser.DisplayName;
                ViewBag.AccountName = signedUser.AccountName;
                //ViewBag.AccountCategory = signedUser.AccountCategory;
                //ViewBag.ParentId = signedUser.ParentId;
                //ViewBag.IsMobile = ismobile;

                return signedUser;
            }

            return null;

            //else
            //{
            //    RedirectToAction("Login", "Account");
            //    return null;
            //    //filterContext.Result = new RedirectResult("~/Account/Login");
            //    //return;
            //}
        }

        protected ActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Account");
        }

        //protected JsonResult JsonResultToLogin()
        //{
        //    return Json(new
        //    {
        //        redirectUrl = Url.Action("Login", "Account"),
        //        isRedirect = true
        //    });
        //}
        protected ActionResult View(SignedUser signedUser)
        {
            if (signedUser == null)
            {
                return RedirectToLogin();
            }
            return base.View();
        }
        protected ActionResult View(SignedUser signedUser, object model)
        {
            if (signedUser == null)
            {
                return RedirectToLogin();
            }
            return base.View(model);
        }
        protected ActionResult View(SignedUser signedUser, string viewName, object model)
        {
            if (signedUser == null)
            {
                return RedirectToLogin();
            }
            return base.View(viewName, model);
        }
        protected ActionResult View(bool loadInfo)
        {
            if (loadInfo)
            {
                var signedUser = LoadUserInfo();
                if (signedUser == null)
                {
                    return RedirectToLogin();
                }
            } 
            return base.View();
        }
        protected ActionResult View(bool loadInfo, object model)
        {
            if (loadInfo)
            {
                var signedUser = LoadUserInfo();
                if (signedUser == null)
                {
                    return RedirectToLogin();
                }
            } 
            return base.View(model);
        }

        protected ActionResult View(bool loadInfo, string viewName, object model)
        {
            if (loadInfo)
            {
                var signedUser = LoadUserInfo();
                if (signedUser == null)
                {
                    return RedirectToLogin();
                }
            }
            return base.View(viewName, model);
        }
        protected ActionResult PartialView(bool loadInfo)
        {
            if (loadInfo)
            {
                var signedUser = LoadUserInfo();
                if (signedUser == null)
                {
                    return RedirectToLogin(); // RedirectResult("~/Account/Login");
                }
            }
            return base.PartialView();
        }
        protected ActionResult PartialView(bool loadInfo, object model)
        {
            if (loadInfo)
            {
                var signedUser = LoadUserInfo();
                if (signedUser == null)
                {
                    return RedirectToLogin();
                }
            }
            return base.PartialView(model);
        }
        protected ActionResult PartialView(bool loadInfo, string viewName, object model)
        {
            if (loadInfo)
            {
                var signedUser = LoadUserInfo();
                if (signedUser == null)
                {
                    return RedirectToLogin();
                }
            }
            return base.PartialView(viewName, model);
        }
        /*
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //HttpContext ctx = HttpContext.Current;
            //if (HttpContext.Current.Session["ID"] == null)
            //{
            //    filterContext.Result = new RedirectResult("~/Home/Login");
            //    return;
            //}
            //base.OnActionExecuting(filterContext);


            if (Request.IsAuthenticated)
            {
                var signedUser = SignedUser.Get(Request.RequestContext.HttpContext);
                if (signedUser != null && signedUser.IsAuthenticated && signedUser.IsBlocked==false)
                {
                    //bool ismobile = DeviceHelper.IsMobile(Request);

                    ViewBag.UserInfo = signedUser;

                    //ViewBag.UserId = signedUser.UserId;
                    //ViewBag.UserName = signedUser.UserName;
                    //ViewBag.UserRole = signedUser.UserRole;
                    //ViewBag.AccountId = signedUser.AccountId;
                    //ViewBag.DisplayName = signedUser.DisplayName;
                    //ViewBag.AccountName = signedUser.AccountName;
                    //ViewBag.AccountCategory = signedUser.AccountCategory;
                    //ViewBag.ParentId = signedUser.ParentId;
                    //ViewBag.IsMobile = ismobile;
                    //ViewBag.UserInfo = new UserModel()
                    //{
                    //    UserId = signedUser.UserId,
                    //    UserName = signedUser.UserName,
                    //    DisplayName = signedUser.DisplayName,
                    //    UserRole = signedUser.UserRole,
                    //    AccountId = signedUser.AccountId,
                    //    AccountName = signedUser.AccountName,
                    //    AccountCategory = signedUser.AccountCategory,
                    //    ParentId = signedUser.ParentId,
                    //    IsMobile = ismobile
                    //};
                    if (signedUser.IsAdmin || signedUser.IsManager)
                    {
                        ViewBag.MngData = signedUser.Data;

                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Account/Login");
                    return;
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }
            //if (Request.UserLanguages != null)
            //{

            //    // Validate culture name
            //    string cultureName = Request.UserLanguages[0]; // obtain it from HTTP header AcceptLanguages
            //    if (!string.IsNullOrEmpty(cultureName))
            //    {
            //        // Modify current thread's culture            
            //        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            //        Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
            //    }
            //}
            base.OnActionExecuting(filterContext);
        }
         */ 
        #endregion

        const string passkey = "PS5467THdgcbrtd90001";

        protected string EncryptPass(string text)
        {
            return Encryption.Encrypt(text, passkey, false);
        }
        protected string DecryptPass(string text)
        {
            return Encryption.Decrypt(text, passkey, false);
        }

        

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        #region signedUser
        //protected string GetSignedUserData()
        //{
        //    var signedUser = SignedUser.Get(Request.RequestContext.HttpContext);
        //    if (signedUser != null && signedUser.IsAuthenticated && signedUser.IsAdmin)
        //    {
        //        return signedUser.Data;
        //    }
        //    return null;
        //}
        //protected void SetSignedUserData(string value)
        //{
        //    var signedUser = SignedUser.Get(Request.RequestContext.HttpContext);
        //    if (signedUser != null && signedUser.IsAuthenticated && signedUser.IsAdmin)
        //    {
        //        signedUser.Data = value;
        //        ViewBag.MngData = value;
        //    }
        //}

        /*
        protected IDictionary<string,object> GetSignedUserData()
        {
            var signedUser = SignedUser.Get(Request.RequestContext.HttpContext);
            if (signedUser != null && signedUser.IsAuthenticated)
            {
                return Nistec.Serialization.JsonSerializer.ToDictionary(signedUser.Data);
            }
            return null;
        }
        protected object GetSignedUserData(string key, string value)
        {
            var signedUser = SignedUser.Get(Request.RequestContext.HttpContext);
            if (signedUser != null && signedUser.IsAuthenticated)
            { 
                object o; 
                IDictionary<string, object> data = Nistec.Serialization.JsonSerializer.ToDictionary(signedUser.Data);
                if (data.TryGetValue(key, out o))
                    return o;
            }
            return null;
        }
        protected void SetSignedUserData(string key,string value)
        {
            var signedUser = SignedUser.Get(Request.RequestContext.HttpContext);
            if (signedUser != null && signedUser.IsAuthenticated)
            {
                IDictionary<string, object> data = Nistec.Serialization.JsonSerializer.ToDictionary(signedUser.Data);
                data[key] = value;

                signedUser.Data = Nistec.Serialization.JsonSerializer.Serialize(data);
            }
        }
        
        protected UserModel GetUserModel()
        {
            var signedUser = SignedUser.Get(Request.RequestContext.HttpContext);
            if (signedUser != null && signedUser.IsAuthenticated)
            {
                 return new UserModel()
                {
                    UserId = signedUser.UserId,
                    UserName = signedUser.UserName,
                    UserRole = signedUser.UserRole,
                    AccountId = signedUser.AccountId
                };
            }
            return null;
        }
         */
        protected SignedUser GetSignedUser()
        {
            //var signedUser = (SignedUser)ViewBag.UserInfo;
            //if (signedUser != null)
            //    return signedUser;

            var signedUser = SignedUser.Get(Request.RequestContext.HttpContext);
            if (signedUser != null && signedUser.IsAuthenticated && signedUser.IsBlocked == false)
            {
               return signedUser;
            }
            //throw new SecurityException(AuthState.UnAuthorized);
            return null;
        }
        protected SignedUser GetAdminSignedUser()
        {
            var signedUser = SignedUser.GetAdmin(Request.RequestContext.HttpContext);
            if (signedUser == null)
            {
                return null;
            }
            if (signedUser.IsBlocked || signedUser.IsAuthenticated == false)
            {
                return null;
            }
            return signedUser;
        }

        protected bool IsLessThenManager()
        {
            var signedUser = GetAdminSignedUser();
            if (signedUser == null)
            {
                return false;
            }
            return signedUser.IsLessThenManager;
        }
        protected bool IsManager()
        {
            var signedUser = GetAdminSignedUser();
            if (signedUser == null)
            {
                return false;
            }
            return signedUser.IsManager;
        }
        protected bool IsAdmin()
        {
            var signedUser = GetAdminSignedUser();
            if (signedUser == null)
            {
                return false;
            }
            return signedUser.IsAdmin;
        }
        protected bool IsAdminOrManager()
        {
            var signedUser = GetAdminSignedUser();
            if (signedUser == null)
            {
                return false;
            }
            return signedUser.IsAdmin || signedUser.IsManager;
        }
        protected bool IsAdminOrMaster()
        {
            var signedUser = GetAdminSignedUser();
            if (signedUser == null)
            {
                return false;
            }
            return signedUser.IsAdmin || signedUser.IsMaster;
        }



        /*
        RoleId	RoleName
        0	User
        1	Super
        5	Manager
        7	Master
        9	Admin
        */

        protected bool ValidateDelete(int userId, string method, bool throwException=true)
        {
            int userRole = GetUserRole();

            if (userRole < 5)
            {
                LogAsync("App", method, string.Format("You have no permission for this action (delete), UserId={0}, UserRole={1}", userId, userRole), Request);
                if (throwException)
                    throw new Exception("You have no permission for this action");
                return false;
            }
            return true;
        }
        protected bool ValidateUpdate(int userId, string method, bool throwException=true)
        {
            int userRole = GetUserRole();
            if (userRole < 5)
            {
                LogAsync("App", method, string.Format("You have no permission for this action (update), UserId={0}, UserRole={1}", userId, userRole), Request);
                if (throwException)
                    throw new Exception("You have no permission for this action");
                return false;
            }
            return true;
        }      

//        protected void ValidateAdmin()
//        {
//            //if(!IsAdmin())
//            //{
//            //    throw new Exception("You have no permission for this method");

//            //}
//#if (!DEBUG)
//            var signedUser = SignedUser.GetAdmin(Request.RequestContext.HttpContext);
//            if (signedUser == null || signedUser.IsAdmin == false)
//            {
//                throw new Exception("You have no permission for this method");
//            }
//#endif
//        }

   
        protected int GetUser()
        {
            var signedUser = GetSignedUser();
            if (signedUser != null)
            {
                //ViewBag.UserName = signedUser.UserName;
                //ViewBag.UserRole = signedUser.UserRole;
                return signedUser.UserId;
            }
            return -1;
        }
        protected int GetUserRole()
        {
            var signedUser = GetSignedUser();
            if (signedUser != null)
            {
                return signedUser.UserRole;
            }
            return -1;
        }
        protected int GetAccountId()
        {
             var signedUser = GetSignedUser();
             if (signedUser != null)
             {
                 //ViewBag.IsVirtual = signedUser.IsVirtual;
                 //ViewBag.ParentId = signedUser.ParentId;
                 return signedUser.AccountId;
             }
             return -1;
        }
        protected int GetParentId()
        {
             var signedUser = GetSignedUser();
             if (signedUser != null)
             {
                 return signedUser.ParentId;
             }
             return -1;
        }
        #endregion

        #region user auth properties
        protected static UserResult GetResult(AuthState state)
        {
            string desc = "";
            switch (state)
            {
                case AuthState.Failed:// = -1,
                    desc = "אירעה שגיאה"; break;
                case AuthState.UnAuthorized:// = 0, //--0=auth faild
                    desc = "פרטי ההזדהות אינם מוכרים במערכת"; break;
                case AuthState.IpNotAlowed:// = 1,//--1=ip not alowed
                    desc = "כתובת השרת אינה מוכרת במערכת"; break;
                case AuthState.EvaluationExpired:// = 2,//--2=Evaluation expired
                    desc = "תוקף תקופת הנסיון הסתיים"; break;
                case AuthState.Blocked:// = 3,//--3=account blocked
                    desc = "משתמש חסום במערכת"; break;
                case AuthState.NonConfirmed:// = 4,//--4=non confirmed, username or password exists
                    desc = "שם משתמש כבר קיים במערכת"; break;
                case AuthState.UserRemoved:// = 5,//user removed
                    desc = "המשתמש הוסר מהמערכת"; break;
                case AuthState.UserNotRemoved:// = 6,//user not removed
                    desc = "המשתמש לא הוסר מהמערכת"; break;
                case AuthState.UserUpdated:// = 7,//user updated
                    desc = "פרטי המשתמש עודכנו במערכת"; break;
                case AuthState.UserNotUpdated:// = 7,//user updated
                    desc = "פרטי המשתמש לא עודכנו במערכת"; break;
                case AuthState.Succeeded:// = 10//--10=ok
                    desc = "Ok"; break;
            }
            return new UserResult() { Status = (int)state, Description = desc };
        }
        protected void SetResult(UserResult ur)
        {
            switch ((AuthState)ur.Status)
            {
                case AuthState.Failed:// = -1,
                    ur.Description = "אירעה שגיאה"; break;
                case AuthState.UnAuthorized:// = 0, //--0=auth faild
                    ur.Description = "פרטי ההזדהות אינם מוכרים במערכת"; break;
                case AuthState.IpNotAlowed:// = 1,//--1=ip not alowed
                    ur.Description = "כתובת השרת אינה מוכרת במערכת"; break;
                case AuthState.EvaluationExpired:// = 2,//--2=Evaluation expired
                    ur.Description = "תוקף תקופת הנסיון הסתיים"; break;
                case AuthState.Blocked:// = 3,//--3=account blocked
                    ur.Description = "משתמש חסום במערכת"; break;
                case AuthState.NonConfirmed:// = 4,//--4=non confirmed, username or password exists
                    ur.Description = "שם משתמש כבר קיים במערכת"; break;
                case AuthState.UserRemoved:// = 5,//user removed
                    ur.Description = "המשתמש הוסר מהמערכת"; break;
                case AuthState.UserNotRemoved:// = 6,//user not removed
                    ur.Description = "המשתמש לא הוסר מהמערכת"; break;
                case AuthState.UserUpdated:// = 7,//user updated
                    ur.Description = "פרטי המשתמש עודכנו במערכת"; break;
                case AuthState.UserNotUpdated:// = 7,//user updated
                    ur.Description = "פרטי המשתמש לא עודכנו במערכת"; break;
                case AuthState.Succeeded:// = 10//--10=ok
                    ur.Description = "Ok"; break;
            }
        }


        #endregion
        
        #region errors
        public ViewResult ErrorNotFound()
        {
            ViewBag.Message = string.Format("{0} : {1}", "שגיאה אירעה ב", Request["aspxerrorpath"]);
            Response.StatusCode = 200;// 404;  //you may want to set this to 200
            return View();
        }
        public ViewResult ErrorUnauthorized()
        {
            ViewBag.Message = string.Format("{0} : {1}", "שגיאה אירעה ב", Request["aspxerrorpath"]);
            Response.StatusCode = 200;// 404;  //you may want to set this to 200
            return View();
        }
        
        protected string GetAllErrors()
        {
            string messages = string.Join("; ", ModelState.Values
                                              .SelectMany(x => x.Errors)
                                              .Select(x => x.ErrorMessage));
            return messages;
        }
        public ViewResult Error()
        {
            ViewBag.Message = string.Format("{0} : {1}", "שגיאה אירעה ב", Request["aspxerrorpath"]);
            Response.StatusCode = 200;// 500;  //you may want to set this to 200
            return View();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            // Bail if we can't do anything; app will crash.
            if (filterContext == null)
                return;
            // since we're handling this, log to elmah
            Exception ex = null;
            //var ex = filterContext.HttpContext.Error.Exception ?? new Exception("No further information exists.");

            if (filterContext.HttpContext != null)
            {
                ex = filterContext.HttpContext.Error;
                if (ex == null)
                {
                    ex = filterContext.Exception;
                    if (ex == null)
                        ex = new Exception("No further information exists.");
                    else if (ex is SecurityException)
                    {
                        filterContext.Result = new RedirectResult("~/Account/Login");
                    }
                }
            }
            else
            {
                ex = filterContext.Exception;
                if (ex == null)
                    ex = new Exception("No further information exists.");
                else if(ex is SecurityException)
                {
                    filterContext.Result = new RedirectResult("~/Account/Login");
                }
            }

            TraceHelper.Log("Application", "OnException", ex.Message + " StackTrace: " + ex.StackTrace, Request, 500);

            filterContext.ExceptionHandled = true;
            //filterContext.Result = new ViewResult()
            //{
            //    ViewName = "Error"
            //};

            //var data = new ErrorPresentation
            //{
            //    ErrorMessage = HttpUtility.HtmlEncode(ex.Message),
            //    TheException = ex,
            //    ShowMessage = !(filterContext.Exception == null),
            //    ShowLink = false
            //};
            //filterContext.Result = View("ErrorPage", data);
        }
        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    base.OnException(filterContext);

        //    Exception e = filterContext.Exception;

        //    //TraceHelper.TraceError(e);

        //    TraceHelper.Log("Application", "OnException", e.Message, Request, 100);

        //    //Log Exception e
        //    //filterContext.ExceptionHandled = true;
        //    //filterContext.Result = new ViewResult()
        //    //{
        //    //    ViewName = "Error"
        //    //};
        //}
        #endregion

        #region Auth
        //protected ActionResult Authenticate(object value)
        //{
        //    var signedUser = SignedUser.Get(Request.RequestContext.HttpContext);
        //    if (signedUser == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    ViewBag.UserName = signedUser.UserName;
        //    ViewBag.UserRole = signedUser.UserRole;
        //    if (value != null)
        //        return View(value);
        //    return View();
        //}

        protected ActionResult AuthenticateAdmin(object value)
        {
            var signedUser = SignedUser.GetAdmin(Request.RequestContext.HttpContext);
            if (signedUser == null || !signedUser.IsAdmin)
            {
                return RedirectToAction("Manager", "Admin");
            }
            ViewBag.UserName = signedUser.UserName;
            if (value != null)
                return View(value);
            return View();
        }
        #endregion

        #region redirect
        protected virtual ActionResult RedirectToIndex(string message)
        {

            ViewBag.Message = message;

            ModelState.AddModelError("ErrorMessage", "The user name or password provided is incorrect.");

            return RedirectToAction("Index", "Home", ModelState);//new { ErrorMessage = message });

        }
        protected virtual ActionResult RedirectToLogin(string message)
        {

            ViewBag.Message = message;

            ModelState.AddModelError("ErrorMessage", "The user name or password provided is incorrect.");

            return RedirectToAction("Login", "Account", ModelState);//new { ErrorMessage = message });

        }
        protected virtual ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Dashboard", "Main");
            }
        }

        protected virtual ActionResult RedirectToStatus(string message)
        {
            return RedirectToAction("StatusMessage", new {msg=message });
        }

        public ViewResult StatusMessage(string msg)
        {
            ViewBag.Message = msg;
            return View();
        }

        protected virtual ActionResult RedirectToFinal(string message)
        {
            return RedirectToAction("Final", "Common", new { m = message });
        }

       
        protected ActionResult GoFinal(string msg, object args)
        {
            TempData["Final"] = args;
            return RedirectToAction("Final", "Common", new { m = msg });
        }

        public ActionResult Final(string m)
        {
            string title = "";
            string message = "";
            var data = TempData["Final"];

            switch (m)
            {
                case "sms-ok":
                    title = "שליחת מסרון";
                    message = (string)data ?? "שליחת הדיוור הסתיימה";
                    break;
                case "mail-ok":
                    title = "שליחת דיוור";
                    message = (string)data ?? "שליחת הדיוור הסתיימה";
                    break;
                case "upload-ok":
                    title = "טעינה";
                    message = "תהליך הטעינה הסתיים הסתיימה";
                    break;
            }
            var model = new ResultModel() { Title = title, Message = message };
            //TempData["resultModel"] = model;
            return View(true,model);
        }

        protected ActionResult GoWarn(string msg, string error)
        {
            TempData["Warn"] = error;
            return RedirectToAction("Warn", "Common", new { m = msg });
        }

        public ActionResult Warn(string m)
        {
            string title = "";
            string message = "";
            //string link = "";
            var data = TempData["Warn"];
            switch (m)
            {
                case "rule-error":
                    title = "הרשאות";
                    message = (string)data ?? "הפעולה שביצעת אינה מורשית אנא פנה לתמיכה";
                    break;
                case "sms-error":
                    title = "שליחת מסרון";
                    message = (string)data??"אירעה שגיאה הדיוור לא נשלח";
                    break;
                case "mail-error":
                    title = "שליחת דיוור";
                    message = (string)data ?? "אירעה שגיאה הדיוור לא נשלח";
                    break;
            }
            var model = new ResultModel() { Title = title, Message = message };
            //TempData["resultModel"] = model;
            return View(true,model);
        }
        #endregion

        #region Async

        [NonAction]
        public async void LogAsync(string folder, string Action, string LogText, HttpRequestBase request, int LogType = 0)
        {
            await Task.Run(() => TraceHelper.Log(folder, Action, LogText, request, LogType));
        }
        [NonAction]
        public async void LogAsync(string folder, string Action, string LogText, string clientIp, string referrer, int LogType = 0)
        {
            await Task.Run(() => TraceHelper.Log(folder, Action, LogText, clientIp, referrer,LogType));
        }

        //[NonAction]
        //public async Task<int> LogAsync(string folder, string Action, string LogText, HttpRequestBase request, int LogType = 0)
        //{
        //    await Task.Run(()=> TraceHelper.Log(folder, Action,  LogText, request, LogType));

        //    return 0;
        //}

        #endregion

        protected ResultModel GetFormResult(int res, string action, string reason, int outputIdentity)
        {
            string title = "";
            string message = "";
            //string buttonTrigger = "<input type=\"button\" id=\"btnTrigger\" value=\"המשך\"/>";
            string link = null;

            if (res > 1) res = 1;

            if (action == null)
            {
                switch (res)
                {
                    case 1: title = "עדכון נתונים"; message = "עודכן בהצלחה"; break;
                    case 0: title = "לא בוצע עדכון"; message = "לא נמצאו נתונים לעדכון"; break;
                    case -1: title = "אירעה שגיאה, לא בוצע עדכון."; message = reason; break;
                }

            }
            else
            {
                switch (res)
                {
                    case 1: title = string.Format("עדכון {0}", action); message = string.Format("{0} עודכן בהצלחה", action); break;
                    case 0: title = string.Format("{0} לא עודכן", action); message = string.Format("לא נמצאו נתונים לעדכון", action); break;
                    case -1: title = string.Format("אירעה שגיאה, {0} לא עודכן.", action); message = reason; break;
                }
            }
            //if (res > 0)
            //{
            //    link = buttonTrigger;
            //}
            var model = new ResultModel() { Status = res, Title = title, Message = message, Link = link, OutputId = outputIdentity };
            return model;
        }


        protected ActionResult PromptResult(string title, string reason)
        {
            var model = new ResultModel() { Title = title, Message = reason };
            TempData["resultModel"] = model;
            return RedirectToAction("_Prompt", "Common");
        }

 

        public ActionResult _Prompt()
        {
            ResultModel model = (ResultModel)TempData["resultModel"];
            return PartialView(model);
        }
        protected void CacheRemove(string key)
        {
            if (key != null)
            {
                HttpContext.Cache.Remove(key);
            }
        }
        protected void CacheAdd(string key, object o, int minutes = 3)
        {
            if (o != null)
            {
                HttpContext.Cache.Add(key, o, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minutes), CacheItemPriority.Normal, null);
            }
        }

        protected object CacheGet(string key)
        {
            if (key==null)
            {
                return null;
            }
            return HttpContext.Cache[key];
        }

        protected JsonResult QueryPager<T>(IEnumerable<T> list, int pagesize, int pagenum)
        {
            int agentId = GetUser();

            if (list == null)
            {
                return null;
            }
            int count = list.Count();
            if (pagesize > 0)
                list = list.Skip(pagesize * pagenum).Take(pagesize);
            var result = new
            {
                AgentId = agentId,
                TotalRows = count,
                Rows = list
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        protected IEnumerable<T> Sort<T>(IEnumerable<T> collection, string sortField, string sortOrder)
        {
            if (sortOrder == "asc")
            {
                collection = collection.OrderBy(c => c.GetType().GetProperty(sortField).GetValue(c, null));
            }
            else
            {
                collection = collection.OrderByDescending(c => c.GetType().GetProperty(sortField).GetValue(c, null));
            }
            return collection;
        }
         protected JsonResult QueryPagerServer<T>(IEnumerable<T> list, int totalRows,int pagesize, int pagenum)
         {
             int agentId = GetUser();

             //if (list == null)
             //{
             //    return null;
             //}
             var result = new
             {
                 AgentId = agentId,
                 TotalRows = totalRows,
                 Rows = list
             };
             return Json(result, JsonRequestBehavior.AllowGet);
         }

         //public ActionResult Export(string _entity, string _sidx, string _sord, string filters)
         //{
         //    string where = "";
         //    if (!string.IsNullOrEmpty(filters))
         //    {
         //        var serializer = new JavaScriptSerializer();
         //        Filters filtersList = serializer.Deserialize<Filters>(filters);
         //        where = filtersList.FilterObjectSet(entity);
         //    }
         //    if (string.IsNullOrEmpty(where))
         //        where = " TRUE ";
         //    Response.ClearContent();
         //    Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
         //    Response.ContentType = "application/excel";
         //    Response.Write(GetAllData(_entity, _sidx, _sord, where));
         //    Response.End();
         //    return View("Index");
         //}

         protected string BuildQuery(System.Collections.Specialized.NameValueCollection query, string prefix)
         {
             string queryString = ""
             + "  SELECT *, ROW_NUMBER() OVER (ORDER BY " + query.GetValues("sortdatafield")[0] + " "
             + query.GetValues("sortorder")[0].ToUpper() + ") as row FROM Customers "
             + " ";
             var filtersCount = int.Parse(query.GetValues("filterscount")[0]);
             var where = "";
             if (filtersCount > 0)
             {
                 where += " WHERE (" + this.BuildFilters(filtersCount, query, prefix);
             }
             queryString += where;
             return queryString;
         }
         protected string BuildFilters(int filtersCount, System.Collections.Specialized.NameValueCollection query, string prefix)
         {
             if (prefix == null)
                 prefix = "";
             var tmpDataField = "";
             var where = "";
             var tmpFilterOperator = "";
             for (var i = 0; i < filtersCount; i += 1)
             {
                 var filterValue = SqlFormatter.ValidateSqlInput(query.GetValues("filtervalue" + i)[0]);
                 var filterCondition = query.GetValues("filtercondition" + i)[0];
                 var filterDataField = prefix + query.GetValues("filterdatafield" + i)[0];
                 var filterOperator = query.GetValues("filteroperator" + i)[0];

                 if (tmpDataField == "")
                 {
                     tmpDataField = filterDataField;
                 }
                 else if (tmpDataField != filterDataField)
                 {
                     where += ") AND (";
                 }
                 else if (tmpDataField == filterDataField)
                 {
                     if (tmpFilterOperator == "")
                     {
                         where += " AND ";
                     }
                     else
                     {
                         where += " OR ";
                     }
                 }
                 // build the "WHERE" clause depending on the filter's condition, value and datafield.
                 where += this.GetFilterCondition(filterCondition, filterDataField, filterValue);
                 if (i == filtersCount - 1)
                 {
                     where += ")";
                 }
                 tmpFilterOperator = filterOperator;
                 tmpDataField = filterDataField;
             }
             return where;
         }
         protected string GetFilterCondition(string filterCondition, string filterDataField, string filterValue)
         {
             switch (filterCondition)
             {
                 case "NOT_EMPTY":
                 case "NOT_NULL":
                     return " " + filterDataField + " NOT LIKE '" + "" + "'";
                 case "EMPTY":
                 case "NULL":
                     return " " + filterDataField + " LIKE '" + "" + "'";
                 case "CONTAINS_CASE_SENSITIVE":
                     return " " + filterDataField + " LIKE '%" + filterValue + "%'" + " COLLATE SQL_Latin1_General_CP1_CS_AS";
                 case "CONTAINS":
                     return " " + filterDataField + " LIKE '%" + filterValue + "%'";
                 case "DOES_NOT_CONTAIN_CASE_SENSITIVE":
                     return " " + filterDataField + " NOT LIKE '%" + filterValue + "%'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                 case "DOES_NOT_CONTAIN":
                     return " " + filterDataField + " NOT LIKE '%" + filterValue + "%'";
                 case "EQUAL_CASE_SENSITIVE":
                     return " " + filterDataField + " = '" + filterValue + "'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                 case "EQUAL":
                     return " " + filterDataField + " = '" + filterValue + "'";
                 case "NOT_EQUAL_CASE_SENSITIVE":
                     return " BINARY " + filterDataField + " <> '" + filterValue + "'";
                 case "NOT_EQUAL":
                     return " " + filterDataField + " <> '" + filterValue + "'";
                 case "GREATER_THAN":
                     return " " + filterDataField + " > '" + filterValue + "'";
                 case "LESS_THAN":
                     return " " + filterDataField + " < '" + filterValue + "'";
                 case "GREATER_THAN_OR_EQUAL":
                     return " " + filterDataField + " >= '" + filterValue + "'";
                 case "LESS_THAN_OR_EQUAL":
                     return " " + filterDataField + " <= '" + filterValue + "'";
                 case "STARTS_WITH_CASE_SENSITIVE":
                     return " " + filterDataField + " LIKE '" + filterValue + "%'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                 case "STARTS_WITH":
                     return " " + filterDataField + " LIKE '" + filterValue + "%'";
                 case "ENDS_WITH_CASE_SENSITIVE":
                     return " " + filterDataField + " LIKE '%" + filterValue + "'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                 case "ENDS_WITH":
                     return " " + filterDataField + " LIKE '%" + filterValue + "'";
             }
             return "";
         }

         protected ContentResult GetJsonResult(string json)
         {
             var content = new ContentResult { Content = json, ContentType = "application/json" };
             return content;
         }
         protected JsonResult RenderJsonResult(object data, JsonRequestBehavior behavior)
         {
             if (!Request.AcceptTypes.Contains("application/json"))
                 return base.Json(data, "text/html", behavior);
             else
                 return base.Json(data, behavior);
         }
    }
}