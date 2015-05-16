using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

using Oogstplanner.Web.Utitilies.Helpers;

namespace Oogstplanner.Web.Utitilies.Filters
{
    /// <summary>
    ///     Returns a JSON object specifying all of the model errors.
    /// </summary>
    public class ValidateAjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                return;
            }

            var modelState = filterContext.Controller.ViewData.ModelState;
            if (!modelState.IsValid)
            {
                var errorModel = JsonHelper.CreateErrorModel(modelState);
                                
                filterContext.Result = new JsonResult() { Data = errorModel };
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
