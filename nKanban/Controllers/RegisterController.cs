using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nKanban.Models;

namespace nKanban.Controllers
{
    public class RegisterController : Controller
    {
        public ActionResult New()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public ActionResult Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToRoute("Dashboard");
            }

            return View("New", model);
        }
    }
}
