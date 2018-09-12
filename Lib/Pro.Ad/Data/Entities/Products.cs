using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Web.Controls;
using ProSystem.Data;
using ProSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProAd.Data.Entities
{

    public class ProductsContext : EntityModelContext<ProductItem>
    {
        const string EntityCacheGroup = EntityCacheGroups.Products;

        public static void Refresh(int AccountId)
        {
            DbContextCache.Remove<ProductItem>(Settings.ProjectName, EntityCacheGroup, AccountId, 0);
        }
        public static ProductsContext Get(int AccountId)
        {
            return new ProductsContext(AccountId);
        }
        public ProductsContext(int AccountId) : base(AccountId, 0, EntityCacheGroup)
        {
        }
    }

    [EntityMapping("Products", "vw_Products", "פריטים")]
    public class ProductItem : IEntityItem
    {

        [EntityProperty(EntityPropertyType.Identity)]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductCategory { get; set; }
        public Decimal DefaultPrice { get; set; }
        public string Tags { get; set; }
        public Decimal CostPrice { get; set; }
        public Decimal MinPrice { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime CreatedDate { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public DateTime Modified { get; set; }


        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<ProductItem>(this, null, null, true);
        }
    }
}
