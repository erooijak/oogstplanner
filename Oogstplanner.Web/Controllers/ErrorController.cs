using System.Web.Mvc;

namespace Oogstplanner.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        //
        // GET: /404
        [HttpGet]
        public ActionResult NotFound()
        {
            ActionResult result;

            object model = Request.Url.PathAndQuery;

            if (Request.IsAjaxRequest())
            {
                result = PartialView("NotFound", model);
            }
            else
            {
                result = View(model);
            }

            Response.StatusCode = 404;

            return result;
        }
    }
}
