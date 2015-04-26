using System.Threading;

using Zk.Helpers;
using Zk.ViewModels;
using Zk.Repositories;
using Zk.Models;

using Autofac.Features.Indexed;

namespace Zk.Services
{
    public class CalendarService
    {
        readonly Repository repository;
        readonly FarmingActionService farmingActionService;
        readonly IUserService userService;
        readonly AuthenticationService authService;

        public CalendarService(
            Repository repository,
            FarmingActionService farmingActionService,
            IIndex<AuthenticatedStatusEnum, IUserService> userServices,
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

        public Calendar Get()
        {
            return repository.GetCalendarByUserId(CurrentUserId);
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

    }
}