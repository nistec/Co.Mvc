using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using Nistec.Channels.RemoteCache;
using Nistec.Web.Controls;
using ProSystem.Data;
using ProSystem.Data.Entities;

namespace ProSystem.Data.Enums
{
     [EntityMapping("Enums_Status")]
    public class EnumStatus : IEntityItem
    {
        public const string TableName = "Enums_Status";

       
        public static IList<EnumStatus> ViewList(string StatusModel)
        {
            string key = WebCache.GetKey(Settings.ProjectName, EntityCacheGroups.Enums, 0,0, TableName, StatusModel);
            return WebCache.GetOrCreateList<EnumStatus>(key, () => ViewDbList(StatusModel), EntityProCache.DefaultCacheTtl);
        }

        public static IList<EnumStatus> ViewDbList(string StatusModel="T")
        {
            using (var db = DbContext.Create<DbSystem>())
                return db.Query<EnumStatus>("select * from Enums_Status where (StatusModel=@StatusModel or StatusModel='A')", "StatusModel", StatusModel);

        }
 
        #region properties


        [EntityProperty(EntityPropertyType.Key, Column = "StatusId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "StatusNameLocal")]
        public string PropName { get; set; }

      //  [StatusId]
      //,[StatusName]
      //,[StatusNameLocal]
      //,[StatusCode]
      //,[TaskState]
      //,[TaskHex]

        [EntityProperty(Column = "StatusModel")]
        [Validator("מודל", true)]
        public string StatusModel { get; set; }

        #endregion

    }
}
