using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nKanban.Services;

namespace nKanban.Controllers
{
    [Authorize]
    public class DashboardController : AbstractBaseController
    {
        public readonly IUserService _userService;

        public DashboardController(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }

            _userService = userService;
        }

        public ActionResult Show()
        {
            return View();
        }
    }
}
