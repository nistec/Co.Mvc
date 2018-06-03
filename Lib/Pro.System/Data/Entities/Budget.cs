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

    public class BudgetContext : EntityModelContext<BudgetItem>
    {
        const string EntityCacheGroup = EntityCacheGroups.Project;

        public static void Refresh(int AccountId)
        {
            DbContextCache.Remove<BudgetItem>(Settings.ProjectName, EntityCacheGroup, AccountId, 0);
        }
        public static BudgetContext Get(int AccountId)
        {
            return new BudgetContext(AccountId);
        }
        public BudgetContext(int AccountId) : base(AccountId, 0, EntityCacheGroup)
        {
        }
    }

    [EntityMapping("Budget", "vw_Budget", "תקציב")]
    public class BudgetItem : IEntityItem
    {
 
        [EntityProperty(EntityPropertyType.Identity)]
        public int BudgetId { get; set; }
        public string BudgetName { get; set; }
        public string Peroid { get; set; }
        public int Year { get; set; }
        public int Q { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime CreatedDate { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime Modified { get; set; }


        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<BudgetItem>(this, null, null, true);
        }
    }
    
    [EntityMapping("Budget_Raw", "Budget_Raw", "תקציב פריטים")]
    public class BudgetRaw : IEntityItem
    {

        [EntityProperty(EntityPropertyType.Identity)]
        public int RawId { get; set; }
        public int BudgetId { get; set; }
        public string BudgetClause { get; set; }
        public int BudgetType { get; set; }
        public int ProjectId { get; set; }
        public decimal Amount_In { get; set; }
        public decimal Amount_Out { get; set; }

        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<BudgetRaw>(this, null, null, true);
        }
    }


}
