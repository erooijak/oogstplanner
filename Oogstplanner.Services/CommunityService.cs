using System.Collections.Generic;

using Oogstplanner.Data;
using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public class CommunityService : ServiceBase, ICommunityService
    {
        public CommunityService(IOogstplannerUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public User GetUser(int id)
        {
            return UnitOfWork.Users.GetById(id);
        }

        public User GetUserByName(string name)
        {
            return UnitOfWork.Users.GetUserByUserName(name);
        }

        public IEnumerable<User> SearchUsers(string searchTerm)
        {
            return UnitOfWork.Users.SearchUsers(searchTerm);
        }

        public IEnumerable<User> GetRecentlyActiveUsers(int count)
        {
            return UnitOfWork.Users.GetRecentlyActiveUsers(count);
        }
    }
}
