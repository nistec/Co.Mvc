using Pro.Mvc.Models;
using Nistec.Web.Security;
using Nistec.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Pro.Lib;
using Pro.Data.Entities;
using System.Net.Http;
using System.Text;

namespace Pro.Mvc.Controllers
{
    public class HomeController : Controller
    {
        //[AllowAnonymous]
       

        

        //[HttpPost]
        //public HttpResponseMessage DashboardMembers()
        //{
        //    int accountId = GetAccountId();
        //    var data = ReportContext.Dashbord_Members(accountId);
        //    return new HttpResponseMessage()
        //    {
        //        Content = new StringContent(
        //            data,
        //            Encoding.UTF8,
        //            "text/csv"
        //        )
        //    };
        //}

         //[HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
         [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

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

        /*
         [HttpGet]
         public ActionResult Login()
         {
             ViewBag.Message = Request["msg"];
             return View();
         }

         //[HttpPost]
         //public ActionResult Login(ModelState state)
         //{
         //    if (ModelState.IsValid)
         //    {
         //        if (state.Value..IsValid(user.UserName, user.Password))
         //        {
         //            FormsAuthentication.SetAuthCookie(user.UserName, user.RememberMe);
         //            return RedirectToAction("Index", "Home");
         //        }
         //        else
         //        {
         //            ModelState.AddModelError("", "Login data is incorrect!");
         //        }
         //    }
         //    return View(user);
         //}

        //[HttpPost]
        //public ActionResult Login()
        //{
        //    bool ok = FormsAuth.DoSignIn(Request.Form["username"], Request.Form["password"], Request.Form.Get<bool>("rememberMe"), false);
        //    if (!ok)
        //    {
        //        return RedirectToAction("LoginFailed");
        //    }

        //    //ViewData["username"] = Request.Form["username"];
        //    //ViewData["password"] = Request.Form["password"];
        //    //ViewData["rememberme"] = Request.Form["rememberMe"];
        //    //if (Request.Form["username"] != "admin" || Request.Form["password"] != "admin123")
        //    //{
        //    //    return RedirectToAction("LoginFailed");
        //    //}
        //    return RedirectToAction("Services");
        //}
        // /Widgets/LoginFailed
        public ActionResult LoginFailed()
        {
            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {

            bool ok = FormsAuth.DoSignIn(model.UserName, model.Password, model.RememberMe, false);
            if (ok)
            {
                return RedirectToLocal(returnUrl);
            }

            string msg = "שם משתמש ו או סיסמה אינם נכונים";

            //ModelState.AddModelError("ErrorMessage", "The user name or password provided is incorrect.");
           
            return RedirectToAction("Login", "Home", new { msg = msg});

        }

       
        public ActionResult Logoff()
        {

            FormsAuth.Instance.SignOut();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string Email)
        {
            var user = UserProfile.GetByEmail(Email);
            //check user existance
            //var user = Membership.GetUser(UserName);
            if (user == null || user.IsAuthenticated == false)
            {
                TempData["Message"] = "User Not exist.";
            }
            else
            {
                string UserName = user.UserName;
                //generate password token
                var token = WebSecurity.GeneratePasswordResetToken(UserName);
                //create url with above token
                //var resetLink = "<a href='" + Url.Action("ResetPassword", "Home", new { un = UserName, rt = token }, "http") + "'>Reset Password</a>";
                var resetLink = Url.Action("ResetPassword", "Home", new { un = UserName, rt = token }, "http");
                
                string result = "";
                try
                {
                    result = UserMembership.ForgotPassword(user, resetLink, token);
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
                TempData["Message"] = result;

                
                //get user emailid

                //UsersContext db = new UsersContext();
                //var emailid = (from i in db.UserProfiles
                //               where i.UserName == UserName
                //               select i.EmailId).FirstOrDefault();
                //send mail
                
                //string subject = "Password Reset Token";
                //string body = "<b>Please find the Password Reset Token</b><br/>" + resetLink; 
                //try
                //{

                    
                //    SendEMail(emailid, subject, body);
                //    TempData["Message"] = "Mail Sent.";
                //}
                //catch (Exception ex)
                //{
                //    TempData["Message"] = "Error occured while sending email." + ex.Message;
                //}
                ////only for testing
                //TempData["Message"] = resetLink;
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string email,string resetToken)
        {
            string result = "";
            try
            {
                result = UserMembership.ResetPassword(email, resetToken);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            TempData["Message"] = result;

            return View();
        }
       */
        //public ActionResult Main()
        //{

        //    return View();
        //    //return Authenticate(null);

        //    //var signedUser = SignedUser.Get(Request.RequestContext.HttpContext);
        //    //if (signedUser == null)
        //    //{
        //    //  return  RedirectToAction("Index", "Home");
        //    //}
        //    //ViewBag.UserName = signedUser.UserName;
        //    ////ViewBag.Layout = signedUser.UserRole == 9 ? "~/Views/_ViewAdmin.cshtml" : "~/Views/_View.cshtml"; ;

        //    //return View();
        //}

        //public class EmployeeController : Controller
        //{
        //    private EmployeesDBEntities db = new EmployeesDBEntities();
        //    // GET: /Employee/Details/5

            public ActionResult Details(int? employeeId)
            {
                if (employeeId == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                Employee employee = new Employee() { BirthDate=DateTime.Now, FirstName="nissim", LastName="trg", Title="nis" }; //db.Employees.Find(employeeId);
                if (employee == null)
                {
                    return HttpNotFound();
                }
                return View(employee);
            }

        /*
            [HttpPost]
            public ActionResult Register()
            {
                DateTime maxDate = new DateTime(2015, 1, 1);
                if (Request.Form["birthDateValidate"] != null)
                {
                    var birthDateValidate = DateTime.Parse(Request.Form["birthDateValidate"], System.Globalization.CultureInfo.CurrentCulture);
                    if (birthDateValidate > maxDate)
                    {
                        return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        return new HttpStatusCodeResult(System.Net.HttpStatusCode.Accepted);
                    }
                }
                if (Request.Form["birthDate"] != null)
                {
                    var birthDate = DateTime.Parse(Request.Form["birthDate"], System.Globalization.CultureInfo.CurrentCulture);
                    if (birthDate > maxDate)
                    {
                        return RedirectToAction("RegisterFailed");
                    }
                }
                var terms = Request.Form["acceptTerms"];
                if (terms != null)
                {
                    if (terms == "false")
                    {
                        return RedirectToAction("RegisterFailed");
                    }
                }
                Employee employee = new Employee();
                employee.FirstName = Request.Form["FirstName"];
                employee.LastName = Request.Form["LastName"];
                employee.Title = Request.Form["Title"];
                if (Request.Form["birthDate"] != null)
                {
                    employee.BirthDate = DateTime.Parse(Request.Form["birthDate"]);
                }
                else employee.BirthDate = new DateTime(1900, 1, 1);
                return View(employee);
            }
            public ActionResult RegisterFailed()
            {
                return View();
            }
          */
            //protected override void Dispose(bool disposing)
            //{
            //    if (disposing)
            //    {
            //        db.Dispose();
            //    }
            //    base.Dispose(disposing);
            //}
        //}
       
    }
}
