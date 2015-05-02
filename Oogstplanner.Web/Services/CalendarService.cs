using Oogstplanner.Utilities.Helpers;
using Oogstplanner.ViewModels;
using Oogstplanner.Repositories;
using Oogstplanner.Models;

using Autofac.Features.Indexed;

namespace Oogstplanner.Services
{
    public class CalendarService : ICalendarService
    {
        readonly Repository repository;
        readonly FarmingActionService farmingActionService;
        readonly IUserService userService;

        public CalendarService(
            Repository repository,
            FarmingActionService farmingActionService,
            IIndex<AuthenticatedStatus, IUserService> userServices,
            AuthenticationService authService)
        {
            this.repository = repository;
            this.farmingActionService = farmingActionService;
            this.userService = userServices[authService.GetAuthenticationStatus()];
        }

        int? currentUserId;
        public int CurrentUserId 
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
            return repository.GetCalendar(CurrentUserId);
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
                DisplayMonth = month.ToString(),
                HarvestingActions = farmingActionService.GetHarvestingActions(CurrentUserId, month),
                SowingActions = farmingActionService.GetSowingActions(CurrentUserId, month)
            };
        }

        public Month GetMonthsWithActions()
        {
            return repository.GetMonthsWithActions(CurrentUserId);
        }

    }
}