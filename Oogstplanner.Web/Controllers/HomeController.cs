using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

using Oogstplanner.Common;
using Oogstplanner.Models;
using Oogstplanner.Services;

namespace Oogstplanner.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        readonly ICalendarService calendarService;

        public HomeController(ICalendarService calendarService)
        {
            if (calendarService == null)
            {
                throw new ArgumentNullException("calendarService");
            }

            this.calendarService = calendarService;
        }

        //
        // GET: /welkom
        public ActionResult Index()
        {
            return View();
        }
    }
}
