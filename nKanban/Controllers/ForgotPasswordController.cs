using System.Web.Mvc;
using nKanban.Filters;
using nKanban.Models;

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
