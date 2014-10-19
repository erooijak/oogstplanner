using System.Web.Mvc;
using SowingCalendar.Models;

namespace SowingCalendar.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

