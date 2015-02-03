using System;

using Zk.Helpers;
using Zk.ViewModels;
using Zk.Repositories;


namespace Zk.BusinessLogic
{
    public class CalendarManager
    {
        readonly Repository _repository;
        readonly FarmingActionManager _farmingActionManager;
        readonly UserManager _userManager;

        public CalendarManager()
        {
            _repository = new Repository();
            _farmingActionManager = new FarmingActionManager();
            _userManager = new UserManager();
        }

        public CalendarManager(Repository repository)
        {
            _repository = repository;
            _farmingActionManager = new FarmingActionManager(repository);
            _userManager = new UserManager(repository);
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