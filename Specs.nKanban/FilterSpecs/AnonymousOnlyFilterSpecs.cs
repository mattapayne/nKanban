using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using nKanban.Filters;
using System.Web.Mvc;
using System.Web;
using FakeItEasy;
using System.Web.Routing;
using nKanban;
using System.Collections.Specialized;
using nKanban.Controllers;

namespace Specs.nKanban.FilterSpecs
{
    [Subject(typeof(AnonymousOnlyFilterAttribute))]
    public class context_for_filter
    {
        protected static ActionExecutingContext ctx;
        protected static AnonymousOnlyFilterAttribute filter;
        protected static HttpRequestBase mockRequest;
        protected static HttpContextBase mockContext;

        Establish context = () => {

            mockContext = A.Fake<HttpContextBase>();
            mockRequest = A.Fake<HttpRequestBase>();

            var controller = new HomeController();
            var stubActionDescriptor = A.Fake<ActionDescriptor>();

            var controllerContext = new ControllerContext(mockContext, new RouteData(), controller);
            controller.ControllerContext = controllerContext;

            filter = new AnonymousOnlyFilterAttribute();
            ctx = new ActionExecutingContext(controllerContext, stubActionDescriptor, new Dictionary<string, object>());

            A.CallTo(() => mockContext.Request).Returns(mockRequest);
        };
    }

    [Subject(typeof(AnonymousOnlyFilterAttribute))]
    public class when_invoked_with_anonymous_user : context_for_filter
    {
        Establish contextAnon = () => { A.CallTo(() => mockRequest.IsAuthenticated).Returns(false); };

        Because of = () => { filter.OnActionExecuting(ctx); };

        It should_not_redirect = () => { ctx.Result.ShouldBeNull(); };
    }

    [Subject(typeof(AnonymousOnlyFilterAttribute))]
    public class when_invoked_with_authenticated_user : context_for_filter
    {
        Establish contextAnon = () => { A.CallTo(() => mockRequest.IsAuthenticated).Returns(true); };

        Because of = () => { filter.OnActionExecuting(ctx); };

        It should_redirect = () => { ctx.Result.ShouldBeOfType(typeof(RedirectToRouteResult)); };

        It should_by_default_redirect_to_dashboard = () => {
            var result = ctx.Result as RedirectToRouteResult;
            result.RouteValues.Values.ShouldContain("Dashboard");
            result.RouteValues.Values.ShouldContain("Show");
        };
    }

    [Subject(typeof(AnonymousOnlyFilterAttribute))]
    public class when_invoked_with_authenticated_user_and_custom_redirect : context_for_filter
    {
        Establish contextAnon = () => 
        {
            filter.ActionToRedirectToIfAuthenticated = "TestAction";
            filter.ControllerToRedirectToIfAuthenticated = "TestController";
            A.CallTo(() => mockRequest.IsAuthenticated).Returns(true); 
        };

        Because of = () => { filter.OnActionExecuting(ctx); };

        It should_redirect = () => { ctx.Result.ShouldBeOfType(typeof(RedirectToRouteResult)); };

        It should_by_default_redirect_to_dashboard = () =>
        {
            var result = ctx.Result as RedirectToRouteResult;
            result.RouteValues.Values.ShouldContain("TestController");
            result.RouteValues.Values.ShouldContain("TestAction");
        };
    }
}
