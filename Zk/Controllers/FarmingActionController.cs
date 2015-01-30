using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Zk.BusinessLogic;
using Zk.Helpers;
using Zk.ViewModels;

namespace Zk.Controllers
{
    public class FarmingActionController : Controller
    {
        readonly FarmingActionManager _manager;

        public FarmingActionController()
        {
            _manager = new FarmingActionManager();
        }

        public FarmingActionController(FarmingActionManager manager)
        {
            _manager = manager;
        }
            
        // GET: /Edit/{month}
        // Returns the farming actions of the month.
        public ActionResult Edit(Month month)
        {
            var farmingMonthViewModel = GetFarmingMonthViewModel(month);
           
            return PartialView(Url.View("_FarmingMonth", "Home"), farmingMonthViewModel);
        }

        /// <summary>
        ///     POST: /Update/{formCollection} 
        ///     Update the relevant farming actions with the new data from the month.
        /// </summary>
        /// <returns>JSONResult indicating success or failure.</returns>
        /// <param name="fc">FormCollection with id of farming action and crop count.</param>
        [HttpPost]
        public JsonResult Update(FormCollection fc)
        {
            // Convert the crop count and farming action id's string arrays to integer arrays.
            var farmingActionIds = fc["action.Id"].Split(',').Select(int.Parse).ToList();
            var cropCounts = fc["action.CropCount"].Split(',').Select(int.Parse).ToList();

            try 
            {
                // Update crop counts in database
                _manager.UpdateCropCounts(farmingActionIds, cropCounts);
            }
            catch (Exception ex)
            {
                // TODO: Implement logging
                return Json (new { success = false });
            }

            return Json (new { success = true });
        }

        FarmingMonthViewModel GetFarmingMonthViewModel(Month month)
        {
            return new FarmingMonthViewModel 
            {
                DisplayMonth = month.ToString(),
                HarvestingActions = _manager.GetHarvestingActions(month),
                SowingActions = _manager.GetSowingActions(month)
            };
        }

    }
}