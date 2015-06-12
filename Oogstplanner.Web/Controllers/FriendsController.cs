using System;
using System.Linq;
using System.Web.Mvc;

using Newtonsoft.Json;

using Oogstplanner.Services;
using Oogstplanner.Web.Utilities.Helpers;

namespace Oogstplanner.Web.Controllers
{
    [Authorize]
    public sealed class FriendsController : Controller
    {        
        readonly ICalendarLikingService calendarLikingService;

        public FriendsController(ICalendarLikingService calendarLikingService)
        {
            if (calendarLikingService == null)
            {
                throw new ArgumentNullException("calendarLikingService");
            }
      
            this.calendarLikingService = calendarLikingService;
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
    }
}
