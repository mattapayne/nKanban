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

namespace Specs.nKanban.ControllerSpecs.Register
{
    [Subject(typeof(RegisterController))]
    public class context_for_controller
    {
        protected static RegisterController controller;
        protected static IUserService userService;

        Establish context = () => 
        {
            AppBootstrapper.Bootstrap();
            userService = A.Fake<IUserService>();
            controller = new RegisterController(userService); 
        };
    }

    [Subject(typeof(RegisterController), ": when instantiating without required dependencies")]
    public class controller_construction
    {
        static Exception exception;
        static RegisterController controller;

        Because of = () => { exception = Catch.Exception(() => { controller = new RegisterController(null); }); };

        It should_throw_an_exception = () => { exception.ShouldNotBeNull(); };
    }

    [Subject(typeof(RegisterController), ": when I go to the Register page")]
    public class the_response : context_for_controller
    {
        static ActionResult result;

        Because of = () => { result = controller.New(); };

        It should_show_the_register_view = () => { result.ShouldBeAView(); };

        It should_have_a_register_view_model = () => { ((ViewResult)result).ShouldHaveModelOfType<RegisterViewModel>(); };
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

        It should_rerender_the_register_view = () => { result.ShouldBeAView().And().ViewName.Should().ContainEquivalentOf("New"); };

        It should_maintain_the_original_model = () => { ((ViewResult)result).Model.ShouldBeTheSameAs(model); };
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

            A.CallTo(() => userService.CreateUser(A<User>.Ignored, A<String>.Ignored)).Returns(new List<ServiceError>());
        };

        Because of = () =>
        {
            result = controller.Create(model);
        };

        It should_create_the_user = () => { A.CallTo(() => userService.CreateUser(A<User>.Ignored, A<String>.Ignored)).MustHaveHappened(Repeated.Exactly.Once); };

        It should_log_the_user_in = () => { A.CallTo(() => userService.LoginUser(A<User>.Ignored, false)).MustHaveHappened(Repeated.Exactly.Once); };

        It should_redirect_to_the_dashboard = () => { result.ShouldBeARedirectToRoute(); };

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
            A.CallTo(() => userService.CreateUser(A<User>.Ignored, A<string>.Ignored)).Returns(errors);
        };

        Because of = () =>
        {
            result = controller.Create(model);
        };

        It should_attempt_to_create_the_user = () => { A.CallTo(() => userService.CreateUser(A<User>.Ignored, A<string>.Ignored)).MustHaveHappened(Repeated.Exactly.Once); };

        It should_not_log_the_user_in = () => { A.CallTo(() => userService.LoginUser(A<User>.Ignored, A<bool>.Ignored)).MustNotHaveHappened(); };

        It should_rerender_the_register_view = () => { result.ShouldBeAView().And().ViewName.Should().ContainEquivalentOf("New"); };

        It should_have_an_error_message = () =>
        {
            controller.ModelState.ShouldNotBeEmpty();
        };
    }
}
