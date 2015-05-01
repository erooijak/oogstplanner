using System.Web.Mvc;

namespace Oogstplanner.Utilities.Helpers
{
    /*
     * Helper class to return JSON string.
     * See http://stackoverflow.com/questions/9777731/mvc-how-to-return-a-string-as-json#9777955
     */
    public class JsonStringResult : ContentResult
    {
        public JsonStringResult(string json)
        {
            Content = json;
            ContentType = "application/json";
        }
    }

}