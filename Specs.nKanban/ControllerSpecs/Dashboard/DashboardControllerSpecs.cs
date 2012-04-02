using System;
using System.Linq;
using Machine.Specifications;
using nKanban.Controllers;
using System.Web.Mvc;
using Machine.Specifications.Mvc;
using nKanban.Models;
using nKanban.Services;
using FakeItEasy;
using nKanban.Shared;

namespace Specs.nKanban.ControllerSpecs.Dashboard
{
    [Subject(typeof(DashboardController))]
    public class context_for_controller
    {
        protected static IKanbanBoardService kanbanBoardService;
        protected static IUserService userService;
        protected static DashboardController controller;
        protected static NKanbanPrincipal user;

        Establish context = () => {
            
            user = new NKanbanPrincipal(new NKanbanIdentity(Guid.NewGuid(), "Test User", "test@test.ca", Enumerable.Empty<string>()));
            userService = A.Fake<IUserService>();
            kanbanBoardService = A.Fake<IKanbanBoardService>();
            controller = new DashboardController(userService, kanbanBoardService);
            //stub out the call
            A.CallTo(() => kanbanBoardService.GetKanbanBoardsByUser(user.Id)).Returns(TestUtilities.GetKanbanBoards());

            //stub out a logged in user
            TestUtilities.StubLoggedInStatus(controller, user);
        };
    }

    [Subject(typeof(DashboardController))]
    public class construction_without_user_service : context_for_controller
    {
        static Exception exception;

        Because of = () => { exception = Catch.Exception(() => { new DashboardController(null, kanbanBoardService); }); };

        It should_throw_an_exception = () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(DashboardController))]
    public class construction_without_kanbanboard_service : context_for_controller
    {
        static Exception exception;

        Because of = () => { exception = Catch.Exception(() => { new DashboardController(userService, null); }); };

        It should_throw_an_exception = () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(DashboardController), ": when I go to the Dashboard page")]
    public class the_response : context_for_controller
    {
        static ActionResult result;

        Because of = () => { result = controller.Show(); };

        It should_show_the_dashboard_view = () => result.ShouldBeAView();
    }

    [Subject(typeof(DashboardController))]
    public class when_viewing_the_dashboard : context_for_controller
    {
        private static ViewResult result;

        private Because of = () => result = controller.Show() as ViewResult;

        private It should_ask_the_kanbanboard_service_to_get_the_users_boards = () => A.CallTo(() => kanbanBoardService.GetKanbanBoardsByUser(user.Id)).MustHaveHappened(Repeated.Exactly.Once);

        private It should_have_a_kanbanboardviewmodel = () => result.Model<DashboardViewModel>().ShouldNotBeNull();

        private It should_have_a_list_of_kanban_boards_for_the_user = () => result.Model<DashboardViewModel>().KanbanBoards.ShouldNotBeNull();
    }
}
