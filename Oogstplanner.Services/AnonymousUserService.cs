using System;

using Oogstplanner.Common;
using Oogstplanner.Data;
using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public class AnonymousUserService : CommunityService, IUserService
    {
        readonly ICookieProvider cookieProvider;
        readonly ILastActivityUpdator lastActivityUpdator;

        public AnonymousUserService(
            IOogstplannerUnitOfWork unitOfWork, 
            ICookieProvider cookieProvider,
            ILastActivityUpdator lastActivityUpdator)
            : base(unitOfWork)
        {
            if (lastActivityUpdator == null)
            {
                throw new ArgumentNullException("lastActivityUpdator");
            }

            this.cookieProvider = cookieProvider;
            this.lastActivityUpdator = lastActivityUpdator;
        }
            
        protected User CurrentAnonymousUser 
        { 
            get 
            { 
                if (string.IsNullOrEmpty(GuidOnClient))
                {
                    var guid = Guid.NewGuid().ToString();
                    AddUser(guid, null, null);
                    GuidOnClient = guid;

                    return UnitOfWork.Users.GetUserByUserName(guid);
                }
                else
                {
                    return UnitOfWork.Users.GetUserByUserName(GuidOnClient);
                }

            }

        }

        string guidOnClient;
        protected string GuidOnClient
        {
            get 
            { 
                if (guidOnClient == null)
                {
                    guidOnClient = cookieProvider.GetCookie(Constants.AnonymousUserCookieKey);
                }
                return guidOnClient;
            }
            set 
            { 
                cookieProvider.SetCookie(
                    Constants.AnonymousUserCookieKey, 
                    value, 
                    Constants.AnonymousUserCookieExpiration); 
            }
        }
            
        public void AddUser(string userName, string fullName, string email)
        {
            var user = new User 
                {
                    Name = userName,
                    AuthenticationStatus = AuthenticatedStatus.Anonymous, // by definition
                    LastActive = DateTime.Now
                };
            user.Calendars.Add(new Calendar { Name = "Mijn kalender" } );
            UnitOfWork.Users.Add(user);
            UnitOfWork.Commit();
        }

        public int GetCurrentUserId()
        {
            int currentUserId = CurrentAnonymousUser.Id;
            UpdateLastActivity(currentUserId);

            return currentUserId;
        }

        /// <summary>
        /// Update last activity and set the cookie to expire later. 
        /// </summary>
        /// <remarks>
        /// If the latter is not done the clean-database.sh script will
        /// remove the user while the cookie still exists, thus causing
        /// an error that the guid from the client cookie cannot be found.
        /// </remarks>
        /// <param name="userId">User identifier.</param>
        void UpdateLastActivity(int userId)
        {
            lastActivityUpdator.UpdateLastActivity(userId);
            cookieProvider.SetCookie(
                Constants.AnonymousUserCookieKey, 
                cookieProvider.GetCookie(Constants.AnonymousUserCookieKey), 
                Constants.AnonymousUserCookieExpiration); 
        }
    }
}
