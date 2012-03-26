using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nKanban.Domain;
using nKanban.Persistence;
using System.Security.Cryptography;
using System.Web.Security;
using System.Web;
using System.Web.Script.Serialization;

namespace nKanban.Services.Impl
{
    public class UserService : AbstractBaseService, IUserService
    {
        public UserService(IRepository repository)
            : base(repository)
        {

        }

        public bool IsEmailAddressUnique(string email)
        {
            bool exists = Repository.Query<User>(u => u.Email == email).Any();
            return !exists;
        }

        public IEnumerable<ServiceError> CreateUser(User user, string password)
        {
            var errors = new List<ServiceError>();

            if (user == null)
            {
                errors.Add(new ServiceError(String.Empty, "User cannot be null."));
            }

            if (String.IsNullOrEmpty(password))
            {
                errors.Add(new ServiceError("Password", "Password cannot be empty."));
            }

            if (user != null && !IsEmailAddressUnique(user.Email))
            {
                errors.Add(new ServiceError("Email", "Sorry, that username is taken."));
            }

            if (!errors.Any())
            {
                user.PasswordSalt = GenerateSalt();
                user.PasswordHash = HashPassword(password, user.PasswordSalt);
                user.DateCreated = DateTime.UtcNow;

                Repository.Insert<User>(user);
            }

            return errors;
        }

        public IEnumerable<ServiceError> VerifyLogin(string username, string password)
        {
            var errors = new List<ServiceError>();

            var user = GetUser(username);

            if (user == null)
            {
                errors.Add(new ServiceError("UserName", "Invalid username or password."));
            }
            else
            {
                var hashedPassword = HashPassword(password, user.PasswordSalt);

                if (hashedPassword != user.PasswordHash)
                {
                    errors.Add(new ServiceError("UserName", "Invalid username or password."));
                }
            }

            return errors;
        }

        public User GetUser(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                return null;
            }

            return Repository.Query<User>(u => u.Email == username).FirstOrDefault();
        }

        protected override string CollectionName
        {
            get { return "Users"; }
        }

        private string GenerateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[20];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        private string HashPassword(string password, string salt)
        {
            var concatenatedPasswordAndSalt = String.Format("{0}{1}", password, salt);
            return FormsAuthentication.HashPasswordForStoringInConfigFile(concatenatedPasswordAndSalt, "MD5");
        }
    }
}
