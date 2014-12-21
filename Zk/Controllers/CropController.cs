using System.Web.Mvc;

using Zk.BusinessLogic;

namespace Zk.Controllers
{
	public class CropController : Controller
	{
		readonly CropManager _manager;

		/// <summary>
		///     Initializes a new instance of the <see cref="Controllers.CropController"/> class.
		/// </summary>
		public CropController()
		{
            _manager = new CropManager();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Controllers.CropController"/> class which
		///     can make use of a "Fake" Entity Framework context for unit testing purposes.
		/// </summary>
        /// <param name="manager"></param>
        public CropController(CropManager manager)
		{
            _manager = manager;
		}
			
		/// <summary>
		///     GET: /index
		/// </summary>
		[HttpGet]
		public ViewResult Index() 
		{
			// Get all crops and return to the view for display.
			var crops = _manager.GetAllCrops();

			return View(crops);
		}
            	
	}
}