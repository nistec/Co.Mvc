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
using Nistec.Runtime;
using ProSystem.Data.Entities;
using Pro.Data.Entities.Props;
using Pro.Data;
using Nistec.Serialization;
using PRO=Pro.Data.Entities;
using ProSystem.Data;
using Nistec.Web.Controls;


namespace Pro.Mvc.Controllers
{
    [Authorize]
    public class SystemController : BaseController
    {
        #region user

        public ActionResult UsersDef()
        {
            return View();
        }
        /*
        public ActionResult DefUser()
        {
            int userId = GetUser();
            //UserView u = UserView.View(userId);
            UserProfile u = UserProfile.Get(userId);
            return View(u);
        }
        [HttpPost]
        public JsonResult DefUserUpdate()
        {

            int res = 0;
            string action = "הגדרת משתמש";
            UserView a = null;
            try
            {
                int userId = GetUser();
                a = EntityContext.Create<UserView>(Request.Form);

                EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת משתמש", "he");
                if (!validator.IsValid)
                {
                    return Json(GetFormResult(-1, action, validator.Result, 0), JsonRequestBehavior.AllowGet);

                }
                a.PropId = userId;

                res = UserView.DoSave(a);
                return Json(GetFormResult(res, action, null, a.PropId), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        */
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
                AccountId = GetAccountId();

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
                UserProfile view = UserProfile.Get(UserId);
                if (IsAdminOrManager() == false)
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
                AccountId = GetAccountId();
                if (IsAdminOrManager() == false)
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
        //    UserResult result = null;
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
        //                PasswordSalt = EncryptPass(Ups),
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

        #region lists

