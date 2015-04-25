using System;

using Zk.Helpers;
using Zk.ViewModels;
using Zk.Repositories;
using Zk.Models;

namespace Zk.Services
{
    public class CalendarService
    {
        readonly Repository repository;
        readonly FarmingActionService farmingActionService;
        readonly IUserService userService;

        public CalendarService(
            Repository repository,
            FarmingActionService farmingActionService,
            IUserService userService)
        {
            this.repository = repository;
            this.farmingActionService = farmingActionService;
            this.userService = userService;
        }

        public Calendar Get(int userId)
        {
            return repository.GetCalendarByUserId(userId);
        }

        public YearCalendarViewModel GetYearCalendar()
        {
            var yearCalendar = new YearCalendarViewModel();
            var currentUserId = userService.GetCurrentUserId();

            foreach (var month in MonthHelper.GetAllMonths())
            {
                var monthCalendar = GetMonthCalendar(currentUserId, month);
                yearCalendar.Add(monthCalendar);
            }

            return yearCalendar;
        }

        public MonthCalendarViewModel GetMonthCalendar(int userId, Month month)
        {
            return new MonthCalendarViewModel 
            {
                DisplayMonth = month.ToString(),
                HarvestingActions = farmingActionService.GetHarvestingActions(userId, month),
                SowingActions = farmingActionService.GetSowingActions(userId, month)
            };
        }

    }
}