using System;
using System.Linq;
using System.Web.Mvc;

using Newtonsoft.Json;

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
      
            this.calendarLikingService = calendarLikingService;
            this.communityService = communityService;
        }

        //
        // GET /gemeenschap/
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Index()
        {
            return View();
        }
            
        //
        // POST /zaaikalender/like
        [HttpPost]
        public JsonResult Like(int calendarId)
        {
            try
            {
                bool wasUnlike;
                calendarLikingService.Like(calendarId, out wasUnlike);
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
        public ContentResult SearchUsers(string searchTerm)
        {
            var users = communityService.SearchUsers(searchTerm);

            throw new NotImplementedException();
        }

        //
        // GET /gemeenschap/actief
        [HttpGet]
        [AllowAnonymous]
        public ContentResult LastActiveUsers(int page = 1, int pageSize = 10)
        {
            const int maxUsers = 3;
            var users = communityService.GetRecentlyActiveUsers(3);

            throw new NotImplementedException();
        }
    }
}
