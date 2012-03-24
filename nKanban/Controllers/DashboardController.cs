using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nKanban.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public ActionResult Show()
        {
            return View();
        }
    }
}
