using Nistec;
using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Web.Controls;
using ProSystem.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSystem.Data.Entities
{
    //today only = 1, from now forword = 2, from now back = 3, all = 0
    public enum ReminderMode
    {
        All=0,
        Today=1,
        Forword=2,
        Backword=3
    }

    public class ReminderContext : EntityModelContext<ReminderItem>//EntityContext<DbSystem, ReminderItem>
    {
        const string EntityCacheGroup = EntityCacheGroups.Reminder;
       
        public static void Refresh(int accountId)
        {
            DbContextCache.Remove<ReminderItem>(Settings.ProjectName, EntityCacheGroup, accountId, 0);
        }
        public static ReminderContext Get(int accountId)
        {
            return new ReminderContext(accountId);
        }

        public ReminderContext(int accountId) : base(accountId, 0, EntityCacheGroup)
        {
            //if (userId > 0)
            //    CacheKey = DbContextCache.GetKey<ReminderItem>(Settings.ProjectName, EntityCacheGroups.Reminder, 0, userId);
        }

        //public IList<ReminderItem> GetList()
        //{
        //    //int ttl = 3;
        //    return DbContextCache.EntityList<DbSystem, ReminderItem>(CacheKey, null);
        //}
        //public IList<ReminderItem> GetList(int id)
        //{
        //    //int ttl = 3;
        //    return DbContextCache.EntityList<DbSystem, ReminderItem>(CacheKey, new object[] { "RemindId", id });
        //}
        //protected override void OnChanged(ProcedureType commandType)
        //{
        //    DbContextCache.Remove(CacheKey);
        //}
        //public FormResult GetFormResult(EntityCommandResult res, string reason)
        //{
        //    return FormResult.Get(res, EntityName, reason);//.GetFormResult(res.AffectedRecords, this.EntityName, reason, res.GetIdentityValue<int>());
        //}

        #region reminder

        //public static DataTable GetReminders()
        //{
        //    using (var db = DbContext.Create<DbSystem>())
        //    {
        //        var dt = db.QueryDataTable(MappingName, "TaskModel", "R");
        //        return dt;

        //    }
        //}

        //public static IEnumerable<ReminderListView> View(object[] parameters)
        //{
        //    int ttl = Settings.DefaultShortTTL;
        //    string key = DbContextCache.GetKey<ReminderListView>(Settings.ProjectName, EntityCacheGroups.Reminder, AccountId, 0);
        //    return DbContextCache.ExecuteList<DbSystem, ReminderListView>(key, ttl, parameters);

        //    //using (var db = DbContext.Create<DbSystem>())
        //    //    return db.ExecuteList<DocListView>("sp_Task_Docs_Report", "PageSize", PageSize, "PageNum", PageNum, "AccountId", AccountId, "UserId", UserId, "AssignBy", AssignBy, "DocStatus", DocStatus);
        //}

        public static IEnumerable<ReminderListView> View(ReminderMode Mode, int AccountId, int UserId, int AssignBy, int RemindStatus)
        {
            int PageSize = 0;
            int PageNum = 0;

            int ttl = Settings.DefaultShortTTL;
            var parameters = new object[] { "PageSize", PageSize, "PageNum", PageNum, "Mode", (int)Mode, "AccountId", AccountId, "UserId", UserId, "AssignBy", AssignBy, "RemindStatus", RemindStatus };
            string key = DbContextCache.GetKey<ReminderListView>(Settings.ProjectName, EntityCacheGroups.Reminder, AccountId, 0);
            return DbContextCache.ExecuteList<DbSystem, ReminderListView>(key, ttl, parameters);

            //using (var db = DbContext.Create<DbSystem>())
            //    return db.ExecuteList<DocListView>("sp_Task_Docs_Report", "PageSize", PageSize, "PageNum", PageNum, "AccountId", AccountId, "UserId", UserId, "AssignBy", AssignBy, "DocStatus", DocStatus);
        }

        //public static IEnumerable<ReminderListView> ViewDocs(int Mode,int AccountId, int UserId, int AssignBy, int RemindStatus)
        //{
        //    int PageSize = 0;
        //    int PageNum = 0;

        //    using (var db = DbContext.Create<DbSystem>())
        //        return db.ExecuteList<ReminderListView>("sp_Reminder_Report", "PageSize", PageSize, "PageNum", PageNum, "Mode", Mode, "AccountId", AccountId, "UserId", UserId, "AssignBy", AssignBy, "RemindStatus", RemindStatus);
        //}

        public int Archive(int RemindId, int AccountId)
        {
            string command = "update [" + MappingName + "] set IsExpired=1 where RemindId=@RemindId and AccountId=@AccountId";
            var parameters = new object[] { "RemindId", RemindId, "AccountId", AccountId };
            return DoCommandNoneQuery(command, ProcedureType.Update, parameters);
        }

        public int AddOrUpdate(ReminderItem item)
        {

            object[] values = new object[]
                {
            "RemindId",item.RemindId
            ,"Remind_Type", item.Remind_Type
            ,"DueDate", item.DueDate
            ,"RemindStatus", item.RemindStatus
            ,"AccountId", item.AccountId
            ,"UserId", item.UserId
            ,"AssignBy", item.AssignBy
            ,"RemindBody", item.RemindBody
            ,"Remind_Parent", item.Remind_Parent
            ,"ParentType", item.ParentType
            //,"CreatedDate", item.CreatedDate
            ,"RemindBefor", item.RemindBefor
            ,"LastReminded", item.LastReminded
            ,"AssignTo", item.AssignTo
            ,"ShareType", item.ShareType
            //,"ShareId", item.ShareId
            ,"IsExpired", item.IsExpired
            ,"ColorFlag", item.ColorFlag
            ,"ClientId", item.ClientId
            ,"Project_Id", item.Project_Id
                };

            return base.ExecuteReturnValue(ProcedureType.Upsert, "sp_Reminder_AddOrUpdate", 0, values);//.UpsertReturnValue(values);

            //using (var db = DbContext.Create<DbSystem>())
            //    return db.ExecuteReturnValue("sp_Reminder_AddOrUpdate", 0, values);
        }

        #endregion
    }

//[Entity(EntityName = "ReminderContext", MappingName = "Reminder", ConnectionKey = "netcell_system", EntityKey = new string[] { "RemindId" })]
//public class ReminderContext : EntityContext<TaskItem>
//{
//    public const string MappingName = "Reminder";

//    #region ctor

//    public ReminderContext()
//    {
//    }

//    public ReminderContext(int RemindId, int AccountId)
//        : base(RemindId)
//    {
//        if (Entity.AccountId != AccountId)
//        {
//            throw new ArgumentException("Incorrecrt account");
//        }
//    }

//    #endregion

//    #region reminder

//    public static int Remainder_AddOrUpdate(ReminderItem item)
//    {


//        object[] values = new object[]
//            {
//        "RemindId",item.RemindId
//        ,"RemindType", item.RemindType
//        ,"DueDate", item.DueDate
//        ,"RemindState", item.RemindState
//        ,"AccountId", item.AccountId
//        ,"UserId", item.UserId
//        ,"AssignBy", item.AssignBy
//        ,"Message", item.Message
//        ,"SourceId", item.SourceId
//        ,"SourceType", item.SourceType
//        ,"CreatedDate", item.CreatedDate
//        ,"LastReminded", item.LastReminded
//        ,"AssignTo", item.AssignTo
//        ,"ShareType", item.ShareType
//        ,"ShareId", item.ShareId
//        ,"IsExpired", item.IsExpired
//            };


//        using (var db = DbContext.Create<DbSystem>())
//            return db.ExecuteReturnValue("sp_Reminder_AddOrUpdate", 0, values);
//    }

//    #endregion
//}

    [EntityMapping("Reminder", "תזכורת")]
    public class ReminderItem : IEntityItem
    {

        [EntityProperty(EntityPropertyType.Identity)]
        public int RemindId { get; set; }
        public int Remind_Type { get; set; }
        public DateTime? DueDate { get; set; }
        public int RemindStatus { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public int AssignBy { get; set; }
        public string RemindBody { get; set; }
        public int Remind_Parent { get; set; }
        public int ParentType { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime CreatedDate { get; set; }
        public DateTime? LastReminded { get; set; }
        public string AssignTo { get; set; }
        public int ShareType { get; set; }
        //public int ShareId { get; set; }
        public bool IsExpired { get; set; }
        public int RemindBefor { get; set; }

        public string ColorFlag { get; set; }
        //[EntityProperty(EntityPropertyType.Optional)]
        //public DateTime LastUpdate { get; set; }
        public int ClientId { get; set; }
        //public string Lang { get; set; }
        //public string Tags { get; set; }
        public int Project_Id { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public int Broadcast_Id { get; set; }


        
        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<ReminderItem>(this, null, null, true);
        }
    }

    [EntityMapping("vw_Reminder", ProcListView = "sp_Reminder_Report")]
    public class ReminderListView : ReminderItem, IEntityListItem
    {
        //public static IList<TaskListView> GetList(int UserId, int userId, int ttl)
        //{
        //    string key = DbContextCache.GetKey<TaskListView>(Settings.ProjectName, EntityCacheGroups.Enums, 0, userId);
        //    return DbContextCache.EntityList<DbSystem, TaskListView>(key, ttl, new object[] { "UserId", UserId });
        //}

        public string ProjectName { get; set; }
        public string StatusName { get; set; }
        //public string DisplayName { get; set; }
        public string TypeName { get; set; }
        public string AssignByName { get; set; }
        public string DisplayName { get; set; }
        public int TotalRows { get; set; }

    }
}
    