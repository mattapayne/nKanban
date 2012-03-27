using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nKanban.Domain
{
    public class Province : AbstractDomainObject
    {
        public string Name { get; set; }
        public Guid CountryId { get; set; }
    }
}
