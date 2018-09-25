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
using Pro.Upload;
using Pro.Data;
using Nistec.Data.Entities;
using Pro.Data.Entities.Props;
using Pro.Data.Registry;
using Nistec.Runtime;
using System.Text;
using Pro.Lib.Api;
using System.Text.RegularExpressions;

namespace Pro.Mvc.Controllers
{

    public class PreviewController : Controller
    {
        protected string GetReferrer()
        {
            string RegHostAddress = Request.UserHostAddress;
            //string RegReferrer = Request.UrlReferrer.AbsoluteUri;

            return RegHostAddress;// Request.ServerVariables["HTTP_REFERER"];
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

        protected RegistryPage ToPreview(RegistryPage rp, bool ph = false)
        {
            rp.Body = Regex.Replace(rp.Body, "registry", "preview", RegexOptions.IgnoreCase);
            rp.Header = Regex.Replace(rp.Header, "registry", "preview", RegexOptions.IgnoreCase);
            rp.Folder = Regex.Replace(rp.Folder, "registry", "preview", RegexOptions.IgnoreCase);
            rp.HiddenScript = "";
            if (ph)
            {
                rp.Ph1 = Regex.Replace(rp.Ph1, "registry", "preview", RegexOptions.IgnoreCase);
                rp.Ph2 = Regex.Replace(rp.Ph2, "registry", "preview", RegexOptions.IgnoreCase);
                rp.Ph3 = Regex.Replace(rp.Ph3, "registry", "preview", RegexOptions.IgnoreCase);
            }
            return rp;
        }

        #region properties

        [HttpPost]
        public JsonResult GetCityView()
        {
            //int accountId = GetAccountId();
            var list = EntityLibPro.ViewEntityList<CityView>(EntityGroups.Enums, CityView.TableName, 0);
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
            RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "Index");
            ViewBag.Title = rp.Title;
            return View(ToPreview(rp));
        }

        [HttpGet]
        public ActionResult About(string folder)
        {
            RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "About");
            ViewBag.Title = rp.Title;
            return View(ToPreview(rp));
        }
        [HttpGet]
        public ActionResult Contact(string folder)
        {
            RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "Contact");
            ViewBag.Title = rp.Title;
            return View(ToPreview(rp));
        }
        [HttpGet]
        public ActionResult Subscribe(string folder)
        {
            RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "Main");
            if (!string.IsNullOrEmpty(rp.Design))
            {
                rp.HScript = Types.NZ(rp.HScript, "") + "\n" + GetTemplateContent(rp.Design);
            }
            ViewBag.Title = rp.Title;
            return View(ToPreview(rp,true));
        }
        [HttpGet]
        public ActionResult Signup(string folder)
        {
            RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "Main");
            if (!string.IsNullOrEmpty(rp.Design))
            {
                rp.HScript = Types.NZ(rp.HScript, "") + "\n" + GetTemplateContent(rp.Design);
            }
            ViewBag.Title = rp.Title;
            return View(ToPreview(rp,true));
        }
        [HttpGet]
        public ActionResult Custom(string folder)
        {
            RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "Main");
            ViewBag.Title = rp.Title;
            return View(ToPreview(rp,true));
        }
        [HttpGet]
        public ActionResult CustomEx(string folder)
        {
            RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "MainEx");
            ViewBag.Title = rp.Title;
            return View(ToPreview(rp,true));
        }
  
         [HttpGet]
        public ActionResult _Statement(string folder)
         {
             RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "Statement");
             ViewBag.Title = rp.Title;
             return PartialView(ToPreview(rp));
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

        #region Credit

         public ActionResult Credit(string folder)
         {

             CreditPage cp = CmsRegistryContext.LoadRegistryPageCredit(folder);
             string req = Request["m"];
             string Src = PaymentDeserilaize(cp.CreditTerminal ,req);
             cp.IframeSrc = Src;
             ViewBag.Title = cp.Title;
             return View(cp);
         }

         public ActionResult Confirm(string folder)
         {
             RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "Confirm");
             ViewBag.Title = rp.Title;
             return View(ToPreview(rp));
         }

         public ActionResult Failure(string folder)
         {

             RegistryPage rp = CmsRegistryContext.LoadRegistryPage(folder, "Failure");
             ViewBag.Title = rp.Title;
             return View(ToPreview(rp));
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

         protected string PaymentDeserilaize(string iframeUrl, string args)
         {
             if (string.IsNullOrEmpty(args))
             {
                 return null;
             }
             string qs = BaseConverter.FromBase32(args);
             //string iframeUrl = "https://direct.tranzila.com/baityehudi/newiframe.php?lang=il&currency=1&nologo=1&tranmode=AK&#args#&trBgColor=#d4e8f1&trTextColor=#000000&trButtonColor=#0076A8";
             //string successPage = "https://co.my-t.co.il/registry/confirm/mifkad";
             //string failedPage = "https://co.my-t.co.il/registry/failure/mifkad";
             //string creditPage = "https://co.my-t.co.il/registry/credit/mifkad";

             //string iframeDesign = "trBgColor=#d4e8f1&trTextColor=#000000&trButtonColor=#0076A8";//&buttonLabel=ביצוע תשלום";
             //string iframeUrl = "https://direct.tranzila.com/mytinteractive/iframe.php?lang=il&currency=1&nologo=1";
             //string iframeSrc = iframeUrl + "&" + qs +"&" + iframeDesign;
             string iframeSrc = iframeUrl.Replace("#args#", qs);
             
             return iframeSrc;
         }

 
         #endregion

         [HttpPost]
         [ValidateAntiForgeryToken()]
         public JsonResult UpdateMemberSignup()
         {

             int res = 0;
             string action = "רישום";
             MemberSignup a = null;
             try
             {
                 string RegHostAddress = Request.UserHostAddress;
                 string RegReferrer = Request.UrlReferrer.AbsoluteUri;

                 a = EntityContext.Create<MemberSignup>(Request.Form);
                 a.RegHostAddress = RegHostAddress;
                 a.RegReferrer = RegReferrer;
                 a.Price = PaymentContext.LookupItemPrice(a.ItemId);

                 EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת מנוי", "he");
                 if (!validator.IsValid)
                 {
                     return Json(new ResultModel() { Status = -1, Title = action, Message = validator.Result, Link = null, OutputId = 0 });

                 }
                 res = 1;
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

    }
}
