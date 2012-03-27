using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Web.Security;
using nKanban.Shared;
using System.Web.Mvc;
using FakeItEasy;
using System.Web.Routing;
using nKanban.Domain;

namespace Specs.nKanban
{
    public static class TestUtilities
    {
        public static string DeconstructAuthCookie(IHttpContext ctx)
        {
            var cookie = ctx.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            if (cookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                return ticket.UserData;
            }

            return String.Empty;
        }

        public static void StubLoggedInStatus(Controller controller, nKanbanPrincipal user, bool loggedin = true)
        {
            //stub out HttpContext.Request.IsAuthenticated and HttpContext.Request.User
            var stubContext = A.Fake<HttpContextBase>();
            var stubRequest = A.Fake<HttpRequestBase>();

            A.CallTo(() => stubContext.Request).Returns(stubRequest);
            A.CallTo(() => stubRequest.IsAuthenticated).Returns(loggedin);
            A.CallTo(() => stubContext.User).Returns(user);

            controller.ControllerContext = new ControllerContext(stubContext, new RouteData(), controller);
        }

        public static IEnumerable<Country> GetCountries()
        {
            return new List<Country>() { new Country() { Id = Guid.NewGuid(), Name = "Canada" }, new Country() { Id = Guid.NewGuid(), Name = "USA" } };
        }

        public static IEnumerable<Province> GetProvinces()
        {
            return new List<Province>() { new Province() { Id = Guid.NewGuid(), Name = "BC" }, new Province() { Id = Guid.NewGuid(), Name = "ON" } };
        }
    }
}
