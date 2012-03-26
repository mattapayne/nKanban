using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nKanban.Domain;

namespace nKanban.Services
{
    public interface IUserService : IService
    {
        bool IsEmailAddressUnique(string email);
        IEnumerable<ServiceError> CreateUser(User user, string password);
        IEnumerable<ServiceError> VerifyLogin(string username, string password);
        User GetUser(string username);
    }
}
