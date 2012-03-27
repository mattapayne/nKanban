using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using nKanban.Controllers;
using nKanban;
using nKanban.Services;
using nKanban.Shared;
using FakeItEasy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Specs.nKanban.ControllerSpecs.AbstractBase
{
    [Subject(typeof(AbstractBaseController))]
    public class context_for_controller
    {
        protected static StubController controller;

        Establish context = () => { controller = new StubController(); };
    }

    [Subject(typeof(AbstractBaseController))]
    public class adding_null_service_errors : context_for_controller
    {
        Because of = () => { controller.AddServiceErrorsPub(null); };

        It should_have_no_modelstate_errors = () => { controller.ModelState.ShouldBeEmpty(); };
    }

    [Subject(typeof(AbstractBaseController))]
    public class adding_service_errors : context_for_controller
    {
        static IEnumerable<ServiceError> errors;

        Establish ctx = () => { errors = new List<ServiceError>() { new ServiceError("PropertyName", "ErrorMessage") }; };

        Because of = () => { controller.AddServiceErrorsPub(errors); };

        It should_have_modelstate_errors = () => { controller.ModelState.ShouldNotBeEmpty(); };
    }

    [Subject(typeof(AbstractBaseController))]
    public class setting_empty_redirect_message : context_for_controller
    {
        Because of = () => { controller.SetRedirectMessagePub(MessageType.Success, String.Empty); };

        It should_have_no_messages_in_tempdata = () => { controller.TempData.ShouldBeEmpty(); };
    }

    [Subject(typeof(AbstractBaseController))]
    public class setting_redirect_message : context_for_controller
    {
        Because of = () => { controller.SetRedirectMessagePub(MessageType.Success, "This is an error"); };

        It should_have_no_messages_in_tempdata = () => { controller.TempData.ShouldNotBeEmpty(); };

        It should_use_the_message_type_as_the_key = () => { controller.TempData.Keys.First().ShouldEqual(MessageType.Success.ToString()); };
    }

    [Subject(typeof(AbstractBaseController))]
    public class getting_current_user_when_authenticated : context_for_controller
    {
        static nKanbanPrincipal user;
        static bool loggedin;

        Establish ctx = () => 
        {
            user = new nKanbanPrincipal(new nKanbanIdentity(Guid.NewGuid(), "Test User", "test@test.ca", null));
            TestUtilities.StubLoggedInStatus(controller, user, true);
        };

        Because of = () => 
        { 
            user = controller.CurrentUserPub;
            loggedin = controller.IsLoggedInPub;
        };

        It it_should_return_the_logged_in_user = () => { user.ShouldNotBeNull(); };

        It should_be_the_correct_user = () => { user.Name.ShouldEqual("Test User"); };

        It should_be_logged_in = () => { loggedin.ShouldBeTrue(); };
    }

    [Subject(typeof(AbstractBaseController))]
    public class getting_current_user_when_not_authenticated : context_for_controller
    {
        static nKanbanPrincipal user;
        static bool loggedin;

        Establish ctx = () =>
        {
            TestUtilities.StubLoggedInStatus(controller, null, false);
        };

        Because of = () =>
        {
            user = controller.CurrentUserPub;
            loggedin = controller.IsLoggedInPub;
        };

        It it_should_return_no_user = () => { user.ShouldBeNull(); };

        It should_not_be_logged_in = () => { loggedin.ShouldBeFalse(); };
    }
}
