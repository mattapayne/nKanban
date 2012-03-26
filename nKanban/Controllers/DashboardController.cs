using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nKanban.Controllers
{
    [Authorize]
    public class DashboardController : AbstractBaseController
    {
        public ActionResult Show()
        {
            return View();
        }
    }
}
