using System;
using System.Collections.Generic;
using System.Linq;

namespace Oogstplanner.Utilities.Helpers
{
    public static class MonthHelper
    {
        public static IEnumerable<Month> GetAllMonths()
        {
            return Enum.GetValues(typeof(Month)).Cast<Month>().Skip(1);
        }

        public static IEnumerable<Month> GetMonths(Month input)
        {
            foreach (var value in GetAllMonths()) 
            {
                if (input.HasFlag(value))
                    yield return value;
            }
        }
            
    }
}

