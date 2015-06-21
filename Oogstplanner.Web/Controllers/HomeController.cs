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

        public HomeController()
        { }

        //
        // GET: /welkom
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /veelgesteldevragen
        public ActionResult Faq()
        {
            return View();
        }
    }
}
