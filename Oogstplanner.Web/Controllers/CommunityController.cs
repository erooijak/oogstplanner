using System;
using System.Linq;
using System.Web.Mvc;

using Newtonsoft.Json;
using PagedList;

using Oogstplanner.Models;
using Oogstplanner.Services;
using Oogstplanner.Web.Utilities.Helpers;

namespace Oogstplanner.Web.Controllers
{
    [Authorize]
    public sealed class CommunityController : Controller
    {        
        readonly ICalendarLikingService calendarLikingService;
        readonly ICommunityService communityService;

        public CommunityController(
            ICalendarLikingService calendarLikingService,
            ICommunityService communityService)
        {
            if (calendarLikingService == null)
            {
                throw new ArgumentNullException("calendarLikingService");
            }
            if (communityService == null)
            {
                throw new ArgumentException("communityService");
            }
      
            this.calendarLikingService = calendarLikingService;
            this.communityService = communityService;
        }

        //
        // GET /gemeenschap/
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(int page = 1, int pageSize = 3)
        {
            ViewBag.SearchDescription = "laatst actieve gebruikers";

            // Just to keep performance down if we happen to scale to millions of users.
            const int maxUsers = 1000;

            var users = communityService.GetRecentlyActiveUsers(maxUsers);
            var model = users.ToPagedList(page, pageSize); 

            return View(model);
        }
            
        //
        // POST /zaaikalender/like
        [HttpPost]
        public JsonResult Like(int calendarId)
        {
            try
            {
                bool wasUnlike;
                calendarLikingService.ToggleLike(calendarId, out wasUnlike);
                return Json(new { success = true, wasUnlike });
            }
            catch (Exception ex) 
            { 
                // TODO: Implement logging
                return Json(new { success = false });
            }
        }

        //
        // GET /zaaikalender/{calendarId}/aantal-likes
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetLikesCount(int calendarId)
        {
            int amountOfLikes = calendarLikingService.GetLikes(calendarId).Count();

            return new JsonStringResult(amountOfLikes.ToString());
        }
            
        //
        // GET /zaaikalender/{calendarId}/gebruikers-die-liken
        [HttpGet]
        [AllowAnonymous]
        public ContentResult GetLikesUserNames(int calendarId)
        {
            var users = calendarLikingService.GetLikes(calendarId).Select(l => l.User.Name);
            var usersJson = JsonConvert.SerializeObject(users);

            return new JsonStringResult(usersJson);
        }

        //
        // GET /gemeenschap/zoek/{searchTerm}
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Search(string searchTerm, int page = 1, int pageSize = 3)
        {
            ViewBag.SearchDescription = string.Empty;
            ViewBag.SearchTerm = searchTerm;

            var users = communityService.SearchUsers(searchTerm);
            var model = new PagedList<User>(users, page, pageSize);

            return View(model);
        }
            
    }
}
