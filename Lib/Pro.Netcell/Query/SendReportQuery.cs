using Nistec;
using Nistec.Data;
using Nistec.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace ProNetcell.Query
{
    [Serializable]
    public class SendReportQuery:QueryBase
    {

        public string Serialize()
        {
           var ser= BinarySerializer.SerializeToBase64(this);
           return ser;
        }

        public static SignupQuery Deserialize(string ser)
        {
            return BinarySerializer.DeserializeFromBase64<SignupQuery>(ser);
        }
   
        public SendReportQuery()
        {
        }

        public SendReportQuery(HttpRequestBase Request)
        {

            this.AuthAccount = Types.ToInt(Request["AuthAccount"]);
            BatchId = Types.ToInt(Request["BatchId"]);
            Target = Request["Target"];
            DateFrom = Types.ToNullableDate(Request["SendDateFrom"]);
            DateTo = Types.ToNullableDate(Request["SendDateTo"]);
            Platform = Types.ToInt(Request["Platform"]);

            LoadSortAndFilter(Request);
        }
        public void Normelize()
        {
            if (Target == "")
               Target = null;
            if (DateFrom == null)
                DateFrom = DateTime.Now.AddMonths(-1);
            if (DateTo == null)
                DateTo = DateTime.Now;

        }

        public int AuthAccount { get; set; }
        public int Platform { get; set; }
        public DateTime? DateFrom{ get; set; }
        public DateTime? DateTo{ get; set; }
        public int BatchId { get; set; }
        public string Target { get; set; }
        public bool EnableNotif { get; set; }
       
       
    }
}
