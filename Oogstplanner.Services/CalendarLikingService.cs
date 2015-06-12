using System;
using System.Collections.Generic;
using System.Linq;

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

        public void Like(int calendarId, out bool wasUnlike)
        {
            var like = new Like { User = CurrentUser };
            var calendars = UnitOfWork.Calendars.GetById(calendarId);

            if (calendars == null)
            {
                wasUnlike = false;
                return;
            }
                
            if (calendars.Likes.Any(l => l.User.Id == CurrentUser.Id))
            {
                UnLike(calendarId);
                wasUnlike = true;
            }
            else
            {
                calendars.Likes.Add(like);
                UnitOfWork.Commit();
                wasUnlike = false;
            }
        }

        void UnLike(int calendarId)
        {
            var likeToDelete = UnitOfWork.Likes.SingleOrDefault(
                l => l.Calendar.Id == calendarId && l.User.Id == CurrentUser.Id);

            if (likeToDelete == null)
            {
                return;
            }

            UnitOfWork.Likes.Delete(likeToDelete);
            UnitOfWork.Commit();
        }

        public IEnumerable<Like> GetLikes(int calendarId)
        {
            return UnitOfWork.Likes.GetByCalendarId(calendarId);
        }
            
    }
}
