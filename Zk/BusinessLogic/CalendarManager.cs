using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

using Zk.Helpers;
using Zk.Models;
using Zk.ViewModels;
using Zk.Repositories;


namespace Zk.BusinessLogic
{
    public class CalendarManager
    {
        readonly Repository _repository;

        public CalendarManager()
        {
            _repository = new Repository();
        }

        public CalendarManager(Repository repository)
        {
            _repository = repository;
        }

        public YearCalendarViewModel GetYearCalendar()
        {
            var vm = new YearCalendarViewModel();

            var userName = Membership.GetUser().UserName;
            var currentUserId = _repository.GetUserIdByUserName(userName);

            var farmingActionsOfUser = _repository.GetFarmingActions(
                fa => fa.Calendar.UserId == currentUserId);

            foreach (var month in MonthHelper.GetAllMonths()) 
            {
                vm.Add(month, farmingActionsOfUser.Where(fa => fa.Month == month).ToList());
            }

            return vm;
        }

    }
}