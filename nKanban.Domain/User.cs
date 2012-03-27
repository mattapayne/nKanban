using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nKanban.Domain
{
    public class User : AbstractDomainObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public Guid? OrganizationId { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public User()
        {
            Roles = Enumerable.Empty<string>();
        }

        public object ToJson()
        {
            return new { Name = String.Format("{0} {1}", FirstName, LastName), Email = Email, Roles = Roles.ToArray() };
        }
    }
}
