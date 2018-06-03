using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using Nistec;
using ProSystem;
using System.Data;
using Nistec.Web.Controls;
using System.Web;
using Nistec.Generic;
using Nistec.Serialization;
using ProSystem.Query;

namespace ProSystem.Data.Entities
{

    public class TaskContext<T> : EntityContext<DbSystem, T> where T : IEntityItem
    {

        public static void Refresh(int AccountId)
        {
            DbContextCache.Remove<T>(Settings.ProjectName, EntityCacheGroups.Task, AccountId, 0);
        }
        public static TaskContext<T> Get(int userId)
        {
            return new TaskContext<T>(userId);
        }
        public TaskContext(int userId)
        {
            if (userId > 0)
                CacheKey = DbContextCache.GetKey<T>(Settings.ProjectName, EntityCacheGroups.Task, 0, userId);
        }
        public IList<T> GetList()
        {
            //int ttl = 3;
            return DbContextCache.EntityList<DbSystem, T>(CacheKey, null);
        }
        public IList<T> GetList(int taskId)
        {
            //int ttl = 3;
            return DbContextCache.EntityList<DbSystem, T>(CacheKey, new object[] { "Task_Id", taskId });
        }
        protected override void OnChanged(ProcedureType commandType)
        {
            DbContextCache.Remove(CacheKey);
        }
        public FormResult GetFormResult(EntityCommandResult res, string reason)
        {
            return FormResult.Get(res, EntityName, reason);//.GetFormResult(res.AffectedRecords, this.EntityName, reason, res.GetIdentityValue<int>());
        }
    }


    [Entity(EntityName = "TaskContext", MappingName = "Task", ConnectionKey = "netcell_system", EntityKey = new string[] { "TaskId" })]
    public class TaskContext : EntityContext<TaskItem>
    {
        public const string MappingName = "Task";

        #region ctor

        public TaskContext()
        {
        }

        public TaskContext(int TaskId, int AccountId)
            : base(TaskId)
        {
            if (Entity.AccountId != AccountId)
            {
                throw new ArgumentException("Incorrecrt account");
            }
        }

        //public TaskContext(int RecordId)
        //    : base()
        //{
        //    SetByParam("RecordId", RecordId);
        //}

        #endregion

        #region update

     /*
        public static int DoInsert(TaskItem entity)
        {
            entity.TaskId=TaskContext.NewTaskId();
            return DoSave(entity.TaskId, entity.AccountId, entity, UpdateCommandType.Insert);
        }
        public static int DoUpdate(TaskItem entity)
        {
           return DoSave(entity.TaskId, entity.AccountId, entity, UpdateCommandType.Update);
        }

        public static int DoSave(int TaskId, int AccountId, TaskItem entity, UpdateCommandType commandType)
        {
            if (commandType == UpdateCommandType.Delete)
                using (TaskContext context = new TaskContext(TaskId, AccountId))
                {
                    return context.SaveChanges(commandType);
                }

            EntityValidator.Validate(entity, "משימה", "he");

            if (commandType == UpdateCommandType.Insert)
                using (TaskContext context = new TaskContext())
                {
                    context.Set(entity);
                    return context.SaveChanges(commandType);
                }

            if (commandType == UpdateCommandType.Update)
                using (TaskContext context = new TaskContext(TaskId, AccountId))
                {
                    context.Set(entity);
                    return context.SaveChanges(commandType);
                }
            return 0;
        }
*/
        public static int Task_AddOrUpdate(TaskItem item)
        {
            object[] values = new object[]
            {
"TaskId", item.TaskId
,"TaskSubject", item.TaskSubject
,"TaskBody", item.TaskBody
,"Task_Type", item.Task_Type
,"Task_Parent", item.Task_Parent
,"Project_Id", item.Project_Id
,"UserId", item.UserId
//,"CreatedDate", item.CreatedDate
,"DueDate", item.DueDate
,"StartedDate", item.StartedDate
,"EndedDate", item.EndedDate
,"EstimateStartTime", item.EstimateStartTime
,"EstimateTakenTime", item.EstimateTakenTime
,"AccountId", item.AccountId
,"TaskStatus", item.TaskStatus
//,"IsShare",item.IsShare
,"Priority", item.Priority
,"Budget",item.Budget
,"TotalTime", item.TotalTime
,"TimePart", item.TimePart
,"AssignBy", item.AssignBy
,"ColorFlag", item.ColorFlag
,"TeamId", item.TeamId
//,"LastUpdate", item.LastUpdate
,"LastAct", item.LastAct
,"RemindDate", item.DueDate
,"TaskModel", item.TaskModel
,"ClientId", item.ClientId
,"ClientDetails", item.ClientDetails
,"AssignByAccount", item.AssignByAccount
,"Tags", item.Tags
,"Lang", item.Lang
,"AssignTo", item.AssignTo
,"ShareType", item.ShareType
,"Folder", item.Folder
            };


            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteReturnValue("sp_Task_AddOrUpdate", 0, values);
        }

