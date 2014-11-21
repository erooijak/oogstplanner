using System.Web.Mvc;
using Zk.Models;
using Zk.Repositories;
using System.Linq;

namespace Zk.Controllers
{
	public class CropController : Controller
	{
		readonly Repository _repo;

		/// <summary>
		///     Initializes a new instance of the <see cref="Controllers.CropController"/> class which
		///     makes use of the real Entity Framework context that connects with the database.
		/// </summary>
		public CropController()
		{
			_repo = new Repository();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Controllers.CropController"/> class which
		///     can make use of a "Fake" Entity Framework context for unit testing purposes.
		/// </summary>
		/// <param name="db">Database context.</param>
		public CropController(IZkContext db)
		{
			_repo = new Repository(db);
		}
			
		/// <summary>
		///     GET: /index
		/// </summary>
		[HttpGet]
		public ViewResult Index() 
		{
			// Get all crops and return to the view for display.
			var crops = _repo.GetAllCrops ();

			return View(crops);
		}

		/// <summary>
		///     GET: /index/[id]
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <example>
		///     GET: /index/1
		/// </example>
		/// <returns>
		///     A crop with the id parameter as primary key.
		/// </returns>
		[HttpGet]
		public ViewResult Crop(int id)
		{
			var crop = _repo.GetCrop(id);
			return View(crop);
		}

		/// <summary>
		///     GET: /index/[name]
		/// </summary>
		/// <param name="name">Name of the crop.</param>
		/// <example>
		///     GET: /index/Broccoli
		/// </example>
		/// <returns>
		///     A crop with the name parameter as name.
		/// </returns>
		[HttpGet]
		public ViewResult Crop(string name)
		{
			var crop = _repo.GetCrop(name);
			return View(crop);
		}
			
		/// <summary>
		/// 	Submits the calendar that the user has entered on the view and update the current calendar
		/// </summary>
		/// <returns></returns>
		/// <param name="submittedCalendar">Submitted calendar.</param>
		[HttpPost]
		public ViewResult SubmitCalendar(Calendar submittedCalendar)
		{
			//   Get crops from SUBMITTEDcalendar

			//   Get CURRENTcalendar of user
			//   _db.Calendars.Where(c => c.User.Name = "UserName");

			// Implement logic to update CURRENT calendar with SUBMITTED calendar.

			return View();
		}


	}
}