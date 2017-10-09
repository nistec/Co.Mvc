using Pro.Data.Entities;
using Pro.Lib.Query;
using Pro.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pro.Mvc.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
         [HttpGet]
        public ActionResult ReportQuery()
        {
            return View(true);
        }
        [HttpPost]
         public ActionResult ReportStatistic()
        {
            StatisticReportQuery rm = new StatisticReportQuery(Request.Form, GetAccountId());
            return View(true,rm);
        }
        [HttpPost]
        public String GetStatisticReport(string report, int campaign, DateTime? dateFrom,DateTime? dateTo)
        {
            StatisticReportQuery rm = new StatisticReportQuery(report);
            rm.AccountId = GetAccountId();
            rm.Campaign = campaign;
            rm.SignupDateFrom = dateFrom;
            rm.SignupDateTo = dateTo;
            var data = ReportContext.DoStatisticReport(rm);
            return data;
        }
       
    }
}
