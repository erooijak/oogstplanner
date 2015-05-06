using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Oogstplanner.Utitilies.Helpers
{
    public static class JsonHelper
    {
        public static IEnumerable<object> CreateErrorModel(ModelStateDictionary modelState)
        {
            return modelState.Keys.Where(key => modelState[key].Errors.Count > 0)
                .Select(key => new
                    {
                        key = key,
                        errors = modelState[key].Errors.Select(m => m.ErrorMessage).ToArray()
                    });
        }
    }
}
