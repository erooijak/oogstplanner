using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Zk.Repositories;
using Zk.Models;
using Zk.ViewModels;

namespace Zk.Controllers
{
	public class HomeController : Controller
	{

		readonly Repository _repo;

		/// <summary>
		///     Initializes a new instance of the <see cref="Controllers.HomeController"/> class which
		///     makes use of the real Entity Framework context that connects with the database.
		/// </summary>
		public HomeController()
		{
			_repo = new Repository();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Controllers.HomeController"/> class which
		///     can make use of a "Fake" Entity Framework context for unit testing purposes.
		/// </summary>
		/// <param name="db">Database context.</param>
		public HomeController(IZkContext db)
		{
			_repo = new Repository(db);
		}

		public ActionResult Index ()
		{
			var mvcName = typeof(Controller).Assembly.GetName ();
			var isMono = Type.GetType ("Mono.Runtime") != null;

			ViewData ["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
			ViewData ["Runtime"] = isMono ? "Mono" : ".NET";

			return View ();
		}

        /// <summary>
        ///     POST: /Update/{formCollection} 
        ///     Update the relevant farming actions with the new data from the month.
        /// 
        ///     TODO: Better model binding! Remove JavaScript and just bind data to partial view.
        /// </summary>
        /// <returns></returns>
        /// <param name="farmingMonth">FormCollection with data of the month and crop count.</param>
        [HttpPost]
        public JsonResult Update(FarmingMonthViewModel farmingMonth)
        {
            // Update crop counts.

            return Json (new { success = true });
        }
	}
}