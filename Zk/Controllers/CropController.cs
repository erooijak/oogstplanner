using System;
using System.Web.Mvc;
using Newtonsoft.Json;

using Zk.BusinessLogic;
using Zk.Helpers;
using Zk.Models;

namespace Zk.Controllers
{
    public class CropController : Controller
    {
        readonly CropManager _manager;

        public CropController(CropManager cropManager)
        {
            _manager = cropManager;
        }
        	
        //
        // GET: /Index
        [HttpGet]
        public ViewResult Index() 
        {
            // Get all crops and return to the view for display.
            var crops = _manager.GetAll();

            return View(crops);
        }

        //
        // GET: /All
        [HttpGet]
        public ActionResult All() 
        {
            var crops = _manager.GetAll();
            var cropsJson = JsonConvert.SerializeObject(crops, new MonthEnumConverter());

            return new JsonStringResult(cropsJson);
        }
            	
    }
}