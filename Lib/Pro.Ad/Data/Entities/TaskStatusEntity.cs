using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using Nistec.Channels.RemoteCache;
using Nistec.Web.Controls;
using ProSystem.Data;

namespace ProSystem.Data.Entities
{
    [EntityMapping("Task_Status")]
    public class TaskStatusEntity : IEntityItem
    {
        public const string TableName = "Task_Status";

        #region properties


        [EntityProperty(EntityPropertyType.Key, Column = "StatusId")]
        public int PropId { get; set; }

        [EntityProperty(Column = "StatusNameLocal")]
        public string PropName { get; set; }


        #endregion

    }


    //public static class TaskStatusMessage
    //{
    //    public static class TaskCompleted
    //    {
    //        public static string Get(int res)
    //        {
    //            switch (res)
    //            {
    //                case -1:
    //                    return "נשארו משימות פתוחות במעקב זמנים, לא ניתן לסגור";
    //                case -2:
    //                    return "נשארו משימות פתוחות במעקב ביצוע, לא ניתן לסגור";
    //                default:
    //                    return "";
    //            }
    //        }
    //    }

    //}

    
}
