using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

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
        // GET: /welkom
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /zaaienenoogsten
        public ActionResult SowingAndHarvesting()
        {
            // Months are used for the CSS classes,
            // to add to the squares, for displayal within the square,
            // and for displayment in notifications.
            var months = MonthHelper.GetAllMonths().ToList();

            // Months where the current user is harvesting or sowing.
            var actionMonths = calendarService.GetMonthsWithAction();
                
            var monthNames = months.Select(m => m.ToString().ToLower()).ToList();
            var displayMonthNames = months.Select(m => m.GetDescription()).ToList();

            // Ordering for the squared boxes view (4 columns for the seasons)
            var monthIndexOrdering = new[] { 7, 4, 1, 10, 
                                             6, 3, 0,  9, 
                                             5, 2, 11, 8 };
            var displayMonthsOrdered = new Stack<MonthViewModel>();

            foreach (var monthIndex in monthIndexOrdering)
            {
                var name = monthNames[monthIndex];
                var displayName = displayMonthNames[monthIndex];
                var month = months[monthIndex];
                var hasAction = actionMonths.HasFlag(month);
                var monthViewModel = new MonthViewModel(name, displayName.ToUpper(), hasAction);

                displayMonthsOrdered.Push(monthViewModel);
            }

            var viewModel = new SowingAndHarvestingViewModel 
            {
                // Seasons in Dutch (singular: "seizoen"; plural: "seizoenen") used for displayal in top row.
                SeasonsForDisplay = new[] { "herfst", "winter", "lente", "zomer" },

                // Seasons used for the CSS classes which refer to the different images.
                SeasonsCssClasses = new[] { "autumn", "winter", "spring", "summer" },

                // Below two properties are for a hidden field so they can be picked up by
                // JavaScript notifications.
                // This is done this way so that if the Month model changes (for example
                // for supporting more languages) there is a single point to change implementation.
                MonthNames = monthNames,

                DisplayMonthNames = displayMonthNames,

                // Months in the squared and information belonging to the month
                OrderedMonthViewModels = displayMonthsOrdered
            };

            return View(viewModel);
        }           
    }
}
