using System;

namespace nKanban.Domain
{
    public class Province : AbstractDomainObject
    {
        public string Name { get; set; }
        public Guid CountryId { get; set; }
    }
}
