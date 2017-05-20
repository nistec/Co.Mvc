using Pro.Data;
using Pro.Data.Entities;
using Nistec;
using Nistec.Generic;
using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pro.Lib;
using Pro.Mvc.Models;
using System.Data;
using Pro.Lib.Upload;
using Pro.Lib.Sender;
using Pro.Lib.Query;

namespace Pro.Mvc.Controllers
{
    [Authorize]
    public class SenderController : BaseController
    {

        #region Broadcast

        public ActionResult SmsMultimedia()
        {
            return View();
        }
        public ActionResult SmsBroadcast()
        {
            return View();
        }
        public ActionResult EmailBroadcast()
        {
            return View();
        }

        public ActionResult SmsEditor()
        {
            MemberQuery query = new MemberQuery(Request.Form,21);
            query.AccountId= GetAccountId();
            query.UserId = GetUser();
            //query.QueryType = 21;

            return View(query);
        }

        public ActionResult SendSms()
        {
            ApiResult model = null;
            try
            {
                string Message = Request["Message"];
                string PersonalDisplay = Request["PersonalDisplay"];
                bool isPersonal = string.IsNullOrEmpty(PersonalDisplay) ? false : true;

                int accountId = GetAccountId();
                int uid = GetUser();

                //var list = TargetView.ViewList(accountId, uid);

                //RestApi api = new RestApi(accountId);
                //model = api.SendSms(Message, PersonalDisplay, list, isPersonal);
                
                
                var Acc = AccountProperty.View(accountId);
                var targets = SmsSender.GetTargetList(accountId, uid,isPersonal);
                model=SmsSender.Send(Acc, Message, PersonalDisplay, targets);


                //if (model.Count > 0)
                //{
                //    return GoFinal("sms-ok", model.ToMessage());
                //}
                     return GoFinal("sms-ok", model.ToMessage());
            }
            catch (Exception ex)
            {
                model = ApiResult.Error(ex.Message);
                return GoWarn("sms-error", model.ToMessage());
            }
            //return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendSmsMultimedia()
        {
            ApiResult model = null;
            try
            {
                string Message = Request["Message"];
                string Body = HttpUtility.UrlDecode(Request["Body"]);
                string PersonalDisplay = Request["PersonalDisplay"];
                bool isPersonal = string.IsNullOrEmpty(PersonalDisplay) ? false : true;
                string category = Request["Category"];
                bool isAll = Types.ToBool(Request["IsAll"], false);

                int accountId = GetAccountId();
                int uid = GetUser();
                if (!RuleContext.ValidateRule(accountId, AccountsRules.EnableSms))
                {
                    return GoWarn("rule-error", "");
                }
                var Acc = AccountProperty.View(accountId);
                var targets = SmsSender.GetTargetsByCategory(accountId, isPersonal, category,PersonalDisplay, isAll);
                model = SmsSender.SendMultimedia(Acc, Message, Body, PersonalDisplay, targets);

                return GoFinal("sms-ok", model.ToMessage());
            }
            catch (Exception ex)
            {
                model = ApiResult.Error(ex.Message);
                return GoWarn("sms-error", model.ToMessage());
            }
            //return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SendSmsCategory()
        {
            ApiResult model = null;
            try
            {
                //int accountId = GetAccountId();
                //int uid = GetUser();

                //string Message = Request["Message"];
                //string PersonalDisplay = Request["PersonalDisplay"];
                //bool isPersonal = string.IsNullOrEmpty(PersonalDisplay) ? false : true;
                //string category = Request["Category"];
                //bool isAll = Types.ToBool(Request["allCategory"], false);
                //DateTime TimeToSend = Types.ToDateTime(Request["TimeToSend"],DateTime.Now);
                
                //if (!RuleContext.ValidateRule(accountId, AccountsRules.EnableSms))
                //{
                //    return GoWarn("rule-error", "");
                //}


                //var Acc = AccountProperty.View(accountId);
                //var targets = SmsSender.GetTargetsByCategory(accountId, isPersonal, category, PersonalDisplay, isAll);
                //model = SmsSender.Send(Acc, Message, PersonalDisplay, targets);

                ApiRequest req = new ApiRequest(Request, false);
                model = req.SendSmsGroup();

                return GoFinal("sms-ok", model.ToMessage());
            }
            catch (Exception ex)
            {
                model = ApiResult.Error(ex.Message);
                return GoWarn("sms-error", model.ToMessage());
            }
            //return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendMailCategory()
        {
            ApiResult model = null;
            try
            {
                //string Message = HttpUtility.UrlDecode(Request["Message"]);
                //string Subject = Request["Subject"];
                //string PersonalDisplay = Request["PersonalDisplay"];
                //bool isPersonal = string.IsNullOrEmpty(PersonalDisplay) ? false : true;
                //string category = Request["Category"];
                //bool isAll = Types.ToBool(Request["allCategory"], false);

                //int accountId = GetAccountId();
                //int uid = GetUser();
                //if (!RuleContext.ValidateRule(accountId, AccountsRules.EnableMail))
                //{
                //    return GoWarn("rule-error", "");
                //}
                //var Acc = AccountProperty.View(accountId);
                //var targets = MailSender.GetTargetsByCategory(accountId, isPersonal, category, PersonalDisplay, isAll);
                //model = MailSender.Send(Acc, Message, Subject, PersonalDisplay, targets);

                ApiRequest req = new ApiRequest(Request, true);
                model = req.SendEmailGroup();

                return GoFinal("mail-ok", model.ToMessage());
            }
            catch (Exception ex)
            {
                model = ApiResult.Error(ex.Message);
                return GoWarn("mail-error", model.ToMessage());
            }
            //return Json(model, JsonRequestBehavior.AllowGet);
        }

/*
        public ActionResult SendSmsMultimedia()
        {
            ApiResult model = null;
            try
            {
                string Message = Request["Message"];
                string Body = HttpUtility.UrlDecode(Request["Body"]);
                string PersonalDisplay = Request["PersonalDisplay"];
                bool isPersonal = string.IsNullOrEmpty(PersonalDisplay) ? false : true;
                int category = Types.ToInt(Request["Category"]);
                bool isAll = Types.ToBool(Request["IsAll"], false);

                int accountId = GetAccountId();
                int uid = GetUser();
                if (!RuleContext.ValidateRule(accountId, AccountsRules.EnableSms))
                {
                    return GoWarn("rule-error", "");
                }
                var Acc = AccountProperty.View(accountId);
                var targets = SmsSender.GetTargetsByCategory(accountId, isPersonal, category, isAll);
                model = SmsSender.SendMultimedia(Acc, Message,Body, PersonalDisplay, targets);

                return GoFinal("sms-ok", model.ToMessage());
            }
            catch (Exception ex)
            {
                model = ApiResult.Error(ex.Message);
                return GoWarn("sms-error", model.ToMessage());
            }
            //return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SendSmsCategory()
        {
            ApiResult model = null;
            try
            {
                int accountId = GetAccountId();
                int uid = GetUser();

                string Message = Request["Message"];
                string PersonalDisplay = Request["PersonalDisplay"];
                bool isPersonal = string.IsNullOrEmpty(PersonalDisplay) ? false : true;
                int category = Types.ToInt( Request["Category"]);
                bool isAll = Types.ToBool(Request["allCategory"], false);

                if (!RuleContext.ValidateRule(accountId, AccountsRules.EnableSms))
                {
                    return GoWarn("rule-error", "");
                }


                var Acc = AccountProperty.View(accountId);
                var targets = SmsSender.GetTargetsByCategory(accountId, isPersonal, category, isAll);
                model = SmsSender.Send(Acc, Message, PersonalDisplay, targets);

                return GoFinal("sms-ok", model.ToMessage());
            }
            catch (Exception ex)
            {
                model = ApiResult.Error(ex.Message);
                return GoWarn("sms-error", model.ToMessage());
            }
            //return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SendMailCategory()
        {
            ApiResult model = null;
            try
            {
                string Message = HttpUtility.UrlDecode(Request["Message"]);
                string Subject = Request["Subject"];
                string PersonalDisplay = Request["PersonalDisplay"];
                bool isPersonal = string.IsNullOrEmpty(PersonalDisplay) ? false : true;
                int category = Types.ToInt(Request["Category"]);
                bool isAll = Types.ToBool(Request["allCategory"], false);

                int accountId = GetAccountId();
                int uid = GetUser();
                if (!RuleContext.ValidateRule(accountId, AccountsRules.EnableMail))
                {
                    return GoWarn("rule-error", "");
                }
                var Acc = AccountProperty.View(accountId);
                var targets = MailSender.GetTargetsByCategory(accountId, isPersonal, category, isAll);
                model = MailSender.Send(Acc, Message, Subject, PersonalDisplay, targets);

                return GoFinal("mail-ok", model.ToMessage());
            }
            catch (Exception ex)
            {
                model = ApiResult.Error(ex.Message);
                return GoWarn("mail-error", model.ToMessage());
            }
            //return Json(model, JsonRequestBehavior.AllowGet);
        }

*/
#endregion

        #region Send report

        public ActionResult SendQuery()
        {
            return View();
        }
        public ActionResult SendReport()
        {
            SendReportQuery query = new SendReportQuery(Request);
            query.AuthAccount = AccountProperty.LookupAuthAccount(GetAccountId());
            query.Normelize();
            return View(query);
        }

        public ActionResult SendExport()
        {
            SendReportQuery query = new SendReportQuery(Request);
            query.AuthAccount = AccountProperty.LookupAuthAccount(GetAccountId());
            query.Normelize();
            var d = ReportContext.SendReportData(query.AuthAccount, query.Platform, query.DateFrom.Value, query.DateTo.Value, query.BatchId, query.Target);
            return CsvActionResult.ExportToCsv(d, "SendReport");

            //return View(query);
        }

        public ContentResult GetSendReportGrid()
        {
            SendReportQuery query = new SendReportQuery(Request);

            query.AuthAccount = AccountProperty.LookupAuthAccount(GetAccountId());

            query.Normelize();
            var list = ReportContext.SendReport(query.AuthAccount, query.Platform, query.DateFrom.Value, query.DateTo.Value, query.BatchId, query.Target);
            return base.GetJsonResult(list);
            //var row = list.FirstOrDefault<SignupReportView>();
            //int totalRows = row == null ? 0 : row.TotalRows;
            //return QueryPagerServer<SignupAccountView>(list, totalRows, query.PageSize, query.PageNum);
        }


        #endregion

    }
}
