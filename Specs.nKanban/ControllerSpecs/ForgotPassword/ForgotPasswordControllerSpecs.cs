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

namespace Specs.nKanban.ControllerSpecs.ForgotPassword
{
    [Subject(typeof(ForgotPasswordController))]
    public class context_for_controller
    {
        protected static ForgotPasswordController controller;

        Establish context = () => { controller = new ForgotPasswordController(); };
    }

    [Subject(typeof(ForgotPasswordController), ": when I go to the Forgot Password page")]
    public class the_response : context_for_controller
    {
        static ActionResult result;

        Because of = () => { result = controller.New(); };

        It should_show_the_forgot_password_view = () => { result.ShouldBeAView(); };

        It should_have_a_forgot_password_view_model = () => { ((ViewResult)result).ShouldHaveModelOfType<ForgotPasswordViewModel>(); };
    }
}
