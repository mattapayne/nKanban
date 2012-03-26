using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nKanban.Domain;
using nKanban.Shared;

namespace nKanban.Services
{
    public interface ILoginService
    {
        void LoginUser(User user, bool persistent);
        void Logoff();
    }
}
