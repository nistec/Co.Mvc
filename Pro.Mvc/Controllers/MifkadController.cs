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

namespace Pro.Mvc.Controllers
{

    public class MifkadController : Controller
    {
        const string PathName = "mifkad";
        const bool EnableCache=false;

        #region Signup
        RegistryPage GetRegistryMain ()
        {
            RegistryPage rm = CmsRegistryContext.GetRegistryPage(PathName, "Main");
            rm.Decode();
            return rm;
        }

        [HttpGet]
        public ActionResult _Statement()
        {
            var content = CmsRegistryContext.GetRegistryPage(PathName, "Statement");
            return PartialView(content);
        }
        [HttpGet]
        public ActionResult Thanks()
        {
            var content = CmsRegistryContext.GetRegistryPage(PathName, "Thanks");
            return View(content);
        }

        [HttpGet]
        public ActionResult Err()
        {
            var content = CmsRegistryContext.GetRegistryHead(PathName);
            content.Args=Request["Message"];
            return View(content);
        }
        

        [HttpGet]
        public ActionResult Index()
        {
            var content = CmsRegistryContext.GetRegistryPage(PathName, "Index");
            content.Decode();
            return View(content);
        }
        [HttpGet]
        public ActionResult Signup1()
        {
            return View(GetRegistryMain());
        }

        [HttpGet]
        public ActionResult Signup2()
        {
            return View(GetRegistryMain());
        }
 

        #endregion

        #region Properties

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

        #region Credit

        public ActionResult Credit()
        {
            
            CreditPage cp = CmsRegistryContext.LoadRegistryPageCredit(PathName);

            string req = Request["m"];

            string Src = PaymentDeserilaize(req);

            int CanPay = 1;// Types.ToInt(Request["CanPay"]);
            
            cp.IframeSrc = Src;
            cp.CanPay = CanPay;

            return View(cp);
        }

        public ActionResult Confirm()
        {
            int PayId = 0;

            bool isValid = false;
            RegistryPage rm = CmsRegistryContext.LoadRegistryPage(PathName,"Confirm");

            string headers = null;
            GenericArgs args = null;
            string ID = null;
            string contact = null;

            using (StreamReader reader = new StreamReader(Request.InputStream))
            {
                headers = reader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(headers))
                 isValid = false;

            if(isValid)
            {
                args = GenericArgs.ParseQueryString(headers);
                ID = Nistec.Types.NZ(args["myid"], "");
                contact = args["contact"];
                if (string.IsNullOrEmpty(ID))
                {
                    isValid = false;
                }
            }

            if (isValid)
            {
                var pi = new PaymentItem()
                {

                    ID = Nistec.Types.NZ(args["myid"], ""),
                    SignId = Nistec.Types.ToInt(args["uid"], 0),
                    Payed = Nistec.Types.ToDecimal(args["sum"], 0),
                    Contact = contact != null ? HttpUtility.UrlDecode(contact) : "",
                    Email = args["email"],
                    Phone = args["phone"],
                    TransIndex = args["index"],
                    ConfirmationCode = args["ConfirmationCode"],
                    Response = args["Response"],
                    Ccno = args["ccno"]
                };

                Pro.Data.Entities.PaymentContext.PaymentConfirm(pi);
                PayId = pi.PayId;
                rm.Args = "התשלום אושר";
            }
            else
            {
                //TODO:
                rm.Args = "אירעה שגיאה בעת אישור התשלום אנא פנה לתמיכה";
            }

            return View(rm);
        }


        protected string PaymentSerialize(MemberSignup ms)
        {
            if (ms==null)
            {
                return null;
            }

            string args = string.Format("uid={0}&sum={1}&contact={2}&email={3}&phone={4}",
                ms.SignupId.ToString(),
                ms.Price,
                ms.MemberName,
                ms.Email,
                ms.CellPhone);
            return BaseConverter.ToBase32(args);
        }

        protected string PaymentDeserilaize(string args)
        {
            if(string.IsNullOrEmpty(args))
            {
                return null;
            }
            string qs = BaseConverter.FromBase32(args);

            string iframeDesign = "trBgColor=#d4e8f1&trTextColor=#000000&trButtonColor=#0076A8";//&buttonLabel=ביצוע תשלום";
            string iframeUrl = "https://direct.tranzila.com/mytinteractive/iframe.php?lang=il&currency=1&nologo=1";
            string iframeSrc = iframeUrl + "&" + qs + "&" + iframeDesign;
            return iframeSrc;
        }

