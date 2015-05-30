using System;
using System.Configuration;
using System.Web;

using Oogstplanner.Models;
using Oogstplanner.Data;

namespace Oogstplanner.Services
{
    public class UserService : ServiceBase, IUserService
    {
        readonly ICookieProvider cookieProvider;

        public UserService(IOogstplannerUnitOfWork unitOfWork, ICookieProvider cookieProvider) 
            : base(unitOfWork)
        {
            if (cookieProvider == null)
            {
                throw new ArgumentNullException("cookieProvider");
            }

            this.cookieProvider = cookieProvider;
        }

        string anonymousUserKey;
        protected string AnonymousUserCookieKey
        {
            get
            {
                if (anonymousUserKey == null)
                {
                    anonymousUserKey = ConfigurationManager.AppSettings["AnonymousUserCookieKey"];
                }
                return anonymousUserKey;
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
                    var existingAnonymousUser = UnitOfWork.Users.GetUserByUserName(clientUserName);
                    existingAnonymousUser.Name = userName;
                    existingAnonymousUser.FullName = fullName;
                    existingAnonymousUser.Email = email;
                    existingAnonymousUser.AuthenticationStatus = AuthenticatedStatus.Authenticated;

                    cookieProvider.RemoveCookie(AnonymousUserCookieKey);

                    UnitOfWork.Users.Update(existingAnonymousUser);

                    UnitOfWork.Commit();
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
                var calendar = new Calendar { Name = "Mijn kalender" };
                newUser.Calendars.Add(calendar);
                UnitOfWork.Users.Add(newUser);
                UnitOfWork.Commit();
            }
        }

        public int GetCurrentUserId()
        {
            // Note: HttpContext.Current.User.Identity.Name returns Username locally, and e-mail address on Debian.
            //       Has to be investigated. For now the quick fix below. :/

            var currentUserEmailOrName = HttpContext.Current.User.Identity.Name;

            int currentUserId = currentUserEmailOrName.Contains("@")
                ? UnitOfWork.Users.GetUserIdByEmail(currentUserEmailOrName)
                : UnitOfWork.Users.GetUserIdByName(currentUserEmailOrName);
          
            return currentUserId;
        }

        public User GetUser(int id)
        {
            return UnitOfWork.Users.GetById(id);
        }    

        public User GetUserByName(string name)
        {
            return UnitOfWork.Users.GetUserByUserName(name);
        }      
    }
}
