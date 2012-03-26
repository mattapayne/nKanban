using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nKanban.Models;
using nKanban.Services;
using nKanban.Infrastructure;
using nKanban.Filters;

namespace nKanban.Controllers
{
    public class SessionController : AbstractBaseController
    {
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;

        public SessionController(IUserService userService, ILoginService loginService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }

            if (loginService == null)
            {
                throw new ArgumentNullException("loginService");
            }

            _loginService = loginService;
            _userService = userService;
        }

        [AnonymousOnlyFilter]
        public ActionResult New()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AnonymousOnlyFilter]
        public ActionResult Create(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var errors = _userService.VerifyLogin(model.UserName, model.Password);

                if (!errors.Any())
                {
                    var user = _userService.GetUser(model.UserName);
                    _loginService.LoginUser(user, model.RememberMe);
                    return RedirectToRoute("Dashboard");
                }
            }

            return View("New", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Delete()
        {
            _loginService.Logoff();
            SetRedirectMessage(MessageType.Success, "Successfully logged you out of the application.");
            return RedirectToAction("Index", "Home");
        }
    }
}
