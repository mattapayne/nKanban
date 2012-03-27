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
using nKanban.Services;
using System.Web;

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
    public class when_verifying_login_for_non_existent_user : context_for_service
    {
        static IEnumerable<ServiceError> errors;

        Establish ctx = () => {
            A.CallTo(() => repo.Query<User>(A<Expression<Func<User, bool>>[]>.Ignored)).Returns(new List<User>());
        };

        Because of = () => { errors = service.VerifyLogin("test@test.ca", "password"); };

        It should_ask_the_repository_to_get_the_user = () => { A.CallTo(() => repo.Query<User>(A<Expression<Func<User, bool>>[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once); };

        It should_have_errors = () => { errors.ShouldNotBeEmpty(); };
    }

    [Subject(typeof(UserService))]
    public class when_verifying_login_for_existing_user_with_bad_password : context_for_service
    {
        static IEnumerable<ServiceError> errors;
        static User user;

        Establish ctx = () =>
        {
            user = new User() { PasswordSalt = "abc123", PasswordHash = "passhash" };
            var users = new[] { user };
            A.CallTo(() => repo.Query<User>(A<Expression<Func<User, bool>>[]>.Ignored)).Returns(users);
        };

        Because of = () => { errors = service.VerifyLogin("test@test.ca", "password"); };

        It should_ask_the_repository_to_get_the_user = () => { A.CallTo(() => repo.Query<User>(A<Expression<Func<User, bool>>[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once); };

        It should_have_errors = () => { errors.ShouldNotBeEmpty(); };
    }

    [Subject(typeof(UserService))]
    public class when_verifying_login_for_existing_user_with_correct_password : context_for_service
    {
        static string passwordHash = "E6FF7030C25AF1EF0BB6D8EB5D1D9391";
        static string passwordSalt = "OWUu3Zy6/ZcKITCFYNz+ty1wh7k=";

        static IEnumerable<ServiceError> errors;
        static User user;

        Establish ctx = () =>
        {
            user = new User() { PasswordSalt = passwordSalt, PasswordHash = passwordHash };
            var users = new[] { user };
            A.CallTo(() => repo.Query<User>(A<Expression<Func<User, bool>>[]>.Ignored)).Returns(users);
        };

        Because of = () => { errors = service.VerifyLogin("test@test.ca", "232423"); };

        It should_ask_the_repository_to_get_the_user = () => { A.CallTo(() => repo.Query<User>(A<Expression<Func<User, bool>>[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once); };

        It should_not_have_errors = () => { errors.ShouldBeEmpty(); };
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

        Because of = () => { service.CreateUser(user, password, null); };

        It should_check_that_the_users_email_is_unique = () => { A.CallTo(() => repo.Query<User>(A<Expression<Func<User, bool>>[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once); };

        It should_set_the_users_password_salt = () => { user.PasswordSalt.ShouldNotBeEmpty(); };

        It should_set_the_users_hashed_password = () => { user.PasswordHash.ShouldNotBeEmpty(); };

        It should_set_the_users_date_created = () => { user.DateCreated.ShouldBeCloseTo(DateTime.UtcNow, new TimeSpan(0, 1, 0)); };

        It should_ask_the_repository_to_save_the_user = () => { A.CallTo(() => repo.Insert<User>(user)).MustHaveHappened(Repeated.Exactly.Once); };
    }

    [Subject(typeof(UserService))]
    public class when_creating_user_with_null_user : context_for_service
    {
        static IEnumerable<ServiceError> errors;

        Because of = () => { errors = service.CreateUser(null, "password", null); };

        It should_not_delegate_to_the_repository = () => { A.CallTo(() => repo.Insert<User>(A<User>.Ignored)).MustNotHaveHappened(); };

        It should_have_errors = () => {errors.ShouldNotBeEmpty(); };
    }

    [Subject(typeof(UserService))]
    public class when_creating_user_with_a_blank_password : context_for_service
    {
        static IEnumerable<ServiceError> errors;

        Because of = () => { errors = service.CreateUser(new User(), String.Empty, null); };

        It should_not_delegate_to_the_repository = () => { A.CallTo(() => repo.Insert<User>(A<User>.Ignored)).MustNotHaveHappened(); };

        It should_have_errors = () => { errors.ShouldNotBeEmpty(); };
    }

    [Subject(typeof(UserService))]
    public class when_creating_user_with_non_unique_email : context_for_service
    {
        static IEnumerable<ServiceError> errors;

        Establish ctx = () => {
            var users = new List<User>() { new User() { Email = "test@test.ca"} };
            A.CallTo(() => repo.Query<User>(A<Expression<Func<User, bool>>[]>.Ignored)).Returns(users);
        };

        Because of = () => { errors = service.CreateUser(new User() { Email= "test@test.ca" }, "password", null); };

        It should_not_delegate_to_the_repository = () => { A.CallTo(() => repo.Insert<User>(A<User>.Ignored)).MustNotHaveHappened(); };

        It should_have_errors = () => { errors.ShouldNotBeEmpty(); };
    }

    [Subject(typeof(UserService))]
    public class when_getting_user_by_username_and_username_is_blank : context_for_service
    {
        static User user;

        Because of = () => { user = service.GetUser(String.Empty); };

        It should_not_delegate_to_the_repository = () => { A.CallTo(() => repo.Query<User>(A<Expression<Func<User, bool>>[]>.Ignored)).MustNotHaveHappened(); };

        It should_return_null = () => { user.ShouldBeNull(); };
    }

    [Subject(typeof(UserService))]
    public class when_getting_user_by_username : context_for_service
    {
        static User user;

        Because of = () => { user = service.GetUser("joe@test.ca"); };

        It should_delegate_to_the_repository = () => { A.CallTo(() => repo.Query<User>(A<Expression<Func<User, bool>>[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once); };
    }
}
