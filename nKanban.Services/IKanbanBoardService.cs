using System;
using System.Collections.Generic;
using nKanban.Domain;

namespace nKanban.Services
{
    public interface IKanbanBoardService : IService
    {
        IEnumerable<KanbanBoard> GetKanbanBoardsByUser(Guid userId);
        IEnumerable<ServiceError> CreateKanbanBoard(Guid userId, KanbanBoard board);
    }
}
