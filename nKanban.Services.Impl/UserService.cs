﻿using System;
using System.Collections.Generic;
using System.Linq;
using nKanban.Domain;
using nKanban.Persistence;
using System.Security.Cryptography;
using System.Web.Security;

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

        public IEnumerable<ServiceError> CreateUser(User user, string password, Organization organization)
        {
            var errors = new List<ServiceError>();

            if (user == null)
            {
                errors.Add(new ServiceError(String.Empty, "User cannot be null."));
                return errors;
            }

            if (String.IsNullOrEmpty(password))
            {
                errors.Add(new ServiceError("Password", "Password cannot be empty."));
            }

            if (!IsEmailAddressUnique(user.Email))
            {
                errors.Add(new ServiceError("Email", "Sorry, that username is taken."));
            }

            if (organization != null && !String.IsNullOrEmpty(organization.Name))
            {
                if (OrganizationExists(organization.Name))
                {
                    errors.Add(new ServiceError("OrganizationName", "Sorry, an organization by that name has already registered."));
                }
            }

            if (!errors.Any())
            {
                if (organization != null && !String.IsNullOrEmpty(organization.Name))
                {
                    //save the organization
                    if (Repository.Insert<Organization>(organization))
                    {
                        user.OrganizationId = organization.Id;
                    }
                    else
                    {
                        errors.Add(new ServiceError(String.Empty, "Sorry, an error occurred while saving the organization."));
                    }
                }

                if (!errors.Any())
                {
                    user.PasswordSalt = GenerateSalt();
                    user.PasswordHash = HashPassword(password, user.PasswordSalt);
                    user.DateCreated = DateTime.UtcNow;

                    Repository.Insert<User>(user);
                }
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

        private string GenerateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[20];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        private string HashPassword(string password, string salt)
        {
            var concatenatedPasswordAndSalt = String.Format("{0}{1}", password, salt);
            return FormsAuthentication.HashPasswordForStoringInConfigFile(concatenatedPasswordAndSalt, "MD5");
        }

        private bool OrganizationExists(string organizationName)
        {
            if (String.IsNullOrEmpty(organizationName))
            {
                return false;
            }

            return Repository.Query<Organization>(o => o.Name.ToLower() == organizationName.ToLower()).Any();
        }
    }
}
