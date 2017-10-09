using Nistec.Web.Security;
using Nistec.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Nistec.Web.Cms;
using Nistec.Data;
using Nistec;
using Pro.Data.Entities;
using System.IO;
using Pro.Mvc.Models;
using Pro.Lib;
using Pro.Lib.Upload;
using Pro.Data;
using Nistec.Data.Entities;
using Pro.Data.Entities.Props;
using Pro.Data.Registry;
using Nistec.Runtime;
using System.Text;
using Pro.Lib.Payments;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc.Attributes;
using System.Threading.Tasks;


namespace Pro.Mvc.Controllers
{

    public class RegistryController : Controller
    {
        #region protected methods
        protected string GetReferrer()
        {
            string referer = Request.ServerVariables["HTTP_REFERER"];
            if (string.IsNullOrEmpty(referer))
                return Request.ServerVariables["HTTP_HOST"]; ;
            return referer;
        }
        protected bool ValidateReferrer(string baseUrl = null)
        {
            if (baseUrl == null)
                baseUrl = NetConfig.Get<string>("baseUrl");
            string referer = Request.ServerVariables["HTTP_REFERER"];
            if (string.IsNullOrEmpty(referer) || string.IsNullOrEmpty(baseUrl))
                return false;
            return Nistec.Regx.RegexValidateIgnoreCase(baseUrl, referer);
        }

        protected string GetTemplateContent(string template)
        {
            if (string.IsNullOrEmpty(template))
                return null;
            StringBuilder sb = new StringBuilder();
            //sb.Append("<link rel=\"stylesheet\" href=\"/Templates/" + template + "/html5reset-1.6.1.css\" />");
            sb.Append("<link rel=\"stylesheet\" href=\"/Templates/" + template + "/registry.css\" />");
            sb.Append("<script type=\"text/javascript\" src=\"/Templates/" + template + "/registry.js\"></script>");
            return sb.ToString();
        }


        //protected RegistryPage GetRegistryPage(int AccountId, string folder, string pageType)
        //{
        //    RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "MainEx");
        //    rp.SignKey = Pro.Data.Entities.SignupContext.CreateSignKey(AccountId);
        //    return rp;
        //}
        #endregion
        #region properties

