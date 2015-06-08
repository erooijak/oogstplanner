using System;
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
            UnitOfWork.Calendars.GetById(calendarId).Likes.Add(like);
            UnitOfWork.Commit();
        }

        public void UnLike(int calendarId)
        {
            var likeToDelete = UnitOfWork.Likes.Find(l => l.User.Id == CurrentUser.Id)
                .ToList().FirstOrDefault();

            if (likeToDelete == null)
            {
                return;
            }

            UnitOfWork.Likes.Delete(likeToDelete);
            UnitOfWork.Commit();
        }
    }
}
