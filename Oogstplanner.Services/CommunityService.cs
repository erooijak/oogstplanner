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
            throw new System.NotImplementedException();
        }

        public IEnumerable<User> GetRecentlyActiveUsers(int count)
        {
            throw new System.NotImplementedException();
        }
    }
}
