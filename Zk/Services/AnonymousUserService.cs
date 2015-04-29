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

        private string guidOnClient;

        public AnonymousUserService(Repository repository, CookieProvider cookieProvider)
        {
            this.repository = repository;
            this.cookieProvider = cookieProvider;
        }
            
        public User CurrentAnonymousUser 
        { 
            get 
            { 
                if (string.IsNullOrEmpty(GuidOnClient))
                {
                    var guid = Guid.NewGuid().ToString();
                    AddUser(guid, null, null);
                    GuidOnClient = guid;

                    return repository.GetUserByUserName(guid);
                }
                else
                {
                    return repository.GetUserByUserName(GuidOnClient);
                }

            }

        }
            
        protected string GuidOnClient
        {
            get 
            { 
                if (guidOnClient == null)
                {
                    guidOnClient = cookieProvider.GetCookie(anonymousUserCookieKey);
                }
                return guidOnClient;
            }
            set 
            { 
                cookieProvider.SetCookie(anonymousUserCookieKey, value, anonymousUserCookieExpiration); 
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
    