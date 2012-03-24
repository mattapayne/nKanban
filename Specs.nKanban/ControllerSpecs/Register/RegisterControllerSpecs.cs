﻿using System;
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
}
