using System;
using System.Collections.Generic;
using System.Linq;

using Oogstplanner.Models;

namespace Oogstplanner.Common
{
    public static class MonthHelper
    {
        public static IEnumerable<Months> GetAllMonths()
        {
            return Enum.GetValues(typeof(Months)).Cast<Months>().Skip(1);
        }

        public static IEnumerable<Months> GetMonths(Months input)
        {
            foreach (var value in GetAllMonths()) 
            {
                if (input.HasFlag(value))
                {
                    yield return value;
                }
            }
        }  
    }
}
