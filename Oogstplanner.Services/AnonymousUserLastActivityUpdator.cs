using System;

using Oogstplanner.Data;
using Oogstplanner.Common;

namespace Oogstplanner.Services
{
    public class AnonymousUserLastActivityUpdator : ServiceBase, ILastActivityUpdator
    {
        readonly ICookieProvider cookieProvider;
        readonly ILastActivityUpdator lastActivityUpdator;

        public AnonymousUserLastActivityUpdator(
            IOogstplannerUnitOfWork unitOfWork,
            ILastActivityUpdator lastActivityUpdator,
            ICookieProvider cookieProvider) 
            : base(unitOfWork)
        {
            if (lastActivityUpdator == null)
            {
                throw new ArgumentException("lastActivityUpdator");
            }
            if (cookieProvider == null)
            {
                throw new ArgumentNullException("cookieProvider");
            }

            this.lastActivityUpdator = lastActivityUpdator;
            this.cookieProvider = cookieProvider;
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
        public void UpdateLastActivity(int userId)
        {
            lastActivityUpdator.UpdateLastActivity(userId);

            var guidOnClient = cookieProvider.GetCookie(Constants.AnonymousUserCookieKey);

            if (string.IsNullOrEmpty(guidOnClient))
            {
                return;
            }

            cookieProvider.SetCookie(
                Constants.AnonymousUserCookieKey, 
                guidOnClient, 
                Constants.AnonymousUserCookieExpiration); 
        }
    }
}
