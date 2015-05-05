using System;
using System.Linq;

using Oogstplanner.Models;

namespace Oogstplanner.Repositories
{
    public class UserRepository : RepositoryBase
    {
        public UserRepository(IOogstplannerContext db) 
            : base(db)
        {
        }

        public void AddUser(User user)
        {
            // Note: it is not necessary to check if user profile already exists since membership provider
            //       does this for us.

            db.Users.Add(user);
            db.SaveChanges();

        }

        public User GetUserById(int id)
        {
            return db.Users.Find(id);
        }

        public User GetUserByUserName(string name)
        {
            var user = db.Users.SingleOrDefault(u => u.Name == name);

            if (user == null)
                throw new ArgumentException("The user with the specified name does not exist.");

            return user; 
        }

        public int GetUserIdByEmail(string email)
        {
            var user = db.Users.SingleOrDefault(u => u.Email == email);

            if (user == null)
                throw new ArgumentException("The user with the specified email does not exist.");

            return user.UserId; 
        }

        public int GetUserIdByName(string name)
        {
            var user = db.Users.SingleOrDefault(u => u.Name == name);

            if (user == null)
                throw new ArgumentException("The user with the specified name does not exist.");

            return user.UserId; 
        }
    }
}
    