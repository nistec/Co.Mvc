using Nistec;
using Nistec.Data;
using Nistec.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Pro.Netcell.Query
{
    [Serializable]
    public class StatisticReportQuery:QueryBase
    {
 

        public int AccountId { get; set; }
        public int UserId { get; set; }

        public StatisticReportQuery()
        {
            Campaign = 0;
        }

        public StatisticReportQuery(string report)
        {
            ReportType = report;
            Normelize();
        }
        public StatisticReportQuery(NameValueCollection Request, int accountId)
        {
            ReportType = Request["query_type"];
            AccountId = accountId;// Types.ToInt(Request["AccountId"]);

            string val = "";
            val = Request["Campaign"];
            Campaign = (val == "") ? 0 : Types.ToInt(Request["Campaign"], 0);

            SignupDateFrom = Types.ToNullableDate(Request["SignupDateFrom"]);
            SignupDateTo = Types.ToNullableDate(Request["SignupDateTo"]);


            Normelize();
        }

        public void Normelize()
        {
            switch (ReportType)
            {
                case "StatisticByItems":
                    HTitle = "התפלגות לפי פריט ומחיר";
                    HDesc = "";
                    break;
                case "StatisticByCategory":
                    HTitle = "התפלגות לפי סיווג";
                    HDesc = "";
                    break;
                case "StatisticByBranch":
                    HTitle = "התפלגות לפי סניפים";
                    HDesc = "";
                    break;
                case "StatisticByCampaign":
                    HTitle = "התפלגות לפי קמפיין";
                    HDesc = "";
                    break;
                case "StatisticByPayments":
                    HTitle = "התפלגות לפי תשלום";
                    HDesc = "";
                    break;
            }
        }

        public string ReportType { get; set; }
        public string HTitle { get; set; }
        public string HDesc { get; set; }
        public string PropName { get; set; }
        public DateTime? SignupDateFrom { get; set; }
        public DateTime? SignupDateTo { get; set; }
     
        public int Campaign { get; set; }
       
       
    }
}