        [HttpPost]
        public JsonResult GetCityView()
        {
            //int accountId = GetAccountId();
            var list = EntityPro.ViewEntityList<CityView>(EntityGroups.Enums, CityView.TableName, 0);
            return Json(list.OrderBy(v => v.PropName).ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetGenderView()
        {
            var list = GenderExtension.GenderList();
            list = list.Where(p => p.PropId !="U").ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region pages
        [HttpGet]
        public ActionResult Index(string folder)
        {
            TraceHelper.Log(folder, "Index", "request", Request);
            RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "Index");
            ViewBag.Title = rp.Title;
            return View(rp);
        }
        [HttpGet]
        public ActionResult About(string folder)
        {
            TraceHelper.Log(folder, "About", "request", Request);
            RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "About");
            ViewBag.Title = rp.Title;
            return View(rp);
        }
        [HttpGet]
        public ActionResult Contact(string folder)
        {
            TraceHelper.Log(folder, "Contact", "request", Request);
            RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "Contact");
            ViewBag.Title = rp.Title;
            return View(rp);
        }
        [HttpGet]
        public ActionResult Subscribe(string folder)
        {
            RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "Main");
            if (!string.IsNullOrEmpty(rp.Design))
            {
                rp.HScript = Types.NZ(rp.HScript, "") + "\n" + GetTemplateContent(rp.Design);
            }
            rp.Ds1 = Pro.Data.Entities.SignupContext.CreateSignKey(rp.AccountId);
            ViewBag.Title = rp.Title;
            return View(rp);
        }
        [HttpGet]
        public ActionResult Signup(string folder)
        {
            TraceHelper.Log(folder, "Signup", "request", Request);
            RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "Main");
            if (!string.IsNullOrEmpty(rp.Design))
            {
                rp.HScript = Types.NZ(rp.HScript, "") + "\n" + GetTemplateContent(rp.Design);
            }
            rp.Ds1 = Pro.Data.Entities.SignupContext.CreateSignKey(rp.AccountId);
            ViewBag.Title = rp.Title;
            return View(rp);
        }
        [HttpGet]
        public ActionResult Custom(string folder)
        {
            TraceHelper.Log(folder, "Custom", "request", Request);
            RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "Main");
            rp.Ds1 = Pro.Data.Entities.SignupContext.CreateSignKey(rp.AccountId);
            ViewBag.Title = rp.Title;
            return View(rp);
        }
        [HttpGet]
        public ActionResult CustomEx(string folder)
        {
            TraceHelper.Log(folder, "CustomEx", "request", Request);
            RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "MainEx");
            rp.Ds1 = Pro.Data.Entities.SignupContext.CreateSignKey(rp.AccountId);
            ViewBag.Title = rp.Title;
            return View(rp);
        }
  
         [HttpGet]
        public ActionResult _Statement(string folder)
         {
             //string pathName = Request["f"];
             RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "Statement");
             ViewBag.Title = rp.Title;
             return PartialView(rp);
         }

         [HttpGet]
         public ActionResult Msg(string folder)
         {
             string message = Request["m"];
             RegistryHead rh = null;
             if (string.IsNullOrEmpty(folder))
             {
                 rh = new RegistryHead();
             }
             else
             {
                 rh = CmsRegistryContext.LoadRegistryHead(folder);
             }

             if (string.IsNullOrEmpty(message))
                 rh.Args = "אירעה שגיאה בלתי צפויה אנא פנה לתמיכה";
             else
                 rh.Args = CmsRegistryContext.LoadRegistryMessageText(rh.AccountId, message);

             return View(rh);
         }

         [HttpGet]
         public ActionResult Err()
         {
             string pathName = Request["f"];
             string message = Request["m"];

             TraceHelper.Log(pathName, "Err", message, Request);

             RegistryHead rh = null;
             if (string.IsNullOrEmpty(pathName))
             {
                 rh = new RegistryHead();
             }
             else
             {
                 rh = CmsRegistryContext.LoadRegistryHead(pathName);
             }

             if (string.IsNullOrEmpty(message))
                 rh.Args = "אירעה שגיאה בלתי צפויה אנא פנה לתמיכה";
             else
                 rh.Args = message;

             return View(rh);
         }

         public ViewResult ErrorNotFound()
         {
             ViewBag.Message = string.Format("{0} : {1}", "שגיאה אירעה ב", Request["aspxerrorpath"]);
             Response.StatusCode = 404;  //you may want to set this to 200
             return View();
         }

         public ViewResult Error()
         {
             ViewBag.Message = string.Format("{0} : {1}", "שגיאה אירעה ב", Request["aspxerrorpath"]);
             Response.StatusCode = 500;  //you may want to set this to 200
             return View();
         }

        #endregion

        #region Info

         [HttpGet]
         public ActionResult Info(string folder)
         {
             TraceHelper.Log(folder, "Info", "request", Request);
             RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "Info");
             ViewBag.Title = rp.Title;
             ViewBag.ErrMessage = ""; 
             return View(rp);
         }

        

        [HttpPost]
         public ActionResult MemberInfo(RegistryPage rp)
        {
            rp = CmsRegistryContext.LoadRegistryPage(rp.Folder, "Info");
            rp.Ds1 = null;
            rp.Ds2 = null;

            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                var memid = Request["MemberId"];
                var guid = Guid.NewGuid().ToString();
                //string token = string.Format("{0}_{1}_{2}", rp.Folder, memid, guid);
                Session.Add("token", guid);
                //return RedirectToAction("MemberInfo", new { folder = rp.Folder, id = Request["MemberId"], token = guid });
                rp.Token = guid;
                
                int recordId = SignupContext.GetMemberRecord(rp.AccountId, memid);

                if (recordId > 0)
                {
                    rp.Id = recordId;
                    rp.Args = recordId.ToString();
                    ViewBag.Title = rp.Title;

                    var minfo = MemberContext.GetInfo(recordId, rp.AccountId); ;
                    EntityDataExtension.EntityResolveJsonFields(minfo);
                    rp.Ds1 = minfo;// Json(minfo).Data;// Newtonsoft.Json.JsonConvert.SerializeObject(minfo);
                    rp.Ds2 = SignupContext.MemberSignupHistory(recordId);

                }
                else
                {
                    rp.Id = 0;
                    ViewBag.Title = "לא נמצאו נתונים";
                }

                //return MemberInfo(rp.Folder, Request["MemberId"], guid);
            }
            else
            {
                rp.Id = 0;
                ViewBag.Title = rp.Title;
                ViewBag.ErrMessage = "שגיאה: אין התאמה, נא לנסות שוב.";
            }
            return View(rp);  
        }

        [HttpPost]
        public JsonResult GetMemberFieldsView(int accid)
        {
            //int accountId = GetAccountId();
            var item = MembersFieldsContext.GetMembersFields(accid);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]//, CaptchaVerify("Captcha is not valid")]
        public JsonResult GetMemberInfo()
        {
            //if (!this.IsCaptchaValid("Captcha is not valid"))
            //{
            //    ViewBag.ErrMessage = "Error: captcha is not valid.";
            //    return Json("", JsonRequestBehavior.AllowGet);
            //}
            string token = Request["token"];
            if (!ValidateToken(token))
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

            int accountId = Types.ToInt(Request["accid"]);
            int recordId = Types.ToInt(Request["id"]);
            //string Id = Request["id"];

            var view = MemberContext.GetInfo(recordId, accountId);
            return Json(view, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetMemberSignupHistory()
        {
            string token = Request["token"];
            if (!ValidateToken(token))
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            int accountId = Types.ToInt(Request["accid"]);
            int recordId = Types.ToInt(Request["rcdid"]);
            var list = SignupContext.MemberSignupHistory(recordId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
         public JsonResult _MemberInfo(string empty)
         {
            
             if (!this.IsCaptchaValid("Captcha is not valid"))
             {
                 ViewBag.ErrMessage = "Error: captcha is not valid.";
                 return Json("", JsonRequestBehavior.AllowGet);
             }

             int accountId = Types.ToInt(Request["accid"]);
             string Id = Request["id"];
             var view = SignupContext.Registry_Info(accountId, Id);
             return Json(view, JsonRequestBehavior.AllowGet);

         }

         protected bool ValidateToken(string token)
         {
             var oses = Session["token"];
             return (oses != null && oses.ToString() == token);

         }
         #endregion

        #region Credit

         public ActionResult Credit(string folder)
         {

             if (!ValidateReferrer())
             {
                 return RedirectToAction("Err", "Registry", new { f = folder, m = "Request not allowed!" });
             }

             TraceHelper.Log(folder, "Credit", "request", Request);

             CreditPage cp = CmsRegistryContext.LoadRegistryPageCredit(folder);

             string req = Request["m"];

             string Src = PaymentDeserilaize(cp.CreditTerminal ,req);

             cp.IframeSrc = Src;

             //int CanPay = 1;// Types.ToInt(Request["CanPay"]);
             //cp.CanPay = CanPay;
             ViewBag.Title = cp.Title;
             return View(cp);
         }

         public ActionResult Confirm(string folder)
         {
             //DEBUG
             if (!ValidateReferrer("https://direct.tranzila.com"))
             {
                 return RedirectToAction("Err", "Registry", new { f = folder, m = "Request not allowed!" });
             }

             bool isValid = false;
             RegistryPage rm = CmsRegistryContext.LoadRegistryPage(folder, "Confirm");
             ViewBag.Title = rm.Title;
             string response = null;// "Response=000&o_tranmode=&trid=50&trBgColor=&expmonth=10&contact=nissim&myid=054649967&email=nissim%40myt.com&currency=1&nologo=1&expyear=17&supplier=#terminal#&sum=1.00&benid=5pb423r0odqe2kcvo40ku1bvm7&o_cred_type=&lang=il&phone=0527464292&ccno=2322&o_npay=&ConfirmationCode=0000000&cardtype=2&cardissuer=6&cardaquirer=6&index=6&Tempref=02570001";
             string clientIp=GetClientIp();

             using (StreamReader sr = new StreamReader(Request.InputStream))
             {
                 response = sr.ReadToEnd();
             }

             TraceHelper.Log(folder, "Confirm", response, Request);

             isValid = (string.IsNullOrEmpty(response)) ? false : true;

             if (isValid)
             {
                 PaymentApi.ExecPaymentReponse(response, clientIp,false);
             }
             else
             {
                 //TODO:
                 rm.Args = "אירעה שגיאה בעת אישור התשלום אנא פנה לתמיכה";
             }

             return View(rm);
         }

         public ActionResult Failure(string folder)
         {

             if (!ValidateReferrer("https://direct.tranzila.com"))
             {
                 return RedirectToAction("Err", "Registry", new { f = folder, m = "Request not allowed!" });
             }

             bool isValid = false;
             RegistryPage rm = CmsRegistryContext.LoadRegistryPage(folder, "Failure");
             ViewBag.Title = rm.Title;
             string response = null;
             //string clientIp = GetClientIp();

             using (StreamReader sr = new StreamReader(Request.InputStream))
             {
                 response = sr.ReadToEnd();
             }

             TraceHelper.Log(folder, "Failure", response, Request);

             isValid = (string.IsNullOrEmpty(response)) ? false : true;

             if (isValid)
             {
                 var payment= PaymentApi.CreatePayment(response);
                 PaymentApi.PaymentFailure(payment);
             }
             //else
             //{
             //    //TODO:
             //    rm.Args = "אירעה שגיאה בעת אי אישור התשלום אנא פנה לתמיכה";
             //}

             return View(rm);
         }
         protected string PaymentSerialize(MemberSignup ms)
         {
             if (ms == null)
             {
                 return null;
             }

             string args = string.Format("trid={0}&sum={1}&contact={2}&email={3}&phone={4}",
                 ms.SignupId.ToString(),
                 ms.Price,
                 HttpUtility.UrlEncode(ms.MemberName),
                 ms.Email,
                 ms.CellPhone);
             return BaseConverter.ToBase32(args);
         }

         // https://direct.tranzila.com/#terminal#/iframe.php?lang=il&currency=1&nologo=1&tranmode=AK&trid=50&sum=1.00&contact=nissim&email=nissim@myt.com&phone=0527464292&trBgColor=#d4e8f1&trTextColor=#000000&trButtonColor=#0076A8" class="payment"></iframe>
         
         protected string PaymentDeserilaize(string iframeUrl, string args)
         {
             if (string.IsNullOrEmpty(args))
             {
                 return null;
             }
             string qs = BaseConverter.FromBase32(args);
             //string iframeUrl = "https://direct.tranzila.com/#terminal#/newiframe.php?lang=il&currency=1&nologo=1&tranmode=AK&#args#&trBgColor=#d4e8f1&trTextColor=#000000&trButtonColor=#0076A8";
             //terminal=
             //args=&trid=50&sum=1.00&contact=nissim&email=nissim@myt.com&phone=0527464292
             string iframeSrc = iframeUrl.Replace("#args#", qs);
             
             return iframeSrc;
         }

         #endregion

         [HttpPost]
         public JsonResult SignupValidity(int AccountId,string MemberId,
            string CellPhone,
            string Email,
            string ExId)
         {

             var res=SignupContext.Member_Signup_Validation(
                AccountId,
                MemberId,
                CellPhone,
                Email,
                ExId);

             string message = CmsRegistryContext.LoadRegistryMessageText(AccountId, res);
             return Json(new ResultModel() { Status = res, Title = "רישום", Message = message});

         }


         //[AcceptVerbs(HttpVerbs.Post)]
         [HttpPost]
         [ValidateAntiForgeryToken()]
         public JsonResult UpdateMemberSignup()
         {

             int res = 0;
             string action = "רישום";
             MemberSignup a = null;
             try
             {

                 a = EntityContext.Create<MemberSignup>(Request.Form);
                 a.RegHostAddress = Request.UserHostAddress;
                 a.RegReferrer = TraceHelper.GetReferrer(Request);
                 a.Price = PaymentContext.LookupItemPrice(a.ItemId);

                 EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת מנוי", "he");
                 if (!validator.IsValid)
                 {
                     return Json(new ResultModel() { Status = -1, Title = action, Message = validator.Result, Link = null, OutputId = 0 });
                 }

                 res = SignupContext.DoSave(a);

                 string message = CmsRegistryContext.LoadRegistryMessageText(a.AccountId, res);
                 string iframeSrc = null;

                 if (res > 0)
                 {
                    iframeSrc = PaymentSerialize(a);
                 }

                 return Json(new ResultModel() { Status = res, Title = action, Message = message, Link = iframeSrc, OutputId = a.SignupId });
             }
             //catch (System.Data.SqlClient.SqlException sex)
             //{
             //    return Json(new ResultModel() { Status = -1, Title = action, Message = ex.Message, Link = null, OutputId = 0 });
             //}
             catch (Exception ex)
             {
                 return Json(new ResultModel() { Status = -1, Title = action, Message = ex.Message, Link = null, OutputId = 0 });
             }
         }

         //[AcceptVerbs(HttpVerbs.Post)]
         [HttpPost]
         [ValidateAntiForgeryToken()]
         public JsonResult UpdateMemberSignupEx()
         {

             int res = 0;
             string action = "רישום";
             MemberSignup a = null;
             try
             {
                 a = EntityContext.Create<MemberSignup>(Request.Form);
                 a.RegHostAddress = Request.UserHostAddress;
                 a.RegReferrer = TraceHelper.GetReferrer(Request);
                 a.Price = PaymentContext.LookupItemPrice(a.ItemId);

                 EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת מנוי", "he");
                 if (!validator.IsValid)
                 {
                     return Json(new ResultModel() { Status = -1, Title = action, Message = validator.Result, Link = null, OutputId = 0 });
                 }

                 res = SignupContext.DoSave(a);

                 string message = CmsRegistryContext.LoadRegistryMessageText(a.AccountId, res);
                 string iframeSrc = null;

                 if (res > 0)
                 {
                     iframeSrc = PaymentSerialize(a);
                 }

                 return Json(new ResultModel() { Status = res, Title = action, Message = message, Link = iframeSrc, OutputId = a.SignupId });
             }
             catch (Exception ex)
             {
                 return Json(new ResultModel() { Status = -1, Title = action, Message = ex.Message, Link = null, OutputId = 0 });
             }
         }

         [HttpPost]
         [ValidateAntiForgeryToken()]
         public JsonResult UpdateMember()
         {
             //MemberCategoryView
             int res = 0;
             string action = "הגדרת מנוי";
             MemberItem a = null;
             try
             {
                 a = EntityContext.Create<MemberItem>(Request.Form);
                
                 EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת מנוי", "he");
                 if (!validator.IsValid)
                 {
                     return Json(new ResultModel() { Status = -1, Title = action, Message = validator.Result});
                 }

                 res = MemberContext.DoSave(a, true, GetCurrentPath(), DataSourceTypes.Register);
                 return Json(new ResultModel() { Status = res, Title = action, Message = "המנוי עודכן בהצלחה", OutputId = a.RecordId });

             }
             catch (Exception ex)
             {
                 return Json(new ResultModel() { Status = -1, Title = action, Message = ex.Message });
             }
         }

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
    }
}
