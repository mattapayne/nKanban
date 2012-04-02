using nKanban.Domain;

namespace nKanban.Services
{
    public interface ILoginService
    {
        void LoginUser(User user, bool persistent);
        void Logoff();
    }
}
