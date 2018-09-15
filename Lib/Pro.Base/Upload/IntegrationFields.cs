using Nistec.Data.Entities;
using Pro.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pro.Upload
{
    public class IntegrationFields : IEntityItem
    {
        public int SourceType { get; set; }
        public string FieldName { get; set; }
        public string SourceName { get; set; }
        public string Semantic { get; set; }
        public int FieldOrder { get; set; }
        public string DefaultValue { get; set; }
        public string SourceFieldMap { get; set; }
        public string SourceFormat { get; set; }
        public string SourceFilter { get; set; }
        public string SourceFilterType { get; set; }
        public string FieldType { get; set; }
        public bool IsRuleField { get; set; }


        public static Dictionary<string, IntegrationFields> LoadFieldsMap(SourceTypes SourceType)
        {
            using (var db = DbContext.Create<DbStg>())
            {
                var list = db.ExecuteList<IntegrationFields>("select * from [vw_Integration_Fields_Map] where SourceType=@SourceType", "SourceType", (int)SourceType);

                Dictionary<string, IntegrationFields> FieldsMap = new Dictionary<string, IntegrationFields>();
                foreach (var entry in list)
                {
                    FieldsMap[entry.SourceName] = entry;
                }

                return FieldsMap;
            }
        }

    }
}
