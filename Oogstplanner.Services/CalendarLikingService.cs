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

        public void Like(int calendarId)
        {
            var like = new Like { User = CurrentUser };
            var calendarLikes = UnitOfWork.Calendars.GetById(calendarId).Likes;

            if (calendarLikes.Any(l => l.User.Id == CurrentUser.Id))
            {
                return;
            }
                
            calendarLikes.Add(like);
            UnitOfWork.Commit();
        }

        public void UnLike(int calendarId)
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
