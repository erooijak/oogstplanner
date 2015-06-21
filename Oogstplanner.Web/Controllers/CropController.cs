using System;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;

using Oogstplanner.Common;
using Oogstplanner.Models;
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
            var vm = crops.Select(c => new CropViewModel
                {
                    Name = c.Name,
                    Race = c.Race,
                    Category = c.Category,

                    /* Round the values and if it is zero or null call it unknown */
                    AreaPerCrop = c.AreaPerCrop.HasValue 
                        ? Math.Round(c.AreaPerCrop.Value, 2, MidpointRounding.AwayFromZero) == 0 
                            ? "Onbekend" 
                            : Math.Round(c.AreaPerCrop.Value, 2, MidpointRounding.AwayFromZero).ToString()
                        : "Onbekend",
                    AreaPerBag = c.AreaPerBag.HasValue 
                        ? Math.Round(c.AreaPerBag.Value, 2, MidpointRounding.AwayFromZero) == 0 
                            ? "Onbekend" 
                            : Math.Round(c.AreaPerBag.Value, 2, MidpointRounding.AwayFromZero).ToString()
                        : "Onbekend",
                    PricePerBag = c.PricePerBag.HasValue
                        ? c.PricePerBag.ToString()
                        : "Onbekend",
      
                    /* Get the descriptions of the months except the first one (assuming enum value is NotSet */
                    SowingMonths = string.Join(", ", c.SowingMonths.GetDescriptions().Skip(1)),
                    HarvestingMonths = string.Join(", ", c.HarvestingMonths.GetDescriptions().Skip(1))
                });

            return View(vm);
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