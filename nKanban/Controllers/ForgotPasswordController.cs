using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nKanban.Models;
using nKanban.Filters;

namespace nKanban.Controllers
{
    [AnonymousOnlyFilter]
    public class ForgotPasswordController : AbstractBaseController
    {
        public ActionResult New()
        {
            return View(new ForgotPasswordViewModel());
        }
    }
}
