using System;
using System.Linq;
using System.Web.Mvc;
using nKanban.Services;
using nKanban.Domain;
using System.Web.SessionState;

namespace nKanban.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class LookupController : AbstractBaseController
    {
        private readonly ISimpleService _lookupService;

        public LookupController(ISimpleService lookupService)
        {
            if (lookupService == null)
            {
                throw new ArgumentNullException("lookupService");
            }

            _lookupService = lookupService;
        }

        [HttpGet]
        public ActionResult Provinces(Guid id)
        {
            var provinces = _lookupService.GetAll<Province>(p => p.CountryId == id).OrderBy(p => p.Name).ToList();
            return Json(provinces, JsonRequestBehavior.AllowGet);
        }
    }
}
