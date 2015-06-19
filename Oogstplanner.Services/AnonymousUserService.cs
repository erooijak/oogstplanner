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
            ILastActivityUpdator anonymousUserLastActivityUpdator)
            : base(unitOfWork)
        {
            if (anonymousUserLastActivityUpdator == null)
            {
                throw new ArgumentNullException("anonymousUserLastActivityUpdator");
            }

            this.cookieProvider = cookieProvider;
            this.lastActivityUpdator = anonymousUserLastActivityUpdator;
        }
            
        protected User CurrentAnonymousUser 
        { 
            get 
            { 
                if (string.IsNullOrEmpty(GuidOnClient))
                {
                    var guid = Guid.NewGuid().ToString();
                    AddUser(guid, null, null); // TODO: this is violating SRP and CQS.
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
            lastActivityUpdator.UpdateLastActivity(currentUserId);

            return currentUserId;
        }
    }
}
