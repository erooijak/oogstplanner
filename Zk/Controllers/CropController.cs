using System.Web.Mvc;

using Zk.BusinessLogic;

namespace Zk.Controllers
{
	public class CropController : Controller
	{
		readonly CropManager _manager;

		public CropController()
		{
            _manager = new CropManager();
		}
            
        public CropController(CropManager manager)
		{
            _manager = manager;
		}
			
		//
        // GET: /index
		[HttpGet]
		public ViewResult Index() 
		{
			// Get all crops and return to the view for display.
			var crops = _manager.GetAllCrops();

			return View(crops);
		}
            	
	}
}