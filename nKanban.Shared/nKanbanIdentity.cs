using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Web.Security;
using System.Web.Script.Serialization;

namespace nKanban.Shared
{
    public class nKanbanIdentity : IIdentity
    {
        private Guid? _id;
        private JavaScriptSerializer _serializer;

        private class IdentityData
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public List<string> Roles { get; set; }

            public IdentityData()
            {

            }
        }

        public nKanbanIdentity(IIdentity identity)
        {
            if (identity is FormsIdentity)
            {
                _serializer = new JavaScriptSerializer();

                FormsIdentity i = identity as FormsIdentity;
                Guid id;

                if (Guid.TryParse(identity.Name, out id))
                {
                    this._id = id;
                    var ident = _serializer.Deserialize(i.Ticket.UserData, typeof(IdentityData)) as IdentityData;
                    this.Name = ident.Name;
                    this.Roles = ident.Roles;
                    this.Email = ident.Email;
                }
            }
        }

        public string AuthenticationType
        {
            get { return "nKanbanCustomAuth"; }
        }

        public bool IsAuthenticated
        {
            get { return _id.HasValue; }
        }

        public string Name { get; private set; }

        public string Email { get; private set; }

        private List<string> Roles { get; set; }

        public Guid? Id
        {
            get { return _id; }
        }

        public bool IsInRole(string role)
        {
            return Roles == null ? false : Roles.Any(r => r == role);
        }
    }
}
