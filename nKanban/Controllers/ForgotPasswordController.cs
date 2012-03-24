using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nKanban.Models;

namespace nKanban.Controllers
{
    public class ForgotPasswordController : Controller
    {
        public ActionResult New()
        {
            return View(new ForgotPasswordViewModel());
        }
    }
}
