using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Web.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSystem.Data.Entities
{
    public class DocsContext<T> : EntityModelContext<T> where T: IEntityItem
    {
        public const string EntityCacheGroup = EntityCacheGroups.Docs;

        public static void Refresh(int AccountId)
        {
            DbContextCache.Remove<DocItem>(Settings.ProjectName, EntityCacheGroup, AccountId, 0);
        }
        public static DocsContext<T> Get(int AccountId)
        {
            return new DocsContext<T>(AccountId);
        }
        public DocsContext(int AccountId) : base(AccountId, 0, EntityCacheGroup)
        {
        }

        //public static DocItemInfo GetInfo(int DocId)
        //{
        //    if (DocId == 0)
        //        return new DocItemInfo();
        //    using (var db = DbContext.Create<DbSystem>())
        //    {
        //        return db.EntityProcGet<DocItemInfo>("DocId", DocId);
        //    }
        //}

        public static IEnumerable<DocListView> ViewDocs(int AccountId, int UserId, int AssignBy, int DocStatus)
        {
            int PageSize = 0;
            int PageNum = 0;
            int ttl = Settings.DefaultShortTTL;
            var parameters = new object[] { "PageSize", PageSize, "PageNum", PageNum, "AccountId", AccountId, "UserId", UserId, "AssignBy", AssignBy, "DocStatus", DocStatus };
            string key = DbContextCache.GetKey<DocListView>(Settings.ProjectName, EntityCacheGroups.Docs, AccountId, 0);
            return DbContextCache.ExecuteList<DbSystem, DocListView>(key, ttl, parameters);

            //using (var db = DbContext.Create<DbSystem>())
            //    return db.ExecuteList<DocListView>("sp_Task_Docs_Report", "PageSize", PageSize, "PageNum", PageNum, "AccountId", AccountId, "UserId", UserId, "AssignBy", AssignBy, "DocStatus", DocStatus);
        }
        public int ArchiveDocs(int DocId, int AccountId)
        {
            string command = "update ["+MappingName+"] set IsExpired=1 where DocId=@DocId and AccountId=@AccountId";
            var parameters = new object[] { "DocId", DocId, "AccountId", AccountId};
            return  DoCommandNoneQuery(command, ProcedureType.Update, parameters);
        }

        public static int DocFormStart(int ItemId, int UserId)
        {
            int res = 0;
            using (var Db = DbContext.Get<DbSystem>())
            {
                res = Db.ExecuteCommandUpdate("Doc_Form", "StartDate=@StartDate,UserId=@UserId", "ItemId=@ItemId", "StartDate", DateTime.Now, "UserId", UserId, "ItemId", ItemId);
            }
            return res;
        }

        public int AddOrUpdate(DocItem item)
        {
            object[] values = new object[]
           {
            "DocId", item.DocId
            ,"DocSubject", item.DocSubject
            ,"DocBody", item.DocBody
            ,"Doc_Type", item.Doc_Type
            ,"Doc_Parent", item.Doc_Parent
            ,"Project_Id", item.Project_Id
            ,"UserId", item.UserId
            //,"CreatedDate", item.CreatedDate
            ,"DueDate", item.DueDate
            ,"StartedDate", item.StartedDate
            ,"EndedDate", item.EndedDate
            ,"EstimateStartTime", item.EstimateStartTime
            ,"EstimateTakenTime", item.EstimateTakenTime
            ,"AccountId", item.AccountId
            ,"DocStatus", item.DocStatus
            //,"IsShare",item.IsShare
            ,"Priority", item.Priority
            ,"Budget",item.Budget
            ,"TotalTime", item.TotalTime
            ,"TimePart", item.TimePart
            ,"AssignBy", item.AssignBy
            ,"ColorFlag", item.ColorFlag
            //,"TeamId", item.TeamId
            //,"LastUpdate", item.LastUpdate
            ,"LastAct", item.LastAct
            ,"RemindDate", item.DueDate
            ,"DocModel", item.DocModel
            ,"ClientId", item.ClientId
            //,"ClientDetails", item.ClientDetails
            //,"AssignByAccount", item.AssignByAccount
            ,"Tags", item.Tags
            ,"Lang", item.Lang
            ,"AssignTo", item.AssignTo
            ,"PermsType", item.PermsType
            ,"Folder", item.Folder
            };
            return base.ExecuteReturnValue( ProcedureType.Upsert,"sp_Docs_AddOrUpdate",0,values);//.UpsertReturnValue(values);
        }

        // public static int AddOrUpdate2(DocItem item)
        //{
        //    object[] values = new object[]
        //    {
        //    "DocId", item.DocId
        //    ,"DocSubject", item.DocSubject
        //    ,"DocBody", item.DocBody
        //    ,"Doc_Type", item.Doc_Type
        //    ,"Doc_Parent", item.Doc_Parent
        //    ,"Project_Id", item.Project_Id
        //    ,"UserId", item.UserId
        //    //,"CreatedDate", item.CreatedDate
        //    ,"DueDate", item.DueDate
        //    ,"StartedDate", item.StartedDate
        //    ,"EndedDate", item.EndedDate
        //    ,"EstimateStartTime", item.EstimateStartTime
        //    ,"EstimateTakenTime", item.EstimateTakenTime
        //    ,"AccountId", item.AccountId
        //    ,"DocStatus", item.DocStatus
        //    //,"IsShare",item.IsShare
        //    ,"Priority", item.Priority
        //    ,"Budget",item.Budget
        //    ,"TotalTime", item.TotalTime
        //    ,"TimePart", item.TimePart
        //    ,"AssignBy", item.AssignBy
        //    ,"ColorFlag", item.ColorFlag
        //    ,"TeamId", item.TeamId
        //    //,"LastUpdate", item.LastUpdate
        //    ,"LastAct", item.LastAct
        //    ,"RemindDate", item.DueDate
        //    ,"DocModel", item.DocModel
        //    ,"ClientId", item.ClientId
        //    ,"ClientDetails", item.ClientDetails
        //    ,"AssignByAccount", item.AssignByAccount
        //    ,"Tags", item.Tags
        //    ,"Lang", item.Lang
        //    ,"AssignTo", item.AssignTo
        //    ,"ShareType", item.ShareType
        //    ,"Folder", item.Folder
        //                };

        //    using (var db = DbContext.Create<DbSystem>())
        //        return db.ExecuteReturnValue("sp_Docs_AddOrUpdate", 0, values);
        //}
    }

    [EntityMapping("vw_Docs",ProcListView = "sp_Docs_Report")]
    public class DocListView : DocItem, IEntityListItem
    {
        //public static IList<DocListView> GetList(int UserId, int userId, int ttl)
        //{
        //    string key = DbContextCache.GetKey<DocListView>(Settings.ProjectName, EntityCacheGroups.Docs, 0, userId);
        //    return DbContextCache.EntityList<DbSystem, DocListView>(key, ttl, new object[] { "UserId", UserId });
        //}

        public string ProjectName { get; set; }
        public string StatusName { get; set; }
        //public string DisplayName { get; set; }
        public string DocTypeName { get; set; }
        //public string AssignByName { get; set; }
        public string TotalTimeView { get; set; }
        public int TotalRows { get; set; }
    }

    [EntityMapping(ProcGet = "sp_Docs_Get_Info")]
    public class DocItemInfo : DocItem
    {
        public int Comments { get; set; }
        //public int Assigns { get; set; }
        //public int Timers { get; set; }
        public int Items { get; set; }
        public int Files { get; set; }

    }

    [EntityMapping("Docs", "vw_Docs", "תיעוד", ProcUpsert = "sp_Docs_AddOrUpdate")]
    public class DocItem : IEntityItem
    {

        [EntityProperty(EntityPropertyType.Key)]
        public int DocId { get; set; }
        public string DocSubject { get; set; }
        public string DocBody { get; set; }
        public int Doc_Type { get; set; }
        public int Doc_Parent { get; set; }
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
        public int DocStatus { get; set; }
        //public bool IsShare { get; set; }
        public int PermsType { get; set; }
        public int Priority { get; set; }
        public decimal Budget { get; set; }
        public int TotalTime { get; set; }
        public string TimePart { get; set; }
        public int AssignBy { get; set; }
        public string ColorFlag { get; set; }
       // public int TeamId { get; set; }
        public string LastAct { get; set; }
        [EntityProperty(EntityPropertyType.Optional)]
        public DateTime LastUpdate { get; set; }
               
        public string DocModel { get; set; }
        public int ClientId { get; set; }
        //public int AssignByAccount { get; set; }
        //public string ClientDetails { get; set; }
        public string Tags { get; set; }
        public string Lang { get; set; }
        public string AssignTo { get; set; }
        public string Folder { get; set; }


        [EntityProperty(EntityPropertyType.Optional)]
        public string StatusNameLocal { get; set; }
        [EntityProperty(EntityPropertyType.Optional)]
        public string DisplayName { get; set; }
        [EntityProperty(EntityPropertyType.Optional)]
        public string AssignByName { get; set; }
        [EntityProperty(EntityPropertyType.Optional)]
        public bool RemindToday { get; set; }


        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<DocItem>(this, null, null, true);
        }
    }

    [EntityMapping("Docs_Comments", "vw_Docs_Comments", "הערות מסמכים", ProcUpsert = "sp_Docs_Comment_Upsert")]
    public class DocComment : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Identity, Order = 1)]
        public int CommentId { get; set; }
        [EntityProperty(Order = 2)]
        public string CommentText { get; set; }
        [EntityProperty(Order = 3)]
        public int UserId { get; set; }
        [EntityProperty(Order = 4)]
        public int Doc_Id { get; set; }
        //public string Attachment { get; set; }
        [EntityProperty(Order = 5)]
        public DateTime ReminderDate { get; set; }
        [EntityProperty(Order = 6)]
        public bool IsSchedule { get; set; }

        [EntityProperty(EntityPropertyType.View)]
        public DateTime CommentDate { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string DisplayName { get; set; }

        [EntityProperty(EntityPropertyType.Optional, Order = 7)]
        public int AccountId { get; set; }
    }

    [EntityMapping("Docs_Form", "vw_Docs_Form", "סיכומים")]
    public class DocForm : IEntityItem
    {

  
        public int Doc_Id { get; set; }

        [EntityProperty(EntityPropertyType.Identity)]
        public int ItemId { get; set; }
        public string ItemLabel { get; set; }
        public string ItemValue { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime ItemDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public string DoneComment { get; set; }
        public bool DoneStatus { get; set; }
        public int UserId { get; set; }
        [EntityProperty(EntityPropertyType.Optional)]
        public string DisplayName { get; set; }
    }
}
