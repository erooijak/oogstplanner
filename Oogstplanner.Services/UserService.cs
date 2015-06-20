using System;
using System.Web;

using Oogstplanner.Data;
using Oogstplanner.Common;
using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public class UserService : CommunityService, IUserService, IDeletableUserService
    {
        readonly ICookieProvider cookieProvider;
        readonly ILastActivityUpdator lastActivityUpdator;

        public UserService(
            IOogstplannerUnitOfWork unitOfWork, 
            ICookieProvider cookieProvider,
            ILastActivityUpdator lastActivityUpdator) 
            : base(unitOfWork)
        {
            if (cookieProvider == null)
            {
                throw new ArgumentNullException("cookieProvider");
            }
            if (lastActivityUpdator == null)
            {
                throw new ArgumentNullException("lastActivityUpdator");
            }

            this.cookieProvider = cookieProvider;
            this.lastActivityUpdator = lastActivityUpdator;
        }
            
        public void AddUser(string userName, string fullName, string email)
        {
            // Update if already exists:
            var clientUserName = cookieProvider.GetCookie(Constants.AnonymousUserCookieKey);
            if (!string.IsNullOrEmpty(clientUserName))
            {
                try
                {
                    var existingAnonymousUser = UnitOfWork.Users.GetUserByUserName(clientUserName);
                    existingAnonymousUser.Name = userName;
                    existingAnonymousUser.FullName = fullName;
                    existingAnonymousUser.Email = email;
                    existingAnonymousUser.AuthenticationStatus = AuthenticatedStatus.Authenticated;

                    cookieProvider.RemoveCookie(Constants.AnonymousUserCookieKey);

                    UnitOfWork.Users.Update(existingAnonymousUser);

                    UnitOfWork.Commit();
                }
                catch (UserNotFoundException ex)
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
                        LastActive = DateTime.Now
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

            lastActivityUpdator.UpdateLastActivity(currentUserId);

            return currentUserId;
        }

        public void RemoveUser(int userId)
        {
            UnitOfWork.Users.Delete(userId);
            UnitOfWork.Commit();
        }
    }
}
