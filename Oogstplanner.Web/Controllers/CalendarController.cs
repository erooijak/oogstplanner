using System;
using System.Linq;
using System.Web.Mvc;

using Newtonsoft.Json;

using Oogstplanner.Utilities.ExtensionMethods;
using Oogstplanner.Models;
using Oogstplanner.Services;
using Oogstplanner.Utilities.Helpers;

namespace Oogstplanner.Controllers
{
    [AllowAnonymous]
    public class CalendarController : Controller
    {        
        readonly ICalendarService calendarService;
        readonly IFarmingActionService farmingActionService;
        readonly ICropProvider cropProvider;

        public CalendarController(
            ICalendarService calendarService,
            IFarmingActionService farmingActionService,
            ICropProvider cropProvider)
        {
            this.calendarService = calendarService;
            this.farmingActionService = farmingActionService;
            this.cropProvider = cropProvider;
        }

        // 
        // GET: /Month/{month}
        // Returns the farming actions of the month.
        public ActionResult Month(Month month)
        {
            var monthCalendarViewModel = calendarService.GetMonthCalendar(month);

            return PartialView(Url.View("_MonthCalendar", "Home"), monthCalendarViewModel);
        }

        //
        // GET: /Calendar/Year
        public ActionResult Year()
        {
            var calendarViewModel = calendarService.GetYearCalendar();

            return View(calendarViewModel);
        }

        /// <summary>
        ///     POST: /UpdateMonth/{formCollection} 
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
        // POST /Calendar/RemoveFarmingAction
        [HttpPost]
        public JsonResult RemoveFarmingAction(int id)
        {
            try 
            {
                // Remove this and related farming action from database
                farmingActionService.RemoveAction(id);
            } 
            catch (Exception ex) 
            {
                // TODO: Implement logging
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult AddFarmingAction(int cropId, Month month, ActionType actionType, int cropCount)
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
                farmingActionService.AddAction(farmingAction);
                return Json(new { success = true });
            }
            catch (Exception ex) 
            { 
                // TODO: Implement logging
                return Json(new { success = false });
            }

        }

        //
        // GET /Calendar/GetMonthsWithAction
        [HttpGet]
        public ActionResult GetMonthsWithAction()
        {
            var monthsWithActionJson = JsonConvert
                .SerializeObject(calendarService.GetMonthsWithAction(), new MonthEnumConverter());

            return new JsonStringResult(monthsWithActionJson);
        }
            
    }
}