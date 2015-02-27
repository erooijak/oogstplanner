using System;
using System.Web.Mvc;
using Newtonsoft.Json;

using Zk.BusinessLogic;
using Zk.Helpers;

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
        // GET: /Index
        [HttpGet]
        public ViewResult Index() 
        {
            // Get all crops and return to the view for display.
            var crops = _manager.GetAllCrops();

            return View(crops);
        }

        //
        // GET: /All
        [HttpGet]
        public ActionResult All() 
        {
            var crops = _manager.GetAllCrops();
            var cropsJson = JsonConvert.SerializeObject(crops, new MonthEnumConverter());

            return new JsonStringResult(cropsJson);
        }
            	
    }
}