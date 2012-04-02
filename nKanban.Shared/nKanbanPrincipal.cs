using System;
using System.Security.Principal;

namespace nKanban.Shared
{
    public class NKanbanPrincipal : IPrincipal
    {
        private readonly NKanbanIdentity _identity;

        public NKanbanPrincipal(NKanbanIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            _identity = identity;
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        public bool IsInRole(string role)
        {
            return _identity.IsInRole(role);
        }

        public Guid Id
        {
            get
            {
                return _identity.Id;
            }
        }

        public string Name
        {
            get
            {
                return _identity.Name;
            }
        }

        public string Email
        {
            get
            {
                return _identity.Email;
            }
        }
    }
}
