using System.Collections.Generic;

using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByUserName(string name);
        IEnumerable<User> SearchUsers(string searchTerm);
        IEnumerable<User> GetRecentlyActiveUsers(int count);
        int GetUserIdByEmail(string email);
        int GetUserIdByName(string name);
    }
}