        //protected PaymentModel GetPaymentModel(MemberSignup ms)
        //{

        //    int CanPay = 0;
        //    string iframeSrc = "";
        //    try
        //    {

        //        //string token = Nistec.Generic.NetConfig.Get<string>("token");
        //        //object o = Session["Token"];
        //        //if (o == null || o.ToString() != token)
        //        //{
        //        //    return new PaymentModel()
        //        //    {
        //        //        CanEdit = 2,
        //        //        IframeSrc = iframeSrc,
        //        //    };
        //        //};

        //        //string baseUrl = Nistec.Generic.NetConfig.Get<string>("baseUrl");
        //        //string referer = Request.ServerVariables["HTTP_REFERER"];
        //        //if (!Nistec.Regx.RegexValidateIgnoreCase(baseUrl, referer))
        //        //{
        //        //    return new PaymentModel()
        //        //    {
        //        //        CanPay = 2,
        //        //        IframeSrc = iframeSrc
        //        //    };
        //        //}


        //        if (ms.SignupId>0)
        //        {
        //            //string iframeDesign = "trBgColor=#d4e8f1&trTextColor=#000000&trButtonColor=#0076A8";//&buttonLabel=ביצוע תשלום";

        //            //iframeSrc = string.Format("{0}?lang=il&currency=1&nologo=1&uid={1}&sum={2}&contact={3}&email={4}&phone={5}&{6}",
        //            //    "https://direct.tranzila.com/mytinteractive/iframe.php",
        //            //    ms.SignupId.ToString(),
        //            //    ms.Price,
        //            //    ms.MemberName,
        //            //    ms.Email,
        //            //    ms.CellPhone,
        //            //    iframeDesign);

        //            iframeSrc = PaymentSerialize(ms);
        //            CanPay = 1;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        CanPay = 0;
        //    }

        //    return new PaymentModel()
        //    {
        //        CanPay = CanPay,
        //        IframeSrc = iframeSrc
        //    };

        //}

        //PaymentModel GetPaymentModel()
        //{
        //    int CanPay = 0;
        //    string Phone = null;
        //    string Email = null;
        //    string Contact = null;
        //    string ID = null;
        //    string iframeSrc = "";
        //    decimal amount = 0;
        //    try
        //    {
        //        //string token = Nistec.Generic.NetConfig.Get<string>("token");
        //        //object o = Session["Token"];
        //        //if (o == null || o.ToString() != token)
        //        //{
        //        //    return new PaymentModel()
        //        //    {
        //        //        CanEdit = 2,
        //        //        IframeSrc = iframeSrc,
        //        //    };
        //        //};

        //        string baseUrl = Nistec.Generic.NetConfig.Get<string>("baseUrl");
        //        string referer = Request.ServerVariables["HTTP_REFERER"];
        //        if (!Nistec.Regx.RegexValidateIgnoreCase(baseUrl, referer))
        //        {
        //            return new PaymentModel()
        //            {
        //                CanPay = 2,
        //                IframeSrc = iframeSrc,
        //            };
        //        }


        //        ID = Nistec.Types.NZ(Request.Form["ID"], null);
        //        //id = (id != "") ? id.TrimStart(new Char[] { '0' }) : "";

        //        //ID = Nistec.Types.ToLong(id, 0);
        //        if (string.IsNullOrEmpty(ID) == false)
        //        {
        //            amount = (decimal)Nistec.Generic.NetConfig.Get<decimal>("amount");
        //            //string Identifier = ID.ToString();
        //            Contact = Request.Form["contact"];
        //            Email = Request.Form["email"];
        //            Phone = Request.Form["phone"];

        //            string iframeDesign = "trBgColor=#d4e8f1&trTextColor=#000000&trButtonColor=#0076A8";//&buttonLabel=ביצוע תשלום";

        //            iframeSrc = string.Format("{0}?lang=il&currency=1&nologo=1&uid={1}&sum={2}&contact={3}&email={4}&phone={5}&{6}",
        //                "https://direct.tranzila.com/mytinteractive/iframe.php",
        //                ID.ToString(),
        //                amount,
        //                Contact,
        //                Email,
        //                Phone,
        //                iframeDesign);

