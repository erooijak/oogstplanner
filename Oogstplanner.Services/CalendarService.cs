using System;

using Autofac.Features.Indexed;

using Oogstplanner.Common;
using Oogstplanner.Data;
using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public class CalendarService : ServiceBase, ICalendarService
    {
        readonly IFarmingActionService farmingActionService;
        readonly IUserService userService;
        readonly IAuthenticationService authService;

        public CalendarService(
            IOogstplannerUnitOfWork unitOfWork, 
            IFarmingActionService farmingActionService,
            IIndex<AuthenticatedStatus, IUserService> userServices,
            IAuthenticationService authService)
            : base(unitOfWork)
        {
            if (farmingActionService == null)
            {
                throw new ArgumentNullException("farmingActionService");
            }
            if (userServices == null)
            {
                throw new ArgumentNullException("userServices");
            }
            if (authService == null)
            {
                throw new ArgumentNullException("authService");
            }

            this.farmingActionService = farmingActionService;
            this.userService = userServices[authService.GetAuthenticationStatus()];
            this.authService = authService;
        }

        int? currentUserId;
        protected int CurrentUserId 
        { 
            get 
            {
                if (currentUserId == null) 
                {
                    currentUserId = userService.GetCurrentUserId();
                }
                return (int)currentUserId;
            }
        }

        public Calendar GetCalendar()
        {
            return UnitOfWork.Calendars.GetByUserId(CurrentUserId);
        }

        public YearCalendarViewModel GetYearCalendar()
        {
            var calendar = GetCalendar();

            var yearCalendar = new YearCalendarViewModel
                {
                    CalendarId = calendar.Id,
                    LikesCount = calendar.Likes.Count,
                    IsOwnCalendar = true
                };

            foreach (var month in MonthHelper.GetAllMonths())
            {
                var monthCalendar = GetMonthCalendar(month);
                yearCalendar.Add(monthCalendar);
            }

            return yearCalendar;
        }

        public MonthCalendarViewModel GetMonthCalendar(Month month)
        {
            return new MonthCalendarViewModel 
            {
                DisplayMonth = month.GetDescription(),
                HarvestingActions = farmingActionService.GetHarvestingActions(CurrentUserId, month),
                SowingActions = farmingActionService.GetSowingActions(CurrentUserId, month)
            };
        }

        public YearCalendarViewModel GetYearCalendar(string userName)
        {
            int userId = userService.GetUserByName(userName).Id;
            var userCalendar = UnitOfWork.Calendars.GetByUserId(userId);

            var yearCalendar = new YearCalendarViewModel 
                { 
                    UserName = userName,
                    CalendarId = userCalendar.Id,
                    LikesCount = userCalendar.Likes.Count,
                    IsOwnCalendar = IsCalendarOfCurrentUser(userCalendar)
                };

            foreach (var month in MonthHelper.GetAllMonths())
            {
                var monthCalendar = GetMonthCalendar(userId, month);
                yearCalendar.Add(monthCalendar);
            }

            return yearCalendar;
        }

        MonthCalendarViewModel GetMonthCalendar(int userId, Month month)
        {
            return new MonthCalendarViewModel 
            {
                DisplayMonth = month.GetDescription(),
                HarvestingActions = farmingActionService.GetHarvestingActions(userId, month),
                SowingActions = farmingActionService.GetSowingActions(userId, month)
            };
        }

        public Month GetMonthsWithAction()
        {
            return UnitOfWork.FarmingActions.GetMonthsWithAction(CurrentUserId);
        }

        bool IsCalendarOfCurrentUser(Calendar calendar)
        {
            return calendar.User.Id == CurrentUserId;
        }
    }
}
