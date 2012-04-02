using System.Security.Principal;
using System.Web.Security;

namespace nKanban.Shared
{
    public interface IFormsIdentity : IIdentity
    {
        FormsAuthenticationTicket Ticket { get; }
    }
}
