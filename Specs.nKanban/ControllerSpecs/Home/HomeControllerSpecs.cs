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

namespace Specs.nKanban.ControllerSpecs.Home
{
    [Subject(typeof(HomeController))]
    public class context_for_controller
    {
        protected static HomeController controller;

        Establish context = () => { controller = new HomeController(); };
    }

    [Subject(typeof(HomeController), ": when I go to the root page")]
    public class the_root_response : context_for_controller
    {
        static ViewResult result;

        Because of = () => { result = controller.Index() as ViewResult; };

        It should_show_the_root_view = () => result.ShouldBeAView();
    }

    [Subject(typeof(HomeController), ": when I go to the About page")]
    public class the_about_response : context_for_controller
    {
        static ViewResult result;

        Because of = () => { result = controller.About() as ViewResult; };

        It should_show_the_about_view = () => result.ShouldBeAView();
    }
}
