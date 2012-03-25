using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using nKanban.Services.Impl;
using FakeItEasy;
using nKanban.Persistence;
using nKanban.Domain;
using System.Linq.Expressions;

namespace Specs.nKanban.ServiceSpecs.UserSvc
{
    [Subject(typeof(UserService))]
    public class context_for_service
    {
        protected static UserService service;
        protected static IRepository repo;

        Establish context = () => 
        {
            repo = A.Fake<IRepository>();
            service = new UserService(repo);
        };
    }

    [Subject(typeof(UserService))]
    public class construction_without_dependencies
    {
        static Exception exception;

        Because of = () => { exception = Catch.Exception(() => { new UserService(null); }); };

        It should_throw_an_exception = () => { exception.ShouldNotBeNull(); };
    }

    [Subject(typeof(UserService))]
    public class construction_with_dependencies
    {
        protected static UserService service;
        protected static IRepository repo;

        Establish ctx = () =>
        {
            repo = A.Fake<IRepository>();
            service = new UserService(repo);
        };

        It should_set_the_collection_name_on_the_repository = () => { A.CallTo(() => repo.SetCollectionName("Users")).MustHaveHappened(Repeated.Exactly.Once); };
    }

    [Subject(typeof(UserService))]
    public class when_checking_email_unique : context_for_service
    {
        static string email = "test@test.ca";

        Because of = () => { service.IsEmailAddressUnique(email); };

        It should_delegate_to_the_repository = () => { A.CallTo(() => repo.Query<User>(A<Expression<Func<User, bool>>[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once); };
    }

    [Subject(typeof(UserService))]
    public class when_creating_user : context_for_service
    {
        static User user;
        static string password = "password";

        Establish ctx = () => { user = new User(); };

        Because of = () => { service.CreateUser(user, password); };

        It should_check_that_the_users_email_is_unique = () => { A.CallTo(() => repo.Query<User>(A<Expression<Func<User, bool>>[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once); };

        It should_set_the_users_password_salt = () => { user.PasswordSalt.ShouldNotBeEmpty(); };

        It should_set_the_users_hashed_password = () => { user.PasswordHash.ShouldNotBeEmpty(); };

        It should_set_the_users_date_created = () => { user.DateCreated.ShouldBeCloseTo(DateTime.UtcNow, new TimeSpan(0, 1, 0)); };

        It should_ask_the_repository_to_save_the_user = () => { A.CallTo(() => repo.Insert<User>(user)).MustHaveHappened(Repeated.Exactly.Once); };
    }
}
