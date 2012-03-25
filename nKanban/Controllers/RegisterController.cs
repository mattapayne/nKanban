using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nKanban.Models;
using nKanban.Services;
using nKanban.Domain;
using AutoMapper;

namespace nKanban.Controllers
{
    public class RegisterController : AbstractBaseController
    {
        private readonly IUserService _userService;

        public RegisterController(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }

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
                    _userService.LoginUser(user, false);
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
