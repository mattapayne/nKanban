using System;
using Machine.Specifications;
using nKanban.Domain;
using nKanban.Services.Impl;


namespace Specs.nKanban.ServiceSpecs.LoginSvc
{
    [Subject(typeof(LoginService))]
    public class context_for_service
    {
        protected static LoginService service;
        protected static StubHttpContext ctx;

        Establish context = () => {
            ctx = new StubHttpContext();
            service = new LoginService(ctx);
        };
    }

    [Subject(typeof(LoginService))]
    public class when_constructed_without_http_context
    {
        static Exception exception;

        Because of = () => { exception = Catch.Exception(() => new LoginService(null) ); };

        It should_throw_an_exception = () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(LoginService))]
    public class when_logging_off : context_for_service
    {
        Because of = () => {
            service.LoginUser(new User() { FirstName = "First", LastName = "Last", Email = "test@test.ca", Id = Guid.NewGuid() }, false);
            service.Logoff(); 
        };

        It should_set_an_expired_cookie = () => 
        {
            ctx.Response.Cookies.ShouldNotBeEmpty();
            var cookie = ctx.Response.Cookies.Get(0);
            cookie.ShouldNotBeNull();
            cookie.Expires.ShouldBeLessThan(DateTime.UtcNow);
        };
    }

    [Subject(typeof(LoginService))]
    public class when_logging_a_user_in : context_for_service
    {
        static User user;
        static string userData;

        Establish context1 = () => {
            user = new User() { FirstName = "First", LastName = "Last", Email = "test@test.ca", Id = Guid.NewGuid() };
        };

        Because of = () => 
        { 
            service.LoginUser(user, true);
            userData = TestUtilities.DeconstructAuthCookie(ctx);
        };

        It should_set_a_cookie = () => ctx.Request.Cookies.ShouldNotBeEmpty();

        It should_have_user_data = () => userData.ShouldNotBeEmpty();

        It should_have_user_email = () => userData.ShouldContain("test@test.ca");

        It should_have_user_full_name = () => userData.ShouldContain("First Last");
    }
}
