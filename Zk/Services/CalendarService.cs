using System;

using Zk.Helpers;
using Zk.ViewModels;
using Zk.Repositories;
using Zk.Models;

namespace Zk.Services
{
    public class CalendarService
    {
        readonly Repository _repository;
        readonly FarmingActionService _farmingActionService;
        readonly UserService _userService;

        public CalendarService(
            Repository repository,
            FarmingActionService farmingActionService,
            UserService userService)
        {
            _repository = repository;
            _farmingActionService = farmingActionService;
            _userService = userService;
        }

        public Calendar Get(int userId)
        {
            return _repository.GetCalendarByUserId(userId);
        }

        public YearCalendarViewModel GetYearCalendar()
        {
            var yearCalendar = new YearCalendarViewModel();
            var currentUserId = _userService.GetCurrentUserId();

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
                HarvestingActions = _farmingActionService.GetHarvestingActions(userId, month),
                SowingActions = _farmingActionService.GetSowingActions(userId, month)
            };
        }

    }
}