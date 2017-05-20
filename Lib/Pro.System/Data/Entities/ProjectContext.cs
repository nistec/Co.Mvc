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
    public class ProjectContext : EntityContext<DbSystem, ProjectItem>
    {

        public static void Refresh(int AccountId)
        {
            DbContextCache.Remove<ProjectItem>(Settings.ProjectName, EntityCacheGroups.System, AccountId, 0);
        }
        
        public ProjectContext(int AccountId)
        {
            if (AccountId > 0)
                CacheKey = DbContextCache.GetKey<ProjectItem>(Settings.ProjectName, EntityCacheGroups.System, AccountId, 0);
        }
        public IList<ProjectItem> GetList(int AccountId, bool ActiveOnly)
        {
            var args = ActiveOnly?  new object[] { "AccountId", AccountId,"IsActive",true } : new object[] { "AccountId", AccountId };
            return DbContextCache.EntityList<DbSystem, ProjectItem>(CacheKey, args);
        }
        protected override void OnChanged(UpdateCommandType commandType)
        {
            DbContextCache.Remove(CacheKey);
        }
        public FormResult GetFormResult(EntityCommandResult res, string reason)
        {
            return FormResult.Get(res, this.EntityName, reason);
        }
    }

    [EntityMapping("Project", "vw_Project", "פרוייקט")]
    public class ProjectItem:IEntityItem
    {

        public int ProjectId { get; set; }
        public int AccountId { get; set; }
        public string ProjectName { get; set; }
        public int Category { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public decimal Budget { get; set; }
        public string Note { get; set; }
        public int UserId { get; set; }
        public int MemberRecord { get; set; }

    }
}
