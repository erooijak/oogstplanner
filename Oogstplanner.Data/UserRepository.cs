using System.Linq;
using System.Collections.Generic;

using Oogstplanner.Common;
using Oogstplanner.Models;

namespace Oogstplanner.Data
{
    public class UserRepository : EntityFrameworkRepository<User>, IUserRepository
    {
        public UserRepository(IOogstplannerContext db) 
            : base(db)
        { }
            
        public User GetUserByUserName(string name)
        {
            var user = DbSet.SingleOrDefault(u => u.Name == name);

            if (user == null)
            {
                throw new UserNotFoundException("The user with the specified name does not exist.");
            }

            return user; 
        }

        public int GetUserIdByEmail(string email)
        {
            var user = DbSet.SingleOrDefault(u => u.Email == email);

            if (user == null)
            {
                throw new UserNotFoundException("The user with the specified email does not exist.");
            }

            return user.Id; 
        }

        public int GetUserIdByName(string name)
        {
            var user = DbSet.SingleOrDefault(u => u.Name == name);

            if (user == null)
            {
                throw new UserNotFoundException("The user with the specified name does not exist.");
            }

            return user.Id; 
        }

        public IEnumerable<User> GetRecentlyActiveUsers(int count) 
        {
            return DbSet.Where(u => u.AuthenticationStatus == AuthenticatedStatus.Authenticated)
                .OrderByDescending(u => u.LastActive).Take(count).ToList();
        }

        public IEnumerable<User> SearchUsers(string searchTerm)
        {
            return DbSet.Where(u => 
                u.AuthenticationStatus == AuthenticatedStatus.Authenticated
                && (u.Name.Contains(searchTerm) || u.FullName.Contains(searchTerm)))
                    .OrderByDescending(u => u.LastActive);
        }
    }
}
