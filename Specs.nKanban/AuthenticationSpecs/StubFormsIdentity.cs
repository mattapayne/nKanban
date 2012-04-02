using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using nKanban.Shared;

namespace Specs.nKanban.AuthenticationSpecs
{
    public class StubFormsIdentity : IFormsIdentity
    {
        private readonly FormsAuthenticationTicket _ticket;

        public StubFormsIdentity(FormsAuthenticationTicket ticket)
        {
            _ticket = ticket;
        }

        public FormsAuthenticationTicket Ticket
        {
            get { return _ticket; }
        }

        public string AuthenticationType
        {
            get { return "Test"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return _ticket.Name; }
        }
    }
}