        [HttpPost]
        public JsonResult GetTaskFormTypeList()
        {
            int accountId = GetAccountId();
            return Json(TaskContext<TaskFormType>.GetEntityList("AccountId", accountId), JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public JsonResult GetUsersInTeamList()
        {
            int accountId = GetAccountId();
            int userId = GetUser();
            return Json(AdUserContext.GeUsersInTeam(accountId, userId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetUserTeamList()
        {
            int accountId = GetAccountId();
            int userId = GetUser();
            return Json(UserContext<UserTeamProfile>.GetEntityList("AccountId",accountId,"UserId", userId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetTaskTypeList()
        {
            int accountId = GetAccountId();
            return Json(TaskTypeEntity.ViewList(accountId), JsonRequestBehavior.AllowGet);
            //return Json(EntityProCache.ViewEntityList<TaskTypeEntity>(EntityCacheGroups.Enums, TaskTypeEntity.TableName, accountId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetTaskStatusList()
        {
            return Json(Lists<TaskStatusEntity>.Get_List(), JsonRequestBehavior.AllowGet);
            //return Json(EntityProCache.Get_List<TaskStatusEntity>(TaskStatusEntity.TableName), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetTaskAssignmentList(int rcdid)
        {
            var db = new TaskContext<TaskAssignment>(0);
            return Json(db.GetList(rcdid), JsonRequestBehavior.AllowGet);
            //return Json(TaskContext.Get_TaskAssignments(rcdid), JsonRequestBehavior.AllowGet);
            //return Json(TaskAssignment.GetList(userId, taskid, 1), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetTaskCommentList(int rcdid)
        {
            var db = new TaskContext<TaskComment>(0);
            return Json(db.GetList(rcdid), JsonRequestBehavior.AllowGet);
            //return Json(TaskContext.Get_TaskComments(rcdid), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetTaskTimerList(int rcdid)
        {
            var db = new TaskContext<TaskTimer>(0);
            return Json(db.GetList(rcdid), JsonRequestBehavior.AllowGet);
            //return Json(TaskContext.Get_TaskTimers(rcdid), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetProjectList()
        {
            int accountId = GetAccountId();
            //int userId = GetUser();
            var db = new ProjectContext(accountId);
            return Json(db.GetList("AccountId", accountId), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Lookups

        [HttpPost]
        public JsonResult Lookup_Autocomplete(string type, string search)
        {
            int accountId = GetAccountId();
            var list = DbLookups.Autocomplete(accountId, type, search);
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Lookup_DisplayList(string type)
        {
            int accountId = GetAccountId();
            var list = SystemLookups.DisplayList(accountId, type);
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Lookup_ProjectName(int id)
        {
            int accountId = GetAccountId();
            var value = SystemLookups.Project("ProjectName", "AccountId", accountId, "ProjectId", id);
            return Json(ContentModel.Get(value), JsonRequestBehavior.AllowGet);
        }

    

        #endregion

        #region Ad

        [HttpGet]
        public ActionResult AdDef()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdDefList()
         {
             int accountId = GetAccountId();
             var db = new AdContext<AdItemView>(accountId);
             return Json(db.GetList(accountId), JsonRequestBehavior.AllowGet);
        }
 
        [HttpPost]
         public ActionResult AdShowMembers()
         {
             int accountId = GetAccountId();
             var db = new AdContext<AdItemView>(accountId);
             return Json(db.GetList(accountId), JsonRequestBehavior.AllowGet);
         }

         [HttpPost]
         public ActionResult AdDefUpdate()
        {
            string action = "עדכון קבוצה";
            AdContext<AdItem> context = null;
            try
            {
                ValidateUpdate(GetUser(), "AdDefUpdate");

                int accountId = GetAccountId();
                context = new AdContext<AdItem>(accountId);
                context.Set(Request.Form);
                var res = context.SaveChanges();
                return Json(context.GetFormResult(res, null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AdDefDelete(int id)
        {
            string action = "מחיקת קבוצה";
            AdContext<AdItem> context = null;
            try
            {
                ValidateDelete(GetUser(), "AdDefDelete");

                int accountId = GetAccountId();
                context = new AdContext<AdItem>(accountId);
                var res = context.Delete("GroupId", id, "AccountId", accountId);
                return Json(FormResult.Get(res, context.EntityName,"ok"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AdDefRel(int id)
        {
            int accountId = GetAccountId();
            var db = new AdContext<AdItemRel>(accountId);
            return Json(db.GetList("AccountId", accountId, "GroupId", id), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AdDefRelAll(int id)
        {
            int accountId = GetAccountId();
            var db = new AdContext<AdItemRelAll>(accountId);
            return Json(db.GetList("GroupId", id, "AccountId", accountId, "IsAll", 2), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AdDefRelToAdd(int id)
        {
            int accountId = GetAccountId();
            var db = new AdContext<AdItemRelAll>(accountId);
            return Json(db.GetList("GroupId", id, "AccountId", accountId, "IsAll", 1), JsonRequestBehavior.AllowGet);
        }

         [HttpPost]
         public ActionResult AdDefRelUpdate()
        {
            AdContext<AdItemRel> context = null;
            string action = "הגדרת קבוצות";
            try
            {
                ValidateUpdate(GetUser(), "AdDefRelUpdate");

                int accountId = GetAccountId();
                int groupid = Types.ToInt(Request["GroupId"]);
                string users = Request["Users"];
                if (groupid > 0 && !string.IsNullOrEmpty(users))
                {
                    context = new AdContext<AdItemRel>(accountId);
                    //@Mode tinyint=0--0= insert,1=upsert,2=delete
                    var model = context.Upsert(UpsertType.Update, ReturnValueType.ReturnValue,"GroupId", groupid, "AccountId", accountId, "Users", users, "Mode", 0);//.Update
                    return Json(context.GetFormResult(model, null), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(FormResult.Get(0, action, null, 0), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AdDefRelDelete()
        {
            string action = "הסרת משתמש מקבוצה";
            AdContext<AdItemRel> context = null;
            int res = 0;
            try
            {
                ValidateDelete(GetUser(), "AdDefRelDelete");
                int groupid = Types.ToInt(Request["GroupId"]);
                int userid = Types.ToInt(Request["UserId"]);
                if (groupid > 0 && userid > 0)
                {
                    int accountId = GetAccountId();
                    context = new AdContext<AdItemRel>(accountId);
                    res = context.Delete("GroupId", groupid, "AccountId", accountId, "UserId", userid);
                }
                return Json(FormResult.Get(res, context.EntityName, "ok"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetError(action, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region AdUsers

        [HttpGet]
        public ActionResult AdUsersDef()
        {
            return View();
        }
  

        [HttpPost]
        public ActionResult AdUserDefList()
        {
            int accountId = GetAccountId();
            int userId = GetUser();
            var db = new AdContext<AdUserProfile>(accountId);
            var list = db.GetList("AccountId", accountId, "UserId", userId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AdUserDefUpdate()
        {
            string action = "עדכון משתמש";
            AdContext<AdUserProfile> context = null;
            try
            {
                ValidateUpdate(GetUser(), "AdUserDefUpdate");
                bool isResetPass = Types.ToBool(Request.Form["IsResetPass"], false);
                int accountId = GetAccountId();
                context = new AdContext<AdUserProfile>(accountId);
                context.Set(Request.Form);
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
                ValidateUpdate(GetUser(), "AdUserDefInsert");

                int accountId = GetAccountId();
                context = new AdContext<AdUserProfile>(accountId);
                context.Set(Request.Form);
                context.Validate(UpdateCommandType.Insert);
                var cur=context.Current;
                var res = context.Upsert(UpsertType.Insert,ReturnValueType.ReturnValue , "DisplayName", cur.DisplayName,
                    "Email", cur.Email,
                    "Phone", cur.Phone,
                    "UserName", cur.UserName,
                    "UserRole", cur.UserRole,
                    "AccountId", accountId,
                    "Lang", cur.Lang,
                    "Evaluation", cur.Evaluation,
                    "IsBlocked", cur.IsBlocked,
                    "Password", Guid.NewGuid().ToString().Substring(0, 8),
                    "Profession", 0,
                    "AppId", ProSettings.AppId);
                int status = res.GetReturnValue();
                bool isok=false;
                var Message = StatusDesc.GetMembershipStatus("MembershipStatus", status, ref isok);
                if (!isok)
                {
                    return Json(FormResult.Get(-1, action, Message), JsonRequestBehavior.AllowGet);
                }
                return Json(FormResult.Get(1, action, Message), JsonRequestBehavior.AllowGet);
                //return Json(context.GetFormResult(res, Message), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AdUserDefDelete(int id)
        {
            string action = "מחיקת משתמש";
            AdContext<AdUserProfile> context = null;
            try
            {
                ValidateDelete(GetUser(), "AdUserDefDelete");

                int accountId = GetAccountId();
                context = new AdContext<AdUserProfile>(accountId);
                var res = context.Delete("UserId", id, "AccountId", accountId);
                return Json(FormResult.Get(res, context.EntityName, "ok"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region AdTeam

        [HttpGet]
        public ActionResult AdTeamDef()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdTeamDefList()
        {
            int accountId = GetAccountId();
            var db = new AdContext<AdTeamItemView>(accountId);
            return Json(db.GetList(accountId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AdTeamShowMembers(int id)
        {
            int accountId = GetAccountId();
            var db = new AdContext<AdTeamItemView>(accountId);
            return Json(db.GetList(accountId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AdTeamDefUpdate()
        {
            string action = "עדכון צוותים";
            AdContext<AdTeamItem> context = null;
            try
            {
                ValidateUpdate(GetUser(), "AdTeamDefUpdate");

                int accountId = GetAccountId();
                context = new AdContext<AdTeamItem>(accountId);
                context.Set(Request.Form);
                var res = context.SaveChanges();
                return Json(context.GetFormResult(res, null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AdTeamDefDelete(int id)
        {
            string action = "מחיקת צוות";
            AdContext<AdTeamItem> context = null;
            try
            {
                ValidateDelete(GetUser(), "AdTeamDefDelete");

                int accountId = GetAccountId();
                context = new AdContext<AdTeamItem>(accountId);
                var res = context.Delete("TeamId", id, "AccountId", accountId);
                return Json(FormResult.Get(res, context.EntityName, "ok"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AdTeamDefRel(int id)
        {
            int accountId = GetAccountId();
            var db = new AdContext<AdTeamItemRel>(accountId);
            return Json(db.GetList("AccountId", accountId, "TeamId", id), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AdTeamDefRelAll(int id)
        {
            int accountId = GetAccountId();
            var db = new AdContext<AdTeamItemRelAll>(accountId);
            return Json(db.GetList("TeamId", id, "AccountId", accountId, "IsAll", 2), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AdTeamDefRelToAdd(int id)
        {
            int accountId = GetAccountId();
            var db = new AdContext<AdTeamItemRelAll>(accountId);
            return Json(db.GetList("TeamId", id, "AccountId", accountId, "IsAll", 1), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AdTeamDefRelUpdate()
        {
            string action = "הגדרת צוותים";
            AdContext<AdTeamItemRel> context = null;
            try
            {
                ValidateUpdate(GetUser(), "AdTeamDefRelUpdate");

                int accountId = GetAccountId();
                int groupid = Types.ToInt(Request["TeamId"]);
                string users = Request["Users"];
                if (groupid > 0 && !string.IsNullOrEmpty(users))
                {
                    context = new AdContext<AdTeamItemRel>(accountId);
                    //@Mode tinyint=0--0= insert,1=upsert,2=delete
                    var model = context.Upsert(UpsertType.Update,ReturnValueType.ReturnValue , "TeamId", groupid, "AccountId", accountId, "Users", users, "Mode", 0);//Update
                    return Json(context.GetFormResult(model, null), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(FormResult.Get(0, action, null), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AdTeamDefRelDelete()
        {
            string action = "מחיקת משתמשים מצוות";
            AdContext<AdTeamItemRel> context = null;
            int res = 0;
            try
            {
                ValidateDelete(GetUser(), "AdTeamDefRelDelete");
                int groupid = Types.ToInt(Request["TeamId"]);
                int userid = Types.ToInt(Request["UserId"]);
                if (groupid > 0 && userid > 0)
                {
                    int accountId = GetAccountId();
                    context = new AdContext<AdTeamItemRel>(accountId);
                    res = context.Delete("TeamId", groupid, "AccountId", accountId, "UserId", userid);
                }
                return Json(FormResult.Get(res, context.EntityName, "ok"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region task



        [HttpGet]
        public ActionResult Tasks()
        {
            TaskQuery model = new TaskQuery()
            {
                AccountId = GetAccountId(),
                UserId = GetUser()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult GetTasksGrid()
        {
            TaskQuery query = new TaskQuery(Request);
            var list = TaskContext.ViewTasks(query.AccountId, query.UserId, query.AssignBy,query.TaskStatus);
            //var row = list.FirstOrDefault<MemberListView>();
            //int totalRows = row == null ? 0 : row.TotalRows;
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTaskInfo(int id)
        {
            var view = TaskContext.Get(id);
            string title = "פרטים";
            var model = new InfoModel() { Id = id, Title = title, Value = view.ToHtml() };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TaskUser()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult TaskUserKanban()
        {
            int status = Types.ToInt(Request["Status"]);
            bool isshare = Types.ToBool(Request["IsShare"],false);
            int accountId = GetAccountId();
            int userId = GetUser();
            var view = TaskContext.TaskUserKanban(accountId, userId, status, isshare);
            return Json(view, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult TaskEdit(int id)
        {
            return View(new EditTaskModel() { PId = id, Option = "e" });
        }
        [HttpGet]
        public ActionResult TaskInfo(int id)
        {
            return View("TaskEdit", new EditTaskModel() { PId = id, Option = "g" });
        }
        [HttpGet]
        public ActionResult TaskStart(int id)
        {
            int accountId = GetAccountId();
            int user = GetUser();
            TaskContext.Task_Status_Change(id, user, 2, "אתחול משימה", null);
            return View("TaskEdit", new EditTaskModel() { PId = id, Option = "e" });
        }
        [HttpGet]
        public ActionResult _TaskEdit(Guid id)
        {
            return PartialView();
        }

        [HttpPost]
        public ContentResult GetTaskEdit()
        {
            int id = Types.ToInt(Request["id"]);
            int accountId = GetAccountId();
            string json = "";
            TaskItem item = null;
            if (id > 0)
            {
                item = TaskContext.Get(id);//, accountId);
            }
            else
            {
                item = new TaskItem() { AccountId = accountId };// DbSystem.SysCounters(2) };
            }
            if (item != null)
                json = JsonSerializer.Serialize(item);

            return base.GetJsonResult(json);

        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult TaskChangeState()
        {
            string action = "שינוי סטאטוס למשימה";
            try
            {
                int newState = Types.ToInt(Request["newstate"]);
                int oldState = Types.ToInt(Request["oldtate"]);
                int itemId = Types.ToInt(Request["itemid"]);
                int accountId = GetAccountId();
                int user = GetUser();
                int res = TaskContext.Task_Status_Change(itemId, user, newState, action, null);
                return Json(FormResult.Get(res, action, SystemStatusMessage.Get(res)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult TaskCompleted()
        {
            string action = "סיום משימה";
            try
            {
                var item = EntityContext.Create<TaskItem>(Request.Form);
                var result = TaskItemUpdate(item, action);
                if (result.Status > 0)
                {
                    int accountId = GetAccountId();
                    int user = GetUser();
                    int res = TaskContext.Task_Status_Change(item.TaskId, user, 16, "סיום משימה", null);
                    if(res>0)
                    {
                        return Json(FormResult.Get(res, action), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        result.Status=res;
                        result.Message = SystemStatusMessage.Get(res);
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                //int id = Types.ToInt(Request["TaskId"]);
                //int accountId = GetAccountId();
                //int user = GetUser();
                //int res = TaskContext.Task_Status_Change(id, user, 16, "סיום משימה", null);
                //return Json(FormResult.Get(res, action), JsonRequestBehavior.AllowGet);
                //return View("TaskEdit", new EditTaskModel() { PId = id, Option = "e" });
                //return Json(GetFormResult(res, action, null,0), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

  

        protected FormResult TaskItemUpdate(TaskItem item,string action)
        {
            item.AccountId = item.AccountId == 0 ? GetAccountId() : item.AccountId;
            item.UserId = item.UserId == 0 ? GetUser() : item.UserId;
            EntityValidator validator = EntityValidator.ValidateEntity(item, "הגדרת מנוי", "he");
            if (!validator.IsValid)
            {
                return new FormResult()
                {
                    Message = validator.Result,
                    Status = -1,
                    Title = action
                };

            }
            int user = GetUser();
            var res = TaskContext.DoUpdate(item);
            return new FormResult(res)
            {
                OutputId = (res > 0) ? item.TaskId : 0,
                Title = action
            };
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult UpdateTask()
        {

            //int res = 0;
            string action = "הגדרת משימה";
            TaskItem a = null;
            try
            {

                a = EntityContext.Create<TaskItem>(Request.Form);
                var result = TaskItemUpdate(a, action);
                return Json(result, JsonRequestBehavior.AllowGet);

                //a.AccountId = a.AccountId == 0 ? GetAccountId() : a.AccountId;
                //a.UserId =a.UserId ==0? GetUser():a.UserId;
                //EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת מנוי", "he");
                //if (!validator.IsValid)
                //{
                //    return Json(GetFormResult(-1, action, validator.Result, 0), JsonRequestBehavior.AllowGet);

                //}
                //int user = GetUser();
                ////if (a.TaskStatus > 1 && a.TaskStatus < 8)
                ////    res = TaskContext.Task_Status_Change(a.TaskId, user, 16, "סיום משימה", null);
                ////else
                //    res = TaskContext.DoUpdate(a);
                //return Json( ResultModel.GetFormResult(res, action, null, a.TaskId), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult DeleteTask(long TaskId)
        {

            int res = 0;
            string action = "הסרת משימה";
            //MemberCategoryView a = null;
            try
            {
                //int RecordId = Types.ToInt(Request["RecordId"]);
                //int accountId = GetAccountId();
                //res = MemberContext.DoDelete(RecordId, accountId);
                string message = "";
                //switch (res)
                //{
                //    case -2:
                //        message = "אירעה שגיאה (Invalid Arguments Account) אנא פנה לתמיכה"; break;
                //    case -1:
                //        message = "המנוי אינו קיים"; break;
                //    case 1:
                //        message = "המנוי הוסר מרשימת החברים"; break;
                //    default:
                //        message = "המנוי לא הוסר"; break;
                //}

                var model = new ResultModel() { Status = res, Title = "הסרת מנוי", Message = message, Link = null, OutputId = 0 };

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

       

        [HttpPost]
        public JsonResult DeleteTaskTimer(Guid TaskId, int TaskTimerId)
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region TaskComment

        [HttpGet]
        public ActionResult _TaskComment(int id, string op)
        {
            EditTaskModel model = new EditTaskModel()
            {
                Id = id,
                Option = op //op= g-a-e-d
            };
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult GetTasksCommentGrid(int pid)
        {
            var db = new TaskContext<TaskComment>(0);
            return Json(db.GetList(pid), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult _TaskCommentAdd(int pid)
        {
            return PartialView("_TaskComment", new EditTaskModel() { PId = pid, Option = "a" });
        }
        [HttpGet]
        public ActionResult _TaskCommentEdit(int id)
        {
            return PartialView("_TaskComment", new EditTaskModel() { Id = id, Option = "e" });
        }

        [HttpPost]
        public ContentResult GetTaskCommentEdit()
        {
            //int taskid = Types.ToInt(Request["taskid"]);
            int id = Types.ToInt(Request["id"]);
            int accountId = GetAccountId();
            string json = "";
            var context = new TaskContext<TaskComment>(GetUser());
            var item = context.Get("CommentId", id);

            if (item != null)
                json = JsonSerializer.Serialize(item);

            return base.GetJsonResult(json);
        }

        [HttpPost]
        public JsonResult TaskCommentAdd()
        {
            TaskContext<TaskComment> context = null;
            try
            {

                //a = EntityContext.Create<TaskComment>(Request.Form);

                //EntityValidator validator = EntityValidator.ValidateEntity(a, "הערות", "he");
                //if (!validator.IsValid)
                //{
                //    return Json(GetFormResult(-1, action, validator.Result, 0), JsonRequestBehavior.AllowGet);
                //}
                context = new TaskContext<TaskComment>(GetUser());
                context.Set(Request.Form);
                context.Current.UserId = GetUser();
                context.Current.AccountId = GetAccountId();
                var res=context.Upsert();
                var model = context.GetFormResult(res, null);
                return Json(model, JsonRequestBehavior.AllowGet);

                //res = TaskComment.Save(a.Task_Id, a.CommentId, a, UpdateCommandType.Insert);
                //return Json(EntityResultModel.GetFormResult(res, context.EntityName, null, context.Current.CommentId), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, context.EntityName, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult TaskCommentAddQs()
        {
            TaskContext<TaskComment> context = null;
            try
            {
                var userId=GetUser();
                var Current = EntityContext.Create<TaskComment>(Request.Form);
                Current.UserId=userId;
                Current.AccountId = GetAccountId();
                context = new TaskContext<TaskComment>(userId);
                context.Set(Current);
                var res = context.Upsert();//.Insert();
                var model = context.GetFormResult(res, null);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, context.EntityName, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult TaskCommentUpdate()
        {
            TaskContext<TaskComment> context = null;
            //string action = "הערות";
            //TaskComment a = null;
            try
            {
                //a = EntityContext.Create<TaskComment>(Request.Form);

                //EntityValidator validator = EntityValidator.ValidateEntity(a, "הערות", "he");
                //if (!validator.IsValid)
                //{
                //    return Json(GetFormResult(-1, action, validator.Result,0), JsonRequestBehavior.AllowGet);
                //}
                context = new TaskContext<TaskComment>(GetUser());
                context.Set(Request.Form);
                context.Current.UserId = GetUser();
                context.Current.AccountId = GetAccountId();
                var res = context.Upsert();//.SaveChanges();
                return Json(context.GetFormResult(res, null), JsonRequestBehavior.AllowGet);

                //res = TaskComment.Save(a.Task_Id,a.CommentId, a,UpdateCommandType.Update);
                //return Json(EntityResultModel.GetFormResult(EntityCommandResult.GetAffectedRecords(res), action, null, context.Current.CommentId), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(FormResult.GetError(context.EntityName, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult TaskCommentDelete(int id)
        {
            int res = 0;
            string action = "הערות";
            //TaskComment a = null;
            try
            {
                var context = new TaskContext<TaskComment>(GetUser());
                res = context.Delete("CommentId", id);
                //res = TaskCommentContext.Delete(id);
                return Json(GetFormResult(res, action, null, id), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region TaskAssign

        [HttpGet]
        public ActionResult _TaskAssign(int id)
        {
            EditTaskModel model = new EditTaskModel()
            {
                Id = id
                //Option = op //op= g-a-e-d
            };
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult GetTasksAssignGrid(int pid)
        {
            var db = new TaskContext<TaskAssignment>(0);
            return Json(db.GetList(pid), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult _TaskAssignAdd(int pid)
        {
            return PartialView("_TaskAssign", new EditTaskModel() { PId = pid, Option = "a" });
        }
        [HttpGet]
        public ActionResult _TaskAssignEdit(int id)
        {
            return PartialView("_TaskAssign", new EditTaskModel() { Id = id, Option = "e" });
        }

        [HttpPost]
        public ContentResult GetTaskAssignEdit()
        {
            //int taskid = Types.ToInt(Request["taskid"]);
            int id = Types.ToInt(Request["id"]);
            int accountId = GetAccountId();
            string json = "";
            var context = new TaskContext<TaskAssignment>(GetUser());
            var item = context.Get("AssignId", id);

            if (item != null)
                json = JsonSerializer.Serialize(item);

            return base.GetJsonResult(json);
        }

        [HttpPost]
        public JsonResult TaskAssignAdd()
        {
            TaskContext<TaskAssignment> context = null;
            try
            {
                context = new TaskContext<TaskAssignment>(GetUser());
                context.Set(Request.Form);
                context.Current.AssignedBy = GetUser();
                context.Current.AccountId = GetAccountId();
                var res = context.Upsert(UpsertType.Insert);//.Insert();
                var model = context.GetFormResult(res, null);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, context.EntityName, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }


        //[HttpGet]
        //public ActionResult _TaskAssignEdit(int id)
        //{
        //    return PartialView();
        //}

        //[HttpPost]
        //public ContentResult GetTaskAssignEdit()
        //{
        //    int id = Types.ToInt(Request["id"]);
        //    int accountId = GetAccountId();
        //    string json = "";
        //    TaskItem item = null;
        //    if (id > 0)
        //    {
        //        item = TaskContext.Get(id);//, accountId);
        //    }
        //    else
        //    {
        //        item = new TaskItem() { AccountId = accountId };
        //    }
        //    if (item != null)
        //        json = JsonSerializer.Serialize(item);

        //    return base.GetJsonResult(json);

        //}

        #endregion

        #region TaskTimer

        [HttpGet]
        public ActionResult _TaskTimer(int id, string op)
        {
            EditTaskModel model = new EditTaskModel()
            {
                Id = id,
                Option = op //op= g-a-e-d
            };
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult GetTasksTimerGrid(int pid)
        {
            var db = new TaskContext<TaskTimer>(0);
            return Json(db.GetList(pid), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult _TaskTimerAdd(int pid)
        {
            return PartialView("_TaskTimer", new EditTaskModel() { PId = pid, Option = "a" });
        }
        [HttpGet]
        public ActionResult _TaskTimerEdit(int id)
        {
            return PartialView("_TaskTimer", new EditTaskModel() { Id = id, Option = "e" });
        }

        [HttpPost]
        public ContentResult GetTaskTimerEdit()
        {
            //int taskid = Types.ToInt(Request["taskid"]);
            int id = Types.ToInt(Request["id"]);
            int accountId = GetAccountId();
            string json = "";
            var context = new TaskContext<TaskTimer>(GetUser());
            var item = context.Get("TaskTimerId", id);

            if (item != null)
                json = JsonSerializer.Serialize(item);

            return base.GetJsonResult(json);
        }

        [HttpPost]
        public JsonResult TaskTimerAdd()
        {
            TaskContext<TaskTimer> context = null;
            try
            {
                context = new TaskContext<TaskTimer>(GetUser());
                context.Set(Request.Form);
                var res = context.Upsert(UpsertType.Insert);//.Insert();
                var model = context.GetFormResult(res, null);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, context.EntityName, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult TaskTimerUpdate()
        {
            string action = "עדכון מעקב זמנים";
            TaskTimer item = null;
            try
            {
                item = Request.Form.Create<TaskTimer>();
                var result = TaskContext.TaskTimerUpdate(item);
                return Json(FormResult.GetLang(result, item.EntityName<TaskTimer>()), JsonRequestBehavior.AllowGet);

                //context = new TaskContext<TaskTimer>(GetUser());
                //context.Set(Request.Form);
                //var item=context.Current;
                //int Mode = item.TaskTimerId > 0 ? 1 : 0;
                //int output=context.ExecuteReturnValue(TaskTimer.ProcName,0,"Mode", Mode, "TaskId", item.Task_Id, "UserId", item.UserId, "TaskTimerId", item.TaskTimerId, "Subject", item.Subject)
                ////var res = context.SaveChanges();
                //var res=new EntityCommandResult((output > 0) ? 1 : 0, output, "TaskTimerId");

                //return Json(context.GetFormResult(res, null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //return Json(EntityResultModel.GetFormResult(-1, context.EntityName, ex.Message, 0), JsonRequestBehavior.AllowGet);
                return Json(FormResult.GetError(action, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult TaskTimerDelete(int id)
        {
            int res = 0;
            string action = "מעקב זמנים";
            //TaskTimer a = null;
            try
            {
                var context = new TaskContext<TaskTimer>(GetUser());
                res = context.Delete("TaskTimerId", id);
                return Json(GetFormResult(res, action, null, id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region TaskForm

        [HttpGet]
        public ActionResult _TaskForm(int id, string op)
        {
            EditTaskModel model = new EditTaskModel()
            {
                Id = id,
                Option = op //op= g-a-e-d
            };
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult GetTasksFormGrid(int pid)
        {
            var db = new TaskContext<TaskForm>(0);
            return Json(db.GetList(pid), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult _TaskFormAdd(int pid)
        {
            return PartialView("_TaskForm", new EditTaskModel() { PId = pid, Option = "a" });
        }
        [HttpGet]
        public ActionResult _TaskFormEdit(int id)
        {
            return PartialView("_TaskForm", new EditTaskModel() { Id = id, Option = "e" });
        }

        [HttpPost]
        public ContentResult GetTaskFormEdit()
        {
            //int taskid = Types.ToInt(Request["taskid"]);
            int id = Types.ToInt(Request["id"]);
            int accountId = GetAccountId();
            string json = "";
            var context = new TaskContext<TaskForm>(GetUser());
            var item = context.Get("ItemId", id);

            if (item != null)
                json = JsonSerializer.Serialize(item);

            return base.GetJsonResult(json);
        }

        //[HttpPost]
        //public JsonResult TaskFormAdd()
        //{
        //    TaskContext<TaskForm> context = null;
        //    try
        //    {
        //        context = new TaskContext<TaskForm>(GetUser());
        //        context.Set(Request.Form);
        //        var res = context.Upsert(UpsertType.Insert);//.Insert();
        //        var model = context.GetFormResult(res, null);
        //        return Json(model, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(GetFormResult(-1, context.EntityName, ex.Message, 0), JsonRequestBehavior.AllowGet);
        //    }
        //}
        [HttpPost]
        public JsonResult TaskFormUpdate()
        {
            TaskContext<TaskForm> context = null;
            try
            {
                int userId=GetUser();
                context = new TaskContext<TaskForm>(userId);
                context.Set(Request.Form);
                var cur = context.Current;
                if (cur.DoneStatus)
                    cur.DoneDate = DateTime.Now;
                if (cur.AssignBy == 0)
                    cur.AssignBy = userId;
                var res = context.SaveChanges();
                return Json(context.GetFormResult(res, null), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(FormResult.GetError(context.EntityName, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult TaskFormDelete(int id)
        {
            int res = 0;
            string action = "טופס";
            //TaskForm a = null;
            try
            {
                var context = new TaskContext<TaskForm>(GetUser());
                res = context.Delete("ItemId", id);
                return Json(GetFormResult(res, action, null, id), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region TaskAction

        [HttpGet]
        public ActionResult _TaskAction(int id, string op)
        {
            EditTaskModel model = new EditTaskModel()
            {
                Id = id,
                Option = op //op= g-a-e-d
            };
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult GetTasksActionGrid(int id)
        {
            var db = new TaskContext<TaskAction>(0);
            return Json(db.GetList(id), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult _TaskActionAdd(int id)
        {
            return PartialView("_TaskAction", new EditTaskModel() { Id = id, Option = "a" });
        }
        [HttpGet]
        public ActionResult _TaskActionEdit(int id)
        {
            return PartialView("_TaskAction", new EditTaskModel() { Id = id, Option = "e" });
        }

        [HttpPost]
        public ContentResult GetTaskActionEdit()
        {
            int taskid = Types.ToInt(Request["taskid"]);
            int id = Types.ToInt(Request["id"]);
            int accountId = GetAccountId();
            string json = "";
            var context = new TaskContext<TaskAction>(GetUser());
            var item = context.Get("ActionId", id);

            if (item != null)
                json = JsonSerializer.Serialize(item);

            return base.GetJsonResult(json);
        }

        [HttpPost]
        public JsonResult TaskActionAdd()
        {
            TaskContext<TaskAction> context = null;
            try
            {
                context = new TaskContext<TaskAction>(GetUser());
                context.Set(Request.Form);
                var res = context.Upsert(UpsertType.Insert);//.Insert();
                var model = context.GetFormResult(res, null);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, context.EntityName, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult TaskActionUpdate()
        {
            TaskContext<TaskAction> context = null;
            try
            {
                context = new TaskContext<TaskAction>(GetUser());
                context.Set(Request.Form);
                var res = context.SaveChanges();
                return Json(context.GetFormResult(res, null), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(FormResult.GetError(context.EntityName, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult TaskActionDelete(int id)
        {
            int res = 0;
            string action = "פעולות לביצוע";
            try
            {
                var context = new TaskContext<TaskAction>(GetUser());
                res = context.Delete("ActionId", id);
                return Json(GetFormResult(res, action, null, id), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region TaskNew

        [HttpGet]
        public ActionResult TaskNew()
        {
            //TaskContext.NewTaskId() 
            return View(new EditTaskModel() { PId = 0});
        }


        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult UpdateNewTask()
        {

            int res = 0;
            string action = "הגדרת משימה";
            TaskItem a = null;
            try
            {
                a = EntityContext.Create<TaskItem>(Request.Form);

                EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת משימה", "he");
                if (!validator.IsValid)
                {
                    return Json(GetFormResult(-1, action, validator.Result, 0), JsonRequestBehavior.AllowGet);

                }
                int user = GetUser();
                int accountId = GetAccountId();
                if (a.TaskId > 0)
                {
                    res = TaskContext.DoUpdate(a);
                }
                else
                {
                    a.AssignByAccount = accountId;
                    a.AccountId = accountId;
                    a.AssignBy = user;
                    res = TaskContext.DoInsert(a);
                }
                return Json(ResultModel.GetFormResult(res, action, null, a.TaskId), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Reminder

        [HttpGet]
        public ActionResult ReminderNew()
        {
            //TaskContext.NewTaskId() 
            return View("Reminder",new EditTaskModel() { PId = 0 });
        }
        [HttpGet]
        public ActionResult ReminderEdit(int id)
        {

            //var dt=TaskContext.GetReminders();

            //string json= Newtonsoft.Json.JsonConvert.SerializeObject(dt);
            //System.Data.DataTable datat = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(json);
            //Console.WriteLine(datat.TableName);

            //string nisjson = dt.ToJson();
            //datat=Nistec.Serialization.JsonSerializer.Deserialize<System.Data.DataTable>(nisjson);
            //Console.WriteLine(datat.TableName);

            var model = new EditTaskModel() { PId = id, Option = "e", Result = TaskContext.Get(id) };
            return View("Reminder", model);
        }
        [HttpGet]
        public ActionResult ReminderInfo(int id)
        {
            return View("Reminder", new EditTaskModel() { PId = id, Option = "g", Result = TaskContext.Get(id) });
        }      

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult ReminderUpdate()
        {

            int res = 0;
            string action = "תזכורת";
            TaskItem a = null;
            try
            {
                a = EntityContext.Create<TaskItem>(Request.Form);

                EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת תזכורת", "he");
                if (!validator.IsValid)
                {
                    return Json(GetFormResult(-1, action, validator.Result, 0), JsonRequestBehavior.AllowGet);

                }
                int user = GetUser();
                int accountId = GetAccountId();
                if (a.TaskId > 0)
                {
                    res = TaskContext.DoUpdate(a);
                }
                else
                {
                    a.UserId = user;
                    a.AssignByAccount = accountId;
                    a.AccountId = accountId;
                    a.AssignBy = user;
                    res = TaskContext.DoInsert(a);
                }
                return Json(ResultModel.GetFormResult(res, action, null, a.TaskId), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region TaskForm Template

        [HttpGet]
        public ActionResult TasksFormTemplate()
        {
            int accountId=GetAccountId();
            var db=new TaskContext<TaskFormTemplate>(GetUser());
            var list=db.GetList("AccountId", accountId);
            var model = new EntityModel()
            {
                Data = Json(db.GetList("AccountId", accountId))
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult GetTasksFormTemplate(int FormId)
        {
            var db = new TaskContext<TaskFormTemplate>(0);
            return Json(db.GetList("FormId", FormId), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult TaskFormTemplateCreate(int id, string name)
        {
            string action = "יצירת תבנית";

            try
            {
                int accountId = GetAccountId();
                int user = GetUser();
                var result = TaskContext.TaskFormTemplateCreate(id, accountId, user, name);
                return Json(FormResult.GetLang(result, action), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex,action,Request), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult TaskFormByTemplate(int TaskId, int FormId)
        {
            string action = "תבנית לביצוע";
            try
            {
                int user = GetUser();
                var res = TaskContext.TaskFormByTemplate(TaskId, user, FormId);
                return Json(FormResult.Get(res, action), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }



        #endregion

        #region ticket

        [HttpGet]
        public ActionResult TicketNew()
        {
            //TaskContext.NewTaskId()
            return View(new EditTaskModel() { PId = 0 });
        }

           
        [HttpGet]
        public ActionResult TicketEdit(int id)
        {
            return View(new EditTaskModel() { PId = id, Option = "e" });
        }
        [HttpGet]
        public ActionResult TicketInfo(int id)
        {
            return View("TaskEdit", new EditTaskModel() { PId = id, Option = "g" });
        }
       
        

        #endregion

        #region Scheduler

        [HttpGet]
        public ActionResult Scheduler()
        {
            return View(new EditTaskModel() { PId = 0, Option="g" });
        }


        #endregion

        #region Project


        #endregion

        #region News


        #endregion

    }
}
