using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SowingCalendar.Models;
using SowingCalendar.Repository;

namespace SowingCalendar.Controllers
{
    public class CropController : Controller
    {
        private IRepository repo;
        private ISowingCalendarContext db;

        public ActionResult Index() 
        {
            return View();
        }

    }
}