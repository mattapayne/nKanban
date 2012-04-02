using System;

namespace nKanban.Domain
{
    public class KanbanBoard : AbstractDomainObject
    {
        public string Name { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
