using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Script.Serialization;

namespace nKanban.Shared
{
    public class NKanbanIdentity : IIdentity
    {
        private readonly bool _authenticated;
        private readonly JavaScriptSerializer _serializer;

        private class IdentityData
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public List<string> Roles { get; set; }

            public IdentityData()
            {

            }
        }

        public NKanbanIdentity(Guid? id, string name, string email, IEnumerable<string> roles)
        {
            this._authenticated = false;

            if (id.HasValue)
            {
                this._authenticated = true;
                this.Id = id.Value;
            }

            this.Name = name;
            this.Email = email;
            this.Roles = roles != null ? roles.ToList() : Enumerable.Empty<string>().ToList();
        }

        public NKanbanIdentity(IFormsIdentity identity)
        {
            _serializer = new JavaScriptSerializer();
            this._authenticated = false;

            Guid id;

            if (Guid.TryParse(identity.Name, out id))
            {
                this.Id = id;
                this._authenticated = true;

                var ident = _serializer.Deserialize(identity.Ticket.UserData, typeof (IdentityData)) as IdentityData;

                if (ident != null)
                {
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
            get { return _authenticated; }
        }

        public string Name { get; private set; }

        public string Email { get; private set; }

        private List<string> Roles { get; set; }

        public Guid Id { get; private set; }

        public bool IsInRole(string role)
        {
            return Roles != null && Roles.Any(r => r == role);
        }
    }
}
