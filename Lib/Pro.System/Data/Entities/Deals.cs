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
    public class DealsContext : EntityModelContext<DealItem>
    {
        const string EntityCacheGroup = EntityCacheGroups.Deals;

        public static void Refresh(int AccountId)
        {
            DbContextCache.Remove<DealItem>(Settings.ProjectName, EntityCacheGroup, AccountId, 0);
        }
        public static DealsContext Get(int AccountId)
        {
            return new DealsContext(AccountId);
        }
        public DealsContext(int AccountId) : base(AccountId, 0, EntityCacheGroup)
        {
        }
    }

    [EntityMapping("Deals", "vw_Deals", "פריטים")]
    public class DealItem : IEntityItem
    {

        [EntityProperty(EntityPropertyType.Identity)]
        public int DealId { get; set; }
        public string DealName { get; set; }
        public int DealType { get; set; }
        public int PackId { get; set; }
        public int ProjectId { get; set; }
        public int AccountId { get; set; }
        public string Invoice { get; set; }
        public Decimal Total { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Period { get; set; }
        public bool AutoRenew { get; set; }
        public bool IsExpired { get; set; }
        public string DealNo { get; set; }
        public int UserId { get; set; }

        [EntityProperty(EntityPropertyType.View)]
        public DateTime CreatedDate { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime Modified { get; set; }


        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<DealItem>(this, null, null, true);
        }
    }

    [EntityMapping("Deals_Pack", "Deals_Pack", "חבילה")]
    public class DealPack : IEntityItem
    {

        [EntityProperty(EntityPropertyType.Identity)]
        public int PackId { get; set; }
        public string PackName { get; set; }
        public Decimal Price { get; set; }
        public int Period { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime CreatedDate { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime Modified { get; set; }
    }

    [EntityMapping("Deals_Pack_Items", "Deals_Pack_Items", "חבילה")]
    public class DealPackItem : IEntityItem
    {

        [EntityProperty(EntityPropertyType.Identity)]
        public int ItemId { get; set; }
        public int PackId { get; set; }
        public string ItemName { get; set; }
        public int ProductId { get; set; }
        public Decimal Price { get; set; }
    }


}