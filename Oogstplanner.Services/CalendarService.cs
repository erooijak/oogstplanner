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
            var yearCalendar = new YearCalendarViewModel();
  
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

        public Month GetMonthsWithAction()
        {
            return UnitOfWork.FarmingActions.GetMonthsWithAction(CurrentUserId);
        }

    }
}
