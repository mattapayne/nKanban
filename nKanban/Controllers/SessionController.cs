using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nKanban.Models;
using nKanban.Services;

namespace nKanban.Controllers
{
    public class SessionController : AbstractBaseController
    {
        private readonly IUserService _userService;

        public SessionController(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }

            _userService = userService;
        }

        public ActionResult New()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Create(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var errors = _userService.VerifyLogin(model.UserName, model.Password);

                if (!errors.Any())
                {
                    var user = _userService.GetUser(model.UserName);
                    _userService.LoginUser(user, model.RememberMe);
                    return RedirectToRoute("Dashboard");
                }
            }

            return View("New", model);
        }
    }
}
