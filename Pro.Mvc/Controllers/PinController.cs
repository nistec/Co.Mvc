using Pro.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pro.Mvc.Controllers
{
    [Authorize]
    public class PinController : BaseController
    {
        //
        // GET: /Pin/
        [HttpGet]
        public ActionResult TaskEdit(int id)
        {
            return View(true, "TaskEdit", "~/Shared/_ViewIframe.cshtml", new EditTaskModel() { Id = id, Option = "e", Layout= "_ViewIframe" });
        }
        [HttpGet]
        public ActionResult TaskInfo(int id)
        {
            return View(true, "TaskInfo", "~/Shared/_ViewIframe.cshtml", new EditTaskModel() { Id = id, Option = "g", IsInfo = true, Layout = "_ViewIframe" });
        }

    }
}
