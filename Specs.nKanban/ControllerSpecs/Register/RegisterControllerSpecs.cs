using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using nKanban.Controllers;
using System.Web.Mvc;
using FluentAssertions;
using Machine.Specifications.Mvc;
using nKanban.Models;
using FakeItEasy;
using nKanban.Services;
using nKanban.Domain;
using nKanban.Infrastructure;
using System.Linq.Expressions;

namespace Specs.nKanban.ControllerSpecs.Register
{
    [Subject(typeof(RegisterController))]
    public class context_for_controller
    {
        protected static RegisterController controller;
        protected static IUserService userService;
        protected static ILoginService loginService;
        protected static ISimpleService lookupService;
        protected static IEnumerable<Country> countries;
        protected static IEnumerable<Province> provinces;

        Establish context = () => 
        {
            AppBootstrapper.Bootstrap();

            countries = TestUtilities.GetCountries();
            provinces = TestUtilities.GetProvinces();

            lookupService = A.Fake<ISimpleService>();
            userService = A.Fake<IUserService>();
            loginService = A.Fake<ILoginService>();
            controller = new RegisterController(userService, loginService, lookupService);

            A.CallTo(() => lookupService.GetAll<Country>(A<Expression<Func<Country, bool>>[]>.Ignored)).Returns(countries);
            A.CallTo(() => lookupService.GetAll<Province>(A<Expression<Func<Province, bool>>[]>.Ignored)).Returns(provinces);
        };
    }

    [Subject(typeof(RegisterController), ": when instantiating without lookup service")]
    public class controller_construction_without_lookup_service
    {
        static Exception exception;
        static RegisterController controller;
        protected static IUserService userService;
        protected static ILoginService loginService;
        protected static ISimpleService lookupService;

        Establish ctx = () => {
            lookupService = A.Fake<ISimpleService>();
            userService = A.Fake<IUserService>();
            loginService = A.Fake<ILoginService>();
        };

        Because of = () => { exception = Catch.Exception(() => { controller = new RegisterController(userService, loginService, null); }); };

