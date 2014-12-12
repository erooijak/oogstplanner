using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Zk.Helpers;
using Zk.Models;
using Zk.Repositories;
using Zk.ViewModels;

namespace Zk.Controllers
{
    public class FarmingActionController : Controller
    {
        readonly Repository _repo;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Controllers.FarmingActionController"/> class which
        ///     makes use of the real Entity Framework context that connects with the database.
        /// </summary>
        public FarmingActionController()
        {
            _repo = new Repository();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Controllers.FarmingActionController"/> class which
        ///     can make use of a "Fake" Entity Framework context for unit testing purposes.
        /// </summary>
        /// <param name="db">Database context.</param>
        public FarmingActionController(IZkContext db)
        {
            _repo = new Repository(db);
        }

        /// <summary>
        ///     GET: /Edit/{month}
        ///     Returns the farming actions of the month.
        /// </summary>
        /// <returns></returns>
        /// <param name="month">Requested month.</param>
        public ActionResult Edit(Month month)
        {
            var farmingMonthViewModel = new FarmingMonthViewModel {
                DisplayMonth = month.ToString(),
                HarvestingActions = _repo.GetHarvestingActions(month),
                SowingActions = _repo.GetSowingActions(month)
            };
           
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
                _repo.UpdateCropCounts(farmingActionIds, cropCounts);
            }
            catch (Exception ex)
            {
                // TODO: Implement logging
                return Json (new { success = false });
            }

            return Json (new { success = true });
        }

    }
}