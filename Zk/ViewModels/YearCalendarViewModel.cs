using System.Collections.Generic;
using Zk.Models;

namespace Zk.ViewModels
{
    /// <summary>
    ///     View model used for displaying the year calendar
    /// </summary>
    public class YearCalendarViewModel : Dictionary<Month, IEnumerable<FarmingAction>>
    {
    }

}