using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Web.Security;
using nKanban.Shared;

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
    }
}
