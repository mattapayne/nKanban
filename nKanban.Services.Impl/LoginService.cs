using System;
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
            var cookie = _context.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (cookie != null)
            {
                //create a new expired cookie based on this one
                var expired = new HttpCookie(FormsAuthentication.FormsCookieName);

                expired.HttpOnly = true;
                expired.Path = FormsAuthentication.FormsCookiePath;
                expired.Expires = DateTime.UtcNow.AddDays(-2);
                expired.Secure = FormsAuthentication.RequireSSL;

                _context.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
                _context.Response.Cookies.Add(expired);
            }
        }
    }
}
