using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Zk.Helpers;
using Zk.Models;
using Zk.Repositories;

namespace Zk.Helpers
{
    /// <summary>
    ///     Helper class to return JSON string.
    ///     See http://stackoverflow.com/questions/9777731/mvc-how-to-return-a-string-as-json#9777955
    /// </summary>
    public class JsonStringResult : ContentResult
    {
        public JsonStringResult(string json)
        {
            Content = json;
            ContentType = "application/json";
        }
    }
}