using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using Machine.Specifications;
using nKanban.Shared;

namespace Specs.nKanban.AuthenticationSpecs
{
    [Subject(typeof(NKanbanPrincipal))]
    public class context_for_principal
    {
        protected static NKanbanIdentity identity;
        protected static NKanbanPrincipal principal;

        private Establish context = () =>
                                        {
                                            identity = new NKanbanIdentity(Guid.NewGuid(), "Test Test", "test@test.ca", new[] {"Owner"});
                                            principal = new NKanbanPrincipal(identity);
                                        };
    }

    [Subject(typeof(NKanbanPrincipal))]
    public class when_constructed_without_dependencies : context_for_principal
    {
        private static Exception exception;

        private Because of = () => exception = Catch.Exception(() => new NKanbanPrincipal(null));

        private It should_throw_an_exception = () => exception.ShouldNotBeNull();
    }
}
