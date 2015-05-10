using Oogstplanner.Models;

namespace Oogstplanner.Repositories
{
    public interface IUserRepository : IRepositoryBase
    {
        void AddUser(User user);

        User GetUserById(int id);

        User GetUserByUserName(string name);

        int GetUserIdByEmail(string email);

        int GetUserIdByName(string name);
    }
}
    