using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pro.Mvc.Models
{
    public class ConfirmModel
    {
        public int CanEdit { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string ID { get; set; }
    }

    public class PaymentModel
    {

        public int CanPay { get; set; }
        //public string Phone { get; set; }
        //public string Email { get; set; }
        //public string Contact { get; set; }
        //public string ID { get; set; }
        //public decimal Amount { get; set; }
        public string IframeSrc { get; set; }
    }

    public class ReportModel
    {
        public string ReportName { get; set; }
        public string HTitle { get; set; }
        public string HDesc { get; set; }
        public string PropName { get; set; }

        public ReportModel() { }

        public ReportModel(string report)
        {
            ReportName = report;
            switch( report)
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
            }

        }

        
    }
}