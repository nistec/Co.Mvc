using Nistec.Data.Entities;
using ProNetcell.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProNetcell.Data
{
    public class AppSettingsContext: DbNetcellContext<AppSettings>
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
            : base(EntityGroups.Settings, accountId)
        {
        }
        public IList<AppSettings> GetBySection(string section)
        {
            return base.ExecOrViewList("Section", section);
        }
        public IList<AppSettings> GetByAccount(int accountId)
        {
            return base.ExecOrViewList("AccountId", accountId);
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
