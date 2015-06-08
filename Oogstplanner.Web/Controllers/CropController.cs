using System;
using System.Web.Mvc;
using Newtonsoft.Json;

using Oogstplanner.Services;
using Oogstplanner.Web.Utilities.Helpers;

namespace Oogstplanner.Web.Controllers
{
    [AllowAnonymous]
    public sealed class CropController : Controller
    {
        readonly ICropProvider cropProvider;

        public CropController(ICropProvider cropProvider)
        {
            if (cropProvider == null)
            {
                throw new ArgumentNullException("cropProvider");
            }

            this.cropProvider = cropProvider;
        }
        	
        //
        // GET: /gewassen
        [HttpGet]
        public ViewResult Index() 
        {
            // Get all crops and return to the view for display.
            var crops = cropProvider.GetAllCrops();

            return View(crops);
        }

        //
        // GET: /gewassen/json
        [HttpGet]
        public ActionResult All() 
        {
            var crops = cropProvider.GetAllCrops();
            var cropsJson = JsonConvert.SerializeObject(crops, new MonthEnumConverter());

            return new JsonStringResult(cropsJson);
        }            	
    }
}