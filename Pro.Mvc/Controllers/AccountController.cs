using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Pro.Mvc.Filters;
using Pro.Mvc.Models;
using Nistec.Web.Security;
using ProSystem.Data.Entities;
using ProSystem;
using System.Threading.Tasks;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc.Attributes;
using ProAd.Data.Entities;

namespace Pro.Mvc.Controllers
{
    //[Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : Controller
    {

        #region protected methods
        protected string GetReferrer()
        {
            string referer = Request.ServerVariables["HTTP_REFERER"];
            if (string.IsNullOrEmpty(referer))
                return Request.ServerVariables["HTTP_HOST"]; ;
            return referer;
        }
        //protected bool ValidateReferrer(string baseUrl = null)
        //{
        //    if (baseUrl == null)
        //        baseUrl = NetConfig.Get<string>("baseUrl");
        //    string referer = Request.ServerVariables["HTTP_REFERER"];
        //    if (string.IsNullOrEmpty(referer) || string.IsNullOrEmpty(baseUrl))
        //        return false;
        //    return Nistec.Regx.RegexValidateIgnoreCase(baseUrl, referer);
        //}

        protected string GetCurrentPath()
        {
            return Request.Url.AbsolutePath;
        }
        protected string GetClientIp()
        {
            return GetClientIp(Request);
        }

        protected string GetClientIp(HttpRequest request)
        {
            if (request.IsLocal)
                return "127.0.0.1";
            else
                return request.UserHostAddress;
        }
        protected string GetClientIp(HttpRequestBase request)
        {
            if (request.IsLocal)
                return "127.0.0.1";
            else
                return request.UserHostAddress;
        }
        #endregion

        #region Log in/out
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

  
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            var RegHostAddress = Request.UserHostAddress;
            var RegReferrer = TraceHelper.GetReferrer(Request);
            var IsMobile = DeviceHelper.IsMobile(Request);

            //var status = FormsAuth.DoSignIn(model.UserName, model.Password, model.RememberMe, RegHostAddress, RegReferrer, Settings.AppName, IsMobile);

            var status = AdUser.DoSignIn(model.UserName, model.Password, model.RememberMe, RegHostAddress, RegReferrer, Settings.AppName, IsMobile);

            if (status== AuthState.Succeeded)
            {
                //var aduser= AdUser.Get(Request.RequestContext.HttpContext);
                //aduser.LoadDataAndClaims();
                return RedirectToAction("Dashboard", "Common");
            }
            
            string msg = "שם משתמש ו או סיסמה אינם מוכרים במערכת";
            if (status == AuthState.NonConfirmed)
                msg = "סיסמתך פגה וממתינה לאתחול, במידה ולא קיבלת הודעה עם הוראות לאתחול סיסמה  יש לפעול באמצעות שכחתי סיסמה";
            ViewBag.Message = msg;
            //ModelState.AddModelError("ErrorMessage", msg);
            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        public ActionResult LogOff()
        {
            FormsAuth.Instance.SignOut();
            //WebSecurity.Logout();
            //return RedirectToAction("Index", "Home");
            return View();
        }
        
       
        #endregion


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Final(string type, int code)
        {
            FinalModel model = new FinalModel(type, code);
            return View(model);
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register(string folder)
        {
            RegisterModel rm = new RegisterModel()
            {
                Folder = folder,
                AccountId = AdContext.LookupAccountFolder(folder)
            };
            if (rm.AccountId <= 0)
            {
                return RedirectToAction("Final", "Account", new { type = "MembershipStatus", code = (int)MembershipStatus.InvalidAccountPath });
            }
            return View(rm);
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            bool isok = false;
            if (ModelState.IsValid)
            {
                try
                {
                    // WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { EmailId = model.EmailId, Details = model.Details });
                    //WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    //WebSecurity.Login(model.UserName, model.Password);

                    UserProfile user = new UserProfile()
                    {
                        AccountId = model.AccountId,
                        DisplayName = model.DisplayName,
                        Email = model.Email,
                        Phone = model.Phone,
                        UserName = model.UserName,
                        UserRole = 0
                    };
                    var status = Authorizer.Register(user, model.Password);
                    if (status != 0)
                    {
                        ViewBag.Message = StatusDesc.GetMembershipStatus("MembershipStatus", status, ref isok);
                        return View(model);
                    }
                    return RedirectToAction("Login", "Account");
                }
                catch (Exception ex)
                {
                    LogAsync("Account", "Register", ex.Message, Request);
                    return RedirectToAction("Final", "Account", new { type = "MembershipStatus", code = MembershipStatus.Error });
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string token)
        {
            UserProfile up= UserMembership.UserVerificationToken(token);

            ResetPasswordModel rm = new ResetPasswordModel()
            {
                Token=token,
                Email = up.Email,
                AccountId = up.AccountId
            };
            if (rm.AccountId <= 0)
            {
                return RedirectToAction("Final", "Account", new { type = "MembershipStatus", code = (int)MembershipStatus.InvalidAccountPath });
            }
            return View(rm);
        }
      

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool isok = false;
                // Attempt to register the user
                try
                {

                    var status = UserMembership.ResetPassword(model.AccountId, model.Email, model.Password, model.Token);
                    if (status != 20)
                    {
                        ViewBag.Message = StatusDesc.GetMembershipStatus("MembershipStatus", status, ref isok);
                        return View(model);
                    }
                    return RedirectToAction("Final", "Account", new { type = "MembershipStatus", code = status });

                }
                catch (Exception ex)
                {
                    LogAsync("Account", "ResetPassword", ex.Message, Request);
                    return RedirectToAction("Final", "Account", new { type = "MembershipStatus", code = (int)MembershipStatus.Error });
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            bool isok = false;
            try
            {
                if (ModelState.IsValid)
                {

                    //string token = Guid.NewGuid().ToString().Replace("-", "");
                    //string ConfirmUrl = "https://co.my-t.co.il/account/resetpassword?token=" + token;
                    //string AppUrl = "https://co.my-t.co.il/";
                    //string MailSenderFrom = "my-t <co@mytdocs.com>";
                    //string Subject = "Reset your Co password";
                    //string profile_name = "coProfile";
                    //var status = UserMembership.ForgotPassword(model.Email, token, ConfirmUrl, AppUrl, MailSenderFrom, Subject, profile_name);

                    if (this.IsCaptchaValid("Captcha is not valid"))
                    {
                        var status = UserMembership.SendResetToken(model.Email, ProSettings.AppId);
                        if (status != 10)
                        {
                            string msg = StatusDesc.GetMembershipStatus("MembershipStatus", status, ref isok);
                            ViewBag.Message = msg;
                            //ModelState.AddModelError("ErrorMessage", msg);
                            return View(model);
                        }
                        return RedirectToAction("Final", "Account", new { type = "MembershipStatus", code = (int)status });
                    }
                    else
                    {
                        ViewBag.Message = "שגיאה: אין התאמה, נא לנסות שוב.";
                        return View(model);
                    }

                    //string token = Authorizer.GenerateRandomPassword(24);
                    //var callbackUrl = Url.Action("ResetPassword", "Account", new { user = model.Email, code = token }, protocol: Request.Url.Scheme);
                    //var resetLink = "<a href='" + Url.Action("ResetPassword", "Account", new { un = model.Email, rt = token }, "http") + "'>Reset Password</a>";
                    //string subject = "Password Reset Token";
                    //string body = "<b>Please find the Password Reset Token</b><br/>" + resetLink; 


                    //var user = await UserManager.FindByNameAsync(model.Email);
                    //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                    //{
                    //    // Don't reveal that the user does not exist or is not confirmed
                    //    return View("ForgotPasswordConfirmation");
                    //}

                    //string code = UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    //return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception ex)
            {
                LogAsync("Account", "ForgotPassword", ex.Message, Request);
                return RedirectToAction("Final", "Account", new { type = "MembershipStatus", code = (int)MembershipStatus.CouldNotResetPassword });
            }
        }

      

        #region Manage

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        #region External Login
        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfileModel user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfileModel { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }
#endregion

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }



        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
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
            await Task.Run(() => TraceHelper.Log(folder, Action, LogText, clientIp, referrer, LogType));
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
                else if (ex is SecurityException)
                {
                    filterContext.Result = new RedirectResult("~/Account/Login");
                }
            }

            TraceHelper.Log("Application", "OnException", ex.Message + " StackTrace: " + ex.StackTrace, Request, 500);

            filterContext.ExceptionHandled = true;
        }

        #endregion
    }
}
