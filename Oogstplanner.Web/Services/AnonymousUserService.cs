using System;

using Oogstplanner.Models;
using Oogstplanner.Repositories;
using System.Configuration;

namespace Oogstplanner.Services
{
    public class AnonymousUserService : IUserService
    {
        readonly IUserRepository userRepository;
        readonly ICalendarRepository calendarRepository;
        readonly ICookieProvider cookieProvider;

        public AnonymousUserService(
            IUserRepository userRepository, 
            ICalendarRepository calendarRepository,
            ICookieProvider cookieProvider)
        {
            this.userRepository = userRepository;
            this.calendarRepository = calendarRepository;
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

        double? anonymousUserCookieExpiration;
        protected double AnonymousUserCookieExpiration
        {
            get
            {
                if (anonymousUserCookieExpiration == null)
                {
                    anonymousUserCookieExpiration = Convert.ToDouble(ConfigurationManager.AppSettings["AnonymousUserCookieExpiration"]);
                }
                return (double)anonymousUserCookieExpiration;
            }
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

                    return userRepository.GetUserByUserName(guid);
                }
                else
                {
                    return userRepository.GetUserByUserName(GuidOnClient);
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
                    guidOnClient = cookieProvider.GetCookie(AnonymousUserCookieKey);
                }
                return guidOnClient;
            }
            set 
            { 
                cookieProvider.SetCookie(AnonymousUserCookieKey, value, AnonymousUserCookieExpiration); 
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
            userRepository.AddUser(user);

            // Get the actual user from the database, so we get the created UserId.
            var newlyCreatedUser = userRepository.GetUserByUserName(userName);

            // Create calendar for the anonymous user
            calendarRepository.CreateCalendar(newlyCreatedUser);
        }

        public int GetCurrentUserId()
        {
            return CurrentAnonymousUser.UserId;
        }

        public User GetUser(int id)
        {
            return userRepository.GetUserById(id);
        }
            
    }
}
    