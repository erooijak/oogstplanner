using System;

using Zk.Helpers;
using Zk.ViewModels;
using Zk.Repositories;
using Zk.Models;

namespace Zk.BusinessLogic
{
    public class CalendarManager
    {
        readonly Repository _repository;
        readonly FarmingActionManager _farmingActionManager;
        readonly UserManager _userManager;

        public CalendarManager(
            Repository repository,
            FarmingActionManager farmingActionManager,
            UserManager userManager)
        {
            _repository = repository;
            _farmingActionManager = farmingActionManager;
            _userManager = userManager;
        }

        public Calendar Get(int userId)
        {
            return _repository.GetCalendarByUserId(userId);
        }

        public YearCalendarViewModel GetYearCalendar()
        {
            var yearCalendar = new YearCalendarViewModel();
            var currentUserId = _userManager.GetCurrentUserId();

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
                HarvestingActions = _farmingActionManager.GetHarvestingActions(userId, month),
                SowingActions = _farmingActionManager.GetSowingActions(userId, month)
            };
        }

    }
}