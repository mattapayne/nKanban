using System.Web.Mvc;

namespace nKanban.Controllers
{
    public class HomeController : AbstractBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
