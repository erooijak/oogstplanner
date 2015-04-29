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
        readonly CookieProvider cookieProvider;

        public UserService(Repository repository, CookieProvider cookieProvider)
        {
            this.repository = repository;
            this.cookieProvider = cookieProvider;
        }

        public void AddUser(string userName, string fullName, string email)
        {

            // Update if already exists:
            var clientUserName = cookieProvider.GetCookie("anonymousUserKey");
            if (!string.IsNullOrEmpty(clientUserName))
            {
                try
                {
                    var existingAnonymousUser = repository.GetUserByUserName(clientUserName);
                    existingAnonymousUser.Name = userName;
                    existingAnonymousUser.FullName = fullName;
                    existingAnonymousUser.Email = email;
                    existingAnonymousUser.AuthenticationStatus = AuthenticatedStatus.Authenticated;

                    repository.Update(existingAnonymousUser);
                    repository.SaveChanges();
                }
                catch (ArgumentException ex)
                {
                    // User does not exist. 
                    // TODO: Implement logging.
                }
                   
            }
            else // create new user it completely new and no actions performed yet.
            {
                var newUser = new User
                    {
                        Name = userName,
                        FullName = fullName,
                        Email = email,
                        AuthenticationStatus = AuthenticatedStatus.Authenticated, // by definition
                        CreationDate = DateTime.Now
                    };

                repository.AddUser(newUser);
                Roles.AddUserToRole(userName, "user");

                // Get the actual user from the database, so we get the created UserId.
                var newlyCreatedUser = repository.GetUserByUserName(userName);

                // Create calendar for the user
                repository.CreateCalendar(newlyCreatedUser);
            }
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