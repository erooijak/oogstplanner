using System;

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
            
        public User CurrentAnonymousUser 
        { 
            get 
            { 
                string guid;
                var guidFromCookie = cookieProvider.GetCookie(anonymousUserCookieKey);

                if (string.IsNullOrEmpty(guidFromCookie))
                {
                    guid = Guid.NewGuid().ToString();
                    AddUser(guid, "Anonymous", null);
                    cookieProvider.SetCookie(anonymousUserCookieKey, guid, anonymousUserCookieExpiration);
                }
                else
                {
                    guid = guidFromCookie;
                }

                return repository.GetUserByUserName(guid);
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
    