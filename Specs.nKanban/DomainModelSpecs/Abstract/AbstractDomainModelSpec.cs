using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using nKanban.Domain;

namespace Specs.nKanban.DomainModelSpecs.Abstract
{
    [Subject(typeof(AbstractDomainObject))]
    public class context_for_model
    {
        protected static StubDomainObject obj;

        Establish context = () => {
            obj = new StubDomainObject();
        };
    }

    [Subject(typeof(AbstractDomainObject))]
    public class when_new : context_for_model
    {
        It should_know_it_is_new = () => obj.IsNew.ShouldBeTrue();
    }

    [Subject(typeof(AbstractDomainObject))]
    public class when_not_new : context_for_model
    {
        Because of = () => { obj.Id = Guid.NewGuid(); };

        It should_know_it_is_not_new = () => obj.IsNew.ShouldBeFalse();
    }
}
