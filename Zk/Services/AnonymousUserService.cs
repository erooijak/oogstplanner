using System;
using System.Web;
using System.Web.Security;

using Zk.Models;
using Zk.Repositories;

namespace Zk.Services
{
    public class AnonymousUserService : IUserService
    {
        readonly Repository repository;
        readonly CookieProvider cookieProvider;

        const string anonymousUserCookieKey = "anonymousUserKey"; 
        const double anonymousUserCookieExpiration = 730;

        public AnonymousUserService(Repository repository, CookieProvider cookieProvider)
        {
            this.repository = repository;
            this.cookieProvider = cookieProvider;
        }

        private User currentAnonymousUser;
        public User CurrentAnonymousUser 
        { 
            get 
            { 
                if (!string.IsNullOrEmpty(cookieProvider.GetCookieValue(anonymousUserCookieKey)))
                {
                    var guid = new Guid().ToString();
                    AddUser(guid, "Anonymous", null);
                    cookieProvider.SetCookieValue(anonymousUserCookieKey, guid, anonymousUserCookieExpiration);
                }

                if (currentAnonymousUser == null)
                {
                    var userName = cookieProvider.GetCookieValue(anonymousUserCookieKey);
                    currentAnonymousUser = repository.GetUserByUserName(userName);
                }

                return currentAnonymousUser;
            }

        }

        public void AddUser(string userName, string fullName, string email)
        {
            var user = new User 
                {
                    Name = userName,
                    AuthenticationStatus = AuthenticatedStatus.Anonymous, // by definition
                    CreationDate = DateTime.Now
                };
            repository.AddUser(user);
            Roles.AddUserToRole(userName, "anonymous");

            // Get the actual user from the database, so we get the created UserId.
            var newlyCreatedUser = repository.GetUserByUserName(userName);

            // Create calendar for the anonymous user
            repository.CreateCalendar(newlyCreatedUser);
        }

        public int GetCurrentUserId()
        {
            return CurrentAnonymousUser.UserId;
        }

        public User GetUser(int id)
        {
            return repository.GetUserById(id);
        }
            
    }
}
    