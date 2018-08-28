using Nistec;
using Nistec.Data;
using Nistec.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace ProSystem.Query
{
    [Serializable]
   
    public class TaskQuery:QueryBase
    {

        public string Serialize()
        {
           var ser= BinarySerializer.SerializeToBase64(this);
           return ser;
        }

        public static TaskQuery Deserialize(string ser)
        {
            return BinarySerializer.DeserializeFromBase64<TaskQuery>(ser);
        }

       
        public TaskQuery()
        {
           
        }

        public TaskQuery(HttpRequestBase Request, bool enableFilter)
        {

            TaskId = Types.ToInt(Request["TaskId"]);
            AccountId = Types.ToInt(Request["AccountId"]);
            UserId = Types.ToInt(Request["UserId"]);
            var assignMe = Types.ToBool(Request["assignMe"], false);
            AssignBy = (assignMe) ? UserId : 0;
            TaskStatus = Types.ToInt(Request["state"]);
            Id = Types.ToInt(Request["id"]);
            Mode = Request["op"];
            DateFrom = Types.ToNullableDateIso(Request["DateFrom"]);
            DateTo = Types.ToNullableDateIso(Request["DateTo"]);
            UserMode = Types.ToBool(Request["UserMode"], false);

            if (enableFilter)
                LoadSortAndFilter(Request);
        }


        public void Normelize()
        {
            
            //DateFrom = SqlFormatter.ValidateSqlInput(DateFrom);
            //Filter = SqlFormatter.ValidateSqlInput(Filter);

        }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public string Mode { get; set; }//g-a-u-d
        public int Id { get; set; }
        public int AssignBy { get; set; }
        public int TaskStatus { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool UserMode { get; set; }

    }
}
