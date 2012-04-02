using System;
using System.Linq;
using System.Web.Mvc;
using FakeItEasy;
using Machine.Specifications;
using Machine.Specifications.Mvc;
using nKanban.Controllers;
using nKanban.Domain;
using nKanban.Infrastructure;
using nKanban.Models;
using nKanban.Services;
using nKanban.Shared;

namespace Specs.nKanban.ControllerSpecs.KanbanBrd
{
    [Subject(typeof(KanbanBoardController))]
    public class context_for_controller
    {
        protected static KanbanBoardController controller;
        protected static IKanbanBoardService service;
        protected static NKanbanPrincipal user;

        private Establish context = () =>
                                        {
                                            AppBootstrapper.Bootstrap();
                                            user = new NKanbanPrincipal(new NKanbanIdentity(Guid.NewGuid(), "Test User", "test@test.ca", Enumerable.Empty<string>()));
                                            service = A.Fake<IKanbanBoardService>();
                                            controller = new KanbanBoardController(service);

                                            TestUtilities.StubLoggedInStatus(controller, user);
                                        };
    }

    [Subject(typeof(KanbanBoardController))]
    public class when_constructed_without_dependencies
    {
        private static Exception exception;

        private Because of = () => exception = Catch.Exception(() => new KanbanBoardController(null));

        private It should_throw_an_exception = () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(KanbanBoardController))]
    public class when_accessing_the_new_view : context_for_controller
    {
        private static ViewResult result;

        private Because of = () => result = controller.New() as ViewResult;

        private It should_be_a_view = () => result.ShouldBeAView();

        private It should_have_a_proper_view_model = () => result.ShouldHaveModelOfType<CreateKanbanBoardViewModel>();
    }

    [Subject(typeof(KanbanBoardController))]
    public class when_creating_a_new_kanban_board_with_no_name : context_for_controller
    {
        private static ViewResult result;
        private static CreateKanbanBoardViewModel model;

        private Establish ctx = () =>
                                    {
                                        controller.ModelState.AddModelError("Error", "Error");
                                        model = new CreateKanbanBoardViewModel();
                                    };

        private Because of = () => result = controller.Create(model) as ViewResult;

        private It should_rerender_the_new_view = () => result.ShouldBeAView();

        private It should_have_the_original_view_model = () => result.Model.ShouldBeTheSameAs(model);
    }

    [Subject(typeof(KanbanBoardController))]
    public class when_creating_a_new_kanban_board_and_service_has_errors : context_for_controller
    {
        private static ViewResult result;
        private static CreateKanbanBoardViewModel model;

        private Establish ctx = () =>
        {
            model = new CreateKanbanBoardViewModel() { Name = "test" };
            A.CallTo(() => service.CreateKanbanBoard(A<Guid>.Ignored, A<KanbanBoard>.Ignored)).Returns(new ServiceError[] { new ServiceError("Error"), });
        };

        private Because of = () => result = controller.Create(model) as ViewResult;

        private It should_rerender_the_new_view = () => result.ShouldBeAView();

        private It should_have_the_original_view_model = () => result.Model.ShouldBeTheSameAs(model);
    }

    [Subject(typeof(KanbanBoardController))]
    public class when_creating_a_new_kanban_board_successfully : context_for_controller
    {
        private static ActionResult result;
        private static CreateKanbanBoardViewModel model;

        private Establish ctx = () =>
        {
            model = new CreateKanbanBoardViewModel() { Name = "test" };
            A.CallTo(() => service.CreateKanbanBoard(A<Guid>.Ignored, A<KanbanBoard>.Ignored)).Returns(new ServiceError[0]);
        };

        private Because of = () => result = controller.Create(model);

        private It should_create_the_kanban_board = () => A.CallTo(() => service.CreateKanbanBoard(A<Guid>.Ignored, A<KanbanBoard>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

        private It should_redirect_to_the_dashboard = () => result.ShouldBeARedirectToRoute();
    }
}
