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

namespace Specs.nKanban.ControllerSpecs.Register
{
    [Subject(typeof(RegisterController))]
    public class context_for_controller
    {
        protected static RegisterController controller;

        Establish context = () => { controller = new RegisterController(); };
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
}
