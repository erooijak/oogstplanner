using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public interface IUserService
    {
        void AddUser(string userName, string fullName, string email);

        int GetCurrentUserId();

        User GetUser(int id);
    }
}
