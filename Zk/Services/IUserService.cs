using Zk.Models;

namespace Zk.Services
{
    public interface IUserService
    {
        void Add(string userName, string fullName, string email);

        int GetCurrentUserId();

        User GetUser(int id);
    }
}
