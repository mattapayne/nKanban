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

namespace Specs.nKanban.ControllerSpecs.Dashboard
{
    [Subject(typeof(DashboardController))]
    public class context_for_controller
    {
        protected static DashboardController controller;

        Establish context = () => { controller = new DashboardController(); };
    }

    [Subject(typeof(DashboardController), ": when I go to the Dashboard page")]
    public class the_response : context_for_controller
    {
        static ActionResult result;

        Because of = () => { result = controller.Show(); };

        It should_show_the_dashboard_view = () => { result.ShouldBeAView(); };
    }
}
