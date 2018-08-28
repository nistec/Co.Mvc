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
using ProSystem.Query;
using ProSystem.Data.Enums;

namespace Pro.Mvc.Controllers
{
    [Authorize]
    public class SystemController : BaseController
    {
        #region user

        public ActionResult UsersDef()
        {
            return View(true);
        }

        public ActionResult _DefUser()
        {
            return PartialView(true);
        }
        [HttpPost]
        public JsonResult GetUserInfo()
        {
            try {
                var su = GetSignedUser(true);
                //int accountId = su.AccountId;
                //int userId = su.UserId;
                UserProfile u = UserProfile.Get(su.UserId);
                return Json(u, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, "פרטים אישיים", ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
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
                if (!IsSignedUser(Nistec.Web.Security.UserRole.Manager, false)) //(IsAdminOrManager() == false)
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
                if (!IsSignedUser(Nistec.Web.Security.UserRole.Manager, false)) //(IsAdminOrManager() == false)
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
        public JsonResult GetEnumTypesList(string model)
        {
            int accountId = GetAccountId();
            return Json(EnumTypes.ViewList(accountId, model), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetEnumStatusList(string model)
        {
            int accountId = GetAccountId();
            return Json(EnumStatus.ViewList(model), JsonRequestBehavior.AllowGet);
        }

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

            var list = UserContext<UserTeamProfile>.ExecEntityList("AccountId", accountId, "UserId", userId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetUsersList()
        {
            int accountId = GetAccountId();
            //int userId = GetUser();

            var list = UserContext<UserItemInfo>.GetEntityList("ParentId", accountId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public JsonResult GetTaskTypeList(string model)
        //{
        //    int accountId = GetAccountId();
        //    return Json(TaskTypeEntity.ViewList(accountId, model), JsonRequestBehavior.AllowGet);
        //    //return Json(EntityProCache.ViewEntityList<TaskTypeEntity>(EntityCacheGroups.Enums, TaskTypeEntity.TableName, accountId), JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult GetTaskStatusList()
        {
            return Json(Lists<TaskStatus>.Get_List(), JsonRequestBehavior.AllowGet);
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
            return Json(db.ExecOrViewList("AccountId", accountId), JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult GetTagsList()
        //{
        //    int accountId = GetAccountId();
        //    var tags = TaskContext.GetTagsList(accountId);
        //    return Json(tags, JsonRequestBehavior.AllowGet);
        //    //return GetJsonResult(tags);
        //}
        public ContentResult GetTagsList()
        {
            int accountId = GetAccountId();
            var tags = TaskContext.ViewTagsJson(accountId);
            //return Json(tags, JsonRequestBehavior.AllowGet);
            return GetJsonResult(tags);
        }
        [HttpPost]
        public ContentResult GetTaskFolderList()
        {
            int accountId = GetAccountId();
            var tags = TaskContext.GetTasksFoldersJson(accountId);
            return GetJsonResult(tags);
        }
        [HttpPost]
        public ContentResult GetLabelList(int id, string source)
        {
            //int accountId = GetAccountId();
            var tags = AdContext.GetLabelJson(id, source);
            return GetJsonResult(tags);
        }
        #endregion

        #region lists items

        [HttpPost]
        public JsonResult GetListsSelect(int model)
        {
            int accountId = GetAccountId();
            var list = Lists.ExecListSelect(accountId, (ListsTypes)model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetListsTags(int model)
        {
            int accountId = GetAccountId();
            var list = Lists.ExecListTags(accountId, (ListsTypes)model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Common Properties

        [HttpPost]
        public JsonResult DefEntityView(string entity)
        {
            int accountId = GetAccountId();

            switch (entity)
            {
                case "task_type":
                    return Json(EntityPro.ViewEntityList<EnumTypes>(EntityGroups.Enums, EnumTypes.TableName, accountId, ListsTypes.Task_Types), JsonRequestBehavior.AllowGet);
                case "topic_type":
                    return Json(EntityPro.ViewEntityList<EnumTypes>(EntityGroups.Enums, EnumTypes.TableName, accountId, ListsTypes.Topic_Types), JsonRequestBehavior.AllowGet);
                case "ticket_type":
                    return Json(EntityPro.ViewEntityList<EnumTypes>(EntityGroups.Enums, EnumTypes.TableName, accountId, ListsTypes.Ticket_Types), JsonRequestBehavior.AllowGet);
                case "doc_type":
                    return Json(EntityPro.ViewEntityList<EnumTypes>(EntityGroups.Enums, EnumTypes.TableName, accountId, ListsTypes.Doc_Types), JsonRequestBehavior.AllowGet);
                //case "tags":
                //    return Json(EntityPro.ViewEntityList<string>(EntityGroups.Enums, TaskTypeEntity.TableName, accountId, ListsTypes.Tags), JsonRequestBehavior.AllowGet);
                default:
                    return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //public JsonResult DefEntityEnumView(string entity)
        //{
        //    int accountId = GetAccountId();

        //    switch (entity)
        //    {
        //        case "status":
        //            return Json(PropsEnum.EntityEnum.ViewEntityList<PropsEnum.StatusView>(PropsEnum.StatusView.TableName, "Status", accountId), JsonRequestBehavior.AllowGet);
        //        //case "category":
        //        //    return Json(EntityEnum.ViewEntityList<CategoryView>(CategoryView.TableName, "Category", accountId), JsonRequestBehavior.AllowGet);
        //        //case "region":
        //        //    return Json(EntityEnum.ViewEntityList<RegionView>(RegionView.TableName, "Region", accountId), JsonRequestBehavior.AllowGet);
        //        case "role":
        //            return Json(PropsEnum.EntityEnum.ViewEntityList<PropsEnum.RoleView>(PropsEnum.RoleView.TableName, "Role", accountId), JsonRequestBehavior.AllowGet);
        //        default:
        //            return Json(null, JsonRequestBehavior.AllowGet);
        //    }
        //}

        #endregion

        #region Def Entity

        public ActionResult DefEntity(string entity)
        {
            switch (entity)
            {
                case "task_type":
                    ViewBag.TagPropId = EnumTypes.TagPropId;
                    ViewBag.TagPropName = EnumTypes.TagPropName;
                    ViewBag.TagPropTitle = EnumTypes.TagPropTitle;
                    break;
                case "topic_type":
                    ViewBag.TagPropId = EnumTypes.TagPropId;
                    ViewBag.TagPropName = EnumTypes.TagPropNameTopic;
                    ViewBag.TagPropTitle = EnumTypes.TagPropNameTopic;
                    break;
                case "ticket_type":
                    ViewBag.TagPropId = EnumTypes.TagPropId;
                    ViewBag.TagPropName = EnumTypes.TagPropNameTicket;
                    ViewBag.TagPropTitle = EnumTypes.TagPropNameTicket;
                    break;
                case "doc_type":
                    ViewBag.TagPropId = EnumTypes.TagPropId;
                    ViewBag.TagPropName = EnumTypes.TagPropNameDoc;
                    ViewBag.TagPropTitle = EnumTypes.TagPropNameDoc;
                    break;
            }

            return View(true);
        }


        [HttpPost]
        public JsonResult DefEntityUpdate(int PropId, string PropName, string EntityType, int command)
        {
            //ValidateAdmin();
            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {

                if ((UpdateCommandType)command == UpdateCommandType.Delete)
                {
                    result = EntityPro.DoDelete(EnumTypes.TableName, EntityType, PropId, 0, accountId);
                }
                else
                {

                    switch (EntityType)
                    {
                        case "task_type":
                            result = EntityPro.DoSave<EnumTypes>(new EnumTypes() { PropId = PropId, PropName = PropName, AccountId = accountId, TyprModel = "T" }, ListsTypes.Task_Types, (UpdateCommandType)command); break;
                        case "topic_type":
                            result = EntityPro.DoSave<EnumTypes>(new EnumTypes() { PropId = PropId, PropName = PropName, AccountId = accountId, TyprModel = "P" }, ListsTypes.Topic_Types, (UpdateCommandType)command); break;
                        case "ticket_type":
                            result = EntityPro.DoSave<EnumTypes>(new EnumTypes() { PropId = PropId, PropName = PropName, AccountId = accountId, TyprModel = "E" }, ListsTypes.Ticket_Types, (UpdateCommandType)command); break;
                        case "doc_type":
                            result = EntityPro.DoSave<EnumTypes>(new EnumTypes() { PropId = PropId, PropName = PropName, AccountId = accountId, TyprModel = "D" }, ListsTypes.Doc_Types, (UpdateCommandType)command); break;
                        //case "tags":
                        //    result = EntityPro.DoSave<TaskTypeEntity>(PropId, PropName, accountId, (UpdateCommandType)command); break;
                    }
                }
                rm = new ResultModel(result);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                result = -1;
                rm = new ResultModel()
                {
                    Status = result,
                    Message = err
                };
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
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


        //[HttpPost]
        //public JsonResult Lookup_GetList(string type)
        //{
        //    int accountId = GetAccountId();

        //    switch (type)
        //    {
        //        case "members":
        //            return Json(DbLookups.ViewEntityList(EntityGroups.Enums, "BranchId", "BranchName", "lu_Members", accountId), JsonRequestBehavior.AllowGet);
        //        case "branch":
        //            return Json(DbLookups.ViewEntityList(EntityGroups.Enums, "BranchId", "BranchName", "Branch", accountId), JsonRequestBehavior.AllowGet);
        //        case "charge":
        //            return Json(DbLookups.ViewEntityList(EntityGroups.Enums, "ChargeId", "ChargeName", "Charge", accountId), JsonRequestBehavior.AllowGet);
        //        case "city":
        //            return Json(DbLookups.ViewEntityList(EntityGroups.Enums, "CityId", "CityName", "Cities", accountId), JsonRequestBehavior.AllowGet);
        //        //case "place":
        //        //    return Json(EntityPro.ViewEntityList<PlaceView>(EntityGroups.Enums, PlaceView.TableName, accountId), JsonRequestBehavior.AllowGet);
        //        case "region":
        //            return Json(DbLookups.ViewEntityList(EntityGroups.Enums, "RegionId", "RegionName", "Region", accountId), JsonRequestBehavior.AllowGet);
        //        case "category":
        //            return Json(DbLookups.ViewEntityList(EntityGroups.Enums, "CategoryId", "CategoryName", "Categories", accountId), JsonRequestBehavior.AllowGet);
        //        case "role":
        //            return Json(DbLookups.ViewEntityList(EntityGroups.Enums, "RoleId", "RoleName", "Roles", accountId), JsonRequestBehavior.AllowGet);
        //        case "status":
        //            return Json(DbLookups.ViewEntityList(EntityGroups.Enums, "StatusId", "StatusName", "Status", accountId), JsonRequestBehavior.AllowGet);
        //        case "enum1":
        //        case "exenum1":
        //            return Json(DbLookups.ViewEnumList(EntityGroups.Enums, "PropId", "PropName", "Enum", accountId, 1), JsonRequestBehavior.AllowGet);
        //        case "enum2":
        //        case "exenum2":
        //            return Json(DbLookups.ViewEnumList(EntityGroups.Enums, "PropId", "PropName", "Enum", accountId, 1), JsonRequestBehavior.AllowGet);
        //        case "enum3":
        //        case "exenum3":
        //            return Json(DbLookups.ViewEnumList(EntityGroups.Enums, "PropId", "PropName", "Enum", accountId, 1), JsonRequestBehavior.AllowGet);
        //        default:
        //            return Json(DbLookups.ViewEntityList(EntityGroups.Enums, "Value", "Label", type, accountId), JsonRequestBehavior.AllowGet);

        //            //return Json(null, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public JsonResult Lookup_ProjectName(int id)
        {
            int accountId = GetAccountId();
            var value = SystemLookups.Project("ProjectName", "AccountId", accountId, "ProjectId", id);
            return Json(value);// ContentModel.Get(value), JsonRequestBehavior.AllowGet);
        }

    

        #endregion

        #region Ad

        [HttpGet]
        public ActionResult AdDef()
        {
            return View(true);
        }

        [HttpPost]
        public ActionResult AdDefList()
         {
             int accountId = GetAccountId();
             var db = new AdContext<AdItem>(accountId);
             return Json(db.GetList(accountId), JsonRequestBehavior.AllowGet);
        }
 
        [HttpPost]
         public ActionResult AdShowMembers()
         {
             int accountId = GetAccountId();
             var db = new AdContext<AdItem>(accountId);
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
                context.Current.AccountId = accountId;
                var res = context.SaveChanges();
                return Json(context.GetFormResult(res, null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AdDefDelete(int GroupId)
        {
            string action = "מחיקת קבוצה";
            AdContext<AdItem> context = null;
            try
            {
                ValidateDelete(GetUser(), "AdDefDelete");

                int accountId = GetAccountId();
                context = new AdContext<AdItem>(accountId);
                var res = context.Delete("GroupId", GroupId, "AccountId", accountId);
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
            return Json(db.ExecOrViewList("AccountId", accountId, "GroupId", id), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AdDefRelAll(int id)
        {
            int accountId = GetAccountId();
            var db = new AdContext<AdItemRelAll>(accountId);
            return Json(db.ExecOrViewList("GroupId", id, "AccountId", accountId, "IsAll", 2), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AdDefRelToAdd(int id)
        {
            int accountId = GetAccountId();
            var db = new AdContext<AdItemRelAll>(accountId);
            return Json(db.ExecOrViewList("GroupId", id, "AccountId", accountId, "IsAll", 1), JsonRequestBehavior.AllowGet);
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
            return View(true);
        }
  

        [HttpPost]
        public ActionResult AdUserDefList()
        {
            var su= GetSignedUser(false);
            if (su == null)
            {
                return RedirectToLogin();
            }
            //int parentId = su.ParentId;
            int userId = su.UserId;
            int accountId = su.AccountId;
            
            var db = new AdContext<AdUserProfile>(accountId);
            var list = db.ExecOrViewList("AccountId", accountId, "UserId", userId);
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
                context.Current.AccountId = accountId;
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
                context.Validate(ProcedureType.Insert);
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
                var reslult = StatusDesc.GetMembershipResult(action, status);
                return Json(reslult, JsonRequestBehavior.AllowGet);
                //return Json(context.GetFormResult(res, Message), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AdUserDefDelete(int UserId)
        {
            string action = "מחיקת משתמש";
            AdContext<AdUserProfile> context = null;
            try
            {
                var user=GetSignedUser(true);
                if (user == null)
                {
                    return RedirectToLogin();
                }
                ValidateDelete(user.UserId, "AdUserDefDelete");

                int accountId = user.AccountId;
                context = new AdContext<AdUserProfile>(accountId);
                //var res = context.Delete("UserId", UserId, "AccountId", accountId, "AssignBy", user.UserId);
                var status = context.DeleteReturnValue(-1, "UserId", UserId, "AccountId", accountId, "AssignBy", user.UserId);
                var reslult = StatusDesc.GetAuthResult(action, status);
                return Json(reslult, JsonRequestBehavior.AllowGet);
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
            return View(true);
        }

        [HttpPost]
        public ActionResult AdTeamDefList()
        {
            int accountId = GetAccountId();
            var db = new AdContext<AdTeamItem>(accountId);
            return Json(db.GetList(accountId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AdTeamShowMembers(int id)
        {
            int accountId = GetAccountId();
            var db = new AdContext<AdTeamItem>(accountId);
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
                context.Current.AccountId = accountId;
                var res = context.SaveChanges();
                return Json(context.GetFormResult(res, null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AdTeamDefDelete(int TeamId)
        {
            string action = "מחיקת צוות";
            AdContext<AdTeamItem> context = null;
            try
            {
                ValidateDelete(GetUser(), "AdTeamDefDelete");

                int accountId = GetAccountId();
                context = new AdContext<AdTeamItem>(accountId);
                var res = context.Delete("TeamId", TeamId, "AccountId", accountId);
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
            return Json(db.ExecOrViewList("AccountId", accountId, "TeamId", id), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AdTeamDefRelAll(int id)
        {
            int accountId = GetAccountId();
            var db = new AdContext<AdTeamItemRelAll>(accountId);
            return Json(db.ExecOrViewList("TeamId", id, "AccountId", accountId, "IsAll", 2), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AdTeamDefRelToAdd(int id)
        {
            int accountId = GetAccountId();
            var db = new AdContext<AdTeamItemRelAll>(accountId);
            return Json(db.ExecOrViewList("TeamId", id, "AccountId", accountId, "IsAll", 1), JsonRequestBehavior.AllowGet);
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

        #region AdShare

        [HttpGet]
        public ActionResult AdShareDef()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdShareList(int id)
        {
            int accountId = GetAccountId();
            int user=GetUser();
            
            var db = new AdContext<AdShare>(accountId,user);
            var list = db.ExecList("ShareModel",0, "AccountId", accountId, "UserId", user);
            var filterlist = list.Where(v => v.ShareModel == 0 || v.ShareModel == id);
            return Json(filterlist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AdShareShowMembers(int id)
        {
            int accountId = GetAccountId();
            int user = GetUser();
            var db = new AdContext<AdShare>();
            var list = db.ExecList("ShareModel", id, "AccountId", accountId, "UserId", user, "ListType", 1);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AdShareDefUpdate()
        {
            string action = "עדכון שיתוף";
            AdContext<AdShare> context = null;
            try
            {
                ValidateUpdate(GetUser(), "AdShareDefUpdate");

                int accountId = GetAccountId();
                int user = GetUser();
                context = new AdContext<AdShare>(accountId, user);
                context.Set(Request.Form);
                //context.Current.AccountId = accountId;
                var res = context.SaveChanges();
                return Json(context.GetFormResult(res, null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AdShareDefDelete(int id, int uid)
        {
            string action = "מחיקת שיתוף";
            AdContext<AdShare> context = null;
            try
            {
                int user = GetUser();
                ValidateDelete(user, "AdSharDefDelete");
                int accountId = GetAccountId();
                context = new AdContext<AdShare>(accountId, user);
                var res = context.Delete("ShareModel", id, "AccountId", accountId, "UserId", uid, "ShareWith", user);
                return Json(FormResult.Get(res, context.EntityName, "ok"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AdShareDefRel(int id)
        {
            int accountId = GetAccountId();
            int user = GetUser();
            var db = new AdContext<AdShare>();
            var list = db.ExecList("ShareModel", id, "AccountId", accountId, "UserId", user, "ListType", 1);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AdShareDefRelAll(int id)
        {
            int accountId = GetAccountId();
            int user = GetUser();
            var db = new AdContext<AdShare>();
            var list = db.ExecList("ShareModel", id, "AccountId", accountId, "UserId", user, "ListType", 0);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AdShareDefRelToAdd(int id)
        {
            int accountId = GetAccountId();
            int user = GetUser();
            var db = new AdContext<AdShare>();
            var list = db.ExecList("ShareModel", id, "AccountId", accountId, "UserId", user, "ListType", 2);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AdShareDefRelUpdate()
        {
            string action = "הגדרת שיתוף";
            AdContext<AdShare> context = null;
            try
            {
                ValidateUpdate(GetUser(), "AdShareDefRelUpdate");
                int user = GetUser();
                int accountId = GetAccountId();
                int ShareModel = Types.ToInt(Request["ShareModel"]);
                string users = Request["Users"];
                if (ShareModel > 0 && !string.IsNullOrEmpty(users))
                {
                    context = new AdContext<AdShare>(accountId, user);
                    //@Mode tinyint=0--0= insert,1=upsert,2=delete
                    var model = context.Upsert(UpsertType.Update, ReturnValueType.ReturnValue, "ShareModel", ShareModel, "AccountId", accountId, "UserId", user, "Mode", 0);//Update
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
        public ActionResult AdShareDefRelDelete()
        {
            string action = "מחיקת משתמשים משיתוף";
            AdContext<AdShare> context = null;
            int res = 0;
            try
            {
                ValidateDelete(GetUser(), "AdShareDefRelDelete");
                int ShareModel = Types.ToInt(Request["ShareModel"]);
                int userid = Types.ToInt(Request["UserId"]);
                if (ShareModel > 0 && userid > 0)
                {
                    int user = GetUser();
                    int accountId = GetAccountId();
                    context = new AdContext<AdShare>(accountId);
                    res = context.Delete("ShareModel", ShareModel, "AccountId", accountId, "UserId", userid, "ShareWith", user);
                }
                return Json(FormResult.Get(res, context.EntityName, "ok"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetTrace<DbSystem>(ex, action, Request), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Reports
        
        [HttpGet]
        public ActionResult SystemBoard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ReportTasks()
        {
            var userinfo = LoadUserInfo();
            TaskQuery model = new TaskQuery()
            {
                AccountId = userinfo.AccountId,
                UserId = userinfo.UserId
            };
            return View(userinfo, model);
        }
        [HttpGet]
        public ActionResult ReportTopics()
        {
            var userinfo = LoadUserInfo();
            TaskQuery model = new TaskQuery()
            {
                AccountId = userinfo.AccountId,
                UserId = userinfo.UserId
            };
            return View(userinfo, model);
        }
        //[HttpGet]
        //public ActionResult ReportDocs()
        //{
        //    var userinfo = LoadUserInfo();
        //    TaskQuery model = new TaskQuery()
        //    {
        //        AccountId = userinfo.AccountId,
        //        UserId = userinfo.UserId
        //    };
        //    return View(userinfo, model);
        //}
        [HttpGet]
        public ActionResult ReportSubTask()
        {
            var userinfo = LoadUserInfo();
            TaskQuery model = new TaskQuery()
            {
                AccountId = userinfo.AccountId,
                UserId = userinfo.UserId
            };
            return View(userinfo, model);
        }
        [HttpPost]
        public ActionResult GetSubTaskGrid()
        {
            TaskQuery query = new TaskQuery(Request,false);
            var list = TaskContext.ViewSubTask(query.AccountId, query.UserId, query.AssignBy, query.TaskStatus,query.DateFrom,query.DateTo);
            //var row = list.FirstOrDefault<MemberListView>();
            //int totalRows = row == null ? 0 : row.TotalRows;
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region topic

       
        [HttpGet]
        public ActionResult Topics()
        {
            var userinfo = LoadUserInfo();
            TaskQuery model = new TaskQuery()
            {
                AccountId = userinfo.AccountId,
                UserId = userinfo.UserId
            };
            return View(userinfo, model);
        }

        [HttpPost]
        public ActionResult GetTopicGrid()
        {
            TaskQuery query = new TaskQuery(Request,false);
            var list = TaskContext.ViewTopics(query.AccountId, query.UserId, query.AssignBy, query.TaskStatus);
            //var row = list.FirstOrDefault<MemberListView>();
            //int totalRows = row == null ? 0 : row.TotalRows;
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TopicNew(int pid)
        {
            //TaskContext.NewTaskId()
            return View(true, new EditTaskModel() { PId = pid });
        }
        [HttpGet]
        public ActionResult TopicStart(int id)
        {
            int accountId = GetAccountId();
            int user = GetUser();
            TaskContext.Task_Status_Change(id, user, 2, "אתחול סוגיה", null);
            return View(true, "TopicEdit", new EditTaskModel() { Id = id, Option = "e" });//, Data = TaskContext.Get(id) });
        }

        [HttpGet]
        public ActionResult TopicEdit(int id)
        {
            return View(true, new EditTaskModel() { Id = id, Option = "e" });//, Data = TaskContext.Get(id) });
        }
        [HttpGet]
        public ActionResult TopicInfo(int id)
        {
            return View(true, new EditTaskModel() { Id = id, Option = "g", IsInfo = true });//, Data = TaskContext.Get(id) });
        }

        [HttpPost]
        public JsonResult TopicTaskFormStart(int id, int itemId)
        {
            TaskContext<TaskForm> context = null;
            try
            {
                int userId = GetUser();
                var res = TaskContext.TopicTaskFormStart(id, itemId);
                return Json(new FormResult(res), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(FormResult.GetError(context.EntityName, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region docs old
        /*

        [HttpGet]
        public ActionResult Docs()
        {
            var userinfo = LoadUserInfo();
            TaskQuery model = new TaskQuery()
            {
                AccountId = userinfo.AccountId,
                UserId = userinfo.UserId
            };
            return View(userinfo, model);
        }

        [HttpPost]
        public ActionResult GetDocGrid()
        {
            TaskQuery query = new TaskQuery(Request,true);
            var list = TaskContext.ViewDocs(query.AccountId, query.UserId, query.AssignBy, query.TaskStatus);
            //var row = list.FirstOrDefault<MemberListView>();
            //int totalRows = row == null ? 0 : row.TotalRows;
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DocNew(int pid)
        {
            //TaskContext.NewTaskId()
            return View(true, new EditTaskModel() { PId = pid });
        }
        [HttpGet]
        public ActionResult DocStart(int id)
        {
            int accountId = GetAccountId();
            int user = GetUser();
            TaskContext.Task_Status_Change(id, user, 2, "אתחול מסמך", null);
            return View(true, "DocEdit", new EditTaskModel() { Id = id, Option = "e" });//, Data = TaskContext.Get(id) });
        }

        [HttpGet]
        public ActionResult DocEdit(int id)
        {
            return View(true, new EditTaskModel() { Id = id, Option = "e" });//, Data = TaskContext.Get(id) });
        }
        [HttpGet]
        public ActionResult DocInfo(int id)
        {
            return View(true, "DocEdit", new EditTaskModel() { Id = id, Option = "g" });//, Data = TaskContext.Get(id) });
        }

        [HttpPost]
        public JsonResult DocTaskFormStart(int id, int itemId)
        {
            TaskContext<TaskForm> context = null;
            try
            {
                int userId = GetUser();
                var res = TaskContext.TopicTaskFormStart(id, itemId);
                return Json(new FormResult(res), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(FormResult.GetError(context.EntityName, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }
        */
        #endregion

        #region docs
        
        [HttpGet]
        public ActionResult ReportDocs()
        {
            var userinfo = LoadUserInfo();
            QueryModel model = new QueryModel()
            {
                AccountId = userinfo.AccountId,
                UserId = userinfo.UserId
            };
            return View(userinfo, model);
        }

        [HttpPost]
        public ActionResult GetDocGrid()
        {
            string action = "תיעוד מסמכים";
            try
            {

                int AssignBy = Types.ToInt(Request["AssignBy"]);
                int DocStatus = Types.ToInt(Request["DocStatus"]);

                var signedUser = GetSignedUser(true);
                int userId = signedUser.UserId;
                int accountId = signedUser.AccountId;

                var list = DocsContext<DocListView>.ViewDocs(accountId, userId, AssignBy, DocStatus);
                //var row = list.FirstOrDefault<MemberListView>();
                //int totalRows = row == null ? 0 : row.TotalRows;
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult DocNew(int pid)
        {
            //TaskContext.NewTaskId()
            return View(true, new EditTaskModel() { PId = pid });
        }

        //[HttpGet]
        //public ActionResult DocStart(int id)
        //{
        //    int accountId = GetAccountId();
        //    int user = GetUser();
        //    TaskContext.Task_Status_Change(id, user, 2, "אתחול מסמך", null);
        //    return View(true, "DocEdit", new EditTaskModel() { Id = id, Option = "e" });
        //}

        [HttpGet]
        public ActionResult DocEdit(int id)
        {
            return View(true, new EditTaskModel() { Id = id, Option = "e" });
        }
        [HttpGet]
        public ActionResult DocInfo(int id)
        {
            return View(true, new EditTaskModel() { Id = id, Option = "g", IsInfo=true });
        }

        [HttpPost]
        public ContentResult GetDocEdit()
        {
            int id = Types.ToInt(Request["id"]);

            string json = "";
            DocItem item = null;
            int accountId = GetAccountId();

            if (id > 0)
            {
                item = DocsContext<DocItem>.Get(accountId).Get("DocId", id);
            }
            else
            {
                item = new DocItem() { AccountId = accountId };// DbSystem.SysCounters(2) };
            }
            if (item != null)
                json = JsonSerializer.Serialize(item);
            return base.GetJsonResult(json);
        }

        [HttpPost]
        public ContentResult GetDocInfo()
        {
            int id = Types.ToInt(Request["id"]);

            string json = "";
            DocItemInfo item = null;
            int accountId = GetAccountId();
            if (id > 0)
            {
                item = DocsContext<DocItemInfo>.Get(accountId).ExecGet("DocId", id);
            }
            else
            {
                //int accountId = GetAccountId();
                //item = new DocItem() { AccountId = accountId };
                return base.GetJsonResult(null);
            }
            if (item != null)
                json = JsonSerializer.Serialize(item);
            return base.GetJsonResult(json);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult UpdateNewDoc()
        {

            int res = 0;
            string action = "תיעוד מסמך";
            DocItem a = null;
            try
            {
                a = EntityContext.Create<DocItem>(Request.Form);

                EntityValidator validator = EntityValidator.ValidateEntity(a, action, "he");
                if (!validator.IsValid)
                {
                    return Json(GetFormResult(-1, action, validator.Result, 0), JsonRequestBehavior.AllowGet);

                }
                var signedUser = GetSignedUser(true);
                int user = signedUser.UserId;
                int accountId = signedUser.AccountId;
                if (a.DocStatus < 1)
                    a.DocStatus = 1;

                if (a.DocId == 0)
                {
                    //a.AssignByAccount = accountId;
                    a.AccountId = accountId;
                    a.AssignBy = user;
                }

                int docId = DocsContext<DocItem>.Get(accountId).AddOrUpdate(a);
                res = docId > 0 ? 1 : 0;
                return Json(ResultModel.GetFormResult(res, action, null, docId), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult UpdateDoc()
        {

            //int res = 0;
            string action = "תיעוד מסמך";
            DocItem a = null;
            try
            {

                a = EntityContext.Create<DocItem>(Request.Form);
                var su = GetSignedUser(true);
                a.AccountId = su.AccountId;
                a.UserId = a.UserId == 0 ?su.UserId : a.UserId;

                EntityValidator validator = EntityValidator.ValidateEntity(a, action, "he");
                if (!validator.IsValid)
                {
                    return Json( new FormResult()
                    {
                        Message = validator.Result,
                        Status = -1,
                        Title = action
                    });

                }
                //int user = GetUser();
                //var res = TaskContext.DoUpdate(item);
                var res = DocsContext<DocItem>.Get(su.AccountId).AddOrUpdate(a);
                return Json(new FormResult(res)
                {
                    OutputId = (res > 0) ? a.DocId : 0,
                    Title = action
                });
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult ArchiveDoc(int id)
        {

            int res = 0;
            string action = "ארכוב תיעוד";
            //MemberCategoryView a = null;
            try
            {
                int accountId = GetAccountId();
                res = DocsContext<DocItem>.Get(accountId).ArchiveDocs(id, accountId);
                string message = res > 0 ? "המסמך אורכב בהצלחה" : "המסמך לא אורכב";
                var model = new ResultModel() { Status = res, Title = action, Message = message, Link = null, OutputId = 0 };

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult DeleteDoc(int id)
        {

            int res = 0;
            string action = "הסרת תיעוד";
            //MemberCategoryView a = null;
            try
            {
                int accountId = GetAccountId();
                //not supported
                //res = DocsContext<DocItem>.Get(accountId).Delete("DocId",id, "AccountId",accountId);
                string message = res > 0 ? "המסמך הוסר בהצלחה" : "המסמך לא הוסר";
                var model = new ResultModel() { Status = res, Title = action, Message = message, Link = null, OutputId = 0 };

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetDocsCommentGrid(int pid)
        {
            var context = new EntityContext<DbSystem, DocComment>();
            return Json(context.ViewList("Doc_Id", pid), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ContentResult GetDocCommentEdit()
        {
            string json = "";
            try
            {
                int id = Types.ToInt(Request["id"]);
                var su = GetSignedUser(true);
                int accountId = su.AccountId;
                int userId = su.UserId;

                var context = new EntityContext<DbSystem, DocComment>();
                var item = context.Get("CommentId", id);

                if (item != null)
                    json = JsonSerializer.Serialize(item);
                return base.GetJsonResult(json);

            }
            catch (Exception ex)
            {
                return base.GetJsonResult(json);
            }
        }

        [HttpPost]
        public JsonResult DocCommentUpdate()
        {
            EntityContext <DbSystem,DocComment> context = null;
            string action = "עדכון הערה";
            try
            {
                var su = GetSignedUser(true);
                var userId = su.UserId;

                context = new EntityContext<DbSystem, DocComment>();
                context.Set(Request.Form);
                context.Current.UserId = GetUser();
                context.Current.AccountId = GetAccountId();
                var res = context.Upsert();//.SaveChanges();
                res.Set(FormResult.GetResultMessage(res.AffectedRecords, action), action);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetError(context.EntityName, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DocCommentDelete(int id)
        {
            int res = 0;
            string action = "הערות";
            try
            {
                var context = new EntityContext<DbSystem,DocComment>();
                res = context.Delete("CommentId", id);
                return Json(GetFormResult(res, action, null, id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetDocsFormGrid(int pid)
        {
            var context = new EntityContext<DbSystem, DocForm>();
            return Json(context.ViewList("Doc_Id", pid), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ContentResult GetDocFormEdit()
        {
            string json = "";
            try
            {
                //int taskid = Types.ToInt(Request["taskid"]);
                int id = Types.ToInt(Request["id"]);
                var su = GetSignedUser(false);
                int accountId = su.AccountId;
                int userId = su.UserId;
                
                var context = new EntityContext<DbSystem, DocForm>();
                var item = context.Get("ItemId", id);

                if (item != null)
                    json = JsonSerializer.Serialize(item);

                return base.GetJsonResult(json);
            }
            catch (Exception ex)
            {
                return base.GetJsonResult(json);
            }
        }

        [HttpPost]
        public JsonResult DocFormUpdate()
        {
            string action = "תיעוד טופס";
            EntityContext<DbSystem, DocForm> context = null;
            try
            {
                int userId = GetUser();
                context = new EntityContext<DbSystem, DocForm>();
                context.Set(Request.Form);
                var cur = context.Current;
                if (cur.DoneStatus)
                    cur.DoneDate = DateTime.Now;
                //if (cur.AssignBy == 0)
                //    cur.AssignBy = userId;
                var res = context.SaveChanges();
                res.Set(FormResult.GetResultMessage(res.AffectedRecords, action), action);

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetError(context.EntityName, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DocFormChecked(int id, bool done)
        {
            string action = "תיעוד טופס";
            EntityContext<DbSystem, DocForm> context = null;
            try
            {
                int userId = GetUser();
                context = new EntityContext<DbSystem, DocForm>();
                var item = context.Get("ItemId", id);
                item.DoneStatus = done;
                if (done)
                {
                    item.DoneDate = DateTime.Now;
                    //if (item.StartDate == null)
                    //    item.StartDate = DateTime.Now;
                }
                else
                {
                    item.DoneDate = null;//new Nullable<DateTime>().Value;
                }

                //if (item.AssignBy == 0)
                //    item.AssignBy = userId;
                context.Set(item);
                var res = context.SaveChanges();
                res.Set(FormResult.GetResultMessage(res.AffectedRecords, action), action);

                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(FormResult.GetError(context.EntityName, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DocFormStart(int id)
        {
            string action = "אתחול טופס";
            try
            {
                int userId = GetUser();
                var res = DocsContext<DocItem>.DocFormStart(id, userId);
                return Json(new FormResult(res), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(FormResult.GetError(action, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DocFormDelete(int id)
        {
            int res = 0;
            string action = "מחיקת שורה";
            try
            {
                //var context = new DocsContext(0);
                var context = new EntityContext<DbSystem, DocForm>();
                res = context.Delete("ItemId", id);
                return Json(GetFormResult(res, action, null, id), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Task

        [HttpGet]
        public ActionResult Tasks()
        {
            var userinfo = LoadUserInfo();
            TaskQuery model = new TaskQuery()
            {
                AccountId = userinfo.AccountId,
                UserId = userinfo.UserId
            };
            return View(userinfo, model);
        }
        [HttpPost]
        public ActionResult GetTasksGrid()
        {
            try
            {
                TaskQuery query = new TaskQuery(Request, false);
                var su = GetSignedUser(true);
                query.AccountId = su.AccountId;
                var list = TaskContext.ViewTasks(query);//.AccountId, query.UserId, query.AssignBy,query.TaskStatus);
                                                        //var row = list.FirstOrDefault<MemberListView>();
                                                        //int totalRows = row == null ? 0 : row.TotalRows;
                return Json(list, JsonRequestBehavior.AllowGet);
                //return QueryPagerServer<TaskListView>(list,su.UserId);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
        }

        

        [HttpGet]
        public ActionResult TaskUser()
        {
            return View(true);
        }

        [HttpPost]
        public JsonResult TaskUserKanban()
        {
            try
            {
                var userinfo = GetSignedUser(true);

                int status = Types.ToInt(Request["Status"]);
                //bool isshare = Types.ToBool(Request["IsShare"], false);
                int accountId = userinfo.AccountId;//GetAccountId();
                int userId = userinfo.UserId;// GetUser();
                var view = TaskContext.TaskUserKanban(accountId, userId, status);
                return Json(view, JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult TaskUserToday()
        {
            try
            {
                var userinfo = GetSignedUser(true);
                //bool isshare = Types.ToBool(Request["IsShare"], false);
                int accountId = userinfo.AccountId;//GetAccountId();
                int userId = userinfo.UserId;// GetUser();
                var view = TaskContext.TaskUserToday(accountId, userId);
                return Json(view, JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult TaskUserShare()
        {
            try
            {
                var userinfo = GetSignedUser(true);
                //bool isshare = Types.ToBool(Request["IsShare"], false);
                int accountId = userinfo.AccountId;//GetAccountId();
                int userId = userinfo.UserId;// GetUser();
                var view = TaskContext.TaskUserShare(accountId, userId);
                return Json(view, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult TaskEdit(int id)
        {
            return View(true,new EditTaskModel() { Id = id,  Option = "e" });//, Data = TaskContext.Get(id) });
        }
        [HttpGet]
        public ActionResult TaskEdit_Pin(int id)
        {
            return View(true, "TaskEdit", new EditTaskModel() { Id = id, Option = "e", Layout="_ViewIframe" });
        }

        [HttpGet]
        public ActionResult TaskInfo(int id)
        {
            //return View(true, "TaskEdit", new EditTaskModel() { Id = id, Option = "g", Data = TaskContext.Get(id) });
            return View(true, new EditTaskModel() { Id = id, Option = "g", IsInfo = true });//, Data = TaskContext.GetInfo(id) });
        }
        [HttpGet]
        public ActionResult TaskStart(int id)
        {
            var userinfo = LoadUserInfo();

            int accountId = userinfo.AccountId;// GetAccountId();
            int user = userinfo.UserId;// GetUser();
            TaskContext.Task_Status_Change(id, user, 2, "אתחול משימה", null);
            return View(userinfo, "TaskEdit", new EditTaskModel() { Id = id, Option = "e" });//, Data = TaskContext.Get(id) });
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
            
            string json = "";
            TaskItem item = null;
            if (id > 0)
            {
                item = TaskContext.Get(id);//, accountId);
            }
            else
            {
                int accountId = GetAccountId();
                item = new TaskItem() { AccountId = accountId };// DbSystem.SysCounters(2) };
            }
            if (item != null)
                json = JsonSerializer.Serialize(item);
            return base.GetJsonResult(json);
        }

        [HttpPost]
        public ContentResult GetTaskInfo()
        {
            int id = Types.ToInt(Request["id"]);

            string json = "";
            TaskItem item = null;
            if (id > 0)
            {
                item = TaskContext.GetInfo(id);//, accountId);
            }
            else
            {
                int accountId = GetAccountId();
                item = new TaskItem() { AccountId = accountId };// DbSystem.SysCounters(2) };
            }
            if (item != null)
                json = JsonSerializer.Serialize(item);
            return base.GetJsonResult(json);
        }

        //[HttpPost]
        //public JsonResult GetTaskInfo(int id)
        //{
        //    var view = TaskContext.Get(id);
        //    string title = "פרטים";
        //    var model = new InfoModel() { Id = id, Title = title, Value = view.ToHtml() };
        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

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
        public JsonResult TaskExpired(int TaskId)
        {
            string action = "ארכוב משימה";
            try
            {
                var signedUser = GetSignedUser(true);
                int accountId = signedUser.AccountId;
                int user = signedUser.UserId;
                int res = TaskContext.Task_Expired(TaskId, user, "ארכוב משימה");

                return Json(FormResult.Get(res, action), JsonRequestBehavior.AllowGet);
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
                    var signedUser = GetSignedUser(true);
                    int accountId = signedUser.AccountId;
                    int user = signedUser.UserId;
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
            EntityValidator validator = EntityValidator.ValidateEntity(item, "עדכון משימה", "he");
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
            //var res = TaskContext.DoUpdate(item);
            var res=TaskContext.Task_AddOrUpdate(item);
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
                a.AccountId = GetAccountId();
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

                var model = new ResultModel() { Status = res, Title = "הסרת משימה", Message = message, Link = null, OutputId = 0 };

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
            var su = GetSignedUser(false);
            int id = Types.ToInt(Request["id"]);
            int accountId = su.AccountId;
            int userId = su.UserId;
            string json = "";
            var context = new TaskContext<TaskComment>(userId);
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
            var su = GetSignedUser(false);
            int accountId = su.AccountId;
            int userId = su.UserId;
            string json = "";
            var context = new TaskContext<TaskForm>(userId);
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
        //        int userId = GetUser();
        //        context = new TaskContext<TaskForm>(userId);
        //        context.Set(Request.Form);
        //        var cur = context.Current;
        //        if (cur.AssignBy == 0)
        //            cur.AssignBy = userId;
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
                int userId = GetUser();
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
        public JsonResult TaskFormChecked(int id,bool done)
        {
            TaskContext<TaskForm> context = null;
            try
            {
                int userId = GetUser();
                context = new TaskContext<TaskForm>(userId);
                var item = context.Get("ItemId", id);
                item.DoneStatus = done;
                if (done)
                {
                    item.DoneDate = DateTime.Now;
                    //if (item.StartDate == null)
                    //    item.StartDate = DateTime.Now;
                }
                else
                {
                    item.DoneDate = null;//new Nullable<DateTime>().Value;
                }

                if (item.AssignBy == 0)
                    item.AssignBy = userId;
                context.Set(item);
                var res = context.SaveChanges();
                return Json(context.GetFormResult(res, null), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(FormResult.GetError(context.EntityName, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult TaskFormStart(int id)
        {
            TaskContext<TaskForm> context = null;
            try
            {
                int userId = GetUser();
                var res = TaskContext.TaskFormStart(id, userId);
                return Json(new FormResult(res), JsonRequestBehavior.AllowGet);

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
                //context.Current.AccountId = accountId;
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
        public ActionResult TaskNew2(int pid)
        {
            //TaskContext.NewTaskId() 
            return View(true, new EditTaskModel() { PId = pid });
        }

        [HttpGet]
        public ActionResult TaskNew(int pid)
        {
            //TaskContext.NewTaskId() 
            return View(true,new EditTaskModel() { PId = pid });
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
                var signedUser = GetSignedUser(true);
                int user = signedUser.UserId;
                int accountId = signedUser.AccountId;
                if (a.TaskStatus < 1)
                    a.TaskStatus = 1;

                //if (a.TaskId > 0)
                //{
                //    res = TaskContext.DoUpdate(a);
                //}
                //else
                //{
                //    a.AssignByAccount = accountId;
                //    a.AccountId = accountId;
                //    a.AssignBy = user;
                //    res = TaskContext.DoInsert(a);
                //}

                if (a.TaskId == 0)
                {
                    a.AssignByAccount = accountId;
                    a.AccountId = accountId;
                    a.AssignBy = user;
                }

                int taskId = TaskContext.Task_AddOrUpdate(a);
                res = taskId > 0 ? 1 : 0;
                return Json(ResultModel.GetFormResult(res, action, null, taskId), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Reminder old
        /*
        [HttpPost]
        public ContentResult GetReminderEdit()
        {
            int id = Types.ToInt(Request["id"]);

            string json = "";
            TaskItem item = null;
            if (id > 0)
            {
                item = TaskContext.Get(id);//, accountId);
            }
            else
            {
                int accountId = GetAccountId();
                item = new TaskItem() { AccountId = accountId };// DbSystem.SysCounters(2) };
            }
            if (item != null)
                json = JsonSerializer.Serialize(item);
            return base.GetJsonResult(json);
        }

        [HttpGet]
        public ActionResult ReminderNew(int pid)
        {
            //TaskContext.NewTaskId() 
            return View(true,"Reminder",new EditTaskModel() { PId = pid });
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

            var model = new EditTaskModel() { Id = id, Option = "e" };//, Data = TaskContext.Get(id) };
            return View(true,"Reminder", model);
        }
        [HttpGet]
        public ActionResult ReminderInfo(int id)
        {
            return View(true,"Reminder", new EditTaskModel() { Id = id, Option = "g" });//, Data = TaskContext.Get(id) });
        }      

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult ReminderUpdate()
        {

            int res = 0;
            string action = "תזכורת";
            ReminderItem a = null;
            try
            {
                a = EntityContext.Create<ReminderItem>(Request.Form);

                EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת תזכורת", "he");
                if (!validator.IsValid)
                {
                    return Json(GetFormResult(-1, action, validator.Result, 0), JsonRequestBehavior.AllowGet);

                }
                int user = GetUser();
                int accountId = GetAccountId();
                if (a.TaskId > 0)
                {
                    res = TaskContext.Remainder_AddOrUpdate(a);
                }
                else
                {
                    a.UserId = user;
                    //a.AssignByAccount = accountId;
                    a.AccountId = accountId;
                    a.AssignBy = user;
                    res = TaskContext.Remainder_AddOrUpdate(a);
                }
                return Json(ResultModel.GetFormResult(res, action, null, a.TaskId), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        */
        #endregion

        #region Reminder

        [HttpPost]
        public ContentResult GetReminderEdit()
        {
            try
            {
                int id = Types.ToInt(Request["id"]);

                string json = "";
                ReminderItem item = null;

                var su = GetSignedUser(true);
                int accountId = su.AccountId;
                int userId = su.UserId;


                if (id > 0)
                {
                    item = ReminderContext.Get(accountId).Get("RemindId",id);//, accountId);
                }
                else
                {
                    item = new ReminderItem() { AccountId = accountId, AssignBy = userId };// DbSystem.SysCounters(2) };
                }
                if (item != null)
                    json = JsonSerializer.Serialize(item);
                return base.GetJsonResult(json);
            }
            catch (Exception ex)
            {
                return base.GetJsonResult(GetFormResult(-1, "תזכורת", ex.Message, 0).ToJson());
            }
        }

        [HttpGet]
        public ActionResult ReminderNew(int pid)
        {
            //TaskContext.NewTaskId() 
            return View(true, "Reminder", new EditTaskModel() { PId = pid });
        }

        [HttpGet]
        public ActionResult ReminderEdit(int id)
        {
            var model = new EditTaskModel() { Id = id, Option = "e" };
            return View(true, "Reminder", model);
        }
        [HttpGet]
        public ActionResult ReminderInfo(int id)
        {
            return View(true, "Reminder", new EditTaskModel() { Id = id, Option = "g", IsInfo = true });
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult ReminderUpdate()
        {

            int res = 0;
            string action = "תזכורת";
            ReminderItem a = null;
            try
            {
                a = EntityContext.Create<ReminderItem>(Request.Form);

                EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת תזכורת", "he");
                if (!validator.IsValid)
                {
                    return Json(GetFormResult(-1, action, validator.Result, 0), JsonRequestBehavior.AllowGet);

                }
                var su = GetSignedUser(true);
                int accountId = su.AccountId;
                int userId = su.UserId;
                               
                if (a.RemindId > 0)
                {
                    res = ReminderContext.Get(accountId).AddOrUpdate(a);
                }
                else
                {
                    //a.UserId = userId;
                    //a.AssignByAccount = accountId;
                    a.AccountId = accountId;
                    a.AssignBy = userId;
                    res = ReminderContext.Get(accountId).AddOrUpdate(a);
                }
                return Json(ResultModel.GetFormResult(res, action, null, a.RemindId), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult ArchiveReminder(int id)
        {

            int res = 0;
            string action = "ארכוב תזכורת";
            //MemberCategoryView a = null;
            try
            {

                int accountId = GetAccountId();
                res = ReminderContext.Get(accountId).Archive(id, accountId);
                string message = GetResultMessage(res, action);
                var model = new ResultModel() { Status = res, Title = action, Message = message, Link = null, OutputId = 0 };

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult ReminderExpired(int id)
        {
            int res = 0;
            string action = "אישור תזכורת";
            try
            {
                var su = GetSignedUser(true);
                res = ReminderContext.Reminder_Expired(id, su.UserId);
                var model = FormResult.GetResultMessage(res, action);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult ReminderNotify(int id)
        {
            int res = 0;
            string action = "אישור תזכורת";
            try
            {
                var su = GetSignedUser(true);
                res = ReminderContext.Reminder_Notify(id, su.UserId);
                var model = FormResult.GetResultMessage(res,"Notify");
                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult ReminderReaded(int id)
        {
            int res = 0;
            string action = "אישור תזכורת";
            try
            {
                var su = GetSignedUser(true);
                res = ReminderContext.Reminder_Readed(id, su.UserId);
                var model = FormResult.GetResultMessage(res, "Readed");
                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult DeleteReminder(int id)
        {
            int res = 0;
            string action = "הסרת תזכורת";
            try
            {
                int accountId = GetAccountId();
                //not supported
                res = ReminderContext.Get(accountId).Delete("RemindId", id, "AccountId",accountId);
                string message = GetResultMessage(res, action);
                var model = new ResultModel() { Status = res, Title = action, Message = message, Link = null, OutputId = 0 };

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetReminderItem(int id)
        {
            try
            {
                var su = GetSignedUser(true);

                var list = ReminderContext.View(su.AccountId, su.UserId, id);
                return Json(list, JsonRequestBehavior.AllowGet);


                //var item = ReminderContext.Get(su.AccountId).Get("RemindId",id);
                //return Json(item, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = FormResult.GetTrace<DbSystem>(ex, "הודעות", Request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetReminderGrid()
        {
            try
            {

                ReminderMode Mode = (ReminderMode)Types.ToInt(Request["Mode"]);
                int AccountId = Types.ToInt(Request["AccountId"]);
                int UserId = Types.ToInt(Request["UserId"]);
                bool Readed = Types.ToBool(Request["Readed"],false);
                int Remind_Parent = Types.ToInt(Request["Remind_Parent"]);
               

                //ReminderQuery query = new ReminderQuery(Request, false);
                var su = GetSignedUser(true);
                AccountId = su.AccountId;
                UserId = su.UserId;

                //var parameters=QueryFilter.GetParameters("PageSize", 0, "PageNum", 0, "Mode", (int)Mode, "AccountId", AccountId, "UserId", UserId, "AssignBy", AssignBy, "RemindStatus", RemindStatus)

                var list = ReminderContext.View(Mode, AccountId, UserId, Readed, Remind_Parent);
                return Json(list, JsonRequestBehavior.AllowGet);

                //var row = list.FirstOrDefault<MemberListView>();
                //int totalRows = row == null ? 0 : row.TotalRows;
                //return QueryPagerServer<ReminderListView>(list,su.UserId);
            }
            catch (Exception ex)
            {
                var result=FormResult.GetTrace<DbSystem>(ex,"הודעות", Request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region TaskForm Template

        [HttpGet]
        public ActionResult TasksFormTemplate()
        {
            int accountId=GetAccountId();
            var db=new TaskContext<TaskFormTemplate>(GetUser());
            var list=db.ExecOrViewList("AccountId", accountId);
            var model = new EntityModel()
            {
                Data = Json(db.ExecOrViewList("AccountId", accountId))
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult GetTasksFormTemplate(int FormId)
        {
            var db = new TaskContext<TaskFormTemplate>(0);
            return Json(db.ExecOrViewList("FormId", FormId), JsonRequestBehavior.AllowGet);
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
        public ActionResult TicketNew(int pid)
        {
            //TaskContext.NewTaskId()
            return View(true,new EditTaskModel() { PId = pid });
        }

        [HttpGet]
        public ActionResult _TicketNew()
        {
            //TaskContext.NewTaskId()
            return PartialView(true, new EditTaskModel() { PId = 0 });
        }

        [HttpGet]
        public ActionResult TicketStart(int id)
        {
            int accountId = GetAccountId();
            int user = GetUser();
            TaskContext.Task_Status_Change(id, user, 2, "אתחול כרטיס", null);
            return View(true, "TicketEdit", new EditTaskModel() { Id = id, Option = "e" });
        }
           
        [HttpGet]
        public ActionResult TicketEdit(int id)
        {
            return View(true,new EditTaskModel() { Id = id, Option = "e" });
        }
        [HttpGet]
        public ActionResult TicketInfo(int id)
        {
            return View(true, new EditTaskModel() { Id = id, Option = "g", IsInfo = true });
        }
       
        

        #endregion

        #region Calendar

        [HttpGet]
        public ActionResult Calendar()
        {
            return View(true,new EditTaskModel() { PId = 0,  Option ="g" });
        }

        [HttpPost]
        public JsonResult CalendarGetItems()
        {
            string viewType = Request.Form["view"];
            int user = Types.ToInt(Request.Form["user"]);
            string  sdateFrom = Request.Form["from"];
            string sdateTo = Request.Form["to"];

            DateTime dateFrom = Types.ToDateTime(sdateFrom);
            DateTime dateTo = Types.ToDateTime(sdateTo);
            if (user == 0)
                user = GetUser();
            int accountId = GetAccountId();
            
            var calendar = new CalendarContext(user);
            var list = calendar.GetListItems(accountId, user, dateFrom, dateTo);
            //var list = calendar.GetList("TimeFrom",dateFrom,"TimeTo",dateTo);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AppointmentAdd()
        {
            string action = "פגישה";
            try
            {
                int accountId = GetAccountId();
                var user = GetUser();

                var item = EntityExtension.Create<CalendarItem>(Request.Form);
                item.ModifiedDate = DateTime.Now;
                item.AccountId = accountId;
                var calendar = new CalendarContext(user);
                calendar.Set(item);
                var res = calendar.Insert();
                return Json(calendar.GetFormResult(res, null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AppointmentChange()
        {
            string action = "פגישה";
            try
            {
                int accountId = GetAccountId();
                var user = GetUser();

                var item = EntityExtension.Create<CalendarItem>(Request.Form);
                item.UserId = item.GetUserId(user);
                item.ModifiedDate = DateTime.Now;
                item.AccountId = accountId;
                var calendar = new CalendarContext(user);
                calendar.Delete("CalendarId", item.CalendarId);
                calendar.Set(item);
                var res = calendar.Insert();
                return Json(calendar.GetFormResult(res, null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AppointmentDelete()
        {
            string action = "פגישה";
            try
            {
                int accountId = GetAccountId();
                var user = GetUser();

                var item = EntityExtension.Create<CalendarItem>(Request.Form);
                var calendar = new CalendarContext(user);
                var res=calendar.Delete("CalendarId", item.CalendarId);
                return Json(FormResult.Get(res,"ביטול פגישה"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult CalendarUpdate()
        {

            int res = 0;
            string action = "פגישה";
            TaskItem a = null;
            try
            {
                a = EntityContext.Create<TaskItem>(Request.Form);

                EntityValidator validator = EntityValidator.ValidateEntity(a, action, "he");
                if (!validator.IsValid)
                {
                    return Json(GetFormResult(-1, action, validator.Result, 0), JsonRequestBehavior.AllowGet);

                }
                int user = GetUser();
                int accountId = GetAccountId();
                //if (a.TaskId > 0)
                //{
                //    res = TaskContext.DoUpdate(a);
                //}
                //else
                //{
                //    a.UserId = user;
                //    a.AssignByAccount = accountId;
                //    a.AccountId = accountId;
                //    a.AssignBy = user;
                //    res = TaskContext.DoInsert(a);
                //}

                if (a.TaskId == 0)
                {
                    a.UserId = user;
                    a.AssignByAccount = accountId;
                    a.AccountId = accountId;
                    a.AssignBy = user;
                }
                res = TaskContext.Task_AddOrUpdate(a);
                return Json(ResultModel.GetFormResult(res, action, null, a.TaskId), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Project


        [HttpGet]
        public ActionResult Project()
        {
            var userinfo = LoadUserInfo();
            QueryModel model = new QueryModel()
            {
                AccountId = userinfo.AccountId,
                UserId = userinfo.UserId
            };
            return View(userinfo, model);
        }

        [HttpPost]
        public ActionResult GetProjectGrid()
        {

            var userinfo = LoadUserInfo();
            QueryModel model = new QueryModel()
            {
                AccountId = userinfo.AccountId,
                UserId = userinfo.UserId,
                //Args[""]=Types.ToInt(Request[""])
            };

            var list = ProjectContext.Get(userinfo.AccountId).ViewList("AccountId", model.AccountId, "UserId", model.UserId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ProjectNew(int pid)
        {
            return View(true, new EditTaskModel() { PId = pid });
        }

        //[HttpGet]
        //public ActionResult ProjectStart(int id)
        //{
        //    int accountId = GetAccountId();
        //    int user = GetUser();
        //    ProjectContext.Task_Status_Change(id, user, 2, "אתחול מסמך", null);
        //    return View(true, "DocEdit", new EditTaskModel() { Id = id, Option = "e" });//, Data = TaskContext.Get(id) });
        //}

        [HttpGet]
        public ActionResult ProjectEdit(int id)
        {
            return View(true, new EditTaskModel() { Id = id, Option = "e" });//, Data = TaskContext.Get(id) });
        }
        [HttpGet]
        public ActionResult ProjectInfo(int id)
        {
            return View(true, "ProjectEdit", new EditTaskModel() { Id = id, Option = "g", IsInfo = true });//, Data = TaskContext.Get(id) });
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult UpdateNewProject()
        {
            string action = "עדכון פרוייקט";
            try
            {
                var su = GetSignedUser(true);
                int user = su.UserId;
                int accountId = su.AccountId;

                var context=ProjectContext.Get(accountId);
                context.Set(Request.Form);
                context.Current.UserId = su.UserId;
                context.Current.AccountId = su.AccountId;
                var validator = context.Validate(action, "he");
                if (!validator.IsValid)
                {
                    return Json(FormResult.GetFormResult(-1, action, validator.Result), JsonRequestBehavior.AllowGet);
                }
                var res=context.Insert();
                res.Set(FormResult.GetResultMessage(res.AffectedRecords, action), action);
                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(FormResult.GetFormResult(-1, action, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult UpdateProjectc()
        {
            string action = "עדכון פרוייקט";
            try
            {

                var su = GetSignedUser(true);
                var context = ProjectContext.Get(su.AccountId);
                context.Set(Request.Form);
                context.Current.AccountId = su.AccountId;
                context.Current.UserId = su.UserId;
                var validator = context.Validate(action, "he");
                if (!validator.IsValid)
                {
                    return Json(new FormResult()
                    {
                        Message = validator.Result,
                        Status = -1,
                        Title = action
                    });
                }
                var res = context.SaveChanges();
                res.Set(FormResult.GetResultMessage(res.AffectedRecords,action), action);
                return Json(res);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetFormResult(-1, action, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult ArchiveProject(int id)
        {

            int res = 0;
            string action = "ארכוב פרוייקט";
            try
            {
                int accountId = GetAccountId();
                res = ProjectContext.Get(accountId).ArchiveDocs(id, accountId);
                return Json(new FormResult(res,action), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(FormResult.GetFormResult(-1, action, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult DeleteProject(int id)
        {

            int res = 0;
            string action = "הסרת פרוייקט";
            try
            {
                int accountId = GetAccountId();
                //not supported
                //res = ProjectContext.Get(accountId).Delete("DocId",id, "AccountId",accountId);
                string message = res > 0 ? "המסמך הוסר בהצלחה" : "המסמך לא הוסר";
                var model = new ResultModel() { Status = res, Title = action, Message = message, Link = null, OutputId = 0 };

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(FormResult.GetFormResult(-1, action, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region News


        #endregion

    }
}
