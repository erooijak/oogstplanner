using System;

using Oogstplanner.Data;
using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public class CalendarLikingService : ServiceBase, ICalendarLikingService
    {
        readonly IUserService userService;

        public CalendarLikingService(
            IOogstplannerUnitOfWork unitOfWork, 
            IUserService userService)
            : base(unitOfWork)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }

            this.userService = userService;
        }

        User currentUser;
        protected User CurrentUser 
        { 
            get 
            {
                int currentUserId = userService.GetCurrentUserId();
                currentUser = userService.GetUser(currentUserId);

                return currentUser;
            }
        }

        public void Like(int calendarId)
        {
            UnitOfWork.Calendars.GetById(calendarId).Likes.Add(CurrentUser);
            UnitOfWork.Commit();
        }

        public void UnLike(int calendarId)
        {
            UnitOfWork.Calendars.GetById(calendarId).Likes.Remove(CurrentUser);
            UnitOfWork.Commit();
        }
    }
}
