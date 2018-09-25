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
using Pro.Mvc.Models;
using System.Data;
using ProNetcell.Data.Entities;
using Pro.Upload;
using ProNetcell.Query;

namespace Pro.Mvc.Controllers
{
    [Authorize]
    public class NetcellController : BaseController
    {
        [HttpGet]
        public ActionResult NetcellBoard()
        {
            return View(true);
        }
        #region Members query

        [HttpGet]
        public ActionResult Members()
        {
            MemberQuery query = new MemberQuery(Request.QueryString, 11);
            var su = GetSignedUser(false);
            if (su == null)
            {
                return RedirectToLogin();
            }
            query.AccountId = su.AccountId;
            query.UserId = su.UserId;
            query.ExType = su.GetDataValue<int>("ExType");
            //query.QueryType = 1;
            return View(true, query);
        }

        [HttpPost]
        [ActionName("Members")]
        public ActionResult MembersPost()
        {
            MemberQuery query = new MemberQuery(Request.Form, 11);
            var su = GetSignedUser(false);
            if (su == null)
            {
                return RedirectToLogin();
            }
            query.AccountId = su.AccountId;
            query.UserId = su.UserId;
            query.ExType = su.GetDataValue<int>("ExType");
            //query.QueryType = 0;
            return View(true, query);
        }



