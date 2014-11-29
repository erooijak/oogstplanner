using System.Web.Mvc;
using Zk.Models;
using Zk.Repositories;
using System.Linq;

namespace Zk.Controllers
{
	public class CalendarController : Controller
	{
		readonly Repository _repo;

		/// <summary>
        ///     Initializes a new instance of the <see cref="Controllers.CalendarController"/> class which
		///     makes use of the real Entity Framework context that connects with the database.
		/// </summary>
		public CalendarController()
		{
			_repo = new Repository();
		}

		/// <summary>
        ///     Initializes a new instance of the <see cref="Controllers.CalendarController"/> class which
		///     can make use of a "Fake" Entity Framework context for unit testing purposes.
		/// </summary>
		/// <param name="db">Database context.</param>
		public CalendarController(IZkContext db)
		{
			_repo = new Repository(db);
		}

		/// <summary>
		/// 	POST: /UpdateCalendar/{FarmingAction} 
		/// 	Update the current calendar with the new data from the month.
		/// </summary>
		/// <returns></returns>
		/// <param name="submittedMonth">Submitted month.</param>
		[HttpPost]
		public ViewResult UpdateCalendar(FarmingAction submittedMonth)
		{
		    // Get data from submittedMonth

			// Implement logic to update CURRENT calendar with SUBMITTED month.

			return View();
		}
			
	}
}