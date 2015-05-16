using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByUserName(string name);
        int GetUserIdByEmail(string email);
        int GetUserIdByName(string name);
    }
}