        #endregion

        #region reminder
        /*
        public static int Remainder_AddOrUpdate(ReminderItem item)
        {
            object[] values = new object[]
            {
            "TaskId",item.TaskId
            ,"TaskSubject",item.TaskSubject
            ,"TaskBody",item.TaskBody
            ,"Task_Parent",item.Task_Parent
            ,"Project_Id",item.Project_Id
            ,"UserId",item.UserId
            ,"DueDate",item.DueDate
            ,"AccountId",item.AccountId 
            ,"TaskStatus",item.TaskStatus 
            //,"IsShare",item.IsShare 
            ,"AssignBy",item.AssignBy 
            ,"ColorFlag",item.ColorFlag 
            ,"RemindDate",item.RemindDate
            ,"ClientId",item.ClientId
            ,"Tags",item.Tags
            ,"Lang",item.Lang
            ,"AssignTo",item.AssignTo
            ,"ShareType",item.ShareType
            };


            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteReturnValue("sp_Task_Reminder_AddOrUpdate", 0, values);
        }
        */
        #endregion

        #region static

        public static int NewTaskId()
        {
            return DbSystem.SysCounters(2);
        }
        public static Guid NewTaskKey()
        {
            return UUID.NewUuid();//..UniqueId();
        }
        public static int Task_Status_Change(int TaskId, int UserId, int Status, string Act, string ColorFlag)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteReturnValue("sp_Task_Status_Change",0, "TaskId", TaskId, "UserId", UserId, "Status", Status,"Act",Act,"ColorFlag",ColorFlag);
        }
        public static int Task_Expired(int TaskId, int UserId, string Act)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteReturnValue("sp_Task_Status_Change", 0, "TaskId", TaskId, "UserId", UserId, "Status", 16, "Act", Act, "IsExpired", true);
        }

        public static IEnumerable<TaskUserItem> TaskUserKanban(int AccountId, int UserId, int Status = 0)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteList<TaskUserItem>("sp_Task_User_kanban_v1", "AccountId", AccountId, "UserId", UserId, "Status", Status);
        }
        public static IEnumerable<TaskItem> TaskUserToday(int AccountId, int UserId)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteList<TaskItem>("sp_Task_User_Today", "AccountId", AccountId, "UserId", UserId);
        }
        public static IEnumerable<TaskItem> TaskUserShare(int AccountId, int UserId)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteList<TaskItem>("sp_Task_User_Share", "AccountId", AccountId, "UserId", UserId);
        }

        //public static IEnumerable<TaskItem> TaskUserList(int AccountId, int UserId, int Status=0)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //        return db.ExecuteList<TaskItem>("sp_Task_User", "AccountId", AccountId, "UserId", UserId, "Status", Status);
        //}

        //public static IEnumerable<TaskItem> TaskUserList(int AccountId,int UserId, int Status, bool IncludeShare, int ProjectId, int TaskParent)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //        return db.ExecuteList<TaskItem>("sp_Task_User", "AccountId",AccountId,"UserId",UserId,"Status",Status,"IncludeShare",IncludeShare,"ProjectId",ProjectId,"TaskParent",TaskParent);
        //}

        public static IEnumerable<TaskItem> TaskList()
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.EntityItemList<TaskItem>(MappingName, null);
        }
        public static TaskItem Get(int TaskId)
        {
            if (TaskId==0)
                return new TaskItem();
            using (var db = DbContext.Create<DbSystem>())
            {
                return db.EntityItemGet<TaskItem>(MappingName, "TaskId", TaskId);
            }
        }
        public static TaskItemInfo GetInfo(int TaskId)
        {
            if (TaskId == 0)
                return new TaskItemInfo();
            using (var db = DbContext.Create<DbSystem>())
            {
                return db.EntityProcGet<TaskItemInfo>("TaskId", TaskId);
            }
        }
        public static string GetJson(int TaskId)
        {
            using (var db = DbContext.Create<DbSystem>())
            {
                //var dt= db.QueryDataTable(MappingName, "TaskId", TaskId);
                //string json = dt.ToJson();
                //var data = JsonSerializer.Deserialize<DataTable>(json);
                //var result = new JsonResults()
                //{
                //    Result = json,
                //    TypeName = typeof(DataTable).FullName,
                //    EncodingName = "utf-8"
                //};
                //return result;

                return db.QueryJsonRecord(MappingName, "TaskId", TaskId);
            }
        }

        public static JsonResults GetJsonResults(int TaskId)
        {
            using (var db = DbContext.Create<DbSystem>())
            {
                //var dt= db.QueryDataTable(MappingName, "TaskId", TaskId);
                //string json = dt.ToJson();
                //var data = JsonSerializer.Deserialize<DataTable>(json);
                //var result = new JsonResults()
                //{
                //    Result = json,
                //    TypeName = typeof(DataTable).FullName,
                //    EncodingName = "utf-8"
                //};
                //return result;

                return db.QueryJsonResults(MappingName, "TaskId", TaskId);
            }
        }

        //public static DataTable GetReminders()
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //    {
        //        var dt = db.QueryDataTable(MappingName, "TaskModel", "R");
        //        return dt;
               
        //    }
        //}

        //public static IEnumerable<TaskListView> ViewByAccount(int AccountId)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //        return db.EntityItemList<TaskListView>("vw_Task", "AccountId", AccountId);
        //}

        //public static IEnumerable<TaskListView> ViewByEmployee(int AccountId, int UserId)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //        return db.EntityItemList<TaskListView>("vw_Task", "AccountId", AccountId, "UserId", UserId);
        //}
        //public static IEnumerable<TaskListView> ViewByUser(int AccountId, int UserId)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //        return db.EntityItemList<TaskListView>("vw_Task", "AccountId", AccountId, "UserId", UserId);
        //}

        public static IEnumerable<TaskListView> ViewDocs(int AccountId, int UserId, int AssignBy, int TaskStatus)
        {
            int PageSize = 0;
            int PageNum = 0;

            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteList<TaskListView>("sp_Task_Docs_Report", "PageSize", PageSize, "PageNum", PageNum, "AccountId", AccountId, "UserId", UserId, "AssignBy", AssignBy, "TaskStatus", TaskStatus);
        }
        public static IEnumerable<TaskListView> ViewTopics(int AccountId, int UserId, int AssignBy, int TaskStatus)
        {
            int PageSize = 0;
            int PageNum = 0;

            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteList<TaskListView>("sp_Task_Topic_Report", "PageSize", PageSize, "PageNum", PageNum, "AccountId", AccountId, "UserId", UserId, "AssignBy", AssignBy, "TaskStatus", TaskStatus);
        }
        public static IEnumerable<SubTaskListView> ViewSubTask(int AccountId, int UserId, int AssignBy, int TaskStatus,DateTime? DateFrom, DateTime? DateTo)
        {
            int PageSize = 0;
            int PageNum = 0;

            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteList<SubTaskListView>("sp_Task_Report_Action", "PageSize", PageSize, "PageNum", PageNum, "AccountId", AccountId, "UserId", UserId, "AssignBy", AssignBy, "TaskStatus", TaskStatus, "DateFrom", DateFrom, "DateTo", DateTo);
        }

        //public static IEnumerable<TaskListView> ViewTasks(int AccountId, int UserId, int AssignBy, int TaskStatus)
        //{
        //    int PageSize=0;
        //    int PageNum=0;
            
        //    using (var db = DbContext.Create<DbSystem>())
        //        return db.ExecuteList<TaskListView>("sp_Task_Report", "PageSize",PageSize,"PageNum",PageNum, "AccountId", AccountId, "UserId", UserId,"AssignBy",AssignBy,"TaskStatus",TaskStatus);
        //}
        
         public static IEnumerable<TaskListView> ViewTasks(TaskQuery q)
        {

            using (var db = DbContext.Create<DbSystem>())
                return db.ExecuteList<TaskListView>("sp_Task_Report", "PageSize", q.PageSize, "PageNum", q.PageNum, "AccountId", q.AccountId, "UserId", q.UserId, "AssignBy", q.AssignBy, "TaskStatus", q.TaskStatus);
        }
        public static IList<string> ViewTagsList(int AccountId)
        {
            string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId, "Tags");
            return WebCache.GetOrCreateList<string>(key, () => GetTagsList(AccountId), EntityProCache.DefaultCacheTtl);
        }

        public static IList<string> GetTagsList(int AccountId)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.Query<string>("select Tag from Tags where AccountId=@AccountId", "AccountId", AccountId);
        }

        public static string ViewTagsJson(int AccountId)
        {
            string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId, "Tags");
            return WebCache.GetOrCreate<string>(key, () => GetTagsJson(AccountId), EntityProCache.DefaultCacheTtl);
        }

        public static string GetTagsJson(int AccountId)
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.QueryJson("select Tag from Tags where AccountId=@AccountId", "AccountId", AccountId);
        }


        //public static IList<string> ViewTaskFoldersList(int AccountId, string TaskModel)
        //{
        //    string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId, "Tags");
        //    return WebCache.GetOrCreateList<string>(key, () =>
        //    {
        //        using (var db = DbContext.Create<DbSystem>())
        //            return db.Query<string>("select Folder from Tasks where AccountId=@AccountId", "AccountId", AccountId, "TaskModel", TaskModel);
        //    },
        //    EntityProCache.DefaultCacheTtl);
        //}
        //public static IList<string> GetTaskFoldersList(int AccountId, string TaskModel)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //        return db.Query<string>("select Folder from Tasks where AccountId=@AccountId", "AccountId", AccountId, "TaskModel", TaskModel);
        //}

        //public static string ViewTaskFoldersJson(int AccountId, string TaskModel)
        //{
        //    string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, AccountId, "Tags");
        //    return WebCache.GetOrCreate<string>(key, () => GetTagsJson(AccountId), EntityProCache.DefaultCacheTtl);
        //}

        public static string GetTasksFoldersJson(int AccountId)//, string TaskModel)
        {
            using (var db = DbContext.Get<DbSystem>())
                return db.QueryJsonArray("select Folder from Folders where AccountId=@AccountId", "AccountId", AccountId);
        }

        #endregion

        #region edit/add

        public static EntityCommandResult TaskTimerUpdate(TaskTimer item)
        {
            int Mode = item.TaskTimerId > 0 ? 1 : 0;
            int res = 0;
            using (IDbContext Db = DbContext.Create<DbSystem>())
            {
                res = Db.ExecuteReturnValue("sp_Task_Timer", 0, "Mode", Mode, "TaskId", item.Task_Id, "UserId", item.UserId, "TaskTimerId", item.TaskTimerId, "Subject", item.Subject);
            }
            return new EntityCommandResult((res > 0) ? 1 : 0, res, "TaskTimerId");
        }
        public static EntityCommandResult TaskFormTemplateCreate(int TaskId,int AccountId,int UserId, string FormName)
        {
            int res = 0;
            using (IDbContext Db = DbContext.Create<DbSystem>())
            {
                res = Db.ExecuteReturnValue("sp_Task_Form_Template_Create", 0, "TaskId", TaskId, "AccountId", AccountId, "UserId", UserId, "FormName", FormName);
            }
            return new EntityCommandResult((res > 0) ? 1 : 0, res, "FormId");
        }
         public static int TaskFormByTemplate(int TaskId,int UserId, int FormId)
        {
            int res = 0;
            using (IDbContext Db = DbContext.Create<DbSystem>())
            {
                res = Db.ExecuteNonQuery("sp_Task_Form_ByTemplate", "TaskId", TaskId, "UserId", UserId, "FormId", FormId);
            }
            return res;
        }

        public static int TaskFormStart(int ItemId, int UserId)
        {
            int res = 0;
            using (var Db = DbContext.Get<DbSystem>())
            {
                res = Db.ExecuteCommandUpdate("Task_Form", "StartDate=@StartDate,UserId=@UserId", "ItemId=@ItemId", "StartDate", DateTime.Now, "UserId", UserId, "ItemId", ItemId);
            }
            return res;
        }
        public static int TopicTaskFormStart(int TopicId ,int ItemId)//,string TaskSubject, string TaskBody, int AssignTo,DateTime TaskDueDate)
        {
            int res = 0;
            using (var Db = DbContext.Get<DbSystem>())
            {
                res = Db.ExecuteNonQuery("sp_Task_Topic_Form_Add", "TopicId", TopicId, "ItemId", ItemId);//, "TaskSubject", TaskSubject, "TaskBody", TaskBody, "AssignTo", AssignTo, "TaskDueDate",TaskDueDate);
            }
            return res;
        }
        public static int DocTaskFormStart(int DocId, int ItemId)//,string TaskSubject, string TaskBody, int AssignTo,DateTime TaskDueDate)
        {
            int res = 0;
            using (var Db = DbContext.Get<DbSystem>())
            {
                res = Db.ExecuteNonQuery("sp_Task_Doc_Form_Add", "DocId", DocId, "ItemId", ItemId);//, "TaskSubject", TaskSubject, "TaskBody", TaskBody, "AssignTo", AssignTo, "TaskDueDate",TaskDueDate);
            }
            return res;
        }
        //public static int TaskTimerStart(int TaskId,int UserId, string Subject)
        //{
        //    int Mode = 0;
        //    int res = 0;
        //    using (IDbContext Db = DbContext.Create<DbSystem>())
        //    {
        //        res = Db.ExecuteReturnValue("sp_Task_Timer",0,"Mode",Mode, "TaskId", TaskId, "UserId", UserId,"TaskTimerId",0, "Subject", Subject);
        //    }
        //    return res;
        //}
        //public static int TaskTimerEnd(int TaskId, int UserId, int TaskTimerId)
        //{
        //    int Mode = 1;
        //    int res = 0;
        //    using (IDbContext Db = DbContext.Create<DbSystem>())
        //    {
        //        res = Db.ExecuteReturnValue("sp_Task_Timer", 0, "Mode", Mode, "TaskId", TaskId, "UserId", UserId, "TaskTimerId", TaskTimerId);
        //    }
        //    return res;
        //}
        //public static int Add_Task_Assign(TaskAssignment task)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //    return db.EntityInsert<TaskAssignment>(task);
        //}
        //public static int Add_Task_Assign(int TaskId, int AccountId, int UserId_AssignedBy, int UserId_AssignedTo)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //    return db.ExecuteNonQuery("sp_Task_Assign", "TaskId", TaskId, "AccountId", AccountId, "UserId_AssignedBy", UserId_AssignedBy, "UserId_AssignedTo", UserId_AssignedTo);
        //}
        //public static int Add_Task_Comment(int TaskId, int AccountId, int UserId, string CommentText, string Attachment)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //    return db.ExecuteNonQuery("sp_Task_Comment", "TaskId", TaskId, "AccountId", AccountId, "UserId", UserId, "CommentText", CommentText, "Attachment", Attachment);
        //}
        //public static int Add_Task_Timer(TaskComment task)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //    return db.EntityInsert<TaskComment>(task);
        //}
        //public static int Add_Task_Timer(TaskTimer task)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //    return db.EntityInsert<TaskTimer>(task);
        //}
        #endregion

        #region get lists
        public static int Get_Task_Items(int AccountId, int UserId, int Status, bool IncludeShare, int ProjectId = 0, int TaskParent = 0)
        {
            using (var db = DbContext.Create<DbSystem>())
            return db.ExecuteNonQuery("sp_Task_Items_Get", "AccountId", AccountId, "UserId", UserId, "Status", Status, "IncludeShare", IncludeShare, "ProjectId", ProjectId, "TaskParent", TaskParent);
        }

        //public static IEnumerable<TaskAssignment> Get_TaskAssignments(int Task_Id)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //        return db.EntityList<TaskAssignment>("vw_Task_Assignments", "Task_Id", Task_Id);
        //}
        //public static IEnumerable<TaskComment> Get_TaskComments(int Task_Id)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //        return db.EntityList<TaskComment>("vw_Task_Comments", "Task_Id", Task_Id);
        //}
        //public static IEnumerable<TaskTimer> Get_TaskTimers(int Task_Id)
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //        return db.EntityList<TaskTimer>("vw_Task_Timers", "Task_Id", Task_Id);
        //}
        #endregion

    }
    [EntityMapping("vw_Task")]
    public class TaskListView : TaskItem, IEntityListItem
    {
        public static IList<TaskListView> GetList(int UserId, int userId, int ttl)
        {
            string key = DbContextCache.GetKey<TaskListView>(Settings.ProjectName, EntityCacheGroups.Enums, 0, userId);
            return DbContextCache.EntityList<DbSystem, TaskListView>(key, ttl, new object[]{"UserId", UserId});
        }

        public string ProjectName { get; set; }
        public string StatusName { get; set; }
        //public string DisplayName { get; set; }
        public string TaskTypeName { get; set; }
        //public string AssignByName { get; set; }
        public string TotalTimeView { get; set; }
        public int TotalRows { get; set; }

    }
    [EntityMapping(ProcListView = "sp_Task_Report_Action")]
    public class SubTaskListView : IEntityItem
    {
        public int TaskParent { get; set; }
        public int SubId { get; set; }
        public int TaskId { get; set; }
        public DateTime SubDate { get; set; }
        public string SubType { get; set; }
        public string SubText { get; set; }
        public string TaskSubject { get; set; }
        public int SubUserId { get; set; }
        public string SubDisplayName { get; set; }
        public int TaskStatus { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public string StatusName { get; set; }
        public string AssignByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int RowId { get; set; }
        public int TotalRows { get; set; }

    }
    public class TaskUserItem : IEntityItem
    {

        [EntityProperty(EntityPropertyType.Key)]
        public int TaskId { get; set; }
        public string TaskSubject { get; set; }
        public string TaskTypeName { get; set; }
        public string TaskBody { get; set; }

        [EntityProperty(EntityPropertyType.View)]
        public DateTime CreatedDate { get; set; }
        public string ColorFlag { get; set; }
        public string TaskState { get; set; }
        public string TaskModel { get; set; }
        public string Tags { get; set; }
    }

    [EntityMapping(ProcGet = "sp_Task_Get_Info")]
    public class TaskItemInfo : TaskItem
    {
        public int Comments { get; set; }
        public int Assigns { get; set; }
        public int Timers { get; set; }
        public int Items { get; set; }
        public int Files { get; set; }

    }

    [EntityMapping("Task","vw_Task","משימה")]
    public class TaskItem : IEntityItem
    {

        [EntityProperty(EntityPropertyType.Key)]
        public int TaskId { get; set; }
        public string TaskSubject { get; set; }
        public string TaskBody { get; set; }
        public int Task_Type { get; set; }
        public int Task_Parent { get; set; }
        public int Project_Id { get; set; }
        public int UserId { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? EndedDate { get; set; }
        public DateTime? EstimateStartTime { get; set; }
        public int EstimateTakenTime { get; set; }
        public int AccountId { get; set; }
        public int TaskStatus { get; set; }
        //public bool IsShare { get; set; }
        public int ShareType { get; set; }
        public int Priority { get; set; }
        public decimal Budget { get; set; }
        public int TotalTime { get; set; }
        public string TimePart { get; set; }
        public int AssignBy { get; set; }
        public string ColorFlag { get; set; }
        public int TeamId { get; set; }
        //public Guid TaskKey { get; set; }
        public string LastAct{ get; set; }
        [EntityProperty(EntityPropertyType.Optional)]
        public DateTime LastUpdate { get; set; }

        [EntityProperty(EntityPropertyType.Optional)]
        public string TaskState { get; set; }
        //[EntityProperty(EntityPropertyType.Optional)]
        //public string resourceId { get; set; }
        

        [EntityProperty(EntityPropertyType.Optional)]
        public string StatusNameLocal { get; set; }
        // [EntityProperty(EntityPropertyType.Optional)]
        //public string TaskHex { get; set; }
         [EntityProperty(EntityPropertyType.Optional)]
         public string DisplayName { get; set; }
         [EntityProperty(EntityPropertyType.Optional)]
         public string AssignByName { get; set; }
         [EntityProperty(EntityPropertyType.Optional)]
         public bool RemindToday { get; set; }
         public string TaskModel { get; set; }
         public int ClientId { get; set; }
         public int AssignByAccount { get; set; }
         public string ClientDetails { get; set; }
         public string Tags { get; set; }
        public string Lang { get; set; }
        public string AssignTo { get; set; }
        public string Folder { get; set; }
        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<TaskItem>(this, null, null, true);
        }
    }

//    [EntityMapping("Task", "vw_Task", "תזכורת")]
//    public class ReminderItem : IEntityItem
//    {

///*
//sp_Task_Reminder_Add
//@TaskId int
//,@TaskSubject varchar(50)
//,@TaskBody varchar(max)
//,@Task_Parent int
//,@Project_Id int
//,@UserId int
//, @DueDate datetime
//,@AccountId int
//,@TaskStatus tinyint
//, @IsShare bit
//,@AssignBy int
//,@ColorFlag varchar(10)
//,@RemindDate date
//,@ClientId int
//,@Lang varchar(10)
//,@AssignTo varchar(500)
//*/

//        [EntityProperty(EntityPropertyType.Key)]
//        public int TaskId { get; set; }
//        public string TaskSubject { get; set; }
//        public string TaskBody { get; set; }
//        public int Task_Parent { get; set; }
//        public int Project_Id { get; set; }
//        public int UserId { get; set; }
//        [EntityProperty(EntityPropertyType.View)]
//        public DateTime CreatedDate { get; set; }
//        public DateTime? DueDate { get; set; }
//        public int AccountId { get; set; }
//        public int TaskStatus { get; set; }
//        //public bool IsShare { get; set; }
//        public int ShareType { get; set; }
//        public int AssignBy { get; set; }
//        public string ColorFlag { get; set; }
//        [EntityProperty(EntityPropertyType.Optional)]
//        public DateTime LastUpdate { get; set; }
//        public DateTime RemindDate { get; set; }
//        public int ClientId { get; set; }
//        public string Lang { get; set; }
//        public string Tags { get; set; }
//        public string AssignTo { get; set; }
//        public string ToHtml()
//        {
//            return EntityProperties.ToHtmlTable<ReminderItem>(this, null, null, true);
//        }
//    }

    [EntityMapping("Task_Assignments", "vw_Task_Assignments", "היסטוריה", ProcInsert = "sp_Task_Assign")]
    public class TaskAssignment : IEntityItem
    {

        [EntityProperty(EntityPropertyType.Identity)]
        public int AssignId { get; set; }
        [EntityProperty(Order=1)]
        public int Task_Id { get; set; }
        [EntityProperty(EntityPropertyType.Optional,Order=2)]
        public int AccountId { get; set; }
          [EntityProperty(Order=3)]
        public int AssignedBy { get; set; }
          [EntityProperty(Order=4)]
        public int AssignedTo { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime AssignDate { get; set; }
         [EntityProperty(Order=5)]
        public string AssignSubject { get; set; }

        [EntityProperty(EntityPropertyType.Optional)]
        public string AssignedByName { get; set; }
        [EntityProperty(EntityPropertyType.Optional)]
        public string AssignedToName { get; set; }

    }
         
    [EntityMapping("Task_Comments", "vw_Task_Comments","מעקב משימות",ProcUpsert="sp_Task_Comment_Upsert")]// ,"he:מעקב משימות;en:Tasks follow up")]
    public class TaskComment : IEntityItem
    {
         [EntityProperty(EntityPropertyType.Identity,Order=1)]
        public int CommentId { get; set; }
        [EntityProperty(Order=2)]
        public string CommentText { get; set; }
        [EntityProperty(Order=3)]
        public int UserId { get; set; }
         [EntityProperty(Order=4)]
        public int Task_Id { get; set; }
        //public string Attachment { get; set; }
         [EntityProperty(Order=5)]
        public DateTime ReminderDate { get; set; }

        [EntityProperty(EntityPropertyType.View)]
        public DateTime CommentDate { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string DisplayName { get; set; }

         [EntityProperty(EntityPropertyType.Optional,Order=6)]
        public int AccountId { get; set; }
    }

    [EntityMapping("Task_Timers", "vw_Task_Timers","מעקב זמנים")]
    public class TaskTimer : IEntityItem
    {
        public const string ProcName = "sp_Task_Timer";

        //public static IList<TaskTimer> GetList(int taskId, int userId, int ttl)
        //{
        //    string key = DbContextCache.GetKey<TaskTimer>(Settings.ProjectName, EntityCacheGroups.Task, 0, userId);
        //    return DbContextCache.EntityList<DbSystem, TaskTimer>(key, ttl, new object[]{"Task_Id", taskId});
        //}
        //public static TaskTimer Get(int TaskId,int TaskTimerId)
        //{
        //    if (TaskTimerId <= 0 && TaskId > 0)
        //        return new TaskTimer() { Task_Id = TaskId };
        //    return DbContext.EntityGet<DbSystem, TaskTimer>("TaskTimerId", TaskTimerId);
        //}
        //public static int Save(int TaskId, int TaskTimerId, TaskTimer entity, UpdateCommandType commandType)
        //{
        //    entity.Task_Id = TaskId;
        //    entity.TaskTimerId = TaskTimerId;
        //    return DbContext.EntitySave<DbSystem, TaskTimer>(entity);//, commandType, new object[] { "Task_Id", TaskId, "TaskTimerId", TaskTimerId });
        //}
        //public static int Delete(int TaskTimerId)
        //{
        //    return DbContext.EntityDelete<DbSystem, TaskTimer>("TaskTimerId", TaskTimerId);
        //}

        //internal const string MappingName = "Task_Timers";

        [EntityProperty(EntityPropertyType.Key)]
        public int Task_Id { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int SubIndex { get; set; }
        public int UserId { get; set; }
        public int Duration { get; set; }
        public string Subject { get; set; }

        [EntityProperty(EntityPropertyType.View)]
        public DateTime StartTime { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime? EndTime { get; set; }

        [EntityProperty(EntityPropertyType.Optional)]
        public string DisplayName { get; set; }
        [EntityProperty(EntityPropertyType.Identity)]
        public int TaskTimerId { get; set; }
        public decimal Cost { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string DurationView { get; set; }
        
    }


     [EntityMapping("Task_Form", "vw_Task_Form","פעולות")]
    public class TaskForm : IEntityItem
    {
        public int Task_Id { get; set; }

        [EntityProperty(EntityPropertyType.Identity)]
        public int ItemId { get; set; }
        public string ItemText { get; set; }
        
        [EntityProperty(EntityPropertyType.View)]
        public DateTime ItemDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string DoneComment { get; set; }
        public bool DoneStatus { get; set; }
        public int UserId { get; set; }
        [EntityProperty(EntityPropertyType.Optional)]
        public string DisplayName { get; set; }
        public int AssignBy { get; set; }
        public int Duration { get; set; }
        public DateTime? ItemDueDate { get; set; }
        public int ItemAssignTo { get; set; }
        public int ItemTask { get; set; }
        public string ItemSubject { get; set; }
    }

    //[EntityMapping("Task_Files", "vw_Task_Files")]
    //public class TaskFile : IEntityItem
    //{
    //    public static IList<TaskFile> GetList(int taskId, int userId, int ttl)
    //    {
    //        string key = DbContextCache.GetKey<TaskFile>(Settings.ProjectName, EntityCacheGroups.Task, 0, userId);
    //        return DbContextCache.EntityList<DbSystem, TaskFile>(key, ttl, "Task_Id", taskId);
    //    }
    //    public static TaskFile Get(int TaskId, int FileId)
    //    {
    //        if (FileId <= 0 && TaskId > 0)
    //            return new TaskFile() { Pid = TaskId };

    //        return DbContext.EntityGet<DbSystem, TaskFile>("FileId", FileId);
    //    }
    //    public static int Save(int TaskId, int FileId, TaskFile entity, UpdateCommandType commandType)
    //    {
    //        return DbContext.EntitySave<DbSystem, TaskFile>(entity, commandType, new object[] { "Pid", TaskId, "FileId", FileId });
    //    }
    //    public static int Delete(int FileId)
    //    {
    //        return DbContext.EntityDelete<DbSystem, TaskFile>("FileId", FileId);
    //    }

    //    internal const string MappingName = "Task_Files";


    //    [EntityProperty(EntityPropertyType.Identity)]
    //    public int FileId { get; set; }
    //    public string FileSubject { get; set; }
    //    public int Pid { get; set; }
    //    public string Ptype { get; set; }
    //    public string FileType { get; set; }
    //    public string FileName { get; set; }
    //    public string FilePath { get; set; }
    //    public string ReferralType { get; set; }
    //    public string ReferralKey { get; set; }

    //    [EntityProperty(EntityPropertyType.View)]
    //    public DateTime Creation { get; set; }

    //    public int UserId { get; set; }
    //    [EntityProperty(EntityPropertyType.View)]
    //    public string UserName { get; set; }
    //}


    //public class MediaSystem : IEntityItem
    //{
    //    public string FilePrefix
    //    {
    //        get { return ReferralType + "_" + ReferralKey + "_"; }
    //    }
    //    public string GetFileName(string filename, string fileExt)
    //    {
    //        return ReferralType + "_" + ReferralKey + "_" + filename + fileExt;
    //    }
    //    public string RootFolder { get; set; }
    //    public string AccountFolder { get; set; }
    //    public string ReferralType { get; set; }
    //    public string ReferralKey { get; set; }
    //    public int UserId { get; set; }
    //    [EntityProperty(EntityPropertyType.View)]
    //    public string UserName { get; set; }
    //}

    [EntityMapping("Task_Action", "vw_Task_Action", "פעולות לביצוע")]
     public class TaskAction : IEntityItem
     {
         public int Task_Id { get; set; }

         [EntityProperty(EntityPropertyType.Identity)]
         public int ActionId { get; set; }
         public int UserId { get; set; }
         public int OrderIndex { get; set; }
         public string Subject { get; set; }
         public string ActionText { get; set; }
         public string Comment { get; set; }

         [EntityProperty(EntityPropertyType.View)]
         public DateTime Creation { get; set; }
         [EntityProperty(EntityPropertyType.View)]
         public DateTime StartTime { get; set; }
         [EntityProperty(EntityPropertyType.View)]
         public DateTime EndTime { get; set; }
         public int Duration { get; set; }
         public DateTime DueDate { get; set; }
         public int AssignBy { get; set; }
         public int AssignTo { get; set; }
         public int Status { get; set; }

         [EntityProperty(EntityPropertyType.Optional)]
         public string AssignByName { get; set; }

         [EntityProperty(EntityPropertyType.Optional)]
         public string AssignToName { get; set; }
         [EntityProperty(EntityPropertyType.Optional)]
         public string StatusName { get; set; }

         [EntityProperty(EntityPropertyType.Optional)]
         public string DisplayName { get; set; }
     }

     [EntityMapping("Task_Form_Type", "Task_Form_Type", "סוגי טפסים")]
     public class TaskFormType : IEntityItem
     {
         [EntityProperty(EntityPropertyType.Identity)]
         public int FormTypeId { get; set; }
         public string FormName { get; set; }
         public int AccountId { get; set; }
     }
     [EntityMapping("Task_Form_Fields", "Task_Form_Fields", "עריכת טפסים")]
     public class TaskFormTemplate : IEntityItem
     {
         [EntityProperty(EntityPropertyType.Key)]
         public string Label { get; set; }
         [EntityProperty(EntityPropertyType.Key)]
         public int FormId { get; set; }
         public string Description { get; set; }
         public int OrderBy { get; set; }
         public bool Required { get; set; }
     }

    

    [Flags]
    public enum TaskStatus
    {
        Open = 1,
        Started = 2,
        Paused = 4,
        None = 8,
        Close = 16
    }

}
