using System;
using System.Web;
using System.Web.Security;

using Zk.Models;
using Zk.Repositories;

namespace Zk.Services
{
    public class UserService : IUserService
    {
        readonly Repository repository;

        public UserService(Repository repository)
        {
            this.repository = repository;
        }

        public void AddUser(string userName, string fullName, string email)
        {
            var user = new User 
            {
                Name = userName,
                FullName = fullName,
                Email = email,
                Enabled = true
            };
            repository.AddUser(user);
            Roles.AddUserToRole(userName, "user");

            // Get the actual user from the database, so we get the created UserId.
            var newlyCreatedUser = repository.GetUserByUserName(userName);

            // Create calendar for the user
            repository.CreateCalendar(newlyCreatedUser);
        }

        public int GetCurrentUserId()
        {
            // Note: HttpContext.Current.User.Identity.Name returns Username locally, and e-mail address on Debian.
            //       Has to be investigated. For now the quick fix below. :/

            var currentUserEmailOrName = HttpContext.Current.User.Identity.Name;

            int currentUserId = currentUserEmailOrName.Contains("@")
                ? repository.GetUserIdByEmail(currentUserEmailOrName)
                : repository.GetUserIdByName(currentUserEmailOrName);
          
            return currentUserId;
        }

        public User GetUser(int id)
        {
            return repository.GetUserById(id);
        }
            
    }
}