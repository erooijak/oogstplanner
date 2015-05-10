using System;
using System.Configuration;
using System.Web;
using System.Web.Security;

using Oogstplanner.Models;
using Oogstplanner.Repositories;

namespace Oogstplanner.Services
{
    public class UserService : IUserService
    {
        readonly IUserRepository userRepository;
        readonly ICalendarRepository calendarRepository;
        readonly ICookieProvider cookieProvider;

        public UserService(
            IUserRepository userRepository, 
            ICalendarRepository calendarRepository,
            ICookieProvider cookieProvider)
        {
            this.userRepository = userRepository;
            this.calendarRepository = calendarRepository;
            this.cookieProvider = cookieProvider;
        }

        string anonymousUserCookieKey;
        protected string AnonymousUserCookieKey
        {
            get
            {
                if (anonymousUserCookieKey == null)
                {
                    anonymousUserCookieKey = ConfigurationManager.AppSettings["AnonymousUserCookieKey"];
                }
                return anonymousUserCookieKey;
            }
        }

        public void AddUser(string userName, string fullName, string email)
        {

            // Update if already exists:
            var clientUserName = cookieProvider.GetCookie(AnonymousUserCookieKey);
            if (!string.IsNullOrEmpty(clientUserName))
            {
                try
                {
                    var existingAnonymousUser = userRepository.GetUserByUserName(clientUserName);
                    existingAnonymousUser.Name = userName;
                    existingAnonymousUser.FullName = fullName;
                    existingAnonymousUser.Email = email;
                    existingAnonymousUser.AuthenticationStatus = AuthenticatedStatus.Authenticated;

                    cookieProvider.RemoveCookie(AnonymousUserCookieKey);

                    userRepository.Update(existingAnonymousUser);
                    userRepository.SaveChanges();
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

                userRepository.AddUser(newUser);
                Roles.AddUserToRole(userName, "user");

                // Get the actual user from the database, so we get the created UserId.
                var newlyCreatedUser = userRepository.GetUserByUserName(userName);

                // Create calendar for the user
                calendarRepository.CreateCalendar(newlyCreatedUser);
            }
        }

        public int GetCurrentUserId()
        {
            // Note: HttpContext.Current.User.Identity.Name returns Username locally, and e-mail address on Debian.
            //       Has to be investigated. For now the quick fix below. :/

            var currentUserEmailOrName = HttpContext.Current.User.Identity.Name;

            int currentUserId = currentUserEmailOrName.Contains("@")
                ? userRepository.GetUserIdByEmail(currentUserEmailOrName)
                : userRepository.GetUserIdByName(currentUserEmailOrName);
          
            return currentUserId;
        }

        public User GetUser(int id)
        {
            return userRepository.GetUserById(id);
        }
            
    }
}
