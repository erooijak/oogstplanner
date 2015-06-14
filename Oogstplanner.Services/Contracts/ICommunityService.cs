using System.Collections.Generic;

using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public interface ICommunityService
    {
        User GetUser(int id);
        User GetUserByName(string name);
        IEnumerable<User> SearchUsers(string searchTerm);
        IEnumerable<User> GetRecentlyActiveUsers(int count);
    }
}
