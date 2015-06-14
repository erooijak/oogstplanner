using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public interface ICommunityService
    {
        User GetUser(int id);
        User GetUserByName(string name);
    }
}
