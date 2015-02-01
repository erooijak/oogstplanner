using System;
using System.Collections.Generic;
using System.Linq;

namespace Zk.Helpers
{
    public static class MonthHelper
    {
        public static IEnumerable<Month> GetAllMonths()
        {
            return Enum.GetValues(typeof(Month)).Cast<Month>().Skip(1);
        }
    }
}

