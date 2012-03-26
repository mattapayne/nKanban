using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nKanban.Shared;
using nKanban.Domain;
using System.Web.Security;
using System.Web;
using System.Web.Script.Serialization;

namespace nKanban.Services.Impl
{
    public class LoginService : ILoginService
    {
        private readonly JavaScriptSerializer _serializer;
        private readonly IHttpContext _context;

        public LoginService(IHttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _context = context;
            _serializer = new JavaScriptSerializer();
        }

        public void LoginUser(User user, bool persistent)
        {
            var ticket = new FormsAuthenticationTicket(1, user.Id.ToString(), DateTime.Now, DateTime.Now.AddMinutes(30), persistent, _serializer.Serialize(user.ToJson()));

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            _context.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
        }

        public void Logoff()
        {
            FormsAuthentication.SignOut();
        }
    }
}
