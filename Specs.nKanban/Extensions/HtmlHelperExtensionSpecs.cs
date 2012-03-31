using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using nKanban.Extensions;
using Machine.Specifications;
using System.Web.Routing;
using nKanban;
using FakeItEasy;

namespace Specs.nKanban.Extensions
{
    [Subject(typeof(HtmlHelper))]
    public class context_for_helper
    {
        protected static HtmlHelper<TestModel> helper;
        protected static TestModel model;

        Establish context = () => {
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);

            model = new TestModel() { Name = "test" };

            var viewContext = new ViewContext();
            viewContext.ViewData = new ViewDataDictionary(model);

            var viewContainer = A.Fake<IViewDataContainer>();

            helper = new HtmlHelper<TestModel>(viewContext, viewContainer, routes);
        };
    }

    [Subject(typeof(HtmlHelper))]
    public class when_rendering_labels_with_no_additional_attributes : context_for_helper
    {
        static MvcHtmlString html;
        private const string result = "<label for=\"Name\">Name</label>";

        Because of = () => {

            html = helper.LabelFor(m => m.Name, null);
        };

        It should_not_have_any_additional_html = () => html.ToString().ShouldBeEqualIgnoringCase(result);
    }

    [Subject(typeof(HtmlHelper))]
    public class when_rendering_labels_with_additional_attributes : context_for_helper
    {
        static MvcHtmlString html;
        private const string result = "<label for=\"Name\" class=\"test-class\" data_val=\"some-val\">Name</label>";

        Because of = () =>
        {
            html = helper.LabelFor(m => m.Name, new { @class= "test-class", data_val = "some-val" });
        };

        It should_have_additional_html = () => html.ToString().ShouldBeEqualIgnoringCase(result);
    }

    [Subject(typeof(HtmlHelper))]
    public class when_rendering_labels_where_display_name_is_set
    {
        static MvcHtmlString html;
        private const string result = "<label for=\"Name\">FullName</label>";
        static HtmlHelper<TestModel2> helper;
        static TestModel2 model;

        Establish context = () =>
        {
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);

            model = new TestModel2() { Name = "test" };

            var viewContext = new ViewContext();
            viewContext.ViewData = new ViewDataDictionary(model);

            var viewContainer = A.Fake<IViewDataContainer>();

            helper = new HtmlHelper<TestModel2>(viewContext, viewContainer, routes);
        };

        Because of = () =>
        {
            html = helper.LabelFor(m => m.Name, null);
        };

        It should_render_the_display_name = () => html.ToString().ShouldBeEqualIgnoringCase(result);
    }
}
