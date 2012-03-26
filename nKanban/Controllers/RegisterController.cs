using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nKanban.Models;
using nKanban.Services;
using nKanban.Domain;
using AutoMapper;
using nKanban.Infrastructure;
using nKanban.Filters;

namespace nKanban.Controllers
{
    [AnonymousOnlyFilter]
    public class RegisterController : AbstractBaseController
    {
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;

        public RegisterController(IUserService userService, ILoginService loginService)
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

        public ActionResult New()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public ActionResult Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Mapper.Map<User>(model);

                var errors = _userService.CreateUser(user, model.Password);

                if (!errors.Any())
                {
                    _loginService.LoginUser(user, false);
                    SetRedirectMessage(MessageType.Success, "Successfully created your account.");
                    return RedirectToRoute("Dashboard");
                }
                else
                {
                    AddServiceErrors(errors);
                }
            }

            return View("New", model);
        }
    }
}
