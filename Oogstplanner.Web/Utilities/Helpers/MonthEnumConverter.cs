using System.Diagnostics;

using Newtonsoft.Json;

using Oogstplanner.Common;
using Oogstplanner.Models;

namespace Oogstplanner.Web.Utilities.Helpers
{
    public class MonthEnumConverter : Newtonsoft.Json.Converters.StringEnumConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Debug.Assert(value is Months, "Input should be month enumeration.");
            var enumValues = (Months)value;

            writer.WriteStartArray();

            var months = MonthHelper.GetMonths(enumValues);
            foreach (var month in months) 
            {
                writer.WriteValue(month.ToString().ToLower());
            }

            writer.WriteEndArray();
        }
    }
}
