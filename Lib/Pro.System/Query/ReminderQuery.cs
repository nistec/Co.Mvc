using Nistec;
using Nistec.Data;
using Nistec.Serialization;
using Pro.Query;
using ProSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace ProSystem.Query
{
    [Serializable]
   
    public class ReminderQuery:QueryBase
    {

        public string Serialize()
        {
           var ser= BinarySerializer.SerializeToBase64(this);
           return ser;
        }

        public static ReminderQuery Deserialize(string ser)
        {
            return BinarySerializer.DeserializeFromBase64<ReminderQuery>(ser);
        }

       
        public ReminderQuery()
        {
           
        }

        public ReminderQuery(HttpRequestBase Request, bool enableFilter=false)
        {
            Mode = (ReminderMode) Types.ToInt(Request["Mode"]);
            AccountId = Types.ToInt(Request["AccountId"]);
            UserId = Types.ToInt(Request["UserId"]);
            RemindStatus = Types.ToInt(Request["RemindStatus"]);
            AssignBy = Types.ToInt(Request["AssignBy"]);

            if (enableFilter)
                LoadSortAndFilter(Request,"");
        }


        public void Normelize()
        {
            
            //DateFrom = SqlFormatter.ValidateSqlInput(DateFrom);
            //Filter = SqlFormatter.ValidateSqlInput(Filter);

        }

        public ReminderMode Mode { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public int AssignBy { get; set; }
        public int RemindStatus { get; set; }
    }
}
