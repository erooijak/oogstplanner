using System;
using System.Collections.Generic;
using System.Web.Mvc;

using Zk.ViewModels;

namespace Zk.Controllers
{
    [Authorize]
	public class HomeController : Controller
	{
		public ActionResult Index ()
		{
			var mvcName = typeof(Controller).Assembly.GetName ();
			var isMono = Type.GetType ("Mono.Runtime") != null;

			ViewData ["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
			ViewData ["Runtime"] = isMono ? "Mono" : ".NET";

            var viewModel = new MainViewModel 
            {
                // Seasons in Dutch (singular: "seizoen"; plural: "seizoenen") used for displayal in top row.
                SeizoenenForDisplay = new[] { "herfst", "winter", "lente", "zomer" },

                // Seasons used for the CSS classes which refer to the different images.
                SeasonsCssClasses = new[] { "autumn", "winter", "spring", "summer" },

                // Months in Dutch (singular: "maand"; plural: "maanden") are used for the CSS classes 
                // to add to the squares and for displayal within the square.
                MonthsOrderedForDisplay = new Queue<string>(new[] 
                    {   "september",  "december",     "maart",    "juni", 
                        "oktober",    "januari",      "april",    "juli", 
                        "november",   "februari",     "mei",      "augustus"
                    })
            };

            return View(viewModel);
		}
           
	}
}