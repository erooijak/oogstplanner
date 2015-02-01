using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Zk.BusinessLogic;
using Zk.Models;
using Zk.Repositories;
using System.Web.Security;

namespace Zk.Controllers
{
	public class CalendarController : Controller
	{        
        readonly CalendarManager _manager;

        public CalendarController()
        {
            _manager = new CalendarManager();
        }

        public CalendarController(CalendarManager manager)
        {
            _manager = manager;
        }

        //
        // GET: /Calendar/Year
        public ActionResult Year()
        {
            var calendarViewModel = _manager.GetYearCalendar();

            return View(calendarViewModel);
        }
	}
}