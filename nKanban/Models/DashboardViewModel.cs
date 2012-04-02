using System.Collections.Generic;
using System.Linq;

namespace nKanban.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<KanbanBoardViewModel> KanbanBoards { get; set; }

        public DashboardViewModel()
        {
            KanbanBoards = Enumerable.Empty<KanbanBoardViewModel>();
        }
    }
}