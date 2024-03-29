﻿using System.Collections.Generic;
using nKanban.Domain;

namespace nKanban.Services
{
    public interface IUserService : IService
    {
        bool IsEmailAddressUnique(string email);
        IEnumerable<ServiceError> CreateUser(User user, string password, Organization organization);
        IEnumerable<ServiceError> VerifyLogin(string username, string password);
        User GetUser(string username);
    }
}
