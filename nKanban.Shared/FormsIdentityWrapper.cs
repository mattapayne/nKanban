using System;
using System.Security.Principal;
using System.Web.Security;

namespace nKanban.Shared
{
    public class FormsIdentityWrapper : IFormsIdentity
    {
        private readonly FormsIdentity _identity;

        public FormsIdentityWrapper(IIdentity identity)
        {
            if(identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            var formsIdentity = identity as FormsIdentity;

            if(formsIdentity == null)
            {
                throw new ArgumentException("identity");
            }

            _identity = formsIdentity;
        }

        public FormsAuthenticationTicket Ticket
        {
            get { return _identity.Ticket; }
        }

        public string AuthenticationType
        {
            get { return _identity.AuthenticationType; }
        }

        public bool IsAuthenticated
        {
            get { return _identity.IsAuthenticated; }
        }

        public string Name
        {
            get { return _identity.Name; }
        }
    }
}