        [HttpPost]
        public JsonResult GetMembersTargets()
        {
            //int QueryType,
            //int AccountId,
            //int UserId,
            //string MemberId,
            //string ExKey,
            //string CellNumber,
            //string Email,
            //string Name,
            //string Address,
            //string City,
            //string Category,
            ////int Region,
            ////string Place,
            ////string Branch,
            //string ExText1,
            //string ExText2,
            //string ExText3,
            ////int Status,
            //int BirthdayMonth,
            //DateTime? JoinedDateFrom,
            //DateTime? JoinedDateTo,
            //int AgeFrom,
            //int AgeTo)

            ResultModel model = null;
            try
            {

                var su = GetSignedUser(true);

                string key = string.Format("GetTargets_{0}_{1}", su.AccountId, su.UserId);
                CacheRemove(key);

                MemberQuery query = new MemberQuery(Request);
                query.AccountId = su.AccountId;
                query.UserId = su.UserId;

                //MemberQuery query = new MemberQuery()
                //{
                //    AccountId = AccountId,
                //    UserId = UserId,
                //    Address = Address,
                //    AgeFrom = AgeFrom,
                //    AgeTo = AgeTo,
                //    BirthdayMonth = BirthdayMonth,
                //    //Branch = Branch,
                //    Category = Category,
                //    City = City,
                //    //MemberRule = MemberRule,

                //    //JoinedDateFrom = JoinedDateFrom,
                //    //JoinedDateTo = JoinedDateTo,
                //    ////MemberId = MemberId,
                //    //ExKey = ExKey,
                //    //CellNumber = CellNumber,
                //    //Email = Email,
                //    //Name = Name,
                //    //QueryType = QueryType,
                //    //ExField1 = ExText1,
                //    //ExField2 = ExText2,
                //    //ExField3 = ExText3,
                //    PageNum = 0,
                //    PageSize = 999999999
                //};
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
            MemberQuery query = new MemberQuery(Request.Form, 13);
            query.AccountId = GetAccountId();
            query.UserId = GetUser();
            query.QueryType = 13;
            return View(query);
        }

        public ActionResult MembersExport()
        {
            MemberQuery query = new MemberQuery(Request.Form, 12);
            query.AccountId = GetAccountId();
            query.UserId = GetUser();
            query.QueryType = 12;
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
            var list = TargetView.ViewList(accountId, uid);
            return TargetView.ToJson(list, isPrsonal);
        }
        #endregion

        #region Members

        [HttpGet]
        public ActionResult MembersUpload()
        {
            return View(true);
        }

        [HttpGet]
        public ActionResult MembersRemove()
        {
            return View(true);
        }

        [HttpGet]
        public ActionResult MembersAdd()
        {
            return View(true);
        }
        [HttpGet]
        public ActionResult MemberEdit()
        {
            return View(true);
        }
        //[HttpGet]
        //public ActionResult MemberDef(int id, int acctype)
        //{
        //    //var av= AccountView.Get(id);
        //    //return Authenticate(av);
        //    return View();
        //}


        //public ActionResult MembersGrid()
        //{
        //    return View(true);
        //}

        public ActionResult MembersQuery()
        {
            return View(true);
        }

        public ActionResult _MembersQuery()
        {
            return PartialView(true);
        }

        public JsonResult GetMembersGrid()
        {
            try
            {
                MemberQuery query = new MemberQuery(Request);
                query.Normelize();
                var su = GetSignedUser(true);
                query.AccountId = su.AccountId;

                var list = MemberContext.ListQueryView(query);
                //var row = list.FirstOrDefault<MemberListView>();
                //int totalRows = row == null ? 0 : row.TotalRows;
                return QueryPagerServer<MemberListView>(list, su.UserId);//, totalRows);//, query.PageSize, query.PageNum);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, "הצגת אנשי קשר", ex.Message, 0), JsonRequestBehavior.AllowGet);

            }
        }


        #endregion

        #region Member edit

        [HttpGet]
        public ActionResult _MemberAdd()
        {
            return PartialView(true, "_MemberEdit", new EditModel() { Option = "a" });
        }
        [HttpGet]
        public ActionResult _MemberEdit(int id)
        {
            return PartialView(true, "_MemberEdit", new EditModel() { Id = id, Option = "e" });
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
            //MemberCategoryView
            int res = 0;
            string action = "הגדרת מנוי";
            MemberItem a = null;
            ResultModel model = null;
            try
            {
                var usr = GetSignedUser(true);
                a = EntityContext.Create<MemberItem>(Request.Form);
                a.AccountId = usr.AccountId;
                var exType = usr.GetDataValue<int>("ExType");
                EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת מנוי", "he");//, new object[] { "@ExType", exType });
                if (!validator.IsValid)
                {
                    return Json(GetFormResult(-1, action, validator.Result, 0), JsonRequestBehavior.AllowGet);

                }

                res = MemberContext.DoSave(a, true, "", DataSourceTypes.CoSystem);
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
            //MemberCategoryView
            int res = 0;
            string action = "הסרת מנוי";
            MemberItem a = null;
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

        //[HttpPost]
        //public JsonResult GetMemberSignupHistory(int rcdid)
        //{
        //    int accountId = GetAccountId();
        //    var list = SignupContext.MemberSignupHistory(rcdid);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}



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
                int rcdid = Types.ToInt(Request.Form["MemberRecord"]);
                string proptypes = Request.Form["Categories"];
                if (rcdid <= 0)
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


#if (false)
        #region Contacts query

        [HttpGet]
        public ActionResult Contacts()
        {
            ContactQuery query = new ContactQuery(Request.QueryString, 11);
            var su = GetSignedUser(false);
            if (su == null)
            {
                return RedirectToLogin();
            }
            query.AccountId = su.AccountId;
            query.UserId = su.UserId;
            query.ExType = su.GetDataValue<int>("ExType");
            //query.QueryType = 1;
            return View(true, query);
        }

        [HttpPost]
        [ActionName("Contacts")]
        public ActionResult ContactsPost()
        {
            ContactQuery query = new ContactQuery(Request.Form, 11);
            var su = GetSignedUser(false);
            if (su == null)
            {
                return RedirectToLogin();
            }
            query.AccountId = su.AccountId;
            query.UserId = su.UserId;
            query.ExType = su.GetDataValue<int>("ExType");
            //query.QueryType = 0;
            return View(true, query);
        }



        [HttpPost]
        public JsonResult GetContactsTargets(
           int QueryType,
           int AccountId,
           int UserId,
           string ContactId,
           string ExKey,
           string CellNumber,
           string Email,
           string Name,
           string Address,
           string City,
           string Category,
           //int Region,
           //string Place,
           //string Branch,
           string ExText1,
           string ExText2,
           string ExText3,
           //int Status,
           int BirthdayMonth,
           DateTime? JoinedDateFrom,
           DateTime? JoinedDateTo,
           int AgeFrom,
           int AgeTo)
        {
            ResultModel model = null;
            try
            {

                string key = string.Format("GetTargets_{0}_{1}", AccountId, UserId);
                CacheRemove(key);

                ContactQuery query = new ContactQuery()
                {
                    AccountId = AccountId,
                    UserId = UserId,
                    Address = Address,
                    AgeFrom = AgeFrom,
                    AgeTo = AgeTo,
                    BirthdayMonth = BirthdayMonth,
                    //Branch = Branch,
                    Category = Category,
                    City = City,
                    //ContactRule = ContactRule,
                    JoinedDateFrom = JoinedDateFrom,
                    JoinedDateTo = JoinedDateTo,
                    //ContactId = ContactId,
                    ExKey = ExKey,
                    CellNumber = CellNumber,
                    Email = Email,
                    Name = Name,
                    QueryType = QueryType,
                    //Region = Region,
                    ExText1 = ExText1,
                    ExText2 = ExText2,
                    ExText3 = ExText3,
                    //Status = Status,
                    PageNum = 0,
                    PageSize = 999999999
                };
                query.Normelize();
                var totalRows = ContactsContext.ListQueryTargetsView(query);
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
            ContactQuery query = new ContactQuery(Request.Form, 13);
            query.AccountId = GetAccountId();
            query.UserId = GetUser();
            query.QueryType = 13;
            return View(query);
        }

        public ActionResult ContactsExport()
        {
            ContactQuery query = new ContactQuery(Request.Form, 12);
            query.AccountId = GetAccountId();
            query.UserId = GetUser();
            query.QueryType = 12;
            query.Normelize();
            var d = ContactsContext.ListQueryDataView(query);
            return CsvActionResult.ExportToCsv(d, "Targets");

            //return View(query);
        }
        [HttpPost]
        public ActionResult DoContactsExport(string q)
        {
            //ContactQuery query = new ContactQuery(Request.Form);
            //query.AccountId = GetAccountId();
            //query.UserId = GetUser();
            //query.QueryType = 20;
            //query.Normelize();
            if (q != null)
            {
                ContactQuery query = ContactQuery.Deserialize(q);
                ViewBag.ContactQuery = null;
                var d = ContactsContext.ListQueryDataView(query);
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
        #endregion

        #region Contacts

        [HttpGet]
        public ActionResult ContactsUpload()
        {
            return View(true);
        }

        [HttpGet]
        public ActionResult ContactsRemove()
        {
            return View(true);
        }

        [HttpGet]
        public ActionResult ContactsAdd()
        {
            return View(true);
        }
        [HttpGet]
        public ActionResult ContactEdit()
        {
            return View(true);
        }
        //[HttpGet]
        //public ActionResult ContactDef(int id, int acctype)
        //{
        //    //var av= AccountView.Get(id);
        //    //return Authenticate(av);
        //    return View();
        //}


        //public ActionResult ContactsGrid()
        //{
        //    return View(true);
        //}

        public ActionResult ContactsQuery()
        {
            return View(true);
        }

        public ActionResult _ContactsQuery()
        {
            return PartialView(true);
        }

        public JsonResult GetContactsGrid()
        {
            try
            {
                ContactQuery query = new ContactQuery(Request);
                query.Normelize();
                var su = GetSignedUser(true);
                query.AccountId = su.AccountId;

                var list = ContactsContext.ListQueryView(query);
                //var row = list.FirstOrDefault<ContactListView>();
                //int totalRows = row == null ? 0 : row.TotalRows;
                return QueryPagerServer<ContactListView>(list, su.UserId);//, totalRows);//, query.PageSize, query.PageNum);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, "הצגת אנשי קשר", ex.Message, 0), JsonRequestBehavior.AllowGet);

            }
        }


        #endregion

        #region Contact edit

        [HttpGet]
        public ActionResult _ContactAdd()
        {
            return PartialView(true, "_ContactEdit", new EditModel() { Option = "a" });
        }
        [HttpGet]
        public ActionResult _ContactEdit(int id)
        {
            return PartialView(true, "_ContactEdit", new EditModel() { Id = id, Option = "e" });
        }
        [HttpGet]
        public ActionResult _ContactView(int id)
        {
            return PartialView("_ContactEdit", new EditModel() { Id = id, Option = "g" });
        }
        [HttpPost]
        public JsonResult GetContactEdit()
        {
            int id = Types.ToInt(Request["id"]);
            int accountId = GetAccountId();
            var item = ContactsContext.ViewOrNewContactItem(id, accountId);

            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult ContactUpdate()
        {
            //ContactCategoryView
            int res = 0;
            string action = "הגדרת מנוי";
            ContactItem a = null;
            ResultModel model = null;
            try
            {
                var usr = GetSignedUser(true);
                a = EntityContext.Create<ContactItem>(Request.Form);
                a.AccountId = usr.AccountId;
                var exType = usr.GetDataValue<int>("ExType");
                EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת מנוי", "he");//, new object[] { "@ExType", exType });
                if (!validator.IsValid)
                {
                    return Json(GetFormResult(-1, action, validator.Result, 0), JsonRequestBehavior.AllowGet);

                }

                res = ContactsContext.DoSave(a, true, "", DataSourceTypes.CoSystem);
                if (res == -1)
                    model = new ResultModel() { Message = "המנוי קיים במערכת", Status = -1, Title = action };
                else
                    model = GetFormResult(res, action, null, a.ContactId);

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
        public JsonResult ContactDelete()
        {
            //ContactCategoryView
            int res = 0;
            string action = "הסרת מנוי";
            ContactItem a = null;
            try
            {
                int RecordId = Types.ToInt(Request["RecordId"]);
                int accountId = GetAccountId();
                res = ContactsContext.DoDelete(RecordId, accountId);
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

        #region Contacts Nested



        [HttpPost]
        public JsonResult GetContactCategories(int rcdid)
        {
            int accountId = GetAccountId();
            var list = ContactCategoriesView.View(rcdid, accountId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult GetContactSignupHistory(int rcdid)
        //{
        //    int accountId = GetAccountId();
        //    var list = SignupContext.ContactSignupHistory(rcdid);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}



        [HttpPost]
        public JsonResult DeleteContactCategories(int rcdid, int propId)
        {
            int res = 0;
            int accountId = GetAccountId();
            ResultModel model = null;
            try
            {
                res = ContactCategoriesView.DeleteCategory(rcdid, propId, accountId);
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
        public JsonResult UpdateContactCategories()
        {
            int res = 0;
            int accountId = GetAccountId();
            string action = "הגדרת סווג";
            try
            {
                int rcdid = Types.ToInt(Request.Form["ContactRecord"]);
                string proptypes = Request.Form["Categories"];
                if (rcdid <= 0)
                {
                    return Json(GetFormResult(-1, action, "נדרש ת.ז", 0), JsonRequestBehavior.AllowGet);
                }

                res = ContactCategoriesView.AddCategory(rcdid, proptypes, accountId);
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

#endif
    }
}
