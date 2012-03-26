using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using nKanban.Services.Impl;
using FakeItEasy;
using nKanban.Persistence;
using nKanban.Domain;
using System.Linq.Expressions;
using nKanban.Services;
using System.Web;
using nKanban.Shared;


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

        It should_set_a_cookie = () => { ctx.Request.Cookies.ShouldNotBeEmpty(); };

        It should_have_user_data = () => { userData.ShouldNotBeEmpty(); };

        It should_have_user_email = () => { userData.ShouldContain("test@test.ca"); };

        It should_have_user_full_name = () => { userData.ShouldContain("First Last"); };
    }
}
