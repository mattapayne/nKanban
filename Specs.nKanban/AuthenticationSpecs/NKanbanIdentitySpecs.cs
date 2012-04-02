using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Security;
using Machine.Specifications;
using nKanban.Domain;
using nKanban.Shared;

namespace Specs.nKanban.AuthenticationSpecs
{
    [Subject(typeof(NKanbanIdentity))]
    public class context_for_identity
    {
        protected static User user;
        protected static string userData;

        private Establish context = () =>
                                        {
                                            user = new User { Email = "test@test.ca", FirstName = "First", LastName = "Last", Id = Guid.NewGuid(), Roles = new[] {"Owner", "Operator"}};
                                            var serializer = new JavaScriptSerializer();
                                            userData = serializer.Serialize(user.ToJson());
                                        };
    }

    [Subject(typeof(NKanbanIdentity))]
    public class when_constructed_from_values
    {
        private static NKanbanIdentity identity;
        private static readonly Guid id = new Guid("e66530da-63bd-4163-b177-9bf43757aaa9");

        private Establish ctx = () =>
        {
            identity = new NKanbanIdentity(id, "First Last", "test@test.ca", new[] {"Owner"}); 
        };

        private It should_be_authenticated = () => identity.IsAuthenticated.ShouldBeTrue();

        private It should_have_an_email = () => identity.Email.ShouldContain("test@test.ca");

        private It should_have_a_fullname = () => identity.Name.ShouldContain("First Last");

        private It should_have_an_id = () => identity.Id.ShouldEqual(id);

        private It should_have_auth_type = () => identity.AuthenticationType.ShouldNotBeEmpty();
    }

    [Subject(typeof(NKanbanIdentity))]
    public class when_constructed_from_a_forms_authentication_ticket : context_for_identity
    {
        private static FormsAuthenticationTicket ticket;
        private static NKanbanIdentity identity;

        private Establish ctx = () =>
                                    {
                                        ticket = new FormsAuthenticationTicket(1, user.Id.ToString(), DateTime.Now, DateTime.Now.AddMinutes(30), false, userData, String.Empty);
                                    };

        private Because of = () => { identity = new NKanbanIdentity(new StubFormsIdentity(ticket));  };

        private It should_be_authenticated = () => identity.IsAuthenticated.ShouldBeTrue();

        private It should_have_an_email = () => identity.Email.ShouldContain(user.Email);

        private It should_have_a_fullname = () => identity.Name.ShouldContain(String.Format("{0} {1}", user.FirstName, user.LastName));

        private It should_have_an_id = () => identity.Id.ShouldEqual(user.Id.Value);

        private It should_have_auth_type = () => identity.AuthenticationType.ShouldNotBeEmpty();
    }

    [Subject(typeof(NKanbanIdentity))]
    public class when_checking_for_role_membership : context_for_identity
    {
        private static FormsAuthenticationTicket ticket;
        private static NKanbanIdentity identity;

        private Establish ctx = () =>
        {
            ticket = new FormsAuthenticationTicket(1, user.Id.ToString(), DateTime.Now, DateTime.Now.AddMinutes(30), false, userData, String.Empty);
        };

        private Because of = () => { identity = new NKanbanIdentity(new StubFormsIdentity(ticket)); };

        private It should_be_in_a_role_that_exists = () => identity.IsInRole("Owner").ShouldBeTrue();

        private It should_not_be_in_a_non_existant_role = () => identity.IsInRole("Tester").ShouldBeFalse();
    }
}
