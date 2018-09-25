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
    public class FormsContext : EntityModelContext<FormItem>
    {
        const string EntityCacheGroup = EntityCacheGroups.Forms;

        public static void Refresh(int AccountId)
        {
            DbContextCache.Remove<FormItem>(Settings.ProjectName, EntityCacheGroup, AccountId, 0);
        }
        public static FormsContext Get(int AccountId)
        {
            return new FormsContext(AccountId);
        }
        public FormsContext(int AccountId) : base(AccountId, 0, EntityCacheGroup)
        {
        }
    }

    [EntityMapping("Forms", "vw_Forms", "טפסים")]
    public class FormItem : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Key)]
        public int FormId { get; set; }
        public string FormName { get; set; }
        public int AccountId { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime CreatedDate { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime Modified { get; set; }
       
        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<FormItem>(this, null, null, true);
        }
    }

    [EntityMapping("Forms_Fields", "Forms_Fields", "עיצוב טופס")]
    public class FormFields : IEntityItem
    {
       
        [EntityProperty(EntityPropertyType.Key)]
        public int Form_Id { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public string Label { get; set; }

        public string DisplayName { get; set; }
        public string Title { get; set; }
        public int FieldOrder { get; set; }
        public string FieldType { get; set; }
        public bool Required { get; set; }
        public int MaxLength { get; set; }
        public string Validation { get; set; }
        public string Message_En { get; set; }
        public string Message_He { get; set; }

    }

}
