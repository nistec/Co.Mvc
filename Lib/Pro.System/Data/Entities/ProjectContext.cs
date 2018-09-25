using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Web.Controls;
using Pro;
using Pro.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSystem.Data.Entities
{

    public class ProjectContext : EntityModelContext<ProjectItem>
    {
        const string EntityCacheGroup = EntityCacheGroups.Project;

        public static void Refresh(int AccountId)
        {
            DbContextCache.Remove<DocItem>(Settings.ProjectName, EntityCacheGroup, AccountId, 0);
        }
        public static ProjectContext Get(int AccountId)
        {
            return new ProjectContext(AccountId);
        }
        public ProjectContext(int AccountId) : base(AccountId, 0, EntityCacheGroup)
        {
        }

        public int ArchiveDocs(int id, int accountId)
        {
            string sql = "update " + MappingName + " set IsExpired=1 where ProjectId=@ProjectId and AccountId=@AccountId";
            return DoCommandNoneQuery(sql, ProcedureType.Update, "ProjectId", id, "AccountId",accountId);
        }

    }

    //public class ProjectContext : EntityContext<DbSystem, ProjectItem>
    //{

    //    public static void Refresh(int AccountId)
    //    {
    //        DbContextCache.Remove<ProjectItem>(Settings.ProjectName, EntityCacheGroups.System, AccountId, 0);
    //    }

    //    public ProjectContext(int AccountId)
    //    {
    //        if (AccountId > 0)
    //            CacheKey = DbContextCache.GetKey<ProjectItem>(Settings.ProjectName, EntityCacheGroups.System, AccountId, 0);
    //    }
    //    public IList<ProjectItem> GetList(int AccountId, bool ActiveOnly)
    //    {
    //        var args = ActiveOnly ? new object[] { "AccountId", AccountId, "IsActive", true } : new object[] { "AccountId", AccountId };
    //        return DbContextCache.EntityList<DbSystem, ProjectItem>(CacheKey, args);
    //    }
    //    protected override void OnChanged(ProcedureType commandType)
    //    {
    //        DbContextCache.Remove(CacheKey);
    //    }
    //    public FormResult GetFormResult(EntityCommandResult res, string reason)
    //    {
    //        return FormResult.Get(res, this.EntityName, reason);
    //    }
    //}

    [EntityMapping("Project", "vw_Project", "פרוייקט")]
    public class ProjectItem : IEntityItem
    {

        [EntityProperty(EntityPropertyType.Identity)]
        public int ProjectId { get; set; }
        public int AccountId { get; set; }
        public string ProjectName { get; set; }
        public int ProjectManager { get; set; }
        public int ProjectCategory { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime CreatedDate { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime LastUpdate { get; set; }
        public DateTime DueDate { get; set; }

        public DateTime? StartedDate { get; set; }
        public DateTime? EndedDate { get; set; }
        public string ProjectDesc { get; set; }
        public int Status { get; set; }
        public int BudgetId { get; set; }
        public string Tags { get; set; }
        public int UserId { get; set; }
        public int ClientId { get; set; }

    }

    [EntityMapping("Project_Gantt", "Project_Gantt", "תכנון פרוייקט")]
    public class ProjectGantt : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Identity)]
        public int GanttId { get; set; }
        public int ProjectId { get; set; }
        public int GanttIndex { get; set; }
        public int AssignTo { get; set; }
        public string GanttSubject { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public int TaskId { get; set; }

    }

}
