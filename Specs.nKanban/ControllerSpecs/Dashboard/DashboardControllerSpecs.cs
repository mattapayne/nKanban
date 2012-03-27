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

namespace Specs.nKanban.ControllerSpecs.Dashboard
{
    [Subject(typeof(DashboardController))]
    public class context_for_controller
    {
        protected static IUserService userService;
        protected static DashboardController controller;

        Establish context = () => {
            userService = A.Fake<IUserService>();
            controller = new DashboardController(userService); 
        };
    }

    [Subject(typeof(DashboardController))]
    public class construction_without_dependencies : context_for_controller
    {
        static Exception exception;

        Because of = () => { exception = Catch.Exception(() => { new DashboardController(null); }); };

        It should_throw_an_exception = () => { exception.ShouldNotBeNull(); };
    }

    [Subject(typeof(DashboardController), ": when I go to the Dashboard page")]
    public class the_response : context_for_controller
    {
        static ActionResult result;

        Because of = () => { result = controller.Show(); };

        It should_show_the_dashboard_view = () => { result.ShouldBeAView(); };
    }
}
