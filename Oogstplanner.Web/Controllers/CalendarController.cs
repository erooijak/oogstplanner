using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;

using Oogstplanner.Common;
using Oogstplanner.Models;
using Oogstplanner.Services;
using Oogstplanner.Web.Utilities.ExtensionMethods;
using Oogstplanner.Web.Utilities.Helpers;

namespace Oogstplanner.Web.Controllers
{
    [AllowAnonymous]
    public sealed class CalendarController : Controller
    {        
        readonly ICalendarService calendarService;
        readonly IFarmingActionService farmingActionService;
        readonly ICropProvider cropProvider;

        public CalendarController(
            ICalendarService calendarService,
            IFarmingActionService farmingActionService,
            ICropProvider cropProvider)
        {
            if (calendarService == null)
            {
                throw new ArgumentNullException("calendarService");
            }
            if (farmingActionService == null)
            {
                throw new ArgumentNullException("farmingActionService");
            }
            if (cropProvider == null)
            {
                throw new ArgumentNullException("cropProvider");
            }

            this.calendarService = calendarService;
            this.farmingActionService = farmingActionService;
            this.cropProvider = cropProvider;
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

        // 
        // GET: /zaaikalender/{month}
        // Returns the farming actions of the month.
        public ActionResult Month(Months month)
        {
            var monthCalendarViewModel = calendarService.GetMonthCalendar(month);

            return PartialView(Url.View("_MonthCalendar", "Calendar"), monthCalendarViewModel);
        }

        //
        // GET: /zaaikalender
        public ActionResult Year()
        {
            var calendarViewModel = calendarService.GetYearCalendar();

            if (calendarViewModel.HasAnyActions())
            {
                return View("Year", calendarViewModel);
            }

            return View("NoCrops");
        }

        //
        // GET: /account/{userName}/zaaikalender
        public ActionResult YearForUser(string userName)
        {
            YearCalendarViewModel calendarViewModel = null;

            try
            {
                calendarViewModel = calendarService.GetYearCalendar(userName);
            }
            catch (UserNotFoundException)
            {
                Response.StatusCode = 404;
                return View("~/Views/Account/UserDoesNotExist.cshtml");
            }
            catch
            {
                // TODO implement logging of inner exception.
                throw new HttpException(500, "Er is iets fout gegaan.");
            }

            if (calendarViewModel.HasAnyActions())
            {
                return View("Year", calendarViewModel);
            }

            return calendarViewModel.IsOwnCalendar 
                ? View("NoCrops") 
                : View("NoCropsOtherUser", calendarViewModel);
        }   

        /// <summary>
        ///     POST: /zaaikalender/update
        ///     Update the relevant farming actions with the new data from the month.
        /// </summary>
        /// <returns>JSONResult indicating success or failure.</returns>
        /// <param name="fc">FormCollection with id of farming action and crop count.</param>
        [HttpPost]
        public JsonResult UpdateMonth(FormCollection fc)
        {
            // Convert the crop count and farming action id's string arrays to integer arrays.
            var farmingActionIds = fc["action.Id"].Split(',').Select(int.Parse).ToList();
            var cropCounts = fc["action.CropCount"].Split(',').Select(int.Parse).ToList();

            try 
            {
                // Update crop counts in database
                farmingActionService.UpdateCropCounts(farmingActionIds, cropCounts);
            } 
            catch (Exception ex) 
            {
                // TODO: Implement logging
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

        //
        // POST /zaaikalender/verwijder
        [HttpPost]
        public JsonResult RemoveFarmingAction(int id)
        {
            try 
            {
                // Remove this and related farming action from database
                farmingActionService.RemoveActionPair(id);
            } 
            catch (Exception ex) 
            {
                // TODO: Implement logging
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

        //
        // POST /zaaikalender/toevoegen
        [HttpPost]
        public JsonResult AddFarmingAction(int cropId, Months month, ActionType actionType, int cropCount)
        {
            var crop = cropProvider.GetCrop(cropId);
            var calendar = calendarService.GetCalendar();

            var farmingAction = new FarmingAction 
            {
                Action = actionType,
                Calendar = calendar,
                Crop = crop,
                CropCount = cropCount,
                Month = month
            };

            try
            {
                farmingActionService.AddActionPair(farmingAction);
                return Json(new { success = true });
            }
            catch (Exception ex) 
            { 
                // TODO: Implement logging
                return Json(new { success = false });
            }

        }

        //
        // GET /zaaikalender/actievemaanden
        [HttpGet]
        public ActionResult GetMonthsWithAction()
        {
            string monthsWithActionJson = JsonConvert
                .SerializeObject(calendarService.GetMonthsWithAction(), new MonthEnumConverter());

            return new JsonStringResult(monthsWithActionJson);
        }
            
    }
}
