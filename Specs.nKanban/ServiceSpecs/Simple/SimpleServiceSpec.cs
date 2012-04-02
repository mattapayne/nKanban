using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FakeItEasy;
using Machine.Specifications;
using nKanban.Domain;
using nKanban.Persistence;
using nKanban.Services.Impl;

namespace Specs.nKanban.ServiceSpecs.Simple
{
    [Subject(typeof(SimpleService))]
    public class context_for_service
    {
        protected static SimpleService service;
        protected static IRepository repo;

        Establish context = () =>
        {
            repo = A.Fake<IRepository>();
            service = new SimpleService(repo);
        };
    }

    [Subject(typeof(SimpleService))]
    public class construction_without_dependencies
    {
        static Exception exception;

        Because of = () => { exception = Catch.Exception(() => { new SimpleService(null); }); };

        It should_throw_an_exception = () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(SimpleService))]
    public class  when_bulk_inserting : context_for_service
    {
        private static Country[] countries;
        private static bool success;

        private Establish ctx = () => countries = TestUtilities.GetCountries().ToArray();

        private Because of = () => success = service.BulkInsert(countries);

        private It should_delegate_call_to_repository = () => A.CallTo(() => repo.BulkInsert(countries)).MustHaveHappened(Repeated.Exactly.Once);
    }

    [Subject(typeof (SimpleService))]
    public class when_bulk_inserting_empty_collection : context_for_service
    {
        private static Country[] countries;
        private static bool success;

        private Establish ctx = () => countries = new Country[0];

        private Because of = () => success = service.BulkInsert(countries);

        private It should_not_delegate_call_to_repository = () => A.CallTo(() => repo.BulkInsert(countries)).MustNotHaveHappened();
    }

    [Subject(typeof(SimpleService))]
    public class when_inserting : context_for_service
    {
        private static Country country;
        private static bool success;

        private Establish ctx = () => country = TestUtilities.GetCountries().First();

        private Because of = () => success = service.Insert(country);

        private It should_delegate_call_to_repository = () => A.CallTo(() => repo.Insert(country)).MustHaveHappened(Repeated.Exactly.Once);
    }

    [Subject(typeof(SimpleService))]
    public class when_getting_all : context_for_service
    {
        private static IEnumerable<Country> countries;

        private Establish ctx = () =>
                                    {
                                        countries = TestUtilities.GetCountries();
                                        A.CallTo(() => repo.Query(A<Expression<Func<Country, bool>>[]>.Ignored)).Returns(countries);
                                    };

        private Because of = () => countries  = service.GetAll<Country>();

        private It should_delegate_call_to_repository = () => A.CallTo(() => repo.Query(A<Expression<Func<Country, bool>>[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
    }

    [Subject(typeof(SimpleService))]
    public class when_getting_one : context_for_service
    {
        private static Country country;

        private Establish ctx = () =>
                                    {
                                        country = TestUtilities.GetCountries().First();
                                        A.CallTo(() => repo.Query(A<Expression<Func<Country, bool>>[]>.Ignored)).Returns(new[] { country });
                                    };
        private Because of = () => country = service.Get<Country>(Guid.NewGuid());

        private It should_delegate_call_to_repository = () => A.CallTo(() => repo.Query(A<Expression<Func<Country, bool>>[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
    }
}
