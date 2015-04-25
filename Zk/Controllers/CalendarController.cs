using System;
using System.Linq;
using System.Web.Mvc;

using Zk.BusinessLogic;
using Zk.Helpers;
using Zk.Models;

namespace Zk.Controllers
{
    public class CalendarController : Controller
    {        
        readonly CalendarService _calendarService;
        readonly UserService _userService;
        readonly FarmingActionService _farmingActionService;
        readonly CropProvider _cropProvider;

        public CalendarController(
            CalendarService calendarService,
            UserService userService,
            FarmingActionService farmingActionService,
            CropProvider cropProvider)
        {
            _calendarService = calendarService;
            _userService = userService;
            _farmingActionService = farmingActionService;
            _cropProvider = cropProvider;
        }

        // 
        // GET: /Month/{month}
        // Returns the farming actions of the month.
        public ActionResult Month(Month month)
        {
            var currentUserId = _userService.GetCurrentUserId();
            var monthCalendarViewModel = _calendarService.GetMonthCalendar(currentUserId, month);

            return PartialView(Url.View("_MonthCalendar", "Home"), monthCalendarViewModel);
        }

        //
        // GET: /Calendar/Year
        public ActionResult Year()
        {
            var calendarViewModel = _calendarService.GetYearCalendar();

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
                _farmingActionService.UpdateCropCounts(farmingActionIds, cropCounts);
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
                _farmingActionService.Remove(id);
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

            var crop = _cropProvider.Get(cropId);
            var currentUserId = _userService.GetCurrentUserId();
            var calendar = _calendarService.Get(currentUserId);

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
                _farmingActionService.Add(farmingAction);
                return Json(new { success = true });
            }
            catch (Exception ex) 
            { 
                // TODO: Implement logging
                return Json(new { success = false });
            }

        }
            
    }
}