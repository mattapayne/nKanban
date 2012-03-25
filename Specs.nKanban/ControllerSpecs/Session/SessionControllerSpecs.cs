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
using nKanban.Services;
using FakeItEasy;

namespace Specs.nKanban.ControllerSpecs.Session
{
    [Subject(typeof(SessionController))]
    public class context_for_controller
    {
        protected static SessionController controller;
        protected static IUserService service;

        Establish context = () => 
        {
            service = A.Fake<IUserService>();
            controller = new SessionController(service); 
        };
    }

    [Subject(typeof(SessionController), ": when instantiating without required dependencies")]
    public class controller_construction
    {
        static Exception exception;
        static SessionController controller;

        Because of = () => { exception = Catch.Exception(() => { controller = new SessionController(null); }); };

        It should_throw_an_exception = () => { exception.ShouldNotBeNull(); };
    }

    [Subject(typeof(SessionController), ": when I go to the Login page")]
    public class the_response : context_for_controller
    {
        static ActionResult result;

        Because of = () => { result = controller.New(); };

        It should_show_the_login_view = () => { result.ShouldBeAView(); };

        It should_have_a_login_view_model = () => { ((ViewResult)result).ShouldHaveModelOfType<LoginViewModel>(); };
    }

    [Subject(typeof(SessionController), ": when I try to Login with missing or invalid data")]
    public class the_response_to_missing_or_bad_data : context_for_controller
    {
        static ActionResult result;
        static LoginViewModel model;

        Because of = () =>
        {
            model = new LoginViewModel() { UserName = "test" };
            controller.ModelState.AddModelError("Anything", "Anything");
            result = controller.Create(model);
        };

        It should_rerender_the_register_view = () => { result.ShouldBeAView().And().ViewName.Should().ContainEquivalentOf("New"); };

        It should_maintain_the_original_model = () => { ((ViewResult)result).Model.ShouldBeTheSameAs(model); };
    }
}
