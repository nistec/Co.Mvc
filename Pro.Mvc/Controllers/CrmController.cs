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
    public class CrmController : BaseController
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

        #region Members query

        [HttpGet]
        public ActionResult Members()
        {
            MemberQuery query = new MemberQuery(Request.QueryString, 1);
            query.AccountId = GetAccountId();
            query.UserId = GetUser();
            //query.QueryType = 1;
            return View(query);
        }

        [HttpPost]
        [ActionName("Members")]
        public ActionResult MembersPost()
        {
            MemberQuery query = new MemberQuery(Request.Form,0);
            query.AccountId = GetAccountId();
            query.UserId = GetUser();
            //query.QueryType = 0;
            return View(query);
        }

       

        [HttpPost]
        public JsonResult GetMembersTargets(
           int QueryType,
           int AccountId,
           int UserId,
           string MemberId,
           string ExId,
           string CellPhone,
           string Email,
           string Name,
           string Address,
           string City,
           string Category,
           int Region,
           //string Place,
           string Branch,
           int ExEnum1,
           int ExEnum2,
           int ExEnum3,
            //int Status,
           int BirthdayMonth,
           int JoinedFrom,
           int JoinedTo,
           int AgeFrom,
           int AgeTo)
        {
            ResultModel model = null;
            try
            {

                string key = string.Format("GetTargets_{0}_{1}", AccountId, UserId);
                CacheRemove(key);

                 MemberQuery query = new MemberQuery()
                {
                    AccountId = AccountId,
                    UserId=UserId,
                    Address = Address,
                    AgeFrom = AgeFrom,
                    AgeTo = AgeTo,
                    BirthdayMonth = BirthdayMonth,
                    Branch = Branch,
                    Category = Category,
                    City = City,
                    //ContactRule = ContactRule,
                    JoinedFrom = JoinedFrom,
                    JoinedTo = JoinedTo,
                    MemberId = MemberId,
                    ExId=ExId,
                    CellPhone=CellPhone,
                    Email=Email,
                    Name = Name,
                    //Place = Place,
                    QueryType = QueryType,
                    Region = Region,
                    ExEnum1=ExEnum1,
                    ExEnum2=ExEnum2,
                    ExEnum3=ExEnum3,
                    //Status = Status,
                    PageNum = 0,
                    PageSize = 999999999
                };
                query.Normelize();
                var totalRows = MemberContext.ListQueryTargetsView(query);
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
            MemberQuery query = new MemberQuery(Request.Form,22);
            query.AccountId = GetAccountId();
            query.UserId = GetUser();
            query.QueryType = 22;
            return View(query);
        }

        public ActionResult MembersExport()
        {
            MemberQuery query = new MemberQuery(Request.Form,20);
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
            //MemberQuery query = new MemberQuery(Request.Form);
            //query.AccountId = GetAccountId();
            //query.UserId = GetUser();
            //query.QueryType = 20;
            //query.Normelize();
            if (q != null)
            {
                MemberQuery query = MemberQuery.Deserialize(q);
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
            var list= TargetView.ViewList(accountId, uid);
            return TargetView.ToJson(list, isPrsonal);
        }
        #endregion 

        #region Members

        [HttpGet]
        public ActionResult MembersUpload()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MembersRemove()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MembersAdd()
        {
            return View();
        }
        [HttpGet]
        public ActionResult MemberEdit()
        {
            return View();
        }
        [HttpGet]
        public ActionResult MemberDef(int id, int acctype)
        {
            //var av= AccountView.Get(id);
            //return Authenticate(av);
            return View();
        }

        
        public ActionResult MembersGrid()
        {
            return Authenticate(null);
        }

        public ActionResult MembersQuery()
        {
            return View();
        }
        public ActionResult MemberQueryGrid()
        {
            return Authenticate(null);
        }

        public JsonResult GetMembersGrid()
        {
            MemberQuery query = new MemberQuery(Request);
            query.Normelize();
            var list = MemberContext.ListQueryView(query);
            var row = list.FirstOrDefault<MemberListView>();
            int totalRows = row == null ? 0 : row.TotalRows;
            return QueryPagerServer<MemberListView>(list, totalRows, query.PageSize, query.PageNum);
        }


        #endregion

        #region Member edit

        [HttpGet]
        public ActionResult _MemberAdd()
        {
            return PartialView("_MemberEdit", new EditModel() {Option = "a" });
        }
        [HttpGet]
        public ActionResult _MemberEdit(int id)
        {
            return PartialView("_MemberEdit", new EditModel() { Id = id, Option = "e" });
        }
        [HttpGet]
        public ActionResult _MemberView(int id)
        {
            return PartialView("_MemberEdit", new EditModel() { Id = id, Option = "g" });
        }
        [HttpPost]
        public JsonResult GetMemberEdit()
        {
            int id = Types.ToInt(Request["id"]);
            int accountId = GetAccountId();
            var item = MemberContext.ViewOrNewMemberItem(id, accountId);

            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult MemberUpdate()
        {

            int res = 0;
            string action = "הגדרת מנוי";
            MemberCategoryView a = null;
            ResultModel model = null;
            try
            {
                a = EntityContext.Create<MemberCategoryView>(Request.Form);

                EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת מנוי", "he");
                if (!validator.IsValid)
                {
                    return Json(GetFormResult(-1, action, validator.Result, 0), JsonRequestBehavior.AllowGet);

                }

                res = MemberContext.DoSave(a,true,"", DataSourceTypes.CoSystem);
                if (res == -1)
                    model = new ResultModel() { Message = "המנוי קיים במערכת", Status = -1, Title = action };
                else
                    model = GetFormResult(res, action, null, a.RecordId);

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(GetFormResult(-1, action, "אירעה שגיאה, המנוי לא עודכן", 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult MemberDelete()
        {

            int res = 0;
            string action = "הסרת מנוי";
            MemberCategoryView a = null;
            try
            {
                int RecordId = Types.ToInt(Request["RecordId"]);
                int accountId = GetAccountId();
                res = MemberContext.DoDelete(RecordId, accountId);
                string message = "";
                switch (res)
                {
                    case -2:
                        message = "אירעה שגיאה (Invalid Arguments Account) אנא פנה לתמיכה"; break;
                    case -1:
                        message = "המנוי אינו קיים"; break;
                    case 1:
                        message = "המנוי הוסר מרשימת החברים"; break;
                    default:
                        message = "המנוי לא הוסר"; break;
                }

                var model = new ResultModel() { Status = res, Title = "הסרת מנוי", Message = message, Link = null, OutputId = 0 };

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Members Nested



        [HttpPost]
        public JsonResult GetMemberCategories(int rcdid)
        {
            int accountId = GetAccountId();
            var list = MemberCategoriesView.View(rcdid, accountId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

         [HttpPost]
         public JsonResult GetMemberSignupHistory(int rcdid)
        {
            int accountId = GetAccountId();
            var list = SignupContext.MemberSignupHistory(rcdid);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        

         [HttpPost]
         public JsonResult DeleteMemberCategories(int rcdid, int propId)
         {
             int res = 0;
             int accountId = GetAccountId();
             ResultModel model = null;
             try
             {
                 res = MemberCategoriesView.DeleteCategory(rcdid, propId, accountId);
                 model = new ResultModel() { Status = res, Title = "עדכון סווג", Message = "סווג הוסר למנוי " + rcdid, Link = null, OutputId = propId };
             }
             catch (Exception ex)
             {
                 model = new ResultModel() { Status = res, Title = "עדכון סווג", Message = ex.Message, Link = null, OutputId = propId };
             }
             return Json(model, JsonRequestBehavior.AllowGet);
         }

         [HttpPost]
         // [ValidateAntiForgeryToken]
         public JsonResult UpdateMemberCategories()
         {
             int res = 0;
             int accountId = GetAccountId();
             string action = "הגדרת סווג";
             try
             {
                 int rcdid = Types.ToInt( Request.Form["MemberRecord"]);
                 string proptypes = Request.Form["Categories"];
                 if (rcdid<=0)
                 {
                     return Json(GetFormResult(-1, action, "נדרש ת.ז", 0), JsonRequestBehavior.AllowGet);
                 }

                 res = MemberCategoriesView.AddCategory(rcdid, proptypes, accountId);
                 return Json(GetFormResult(res, action, null, 0), JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
             }
         }


         //public ActionResult UploadProc(string uk)
         //{
         //    //Response.AddHeader("Refresh", "30");
         //    return View();//new UploadProcModel() { UploadKey=uk });
         //}

         public ActionResult _UploadProc(string uk)
         {
             return PartialView();
         }

        [HttpPost]
        public ActionResult DoUploadProc(string uploadKey)
        {
            ResultModel model = null;
            try
            {
                if (string.IsNullOrEmpty(uploadKey))
                {
                    return RedirectToFinal("אירעה שגיאה בתהליך הטעינה,לא נמצא מזהה רשימה");
                }

                var mnger = UploadManager.Get(uploadKey);
                var html = mnger.ToHtml();
                model = new ResultModel() { Status = mnger.Status, Message = html, Title = "upload process", Args = uploadKey };

               
            }
            catch (Exception ex)
            {
                var msg = "אירעה שגיאה בתהליך הטעינה " + ex.Message;
                model = new ResultModel() { Status = -1, Message = msg, Title = "upload process", Args = uploadKey };
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Payments

        public ActionResult PaymentsQuery()
        {
            return View();
        }
        public ActionResult PaymentsReport()
        {
            PaymentQuery query = new PaymentQuery(Request.Form);
            query.AccountId = GetAccountId();
            query.UserId = GetUser();
            return View(query);
        }

        public JsonResult GetPaymentsGrid()
        {
            try
            {
                PaymentQuery query = new PaymentQuery(Request);
                query.AccountId = GetAccountId();

                query.Normelize();
                var list = PaymentContext.ListQueryView(query);
                var row = list.FirstOrDefault<PaymentReportView>();
                int totalRows = row == null ? 0 : row.TotalRows;
                return QueryPagerServer<PaymentReportView>(list, totalRows, query.PageSize, query.PageNum);

            }
            catch(Exception ex)
            {
                TraceHelper.Log("Crm", "GetPaymentsGrid", ex.Message, Request, -1);
                return null;
            }
        }
        public ActionResult PaymentsExport()
        {
            PaymentQuery query = new PaymentQuery(Request);
            query.AccountId = GetAccountId();
            query.UserId = GetUser();
            query.Normelize();
            var d = PaymentContext.ListQueryDataView(query);
            return CsvActionResult.ExportToCsv(d, "PaymentTargets");

            //return View(query);
        }

        #endregion

        #region Signup report

        public ActionResult SignupReport()
        {
            SignupQuery query = new SignupQuery(Request.Form, GetAccountId());
            query.UserId = GetUser();
            return View(query);
        }

        public ActionResult SignupQuery()
        {
            return View();
        }

        public ActionResult SignupExport()
        {
            SignupQuery query = new SignupQuery(Request, GetAccountId());
            query.UserId = GetUser();
            query.Normelize();
            var d = SignupContext.ListQueryDataView(query);
            return CsvActionResult.ExportToCsv(d, "SignupTargets");

            //return View(query);
        }

        public JsonResult GetSignupGrid()
        {

            SignupQuery query = new SignupQuery(Request, GetAccountId());

            query.Normelize();
            var list = SignupContext.ListQueryView(query);
            var row = list.FirstOrDefault<SignupReportView>();
            int totalRows = row == null ? 0 : row.TotalRows;
            return QueryPagerServer<SignupAccountView>(list, totalRows, query.PageSize, query.PageNum);
        }


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

        #region Management
        /*
        public ActionResult ManagementGrid()
         {
             return View();
         }

        [HttpPost]
         public JsonResult GetManagementGrid()
         {
             int accountId = GetAccountId();

             int pagesize = Types.ToInt(Request["pagesize"]);
             int pagenum = Types.ToInt(Request["pagenum"]);
             string sortfield = Request["sortdatafield"];
             string sortorder = Request["sortorder"] ?? "asc";

             var filterValue = Request["filtervalue0"];
             var filterCondition = Request["filtercondition0"];
             var filterDataField = Request["filterdatafield0"];
             var filterOperator = Request["filteroperator0"];

             IEnumerable<ManagementListView> list = ManagementListView.ViewList(accountId);
           
             //string key = string.Format("GetInvestmentGrid");

             //IEnumerable<ManagementListView> list = (IEnumerable<ManagementListView>)CacheGet(key);
             //if (list == null)
             //{
             //    list = ManagementListView.ViewList(accountId);
             //    if (list != null)
             //    {
             //        CacheAdd(key, list);
             //    }
             //}
             //if (sortfield != null)
             //    list = Sort<ManagementListView>(list, sortfield, sortorder);

             //if (filterValue != null)
             //{
             //    switch (filterDataField)
             //    {
             //        case "MemberId":
             //            list = list.Where(v => v.MemberId != null && v.MemberId == filterValue); break;
             //        case "MemberName":
             //            list = list.Where(v => v.MemberName != null && v.MemberName.Contains(filterValue)); break;
             //        case "Address":
             //            list = list.Where(v => v.Address != null && v.Address.Contains(filterValue)); break;
             //        case "City":
             //        case "CityName":
             //            list = list.Where(v => v.City != null && v.CityName.Contains(filterValue)); break;

             //    }
             //}

             //return base.QueryPager<ManagementListView>(list, pagesize, pagenum);

             return Json(list, JsonRequestBehavior.AllowGet);
         }
        */

        #endregion

        #region Methods

        #endregion

        #region reports

        [HttpGet]
        public ActionResult ReportQuery()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReportStatistic()
        {
            StatisticReportQuery rm = new StatisticReportQuery(Request.Form, GetAccountId());
            return View(rm);
        }
        [HttpPost]
        public String GetStatisticReport(string report, int campaign, DateTime? dateFrom, DateTime? dateTo)
        {
            StatisticReportQuery rm = new StatisticReportQuery(report);
            rm.AccountId = GetAccountId();
            rm.Campaign = campaign;
            rm.SignupDateFrom = dateFrom;
            rm.SignupDateTo = dateTo;
            var data = ReportContext.DoStatisticReport(rm);
            return data;
        }
        #endregion

    }
}
