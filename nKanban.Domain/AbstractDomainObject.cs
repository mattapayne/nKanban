using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nKanban.Domain
{
    public abstract class AbstractDomainObject
    {
        public Guid? Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public bool IsNew { get { return !Id.HasValue; } }
    }
}
