using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Zk.Helpers;
using Zk.ViewModels;

namespace Zk.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {

        //
        // GET: /Index
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Home/SowingAndHarvesting
        public ActionResult SowingAndHarvesting()
        {

            // Months are used for the CSS classes 
            // to add to the squares and for displayal within the square.
            var months = MonthHelper.GetAllMonths()
                .Select(m => m.ToString().ToLower())
                .ToList();
                
            var viewModel = new SowingAndHarvestingViewModel 
            {
                // Seasons in Dutch (singular: "seizoen"; plural: "seizoenen") used for displayal in top row.
                SeizoenenForDisplay = new[] { "herfst", "winter", "lente", "zomer" },

                // Seasons used for the CSS classes which refer to the different images.
                SeasonsCssClasses = new[] { "autumn", "winter", "spring", "summer" },

                MonthsOrdered = new Stack<string>(new[] 
                    {   
                        months[7], months[4], months[1],  months[10], 
                        months[6], months[3], months[0],  months[9], 
                        months[5], months[2], months[11], months[8] 
                    }) 
            };

            return View(viewModel);
        }
           
    }
}