        It should_throw_an_exception = () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(RegisterController), ": when instantiating without login service")]
    public class controller_construction_without_login_service
    {
        static Exception exception;
        static RegisterController controller;
        protected static IUserService userService;
        protected static ILoginService loginService;
        protected static ISimpleService lookupService;

        Establish ctx = () =>
        {
            lookupService = A.Fake<ISimpleService>();
            userService = A.Fake<IUserService>();
            loginService = A.Fake<ILoginService>();
        };

        Because of = () => { exception = Catch.Exception(() => { controller = new RegisterController(userService, null, lookupService); }); };

        It should_throw_an_exception = () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(RegisterController), ": when instantiating without user service")]
    public class controller_construction_without_user_service
    {
        static Exception exception;
        static RegisterController controller;
        protected static IUserService userService;
        protected static ILoginService loginService;
        protected static ISimpleService lookupService;

        Establish ctx = () =>
        {
            lookupService = A.Fake<ISimpleService>();
            userService = A.Fake<IUserService>();
            loginService = A.Fake<ILoginService>();
        };

        Because of = () => { exception = Catch.Exception(() => { controller = new RegisterController(null, loginService, lookupService); }); };

        It should_throw_an_exception = () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(RegisterController), ": when I go to the Register page")]
    public class the_response : context_for_controller
    {
        static ActionResult result;

        Because of = () => { result = controller.New(); };

        It should_show_the_register_view = () => result.ShouldBeAView();

        It should_have_a_register_view_model = () => ((ViewResult)result).ShouldHaveModelOfType<RegisterViewModel>();

        It should_load_the_countries = () => A.CallTo(() => lookupService.GetAll<Country>(A<Expression<Func<Country, bool>>[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

        It should_have_a_collection_of_countries = () => {
            var model = ((ViewResult)result).Model as RegisterViewModel;
            model.Countries.Count().ShouldBeGreaterThan(0);
        };
    }

    [Subject(typeof(RegisterController), ": when I try to Register with missing or invalid data")]
    public class the_response_to_missing_or_bad_data : context_for_controller
    {
        static ActionResult result;
        static RegisterViewModel model;

        Because of = () => 
        {
            model = new RegisterViewModel() { FirstName = "test", LastName = "test" };
            controller.ModelState.AddModelError("Anything", "Anything");
            result = controller.Create(model); 
        };

        It should_rerender_the_register_view = () => result.ShouldBeAView().And().ViewName.Should().ContainEquivalentOf("New");

        It should_maintain_the_original_model = () => ((ViewResult)result).Model.ShouldBeTheSameAs(model);

        It should_reload_the_countries = () => A.CallTo(() => lookupService.GetAll<Country>(A<Expression<Func<Country, bool>>[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

        It should_have_a_collection_of_countries = () =>
        {
            var model = ((ViewResult)result).Model as RegisterViewModel;
            model.Countries.Count().ShouldBeGreaterThan(0);
        };
    }

    [Subject(typeof(RegisterController), ": when I try to Register with missing or invalid data with a province selected")]
    public class the_response_to_missing_or_bad_data_with_a_province_selected : context_for_controller
    {
        static ActionResult result;
        static RegisterViewModel model;
        static Guid provinceId = new Guid("dbf0c87a-c922-4a46-a99d-7efbfe04e94d");
        static Guid countryId = new Guid("bcea0d14-8d95-4ea0-8ef1-8d1c22e09445");

        Because of = () =>
        {
            model = new RegisterViewModel() { FirstName = "test", LastName = "test", ProvinceId = provinceId, CountryId = countryId };
            controller.ModelState.AddModelError("Anything", "Anything");
            result = controller.Create(model);
        };

        It should_rerender_the_register_view = () => result.ShouldBeAView().And().ViewName.Should().ContainEquivalentOf("New");

        It should_maintain_the_original_model = () => ((ViewResult)result).Model.ShouldBeTheSameAs(model);

        It should_reload_the_countries = () => A.CallTo(() => lookupService.GetAll<Country>(A<Expression<Func<Country, bool>>[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

        It should_reload_the_provinces = () => A.CallTo(() => lookupService.GetAll<Province>(A<Expression<Func<Province, bool>>[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

        It should_have_a_collection_of_countries = () =>
        {
            var model = ((ViewResult)result).Model as RegisterViewModel;
            model.Countries.Count().ShouldBeGreaterThan(0);
        };

        It should_have_a_collection_of_provinces = () =>
        {
            var model = ((ViewResult)result).Model as RegisterViewModel;
            model.Provinces.Count().ShouldBeGreaterThan(0);
        };
    }

    [Subject(typeof(RegisterController), ": when I try to Register with valid data and a unique email address")]
    public class the_response_to_valid_data : context_for_controller
    {
        static ActionResult result;
        static RegisterViewModel model;

        Establish ctx = () => 
        {
            model = new RegisterViewModel()
            {
                FirstName = "Joe",
                LastName = "Smith",
                Email = "joe.smith@test.ca",
                Password = "232423",
                PasswordConfirmation = "232423"
            };

            A.CallTo(() => userService.CreateUser(A<User>.Ignored, A<String>.Ignored, A<Organization>.Ignored)).Returns(new List<ServiceError>());
        };

        Because of = () =>
        {
            result = controller.Create(model);
        };

        It should_create_the_user = () => A.CallTo(() => userService.CreateUser(A<User>.Ignored, A<String>.Ignored, A<Organization>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

        It should_log_the_user_in = () => A.CallTo(() => loginService.LoginUser(A<User>.Ignored, false)).MustHaveHappened(Repeated.Exactly.Once);

        It should_redirect_to_the_dashboard = () => result.ShouldBeARedirectToRoute();

        It should_have_a_success_message = () => 
        {
            controller.TempData.ShouldNotBeEmpty();
            controller.TempData.First().Key.Should().ContainEquivalentOf("success");
        };
    }

    [Subject(typeof(RegisterController), ": when I try to Register with valid data and a country")]
    public class the_response_to_valid_data_with_country_set : context_for_controller
    {
        static ActionResult result;
        static RegisterViewModel model;
        static Guid countryId = new Guid("dbf0c87a-c922-4a46-a99d-7efbfe04e94d");
        static Country country;

        Establish ctx = () =>
        {
            country = new Country() { Id = countryId, Name = "Canada" };

            model = new RegisterViewModel()
            {
                FirstName = "Joe",
                LastName = "Smith",
                Email = "joe.smith@test.ca",
                Password = "232423",
                PasswordConfirmation = "232423",
                CountryId = countryId
            };

            A.CallTo(() => userService.CreateUser(A<User>.Ignored, A<String>.Ignored, A<Organization>.Ignored)).Returns(new List<ServiceError>());
            A.CallTo(() => lookupService.Get<Country>(countryId)).Returns(country);
        };

        Because of = () =>
        {
            result = controller.Create(model);
        };

        It should_lookup_the_country = () => A.CallTo(() => lookupService.Get<Country>(countryId)).MustHaveHappened(Repeated.Exactly.Once);

        It should_create_the_user = () => A.CallTo(() => userService.CreateUser(A<User>.Ignored, A<String>.Ignored, A<Organization>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

        It should_log_the_user_in = () => A.CallTo(() => loginService.LoginUser(A<User>.Ignored, false)).MustHaveHappened(Repeated.Exactly.Once);

        It should_redirect_to_the_dashboard = () => result.ShouldBeARedirectToRoute();

        It should_have_a_success_message = () =>
        {
            controller.TempData.ShouldNotBeEmpty();
            controller.TempData.First().Key.Should().ContainEquivalentOf("success");
        };
    }

    [Subject(typeof(RegisterController), ": when I try to Register with valid data and a province")]
    public class the_response_to_valid_data_with_province_set : context_for_controller
    {
        static ActionResult result;
        static RegisterViewModel model;
        static Guid provinceId = new Guid("dbf0c87a-c922-4a46-a99d-7efbfe04e94d");
        static Province province;

        Establish ctx = () =>
        {
            province = new Province() { Id = provinceId, Name = "British Columbia" };

            model = new RegisterViewModel()
            {
                FirstName = "Joe",
                LastName = "Smith",
                Email = "joe.smith@test.ca",
                Password = "232423",
                PasswordConfirmation = "232423",
                ProvinceId = provinceId
            };

            A.CallTo(() => userService.CreateUser(A<User>.Ignored, A<String>.Ignored, A<Organization>.Ignored)).Returns(new List<ServiceError>());
            A.CallTo(() => lookupService.Get<Province>(provinceId)).Returns(province);
        };

        Because of = () =>
        {
            result = controller.Create(model);
        };

        It should_lookup_the_province = () => A.CallTo(() => lookupService.Get<Province>(provinceId)).MustHaveHappened(Repeated.Exactly.Once);

        It should_create_the_user = () => A.CallTo(() => userService.CreateUser(A<User>.Ignored, A<String>.Ignored, A<Organization>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

        It should_log_the_user_in = () => A.CallTo(() => loginService.LoginUser(A<User>.Ignored, false)).MustHaveHappened(Repeated.Exactly.Once);

        It should_redirect_to_the_dashboard = () => result.ShouldBeARedirectToRoute();

        It should_have_a_success_message = () =>
        {
            controller.TempData.ShouldNotBeEmpty();
            controller.TempData.First().Key.Should().ContainEquivalentOf("success");
        };
    }

    [Subject(typeof(RegisterController), ": when I try to Register with valid data and a unique email address, but the service returns errors")]
    public class the_response_to_service_errors : context_for_controller
    {
        static ActionResult result;
        static RegisterViewModel model;

        Establish ctx = () =>
        {
            model = new RegisterViewModel()
            {
                FirstName = "Joe",
                LastName = "Smith",
                Email = "joe.smith@test.ca",
                Password = "232423",
                PasswordConfirmation = "232423"
            };

            var errors = new List<ServiceError>() { new ServiceError("Error") };
            A.CallTo(() => userService.CreateUser(A<User>.Ignored, A<String>.Ignored, A<Organization>.Ignored)).Returns(errors);
        };

        Because of = () =>
        {
            result = controller.Create(model);
        };

        It should_attempt_to_create_the_user = () => A.CallTo(() => userService.CreateUser(A<User>.Ignored, A<String>.Ignored, A<Organization>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

        It should_not_log_the_user_in = () => A.CallTo(() => loginService.LoginUser(A<User>.Ignored, A<bool>.Ignored)).MustNotHaveHappened();

        It should_rerender_the_register_view = () => result.ShouldBeAView().And().ViewName.Should().ContainEquivalentOf("New");

        It should_have_an_error_message = () => controller.ModelState.ShouldNotBeEmpty();

        It should_have_the_original_model_in_viewdata = () => 
        {
            var viewResult = result as ViewResult;
            viewResult.Model.ShouldBeTheSameAs(model);
        };
    }
}
