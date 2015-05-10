using Autofac.Features.Indexed;

using Oogstplanner.Models;
using Oogstplanner.Repositories;
using Oogstplanner.Utilities.ExtensionMethods;
using Oogstplanner.Utilities.Helpers;
using Oogstplanner.ViewModels;

namespace Oogstplanner.Services
{
    public class CalendarService : ICalendarService
    {
        readonly ICalendarRepository repository;
        readonly IFarmingActionService farmingActionService;
        readonly IUserService userService;

        public CalendarService(
            ICalendarRepository repository,
            IFarmingActionService farmingActionService,
            IIndex<AuthenticatedStatus, IUserService> userServices,
            IAuthenticationService authService)
        {
            this.repository = repository;
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
                DisplayMonth = month.GetDescription(),
                HarvestingActions = farmingActionService.GetHarvestingActions(CurrentUserId, month),
                SowingActions = farmingActionService.GetSowingActions(CurrentUserId, month)
            };
        }

        public Month GetMonthsWithAction()
        {
            return repository.GetMonthsWithAction(CurrentUserId);
        }

    }
}
