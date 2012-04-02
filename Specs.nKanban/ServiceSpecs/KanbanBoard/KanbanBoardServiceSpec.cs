using System;
using System.Linq.Expressions;
using FakeItEasy;
using Machine.Specifications;
using nKanban.Domain;
using nKanban.Persistence;
using nKanban.Services.Impl;

namespace Specs.nKanban.ServiceSpecs.KanbanBoardSvc
{
    [Subject(typeof(KanbanBoardService))]
    public class context_for_service
    {
        protected static KanbanBoardService service;
        protected static IRepository repo;

        Establish context = () =>
                                {
                                    repo = A.Fake<IRepository>();
                                    service = new KanbanBoardService(repo);
        };
    }

    [Subject(typeof(KanbanBoardService))]
    public class when_constructed_without_repository
    {
        static Exception exception;

        Because of = () => { exception = Catch.Exception(() => new KanbanBoardService(null)); };

        It should_throw_an_exception = () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(KanbanBoardService))]
    public class when_getting_kanban_boards_by_user : context_for_service
    {
        private Because of = () => service.GetKanbanBoardsByUser(Guid.NewGuid());
        private It should_delegate_to_the_repo = () => A.CallTo(() => repo.Query<KanbanBoard>(A<Expression<Func<KanbanBoard, bool>>[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
    }
}
