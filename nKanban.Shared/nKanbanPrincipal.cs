using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace nKanban.Shared
{
    public class nKanbanPrincipal : IPrincipal
    {
        private readonly nKanbanIdentity _identity;

        public nKanbanPrincipal(nKanbanIdentity identity)
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

        public nKanbanIdentity CustomIdentity
        {
            get { return _identity; }
        }

        public bool IsInRole(string role)
        {
            return _identity.IsInRole(role);
        }
    }
}
