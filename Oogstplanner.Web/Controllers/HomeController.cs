using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Oogstplanner.Common;
using Oogstplanner.Models;
using Oogstplanner.Services;

namespace Oogstplanner.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        readonly ICalendarService calendarService;

        public HomeController(ICalendarService calendarService)
        {
            if (calendarService == null)
            {
                throw new ArgumentNullException("calendarService");
            }

            this.calendarService = calendarService;
        }

        //
        // GET: /Index
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Home/SowingAndHarvesting
        public ActionResult SowingAndHarvesting()
        {
            // Months are used for the CSS classes 
            // to add to the squares and for displayal within the square.
            var months = MonthHelper.GetAllMonths().ToList();

            // Months where the current user is harvesting or sowing.
            var actionMonths = calendarService.GetMonthsWithAction();
                
            // Ordering for the squared boxes view (4 columns for the seasons)
            var monthIndexOrdering = new[] { 7, 4, 1, 10, 
                                             6, 3, 0,  9, 
                                             5, 2, 11, 8 };
            var displayMonthsOrdered = new Stack<MonthViewModel>();
            foreach (var monthIndex in monthIndexOrdering)
            {
                var month = months[monthIndex];
                var name = month.ToString();
                var displayName = month.GetDescription();
                var hasAction = actionMonths.HasFlag(month);
                var monthViewModel = new MonthViewModel(name, displayName, hasAction);

                displayMonthsOrdered.Push(monthViewModel);
            }

            var viewModel = new SowingAndHarvestingViewModel 
            {
                // Seasons in Dutch (singular: "seizoen"; plural: "seizoenen") used for displayal in top row.
                SeasonsForDisplay = new[] { "herfst", "winter", "lente", "zomer" },

                // Seasons used for the CSS classes which refer to the different images.
                SeasonsCssClasses = new[] { "autumn", "winter", "spring", "summer" },

                // Months in the squared and information belonging to the month
                OrderedMonthViewModels = displayMonthsOrdered
            };

            return View(viewModel);
        }
           
    }
}
