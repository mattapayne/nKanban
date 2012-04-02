using System;
using System.Collections.Generic;
using System.Linq;
using nKanban.Domain;
using nKanban.Persistence;

namespace nKanban.Services.Impl
{
    public class KanbanBoardService : AbstractBaseService, IKanbanBoardService
    {
        public KanbanBoardService(IRepository repository) 
            : base(repository)
        {
        }

        public IEnumerable<KanbanBoard> GetKanbanBoardsByUser(Guid userId)
        {
            return Repository.Query<KanbanBoard>(board => board.CreatedBy == userId).ToList();
        }

        public IEnumerable<ServiceError> CreateKanbanBoard(Guid userId, KanbanBoard board)
        {
            var errors = new List<ServiceError>();

            board.CreatedBy = userId;
            board.DateCreated = DateTime.UtcNow;

            if(!Repository.Insert<KanbanBoard>(board))
            {
                errors.Add(new ServiceError(String.Empty, "An unknown error occurred while saving the Kanban board."));
            }

            return errors;
        }
    }
}
