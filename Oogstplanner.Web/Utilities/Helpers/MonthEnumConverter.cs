using System;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Oogstplanner.Utilities.Helpers
{
    public class MonthEnumConverter : Newtonsoft.Json.Converters.StringEnumConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Debug.Assert(value is Month, "Input should be month enumeration.");
            var enumValues = (Month)value;

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