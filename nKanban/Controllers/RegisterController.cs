using System;
using System.Linq;
using System.Web.Mvc;
using nKanban.Models;
using nKanban.Services;
using nKanban.Domain;
using AutoMapper;
using nKanban.Filters;

namespace nKanban.Controllers
{
    [AnonymousOnlyFilter]
    public class RegisterController : AbstractBaseController
    {
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        private readonly ISimpleService _lookupService;

        public RegisterController(IUserService userService, ILoginService loginService, ISimpleService lookupService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }

            if (loginService == null)
            {
                throw new ArgumentNullException("loginService");
            }

            if (lookupService == null)
            {
                throw new ArgumentNullException("lookupService");
            }

            _lookupService = lookupService;
            _loginService = loginService;
            _userService = userService;
        }

        public ActionResult New()
        {
            var model = new RegisterViewModel();
            PopulateRegisterModel(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Mapper.Map<User>(model);
                var organization = Mapper.Map<Organization>(model);

                if (model.CountryId.HasValue)
                {
                    organization.Country = _lookupService.Get<Country>(model.CountryId.Value);
                }

                if (model.ProvinceId.HasValue)
                {
                    organization.Province = _lookupService.Get<Province>(model.ProvinceId.Value);
                }

                var errors = _userService.CreateUser(user, model.Password, organization);

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

            PopulateRegisterModel(model);
            return View("New", model);
        }

        private void PopulateRegisterModel(RegisterViewModel model)
        {
            var countries = _lookupService.GetAll<Country>().
                OrderBy(c => c.Name).Select(c => new SelectListItem()
                                                {
                                                    Text = c.Name,
                                                    Value = c.Id.ToString(),
                                                    Selected = model.CountryId.HasValue && model.CountryId.Value == c.Id
                                                }).ToList();
            model.Countries = countries;

            if (model.CountryId.HasValue)
            {
                var provinces = _lookupService.GetAll<Province>(p => p.CountryId == model.CountryId.Value).
                    OrderBy(p => p.Name).Select(p => new SelectListItem() 
                                                    { 
                                                        Selected = model.ProvinceId.HasValue && model.ProvinceId.Value == p.Id,
                                                        Text = p.Name,
                                                        Value = p.Id.ToString()
                                                    }).ToList();

                model.Provinces = provinces;
            }
        }
    }
}