        //            CanPay = 1;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        CanPay = 0;
        //    }
        //    return new PaymentModel()
        //    {
        //        CanPay = CanPay,
        //        IframeSrc = iframeSrc,
        //    };
        //}

        #endregion
        

        [HttpGet]
        public ActionResult Mifkad()
        {
            RegistryPage rm = CmsRegistryContext.GetRegistryPage("Mifkad", "Main");
            //rm.Header = System.Web.HttpUtility.HtmlDecode(rm.Header);

            return View(rm);
        }

        [HttpPost]
        //[AcceptVerbs(HttpVerbs.Post)]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateMemberSignup()
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
                
                EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת מנוי", "he");
                if (!validator.IsValid)
                {
                    return Json(new ResultModel() { Status = -1, Title = action, Message = validator.Result, Link = null, OutputId = 0 });
                }

                res = SignupContext.DoSave(a);
                if (res > 0)
                {
                    //var pm = GetPaymentModel(a);

                    string iframeSrc = PaymentSerialize(a);

                    return Json(new ResultModel() { Status = 1, Title = action, Message = "ok", Link = iframeSrc, OutputId = a.SignupId });

                    //return RedirectToAction("Credit", new { cp = pm.CanPay, src = pm.IframeSrc });
                }

                return Json(new ResultModel() { Status = 0, Title = action, Message = "Member not added", Link = null, OutputId = 0 });

            }
            catch (Exception ex)
            {
                return Json(new ResultModel() { Status = -1, Title = action, Message = ex.Message, Link = null, OutputId = 0 });
            }
        }

 

        // [HttpPost]
        //public ActionResult UpdateMember()
        // {

        //     int res = 0;
        //     string action = "רישום";
        //     MemberSignup a = null;
        //     try
        //     {
        //         string RegHostAddress = Request.UserHostAddress;
        //         string RegReferrer = Request.UrlReferrer.AbsoluteUri;

        //         a = EntityContext.Create<MemberSignup>(Request.Form);
        //         a.RegHostAddress = RegHostAddress;
        //         a.RegReferrer = RegReferrer;
                 

        //         EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת מנוי", "he");
        //         if (!validator.IsValid)
        //         {
        //             return Json(GetFormResult(-1, action, validator.Result, 0), JsonRequestBehavior.AllowGet);

        //         }

        //         res = SignupContext.DoSave(a);
        //         if (res > 0)
        //         {
        //             var pm = GetPaymentModel(a);

        //            return RedirectToAction("Credit", new { cp = pm.CanPay, src = pm.IframeSrc });
        //         }

        //         string message = CmsRegistryContext.GetRegistryMessageText(a.AccountId, res);

        //         return Json(GetFormResult(res, action, message, a.SignupId), JsonRequestBehavior.AllowGet);

        //     }
        //     catch (Exception ex)
        //     {
        //         return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
        //     }
        // }

         //protected ResultModel GetFormResult(int res, string action, string reason, int outputIdentity)
         //{
         //    string title = "";
         //    string message = "";
         //    string buttonTrigger = "<input type=\"button\" id=\"btnTrigger\" value=\"המשך\"/>";
         //    string link = "";

         //    if (res > 1) res = 1;

         //    if (action == null)
         //    {
         //        switch (res)
         //        {
         //            case 1: title = "עדכון נתונים"; message = "עודכן בהצלחה"; break;
         //            case 0: title = "לא בוצע עדכון"; message = "לא נמצאו נתונים לעדכון"; break;
         //            case -1: title = "אירעה שגיאה, לא בוצע עדכון."; message = reason; break;
         //        }

         //    }
         //    else
         //    {
         //        switch (res)
         //        {
         //            case 1: title = string.Format("עדכון {0}", action); message = string.Format("{0} עודכן בהצלחה", action); break;
         //            case 0: title = string.Format("{0} לא עודכן", action); message = string.Format("לא נמצאו נתונים לעדכון", action); break;
         //            case -1: title = string.Format("אירעה שגיאה, {0} לא עודכן.", action); message = reason; break;
         //        }
         //    }
         //    if (res > 0)
         //    {
         //        link = buttonTrigger;
         //    }
         //    var model = new ResultModel() { Status = res, Title = title, Message = message, Link = link, OutputId = outputIdentity };
         //    return model;
         //}

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
    }
}
