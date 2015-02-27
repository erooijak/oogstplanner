using System;
using System.Linq;
using System.Web.Mvc;

using Zk.BusinessLogic;
using Zk.Helpers;

namespace Zk.Controllers
{
	public class CalendarController : Controller
	{        
        readonly CalendarManager _calendarManager;
        readonly UserManager _userManager;
        readonly FarmingActionManager _farmingActionManager;
        readonly CropManager _cropManager;

        public CalendarController()
        {
            _calendarManager = new CalendarManager();
            _userManager = new UserManager();
            _farmingActionManager = new FarmingActionManager();
            _cropManager = new CropManager();
        }

        public CalendarController(CalendarManager calendarManager, 
            UserManager userManager, 
            FarmingActionManager farmingActionManager,
            CropManager cropManager)
        {
            _calendarManager = calendarManager;
            _userManager = userManager;
            _farmingActionManager = farmingActionManager;
            _cropManager = cropManager;
        }

        // 
        // GET: /Month/{month}
        // Returns the farming actions of the month.
        public ActionResult Month(Month month)
        {
            var currentUserId = _userManager.GetCurrentUserId();
            var monthCalendarViewModel = _calendarManager.GetMonthCalendar(currentUserId, month);

            return PartialView(Url.View("_MonthCalendar", "Home"), monthCalendarViewModel);
        }

        //
        // GET: /Calendar/Year
        public ActionResult Year()
        {
            var calendarViewModel = _calendarManager.GetYearCalendar();

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
                _farmingActionManager.UpdateCropCounts(farmingActionIds, cropCounts);
            } 
            catch (Exception ex) 
            {
                // TODO: Implement logging
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult AddFarmingAction(int cropId, Month month, ActionType actionType)
        {

            var crop = _cropManager.GetCrop(cropId);
            var currentUserId = _userManager.GetCurrentUserId();

            // TODO: create farming action and update related.

            return Json(new { success = true });
        }
            
	}
}