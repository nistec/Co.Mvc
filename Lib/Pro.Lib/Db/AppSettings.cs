using Nistec.Data.Entities;
using Pro.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pro.Data
{
    public class AppSettingsContext: DbProContext<AppSettings>
    {
        public static V GetValue<V>(string key, V defaultValue)
        {
            return GetScalar<V>("ItemValue", "AppSettings", defaultValue, "ItemKey", key);
        }
        public static string GetValue(string key,string defaultValue=null)
        {
            return GetScalar<string>("ItemValue", "AppSettings", defaultValue, "ItemKey", key);
        }
        public static AppSettingsContext Get(int accountId)
        {
            return new AppSettingsContext(accountId);
        }
        public AppSettingsContext(int accountId)
            : base(EntityCacheGroups.Settings, accountId)
        {
        }
        public IList<AppSettings> GetBySection(string section)
        {
            return base.GetList("Section", section);
        }
        public IList<AppSettings> GetByAccount(int accountId)
        {
            return base.GetList("AccountId", accountId);
        }

    }

    [EntityMapping("AppSettings")]
    public class AppSettings : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Key)]
        public string ItemKey { get; set; }
        public string ItemValue { get; set; }
        public string Section { get; set; }
        public int AccountId { get; set; }

    }
}
