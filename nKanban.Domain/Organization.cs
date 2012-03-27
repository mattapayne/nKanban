using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nKanban.Domain
{
    public class Organization : AbstractDomainObject
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public Province Province { get; set; }
        public Country Country { get; set; }
        public string PostalCode { get; set; }
    }
}
