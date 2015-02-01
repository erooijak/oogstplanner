using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Zk.Helpers;
using Zk.ViewModels;

namespace Zk.Controllers
{
    [Authorize]
	public class HomeController : Controller
	{
		public ActionResult Index ()
		{
			var mvcName = typeof(Controller).Assembly.GetName ();
			var isMono = Type.GetType("Mono.Runtime") != null;

			ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? 
                "<a href=\"http://www.mono-project.com/\">Mono</a>" 
                : "<a href=\"http://www.microsoft.com/net/\">.NET</a>";

            // Months are used for the CSS classes 
            // to add to the squares and for displayal within the square.
            var months = MonthHelper.GetAllMonths()
                .Select(m => m.ToString().ToLower())
                .ToList();
                
            var viewModel = new MainViewModel 
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